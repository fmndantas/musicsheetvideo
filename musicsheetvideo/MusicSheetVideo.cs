using musicsheetvideo.Frame;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.VideoProducer;

namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly IFrameProcessor _frameProcessor;
    private readonly IVideoProducer _videoProducer;
    private readonly IPdfConverter _pdfConverter;

    public MusicSheetVideo(
        IPdfConverter pdfConverter,
        IFrameProcessor frameProcessor,
        IVideoProducer videoProducer
    )
    {
        _pdfConverter = pdfConverter;
        _frameProcessor = frameProcessor;
        _videoProducer = videoProducer;
    }

    public void MakeVideo(List<Frame.Frame> frames)
    {
        _pdfConverter.ConvertPdfToImages();
        _videoProducer.MakeVideo(_frameProcessor.ProcessFrames(frames));
    }
}
