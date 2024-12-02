using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class BoldTagToken : TagToken
{
    public override MarkdownTagType TagType => MarkdownTagType.Bold;

    public override string Content => "__";

    public override bool IsTagToken(string line, int position)
    {
        if (line.Length < Content.Length)
            return false;

        return line.Substring(position, Content.Length) == Content;
    }
}