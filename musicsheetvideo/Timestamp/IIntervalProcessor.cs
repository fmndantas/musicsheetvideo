namespace musicsheetvideo.Timestamp;

public interface IIntervalProcessor
{
    List<Interval> ProcessIntervals(List<Interval> intervals);
}