using CommandLine;

namespace file;

public class ParserOptions
{
    [Option(longName: "file", shortName: 'f', Required = true, HelpText = "Set input file path")]
    public string FilePath { get; set; }

    [Option(longName: "debug", shortName: 'd', Required = false, HelpText = "Makes application to run in debug mode")]
    public bool DebugMode { get; set; }
    
    [Option(longName: "mode", shortName: 'm', Required = false, HelpText = "Indicates the type of the input (a - audio; v - video)", Default = "a")]
    public string Mode { get; set; }
}