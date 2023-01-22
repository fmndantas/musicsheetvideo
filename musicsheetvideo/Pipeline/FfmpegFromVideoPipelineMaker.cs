using musicsheetvideo.Command;
using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegOverlayVideosCommand;
using musicsheetvideo.Command.FfmpegRescaleVideoCommand;
using musicsheetvideo.Configuration.FromVideo;

namespace musicsheetvideo.Pipeline;

public class FfmpegFromVideoPipelineMaker : IFfmpegPipelineMaker
{
    private readonly FromVideoConfigurationBuilder _configuration;
    private readonly IProgressNotification _progressNotification;

    public FfmpegFromVideoPipelineMaker(FromVideoConfigurationBuilder configuration,
        IProgressNotification progressNotification)
    {
        _configuration = configuration;
        _progressNotification = progressNotification;
    }

    public List<ICommand> MakePipeline()
    {
        return new List<ICommand>
        {
            new FfmpegRescaleVideoCommand(_configuration.BuildRescaleVideoCommandInput(), _progressNotification),
            new FfmpegSlideshowCommand(divideHeigthBy: 3, configuration: _configuration.BuildSlideshowCommandInput(),
                progressNotification: _progressNotification),
            new FfmpegOverlayVideosCommand(_configuration.BuildOverlayVideosCommandInput(), _progressNotification)
        };
    }
}