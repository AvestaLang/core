namespace Core.Ast.Values;

public sealed class NumberValueNode : ValueNode
{
    public double Value { get; }

    public NumberValueNode(
        double value,
        SourceLocation location)
        : base(location)
    {
        Value = value;
    }
}