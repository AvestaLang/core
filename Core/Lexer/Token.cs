namespace Core.Lexer;

/// <summary>
/// ساختار خروجی ای که یک توکن پیدا شده از متن میتونه داشته باشه
/// اینجا در غالب این خروجی تعیین میشه
/// یعنی هر توکنی که در نهایت از مرحله لکسر میتونه پیدا بشه در این غالب خروجی داده میشه
/// </summary>
public sealed class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Line { get; }
    public int Column { get; }

    public Token(
        TokenType type,
        string value,
        int line,
        int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }
}