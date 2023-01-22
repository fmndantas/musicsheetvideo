using musicsheetvideo;
using NUnit.Framework;

namespace test.Stubs;

public class NunitProgressNotification : IProgressNotification
{
    public void NotifyProgress(string message)
    {
        TestContext.Progress.WriteLine(message);
    }
}