namespace musicsheetvideo.Timestamp;

public class Tick : IComparable
{
    private readonly int _minutes;
    private readonly int _seconds;
    private readonly int _milliseconds;

    public Tick(int minutes, int seconds, int milliseconds)
    {
        _minutes = minutes;
        _seconds = seconds;
        _milliseconds = milliseconds;
    }

    private Tick Delta(Tick otherTick)
    {
        return new Tick(
            otherTick._minutes - _minutes,
            otherTick._seconds - _seconds,
            otherTick._milliseconds - _milliseconds
        );
    }

    public long DeltaMilisseconds(Tick otherTick)
    {
        var delta = Delta(otherTick);
        return 60000 * delta._minutes + 1000 * delta._seconds + delta._milliseconds;
    }

    public long DurationMilissecondsToZero
        => new Tick(0, 0, 0).DeltaMilisseconds(this);

    public int CompareTo(object? obj)
    {
        if (obj == null) return 0;
        var otherTick = obj as Tick;
        if (otherTick == null)
        {
            throw new Exception("Object is not a Tick");
        }

        if (_minutes == otherTick._minutes)
        {
            if (_seconds == otherTick._seconds)
            {
                return _milliseconds.CompareTo(otherTick._milliseconds);
            }

            return _seconds.CompareTo(otherTick._seconds);
        }

        return _minutes.CompareTo(otherTick._minutes);
    }

    public override string ToString()
    {
        return
            $"{_minutes.ToString().PadLeft(2, '0')}:{_seconds.ToString().PadLeft(2, '0')}:{_milliseconds.ToString().PadLeft(3, '0')}";
    }
}