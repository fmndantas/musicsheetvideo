using System.Collections.Generic;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.CustomException;
using NUnit.Framework;
using test.Stubs;

namespace test;

[TestFixture]
public class TheShellCommands
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private ShellCommand? _command;

    public TheShellCommands()
    {
        _configuration = new MusicSheetVideoConfiguration(
            "/home/fernando/tmp/msv/two-pages",
            string.Empty,
            string.Empty,
            string.Empty,
            "page",
            "jpg"
        );
    }

    [Test]
    public void FfmpegSlideshowCommandHasContent()
    {
        _command = new FfmpegSlideshowCommand(_configuration, new NullProgressNotification());
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
    public void FfmpegLengthCommandHasContent()
    {
        _command = new FfprobeVideoLengthCommand(_configuration, new NullProgressNotification());
        Assert.AreEqual($"ffprobe -v error -show_entries format=duration " +
                        $"-of default=noprint_wrappers=1:nokey=1 {_configuration.VideoPath}",
            _command.Command);
    }

    [Test]
    public void ImagemagickCommandHasContent()
    {
        _command = new ImagemagickPdfConversionCommand(_configuration, new NullProgressNotification());
        Assert.AreEqual($"convert -density 300 {_configuration.PdfPath} -quality 100 " +
                        $"{_configuration.ImagesPath}/{_configuration.ImagePrefix}-%d.{_configuration.ImageFormat}",
            _command.Command);
    }


    [Test]
    public void ThrowsShellCommandExecutionExceptionWhenIsInvalid()
    {
        Assert.Throws<ShellCommandExecutionException>(() => { new WrongCommand().Do(); });
    }
}