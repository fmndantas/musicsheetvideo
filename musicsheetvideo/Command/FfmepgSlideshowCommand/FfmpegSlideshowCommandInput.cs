namespace musicsheetvideo.Command.FfmepgSlideshowCommand;

public class FfmpegSlideshowCommandInput
{
    public FfmpegSlideshowCommandInput(string textInputPath, string outputVideoPath)
    {
        TextInputPath = textInputPath;
        OutputVideoPath = outputVideoPath;
    }

    public string TextInputPath { get; }
    public string OutputVideoPath { get; }
}