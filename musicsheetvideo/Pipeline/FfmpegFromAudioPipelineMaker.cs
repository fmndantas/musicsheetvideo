using musicsheetvideo.Command;
using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegJoinAudioCommand;
using musicsheetvideo.Configuration.FromAudio;

namespace musicsheetvideo.Pipeline;

public class FfmpegFromAudioPipelineMaker : IFfmpegPipelineMaker
{
    private readonly IProgressNotification _progressNotification;
    private readonly FromAudioConfigurationBuilder _configuration;

    public FfmpegFromAudioPipelineMaker(FromAudioConfigurationBuilder configuration, IProgressNotification progressNotification)
    {
        _configuration = configuration;
        _progressNotification = progressNotification;
    }

    public List<ICommand> MakePipeline()
    {
        return new List<ICommand>
        {
            new FfmpegSlideshowCommand(_configuration.BuildFfmpegSlideshowCommandInput(), _progressNotification),
            new FfmpegJoinAudioCommand(_configuration.BuildFfmpegJoinAudioCommandInput(), _progressNotification)
        };
    }
}