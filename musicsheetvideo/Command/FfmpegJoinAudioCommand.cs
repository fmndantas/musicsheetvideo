namespace musicsheetvideo.Command;

public class FfmpegJoinAudioCommand : ShellCommand
{
    public FfmpegJoinAudioCommand(MusicSheetVideoConfiguration configuration) : base(
        "ffmpeg",
        $"-hide_banner -loglevel error -y " +
        $"-i {configuration.VideoPath} -i {configuration.AudioPath} -shortest {configuration.FinalVideoPath}"
    )
    {
    }

    public override string DescribeItselfRunning => "Joining audio with slideshow through ffmpeg";
}