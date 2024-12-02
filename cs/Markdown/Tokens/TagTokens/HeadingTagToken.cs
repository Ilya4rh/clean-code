using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class HeadingTagToken : TagToken
{
    public override MarkdownTagType TagType => MarkdownTagType.Heading;

    public override string Content => "#";

    public override bool IsTagToken(string line, int position)
    {
        return line[position].ToString() == Content;
    }
}