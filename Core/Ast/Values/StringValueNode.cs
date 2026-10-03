namespace Core.Ast.Values;

public sealed class StringValueNode : ValueNode
{
    public string Value { get; }

    public StringValueNode(
        string value,
        SourceLocation location)
        : base(location)
    {
        Value = value;
    }
}