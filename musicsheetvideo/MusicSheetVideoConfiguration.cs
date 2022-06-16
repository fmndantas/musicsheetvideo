namespace musicsheetvideo;

public class MusicSheetVideoConfiguration
{
    private readonly string _outputPath;
    private readonly string _pdfPath;
    private readonly string _audioPath;
    private readonly string _defaultImage;
    public string OutputPath => _outputPath;
    public string PdfPath => _pdfPath;
    public string AudioPath => _audioPath;
    public string ImagesPath => Path.Combine(OutputPath, "images");
    public string InputPath => Path.Combine(OutputPath, "input.txt");
    public string VideoPath => Path.Combine(OutputPath, "output.mp4");
    public string FinalVideoPath => Path.Combine(OutputPath, "final-output.mp4");
    public string DefaultImage => _defaultImage;

    public MusicSheetVideoConfiguration(
        string outputPath,
        string pdfPath,
        string audioPath,
        string defaultImagePath
    )
    {
        _outputPath = outputPath;
        _pdfPath = pdfPath;
        _audioPath = audioPath;
        _defaultImage = defaultImagePath;
    }
}