using Core.Ast;
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

        return new ProgramNode(elements);
    }

    private ElementNode ParseElement()
    {
        var name = Consume(
            TokenType.Identifier,
            "Expected element name."
        );

        Consume(
            TokenType.OpenBrace,
            "Expected '{' after element name."
        );

        var properties = new List<PropertyNode>();
        var children = new List<ElementNode>();

        while (!Check(TokenType.CloseBrace) && !IsAtEnd())
        {
            if (!Check(TokenType.Identifier))
                throw Error("Expected property or element.");

            var next = PeekNext();

            if (next.Type == TokenType.Colon)
            {
                properties.Add(ParseProperty());
                continue;
            }

            if (next.Type == TokenType.OpenBrace)
            {
                children.Add(ParseElement());
                continue;
            }

            throw Error(
                $"Expected ':' or '{{' after '{Peek().Value}'."
            );
        }

        Consume(
            TokenType.CloseBrace,
            "Expected '}' after element."
        );

        return new ElementNode(
            name.Value,
            properties,
            children
        );
    }

    private PropertyNode ParseProperty()
    {
        var name = Consume(
            TokenType.Identifier,
            "Expected property name."
        );

        Consume(
            TokenType.Colon,
            "Expected ':' after property name."
        );

        var value = ParseValue();

        return new PropertyNode(
            name.Value,
            value
        );
    }

    private object ParseValue()
    {
        if (Check(TokenType.String))
            return Advance().Value;

        if (Check(TokenType.Number))
            return int.Parse(Advance().Value);

        if (Check(TokenType.Identifier))
            return Advance().Value;

        throw Error("Expected property value.");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type))
            return Advance();

        throw Error(message);
    }

    private Token Advance()
    {
        if (!IsAtEnd())
            _position++;

        return Previous();
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

    private Exception Error(string message)
    {
        var token = Peek();

        return new Exception(
            $"{message} At {token.Line}:{token.Column}."
        );
    }
}