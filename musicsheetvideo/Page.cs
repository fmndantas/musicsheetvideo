namespace musicsheetvideo;

public class Page : IComparable
{
    private Interval Interval { get; }
    public int PageNumber { get; }

    public Page(Interval interval, int pageNumber)
    {
        Interval = interval;
        PageNumber = pageNumber;
    }

    public Page Gap(Page nextPage)
    {
        return new Page(Interval.Gap(nextPage.Interval), -1);
    }

    public long LengthMilisseconds => Interval.LengthMilisseconds;

    public Page DecreaseOneMilissecondEnd()
    {
        return new Page(Interval.DecreaseOneMilissecond(), PageNumber);
    }

    public int CompareTo(object? obj)
    {
        if (obj == null)
        {
            return 0;
        }

        var otherPage = obj as Page;
        if (otherPage == null)
        {
            throw new Exception("Object is not a Page");
        }

        return Interval.CompareTo(otherPage.Interval);
    }
}