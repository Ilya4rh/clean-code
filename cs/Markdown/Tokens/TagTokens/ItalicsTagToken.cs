namespace Markdown.Tokens.TagTokens;

public class ItalicsTagToken : TagToken
{
    public ItalicsTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public const string ItalicsTokenContent = "_";

    public override string Content => ItalicsTokenContent;
    
    public override int PositionInTokens { get; }

    public static bool IsItalicsTagToken(string line, int position)
    {
        return line[position].ToString() == ItalicsTokenContent;
    }
}