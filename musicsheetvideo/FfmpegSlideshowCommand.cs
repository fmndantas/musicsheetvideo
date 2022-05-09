namespace musicsheetvideo;

public class FfmpegSlideshowCommand : ShellCommand
{
    public FfmpegSlideshowCommand(MusicSheetVideoConfiguration configuration) : base(
        "ffmpeg",
        $"-y -f concat -safe 0 -i {configuration.InputPath} " +
        "-vf \"crop=trunc(iw/2)*2:trunc(ih/2)*2\" -vsync vfr " +
        $"-pix_fmt yuv420p -hide_banner -loglevel error {configuration.VideoPath}"
    )
    {
    }
}