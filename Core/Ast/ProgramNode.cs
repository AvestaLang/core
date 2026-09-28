namespace Core.Ast;

public class ProgramNode : AstNode
{
    public List<ElementNode> Elements { get; }

    public ProgramNode(List<ElementNode> elements)
    {
        Elements = elements;
    }
}