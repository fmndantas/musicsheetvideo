namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly IIntervalProcesser _intervalProcesser;
    private readonly IVideoProducer _videoProducer;

    public MusicSheetVideo(
        MusicSheetVideoConfiguration configuration,
        IIntervalProcesser intervalProcesser,
        IVideoProducer videoProducer
    )
    {
        _configuration = configuration;
        _intervalProcesser = intervalProcesser;
        _videoProducer = videoProducer;
    }

    public async Task MakeVideo(List<Frame> frames)
    {
        var intervals = frames.Select(x => x.Interval).ToList();
        var treatedIntervals = _intervalProcesser.ProcessIntervals(intervals);
        var treatedFrames = new List<Frame>();
        var i = 0;
        foreach (var filledInterval in treatedIntervals)
        {
            treatedFrames.Add(new Frame(filledInterval, filledInterval.FillingGap ? -1 : frames[i++].PageNumber));
        }

        await _videoProducer.MakeVideo(treatedFrames);
    }
}