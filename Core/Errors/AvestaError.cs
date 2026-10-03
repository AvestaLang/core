namespace Core.Errors;

public sealed class AvestaError
{
    public string Code { get; }
    public string Message { get; }
    public int Line { get; }
    public int Column { get; }

    public AvestaError(
        string code,
        string message,
        int line,
        int column)
    {
        Code = code;
        Message = message;
        Line = line;
        Column = column;
    }
}