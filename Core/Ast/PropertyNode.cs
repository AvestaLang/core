using Core.Ast.Values;

namespace Core.Ast;

public sealed class PropertyNode : AstNode
{
    public string Name { get; }
    public ValueNode Value { get; }

    public PropertyNode(
        string name,
        ValueNode value,
        SourceLocation location)
        : base(location)
    {
        Name = name;
        Value = value;
    }
}