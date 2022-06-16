using System.Collections.Generic;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.CustomException;
using NUnit.Framework;

namespace test;

public class ShellCommandTests
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private ShellCommand? _command;

    public ShellCommandTests()
    {
        _configuration = new MusicSheetVideoConfiguration(
            "/home/fernando/tmp/msv/two-pages",
            string.Empty,
            string.Empty,
            string.Empty
        );
    }

    [Test]
    public void TestSlideshowCommand()
    {
        _command = new FfmpegSlideshowCommand(_configuration);
        var target = new List<string>
        {
            "ffmpeg", "-y", "-f", "concat", "-safe",
            "0", "-i", _configuration.InputPath, "-vf", "\"crop=trunc(iw/2)*2:trunc(ih/2)*2\"",
            "-vsync", "vfr", "-pix_fmt", "yuv420p", "-hide_banner", "-loglevel", "error",
            _configuration.VideoPath
        };
        Assert.AreEqual(string.Join(" ", target), _command.Command);
    }

    [Test]
    public void TestLengthCommand()
    {
        _command = new FfprobeVideoLengthCommand(_configuration);
        Assert.AreEqual($"ffprobe -v error -show_entries format=duration " +
                        $"-of default=noprint_wrappers=1:nokey=1 {_configuration.VideoPath}",
            _command.Command);
    }

    [Test]
    public void TestProhibitedCases()
    {
        Assert.Throws<ShellCommandExecutionException>(TestStderrOutputWhenCommandDoesNotExists);
    }
    
    private void TestStderrOutputWhenCommandDoesNotExists()
    {
        new WrongCommand().Do();
    }
}