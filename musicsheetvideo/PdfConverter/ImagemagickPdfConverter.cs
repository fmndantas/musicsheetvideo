using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ICommand _command;

    public ImagemagickPdfConverter(ICommand command)
    {
        _command = command;
    }

    public void ConvertPdfToImages(IPdfConverterConfiguration configuration)
    {
        if (!Directory.Exists(configuration.ImagesDirectoryPath))
        {
            Directory.CreateDirectory(configuration.ImagesDirectoryPath);
        }

        _command.Do();
    }
}