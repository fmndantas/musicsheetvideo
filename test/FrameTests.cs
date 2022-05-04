using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class FrameTests
{
    [Test]
    public void TestGapNextFrame()
    {
        var tickA = new Tick(0, 0, 0);
        var tickB = new Tick(0, 5, 0);
        var tickC = new Tick(0, 10, 0);
        var interval1 = new Interval(tickA, tickB);
        var interval2 = new Interval(tickB, tickC);
        var frameA = new Frame(interval1, 2);
        var frameB = new Frame(interval2, 1);
        var gap = frameA.Gap(frameB);
        Assert.AreEqual(0, gap.LengthMilisseconds);
    }
}