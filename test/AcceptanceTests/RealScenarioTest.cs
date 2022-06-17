using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using musicsheetvideo.Frame;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests;

public class RealScenarioTest : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var basePath = "/home/fernando/tmp/msv/contra-babilonia";
        var configuration = new MusicSheetVideoConfiguration(
            basePath, Path.Combine(basePath, "three-pages.pdf"),
            Path.Combine(basePath, "extrair-audio.wav"),
            "/home/fernando/black.jpg",
            "page",
            "jpg"
        );
        var frames = new List<Frame>
        {
            new(new(
                    new Tick(0, 0, 0),
                    new Tick(0, 10, 0)),
                1),
            new(new Interval(
                    new Tick(0, 10, 500),
                    new Tick(0, 32, 3)),
                2),
            new(new Interval(
                new Tick(0, 32, 4),
                new Tick(0, 40, 0)
            ), 3)
        };
        StartTest(
            configuration,
            new ImagemagickPdfConverter(configuration),
            new FrameProcessor(new IntervalProcessor()),
            new FfmpegVideoProducer(configuration),
            frames
        );
    }

    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "page-0.jpg", "page-1.jpg", "page-2.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 3;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual("duration 10.000", lines[1]);
        Assert.AreEqual($"file {_configuration.DefaultImage}", lines[2]);
        Assert.AreEqual("duration 0.500", lines[3]);
        Assert.AreEqual($"file {Path.Join(_configuration.ImagesPath, "page-1.jpg")}", lines[4]);
        Assert.AreEqual("duration 21.503", lines[5]);
        Assert.AreEqual($"file {_configuration.DefaultImage}", lines[6]);
        Assert.AreEqual("duration 0.001", lines[7]);
        Assert.AreEqual($"file {Path.Join(_configuration.ImagesPath, "page-2.jpg")}", lines[8]);
        Assert.AreEqual("duration 7.996", lines[9]);
        Assert.AreEqual($"file {Path.Join(_configuration.ImagesPath, "page-2.jpg")}", lines[10]);
    }
}