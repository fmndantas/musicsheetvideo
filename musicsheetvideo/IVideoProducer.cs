namespace musicsheetvideo;

public interface IVideoProducer
{
    Task MakeVideo(List<Page> pages);
}