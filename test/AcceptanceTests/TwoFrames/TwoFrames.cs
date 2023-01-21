using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;
using test.Stubs;

namespace test.AcceptanceTests.TwoFrames;

[TestFixture]
public class TwoFrames : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "TwoFrames/Data");
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
                    new Tick(0, 5, 0)
                ),
                2),
            new(new Interval(
                    new Tick(0, 5, 0),
                    new Tick(0, 10, 0)),
                1)
        };
        var logger = new NullProgressNotification();
        StartTest(
            configuration,
            new ImagemagickPdfConverter(new ImagemagickPdfConversionCommand(configuration, logger), logger),
            new FrameProcessor(new IntervalProcessor(), logger),
            new FfmpegVideoMaker(
                new List<ICommand>
                    { new FfmpegSlideshowCommand(configuration, logger), new FfmpegJoinAudioCommand(configuration, logger) },
                logger
            ),
            frames
        );
    }

    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "page-0.jpg", "page-1.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 2;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(6, lines.Length);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesPath, "page-1.jpg")}", lines[0]);
        Assert.AreEqual($"duration 5.000", lines[1]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesPath, "page-0.jpg")}", lines[2]);
        Assert.AreEqual($"duration 5.000", lines[3]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesPath, "page-0.jpg")}", lines[4]);
    }
}