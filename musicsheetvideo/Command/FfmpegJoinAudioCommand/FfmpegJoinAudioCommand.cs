namespace musicsheetvideo.Command.FfmpegJoinAudioCommand;

public class FfmpegJoinAudioCommand : ShellCommand<FfmpegJoinAudioCommandInput>
{
    public FfmpegJoinAudioCommand(FfmpegJoinAudioCommandInput configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => "-hide_banner -loglevel error -y " +
                                           $"-i {Configuration.InputVideoPath} " +
                                           $"-i {Configuration.AudioPath} " +
                                           $"-shortest {Configuration.OutputVideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Joining audio with slideshow through ffmpeg. Output path is \"{Configuration.OutputVideoPath}\"");
    }
}