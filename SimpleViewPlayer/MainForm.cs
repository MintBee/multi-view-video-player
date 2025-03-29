namespace SimpleViewMultiViewPlayer;

public partial class MainForm : Form, IVideoPlayerView
{
    private VideoPlayerPresenter _presenter;

    // --- Events required by IVideoPlayerView ---
    public event EventHandler OpenFileClicked;
    public event EventHandler PlayClicked;
    public event EventHandler PauseClicked;
    public event EventHandler StopClicked;
    public event EventHandler<int> Seek;

    public MainForm()
    {
        InitializeComponent();

        // Instantiate Model and Presenter
        IVideoModel model = new VideoModel();
        _presenter = new VideoPlayerPresenter(this, model, playbackTimer); // Pass timer

        // Wire up UI control events to raise View events
        btnOpen.Click += (s, e) => OpenFileClicked?.Invoke(this, EventArgs.Empty);
        btnPlay.Click += (s, e) => PlayClicked?.Invoke(this, EventArgs.Empty);
        btnPause.Click += (s, e) => PauseClicked?.Invoke(this, EventArgs.Empty);
        btnStop.Click += (s, e) => StopClicked?.Invoke(this, EventArgs.Empty);

        // Handle TrackBar scroll/value changed for seeking
        // Use Scroll event for real-time feedback while dragging
        trackBarSeek.Scroll += (s, e) => Seek?.Invoke(this, trackBarSeek.Value);
        // Optional: Use MouseUp if you only want to seek when the user releases the thumb
        // trackBarSeek.MouseUp += (s, e) => Seek?.Invoke(this, trackBarSeek.Value);

        // Initial UI State
        SetPlaybackControlsEnabled(false, false);
    }

    // --- Implement IVideoPlayerView Methods ---

    public void DisplayFrame(Bitmap frame)
    {
        // Ensure UI updates happen on the UI thread
        if (pictureBoxDisplay.InvokeRequired)
        {
            pictureBoxDisplay.Invoke(new Action(() => DisplayFrameInternal(frame)));
        }
        else
        {
            DisplayFrameInternal(frame);
        }
    }

    private void DisplayFrameInternal(Bitmap frame)
    {
        // Dispose previous image to prevent memory leaks
        pictureBoxDisplay.Image?.Dispose();
        pictureBoxDisplay.Image = frame; // Frame is owned/disposed by Model/Presenter
    }

    public void SetStatus(string status)
    {
        if (lblStatus.InvokeRequired)
        {
            lblStatus.Invoke(new Action(() => lblStatus.Text = $"Status: {status}"));
        }
        else
        {
            lblStatus.Text = $"Status: {status}";
        }
    }

    public void SetPlaybackControlsEnabled(bool isVideoLoaded, bool isPlaying)
    {
        Action updateAction = () =>
        {
            btnPlay.Enabled = isVideoLoaded && !isPlaying;
            btnPause.Enabled = isVideoLoaded && isPlaying;
            btnStop.Enabled = isVideoLoaded;
            trackBarSeek.Enabled = isVideoLoaded;
        };

        if (this.InvokeRequired) // Check if invoke needed for the form itself
        {
            this.Invoke(updateAction);
        }
        else
        {
            updateAction();
        }
    }


    public void SetTrackBarRange(int maxFrames)
    {
        Action updateAction = () =>
        {
            // MaxValue is inclusive, so set to count - 1 if count > 0
            trackBarSeek.Maximum = (maxFrames > 0) ? maxFrames - 1 : 0;
            trackBarSeek.Value = 0; // Reset position
        };
        if (trackBarSeek.InvokeRequired)
        {
            trackBarSeek.Invoke(updateAction);
        }
        else
        {
            updateAction();
        }
    }

    public void UpdateTrackBarPosition(int frameNumber)
    {
        Action updateAction = () =>
        {
            if (frameNumber >= trackBarSeek.Minimum && frameNumber <= trackBarSeek.Maximum)
            {
                // Only update if the user isn't currently dragging the thumb
                // This prevents jittery movement during dragging + playback
                if (!trackBarSeek.Capture) // Capture is true when mouse is down on it
                {
                    trackBarSeek.Value = frameNumber;
                }
            }
        };
        if (trackBarSeek.InvokeRequired)
        {
            trackBarSeek.Invoke(updateAction);
        }
        else
        {
            updateAction();
        }
    }

    public void ShowErrorMessage(string title, string message)
    {
        // MessageBox is thread-safe, but good practice if called from other threads
        if (this.InvokeRequired)
        {
            this.Invoke(new Action(() => MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error)));
        }
        else
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public string AskUserForFilePath()
    {
        if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
        {
            return openFileDialog1.FileName;
        }
        return null; // User cancelled
    }

    public void ClearDisplay()
    {
        Action clearAction = () =>
        {
            pictureBoxDisplay.Image?.Dispose();
            pictureBoxDisplay.Image = null;
        };

        if (pictureBoxDisplay.InvokeRequired)
        {
            pictureBoxDisplay.Invoke(clearAction);
        }
        else
        {
            clearAction();
        }
    }

    // Ensure timer is stopped and resources potentially released when form closes
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _presenter?.Cleanup(); // Ask presenter to clean up model resources
        base.OnFormClosing(e);
    }
    

    private void MainForm_Load(object sender, EventArgs e)
    {
    }

}