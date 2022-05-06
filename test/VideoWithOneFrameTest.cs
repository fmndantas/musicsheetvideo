using System.Collections.Generic;
using musicsheetvideo;
using NUnit.Framework;
using Path = System.IO.Path;

namespace test;

public class VideoWithOneFrameTest : AcceptanceTestsBase
{
    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "1.png" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 1;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "1.png")}", lines[0]);
        Assert.AreEqual("duration 10", lines[1]);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "1.png")}", lines[2]);
    }

    [Test]
    public void Entrypoint()
    {
        var basePath = "/home/fernando/tmp/msv/one-page";
        var configuration = new MusicSheetVideoConfiguration(
            basePath, Path.Combine(basePath, "one-page.pdf"),
            Path.Combine(basePath, "audio.wav")
        );
        var frames = new List<Frame>
        {
            new(new(
                    new Tick(0, 0, 0),
                    new Tick(0, 10, 0)
                ),
                1),
        };
        StartTest(
            configuration,
            new IntervalProcesser(),
            new FfmpegVideoProducer(configuration),
            frames
        );
    }
}