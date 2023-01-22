using System.Collections.Generic;
using System.IO;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;
using musicsheetvideo.Configuration.FromVideo;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Pipeline;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests.BottomOverlay;

[Category("bottom_overlay")]
public class BottomOverlay : AcceptanceTestsBase
{
    [Test]
    public void Entrypoint()
    {
        var here = Path.Combine(BasePath, "BottomOverlay/Data");
        var configuration = FromVideoConfigurationBuilder.OneConfiguration()
            .WithOutputPath(here)
            .WithPdfPath(Path.Combine(here, "pdf.pdf"))
            .WithInputVideoPath(Path.Combine(here, "input.mp4"))
            .WithDefaultImagePath(DefaultImagePath)
            .WithImagePrefix("fragment")
            .WithImageFormat("jpg");
        var pipeline = new FfmpegFromVideoPipelineMaker(configuration, Logger).MakePipeline();
        var frames = new List<Frame>
        {
            new(new Interval(new(0, 0, 0), new Tick(0, 3, 651)), 1),
            new(new Interval(new(0, 3, 651), new Tick(0, 9, 204)), 2),
            new(new Interval(new(0, 9, 204), new(0, 12, 902)), 3),
            new(new Interval(new(0, 12, 902), new(0, 18, 411)), 4),
            new(new Interval(new(0, 18, 411), new(0, 22, 132)), 5),
            new(new Interval(new(0, 22, 132), new(0, 27, 664)), 6),
            new(new Interval(new(0, 27, 664), new(0, 31, 350)), 7),
            new(new Interval(new(0, 31, 350), new(0, 35, 52)), 8),
            new(new Interval(new(0, 35, 52), new(0, 38, 703)), 9),
            new(new Interval(new(0, 38, 703), new(0, 42, 384)), 10),
            new(new Interval(new(0, 42, 384), new(0, 45, 0)), 11),
        };
        StartTest(
            configuration.Build(),
            new ImagemagickPdfConverter(
                new ImagemagickPdfConversionCommand(configuration.BuildImagemagickPdfConversionCommandInput(), Logger)),
            new FrameProcessor(new IntervalProcessor(), Logger),
            new FfmpegVideoMaker(pipeline, Logger),
            frames
        );
    }

    protected override IEnumerable<string> ImagesNamesConvertedFromPdf()
    {
        var result = new List<string>();
        for (var i = 0; i < 11; ++i)
        {
            result.Add($"{Configuration.ImagePrefix}-{i}.{Configuration.ImageFormat}");
        }

        return result;
    }

    protected override int NumberOfExpectedImages()
    {
        return 11;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
    }
}