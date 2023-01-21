namespace musicsheetvideo.Command;

public class FfmpegVersionCommand : ShellCommand
{
    public FfmpegVersionCommand(IProgressNotification progressNotification) : base(progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";
    protected override string Arguments => "-version";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress("Getting Ffpmeg version");
    }
}