namespace musicsheetvideo.VideoProducer;

public interface IVideoMaker
{
    void MakeVideo(List<Frame.Frame> frames);
}