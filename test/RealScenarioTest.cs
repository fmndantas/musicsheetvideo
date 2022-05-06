using System.Collections.Generic;
using System.IO;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class RealScenarioTest
{
    [Test]
    public void Entrypoint()
    {
        var basePath = "/home/fernando/tmp/msv/contra-babilonia";
        var configuration = new MusicSheetVideoConfiguration(
            basePath, Path.Combine(basePath, "Contra-Babilonia.pdf"),
            Path.Combine(basePath, "extrair-audio.wav")
        );
        var frames = new List<Frame>
        {
            new(new(
                    new Tick(0, 0, 0),
                    new Tick(0, 10, 500)),
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
        var app = new MusicSheetVideo(configuration,
            new IntervalProcesser(),
            new FfmpegVideoProducer(configuration));
        app.MakeVideo(frames);
    }
}