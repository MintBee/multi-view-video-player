namespace SimpleViewMultiViewPlayer;

public interface IVideoPlayerView
{
    // --- Methods for Presenter to update the View ---
    void DisplayFrame(Bitmap frame);
    void SetStatus(string status);
    void SetPlaybackControlsEnabled(bool isVideoLoaded, bool isPlaying);
    void SetTrackBarRange(int maxFrames);
    void UpdateTrackBarPosition(int frameNumber);
    void ShowErrorMessage(string title, string message);
    string AskUserForFilePath();
    void ClearDisplay(); // To clear the picture box when stopped

    // --- Events raised by the View for Presenter to handle ---
    event EventHandler OpenFileClicked;
    event EventHandler PlayClicked;
    event EventHandler PauseClicked;
    event EventHandler StopClicked;
    event EventHandler<int> Seek; // Event passes the target frame number
}