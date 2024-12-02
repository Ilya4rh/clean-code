using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class MarkdownBoldTag : MarkdownTag
{
    public MarkdownBoldTag(TagToken tagToken, bool isClosedTag = false)
    {
        TagToken = tagToken;
        IsClosedTag = isClosedTag;
    }

    // public override MarkdownTagType TagType => MarkdownTagType.Bold;
    //
    // public override int Position { get; }
    //
    // public override int Length => 2;

    public override TagToken TagToken { get; }
    public override bool IsClosedTag { get; }
}