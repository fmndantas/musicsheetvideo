namespace musicsheetvideo;

public class MusicSheetVideoConfiguration
{
    private readonly string _basePath;
    private readonly string _pdfPath;
    private readonly string _audioPath;
    private readonly string _defaultImage;
    public string BasePath => _basePath;
    public string PdfPath => _pdfPath;
    public string AudioPath => _audioPath;
    public string ImagesPath => Path.Combine(BasePath, "images");
    public string InputPath => Path.Combine(BasePath, "input.txt");
    public string VideoPath => Path.Combine(BasePath, "output.mp4");
    public string FinalVideoPath => Path.Combine(BasePath, "final-output.mp4");
    public string DefaultImage => _defaultImage;

    public MusicSheetVideoConfiguration(
        string basePath,
        string pdfPath,
        string audioPath,
        string defaultImagePath
    )
    {
        _basePath = basePath;
        _pdfPath = pdfPath;
        _audioPath = audioPath;
        _defaultImage = defaultImagePath;
    }
}