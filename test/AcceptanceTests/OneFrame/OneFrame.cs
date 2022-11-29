using System.Collections.Generic;
using musicsheetvideo;
using musicsheetvideo.Frame;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;
using Path = System.IO.Path;

namespace test.AcceptanceTests.OneFrame;

public class OneFrame : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "OneFrame/Data");
        var configuration = new MusicSheetVideoConfiguration(
            here, Path.Combine(here, "pdf.pdf"),
            Path.Combine(here, "audio.wav"),
            DefaultImagePath,
            "page",
            "jpg"

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
            new ImagemagickPdfConverter(configuration),
            new FrameProcessor(new IntervalProcessor()),
            new FfmpegVideoMaker(configuration),
            frames
        );
    }

    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "page-0.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 1;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "page-0.jpg")}", lines[0]);
        Assert.AreEqual("duration 10.000", lines[1]);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "page-0.jpg")}", lines[2]);
    }
}