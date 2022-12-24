using musicsheetvideo.Command;

namespace test.Stubs;

public class NullCommand : ICommand
{
    public string Do()
    {
        return "";
    }

    public string DescribeItselfRunning => "";
}