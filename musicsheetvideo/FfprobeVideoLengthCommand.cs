namespace musicsheetvideo;

public class FfprobeVideoLengthCommand : ShellCommand
{
    public FfprobeVideoLengthCommand(MusicSheetVideoConfiguration configuration) : base(
        "ffprobe",
        $"-v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 {configuration.VideoPath}"
    )
    {
    }
}