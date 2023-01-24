namespace musicsheetvideo.Command.FfmpegRescaleVideoCommand;

public class FfmpegRescaleVideoCommandInput
{
    public FfmpegRescaleVideoCommandInput(string inputVideoPath, string outputVideoPath)
    {
        InputVideoPath = inputVideoPath;
        OutputVideoPath = outputVideoPath;
    }

    public string OutputVideoPath { get; }
    public string InputVideoPath { get; }
}