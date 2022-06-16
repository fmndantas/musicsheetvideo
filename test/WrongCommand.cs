using musicsheetvideo;
using musicsheetvideo.Command;

namespace test;

public class WrongCommand : ShellCommand
{
    public WrongCommand() : base("foolish", "command")
    {
    }
}