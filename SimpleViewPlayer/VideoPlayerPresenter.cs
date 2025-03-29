using Timer = System.Windows.Forms.Timer; // For Timer

namespace SimpleViewMultiViewPlayer;

public class VideoPlayerPresenter
{
    private readonly IVideoPlayerView _view;
    private readonly IVideoModel _model;
    private readonly Timer _playbackTimer; // Use the timer from the View
    private bool _isPlaying = false;
    private Bitmap _currentDisplayFrame = null; // Keep ref to dispose later

    public VideoPlayerPresenter(IVideoPlayerView view, IVideoModel model, Timer timer)
    {
        _view = view;
        _model = model;
        _playbackTimer = timer;

        // Subscribe to View events
        _view.OpenFileClicked += OnOpenFileClicked;
        _view.PlayClicked += OnPlayClicked;
        _view.PauseClicked += OnPauseClicked;
        _view.StopClicked += OnStopClicked;
        _view.Seek += OnSeek;

        // Subscribe to Timer tick event
        _playbackTimer.Tick += OnTimerTick;

        // Initial state update
        UpdateUIState();
    }

    private void OnOpenFileClicked(object sender, EventArgs e)
    {
        string filePath = _view.AskUserForFilePath();
        if (!string.IsNullOrEmpty(filePath))
        {
            // Stop current playback if any before loading new
            StopPlayback();

            if (_model.LoadVideo(filePath))
            {
                _view.SetStatus($"Loaded: {System.IO.Path.GetFileName(filePath)}");
                _view.SetTrackBarRange(_model.TotalFrames);

                // Display the first frame immediately after loading
                ShowFrame(_model.CurrentFrameNumber); // Should be 0
            }
            else
            {
                _view.ShowErrorMessage("Error", "Could not load video file.");
                _view.SetStatus("Error loading file");
                _view.SetTrackBarRange(0); // Reset trackbar
                _view.ClearDisplay();
            }
            UpdateUIState(); // Update button enables etc.
        }
    }

    private void OnPlayClicked(object sender, EventArgs e)
    {
        if (!_model.IsVideoLoaded || _isPlaying) return;

        if (_model.FrameRate > 0)
        {
            _playbackTimer.Interval = (int)(1000 / _model.FrameRate);
            _playbackTimer.Start();
            _isPlaying = true;
            _view.SetStatus("Playing");
        }
        else
        {
            // Handle case where FrameRate is invalid/zero
            _view.SetStatus("Cannot play (invalid frame rate)");
        }
        UpdateUIState();
    }

    private void OnPauseClicked(object sender, EventArgs e)
    {
        if (!_isPlaying) return;

        _playbackTimer.Stop();
        _isPlaying = false;
        _view.SetStatus("Paused");
        UpdateUIState();
    }

    private void OnStopClicked(object sender, EventArgs e)
    {
        StopPlayback();
        if (_model.IsVideoLoaded)
        {
            _model.Seek(0); // Reset to beginning
            ShowFrame(0); // Display the first frame again
            _view.SetStatus("Stopped");
            _view.UpdateTrackBarPosition(0);
        }
        else
        {
            _view.ClearDisplay(); // Clear picture if no video loaded
            _view.SetStatus("Idle");
            _view.UpdateTrackBarPosition(0); // Ensure trackbar is at 0
        }
        UpdateUIState();
    }

    // Handles seeking requested by the TrackBar
    private void OnSeek(object sender, int frameNumber)
    {
        if (!_model.IsVideoLoaded) return;

        // Only seek and update display if not currently playing,
        // or if you want seeking to interrupt playback (pausing is safer)
        if (!_isPlaying)
        {
            if (_model.Seek(frameNumber))
            {
                ShowFrame(frameNumber); // Show the frame at the new position
                _view.SetStatus($"Seeked to frame {frameNumber}");
            }
        }
        else
        {
            // Optional: If playing, pause, seek, show frame, maybe resume?
            // Simpler: Just update the internal model position. Timer will catch up.
            _model.Seek(frameNumber);
            // The timer tick will fetch the correct next frame based on the new position
        }
        // Update trackbar immediately regardless of playing state if needed,
        // though the view's Scroll event handler might already do this.
        // Redundant call might be okay or handled by view logic.
        // _view.UpdateTrackBarPosition(frameNumber);
    }


    private void OnTimerTick(object sender, EventArgs e)
    {
        if (!_isPlaying || !_model.IsVideoLoaded)
        {
            StopPlayback(); // Should not tick if not playing
            return;
        }

        int frameBeforeRead = _model.CurrentFrameNumber;
        Bitmap nextFrame = _model.GetNextFrame(); // Reads frame and advances position

        if (nextFrame != null)
        {
            DisplayAndManageFrame(nextFrame);
            _view.UpdateTrackBarPosition(frameBeforeRead); // Update UI for the frame just displayed
        }
        else
        {
            // Reached end of video or error
            StopPlayback();
            _model.Seek(0); // Optional: Reset to start after finishing
            ShowFrame(0); // Show first frame again after finishing
            _view.SetStatus("Finished");
            _view.UpdateTrackBarPosition(_model.TotalFrames > 0 ? _model.TotalFrames -1 : 0); // Go to end
            UpdateUIState();
        }
    }

    // Helper to display frame and manage disposal of the previous one
    private void DisplayAndManageFrame(Bitmap newFrame)
    {
        _currentDisplayFrame?.Dispose(); // Dispose the *previous* frame Bitmap
        _currentDisplayFrame = newFrame; // Keep reference to the *new* frame
        _view.DisplayFrame(_currentDisplayFrame); // View displays the new frame
    }

    // Helper to show a specific frame (used for Load, Stop, Seek)
    private void ShowFrame(int frameNumber)
    {
        if (!_model.IsVideoLoaded) return;

        // Temporarily pause if playing to avoid race conditions (optional but safer)
        bool wasPlaying = _isPlaying;
        if (wasPlaying) _playbackTimer.Stop();

        Bitmap frame = _model.GetFrame(frameNumber); // Use GetFrame which doesn't advance position
        if (frame != null)
        {
            DisplayAndManageFrame(frame);
            _view.UpdateTrackBarPosition(frameNumber);
        }
        else
        {
            // Handle error getting frame? Maybe clear display.
            _view.ClearDisplay();
        }

        // Resume if it was playing before
        if (wasPlaying) _playbackTimer.Start();
    }

    // Common method to stop playback process
    private void StopPlayback()
    {
        _playbackTimer.Stop();
        _isPlaying = false;
        // Don't reset model position here, Stop/Load handlers do that
    }

    // Updates enabled/disabled state of UI controls via the View
    private void UpdateUIState()
    {
        _view.SetPlaybackControlsEnabled(_model.IsVideoLoaded, _isPlaying);
    }

    // Called by the View when closing to release resources
    public void Cleanup()
    {
        StopPlayback();
        _model?.Dispose(); // Dispose the model (which disposes VideoCapture)
        _currentDisplayFrame?.Dispose(); // Dispose the last displayed frame
    }
}