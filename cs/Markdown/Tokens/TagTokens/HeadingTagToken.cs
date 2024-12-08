using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class HeadingTagToken : TagToken
{
    public HeadingTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    private const string HeadingTag = "#";

    public override string Content => HeadingTag;
    
    public override int PositionInTokens { get; }

    public static bool IsHeadingTagToken(string line, int position)
    {
        return line[position].ToString() == HeadingTag;
    }
}