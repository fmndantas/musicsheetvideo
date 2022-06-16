namespace musicsheetvideo.CustomException;

public class ShellCommandExecutionException : System.Exception
{
    private static string Message = "Error when executing shell command";

    public ShellCommandExecutionException() : base(Message)
    {
    }

    public ShellCommandExecutionException(string message) : base(Message + $": {message}")
    {
    }

    public ShellCommandExecutionException(string message, System.Exception ex) : base(Message + $": {message}", ex)
    {
    }
}