using musicsheetvideo.Frame;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.VideoProducer;

namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly IFrameProcessor _frameProcessor;
    private readonly IVideoMaker _videoMaker;
    private readonly IPdfConverter _pdfConverter;

    public MusicSheetVideo(
        IPdfConverter pdfConverter,
        IFrameProcessor frameProcessor,
        IVideoMaker videoMaker
    )
    {
        _pdfConverter = pdfConverter;
        _frameProcessor = frameProcessor;
        _videoMaker = videoMaker;
    }

    public void MakeVideo(List<Frame.Frame> frames)
    {
        _pdfConverter.ConvertPdfToImages();
        _videoMaker.MakeVideo(_frameProcessor.ProcessFrames(frames));
    }
}
