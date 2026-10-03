namespace Core.Ast;

/// <summary>
///    این کلاس برای این است که متوجه بشیم دقیقا یک قسمت 
///    ast
///    از کجا اومده و اگر ارور داشت بدونیم مربوط به چه فایلی هست
///    بدون این بخش اگر به اروری بخوریم نمیتونیم دقیق متوجه بشیم دقیقا مشکل از کجاست
/// </summary>
public sealed class SourceLocation
{
    public int Line { get; }
    public int Column { get; }

    public SourceLocation(int line, int column)
    {
        Line = line;
        Column = column;
    }
}