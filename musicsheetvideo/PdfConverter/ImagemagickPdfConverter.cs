using musicsheetvideo.Command;

namespace musicsheetvideo.PdfConverter;

public class ImagemagickPdfConverter : IPdfConverter
{
    private readonly ICommand _command;
    private readonly IProgressNotification _progressNotification;

    public ImagemagickPdfConverter(IProgressNotification progressNotification, ICommand command)
    {
        _command = command;
        _progressNotification = progressNotification;
    }

    public void ConvertPdfToImages(MusicSheetVideoConfiguration configuration)
    {
        if (!Directory.Exists(configuration.ImagesPath))
        {
            Directory.CreateDirectory(configuration.ImagesPath);
        }

        _progressNotification.NotifyProgress($"{_command.DescribeItselfRunning}. Output directory is {configuration.ImagesPath}");
        _command.Do();
    }
}