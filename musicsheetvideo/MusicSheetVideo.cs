namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly IFrameProcessor _frameProcessor;
    private readonly IVideoProducer _videoProducer;

    public MusicSheetVideo(
        MusicSheetVideoConfiguration configuration,
        IFrameProcessor frameProcessor,
        IVideoProducer videoProducer
    )
    {
        _configuration = configuration;
        _frameProcessor = frameProcessor;
        _videoProducer = videoProducer;
    }

    public void MakeVideo(List<Frame> frames)
    {
        _videoProducer.MakeVideo(_frameProcessor.ProcessFrames(frames));
    }
}