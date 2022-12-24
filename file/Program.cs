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

if (parserResult.Errors.Any())
{
    var message = string.Join(",", parserResult.Errors.Select(x => x.ToString()));
    Console.WriteLine(message);
    return;
}

if (!File.Exists(inputFilePath))
{
    Console.WriteLine($"Inexistent file {inputFilePath}");
    return;
}

var inputFile = File.ReadAllText(inputFilePath);
var inputFileJson = JsonConvert.DeserializeObject<ConfigurationDto>(inputFile);

if (inputFileJson == null)
{
    Console.WriteLine("Input file was not parsed correctly");
    return;
}

if (!inputFileJson.Valid)
{
    var validation = inputFileJson.Validate();
    Console.WriteLine("Some errors were found on input file:");
    foreach (var error in validation)
    {
        Console.WriteLine($"    - {error}");
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

var serilogProgressNotification = new SerilogProgressNotification();
var entrypoint = new Entrypoint(
    new ImagemagickPdfConverter(serilogProgressNotification, new ImagemagickPdfConversionCommand(configuration)),
    new FrameProcessor(new IntervalProcessor(), serilogProgressNotification),
    new FfmpegVideoMaker(
        new List<ICommand> { new FfmpegSlideshowCommand(configuration), new FfmpegJoinAudioCommand(configuration) },
        serilogProgressNotification
    ),
    serilogProgressNotification
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
}