using musicsheetvideo.Command;
using musicsheetvideo.Timestamp;

namespace musicsheetvideo.VideoProducer;

public class FfmpegVideoMaker : IVideoMaker
{
    private readonly List<ICommand> _commands;
    private readonly IProgressNotification _progressNotification;

    private void AssertFfpmegIsInstalled()
    {
        new FfmpegVersionCommand(_progressNotification).Do();
    }

    public FfmpegVideoMaker(List<ICommand> commands, IProgressNotification progressNotification)
    {
        _commands = commands;
        _progressNotification = progressNotification;
        AssertFfpmegIsInstalled();
    }

    public void MakeVideo(List<Frame> frames, MusicSheetVideoConfiguration configuration)
    {
        using var sw = new StreamWriter(configuration.InputPath);
        _progressNotification.NotifyProgress(
            $"Producing ffmepg text file. Output path is {configuration.InputPath}");
        foreach (var frame in frames)
        {
            _progressNotification.NotifyProgress($"Processing frame {frame}");
            var imageOutputPath = frame.FillingGap
                ? configuration.DefaultImage
                : Path.Combine(configuration.ImagesPath,
                    $"{configuration.ImagePrefix}-{frame.PageNumber - 1}.{configuration.ImageFormat}");
            var duration = frame.LengthSeconds;
            sw.WriteLine($"file {imageOutputPath}");
            sw.WriteLine($"duration {duration:0.000}");
            if (frame.Equals(frames.Last()))
            {
                sw.WriteLine($"file {imageOutputPath}");
            }
        }

        sw.Close();
        foreach (var command in _commands)
        {
            command.Do();
        }
    }
}