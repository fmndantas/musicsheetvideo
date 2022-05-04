using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class PageTests
{
    [Test]
    public void TestGapNextPage()
    {
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(0, 5, 0);
        var tickC = new Tick(0, 10, 0);
        var interval1 = new Interval(tickA, tickB);
        var interval2 = new Interval(tickB, tickC);
        var durationA = new Page(interval1, 2);
        var durationB = new Page(interval2, 1);
        var gap = durationA.Gap(durationB);
        Assert.AreEqual(0, gap.LengthMilisseconds);
    }
}