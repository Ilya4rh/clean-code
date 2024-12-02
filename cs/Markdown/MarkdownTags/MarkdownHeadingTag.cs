using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class MarkdownHeadingTag : MarkdownTag
{
    public MarkdownHeadingTag(TagToken tagToken)
    {
        TagToken = tagToken;
    }

    // public override MarkdownTagType TagType => MarkdownTagType.Heading;
    //
    // public override int Position { get; }
    //
    // public override int Length => 1;

    public override TagToken TagToken { get; }
    public override bool IsClosedTag => false;
}