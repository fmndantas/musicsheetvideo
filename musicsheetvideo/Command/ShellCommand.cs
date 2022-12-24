using System.Diagnostics;
using musicsheetvideo.CustomException;

namespace musicsheetvideo.Command;

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
        var output = string.Empty;
        var error = string.Empty;
        try
        {
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();
            if (error.Length > 0)
            {
                throw new Exception(error);
            }

            process.WaitForExit();
        }
        catch (Exception ex)
        {
            throw new ShellCommandExecutionException(ex.Message);
        }

        return output;
    }

    public abstract string DescribeItselfRunning { get; }
}