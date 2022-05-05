using System.Collections.Generic;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class IntervalTest
{
    public static IEnumerable<TestCaseData> OverlappingTestCases
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
                new Tick(0,0,2),
                new Tick(0,0,2),
                new Tick(0,0,3),
                false
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10,0,0),
                new Tick(0,1,0),
                new Tick(0,2,0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10,0,0),
                new Tick(0,0,0),
                new Tick(0,2,0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(10,0,0),
                new Tick(9, 59,999),
                new Tick(10,0,0),
                true
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(0,0,999),
                new Tick(1, 0,0),
                new Tick(1,59,999),
                false
            );

        }
    }

    [Test, TestCaseSource("OverlappingTestCases")]
    public void TestOverlapping(Tick tick1, Tick tick2, Tick tick3, Tick tick4, bool target)
    {
        var interval1 = new Interval(tick1, tick2);
        var interval2 = new Interval(tick3, tick4);
        Assert.AreEqual(target, interval1.Overlaps(interval2));
    }

    [Test]
    public void TestEndInSeconds()
    {
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(0, 10, 0);
        var interval = new Interval(tickA, tickB);
        Assert.AreEqual(10, interval.EndInSeconds);
    }
}