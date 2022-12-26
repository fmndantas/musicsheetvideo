using musicsheetvideo;

namespace test.Stubs;

public class NullProgressNotification : IProgressNotification
{
    public void NotifyProgress(string message)
    {
    }
}