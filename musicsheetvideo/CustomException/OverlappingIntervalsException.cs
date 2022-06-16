namespace musicsheetvideo.CustomException;

public class OverlappingIntervalsException : System.Exception
{
    public OverlappingIntervalsException() : base("Intervals are overlapping")
    {
    }

    public OverlappingIntervalsException(string message) : base(message)
    {
    }

    public OverlappingIntervalsException(string message, System.Exception ex) : base(message, ex)
    {
    }
}