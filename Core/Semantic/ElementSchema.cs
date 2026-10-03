namespace Core.Semantic;

public sealed class ElementSchema
{
    public string Name { get; }

    public string? HtmlTag { get; }

    public bool IsDocument { get; }

    public IReadOnlyList<PropertySchema> Properties { get; }

    public IReadOnlyList<string> AllowedChildren { get; }

    public ElementSchema(
        string name,
        string? htmlTag,
        IReadOnlyList<PropertySchema> properties,
        IReadOnlyList<string> allowedChildren,
        bool isDocument = false)
    {
        Name = name;
        HtmlTag = htmlTag;
        Properties = properties;
        AllowedChildren = allowedChildren;
        IsDocument = isDocument;
    }
}