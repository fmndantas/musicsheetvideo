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
        process.StartInfo = startInfo;
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        if (error.Length > 0)
        {
            throw new ShellCommandExecutionException(error);
        }
        process.WaitForExit();
        return output;
    }
}