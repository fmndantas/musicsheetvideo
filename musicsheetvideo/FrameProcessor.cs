namespace musicsheetvideo;

public class FrameProcessor : IFrameProcessor
{
    private readonly IIntervalProcessor _intervalProcessor;

    public FrameProcessor(IIntervalProcessor intervalProcessor)
    {
        _intervalProcessor = intervalProcessor;
    }

    public List<Frame> ProcessFrames(List<Frame> frames)
    {
        var intervals = frames
            .Select(x => x.Interval)
            .ToList();
        var treatedIntervals = _intervalProcessor.ProcessIntervals(intervals);
        var treatedFrames = new List<Frame>();
        var i = 0;
        foreach (var filledInterval in treatedIntervals)
        {
            treatedFrames.Add(new Frame(
                filledInterval,
                filledInterval.FillingGap
                    ? -1
                    : frames[i++].PageNumber
            ));
        }

        return treatedFrames;
    }
}