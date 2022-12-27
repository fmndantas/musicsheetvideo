namespace musicsheetvideo.Command;

public class FfmpegSlideshowCommand : ShellCommand
{
    public FfmpegSlideshowCommand(MusicSheetVideoConfiguration configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => $"-y -f concat -safe 0 -i {Configuration.InputPath} " +
                                           "-vf \"scale=1920:1080:force_original_aspect_ratio=decrease,pad=1920:1080:(ow-iw)/2:(oh-ih)/2,setsar=1\" " +
                                           $"-vsync vfr -pix_fmt yuv420p -hide_banner -loglevel error {Configuration.VideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Producing slideshow through ffmpeg. Output path is \"{Configuration.VideoPath}\"");
    }
}