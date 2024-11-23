namespace Markdown.MarkdownTags;

/// <summary>
/// Парный markdown тэг
/// </summary>
public class MarkdownPairedTag : MarkdownTag
{
    public bool IsClosedTag;
    
    public MarkdownPairedTag(MarkdownTagType tagType, int position, bool isClosedTag = false) : base(tagType, position)
    {
        IsClosedTag = isClosedTag;
    }

    public override int Length { get; }
}