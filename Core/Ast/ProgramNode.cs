namespace Core.Ast;

public sealed class ProgramNode : AstNode
{
    public List<ElementNode> Elements { get; }

    public ProgramNode(
        List<ElementNode> elements,
        SourceLocation location)
        : base(location)
    {
        Elements = elements;
    }
}