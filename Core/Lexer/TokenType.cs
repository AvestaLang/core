namespace Core.Lexer;

public enum TokenType
{
    Identifier,
    String,
    Number,

    OpenBrace,
    CloseBrace,
    Colon,

    EndOfFile
}