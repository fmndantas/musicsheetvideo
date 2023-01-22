using musicsheetvideo.VideoProducer;

namespace musicsheetvideo.Configuration;

public interface IMusicSheetVideoConfiguration : IVideoMakerConfiguration
{
    string OutputPath { get; }
    string SlideshowOutputPath { get; }
    string OutputVideoPath { get; }
}