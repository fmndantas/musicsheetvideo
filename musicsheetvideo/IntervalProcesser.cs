namespace musicsheetvideo;

public class IntervalProcesser : IIntervalProcesser
{
    public List<Interval> ProcessIntervals(List<Interval> intervals)
    {
        intervals.Sort();
        AssertAllIntervalsAreNonOverlapping(intervals);
        var processedIntervals = new List<Interval>();
        var tick0 = new Tick(0, 0, 0);
        var dummyInterval = new Interval(tick0, tick0)
            .Gap(intervals[0])
            .DecreaseOneMilissecondEnd();
        if (dummyInterval.LengthMilisseconds > 0)
        {
            dummyInterval.FillingGap = true;
            processedIntervals.Add(dummyInterval);
        }
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

    private void AssertAllIntervalsAreNonOverlapping(List<Interval> intervals)
    {
        var overlappingList = new List<List<int>>();
        for (var i = 0; i < intervals.Count - 1; ++i)
        {
            if (intervals[i].Overlaps(intervals[i + 1]))
            {
                overlappingList.Add(new List<int> { i, i + 1 });
            }
        }

        if (overlappingList.Count > 0)
        {
            var overlappingIntervalsText =
                string.Join(", ", overlappingList.Select(x => $"{x[0]} and {x[1]}"));
            throw new OverlappingIntervalsException($"Some intervals overlapped; they are ({overlappingIntervalsText})");
        }
    }
}