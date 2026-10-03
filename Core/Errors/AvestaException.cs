namespace Core.Errors;

public sealed class AvestaException : Exception
{
    public AvestaError Error { get; }

    public AvestaException(AvestaError error)
        : base(error.Message)
    {
        Error = error;
    }
}