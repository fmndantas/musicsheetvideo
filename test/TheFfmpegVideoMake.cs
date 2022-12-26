using System.Collections.Generic;
using System.Linq;
using Moq;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test;

[TestFixture]
public class TheFfmpegVideoMake
{
    private readonly FfmpegVideoMaker _subject;
    private readonly Mock<IProgressNotification> _mockProgressNotification;

    public TheFfmpegVideoMake()
    {
        var commands = new List<ICommand>();
        _mockProgressNotification = new Mock<IProgressNotification>();
        _subject = new FfmpegVideoMaker(commands, _mockProgressNotification.Object);
    }

    [Test]
    public void NotifiesFramesTranslationIntoTheFfmpegFiles()
    {
        var configuration = new MusicSheetVideoConfiguration("", "", "", "", "", "");
        var oneFrame = new Frame(new Interval(new Tick(0, 0, 0), new Tick(0, 0, 0)), 0);
        var frames = new List<Frame> { oneFrame, oneFrame, oneFrame };
        _subject.MakeVideo(frames, configuration);
        _mockProgressNotification.Verify(x => x.NotifyProgress(It.IsAny<string>()), Times.AtLeast(frames.Count));
    }
}