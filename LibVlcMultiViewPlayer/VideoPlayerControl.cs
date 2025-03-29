using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace LibVlcMultiViewPlayer;

public partial class VideoPlayerControl : UserControl
{
    // Events to notify the Presenter about user actions
    public event EventHandler PlayPauseClicked;
    public event EventHandler MuteToggleClicked;
    public event EventHandler CloseRequested;
    public event EventHandler<float> SeekRequested; // Value between 0.0f and 1.0f

    // The actual VLC video view
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

    // The actual VLC video view
    public VideoView VlcVideoView { get; private set; }

    public VideoPlayerControl()
    {
        InitializeComponent();

        // Create and add the VideoView programmatically AFTER InitializeComponent
        VlcVideoView = new VideoView { Dock = DockStyle.Fill };
        panelVideo.Controls.Add(VlcVideoView); // Add to the top panel

        // Wire up control events to raise our custom events
        btnPlayPause.Click += (s, e) => PlayPauseClicked?.Invoke(this, EventArgs.Empty);
        btnMute.Click += (s, e) => MuteToggleClicked?.Invoke(this, EventArgs.Empty);
        btnClose.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);
        trackBarSeek.Scroll += TrackBarSeek_Scroll;
    }

    private void TrackBarSeek_Scroll(object sender, EventArgs e)
    {
        // Convert TrackBar value (0-1000) to float position (0.0f - 1.0f)
        SeekRequested?.Invoke(this, trackBarSeek.Value / 1000f);
    }

    // --- Methods to Update UI (Called by Presenter - Thread Safe) ---

    public void UpdatePlayPauseButton(bool isPlaying)
    {
        if (InvokeRequired)
        {
            BeginInvoke((MethodInvoker)(() => btnPlayPause.Text = isPlaying ? "Pause" : "Play"));
        }
        else
        {
            btnPlayPause.Text = isPlaying ? "Pause" : "Play";
        }
    }

    public void UpdateMuteButton(bool isMuted)
    {
        if (InvokeRequired)
        {
            BeginInvoke((MethodInvoker)(() => btnMute.Text = isMuted ? "Unmute" : "Mute"));
        }
        else
        {
            btnMute.Text = isMuted ? "Unmute" : "Mute";
        }
    }

    public void UpdateSeekBar(long time, long length)
    {
        if (InvokeRequired)
        {
            BeginInvoke((MethodInvoker)(() => UpdateSeekBarInternal(time, length)));
        }
        else
        {
            UpdateSeekBarInternal(time, length);
        }
    }

    private void UpdateSeekBarInternal(long time, long length)
    {
        if (trackBarSeek.Capture) return; // Don't update if user is dragging

        if (length > 0)
        {
            float position = (float)time / length;
            trackBarSeek.Value = (int)(position * 1000);
            lblTime.Text = $"{TimeSpan.FromMilliseconds(time):mm\\:ss} / {TimeSpan.FromMilliseconds(length):mm\\:ss}";
        }
        else
        {
            trackBarSeek.Value = 0;
            lblTime.Text = $"{TimeSpan.FromMilliseconds(time):mm\\:ss} / --:--";
        }
    }

    // Optional: Prevent flickering during resize
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
            return cp;
        }
    }
}