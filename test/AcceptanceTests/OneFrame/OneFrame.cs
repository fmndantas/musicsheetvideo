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

namespace test.AcceptanceTests.OneFrame;

[Category("one_frame")]
public class OneFrame : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "OneFrame/Data");
        var configuration = FromAudioConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithAudioPath(Path.Combine(here, "audio.wav"))
            .WithDefaultImagePath(DefaultImagePath);
        var frames = new List<Frame>
        {
            new(new(new(0, 0, 0), new(0, 10, 0)), 1)
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
        return new List<string> { "page-0.jpg" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 1;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(4, lines.Length);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesDirectoryPath, "page-0.jpg")}", lines[0]);
        Assert.AreEqual("duration 10.000", lines[1]);
        Assert.AreEqual($"file {Path.Combine(Configuration.ImagesDirectoryPath, "page-0.jpg")}", lines[2]);
    }
}