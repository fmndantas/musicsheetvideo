namespace musicsheetvideo.Configuration.FromAudio;

public class FromAudioConfiguration : IMusicSheetVideoConfiguration
{
    public FromAudioConfiguration(
        string outputPath,
        string pdfPath,
        string audioPath,
        string defaultImagePath,
        string imagePrefix,
        string imageFormat
    )
    {
        OutputPath = outputPath;
        PdfPath = pdfPath;
        AudioPath = audioPath;
        DefaultImagePath = defaultImagePath;
        ImagePrefix = imagePrefix;
        ImageFormat = imageFormat;
    }
    
    public string OutputPath { get; }
    public string PdfPath { get; }
    public string AudioPath { get; }
    public string DefaultImagePath { get; }
    public string ImagePrefix { get; }
    public string ImageFormat { get; }
    public string ImagesDirectoryPath => Path.Combine(OutputPath, "images");
    public string SlideshowInputPath => Path.Combine(OutputPath, "input.txt");
    public string SlideshowOutputPath => Path.Combine(OutputPath, "slideshow.mp4");
    public string OutputVideoPath => Path.Combine(OutputPath, "output.mp4");
}