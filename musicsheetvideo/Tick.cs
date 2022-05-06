namespace musicsheetvideo;

public class Tick : IComparable
{
    public int Minutes { get; }
    public int Seconds { get; }
    public int Milisseconds { get; }

    public Tick(int minutes, int seconds, int milisseconds)
    {
        Minutes = minutes;
        Seconds = seconds;
        Milisseconds = milisseconds;
    }

    private Tick Delta(Tick otherTick)
    {
        return new Tick(
            otherTick.Minutes - Minutes,
            otherTick.Seconds - Seconds,
            otherTick.Milisseconds - Milisseconds
        );
    }

    public long DeltaMilisseconds(Tick otherTick)
    {
        var delta = Delta(otherTick);
        return 60000 * delta.Minutes + 1000 * delta.Seconds + delta.Milisseconds;
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

        if (Minutes == otherTick.Minutes)
        {
            if (Seconds == otherTick.Seconds)
            {
                return Milisseconds.CompareTo(otherTick.Milisseconds);
            }

            return Seconds.CompareTo(otherTick.Seconds);
        }

        return Minutes.CompareTo(otherTick.Minutes);
    }

    public Tick DecreaseOneMilissecond()
    {
        if (Milisseconds > 0)
        {
            return new Tick(Minutes, Seconds, Milisseconds - 1);
        }

        if (Seconds > 0)
        {
            return new Tick(Minutes, Seconds - 1, 999);
        }

        if (Minutes > 0)
        {
            return new Tick(Minutes - 1, 59, 999);
        }

        return new Tick(0, 0, 0);
    }

    public override string ToString()
    {
        return $"[{Minutes}:{Seconds}:{Milisseconds}]";
    }
}