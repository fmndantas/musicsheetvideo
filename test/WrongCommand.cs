using musicsheetvideo;
using musicsheetvideo.Command;
using NUnit.Framework;
using test.Stubs;

namespace test;

[TestFixture]
public class WrongCommand : ShellCommand
{
    public WrongCommand() : base(new MusicSheetVideoConfiguration("", "", "", "", "", ""),
        new NullProgressNotification())
    {
    }

    protected override string CommandName => "wrong";
    protected override string Arguments => "command";

    protected override void DescribeItselfRunning()
    {
    }
}