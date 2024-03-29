using musicsheetvideo.Configuration;
using musicsheetvideo.Configuration.FromAudio;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;

namespace musicsheetvideo;

public class MusicSheetVideo
{
    private readonly IFrameProcessor _frameProcessor;
    private readonly IVideoMaker _videoMaker;
    private readonly IPdfConverter _pdfConverter;
    private readonly IProgressNotification _progressNotification;

    public MusicSheetVideo(IPdfConverter pdfConverter, IFrameProcessor frameProcessor, IVideoMaker videoMaker,
        IProgressNotification progressNotification
    )
    {
        _pdfConverter = pdfConverter;
        _frameProcessor = frameProcessor;
        _videoMaker = videoMaker;
        _progressNotification = progressNotification;
    }

    public void MakeVideo(List<Frame> frames, IMusicSheetVideoConfiguration configuration)
    {
        _progressNotification.NotifyProgress("Starting video making");
        _pdfConverter.ConvertPdfToImages(configuration);
        _videoMaker.MakeVideo(_frameProcessor.ProcessFrames(frames), configuration);
        _progressNotification.NotifyProgress($"Done! Video path is \"{configuration.OutputVideoPath}\"");
    }
}