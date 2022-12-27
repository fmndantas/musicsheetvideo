using CommandLine;
using file;
using musicsheetvideo;
using musicsheetvideo.Command;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

var inputFilePath = "";
var parserResult = Parser.Default.ParseArguments<ParserOptions>(args).WithParsed(o => { inputFilePath = o.FilePath; });

var logger = new SerilogProgressNotification();

if (parserResult.Errors.Any()) return;

if (!File.Exists(inputFilePath))
{
    logger.NotifyError($"Inexistent file {inputFilePath}");
    return;
}

var inputFile = File.ReadAllText(inputFilePath);
var inputFileJson = JsonConvert.DeserializeObject<ConfigurationDto>(inputFile);

if (inputFileJson == null)
{
    logger.NotifyError("Input file was not parsed correctly");
    return;
}

if (!inputFileJson.Valid)
{
    var validation = inputFileJson.Validate();
    logger.NotifyError("Some errors were found on input file:");
    foreach (var error in validation)
    {
        logger.NotifyError($"    - {error}");
    }

    return;
}

var configurationBuilder = MusicSheetConfigurationBuilder
    .OneConfiguration()
    .WithPdfPath(inputFileJson.PdfPath!)
    .WithAudioPath(inputFileJson.AudioPath!)
    .WithOutputPath(inputFileJson.OutputPath!)
    .WithDefaultImagePath(inputFileJson.DefaultImagePath!);

var configuration = configurationBuilder.Build();

var entrypoint = new Entrypoint(
    new ImagemagickPdfConverter(new ImagemagickPdfConversionCommand(configuration, logger)),
    new FrameProcessor(new IntervalProcessor(), logger),
    new FfmpegVideoMaker(
        new List<ICommand>
            { new FfmpegSlideshowCommand(configuration, logger), new FfmpegJoinAudioCommand(configuration, logger) },
        logger
    ),
    logger
);
entrypoint.MakeVideo(inputFileJson.DomainFrames, configuration);

class SerilogProgressNotification : IProgressNotification
{
    private readonly Logger _delegate;

    public SerilogProgressNotification()
    {
        _delegate = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    }

    public void NotifyProgress(string message)
    {
        _delegate.Information(message);
    }

    public void NotifyError(string message)
    {
        _delegate.Error(message);
    }
}