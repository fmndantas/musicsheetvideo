using musicsheetvideo.Command;

namespace musicsheetvideo.Pipeline;

public interface IFfmpegPipelineMaker
{
    List<ICommand> MakePipeline();
}