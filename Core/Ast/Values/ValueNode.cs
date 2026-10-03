namespace Core.Ast.Values;

public abstract class ValueNode : AstNode
{
    protected ValueNode(SourceLocation location)
        : base(location)
    {
    }
}