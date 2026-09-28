namespace Core.Lexer;

public sealed class Lexer
{
    private readonly string _source;
    private int _position;
    private int _line = 1;
    private int _column = 1;

    public Lexer(string source)
    {
        _source = NormalizeSpaces(source);
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (!IsAtEnd())
        {
            SkipWhitespace();

            if (IsAtEnd())
                break;

            var line = _line;
            var column = _column;
            var current = Advance();

            switch (current)
            {
                case '{':
                    tokens.Add(new(TokenType.OpenBrace, "{", line, column));
                    break;

                case '}':
                    tokens.Add(new(TokenType.CloseBrace, "}", line, column));
                    break;

                case ':':
                    tokens.Add(new(TokenType.Colon, ":", line, column));
                    break;

                case '«':
                    tokens.Add(ReadString(line, column));
                    break;

                default:
                    if (char.IsDigit(current))
                    {
                        tokens.Add(ReadNumber(current, line, column));
                        break;
                    }

                    if (char.IsLetter(current))
                    {
                        tokens.Add(ReadIdentifier(current, line, column));
                        break;
                    }

                    throw new Exception(
                        $"Unexpected character '{current}' -> code: {(int)current} at {line}:{column}"
                    );
            }
        }

        tokens.Add(new(
            TokenType.EndOfFile,
            string.Empty,
            _line,
            _column
        ));

        return tokens;
    }

    private Token ReadString(int line, int column)
    {
        var value = string.Empty;

        while (!IsAtEnd() && Peek() != '»')
            value += Advance();

        if (IsAtEnd())
            throw new Exception(
                $"Unterminated string at {line}:{column}"
            );

        Advance();

        return new(TokenType.String, value, line, column);
    }

    private Token ReadNumber(char first, int line, int column)
    {
        var value = first.ToString();

        while (!IsAtEnd() && char.IsDigit(Peek()))
            value += Advance();

        return new(TokenType.Number, value, line, column);
    }

    private Token ReadIdentifier(char first, int line, int column)
    {
        var value = first.ToString();

        while (!IsAtEnd() && (char.IsLetterOrDigit(Peek()) || Peek() == '_'))
            value += Advance();

        return new(TokenType.Identifier, value, line, column);
    }

    private void SkipWhitespace()
    {
        while (!IsAtEnd() && char.IsWhiteSpace(Peek()))
            Advance();
    }

    private char Advance()
    {
        var character = _source[_position++];

        if (character == '\n')
        {
            _line++;
            _column = 1;
        }
        else
        {
            _column++;
        }

        return character;
    }

    private char Peek()
    {
        return IsAtEnd() ? '\0' : _source[_position];
    }

    private bool IsAtEnd()
    {
        return _position >= _source.Length;
    }

    public string NormalizeSpaces(string value)
    {
        return value
            .Replace('\u200C', ' ')
            .Replace('\u200E', ' ')
            .Replace('\u200F', ' ');
    }
}