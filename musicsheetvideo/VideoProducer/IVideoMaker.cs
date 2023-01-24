using musicsheetvideo.Timestamp;

namespace musicsheetvideo.VideoProducer;

public interface IVideoMaker
{
    void MakeVideo(List<Frame> frames, IVideoMakerConfiguration configuration);
}