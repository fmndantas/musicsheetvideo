namespace musicsheetvideo.Command;

public interface ICommand
{
    string Do();
    string DescribeItselfRunning { get; }
}