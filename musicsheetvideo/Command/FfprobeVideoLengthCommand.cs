namespace musicsheetvideo.Command;

public class FfprobeVideoLengthCommand : ShellCommand
{
    public FfprobeVideoLengthCommand(MusicSheetVideoConfiguration configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffprobe";

    protected override string Arguments =>
        $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {Configuration.VideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress("Testing video length through ffmpeg");
    }
}