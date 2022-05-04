using System.Diagnostics;

namespace musicsheetvideo;

public abstract class ShellCommand : ICommand
{
    private readonly string _command;
    private readonly string _arguments;
    
    protected ShellCommand(string command, string arguments, int delay)
    {
        _command = command;
        _arguments = arguments;
    }

    public string Command => $"{_command} {_arguments}";
    
    public async Task Do()
    {
        var startInfo = new ProcessStartInfo(_command)
        {
            Arguments = _arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var process = new Process();
        try
        {
            process.StartInfo = startInfo;
            process.Start();
            while (!process.StandardOutput.EndOfStream)
            {
                var line = await process.StandardOutput.ReadLineAsync();
                Console.WriteLine(line);
            }

            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem on slideshow command execution: \"{ex.Message}\"");
        }
    }
}