using System.Collections.Generic;
using System.IO;
using System.Linq;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.Frame;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using NUnit.Framework;

namespace test.AcceptanceTests;

[TestFixture]
[Category("integration")]
[Category("acceptance")]
public abstract class AcceptanceTestsBase
{
    protected const string BasePath = "/home/fernando/Documents/github/musicsheetvideo/test/AcceptanceTests/";
    protected readonly string DefaultImagePath;
    protected MusicSheetVideoConfiguration _configuration;

    protected AcceptanceTestsBase()
    {
        DefaultImagePath = Path.Combine(BasePath, "Data/default-image.jpg");
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
        frames.Sort();
        _lastFrame = frames.Last();
        _configuration = configuration;
        DeleteGeneratedFiles();
        _app = new MusicSheetVideo(pdfConverter, frameProcessor, videoMaker);
        _app.MakeVideo(frames);
        AssertImagesWereCreatedCorrectly();
        AssertFfmpegInputFileWasCreatedCorrectly();
        AssertSlideshowWasCorrectlyProduced();
    }

    private void DeleteGeneratedFiles()
    {
        if (File.Exists(_configuration.InputPath))
        {
            File.Delete(_configuration.InputPath);
        }

        if (File.Exists(_configuration.VideoPath))
        {
            File.Delete(_configuration.VideoPath);
        }

        if (File.Exists(_configuration.FinalVideoPath))
        {
            File.Delete(_configuration.FinalVideoPath);
        }

        if (Directory.Exists(_configuration.ImagesPath))
        {
            foreach (var image in Directory.GetFiles(_configuration.ImagesPath, "*", SearchOption.AllDirectories))
            {
                File.Delete(image);
            }
        }
    }

    private void AssertImagesWereCreatedCorrectly()
    {
        Assert.True(Directory.Exists(_configuration.ImagesPath));
        var images = Directory.GetFiles(_configuration.ImagesPath, "*", SearchOption.AllDirectories);
        Assert.AreEqual(NumberOfExpectedImages(), images.Length);
        var imagesNames = images.Select(x => x.Split("/").Last()).ToArray();
        foreach (var fileName in FileNames())
        {
            Assert.True(imagesNames.Contains(fileName));
        }
    }

    private void AssertFfmpegInputFileWasCreatedCorrectly()
    {
        var ffmpegInput = Path.Combine(_configuration.OutputPath, "input.txt");
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
        var output = Directory.GetFiles(_configuration.OutputPath, "*.mp4",
            SearchOption.TopDirectoryOnly);
        Assert.AreEqual(2, output.Length);
        Assert.AreEqual(_configuration.VideoPath, output.First());
        AssertSlideshowDurationIsCoerent();
    }

    private void AssertSlideshowDurationIsCoerent()
    {
        var command = new FfprobeVideoLengthCommand(_configuration);
        decimal.TryParse(command.Do(), out var lengthDecimal);
        Assert.GreaterOrEqual(lengthDecimal, _lastFrame.EndSecond);
    }

    protected abstract IEnumerable<string> FileNames();

    protected abstract int NumberOfExpectedImages();

    protected abstract void AnalyseInputFile(string[] lines);
}