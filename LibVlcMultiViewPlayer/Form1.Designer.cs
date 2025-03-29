namespace LibVlcMultiViewPlayer;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
        btnAddVideo = new System.Windows.Forms.Button();
        flowLayoutPanelPlayers = new System.Windows.Forms.FlowLayoutPanel();
        SuspendLayout();
        // 
        // btnAddVideo
        // 
        btnAddVideo.Dock = System.Windows.Forms.DockStyle.Top;
        btnAddVideo.Location = new System.Drawing.Point(0, 0);
        btnAddVideo.Name = "btnAddVideo";
        btnAddVideo.Size = new System.Drawing.Size(800, 45);
        btnAddVideo.TabIndex = 0;
        btnAddVideo.Text = "Add Video";
        btnAddVideo.UseVisualStyleBackColor = true;
        btnAddVideo.Click += btnAddVideo_Click;
        // 
        // flowLayoutPanelPlayers
        // 
        flowLayoutPanelPlayers.AutoScroll = true;
        flowLayoutPanelPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
        flowLayoutPanelPlayers.Location = new System.Drawing.Point(0, 45);
        flowLayoutPanelPlayers.Name = "flowLayoutPanelPlayers";
        flowLayoutPanelPlayers.Size = new System.Drawing.Size(800, 405);
        flowLayoutPanelPlayers.TabIndex = 1;
        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(flowLayoutPanelPlayers);
        Controls.Add(btnAddVideo);
        Text = "Form1";
        ResumeLayout(false);
    }

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelPlayers;

    private System.Windows.Forms.Button btnAddVideo;

    #endregion
}