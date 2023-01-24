namespace musicsheetvideo.Command.FfmpegJoinAudioCommand;

public class FfmpegJoinAudioCommandInput
{
    public FfmpegJoinAudioCommandInput(string inputVideoPath, string audioPath, string outputVideoPath)
    {
        InputVideoPath = inputVideoPath;
        AudioPath = audioPath;
        OutputVideoPath = outputVideoPath;
    }

    public string InputVideoPath { get; }
    public string AudioPath { get; }
    public string OutputVideoPath { get; }
}