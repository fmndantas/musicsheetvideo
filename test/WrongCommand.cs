using musicsheetvideo.Command;
using NUnit.Framework;
using test.Stubs;

namespace test;

[TestFixture]
public class WrongCommand : ShellCommand<object>
{
    public WrongCommand() : base(new object(), new NullProgressNotification())
    {
    }

    protected override string CommandName => "wrong";
    protected override string Arguments => "command";

    protected override void DescribeItselfRunning()
    {
    }
}