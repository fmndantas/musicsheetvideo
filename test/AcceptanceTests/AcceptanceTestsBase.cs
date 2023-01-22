using System.Collections.Generic;
using System.IO;
using System.Linq;
using musicsheetvideo;
using musicsheetvideo.Command.FfprobeVideoLengthCommand;
using musicsheetvideo.Configuration;
using musicsheetvideo.Configuration.FromAudio;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;
using test.Stubs;

namespace test.AcceptanceTests;

[TestFixture]
[Category("integration")]
[Category("acceptance")]
public abstract class AcceptanceTestsBase
{
    private Frame _lastFrame;
    protected const string BasePath = "/home/fernando/Documents/github/musicsheetvideo/test/AcceptanceTests/";
    protected readonly string DefaultImagePath;
    protected readonly IProgressNotification Logger;
    protected IMusicSheetVideoConfiguration Configuration;

    protected AcceptanceTestsBase()
    {
        DefaultImagePath = Path.Combine(BasePath, "Data/default-image.jpg");
        Logger = new NunitProgressNotification();
        var zeroTick = new Tick(0, 0, 0);
        _lastFrame = new Frame(new Interval(zeroTick, zeroTick), -1);
        Configuration = FromAudioConfigurationBuilder.OneConfiguration().Build();
    }

    protected void StartTest(
        IMusicSheetVideoConfiguration configuration,
        IPdfConverter pdfConverter,
        IFrameProcessor frameProcessor,
        IVideoMaker videoMaker,
        List<Frame> frames
    )
    {
        Logger.NotifyProgress($"Running test case \"{GetType().Name}\"");
        frames.Sort();
        _lastFrame = frames.Last();
        Configuration = configuration;
        DeleteGeneratedFiles();
        var app = new MusicSheetVideo(pdfConverter, frameProcessor, videoMaker, new NullProgressNotification());
        app.MakeVideo(frames, Configuration);
        AssertImagesWereCreatedCorrectly();
        AssertFfmpegInputFileWasCreatedCorrectly();
        AssertSlideshowWasCorrectlyProduced();
        Logger.NotifyProgress("Done!\n");
    }

    private void DeleteGeneratedFiles()
    {
        if (File.Exists(Configuration.SlideshowInputPath))
        {
            File.Delete(Configuration.SlideshowInputPath);
        }

        if (Directory.Exists(Configuration.OutputPath))
        {
            foreach (var video in Directory.GetFiles(Configuration.OutputPath, "*.mp4", SearchOption.AllDirectories))
            {
                // pattern used in integration tests with video input
                if (!video.Contains("input.mp4"))
                {
                    File.Delete(video);
                }
            }
        }

        if (Directory.Exists(Configuration.ImagesDirectoryPath))
        {
            foreach (var image in Directory.GetFiles(Configuration.ImagesDirectoryPath, "*",
                         SearchOption.AllDirectories))
            {
                File.Delete(image);
            }
        }
    }

    private void AssertImagesWereCreatedCorrectly()
    {
        Assert.True(Directory.Exists(Configuration.ImagesDirectoryPath));
        var images = Directory.GetFiles(Configuration.ImagesDirectoryPath, "*", SearchOption.AllDirectories);
        Assert.AreEqual(NumberOfExpectedImages(), images.Length);
        var imagesNames = images.Select(x => x.Split("/").Last()).ToArray();
        foreach (var fileName in ImagesNamesConvertedFromPdf())
        {
            Assert.True(imagesNames.Contains(fileName), $"could not find image \"{fileName}\"");
        }
    }

    private void AssertFfmpegInputFileWasCreatedCorrectly()
    {
        var ffmpegInput = Path.Combine(Configuration.OutputPath, "input.txt");
        Assert.True(File.Exists(ffmpegInput));
        var content = string.Empty;
        try
        {
            using var sr = new StreamReader(ffmpegInput);
            content = sr.ReadToEnd();
        }
        catch (IOException ex)
        {
            Assert.Fail("Output bash script file could not be read: " + ex.Message);
        }

        var lines = content.Split("\n");
        AnalyseInputFile(lines);
    }

    private void AssertSlideshowWasCorrectlyProduced()
    {
        var output = Directory.GetFiles(Configuration.OutputPath, "*.mp4", SearchOption.TopDirectoryOnly);
        var expectedNumberOfMp4Files = 2;
        Assert.That(output.Length, Is.AtLeast(expectedNumberOfMp4Files), $"at least {expectedNumberOfMp4Files} *.mp4 were expected, but {output.Length} were found");
        AssertSlideshowDurationIsCoerent();
    }

    private void AssertSlideshowDurationIsCoerent()
    {
        var input = new FfprobeVideoLengthCommandInput(Configuration.SlideshowOutputPath);
        var command = new FfprobeVideoLengthCommand(input, new NullProgressNotification());
        decimal.TryParse(command.Do(), out var lengthDecimal);
        Assert.GreaterOrEqual(lengthDecimal, _lastFrame.EndSecond);
    }

    protected abstract IEnumerable<string> ImagesNamesConvertedFromPdf();

    protected abstract int NumberOfExpectedImages();

    protected abstract void AnalyseInputFile(string[] lines);
}