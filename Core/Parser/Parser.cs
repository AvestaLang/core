using Core.Ast;
using Core.Ast.Values;
using Core.Errors;
using Core.Lexer;

namespace Core.Parser;

public sealed class Parser
{
    private readonly List<Token> _tokens;
    private int _position;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public ProgramNode Parse()
    {
        var elements = new List<ElementNode>();

        while (!IsAtEnd())
        {
            elements.Add(ParseElement());
        }

        var location = elements.Count > 0
            ? elements[0].Location
            : new SourceLocation(1, 1);

        return new ProgramNode(
            elements,
            location
        );
    }

    private ElementNode ParseElement()
    {
        var name = Consume(
            TokenType.Identifier,
            "نام عنصر مورد انتظار است."
        );

        var location = ToLocation(name);

        Consume(
            TokenType.OpenBracket,
            "بعد از نام عنصر باید '{' قرار بگیرد."
        );

        var properties = new List<PropertyNode>();
        var children = new List<ElementNode>();

        while (!Check(TokenType.CloseBracket) && !IsAtEnd())
        {
            if (!Check(TokenType.Identifier))
            {
                throw Error(
                    "انتظار نام ویژگی یا عنصر می‌رود."
                );
            }

            if (PeekNext().Type == TokenType.Colon)
            {
                properties.Add(ParseProperty());
                continue;
            }

            if (PeekNext().Type == TokenType.OpenBracket)
            {
                children.Add(ParseElement());
                continue;
            }

            throw Error(
                $"بعد از '{Peek().Value}' باید ':' یا '{{' قرار بگیرد."
            );
        }

        Consume(
            TokenType.CloseBracket,
            "بعد از عنصر باید '}' قرار بگیرد."
        );

        return new ElementNode(
            name.Value,
            properties,
            children,
            location
        );
    }

    private PropertyNode ParseProperty()
    {
        var name = Consume(
            TokenType.Identifier,
            "نام ویژگی مورد انتظار است."
        );

        var location = ToLocation(name);

        Consume(
            TokenType.Colon,
            "بعد از نام ویژگی باید ':' قرار بگیرد."
        );

        var value = ParseValue();

        return new PropertyNode(
            name.Value,
            value,
            location
        );
    }

    private ValueNode ParseValue()
    {
        if (Check(TokenType.String))
            return ParseStringValue();

        if (Check(TokenType.Number))
            return ParseNumberValue();

        if (Check(TokenType.Boolean))
            return ParseBooleanValue();

        throw Error(
            "مقدار معتبر مورد انتظار است."
        );
    }

    private StringValueNode ParseStringValue()
    {
        var token = Advance();

        return new StringValueNode(
            token.Value,
            ToLocation(token)
        );
    }

    private NumberValueNode ParseNumberValue()
    {
        var token = Advance();

        if (!double.TryParse(
                token.Value,
                out var value))
        {
            throw new AvestaException(
                new AvestaError(
                    "AVE1003",
                    $"عدد '{token.Value}' معتبر نیست.",
                    token.Line,
                    token.Column
                )
            );
        }

        return new NumberValueNode(
            value,
            ToLocation(token)
        );
    }

    private BooleanValueNode ParseBooleanValue()
    {
        var token = Advance();

        var value = token.Value switch
        {
            "درست" => true,
            "نادرست" => false,
            _ => throw new AvestaException(
                new AvestaError(
                    "AVE1004",
                    $"مقدار بولی '{token.Value}' معتبر نیست.",
                    token.Line,
                    token.Column
                )
            )
        };

        return new BooleanValueNode(
            value,
            ToLocation(token)
        );
    }

    private Token Consume(
        TokenType type,
        string message)
    {
        if (Check(type))
            return Advance();

        throw Error(message);
    }

    private Token Advance()
    {
        var token = Peek();

        if (!IsAtEnd())
            _position++;

        return token;
    }

    private Token Previous()
    {
        return _tokens[_position - 1];
    }

    private Token Peek()
    {
        return _tokens[_position];
    }

    private Token PeekNext()
    {
        if (_position + 1 >= _tokens.Count)
            return _tokens[^1];

        return _tokens[_position + 1];
    }

    private bool Check(TokenType type)
    {
        return Peek().Type == type;
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EndOfFile;
    }

    private SourceLocation ToLocation(Token token)
    {
        return new SourceLocation(
            token.Line,
            token.Column
        );
    }

    private AvestaException Error(string message)
    {
        var token = Peek();

        return new AvestaException(
            new AvestaError(
                "AVE1001",
                message,
                token.Line,
                token.Column
            )
        );
    }
}