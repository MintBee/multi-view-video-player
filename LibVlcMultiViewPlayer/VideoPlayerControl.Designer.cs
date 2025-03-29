using System.ComponentModel;

namespace LibVlcMultiViewPlayer;

partial class VideoPlayerControl
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        panelVideo = new System.Windows.Forms.Panel();
        btnClose = new System.Windows.Forms.Button();
        lblTime = new System.Windows.Forms.Label();
        trackBarSeek = new System.Windows.Forms.TrackBar();
        panel1 = new System.Windows.Forms.Panel();
        btnMute = new System.Windows.Forms.Button();
        btnPlayPause = new System.Windows.Forms.Button();
        panelVideo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)trackBarSeek).BeginInit();
        panel1.SuspendLayout();
        SuspendLayout();
        // 
        // panelVideo
        // 
        panelVideo.Controls.Add(btnClose);
        panelVideo.Controls.Add(lblTime);
        panelVideo.Controls.Add(trackBarSeek);
        panelVideo.Controls.Add(panel1);
        panelVideo.Dock = System.Windows.Forms.DockStyle.Fill;
        panelVideo.Location = new System.Drawing.Point(0, 0);
        panelVideo.Name = "panelVideo";
        panelVideo.Size = new System.Drawing.Size(320, 218);
        panelVideo.TabIndex = 0;
        // 
        // btnClose
        // 
        btnClose.Location = new System.Drawing.Point(287, 6);
        btnClose.Name = "btnClose";
        btnClose.Size = new System.Drawing.Size(32, 29);
        btnClose.TabIndex = 3;
        btnClose.Text = "X";
        btnClose.UseVisualStyleBackColor = true;
        // 
        // lblTime
        // 
        lblTime.Location = new System.Drawing.Point(93, 11);
        lblTime.Name = "lblTime";
        lblTime.Size = new System.Drawing.Size(130, 25);
        lblTime.TabIndex = 2;
        lblTime.Text = "00:00 / 00:00";
        lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // trackBarSeek
        // 
        trackBarSeek.Location = new System.Drawing.Point(41, 121);
        trackBarSeek.Maximum = 1000;
        trackBarSeek.Name = "trackBarSeek";
        trackBarSeek.Size = new System.Drawing.Size(233, 58);
        trackBarSeek.TabIndex = 1;
        trackBarSeek.TickFrequency = 100;
        // 
        // panel1
        // 
        panel1.Controls.Add(btnMute);
        panel1.Controls.Add(btnPlayPause);
        panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
        panel1.Location = new System.Drawing.Point(0, 178);
        panel1.Name = "panel1";
        panel1.Size = new System.Drawing.Size(320, 40);
        panel1.TabIndex = 0;
        // 
        // btnMute
        // 
        btnMute.Location = new System.Drawing.Point(124, 7);
        btnMute.Name = "btnMute";
        btnMute.Size = new System.Drawing.Size(82, 27);
        btnMute.TabIndex = 2;
        btnMute.Text = "Unmute";
        btnMute.UseVisualStyleBackColor = true;
        // 
        // btnPlayPause
        // 
        btnPlayPause.Location = new System.Drawing.Point(13, 7);
        btnPlayPause.Name = "btnPlayPause";
        btnPlayPause.Size = new System.Drawing.Size(82, 27);
        btnPlayPause.TabIndex = 1;
        btnPlayPause.Text = "Play";
        btnPlayPause.UseVisualStyleBackColor = true;
        // 
        // VideoPlayerControl
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        Controls.Add(panelVideo);
        Size = new System.Drawing.Size(320, 218);
        panelVideo.ResumeLayout(false);
        panelVideo.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)trackBarSeek).EndInit();
        panel1.ResumeLayout(false);
        ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnClose;

    private System.Windows.Forms.Label lblTime;

    private System.Windows.Forms.TrackBar trackBarSeek;

    private System.Windows.Forms.Button btnMute;

    private System.Windows.Forms.Button btnPlayPause;

    private System.Windows.Forms.Panel panel1;

    private System.Windows.Forms.Panel panelVideo;

    #endregion
}