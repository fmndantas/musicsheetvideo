using System.Collections.Generic;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.CustomException;
using NUnit.Framework;

namespace test;

public class TheShellCommand
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private ShellCommand? _command;

    public TheShellCommand()
    {
        _configuration = new MusicSheetVideoConfiguration(
            "/home/fernando/tmp/msv/two-pages",
            string.Empty,
            string.Empty,
            string.Empty
        );
    }

    [Test]
    public void SlideshowCommandHasFollowingContent()
    {
        _command = new FfmpegSlideshowCommand(_configuration);
        var target = new List<string>
        {
            "ffmpeg", "-y", "-f", "concat", "-safe",
            "0", "-i", _configuration.InputPath, "-vf", 
            "\"scale=1920:1080:force_original_aspect_ratio=decrease,pad=1920:1080:(ow-iw)/2:(oh-ih)/2,setsar=1\"",
            "-vsync", "vfr", "-pix_fmt", "yuv420p", "-hide_banner", "-loglevel", "error",
            _configuration.VideoPath
        };
        Assert.AreEqual(string.Join(" ", target), _command.Command);
    }

    [Test]
    public void LengthCommandHasFollowingContent()
    {
        _command = new FfprobeVideoLengthCommand(_configuration);
        Assert.AreEqual($"ffprobe -v error -show_entries format=duration " +
                        $"-of default=noprint_wrappers=1:nokey=1 {_configuration.VideoPath}",
            _command.Command);
    }

    [Test]
    public void ThrowsShellCommandExecutionExceptionWhenIsInvalid()
    {
        Assert.Throws<ShellCommandExecutionException>(() => { new WrongCommand().Do(); } );
    }
}
