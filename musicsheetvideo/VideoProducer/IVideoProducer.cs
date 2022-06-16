namespace musicsheetvideo.VideoProducer;

public interface IVideoProducer
{
    void MakeVideo(List<Frame.Frame> frames);
}