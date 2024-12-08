namespace Markdown.Tokens.TagTokens;

public class HeadingTagToken : TagToken
{
    public HeadingTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public const string HeadingTokenContent = "#";

    public override string Content => HeadingTokenContent;
    
    public override int PositionInTokens { get; }

    public static bool IsHeadingTagToken(string line, int position)
    {
        return line[position].ToString() == HeadingTokenContent;
    }
}