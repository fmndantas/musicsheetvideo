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

namespace test.AcceptanceTests.AComplexCase;

[Category("complex_case")]
public class ComplexCase : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "AComplexCase/Data");
        var configuration = FromAudioConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithAudioPath(Path.Combine(here, "audio.wav"))
            .WithDefaultImagePath(DefaultImagePath);
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
        return new List<string> { "page-0.jpg", "page-1.jpg", "page-2.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 3;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual("duration 10.000", lines[1]);
        Assert.AreEqual($"file {Configuration.DefaultImagePath}", lines[2]);
        Assert.AreEqual("duration 0.500", lines[3]);
        Assert.AreEqual($"file {Path.Join(Configuration.ImagesDirectoryPath, "page-1.jpg")}", lines[4]);
        Assert.AreEqual("duration 21.503", lines[5]);
        Assert.AreEqual($"file {Configuration.DefaultImagePath}", lines[6]);
        Assert.AreEqual("duration 0.001", lines[7]);
        Assert.AreEqual($"file {Path.Join(Configuration.ImagesDirectoryPath, "page-2.jpg")}", lines[8]);
        Assert.AreEqual("duration 7.996", lines[9]);
        Assert.AreEqual($"file {Path.Join(Configuration.ImagesDirectoryPath, "page-2.jpg")}", lines[10]);
    }
}