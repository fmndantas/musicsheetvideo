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
        _configuration = MusicSheetConfigurationBuilder.OneConfiguration()
            .WithOutputPath("/home/fernando/tmp/msv/two-pages")
            .Build();
    }

    [Test]
    public void TheFfmpegSlideshowCommandHasContent()
    {
        _command = new FfmpegSlideshowCommand(_configuration, new NullProgressNotification());
        var target = new List<string>
        {
            "ffmpeg", "-y", "-f", "concat", "-safe",
            "0", "-i", _configuration.SlideshowTextInputPath, "-vf",
            "\"scale=1920:1080:force_original_aspect_ratio=decrease,pad=1920:1080:(ow-iw)/2:(oh-ih)/2,setsar=1\"",
            "-vsync", "vfr", "-pix_fmt", "yuv420p", "-hide_banner", "-loglevel", "error",
            _configuration.SlideshowVideoPath
        };
        Assert.That(_command.Command, Is.EqualTo(string.Join(" ", target)));
    }

    [Test]
    public void TheFfmpegLengthCommandHasContent()
    {
        _command = new FfprobeVideoLengthCommand(_configuration, new NullProgressNotification());
        Assert.That(_command.Command, Is.EqualTo("ffprobe -v error -show_entries format=duration " +
                                                 $"-of default=noprint_wrappers=1:nokey=1 {_configuration.SlideshowVideoPath}"));
    }

    [Test]
    public void TheImagemagickCommandHasContent()
    {
        _command = new ImagemagickPdfConversionCommand(_configuration, new NullProgressNotification());
        Assert.That(_command.Command, Is.EqualTo($"convert -density 300 {_configuration.PdfPath} " +
                                                 $"-quality 100 {_configuration.ImagesPath}/{_configuration.ImagePrefix}-%d.{_configuration.ImageFormat}"));
    }

    [Test]
    public void TheFfmpegJoinAudioCommandHasContent()
    {
        _command = new FfmpegJoinAudioCommand(_configuration, new NullProgressNotification());
        Assert.That(_command.Command, Is.EqualTo("ffmpeg -hide_banner -loglevel error -y " +
                                                 $"-i {_configuration.SlideshowVideoPath} -i {_configuration.AudioPath} " +
                                                 $"-shortest {_configuration.OutputVideoPath}"));
    }

    [Test]
    public void ThrowsShellCommandExecutionExceptionWhenIsInvalid()
    {
        Assert.Throws<ShellCommandExecutionException>(() => { new WrongCommand().Do(); });
    }
}