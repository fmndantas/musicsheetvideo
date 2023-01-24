using CommandLine;
using file;
using musicsheetvideo;
using musicsheetvideo.Command.ImagemagickPdfConversionCommand;
using musicsheetvideo.Configuration;
using musicsheetvideo.PdfConverter;
using musicsheetvideo.Pipeline;
using musicsheetvideo.Timestamp;
using musicsheetvideo.VideoProducer;
using Newtonsoft.Json;

var logger = new SerilogProgressNotification();

var inputFilePath = "";
var debugMode = false;
var mode = "";
var parserResult = Parser.Default.ParseArguments<ParserOptions>(args).WithParsed(o =>
{
    inputFilePath = o.FilePath;
    debugMode = o.DebugMode;
    mode = o.Mode;
});

if (parserResult.Errors.Any()) return;

logger.NotifyProgress($"Running in {(debugMode ? "DEBUG" : "COMMON")} mode");
logger.NotifyProgress($"Using \"{mode}\" mode");

if (!File.Exists(inputFilePath))
{
    logger.NotifyError($"Inexistent file \"{inputFilePath}\"");
    return;
}

MusicSheetVideo maker = null!;
IMusicSheetVideoConfiguration configuration = null!;
var frames = new List<Frame>();

void SetupForAudioMode()
{
    var inputFile = File.ReadAllText(inputFilePath);
    var inputFileJson = JsonConvert.DeserializeObject<FromAudioConfigurationDto>(inputFile);
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

    var configurationBuilder = inputFileJson.GetConfiguration();
    configuration = configurationBuilder.Build();
    frames = inputFileJson.DomainFrames;
    var pipeline = new FfmpegFromAudioPipelineMaker(configurationBuilder, logger).MakePipeline();
    maker = new MusicSheetVideo(
        new ImagemagickPdfConverter(
            new ImagemagickPdfConversionCommand(
                configurationBuilder.BuildImagemagickPdfConversionCommandInput(),
                logger
            )
        ),
        new FrameProcessor(new IntervalProcessor(), logger),
        new FfmpegVideoMaker(pipeline, logger),
        logger
    );
}

void SetupForVideoMode()
{
    var inputFile = File.ReadAllText(inputFilePath);
    var inputFileJson = JsonConvert.DeserializeObject<FromVideoConfigurationDto>(inputFile);
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

    var configurationBuilder = inputFileJson.GetConfiguration();
    configuration = configurationBuilder.Build();
    frames = inputFileJson.DomainFrames;
    var pipeline = new FfmpegFromVideoPipelineMaker(configurationBuilder, logger).MakePipeline();
    maker = new MusicSheetVideo(
        new ImagemagickPdfConverter(
            new ImagemagickPdfConversionCommand(
                configurationBuilder.BuildImagemagickPdfConversionCommandInput(),
                logger
            )
        ),
        new FrameProcessor(new IntervalProcessor(), logger),
        new FfmpegVideoMaker(pipeline, logger),
        logger
    );
}

if (mode == "a")
{
    SetupForAudioMode();
}
else
{
    SetupForVideoMode();
}

try
{
    maker.MakeVideo(frames, configuration);
}
catch (Exception e)
{
    logger.NotifyError(debugMode ? e.ToString() : e.Message);
}