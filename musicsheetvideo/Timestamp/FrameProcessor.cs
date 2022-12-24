namespace musicsheetvideo.Timestamp;

public class FrameProcessor : IFrameProcessor
{
    private readonly IIntervalProcessor _intervalProcessor;
    private readonly IProgressNotification _progressNotification;

    public FrameProcessor(IIntervalProcessor intervalProcessor, IProgressNotification progressNotification)
    {
        _intervalProcessor = intervalProcessor;
        _progressNotification = progressNotification;
    }

    public List<Frame> ProcessFrames(List<Frame> frames)
    {
        var intervals = frames
            .Select(x => x.Interval)
            .ToList();
        var treatedIntervals = _intervalProcessor.ProcessIntervals(intervals);
        var treatedFrames = new List<Frame>();
        var i = 0;
        _progressNotification.NotifyProgress("Generating frames");
        foreach (var filledInterval in treatedIntervals)
        {
            var frame = new Frame(filledInterval, filledInterval.FillingGap ? Frame.PageToFillingFrame : frames[i++].PageNumber);
            treatedFrames.Add(frame);
            _progressNotification.NotifyProgress($"Generated frame: {frame}");
        }

        return treatedFrames;
    }
}