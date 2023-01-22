namespace musicsheetvideo.Command.FfmpegRescaleVideoCommand;

public class FfmpegRescaleVideoCommand : ShellCommand<FfmpegRescaleVideoCommandInput>
{
    public FfmpegRescaleVideoCommand(FfmpegRescaleVideoCommandInput configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => $"-y -i {Configuration.InputVideoPath} -vf scale=1920:1080,setsar=1:1 " +
                                           $"-hide_banner -loglevel error {Configuration.OutputVideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Rescaling video \"{Configuration.InputVideoPath}\". " +
                                            $"Output video is \"{Configuration.OutputVideoPath}\"");
    }
}