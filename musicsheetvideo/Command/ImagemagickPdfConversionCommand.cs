namespace musicsheetvideo.Command;

public class ImagemagickPdfConversionCommand : ShellCommand
{
    public ImagemagickPdfConversionCommand(MusicSheetVideoConfiguration configuration) : base(
        "convert",
        "-density 300 " +
        configuration.PdfPath + " " +
        "-quality 100 " +
        $"{configuration.ImagesPath}/{configuration.ImagePrefix}-%d.{configuration.ImageFormat}"
    )
    {
    }
}