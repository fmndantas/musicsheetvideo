namespace musicsheetvideo;

public class Interval : IComparable
{
    private readonly Tick _start;
    private readonly Tick _end;

    public bool FillingGap { get; set; }

    public Interval(Tick start, Tick end)
    {
        _start = start;
        _end = end;
        FillingGap = false;
    }

    public long LengthMilisseconds => _start.DeltaMilisseconds(_end);

    public decimal EndInSeconds => Convert.ToDecimal(_end.DurationMilissecondsToZero / 1000.0);

    public Interval Gap(Interval nextInterval)
    {
        var result = new Interval(_end, nextInterval._start);
        result.FillingGap = true;
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
}