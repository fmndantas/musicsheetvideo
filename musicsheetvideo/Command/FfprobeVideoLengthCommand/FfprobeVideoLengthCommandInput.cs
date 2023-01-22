namespace musicsheetvideo.Command.FfprobeVideoLengthCommand;

public class FfprobeVideoLengthCommandInput
{
    public FfprobeVideoLengthCommandInput(string inputVideoPath)
    {
        InputVideoPath = inputVideoPath;
    }

    public string InputVideoPath { get; }
}