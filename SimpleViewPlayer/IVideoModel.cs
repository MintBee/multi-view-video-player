namespace SimpleViewMultiViewPlayer;

public interface IVideoModel : IDisposable // Important for releasing resources
{
    bool LoadVideo(string filePath);
    Bitmap GetFrame(int frameNumber);
    Bitmap GetNextFrame(); // Gets frame at current position and advances
    bool Seek(int frameNumber);

    // Properties
    bool IsVideoLoaded { get; }
    int TotalFrames { get; }
    double FrameRate { get; }
    int CurrentFrameNumber { get; } // 0-based index
    Size FrameSize { get; }
}