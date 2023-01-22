using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests.OneFrame;

[Category("one_frame")]
public class OneFrame : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "OneFrame/Data");
        var configuration = MusicSheetConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithAudioPath(Path.Combine(here, "audio.wav"))
            .WithDefaultImagePath(DefaultImagePath)
            .Build();
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
        return new List<string> { "page-0.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 1;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesPath, "page-0.jpg")}", lines[0]);
        Assert.AreEqual("duration 10.000", lines[1]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesPath, "page-0.jpg")}", lines[2]);
    }
}