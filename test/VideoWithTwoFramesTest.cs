using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class VideoWithTwoFramesTest : AcceptanceTestsBase
{
    protected override IEnumerable<string> FileNames()
    {
        return new List<string> { "1.png", "2.png" };
    }

    protected override int NumberOfExpectedImages()
    {
        return 2;
    }

    protected override void AnalyseInputFile(string[] lines)
    {
        Assert.AreEqual(6, lines.Length);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "2.png")}", lines[0]);
        Assert.AreEqual($"duration 4.999", lines[1]);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "1.png")}", lines[2]);
        Assert.AreEqual($"duration 4.999", lines[3]);
        Assert.AreEqual($"file {Path.Combine(_configuration.ImagesPath, "1.png")}", lines[4]);
    }

    [Test]
    public async Task Entrypoint()
    {
        var basePath = "/home/fernando/tmp/msv/two-pages";
        var configuration = new MusicSheetVideoConfiguration(
            basePath, Path.Combine(basePath, "two-pages.pdf"), Path.Combine(basePath, "two-pages.wav")
        );
        var pages = new List<Page>
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
        await StartTest(
            configuration,
            new GapFiller(),
            new FfmpegProducer(configuration),
            pages
        );
    }
}