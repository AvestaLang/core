using System.Net.Http.Headers;

namespace Core.Semantic;

public static class LanguageSchema
{
    public static IReadOnlyDictionary<string, ElementSchema> Elements { get; } =
        new Dictionary<string, ElementSchema>
        {
            ["صفحه"] = new ElementSchema(
                name: "صفحه",
                htmlTag: null,
                properties: Array.Empty<PropertySchema>(),
                allowedChildren: new[]
                {
                    "عنوان",
                    "دکمه"
                },
                isDocument: true
            ),
        };
}