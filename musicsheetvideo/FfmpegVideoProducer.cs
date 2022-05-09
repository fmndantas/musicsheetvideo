namespace musicsheetvideo;

public class FfmpegVideoProducer : IVideoProducer
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly List<ICommand> _commands;

    public FfmpegVideoProducer(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
        _commands = new List<ICommand>
        {
            new FfmpegSlideshowCommand(configuration),
            new FfmpegJoinAudioCommand(configuration)
        };
    }

    public void MakeVideo(List<Frame> frames)
    {
        using var sw = new StreamWriter(_configuration.InputPath);
        foreach (var frame in frames)
        {
            var imageOutputPath = frame.FillingGap
                ? _configuration.DefaultImage
                : Path.Combine(_configuration.ImagesPath, $"{frame.PageNumber}.png");
            var duration = frame.LengthMilisseconds / 1000.0;
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