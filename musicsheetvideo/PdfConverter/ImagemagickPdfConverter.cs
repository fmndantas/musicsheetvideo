using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ICommand _command;

    public ImagemagickPdfConverter(ICommand command)
    {
        _command = command;
    }

    public void ConvertPdfToImages(MusicSheetVideoConfiguration configuration)
    {
        if (!Directory.Exists(configuration.ImagesPath))
        {
            Directory.CreateDirectory(configuration.ImagesPath);
        }

        _command.Do();
    }
}