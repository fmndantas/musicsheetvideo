namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly IGapFiller _gapFiller;
    private readonly IVideoProducer _videoProducer;

    public MusicSheetVideo(
        MusicSheetVideoConfiguration configuration,
        IGapFiller gapFiller,
        IVideoProducer videoProducer
    )
    {
        _configuration = configuration;
        _gapFiller = gapFiller;
        _videoProducer = videoProducer;
    }

    public async Task MakeVideo(List<Page> pages)
    {
        var filledPages = _gapFiller.FillGap(pages);
        await _videoProducer.MakeVideo(filledPages);
    }
}