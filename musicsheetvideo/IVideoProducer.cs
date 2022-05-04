namespace musicsheetvideo;

public interface IVideoProducer
{
    Task MakeVideo(List<Frame> frames);
}