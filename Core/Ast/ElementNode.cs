namespace Core.Ast;

public class ElementNode : AstNode
{
    public string Name { get; }
    public List<PropertyNode> Properties { get; }
    public List<ElementNode> Children { get; }

    public ElementNode(
        string name,
        List<PropertyNode> properties,
        List<ElementNode> children)
    {
        Name = name;
        Properties = properties;
        Children = children;
    }
}