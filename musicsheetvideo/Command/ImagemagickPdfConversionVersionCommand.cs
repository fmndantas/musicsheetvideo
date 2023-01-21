namespace musicsheetvideo.Command;

public class ImagemagickPdfConversionVersionCommand : ShellCommand
{
    public ImagemagickPdfConversionVersionCommand(IProgressNotification progressNotification) : base(
        progressNotification)
    {
    }

    protected override string CommandName => "convert";
    protected override string Arguments => "--version";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress("Getting Imagemagick version");
    }
}