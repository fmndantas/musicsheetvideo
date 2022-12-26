using musicsheetvideo.CustomException;

namespace musicsheetvideo.Timestamp;

public class Interval : IComparable
{
    private readonly Tick _start;
    private readonly Tick _end;

    public bool FillingGap { get; }

    public Interval(Tick start, Tick end)
    {
        _start = start;
        _end = end;
        FillingGap = false;
        AssertIntervalIsNotInverted(start, end);
    }

    private Interval(Tick start, Tick end, bool toFillGap)
    {
        _start = start;
        _end = end;
        FillingGap = toFillGap;
        AssertIntervalIsNotInverted(start, end);
    }

    private void AssertIntervalIsNotInverted(Tick start, Tick end)
    {
        var startUntil0 = start.DurationMilissecondsToZero;
        var endUntil0 = end.DurationMilissecondsToZero;
        if (startUntil0 > endUntil0)
        {
            throw new InvertedIntervalException(
                $"the interval is inverted; start tick {start} should be less or equal than end tick {end}"
            );
        }
    }

    public long LengthMilisseconds => _start.DeltaMilisseconds(_end);

    public decimal EndInSeconds => Convert.ToDecimal(_end.DurationMilissecondsToZero / 1000.0);

    public Interval Gap(Interval nextInterval)
    {
        var result = new Interval(_end, nextInterval._start, true);
        return result;
    }

    public bool Overlaps(Interval other)
    {
        var intervals = new List<Interval> { this, other };
        intervals.Sort();
        return intervals[0]._end.CompareTo(intervals[1]._start) > 0;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null) return 0;
        var otherInterval = obj as Interval;
        if (otherInterval == null) throw new Exception("Object is not a Interval");
        return _start.CompareTo(otherInterval._start);
    }

    public override string ToString()
    {
        return $"[Start={_start}->End={_end}]";
    }
}