namespace musicsheetvideo;

public class FfmpegJoinAudioCommand : ShellCommand
{
    public FfmpegJoinAudioCommand(MusicSheetVideoConfiguration configuration) : base(
        "ffmpeg",
        $"-y -i {configuration.VideoPath} -i {configuration.AudioPath} -shortest {configuration.FinalVideoPath}"
    )
    {
    }
}