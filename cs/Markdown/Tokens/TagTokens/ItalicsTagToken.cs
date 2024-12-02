using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class ItalicsTagToken : TagToken
{
    public override MarkdownTagType TagType => MarkdownTagType.Italics;

    public override string Content => "_";

    public override bool IsTagToken(string line, int position)
    {
        return line[position].ToString() == Content;
    }
}