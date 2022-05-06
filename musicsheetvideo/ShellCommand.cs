using System.Diagnostics;

namespace musicsheetvideo;

public abstract class ShellCommand : ICommand
{
    private readonly string _command;
    private readonly string _arguments;

    protected ShellCommand(string command, string arguments)
    {
        _command = command;
        _arguments = arguments;
    }

    public string Command => $"{_command} {_arguments}";

    public string Do()
    {
        var startInfo = new ProcessStartInfo(_command)
        {
            Arguments = _arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var process = new Process();
        var output = string.Empty;
        var error = string.Empty;
        try
        {
            process.StartInfo = startInfo;
            process.Start();
            output += process.StandardOutput.ReadToEnd();
            error += process.StandardError.ReadToEnd();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            var message = $"Problem on ShellCommand execution: \"{ex.Message}\"";
            message += error.Length > 0 ? $"\nstderror: {error}" : string.Empty;
            throw new Exception(message);
        }

        return output;
    }
}