using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public abstract class AcceptanceTestsBase
{
    protected MusicSheetVideoConfiguration _configuration;
    private IVideoProducer _producer;
    private IIntervalProcesser _intervalProcesser;
    private MusicSheetVideo _app;
    private Frame _lastFrame;

    protected void StartTest(
        MusicSheetVideoConfiguration configuration,
        IIntervalProcesser intervalProcesser,
        IVideoProducer producer,
        List<Frame> frames
    )
    {
        frames.Sort();
        _lastFrame = frames.Last();
        _configuration = configuration;
        _producer = producer;
        _app = new MusicSheetVideo(configuration, intervalProcesser, producer);
        DeleteGeneratedFiles();
        _app.MakeVideo(frames);
        AssertImagesWereCreatedCorrectly();
        AssertFfmpegInputFileWasCreatedCorrectly();
        AssertSlideshowWasCorrectlyProduced();
    }

    private void DeleteGeneratedFiles()
    {
        File.Delete(_configuration.InputPath);
        File.Delete(_configuration.VideoPath);
        File.Delete(_configuration.FinalVideoPath);
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
        var ffmpegInput = Path.Combine(_configuration.BasePath, "input.txt");
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
        var output = Directory.GetFiles(_configuration.BasePath, "*.mp4",
            SearchOption.TopDirectoryOnly);
        Assert.AreEqual(2, output.Length);
        Assert.AreEqual(_configuration.VideoPath, output.First());
        AssertSlideshowDurationIsCoerent();
    }

    private void AssertSlideshowDurationIsCoerent()
    {
        var command = new FfprobeVideoLengthCommand(_configuration);
        decimal.TryParse(command.Do(), out var lengthDecimal);
        Assert.LessOrEqual(
            Math.Abs(_lastFrame.EndSecond - lengthDecimal),
            1,
            $"Target: {_lastFrame.EndSecond}, Actual: {lengthDecimal}"
        );
    }

    protected abstract IEnumerable<string> FileNames();

    protected abstract int NumberOfExpectedImages();

    protected abstract void AnalyseInputFile(string[] lines);
}