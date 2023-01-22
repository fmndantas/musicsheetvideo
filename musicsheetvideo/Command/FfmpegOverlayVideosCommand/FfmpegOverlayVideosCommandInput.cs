namespace musicsheetvideo.Command.FfmpegOverlayVideosCommand;

public class FfmpegOverlayVideosCommandInput
{
    public FfmpegOverlayVideosCommandInput(string videoBelowPath, string videoAbovePath, string outputVideoPath)
    {
        VideoBelowPath = videoBelowPath;
        VideoAbovePath = videoAbovePath;
        OutputVideoPath = outputVideoPath;
    }

    public string VideoAbovePath { get; }
    public string VideoBelowPath { get; }
    public string OutputVideoPath { get; }
}