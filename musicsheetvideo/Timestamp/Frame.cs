namespace musicsheetvideo.Timestamp;

public class Frame : IComparable
{
    public const int PageToFillingFrame = -1;
    public Interval Interval { get; }
    public int PageNumber { get; }

    public Frame(Interval interval, int pageNumber)
    {
        Interval = interval;
        PageNumber = pageNumber;
    }

    public decimal EndSecond => Interval.EndInSeconds;

    private long LengthMilisseconds => Interval.LengthMilisseconds;

    public double LengthSeconds => LengthMilisseconds / 1000.0;

    public bool FillingGap => Interval.FillingGap;

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 0;
        }

        var otherFrame = obj as Frame;
        if (otherFrame == null)
        {
            throw new Exception("Object is not a Frame");
        }

        return Interval.CompareTo(otherFrame.Interval);
    }

    public override string ToString()
    {
        return
            $"Frame[{(PageNumber == PageToFillingFrame ? "" : $"PageNumber={PageNumber}, ")}Interval={Interval}]";
    }
}