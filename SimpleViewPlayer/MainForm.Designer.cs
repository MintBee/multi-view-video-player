using System.ComponentModel;

namespace SimpleViewMultiViewPlayer;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pictureBoxDisplay = new System.Windows.Forms.PictureBox();
        btnOpen = new System.Windows.Forms.Button();
        btnPlay = new System.Windows.Forms.Button();
        btnPause = new System.Windows.Forms.Button();
        btnStop = new System.Windows.Forms.Button();
        trackBarSeek = new System.Windows.Forms.TrackBar();
        lblStatus = new System.Windows.Forms.Label();
        playbackTimer = new System.Windows.Forms.Timer(components);
        openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        ((System.ComponentModel.ISupportInitialize)pictureBoxDisplay).BeginInit();
        ((System.ComponentModel.ISupportInitialize)trackBarSeek).BeginInit();
        SuspendLayout();
        // 
        // pictureBoxDisplay
        // 
        pictureBoxDisplay.AccessibleName = "pictureBoxDisplay";
        pictureBoxDisplay.Location = new System.Drawing.Point(144, 76);
        pictureBoxDisplay.Name = "pictureBoxDisplay";
        pictureBoxDisplay.Size = new System.Drawing.Size(368, 158);
        pictureBoxDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        pictureBoxDisplay.TabIndex = 0;
        pictureBoxDisplay.TabStop = false;
        // 
        // btnOpen
        // 
        btnOpen.AccessibleName = "btnOpen";
        btnOpen.Location = new System.Drawing.Point(65, 304);
        btnOpen.Name = "btnOpen";
        btnOpen.Size = new System.Drawing.Size(85, 62);
        btnOpen.TabIndex = 1;
        btnOpen.Text = "Open";
        btnOpen.UseVisualStyleBackColor = true;
        // 
        // btnPlay
        // 
        btnPlay.AccessibleName = "btnPlay";
        btnPlay.Location = new System.Drawing.Point(210, 304);
        btnPlay.Name = "btnPlay";
        btnPlay.Size = new System.Drawing.Size(85, 62);
        btnPlay.TabIndex = 2;
        btnPlay.Text = "Play";
        btnPlay.UseVisualStyleBackColor = true;
        // 
        // btnPause
        // 
        btnPause.AccessibleName = "btnPause";
        btnPause.Location = new System.Drawing.Point(360, 304);
        btnPause.Name = "btnPause";
        btnPause.Size = new System.Drawing.Size(85, 62);
        btnPause.TabIndex = 3;
        btnPause.Text = "Pause";
        btnPause.UseVisualStyleBackColor = true;
        // 
        // btnStop
        // 
        btnStop.AccessibleName = "btnStop";
        btnStop.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
        btnStop.Location = new System.Drawing.Point(514, 304);
        btnStop.Name = "btnStop";
        btnStop.Size = new System.Drawing.Size(85, 62);
        btnStop.TabIndex = 4;
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = true;
        // 
        // trackBarSeek
        // 
        trackBarSeek.AccessibleName = "trackBarSeek";
        trackBarSeek.Location = new System.Drawing.Point(210, 240);
        trackBarSeek.Maximum = 100;
        trackBarSeek.Name = "trackBarSeek";
        trackBarSeek.Size = new System.Drawing.Size(235, 58);
        trackBarSeek.TabIndex = 5;
        // 
        // lblStatus
        // 
        lblStatus.AccessibleName = "lblStatus";
        lblStatus.AutoSize = true;
        lblStatus.Location = new System.Drawing.Point(289, 34);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new System.Drawing.Size(84, 21);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "Status: Idle";
        // 
        // openFileDialog1
        // 
        openFileDialog1.FileName = "openFileDialog1";
        // 
        // MainForm
        // 
        ClientSize = new System.Drawing.Size(633, 396);
        Controls.Add(lblStatus);
        Controls.Add(trackBarSeek);
        Controls.Add(btnStop);
        Controls.Add(btnPause);
        Controls.Add(btnPlay);
        Controls.Add(btnOpen);
        Controls.Add(pictureBoxDisplay);
        Load += MainForm_Load;
        ((System.ComponentModel.ISupportInitialize)pictureBoxDisplay).EndInit();
        ((System.ComponentModel.ISupportInitialize)trackBarSeek).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private System.Windows.Forms.OpenFileDialog openFileDialog1;

    private System.Windows.Forms.Timer playbackTimer;

    private System.Windows.Forms.Label lblStatus;

    private System.Windows.Forms.TrackBar trackBarSeek;

    private System.Windows.Forms.Button btnPlay;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnStop;

    private System.Windows.Forms.Button btnOpen;

    private System.Windows.Forms.PictureBox pictureBoxDisplay;

    #endregion
}