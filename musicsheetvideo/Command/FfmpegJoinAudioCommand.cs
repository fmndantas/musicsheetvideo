namespace musicsheetvideo.Command;

public class FfmpegJoinAudioCommand : ShellCommand
{
    public FfmpegJoinAudioCommand(MusicSheetVideoConfiguration configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => "-hide_banner -loglevel error -y " +
                                           $"-i {Configuration.SlideshowVideoPath} " +
                                           $"-i {Configuration.AudioPath} " +
                                           $"-shortest {Configuration.OutputVideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Joining audio with slideshow through ffmpeg. Output path is \"{Configuration.OutputVideoPath}\"");
    }
}