using LibVLCSharp.Shared;
using System;
using System.IO;

namespace LibVlcMultiViewPlayer;

public class VideoPlayerPresenter : IDisposable
{
    private readonly VideoPlayerControl _view;
    private readonly LibVLC _libVlc;
    private readonly Action<VideoPlayerPresenter> _requestUnmuteCallback;
    private readonly Action<VideoPlayerPresenter> _requestCloseCallback;

    public MediaPlayer MediaPlayer { get; private set; }
    private bool _isMutedInternally = true; // Track our desired state

    public VideoPlayerPresenter(VideoPlayerControl view, LibVLC libVlc, Action<VideoPlayerPresenter> requestUnmuteCallback, Action<VideoPlayerPresenter> requestCloseCallback)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _libVlc = libVlc ?? throw new ArgumentNullException(nameof(libVlc));
        _requestUnmuteCallback = requestUnmuteCallback ?? throw new ArgumentNullException(nameof(requestUnmuteCallback));
        _requestCloseCallback = requestCloseCallback ?? throw new ArgumentNullException(nameof(requestCloseCallback));

        // Use command line args for potential HW acceleration (optional, default often works)
        // See VLC command line help for options like: "--avcodec-hw=dxva2", "--avcodec-hw=d3d11va", "--avcodec-hw=any"
        // string[] vlcOptions = new string[] { "--avcodec-hw=any" }; // Example
        // MediaPlayer = new MediaPlayer(_libVLC, vlcOptions);

        MediaPlayer = new MediaPlayer(_libVlc);

        // Link MediaPlayer to the View
        _view.VlcVideoView.MediaPlayer = MediaPlayer;

        // Subscribe to View events
        _view.PlayPauseClicked += View_PlayPauseClicked;
        _view.MuteToggleClicked += View_MuteToggleClicked;
        _view.SeekRequested += View_SeekRequested;
        _view.CloseRequested += View_CloseRequested;

        // Subscribe to MediaPlayer events
        MediaPlayer.Playing += MediaPlayer_StateChanged;
        MediaPlayer.Paused += MediaPlayer_StateChanged;
        MediaPlayer.Muted += MediaPlayer_Muted;
        MediaPlayer.Unmuted += MediaPlayer_Unmuted;
        MediaPlayer.TimeChanged += MediaPlayer_TimeChanged;
        MediaPlayer.LengthChanged += MediaPlayer_LengthChanged;
        MediaPlayer.EncounteredError += MediaPlayer_EncounteredError;
        MediaPlayer.EndReached += MediaPlayer_EndReached;

        // Set initial UI state
        _view.UpdatePlayPauseButton(false);
        _view.UpdateMuteButton(_isMutedInternally); // Start muted
        _view.UpdateSeekBar(0, 0);
    }

    public void LoadMedia(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Console.WriteLine($"Error: File not found - {filePath}");
            // Optionally show error on the control itself
            return;
        }

        var media = new Media(_libVlc, new Uri(filePath));
        MediaPlayer.Media = media;
        media.Dispose(); // Media object can be disposed after assigning to MediaPlayer

        MediaPlayer.Mute = _isMutedInternally; // Ensure it loads muted
        MediaPlayer.Play(); // Start playing immediately (but muted)
    }

    // --- Event Handlers from View ---

    private void View_PlayPauseClicked(object sender, EventArgs e)
    {
        if (MediaPlayer.IsPlaying)
            MediaPlayer.Pause();
        else
            MediaPlayer.Play(); // Resumes if paused, starts if stopped/ended
    }

    private void View_MuteToggleClicked(object sender, EventArgs e)
    {
        if (_isMutedInternally)
        {
            // Request to be unmuted (Coordinator will mute others and call ForceMute(false) on us)
            _requestUnmuteCallback(this);
        }
        else
        {
            // Requesting to mute self
            ForceMute(true);
        }
    }

    private void View_SeekRequested(object sender, float position)
    {
        if (MediaPlayer.IsSeekable)
        {
            MediaPlayer.Position = position;
        }
    }

    private void View_CloseRequested(object sender, EventArgs e)
    {
        _requestCloseCallback(this); // Ask coordinator to close and dispose us
    }


    // --- Event Handlers from MediaPlayer ---

    private void MediaPlayer_StateChanged(object sender, EventArgs e)
    {
        _view.UpdatePlayPauseButton(MediaPlayer.IsPlaying);
    }

    private void MediaPlayer_Muted(object sender, EventArgs e)
    {
        // This event fires when MediaPlayer.Mute is set to true
        // We manage our state via _isMutedInternally and ForceMute
    }

    private void MediaPlayer_Unmuted(object sender, EventArgs e)
    {
        // This event fires when MediaPlayer.Mute is set to false
    }

    private void MediaPlayer_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
    {
        // Update seek bar based on Time (current) and Length (total)
        _view.UpdateSeekBar(e.Time, MediaPlayer.Length);
    }

    private void MediaPlayer_LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
    {
        // Update seek bar when total length is known
        _view.UpdateSeekBar(MediaPlayer.Time, e.Length);
    }

    private void MediaPlayer_EncounteredError(object sender, EventArgs e)
    {
        Console.WriteLine("Error playing media.");
        // Update UI to show error state
        _view.UpdatePlayPauseButton(false);
    }

    private void MediaPlayer_EndReached(object sender, EventArgs e)
    {
        // Optionally loop, stop, close, etc.
        _view.UpdatePlayPauseButton(false);
        _view.UpdateSeekBar(0, MediaPlayer.Length); // Reset time display
        MediaPlayer.Stop(); // Reset position to start for next play
    }


    // --- Public Methods (Called by Coordinator) ---

    public void ForceMute(bool mute)
    {
        _isMutedInternally = mute;
        MediaPlayer.Mute = mute; // Apply to player
        _view.UpdateMuteButton(_isMutedInternally); // Update UI
    }

    // --- IDisposable ---

    private bool _disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            // Unsubscribe from View events
            _view.PlayPauseClicked -= View_PlayPauseClicked;
            _view.MuteToggleClicked -= View_MuteToggleClicked;
            _view.SeekRequested -= View_SeekRequested;
            _view.CloseRequested -= View_CloseRequested;

            // Unsubscribe from MediaPlayer events (important!)
            MediaPlayer.Playing -= MediaPlayer_StateChanged;
            MediaPlayer.Paused -= MediaPlayer_StateChanged;
            MediaPlayer.Muted -= MediaPlayer_Muted;
            MediaPlayer.Unmuted -= MediaPlayer_Unmuted;
            MediaPlayer.TimeChanged -= MediaPlayer_TimeChanged;
            MediaPlayer.LengthChanged -= MediaPlayer_LengthChanged;
            MediaPlayer.EncounteredError -= MediaPlayer_EncounteredError;
            MediaPlayer.EndReached -= MediaPlayer_EndReached;

            // Dispose LibVLCSharp objects
            // Crucially, detach the player from the view BEFORE disposing the player
            if (_view.VlcVideoView != null)
                _view.VlcVideoView.MediaPlayer = null;

            MediaPlayer?.Stop(); // Ensure media is stopped
            MediaPlayer?.Dispose();
            MediaPlayer = null;

            Console.WriteLine("Presenter Disposed");
        }
        _disposed = true;
    }
}