using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;

namespace Core.Lexer;

public static class Lexer
{
    private static string _source;

    private static int 
        _position = 0,
        _line = 1,
        _column = 1;


    public static List<Token> Tokenize(string source)
    {
        List<Token> tokens = new List<Token>();
        _source = source;

        while(_position <= _source.Length - 1)
        {
            char character = Peek();

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

        return tokens;
    }

    private static Token ReadString()
    {
        string text = string.Empty;

        while (_source[_position] != '»') {
            text += Peek();
        }


        return new Token(
            type: TokenType.String,
            value: text,
            line: _line,
            column: _column
        );
    }

    private static Token ReadNumber(char first)
    {
        string text = first.ToString();

        while (char.IsDigit(_source[_position]))
            text += Peek();

        return new Token(
            type: TokenType.Number,
            value: text,
            line: _line,
            column: _column
        );
    }

    private static Token ReadIdentifier(char first)
    {
        string text = first.ToString();

        char character = _source[_position];

        while ((char.IsLetterOrDigit(_source[_position]) || _source[_position] == '_'))
            text += Peek();

        return new Token(
            type: TokenType.Identifier,
            value: text,
            line: _line,
            column: _column
        );
    }

    private static char Peek()
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