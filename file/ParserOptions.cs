using CommandLine;

namespace file;

public class ParserOptions
{
    [Option(longName: "file", shortName: 'f', Required = true, HelpText = "Set input file path")]
    public string FilePath { get; set; }
}