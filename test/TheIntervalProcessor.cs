using System.Collections.Generic;
using musicsheetvideo.CustomException;
using musicsheetvideo.Timestamp;
using NUnit.Framework;

namespace test;

[TestFixture]
public class TheIntervalProcessor
{
    private readonly IIntervalProcessor _subject;

    public TheIntervalProcessor()
    {
        _subject = new IntervalProcessor();
    }

    private static IEnumerable<TestCaseData> ProcessingWithGapData
    {
        get
        {
            yield return new TestCaseData(
                new List<Tick>
                {
                    new(0, 0, 0),
                    new(0, 5, 0),
                    new(1, 6, 1),
                    new(10, 0, 0),
                },
                3,
                new List<int> { 5000, 61001, 999 + 53 * 1000 + 8 * 1000 * 60 }
            );
            yield return new TestCaseData(
                new List<Tick>
                {
                    new(0, 0, 0),
                    new(11, 11, 11)
                },
                1,
                new List<int> { 11 * (1000 * 60 + 1000 + 1) }
            );
            yield return new TestCaseData(
                new List<Tick>
                {
                    new(0, 2, 0),
                    new(0, 3, 0)
                },
                2,
                new List<int> { 2000, 1000 }
            );
            yield return new TestCaseData(
                new List<Tick>
                {
                    new(0, 0, 0),
                    new(0, 5, 0),
                    new(0, 5, 0),
                    new(0, 10, 0)
                },
                2,
                new List<int> { 5000, 5000 }
            );
            yield return new TestCaseData(
                new List<Tick>
                {
                    new(0, 0, 0),
                    new(0, 0, 499),
                    new(0, 0, 500),
                    new(0, 1, 0)
                },
                3,
                new List<int> { 499, 1, 500 }
            );
        }
    }

    [Test, TestCaseSource(nameof(ProcessingWithGapData))]
    public void ProcessIntervalsAndFillThemIfThereIsAnyGap(List<Tick> ticks, int numberOfIntervals, List<int> lengths)
    {
        var intervals = new List<Interval>();
        for (var i = 0; i < ticks.Count; i += 2)
        {
            intervals.Add(new Interval(ticks[i], ticks[i + 1]));
        }

        var filledIntervals = _subject.ProcessIntervals(intervals);
        Assert.AreEqual(numberOfIntervals, filledIntervals.Count);
        for (var i = 0; i < filledIntervals.Count; ++i)
        {
            Assert.AreEqual(lengths[i], filledIntervals[i].LengthMilisseconds);
        }
    }

    [Test]
    public void ThrowsOverlappingIntervalsExceptionWhenTwoIntervalsOverlap()
    {
        Assert.Throws<OverlappingIntervalsException>(TestGapFillingWithOverlappingIntervals);
    }

    private void TestGapFillingWithOverlappingIntervals()
    {
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(10, 0, 0);
        var interval1 = new Interval(tickA, tickB);
        var interval2 = new Interval(tickA, tickB);
        var intervals = new List<Interval> { interval1, interval2 };
        _subject.ProcessIntervals(intervals);
    }
}