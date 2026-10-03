using Core.Ast;
using Core.Ast.Values;
using Core.Errors;

namespace Core.Semantic;

public sealed class SemanticAnalyzer
{
    public void Analyze(ProgramNode program)
    {
        ValidateRoot(program);

        AnalyzeElement(
            program.Elements[0],
            null
        );
    }

    private void ValidateRoot(ProgramNode program)
    {
        if (program.Elements.Count != 1)
        {
            throw new AvestaException(
                new AvestaError(
                    "AVE3001",
                    "برنامه باید دقیقاً یک صفحه داشته باشد.",
                    program.Location.Line,
                    program.Location.Column
                )
            );
        }

        var root = program.Elements[0];

        if (root.Name != "صفحه")
        {
            throw new AvestaException(
                new AvestaError(
                    "AVE3002",
                    "عنصر ریشه باید «صفحه» باشد.",
                    root.Location.Line,
                    root.Location.Column
                )
            );
        }
    }

    private void AnalyzeElement(
        ElementNode element,
        ElementSchema? parent)
    {
        if (!LanguageSchema.Elements.TryGetValue(
                element.Name,
                out var schema))
        {
            throw new AvestaException(
                new AvestaError(
                    "AVE3003",
                    $"عنصر «{element.Name}» تعریف نشده است.",
                    element.Location.Line,
                    element.Location.Column
                )
            );
        }

        if (parent != null &&
            !parent.AllowedChildren.Contains(element.Name))
        {
            throw new AvestaException(
                new AvestaError(
                    "AVE3004",
                    $"عنصر «{element.Name}» نمی‌تواند داخل «{parent.Name}» قرار بگیرد.",
                    element.Location.Line,
                    element.Location.Column
                )
            );
        }

        ValidateProperties(
            element,
            schema
        );

        foreach (var child in element.Children)
        {
            AnalyzeElement(
                child,
                schema
            );
        }
    }

    private void ValidateProperties(
        ElementNode element,
        ElementSchema schema)
    {
        var definedProperties = new HashSet<string>();

        foreach (var property in element.Properties)
        {
            if (!definedProperties.Add(property.Name))
            {
                throw new AvestaException(
                    new AvestaError(
                        "AVE3005",
                        $"ویژگی «{property.Name}» برای عنصر «{element.Name}» تکراری است.",
                        property.Location.Line,
                        property.Location.Column
                    )
                );
            }

            var propertySchema = schema.Properties
                .FirstOrDefault(x => x.Name == property.Name);

            if (propertySchema == null)
            {
                throw new AvestaException(
                    new AvestaError(
                        "AVE3006",
                        $"ویژگی «{property.Name}» برای عنصر «{element.Name}» تعریف نشده است.",
                        property.Location.Line,
                        property.Location.Column
                    )
                );
            }

            ValidateValue(
                property,
                propertySchema
            );
        }

        ValidateRequiredProperties(
            element,
            schema,
            definedProperties
        );
    }

    private void ValidateRequiredProperties(
        ElementNode element,
        ElementSchema schema,
        HashSet<string> definedProperties)
    {
        foreach (var property in schema.Properties)
        {
            if (!property.Required)
                continue;

            if (definedProperties.Contains(property.Name))
                continue;

            throw new AvestaException(
                new AvestaError(
                    "AVE3007",
                    $"ویژگی «{property.Name}» برای عنصر «{element.Name}» الزامی است.",
                    element.Location.Line,
                    element.Location.Column
                )
            );
        }
    }

    private void ValidateValue(
        PropertyNode property,
        PropertySchema schema)
    {
        var actualType = GetValueType(
            property.Value
        );

        if (actualType == schema.Type)
            return;

        throw new AvestaException(
            new AvestaError(
                "AVE3008",
                $"نوع مقدار ویژگی «{property.Name}» باید «{schema.Type}» باشد.",
                property.Location.Line,
                property.Location.Column
            )
        );
    }

    private ValueType GetValueType(ValueNode value)
    {
        return value switch
        {
            StringValueNode => ValueType.String,
            NumberValueNode => ValueType.Number,
            BooleanValueNode => ValueType.Boolean,

            _ => throw new InvalidOperationException(
                $"Unknown value node: {value.GetType().Name}"
            )
        };
    }
}