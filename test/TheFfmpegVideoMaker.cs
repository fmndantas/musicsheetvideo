using System.Collections.Generic;
using Moq;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.Configuration.FromAudio;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test;

[TestFixture]
public class TheFfmpegVideoMaker
{
    private readonly FfmpegVideoMaker _subject;
    private readonly Mock<IProgressNotification> _mockProgressNotification;

    public TheFfmpegVideoMaker()
    {
        var commands = new List<ICommand>();
        _mockProgressNotification = new Mock<IProgressNotification>();
        _subject = new FfmpegVideoMaker(commands, _mockProgressNotification.Object);
    }

    [Test]
    public void NotifiesFramesTranslationIntoTheFfmpegInputFile()
    {
        var configuration = FromAudioConfigurationBuilder.OneConfiguration().Build();
        var oneFrame = new Frame(new Interval(new Tick(0, 0, 0), new Tick(0, 0, 0)), 0);
        var frames = new List<Frame> { oneFrame, oneFrame, oneFrame };
        _subject.MakeVideo(frames, configuration);
        _mockProgressNotification.Verify(x => x.NotifyProgress(It.IsAny<string>()), Times.AtLeast(frames.Count));
    }
}