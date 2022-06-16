namespace musicsheetvideo.CustomException;

public class InvertedIntervalException : Exception
{
    public InvertedIntervalException() : base("Interval is inverted")
    {
    }

    public InvertedIntervalException(string message) : base(message)
    {
    }

    public InvertedIntervalException(string message, System.Exception ex) : base(message, ex)
    {
    }
}