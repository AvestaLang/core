namespace Core.Ast;

public abstract class AstNode
{
    public SourceLocation Location { get; }

    protected AstNode(SourceLocation location)
    {
        Location = location;
    }
}