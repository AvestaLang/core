namespace Core.Semantic;

public sealed class PropertySchema
{
    public string Name { get; }

    public ValueType Type { get; }

    public bool Required { get; }

    public object? DefaultValue { get; }

    public string? HtmlAttribute { get; }

    public PropertySchema(
        string name,
        ValueType type,
        bool required,
        object? defaultValue = null,
        string? htmlAttribute = null
    {
        Name = name;
        Type = type;
        Required = required;
        DefaultValue = defaultValue;
        HtmlAttribute = htmlAttribute;
    }
}