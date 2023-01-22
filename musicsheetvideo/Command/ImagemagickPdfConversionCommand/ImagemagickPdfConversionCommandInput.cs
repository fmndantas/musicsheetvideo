namespace musicsheetvideo.Command.ImagemagickPdfConversionCommand;

public class ImagemagickPdfConversionCommandInput
{
    public ImagemagickPdfConversionCommandInput(string pdfPath, string imagesPath, string imagePrefix, string imageFormat)
    {
        PdfPath = pdfPath;
        ImagesPath = imagesPath;
        ImagePrefix = imagePrefix;
        ImageFormat = imageFormat;
    }

    public string PdfPath { get; }
    public string ImagesPath { get; }
    public string ImagePrefix { get; }
    public string ImageFormat { get; }
}