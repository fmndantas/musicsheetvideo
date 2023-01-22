using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests.OneFrameOneGapOneFrame;

public class OneFrameOneGapOneFrame : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "OneFrameOneGapOneFrame/Data");
        var configuration = MusicSheetConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithAudioPath(Path.Combine(here, "audio.wav"))
            .WithDefaultImagePath(DefaultImagePath)
            .Build();
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
            new ImagemagickPdfConverter(new ImagemagickPdfConversionCommand(configuration, Logger)),
            new FrameProcessor(new IntervalProcessor(), Logger),
            new FfmpegVideoMaker(
                new List<ICommand>
                {
                    new FfmpegSlideshowCommand(configuration, Logger), new FfmpegJoinAudioCommand(configuration, Logger)
                },
                Logger
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
        Assert.AreEqual(10, lines.Length);
        Assert.AreEqual($"file {Configuration.DefaultImage}", lines[0]);
        Assert.AreEqual($"duration 2.000", lines[1]);
        Assert.AreEqual($"file {Configuration.ImagesPath}/page-0.jpg", lines[2]);
        Assert.AreEqual($"duration 1.000", lines[3]);
        Assert.AreEqual($"file {Configuration.DefaultImage}", lines[4]);
        Assert.AreEqual($"duration 1.000", lines[5]);
        Assert.AreEqual($"file {Configuration.ImagesPath}/page-1.jpg", lines[6]);
        Assert.AreEqual($"duration 1.000", lines[7]);
        Assert.AreEqual($"file {Configuration.ImagesPath}/page-1.jpg", lines[8]);
    }
}