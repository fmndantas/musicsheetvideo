using System;
using System.Collections.Generic;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class TickTests
{
    [Test]
    public void TestDurationInMilisseconds()
    {
        var tick = new Tick(10, 10, 10);
        Assert.AreEqual(10 * 60 * 1000 + 10 * 1000 + 10, tick.DurationMilissecondsToZero);
    }

    private static IEnumerable<TestCaseData> OneMillisecondDecreasedTicks
    {
        get
        {
            yield return new TestCaseData(
                new Tick(1, 0, 0),
                new Tick(0, 59, 999)
            );
            yield return new TestCaseData(
                new Tick(1,0,1),
                new Tick(1, 0,0)
            );
            yield return new TestCaseData(
                new Tick(0, 0, 999),
                new Tick(0, 0, 998)
            );
            yield return new TestCaseData(
                new Tick(100,0,0),
                new Tick(99,59,999)
            );
            yield return new TestCaseData(
                new Tick(0,0,0),
                new Tick(0,0,0)
            );
            yield return new TestCaseData(
                new Tick(5,0,0),
                new Tick(4,59,999)
            );
            yield return new TestCaseData(
                new Tick(1, 6, 1),
                new Tick(1, 6, 0)
            );
        }
    }

    [Test, TestCaseSource("OneMillisecondDecreasedTicks")]
    public void TestDecreaseOneMilissecond(Tick tested, Tick target)
    {
        AssertTickEqualTo(tested.DecreaseOneMilissecond(), target);
    }
    
    private void AssertTickEqualTo(Tick toBeTested, Tick target)
    {
        Assert.True(toBeTested.CompareTo(target) == 0);
    }
    
    private static IEnumerable<TestCaseData> DurationMilissecondsData
    {
        get
        {
            yield return new TestCaseData(
                new Tick(0, 5, 0),
                new Tick(0, 5, 0),
                0
            );
            yield return new TestCaseData(
                new Tick(0,0,0),
                new Tick(0,10,0),
                10000
            );
            yield return new TestCaseData(
                new Tick(0, 5, 0),
                new Tick(1, 6, 1).DecreaseOneMilissecond(),
                61000
            );
        }
    }

    [Test, TestCaseSource("DurationMilissecondsData")]
    public void TestDecreaseOneMilissecond(Tick tick, Tick otherTick, long target)
    {
        AssertTickDurationEqualTo(tick.DeltaMilisseconds(otherTick), target);
    }
    
    private void AssertTickDurationEqualTo(long tested, long target)
    {
        Assert.AreEqual(target, tested);
    }
}