using System.Diagnostics;

namespace musicsheetvideo;

public class FfmpegSlideshowCommand : ICommand
{
    private readonly MusicSheetVideoConfiguration _configuration;
    private readonly string _command;
    private readonly string _arguments;
    private TaskCompletionSource<bool> _eventHandled;

    public string Command => _command;
    public string Arguments => _arguments;

    public FfmpegSlideshowCommand(MusicSheetVideoConfiguration configuration)
    {
        _configuration = configuration;
        _command = "ffmpeg";
        _arguments = $"-f concat -safe 0 -i {_configuration.InputPath} -vf"
                     + $" \"crop=trunc(iw/2)*2:trunc(ih/2)*2\" -vsync vfr -pix_fmt yuv420p {_configuration.VideoPath}";
        _eventHandled = new TaskCompletionSource<bool>();
    }

    public async Task Do()
    {
        _eventHandled = new TaskCompletionSource<bool>();
        var startInfo = new ProcessStartInfo(Command) { 
            Arguments = Arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using (var process = new Process())
        {
            try
            {
                process.StartInfo = startInfo;
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(FinishProcess);
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem on slideshow command execution: \"{ex.Message}\"");
                return;
            }
        }

        await Task.WhenAny(_eventHandled.Task, Task.Delay(5000));
    }

    private void FinishProcess(object sender, System.EventArgs e)
    {
        _eventHandled.TrySetResult(true);
    }
}