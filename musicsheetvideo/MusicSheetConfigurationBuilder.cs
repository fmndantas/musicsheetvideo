namespace musicsheetvideo;

public class MusicSheetConfigurationBuilder
{
    private string _outputPath;
    private string _pdfPath;
    private string _audioPath;
    private string _defaultImagePath;
    private string _imagePrefix;
    private string _imageFormat;

    private MusicSheetConfigurationBuilder()
    {
        _outputPath = "";
        _pdfPath = "";
        _audioPath = "";
        _defaultImagePath = "";
        _imageFormat = "jpg";
        _imagePrefix = "page";
    }

    public MusicSheetConfigurationBuilder WithOutputPath(string outputPath)
    {
        _outputPath = outputPath;
        return this;
    }

    public MusicSheetConfigurationBuilder WithPdfPath(string pdfPath)
    {
        _pdfPath = pdfPath;
        return this;
    }

    public MusicSheetConfigurationBuilder WithAudioPath(string audioPath)
    {
        _audioPath = audioPath;
        return this;
    }

    public MusicSheetConfigurationBuilder WithDefaultImagePath(string defaultImagePath)
    {
        _defaultImagePath = defaultImagePath;
        return this;
    }

    public MusicSheetConfigurationBuilder WithImagePrefix(string imagePrefix)
    {
        _imagePrefix = imagePrefix;
        return this;
    }

    public MusicSheetConfigurationBuilder WithImageFormat(string imageFormat)
    {
        _imageFormat = imageFormat;
        return this;
    }

    public MusicSheetVideoConfiguration Build()
    {
        return new MusicSheetVideoConfiguration(_outputPath, _pdfPath, _audioPath, _defaultImagePath, _imagePrefix,
            _imageFormat);
    }

    public static MusicSheetConfigurationBuilder OneConfiguration()
    {
        return new MusicSheetConfigurationBuilder();
    }
}