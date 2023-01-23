namespace musicsheetvideo.Configuration.FromVideo;

public class FromVideoConfiguration : IMusicSheetVideoConfiguration
{
    public FromVideoConfiguration(
        string outputPath,
        string pdfPath,
        string inputVideoPath,
        string defaultImagePath,
        string imagePrefix,
        string imageFormat
    )
    {
        OutputPath = outputPath;
        PdfPath = pdfPath;
        InputVideoPath = inputVideoPath;
        DefaultImagePath = defaultImagePath;
        ImagePrefix = imagePrefix;
        ImageFormat = imageFormat;
    }

    public string OutputPath { get; }
    public string PdfPath { get; }
    public string InputVideoPath { get; }
    public string DefaultImagePath { get; }
    public string ImagePrefix { get; }
    public string ImageFormat { get; }
    public string ImagesDirectoryPath => Path.Combine(OutputPath, "images");
    public string RescaledInputPath => Path.Combine(OutputPath, "rescaled.mp4");
    public string SlideshowInputPath => Path.Combine(OutputPath, "input.txt");
    public string SlideshowOutputPath => Path.Combine(OutputPath, "slideshow.mp4");
    public string OutputVideoPath => Path.Combine(OutputPath, "output.mp4");
}