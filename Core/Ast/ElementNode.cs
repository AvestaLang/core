namespace Core.Ast;

public sealed class ElementNode : AstNode
{
    public string Name { get; }

    public List<PropertyNode> Properties { get; }

    public List<ElementNode> Children { get; }

    public ElementNode(
        string name,
        List<PropertyNode> properties,
        List<ElementNode> children,
        SourceLocation location)
        : base(location)
    {
        Name = name;
        Properties = properties;
        Children = children;
    }
}