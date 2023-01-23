namespace musicsheetvideo.Command.ImagemagickPdfConversionCommand;

public class ImagemagickPdfConversionCommand : ShellCommand<ImagemagickPdfConversionCommandInput>
{
    public ImagemagickPdfConversionCommand(ImagemagickPdfConversionCommandInput configuration,
        IProgressNotification progressNotification) : base(configuration, progressNotification)
    {
    }

    protected override string CommandName => "convert";

    protected override string Arguments => $"-density 300 {Configuration.PdfPath} -quality 100 " +
                                           $"{Configuration.ImagesPath}/{Configuration.ImagePrefix}-%d.{Configuration.ImageFormat}";

    protected override void DescribeItselfRunning()
    {
        ProgressNotification.NotifyProgress($"Converting .pdf to images through Imagemagick. Output path is \"{Configuration.ImagesPath}\"");
    }
}