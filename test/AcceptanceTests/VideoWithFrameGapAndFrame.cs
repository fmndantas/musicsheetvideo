using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using musicsheetvideo.Frame;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests;

public class VideoWithFrameGapAndFrame : AcceptanceTestsBase
{
    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "-1.png", "1.png", "2.png" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 3;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(10, lines.Length);
        Assert.AreEqual($"file {_configuration.ImagesPath}/-1.png", lines[0]);
        Assert.AreEqual($"duration 2.000", lines[1]);
        Assert.AreEqual($"file {_configuration.ImagesPath}/1.png", lines[2]);
        Assert.AreEqual($"duration 1.000", lines[3]);
        Assert.AreEqual($"file {_configuration.ImagesPath}/-1.png", lines[4]);
        Assert.AreEqual($"duration 1.000", lines[5]);
        Assert.AreEqual($"file {_configuration.ImagesPath}/2.png", lines[6]);
        Assert.AreEqual($"duration 1.000", lines[7]);
        Assert.AreEqual($"file {_configuration.ImagesPath}/2.png", lines[8]);
    }

    [Test]
    public void Entrypoint()
    {
        var basePath = "/home/fernando/tmp/msv/frame-gap-frame";
        var configuration = new MusicSheetVideoConfiguration(
            basePath, Path.Combine(basePath, "frame-gap-frame.pdf"),
            Path.Combine(basePath, "audio.wav"),
            Path.Combine(basePath, "images/-1.png")
        );
        var frames = new List<Frame>
        {
            new(new(
                    new Tick(0, 2, 0),
                    new Tick(0, 3, 0)
                ),
                1),
            new(new(
                    new Tick(0, 4, 0),
                    new Tick(0, 5, 0)
                ),
                2),
        };
        StartTest(
            configuration,
            new FrameProcessor(new IntervalProcessor()),
            new FfmpegVideoProducer(configuration),
            frames
        );
    }
}