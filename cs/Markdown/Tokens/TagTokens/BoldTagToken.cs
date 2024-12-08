namespace Markdown.Tokens.TagTokens;

public class BoldTagToken : TagToken
{
    public BoldTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    public const string BoldTokenContent = "__";

    public override string Content => BoldTokenContent;
    
    public override int PositionInTokens { get; }

    public static bool IsBoldTagToken(string line, int position)
    {
        if (line.Length - position < BoldTokenContent.Length)
            return false;

        return line.Substring(position, BoldTokenContent.Length) == BoldTokenContent;
    }
}