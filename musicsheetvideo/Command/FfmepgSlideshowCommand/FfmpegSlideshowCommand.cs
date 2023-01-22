namespace musicsheetvideo.Command.FfmepgSlideshowCommand;

public class FfmpegSlideshowCommand : ShellCommand<FfmpegSlideshowCommandInput>
{
    private readonly int _divideHeightBy;

    public FfmpegSlideshowCommand(FfmpegSlideshowCommandInput configuration, IProgressNotification progressNotification,
        int divideHeigthBy = 1) : base(configuration, progressNotification)
    {
        _divideHeightBy = divideHeigthBy;
    }

    private int Height => 1080 / _divideHeightBy;

    protected override string CommandName => "ffmpeg";

    protected override string Arguments => $"-y -f concat -safe 0 -i {Configuration.TextInputPath} " +
                                           $"-vf \"scale=1920:{Height}:force_original_aspect_ratio=decrease,pad=1920:{Height}:(ow-iw)/2:(oh-ih)/2,setsar=1\" " +
                                           $"-vsync vfr -pix_fmt yuv420p -hide_banner -loglevel error {Configuration.OutputVideoPath}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress(
            $"Producing slideshow through ffmpeg. Output path is \"{Configuration.OutputVideoPath}\"");
    }
}