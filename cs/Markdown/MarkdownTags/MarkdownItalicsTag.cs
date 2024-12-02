using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class MarkdownItalicsTag : MarkdownTag
{
    public MarkdownItalicsTag(TagToken tagToken, bool isClosedTag)
    {
        TagToken = tagToken;
        IsClosedTag = isClosedTag;
    }

    // public override MarkdownTagType TagType => MarkdownTagType.Italics;
    //
    // public override int Position { get; }
    //
    // public override int Length => 1;

    public override TagToken TagToken { get; }
    public override bool IsClosedTag { get; }
}