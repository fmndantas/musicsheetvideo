using System.Collections.Generic;
using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegJoinAudioCommand;
using musicsheetvideo.Command.FfmpegOverlayVideosCommand;
using musicsheetvideo.Command.FfmpegRescaleVideoCommand;
using musicsheetvideo.Command.FfprobeVideoLengthCommand;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;
using musicsheetvideo.CustomException;
using NUnit.Framework;
using test.Stubs;
using test.Utils;

namespace test;

[TestFixture]
public class TheShellCommands
{
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void TheFfmpegSlideshowCommandHasContent(int fractionOfTotalHeight)
    {
        var input = new FfmpegSlideshowCommandInput(Generator.RandomString(), Generator.RandomString());
        var command = new FfmpegSlideshowCommand(input, new NullProgressNotification(), fractionOfTotalHeight);
        var desiredHeigth = 1080 / fractionOfTotalHeight;
        var target = new List<string>
        {
            "ffmpeg", "-y", "-f", "concat", "-safe",
            "0", "-i", input.TextInputPath, "-vf",
            $"\"scale=1920:{desiredHeigth}:force_original_aspect_ratio=decrease,pad=1920:{desiredHeigth}:(ow-iw)/2:(oh-ih)/2,setsar=1\"",
            "-vsync", "vfr", "-pix_fmt", "yuv420p", "-hide_banner", "-loglevel", "error",
            input.OutputVideoPath
        };
        Assert.That(command.Command, Is.EqualTo(string.Join(" ", target)));
    }

    [Test]
    public void TheFfmpegLengthCommandHasContent()
    {
        var input = new FfprobeVideoLengthCommandInput(Generator.RandomString());
        var command = new FfprobeVideoLengthCommand(input, new NullProgressNotification());
        Assert.That(command.Command, Is.EqualTo("ffprobe -v error -show_entries format=duration " +
                                                $"-of default=noprint_wrappers=1:nokey=1 {input.InputVideoPath}"));
    }

    [Test]
    public void TheImagemagickCommandHasContent()
    {
        var input = new ImagemagickPdfConversionCommandInput(Generator.RandomString(), Generator.RandomString(),
            Generator.RandomString(), Generator.RandomString());
        var command = new ImagemagickPdfConversionCommand(input, new NullProgressNotification());
        Assert.That(command.Command, Is.EqualTo($"convert -density 300 {input.PdfPath} " +
                                                $"-quality 100 {input.ImagesPath}/{input.ImagePrefix}-%d.{input.ImageFormat}"));
    }

    [Test]
    public void TheFfmpegJoinAudioCommandHasContent()
    {
        var input = new FfmpegJoinAudioCommandInput(Generator.RandomString(), Generator.RandomString(),
            Generator.RandomString());
        var command = new FfmpegJoinAudioCommand(input, new NullProgressNotification());
        Assert.That(command.Command, Is.EqualTo("ffmpeg -hide_banner -loglevel error -y " +
                                                $"-i {input.InputVideoPath} -i {input.AudioPath} " +
                                                $"-shortest {input.OutputVideoPath}"));
    }

    [Test]
    public void TheFfmpegRescaleVideoCommandHasContent()
    {
        var input = new FfmpegRescaleVideoCommandInput(Generator.RandomString(), Generator.RandomString());
        var command = new FfmpegRescaleVideoCommand(input, new NullProgressNotification());
        Assert.That(command.Command, Is.EqualTo($"ffmpeg -y -i {input.InputVideoPath} " +
                                                "-vf scale=1920:1080,setsar=1:1 -hide_banner -loglevel error " +
                                                $"{input.OutputVideoPath}"));
    }

    [Test]
    public void TheFfmpegOverlayVideosCommandHasContent()
    {
        var input = new FfmpegOverlayVideosCommandInput(
            videoBelowPath: Generator.RandomString(),
            videoAbovePath: Generator.RandomString(),
            outputVideoPath: Generator.RandomString()
        );
        var command = new FfmpegOverlayVideosCommand(input, new NullProgressNotification());
        Assert.That(command.Command, Is.EqualTo($"ffmpeg -y -i {input.VideoBelowPath} -i {input.VideoAbovePath} " +
                                                "-filter_complex [1:v]format=argb,colorchannelmixer=aa=0.7[t];[0:v][t]overlay=0:main_h-overlay_h " +
                                                $"-hide_banner -loglevel error {input.OutputVideoPath}"));
    }

    [Test]
    public void ThrowsShellCommandExecutionExceptionWhenIsInvalid()
    {
        Assert.Throws<ShellCommandExecutionException>(() => { new WrongCommand().Do(); });
    }
}