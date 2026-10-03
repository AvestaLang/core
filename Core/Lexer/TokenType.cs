namespace Core.Lexer;

/// <summary>
/// هر قسمت از یک متن قراره به یک تایپ تبدیل بشه 
/// برای مثال کلمه صفحه تایپ 
/// Identifier
/// را به خود اختصاص میدهد 
/// اینجا لیست تایپ هارو میتونین ببینین
/// </summary>
public enum TokenType
{
    Identifier,

    String,
    Number,
    Boolean,

    OpenBracket,
    CloseBracket,

    Colon,

    EndOfFile
}