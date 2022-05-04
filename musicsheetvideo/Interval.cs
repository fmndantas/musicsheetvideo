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

    public int CompareTo(object? obj)
    {
        if (obj == null) return 0;
        var otherInterval = obj as Interval;
        if (otherInterval == null) throw new Exception("Object is not a Interval");
        return _start.CompareTo(otherInterval._start);
    }

    public Interval Gap(Interval nextInterval)
    {
        return new Interval(_end, nextInterval._start);
    }

    public Interval DecreaseOneMilissecondEnd()
    {
        return new Interval(_start, _end.DecreaseOneMilissecond());
    }
    
}