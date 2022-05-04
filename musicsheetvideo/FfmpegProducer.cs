namespace musicsheetvideo;

public class FfmpegProducer : IVideoProducer
{
    private readonly MusicSheetVideoConfiguration _configuration;

    public FfmpegProducer(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task MakeVideo(List<Page> pages)
    {
        await using var sw = new StreamWriter(_configuration.InputPath);
        foreach (var processedPage in pages)
        {
            var imageOutputPath = Path.Combine(_configuration.ImagesPath, $"{processedPage.PageNumber}.png");
            await sw.WriteLineAsync($"file {imageOutputPath}");
            await sw.WriteLineAsync($"duration {Convert.ToDecimal(processedPage.LengthMilisseconds / 1000.0)}");
            if (processedPage.Equals(pages.Last()))
            {
                await sw.WriteLineAsync($"file {imageOutputPath}");
            }
        }

        sw.Close();
        var slideshowCommand = new FfmpegSlideshowCommand(_configuration);
        await slideshowCommand.Do();
    }
}