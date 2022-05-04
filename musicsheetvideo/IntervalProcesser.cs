namespace musicsheetvideo;

public class IntervalProcesser : IIntervalProcesser
{
    public List<Interval> ProcessIntervals(List<Interval> intervals)
    {
        intervals.Sort();
        var processedIntervals = new List<Interval>();
        for (var i = 0; i < intervals.Count; ++i)
        {
            processedIntervals.Add(intervals[i].DecreaseOneMilissecondEnd());
            if (i + 1 < intervals.Count)
            {
                var gapFilling = intervals[i]
                    .Gap(intervals[i + 1])
                    .DecreaseOneMilissecondEnd();
                if (gapFilling.LengthMilisseconds > 0)
                {
                    gapFilling.FillingGap = true;
                    processedIntervals.Add(gapFilling);
                }
            }
        }

        return processedIntervals;
    }
}