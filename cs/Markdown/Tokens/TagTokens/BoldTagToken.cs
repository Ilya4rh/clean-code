using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class BoldTagToken : TagToken
{
    public BoldTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    private const string BoldTag = "__";
    
    public override MarkdownTagType MarkdownTagType => MarkdownTagType.Bold;

    public override string Content => BoldTag;
    
    public override int PositionInTokens { get; }

    public static bool IsBoldTagToken(string line, int position)
    {
        if (line.Length - position < BoldTag.Length)
            return false;

        return line.Substring(position, BoldTag.Length) == BoldTag;
    }
}