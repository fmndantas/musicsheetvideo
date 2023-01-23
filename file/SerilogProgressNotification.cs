using musicsheetvideo;
using Serilog;
using Serilog.Core;

namespace file;

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

    public void NotifyError(string message)
    {
        _delegate.Error(message);
    }
}