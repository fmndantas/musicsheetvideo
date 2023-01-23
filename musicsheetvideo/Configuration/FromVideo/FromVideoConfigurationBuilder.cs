using musicsheetvideo.Command.FfmepgSlideshowCommand;
using musicsheetvideo.Command.FfmpegOverlayVideosCommand;
using musicsheetvideo.Command.FfmpegRescaleVideoCommand;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;

namespace musicsheetvideo.Configuration.FromVideo;

public class FromVideoConfigurationBuilder
{
    private string _outputPath;
    private string _pdfPath;
    private string _inputVideoPath;
    private string _defaultImagePath;
    private string _imagePrefix;
    private string _imageFormat;

    private FromVideoConfigurationBuilder()
    {
        _outputPath = "";
        _pdfPath = "";
        _inputVideoPath = "";
        _defaultImagePath = "";
        _imagePrefix = "page";
        _imageFormat = "jpg";
    }

    public FromVideoConfigurationBuilder WithOutputPath(string outputPath)
    {
        _outputPath = outputPath;
        return this;
    }

    public FromVideoConfigurationBuilder WithPdfPath(string pdfPath)
    {
        _pdfPath = pdfPath;
        return this;
    }

    public FromVideoConfigurationBuilder WithInputVideoPath(string inputVideoPath)
    {
        _inputVideoPath = inputVideoPath;
        return this;
    }

    public FromVideoConfigurationBuilder DefaultImagePath(string defaultImagePath)
    {
        _defaultImagePath = defaultImagePath;
        return this;
    }

    public FromVideoConfigurationBuilder WithImagePrefix(string imagePrefix)
    {
        _imagePrefix = imagePrefix;
        return this;
    }

    public FromVideoConfigurationBuilder WithImageFormat(string imageFormat)
    {
        _imageFormat = imageFormat;
        return this;
    }

    public FromVideoConfigurationBuilder WithDefaultImagePath(string defaultImagePath)
    {
        _defaultImagePath = defaultImagePath;
        return this;
    }

    public FromVideoConfiguration Build()
    {
        return new FromVideoConfiguration(_outputPath, _pdfPath, _inputVideoPath, _defaultImagePath, _imagePrefix,
            _imageFormat);
    }

    public FfmpegRescaleVideoCommandInput BuildRescaleVideoCommandInput()
    {
        var configuration = Build();
        return new FfmpegRescaleVideoCommandInput(inputVideoPath: _inputVideoPath,
            outputVideoPath: configuration.RescaledInputPath);
    }

    public FfmpegSlideshowCommandInput BuildSlideshowCommandInput()
    {
        var configuration = Build();
        return new FfmpegSlideshowCommandInput(textInputPath: configuration.SlideshowInputPath,
            outputVideoPath: configuration.SlideshowOutputPath);
    }

    public FfmpegOverlayVideosCommandInput BuildOverlayVideosCommandInput()
    {
        var configuration = Build();
        return new FfmpegOverlayVideosCommandInput(videoBelowPath: configuration.RescaledInputPath,
            videoAbovePath: configuration.SlideshowOutputPath, outputVideoPath: configuration.OutputVideoPath);
    }

    public ImagemagickPdfConversionCommandInput BuildImagemagickPdfConversionCommandInput()
    {
        var configuration = Build();
        return new ImagemagickPdfConversionCommandInput(configuration.PdfPath, configuration.ImagesDirectoryPath,
            configuration.ImagePrefix, configuration.ImageFormat);
    }

    public static FromVideoConfigurationBuilder OneConfiguration()
    {
        return new FromVideoConfigurationBuilder();
    }
}