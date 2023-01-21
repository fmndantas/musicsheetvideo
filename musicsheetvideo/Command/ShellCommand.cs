using System.Diagnostics;
using musicsheetvideo.CustomException;

namespace musicsheetvideo.Command;

public abstract class ShellCommand : ICommand
{
    protected readonly IProgressNotification ProgressNotification;

    protected ShellCommand(MusicSheetVideoConfiguration configuration, IProgressNotification progressNotification)
    {
        Configuration = configuration;
        ProgressNotification = progressNotification;
    }

    protected ShellCommand(IProgressNotification progressNotification)
    {
        Configuration = new MusicSheetVideoConfiguration("", "", "", "", "", "");
        ProgressNotification = progressNotification;
    }

    protected MusicSheetVideoConfiguration Configuration { get; }
    protected abstract string CommandName { get; }
    protected abstract string Arguments { get; }
    public string Command => $"{CommandName} {Arguments}";

    public string Do()
    {
        DescribeItselfRunning();
        var startInfo = new ProcessStartInfo(CommandName)
        {
            Arguments = Arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var process = new Process();
        var output = string.Empty;
        try
        {
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
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
        finally
        {
            process.Dispose();
        }

        return output;
    }

    protected abstract void DescribeItselfRunning();
}