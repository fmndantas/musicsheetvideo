namespace musicsheetvideo;

public class MusicSheetVideoConfiguration
{
    public string OutputPath { get; }
    public string PdfPath { get; }
    public string AudioPath { get; }
    public string DefaultImage { get; }
    public string ImagePrefix { get; }
    public string ImageFormat { get; }
    public string ImagesPath => Path.Combine(OutputPath, "images");
    public string InputPath => Path.Combine(OutputPath, "input.txt");
    public string VideoPath => Path.Combine(OutputPath, "output.mp4");
    public string FinalVideoPath => Path.Combine(OutputPath, "final-output.mp4");

    public MusicSheetVideoConfiguration(
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
        DefaultImage = defaultImagePath;
        ImagePrefix = imagePrefix;
        ImageFormat = imageFormat;
    }
}