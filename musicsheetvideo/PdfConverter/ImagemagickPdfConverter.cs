using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ShellCommand _command;
    private readonly MusicSheetVideoConfiguration _configuration;

    public ImagemagickPdfConverter(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
        _command = new ImagemagickPdfConversionCommand(configuration);
    }

    public void ConvertPdfToImages()
    {
        if (!Directory.Exists(_configuration.ImagesPath))
        {
            Directory.CreateDirectory(_configuration.ImagesPath);
        }

        _command.Do();
    }
}