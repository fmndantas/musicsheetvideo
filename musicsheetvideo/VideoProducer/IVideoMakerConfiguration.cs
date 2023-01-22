using musicsheetvideo.PdfConverter;

namespace musicsheetvideo.VideoProducer;

public interface IVideoMakerConfiguration : IPdfConverterConfiguration
{
    public string DefaultImagePath { get; }
    public string ImagePrefix { get; }
    public string ImageFormat { get; }
    public string SlideshowInputPath { get; }
}