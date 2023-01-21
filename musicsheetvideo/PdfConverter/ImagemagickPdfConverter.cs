using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ICommand _command;
    private readonly IProgressNotification _progressNotification;

    private void AssertImagemagickIsInstalled()
    {
        new ImagemagickPdfConversionVersionCommand(_progressNotification).Do();
    }

    public ImagemagickPdfConverter(ICommand command, IProgressNotification progressNotification)
    {
        _command = command;
        _progressNotification = progressNotification;
        AssertImagemagickIsInstalled();
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