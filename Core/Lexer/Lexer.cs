using Core.Ast.Values;

namespace Core.Lexer;

public class Lexer
{
    private string _source;

    public Lexer(string source)
    {
        _source = source;
    }

    private int 
        _position = 0,
        _line = 1,
        _column = 1;


    public List<Token> Tokenize()
    {
        List<Token> tokens = new List<Token>();
        
        while(_position <= _source.Length - 1)
        {
            char character = Advance();

            if (character == ' ')
            {
                continue;
            }

            switch(character)
            {
                case '«':
                    tokens.Add(ReadString());
                    break;
                case '{':
                    tokens.Add(new Token(
                        type: TokenType.OpenBracket,
                        value: character.ToString(),
                        line: _line,
                        column: _column
                    ));
                    break;
                case '}':
                    tokens.Add(new Token(
                        type: TokenType.CloseBracket,
                        value: character.ToString(),
                        line: _line,
                        column: _column
                    ));
                    break;
                case ':':
                    tokens.Add(new Token(
                        type: TokenType.Colon,
                        value: character.ToString(),
                        line: _line,
                        column: _column
                    ));
                    break;
                
                default:
                    if (char.IsDigit(character))
                    {
                        tokens.Add(ReadNumber(character));
                        break;
                    }

                    if (char.IsLetter(character))
                    {
                        tokens.Add(ReadIdentifier(character));
                        break;
                    }

                    break;
            }
        }

        tokens.Add(new Token(
            type: TokenType.EndOfFile,
            value: "",
            line: _line,
            column: _column
        ));

        return tokens;
    }

    private Token ReadString()
    {
        string text = string.Empty;

        while (_source[_position] != '»') {
            text += Advance();
        }


        return new Token(
            type: TokenType.String,
            value: text,
            line: _line,
            column: _column
        );
    }

    private Token ReadNumber(char first)
    {
        string text = first.ToString();

        while (char.IsDigit(_source[_position]))
            text += Advance();

        return new Token(
            type: TokenType.Number,
            value: text,
            line: _line,
            column: _column
        );
    }

    private Token ReadIdentifier(char first)
    {
        string text = first.ToString();

        char character = _source[_position];

        while ((char.IsLetterOrDigit(_source[_position]) || _source[_position] == '_'))
            text += Advance();

        if(text == "درست" || text == "نادرست")
        {
            return new Token(
                type: TokenType.Boolean,
                value: text,
                line: _line,
                column: _column
            );
        }

        return new Token(
            type: TokenType.Identifier,
            value: text,
            line: _line,
            column: _column
        );
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
}