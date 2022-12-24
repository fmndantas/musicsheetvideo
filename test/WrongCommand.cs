using musicsheetvideo.Command;
using NUnit.Framework;

namespace test;

[TestFixture]
public class WrongCommand : ShellCommand
{
    public WrongCommand() : base("wrong", "command")
    {
    }

    public override string DescribeItselfRunning => "";
}