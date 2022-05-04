namespace musicsheetvideo;

public class FfmpegVideoProducer : IVideoProducer
{
    private readonly MusicSheetVideoConfiguration _configuration;

    public FfmpegVideoProducer(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task MakeVideo(List<Frame> frames)
    {
        await using var sw = new StreamWriter(_configuration.InputPath);
        foreach (var frame in frames)
        {
            var imageOutputPath = Path.Combine(_configuration.ImagesPath, $"{frame.PageNumber}.png");
            await sw.WriteLineAsync($"file {imageOutputPath}");
            await sw.WriteLineAsync($"duration {Convert.ToDecimal(frame.LengthMilisseconds / 1000.0)}");
            if (frame.Equals(frames.Last()))
            {
                await sw.WriteLineAsync($"file {imageOutputPath}");
            }
        }

        sw.Close();
        var slideshowCommand = new FfmpegSlideshowCommand(_configuration);
        await slideshowCommand.Do();
    }
}