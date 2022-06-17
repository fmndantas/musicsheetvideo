using System.Collections.Generic;
using musicsheetvideo.CustomException;
using musicsheetvideo.Timestamp;
using NUnit.Framework;

namespace test;

public class TheInterval
{
    public static IEnumerable<TestCaseData> OverlappingIntervalsData
    {
        get
        {
            yield return new TestCaseData(
                new Tick(0, 0, 1),
                new Tick(0, 0, 2),
                new Tick(0, 0, 1),
                new Tick(0, 0, 2),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 1),
                new Tick(0, 0, 2),
                new Tick(0, 0, 2),
                new Tick(0, 0, 3),
                false
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10, 0, 0),
                new Tick(0, 1, 0),
                new Tick(0, 2, 0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10, 0, 0),
                new Tick(0, 0, 0),
                new Tick(0, 2, 0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10, 0, 0),
                new Tick(9, 59, 999),
                new Tick(10, 0, 0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(0, 0, 999),
                new Tick(1, 0, 0),
                new Tick(1, 59, 999),
                false
            );
        }
    }

    [Test, TestCaseSource("OverlappingIntervalsData")]
    public void InformsWhenOverlapsWithAnotherInterval(Tick tick1, Tick tick2, Tick tick3, Tick tick4, bool target)
    {
        var interval1 = new Interval(tick1, tick2);
        var interval2 = new Interval(tick3, tick4);
        Assert.AreEqual(target, interval1.Overlaps(interval2));
    }

    [Test]
    public void InformsItsTotalLenghtInSeconds()
    {
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(0, 10, 0);
        var interval = new Interval(tickA, tickB);
        Assert.AreEqual(10, interval.EndInSeconds);
    }

    [TestCase(0, 0, 1, 0, 0, 0, true)]
    [TestCase(0, 0, 0, 0, 0, 1, false)]
    [TestCase(0, 0, 0, 0, 0, 0, false)]
    [TestCase(0, 0, 1, 0, 0, 1, false)]
    [TestCase(0, 1, 0, 0, 0, 999, true)]
    public void ThrowsErrorOnInitializationWhenStartTickIsSetAfterEndTick(
        int sm, int ss, int sms, int em, int es, int ems, bool shouldThrow
    )
    {
        var start = new Tick(sm, ss, sms);
        var end = new Tick(em, es, ems);
        if (shouldThrow)
        {
            Assert.Throws<InvertedIntervalException>(() =>
            {
                var _ = new Interval(start, end);
            });
        }
        else
        {
            Assert.DoesNotThrow(() =>
            {
                var _ = new Interval(start, end);
            });
        }
    }
}