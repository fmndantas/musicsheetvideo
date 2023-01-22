using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegJoinAudioCommand;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;

namespace musicsheetvideo.Configuration.FromAudio;

public class FromAudioConfigurationBuilder
{
    private string _outputPath;
    private string _pdfPath;
    private string _audioPath;
    private string _defaultImagePath;
    private string _imagePrefix;
    private string _imageFormat;

    private FromAudioConfigurationBuilder()
    {
        _outputPath = "";
        _pdfPath = "";
        _audioPath = "";
        _defaultImagePath = "";
        _imageFormat = "jpg";
        _imagePrefix = "page";
    }

    public FromAudioConfigurationBuilder WithOutputPath(string outputPath)
    {
        _outputPath = outputPath;
        return this;
    }

    public FromAudioConfigurationBuilder WithPdfPath(string pdfPath)
    {
        _pdfPath = pdfPath;
        return this;
    }

    public FromAudioConfigurationBuilder WithAudioPath(string audioPath)
    {
        _audioPath = audioPath;
        return this;
    }

    public FromAudioConfigurationBuilder WithDefaultImagePath(string defaultImagePath)
    {
        _defaultImagePath = defaultImagePath;
        return this;
    }

    public FromAudioConfigurationBuilder WithImagePrefix(string imagePrefix)
    {
        _imagePrefix = imagePrefix;
        return this;
    }

    public FromAudioConfigurationBuilder WithImageFormat(string imageFormat)
    {
        _imageFormat = imageFormat;
        return this;
    }

    public FromAudioConfiguration Build()
    {
        return new FromAudioConfiguration(_outputPath, _pdfPath, _audioPath, _defaultImagePath, _imagePrefix,
            _imageFormat);
    }

    public ImagemagickPdfConversionCommandInput BuildImagemagickPdfConversionCommandInput()
    {
        var defaultConfiguration = Build();
        return new ImagemagickPdfConversionCommandInput(pdfPath: _pdfPath, imagesPath: defaultConfiguration.ImagesDirectoryPath,
            imagePrefix: _imagePrefix, imageFormat: _imageFormat);
    }

    public FfmpegSlideshowCommandInput BuildFfmpegSlideshowCommandInput()
    {
        var defaultConfiguration = Build();
        return new FfmpegSlideshowCommandInput(textInputPath: defaultConfiguration.SlideshowInputPath,
            outputVideoPath: defaultConfiguration.SlideshowOutputPath);
    }

    public FfmpegJoinAudioCommandInput BuildFfmpegJoinAudioCommandInput()
    {
        var defaultConfiguration = Build();
        return new FfmpegJoinAudioCommandInput(inputVideoPath: defaultConfiguration.SlideshowOutputPath,
            audioPath: _audioPath, outputVideoPath: defaultConfiguration.OutputVideoPath);
    }

    public static FromAudioConfigurationBuilder OneConfiguration()
    {
        return new FromAudioConfigurationBuilder();
    }
}