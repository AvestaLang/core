namespace Core.Ast.Values;

public sealed class BooleanValueNode : ValueNode
{
    public bool Value { get; }

    public BooleanValueNode(
        bool value,
        SourceLocation location)
        : base(location)
    {
        Value = value;
    }
}