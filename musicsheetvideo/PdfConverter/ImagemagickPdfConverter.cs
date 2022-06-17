using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ShellCommand _command;

    public ImagemagickPdfConverter(MusicSheetVideoConfiguration configuration)
    {
        _command = new ImagemagickPdfConversionCommand(configuration);
    }

    public void ConvertPdfToImages()
    {
        _command.Do();
    }
}