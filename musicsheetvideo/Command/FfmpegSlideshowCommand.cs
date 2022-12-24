namespace musicsheetvideo.Command;

public class FfmpegSlideshowCommand : ShellCommand
{
    public FfmpegSlideshowCommand(MusicSheetVideoConfiguration configuration) : base(
        "ffmpeg",
        $"-y " +
        "-f concat " +
        "-safe 0 " +
        $"-i {configuration.InputPath} " +
        "-vf \"scale=1920:1080:force_original_aspect_ratio=decrease,pad=1920:1080:(ow-iw)/2:(oh-ih)/2,setsar=1\" " +
        "-vsync vfr " +
        "-pix_fmt yuv420p " +
        "-hide_banner " +
        "-loglevel error " +
        $"{configuration.VideoPath}"
    )
    {
    }

    public override string DescribeItselfRunning => "Producing slideshow through ffmpeg";
}