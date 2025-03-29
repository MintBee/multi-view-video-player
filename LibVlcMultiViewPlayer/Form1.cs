using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LibVlcMultiViewPlayer;

public partial class Form1 : Form
{
    private LibVLC _libVLC;
    private List<VideoPlayerPresenter> _presenters = new List<VideoPlayerPresenter>();
    private const int MAX_PLAYERS = 16;

    public Form1()
    {
        InitializeComponent();

        // Initialize LibVLCSharp
        Core.Initialize();
        
        string[] vlcOptions = new string[]
        {
            "--no-video-title-show", // Hide video title
            "--avcodec-hw=any" // Use any available hardware acceleration
        };
        
        _libVLC = new LibVLC(vlcOptions); // Create one LibVLC instance for the app

        this.FormClosing += Form1_FormClosing;
    }

    private void btnAddVideo_Click(object sender, EventArgs e)
    {
        if (_presenters.Count >= MAX_PLAYERS)
        {
            MessageBox.Show($"Maximum number of players ({MAX_PLAYERS}) reached.", "Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Filter = "Video Files|*.mp4;*.avi;*.mkv;*.mov;*.wmv|All Files|*.*";
            ofd.Title = "Select Video File";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AddVideoPlayer(ofd.FileName);
            }
        }
    }

    private void AddVideoPlayer(string filePath)
    {
        var view = new VideoPlayerControl
        {
            // Set a reasonable default size for the control within the FlowLayoutPanel
            Width = 320,
            Height = 240,
            Margin = new Padding(5) // Add some spacing
        };

        var presenter = new VideoPlayerPresenter(
            view,
            _libVLC,
            HandleUnmuteRequest, // Pass the callback for unmute requests
            HandleCloseRequest   // Pass the callback for close requests
        );

        _presenters.Add(presenter);
        flowLayoutPanelPlayers.Controls.Add(view); // Add the view to the layout panel

        presenter.LoadMedia(filePath); // Load and start the video (muted)
    }

    // Callback for Presenter requesting to be unmuted
    private void HandleUnmuteRequest(VideoPlayerPresenter requestingPresenter)
    {
        foreach (var presenter in _presenters)
        {
            if (presenter != requestingPresenter)
            {
                presenter.ForceMute(true); // Mute others
            }
        }
        requestingPresenter.ForceMute(false); // Unmute the requester
    }

    // Callback for Presenter requesting to be closed
    private void HandleCloseRequest(VideoPlayerPresenter requestingPresenter)
    {
        // Find the view associated with the presenter (we stored it in the layout panel)
        var viewToRemove = flowLayoutPanelPlayers.Controls.OfType<VideoPlayerControl>()
            .FirstOrDefault(v => v.VlcVideoView.MediaPlayer == requestingPresenter.MediaPlayer);

        _presenters.Remove(requestingPresenter); // Remove from our list

        if (viewToRemove != null)
        {
            flowLayoutPanelPlayers.Controls.Remove(viewToRemove); // Remove from UI
            viewToRemove.Dispose(); // Dispose the UserControl
        }

        requestingPresenter.Dispose(); // Dispose the presenter and its MediaPlayer

        Console.WriteLine($"Players remaining: {_presenters.Count}");
    }


    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Dispose all presenters and their resources when the form closes
        foreach (var presenter in _presenters.ToList()) // Iterate on a copy
        {
            HandleCloseRequest(presenter); // Reuse close logic for cleanup
        }
        _presenters.Clear();

        // Dispose the main LibVLC instance
        _libVLC?.Dispose();
        _libVLC = null;
    }
}