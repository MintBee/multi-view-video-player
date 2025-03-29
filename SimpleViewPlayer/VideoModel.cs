using Emgu.CV;
using Emgu.CV.CvEnum;

// For CapProp

namespace SimpleViewMultiViewPlayer;

public class VideoModel : IVideoModel
{
    private VideoCapture _capture;
    private Mat _currentFrameMat; // Reusable Mat object

    public bool IsVideoLoaded { get; private set; } = false;
    public int TotalFrames { get; private set; } = 0;
    public double FrameRate { get; private set; } = 0;
    public Size FrameSize { get; private set; } = Size.Empty;

    // CurrentFrameNumber property to get the *next* frame index
    public int CurrentFrameNumber => _capture != null ? (int)_capture.Get(CapProp.PosFrames) : 0;

    public VideoModel()
    {
        _currentFrameMat = new Mat(); // Initialize reusable Mat
    }

    public bool LoadVideo(string filePath)
    {
        DisposeCapture(); // Release previous video if any

        try
        {
            _capture = new VideoCapture(filePath);

            if (!_capture.IsOpened)
            {
                IsVideoLoaded = false;
                return false;
            }

            TotalFrames = (int)_capture.Get(CapProp.FrameCount);
            FrameRate = _capture.Get(CapProp.Fps);
            FrameSize = new Size(
                (int)_capture.Get(CapProp.FrameWidth),
                (int)_capture.Get(CapProp.FrameHeight)
            );
            IsVideoLoaded = true;
            Seek(0); // Go to beginning
            return true;
        }
        catch (Exception) // Catch potential exceptions during VideoCapture creation
        {
            DisposeCapture();
            IsVideoLoaded = false;
            return false;
        }
    }

    // Gets a specific frame without changing the internal position
    public Bitmap GetFrame(int frameNumber)
    {
        if (!IsVideoLoaded || frameNumber < 0 || frameNumber >= TotalFrames)
            return null;

        // Store current position
        double currentPos = _capture.Get(CapProp.PosFrames);
        Bitmap frameBitmap = null;

        if (_capture.Set(CapProp.PosFrames, frameNumber))
        {
            if (_capture.Read(_currentFrameMat) && !_currentFrameMat.IsEmpty)
            {
                // Convert Mat to Bitmap. ToBitmap creates a copy.
                frameBitmap = _currentFrameMat.ToBitmap();
            }
        }

        // Restore original position
        _capture.Set(CapProp.PosFrames, currentPos);

        return frameBitmap;
    }

    // Gets the frame at the current position AND advances the position
    public Bitmap GetNextFrame()
    {
        if (!IsVideoLoaded || CurrentFrameNumber >= TotalFrames)
            return null; // End of video or not loaded

        try
        {
            // Read advances the internal frame pointer
            if (_capture.Read(_currentFrameMat) && !_currentFrameMat.IsEmpty)
            {
                // Convert Mat to Bitmap. ToBitmap() creates a managed copy.
                return _currentFrameMat.ToBitmap();
            }
            else
            {
                // Reached end or error reading frame
                return null;
            }
        }
        catch (Exception) // Handle potential errors during read/convert
        {
            return null;
        }
    }


    public bool Seek(int frameNumber)
    {
        if (!IsVideoLoaded || frameNumber < 0 || frameNumber >= TotalFrames)
            return false;

        // Setting position might not be perfectly accurate for all codecs/files
        return _capture.Set(CapProp.PosFrames, frameNumber);
    }

    public void Dispose()
    {
        DisposeCapture();
        _currentFrameMat?.Dispose(); // Dispose reusable Mat
    }

    private void DisposeCapture()
    {
        _capture?.Dispose(); // Release the VideoCapture object
        _capture = null;
        IsVideoLoaded = false; // Reset state
        TotalFrames = 0;
        FrameRate = 0;
        FrameSize = Size.Empty;
    }
}