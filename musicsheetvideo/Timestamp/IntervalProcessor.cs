using musicsheetvideo.CustomException;

namespace musicsheetvideo.Timestamp;

public class IntervalProcessor : IIntervalProcessor
{
    public List<Interval> ProcessIntervals(List<Interval> intervals)
    {
        intervals.Sort();
        AssertAllIntervalsAreNonOverlapping(intervals);
        var processedIntervals = new List<Interval>();
        var tick0 = new Tick(0, 0, 0);
        var dummyInterval = new Interval(tick0, tick0).Gap(intervals[0]);
        if (dummyInterval.LengthMilisseconds > 0)
        {
            processedIntervals.Add(dummyInterval);
        }

        for (var i = 0; i < intervals.Count; ++i)
        {
            processedIntervals.Add(intervals[i]);
            AddGapUntilNextIntervalIfItsLengthIsPositive(i, intervals, processedIntervals);
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
            throw new OverlappingIntervalsException(
                $"Some intervals overlapped; they are ({overlappingIntervalsText})");
        }
    }

    private void AddGapUntilNextIntervalIfItsLengthIsPositive(
        int position,
        List<Interval> allIntervals,
        List<Interval> processedIntervals
    )
    {
        if (position + 1 >= allIntervals.Count) return;
        var gapFilling = allIntervals[position].Gap(allIntervals[position + 1]);
        if (gapFilling.LengthMilisseconds > 0)
        {
            processedIntervals.Add(gapFilling);
        }
    }
}