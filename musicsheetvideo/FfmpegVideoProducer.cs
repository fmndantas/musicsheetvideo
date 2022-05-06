namespace musicsheetvideo;

public class FfmpegVideoProducer : IVideoProducer
{
    private readonly MusicSheetVideoConfiguration _configuration;

    public FfmpegVideoProducer(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void MakeVideo(List<Frame> frames)
    {
        using var sw = new StreamWriter(_configuration.InputPath);
        foreach (var frame in frames)
        {
            var imageOutputPath = Path.Combine(_configuration.ImagesPath, $"{frame.PageNumber}.png");
            sw.WriteLine($"file {imageOutputPath}");
            sw.WriteLine($"duration {Convert.ToDecimal(frame.LengthMilisseconds / 1000.0)}");
            if (frame.Equals(frames.Last()))
            {
                sw.WriteLine($"file {imageOutputPath}");
            }
        }

        sw.Close();
        var slideshowCommand = new FfmpegSlideshowCommand(_configuration);
        var joinAudioCommand = new FfmpegJoinAudioCommand(_configuration);
        slideshowCommand.Do();
        joinAudioCommand.Do();
    }
}