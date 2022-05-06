namespace musicsheetvideo;

public class OverlappingIntervalsException : Exception
{
    public OverlappingIntervalsException() : base("Intervals are overlapping")
    {
    }

    public OverlappingIntervalsException(string message) : base(message)
    {
    }

    public OverlappingIntervalsException(string message, Exception ex) : base(message, ex)
    {
    }
}