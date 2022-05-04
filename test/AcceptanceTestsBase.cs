using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using musicsheetvideo;
using NUnit.Framework;

namespace test;

public abstract class AcceptanceTestsBase
{
    protected MusicSheetVideoConfiguration _configuration;
    private IVideoProducer _producer;
    private IGapFiller _gapFiller;
    private MusicSheetVideo _app;

    protected async Task StartTest(
        MusicSheetVideoConfiguration configuration,
        IGapFiller gapFiller,
        IVideoProducer producer,
        List<Page> pages
    )
    {
        _configuration = configuration;
        _producer = producer;
        _app = new MusicSheetVideo(configuration, gapFiller, producer);
        RemoveGeneratedFiles();
        await _app.MakeVideo(pages);
        AssertImagesWereCreatedCorrectly();
        AssertFfmpegInputFileWasCreatedCorrectly();
        AssertSlideshowWasCorrectlyProduced();
        AssertFinalVideoWasCorrectlyProduced();
    }

    private void RemoveGeneratedFiles()
    {
        File.Delete(_configuration.InputPath);
        File.Delete(_configuration.VideoPath);
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
        Assert.AreEqual(1, output.Length);
        Assert.AreEqual(_configuration.VideoPath, output.First());
        // ToDo: check length
        // ToDo: check that has no audio
    }

    private void AssertFinalVideoWasCorrectlyProduced()
    {
        throw new NotImplementedException();
    }

    protected abstract IEnumerable<string> FileNames();

    protected abstract int NumberOfExpectedImages();

    protected abstract void AnalyseInputFile(string[] lines);
}