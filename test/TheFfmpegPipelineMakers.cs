using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegJoinAudioCommand;
using musicsheetvideo.Command.FfmpegOverlayVideosCommand;
using musicsheetvideo.Command.FfmpegRescaleVideoCommand;
using musicsheetvideo.Configuration.FromAudio;
using musicsheetvideo.Configuration.FromVideo;
using musicsheetvideo.Pipeline;
using NUnit.Framework;
using test.Stubs;

namespace test;

[TestFixture]
public class TheFfmpegPipelineMakers
{
    [Test]
    public void ProducesPipelineForFullscreenVideo()
    {
        var configuration = FromAudioConfigurationBuilder.OneConfiguration();
        var pipeline = new FfmpegFromAudioPipelineMaker(configuration, new NullProgressNotification()).MakePipeline();
        Assert.That(pipeline.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.IsInstanceOf<FfmpegSlideshowCommand>(pipeline[0]);
            Assert.IsInstanceOf<FfmpegJoinAudioCommand>(pipeline[1]);
        });
    }

    [Test]
    public void ProducesPipelineForBottomVideo()
    {
        var configuration = FromVideoConfigurationBuilder.OneConfiguration();
        var pipeline = new FfmpegFromVideoPipelineMaker(configuration, new NullProgressNotification()).MakePipeline();
        Assert.That(pipeline.Count, Is.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.IsInstanceOf<FfmpegRescaleVideoCommand>(pipeline[0]);
            Assert.IsInstanceOf<FfmpegSlideshowCommand>(pipeline[1]);
            Assert.IsInstanceOf<FfmpegOverlayVideosCommand>(pipeline[2]);
        });
    }
}