using System.Collections.Generic;
using musicsheetvideo.Timestamp;
using NUnit.Framework;

namespace test;

[TestFixture]
public class TheTick
{
    [Test]
    public void HaveDurationInMilissecondsBasedOnItsTimestampAttributes()
    {
        var tick = new Tick(10, 10, 10);
        Assert.AreEqual(10 * 60 * 1000 + 10 * 1000 + 10, tick.DurationMilissecondsToZero);
    }

    private static IEnumerable<TestCaseData> IntervalDurationInMilissecondsData
    {
        get
        {
            yield return new TestCaseData(
                new Tick(0, 5, 0),
                new Tick(0, 5, 0),
                0
            );
            yield return new TestCaseData(
                new Tick(0, 0, 0),
                new Tick(0, 10, 0),
                10000
            );
            yield return new TestCaseData(
                new Tick(0, 5, 0),
                new Tick(1, 6, 1),
                61001
            );
        }
    }

    [Test, TestCaseSource(nameof(IntervalDurationInMilissecondsData))]
    public void InformsTheIntervalDurationInMilissecondsBetweenItselfAndAnotherTick(
        Tick tick, Tick otherTick, long target
    )
    {
        Assert.AreEqual(target, tick.DeltaMilisseconds(otherTick));
    }
}