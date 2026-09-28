namespace Core.Ast;

public class PropertyNode : AstNode
{
    public string Name { get; }
    public object Value { get; }

    public PropertyNode(string name, object value)
    {
        Name = name;
        Value = value;
    }
}