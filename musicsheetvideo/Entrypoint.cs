using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;

namespace musicsheetvideo;

public class Entrypoint
{
    private readonly MusicSheetVideo _delegate;

    public Entrypoint(IPdfConverter pdfConverter, IFrameProcessor frameProcessor, IVideoMaker videoMaker, IProgressNotification progressNotification)
    {
        _delegate = new MusicSheetVideo(pdfConverter, frameProcessor, videoMaker, progressNotification);
    }

    public void MakeVideo(List<Frame> frames, MusicSheetVideoConfiguration configuration)
    {
        _delegate.MakeVideo(frames, configuration);
    }
}