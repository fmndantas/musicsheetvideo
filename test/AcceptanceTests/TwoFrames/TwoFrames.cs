using System.Collections.Generic;
using System.IO;
using musicsheetvideo.Command;
using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegJoinAudioCommand;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;
using musicsheetvideo.Configuration.FromAudio;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests.TwoFrames;

[Category("two_frames")]
public class TwoFrames : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "TwoFrames/Data");
        var configuration = FromAudioConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithAudioPath(Path.Combine(here, "audio.wav"))
            .WithDefaultImagePath(DefaultImagePath);
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
        StartTest(
            configuration.Build(),
            new ImagemagickPdfConverter(
                new ImagemagickPdfConversionCommand(configuration.BuildImagemagickPdfConversionCommandInput(), Logger)),
            new FrameProcessor(new IntervalProcessor(), Logger),
            new FfmpegVideoMaker(
                new List<ICommand>
                {
                    new FfmpegSlideshowCommand(configuration.BuildFfmpegSlideshowCommandInput(), Logger),
                    new FfmpegJoinAudioCommand(configuration.BuildFfmpegJoinAudioCommandInput(), Logger)
                },
                Logger
            ),
            frames
        );
    }

    protected override IEnumerable<string> ImagesNamesConvertedFromPdf()
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
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesDirectoryPath, "page-1.jpg")}", lines[0]);
        Assert.AreEqual($"duration 5.000", lines[1]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesDirectoryPath, "page-0.jpg")}", lines[2]);
        Assert.AreEqual($"duration 5.000", lines[3]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesDirectoryPath, "page-0.jpg")}", lines[4]);
    }
}