using musicsheetvideo.Frame;
using musicsheetvideo.VideoProducer;

namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly IFrameProcessor _frameProcessor;
    private readonly IVideoProducer _videoProducer;

    public MusicSheetVideo(
        IFrameProcessor frameProcessor,
        IVideoProducer videoProducer
    )
    {
        _frameProcessor = frameProcessor;
        _videoProducer = videoProducer;
    }

    public void MakeVideo(List<Frame.Frame> frames)
    {
        _videoProducer.MakeVideo(_frameProcessor.ProcessFrames(frames));
    }
}