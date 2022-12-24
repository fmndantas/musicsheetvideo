using System.Collections.Generic;
using System.Linq;
using Moq;
using musicsheetvideo;
using musicsheetvideo.Timestamp;
using NUnit.Framework;

namespace test;

[TestFixture]
public class TheFrameProcessor
{
    private FrameProcessor _subject;
    private Mock<IProgressNotification> _mockProgressNotification;

    public TheFrameProcessor()
    {
        _mockProgressNotification = new Mock<IProgressNotification>();
        _subject = new FrameProcessor(new IntervalProcessor(), _mockProgressNotification.Object);
    }

    [Test]
    public void NotifiesWhenFramesAreBeingProcessed()
    {
        var oneFrame = new Frame(new Interval(new Tick(0, 0, 0), new Tick(0, 0, 0)), 1);
        var frames = new List<Frame> { oneFrame, oneFrame, oneFrame };
        _subject.ProcessFrames(frames);
        _mockProgressNotification.Verify(x => x.NotifyProgress(It.IsAny<string>()), Times.AtLeast(frames.Count));
    }
}