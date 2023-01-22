using System.Collections.Generic;
using System.IO;
using System.Linq;
using musicsheetvideo;
using musicsheetvideo.Command;
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
    protected const string BasePath = "/home/fernando/Documents/github/musicsheetvideo/test/AcceptanceTests/";
    protected readonly string DefaultImagePath;
    protected MusicSheetVideoConfiguration Configuration;
    protected readonly IProgressNotification Logger;

    protected AcceptanceTestsBase()
    {
        DefaultImagePath = Path.Combine(BasePath, "Data/default-image.jpg");
        Logger = new NunitProgressNotification();
    }

    private IIntervalProcessor _intervalProcessor;
    private MusicSheetVideo _app;
    private Frame _lastFrame;

    protected void StartTest(
        MusicSheetVideoConfiguration configuration,
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
        _app = new MusicSheetVideo(pdfConverter, frameProcessor, videoMaker, new NullProgressNotification());
        _app.MakeVideo(frames, configuration);
        AssertImagesWereCreatedCorrectly();
        AssertFfmpegInputFileWasCreatedCorrectly();
        AssertSlideshowWasCorrectlyProduced();
        Logger.NotifyProgress("Done\n");
    }

    private void DeleteGeneratedFiles()
    {
        if (File.Exists(Configuration.SlideshowTextInputPath))
        {
            File.Delete(Configuration.SlideshowTextInputPath);
        }

        if (Directory.Exists(Configuration.OutputPath))
        {
            foreach (var video in Directory.GetFiles(Configuration.OutputPath, "*.mp4", SearchOption.AllDirectories))
            {
                File.Delete(video);
            }
        }

        if (Directory.Exists(Configuration.ImagesPath))
        {
            foreach (var image in Directory.GetFiles(Configuration.ImagesPath, "*", SearchOption.AllDirectories))
            {
                File.Delete(image);
            }
        }
    }

    private void AssertImagesWereCreatedCorrectly()
    {
        Assert.True(Directory.Exists(Configuration.ImagesPath));
        var images = Directory.GetFiles(Configuration.ImagesPath, "*", SearchOption.AllDirectories);
        Assert.AreEqual(NumberOfExpectedImages(), images.Length);
        var imagesNames = images.Select(x => x.Split("/").Last()).ToArray();
        foreach (var fileName in FileNames())
        {
            Assert.True(imagesNames.Contains(fileName));
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
        Assert.That(output.Length, Is.EqualTo(expectedNumberOfMp4Files),
            $"{expectedNumberOfMp4Files} *.mp4 were expected, but {output.Length} were found");
        AssertSlideshowDurationIsCoerent();
    }

    private void AssertSlideshowDurationIsCoerent()
    {
        var command = new FfprobeVideoLengthCommand(Configuration, new NullProgressNotification());
        decimal.TryParse(command.Do(), out var lengthDecimal);
        Assert.GreaterOrEqual(lengthDecimal, _lastFrame.EndSecond);
    }

    protected abstract IEnumerable<string> FileNames();

    protected abstract int NumberOfExpectedImages();

    protected abstract void AnalyseInputFile(string[] lines);
}