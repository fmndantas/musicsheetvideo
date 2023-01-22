using CommandLine;
using file;
using musicsheetvideo;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

var logger = new SerilogProgressNotification();

var inputFilePath = "";
var debugMode = false;
var parserResult = Parser.Default.ParseArguments<ParserOptions>(args).WithParsed(o =>
{
    inputFilePath = o.FilePath;
    debugMode = o.DebugMode;
});

logger.NotifyProgress($"Running in {(debugMode ? "DEBUG" : "COMMON")} mode");

if (parserResult.Errors.Any()) return;

if (!File.Exists(inputFilePath))
{
    logger.NotifyError($"Inexistent file \"{inputFilePath}\"");
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

var configurationBuilder = MusicSheetVideoConfigurationBuilder
    .OneConfiguration()
    .WithPdfPath(inputFileJson.PdfPath!)
    .WithAudioPath(inputFileJson.AudioPath!)
    .WithOutputPath(inputFileJson.OutputPath!)
    .WithDefaultImagePath(inputFileJson.DefaultImagePath!);

var pipelineMaker = new FFmpegPipelineMaker(configurationBuilder, logger);
var videoMaker = new FfmpegVideoMaker(pipelineMaker.MakePipeline(PipelineMode.Fullscreen), logger);
var entrypoint = new Entrypoint(
    new ImagemagickPdfConverter(new ImagemagickPdfConversionCommand(configurationBuilder.BuildImagemagickPdfConversionCommandInput(), logger)),
    new FrameProcessor(new IntervalProcessor(), logger),
    videoMaker,
    logger
);

try
{
    entrypoint.MakeVideo(inputFileJson.DomainFrames, configurationBuilder.Build());
}
catch (Exception e)
{
    logger.NotifyError(debugMode ? e.ToString() : e.Message);
}

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