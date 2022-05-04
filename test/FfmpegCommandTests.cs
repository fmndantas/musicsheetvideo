using System.Collections.Generic;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public class FfmpegCommandTests
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly FfmpegSlideshowCommand _command;

    public FfmpegCommandTests()
    {
        _configuration = new MusicSheetVideoConfiguration(
            "/home/fernando/tmp/msv/two-pages", string.Empty, string.Empty
        );
        _command = new FfmpegSlideshowCommand(_configuration);
    }

    [Test]
    public void TestSlideshowCommand()
    {
        var target = new List<string>
        {
            "ffmpeg", "-f", "concat", "-safe",
            "0", "-i", _configuration.InputPath, "-vf", "\"crop=trunc(iw/2)*2:trunc(ih/2)*2\"",
            "-vsync", "vfr", "-pix_fmt", "yuv420p", _configuration.VideoPath
        };
        AssertFragmentsAreEqualTo($"{_command.Command} {_command.Arguments}", target);
    }

    private void AssertFragmentsAreEqualTo(string command, List<string> target)
    {
        Assert.AreEqual(string.Join(" ", target), command);
    }
}