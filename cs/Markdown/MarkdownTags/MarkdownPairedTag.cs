namespace Markdown.MarkdownTags;

/// <summary>
/// Парный markdown тэг
/// </summary>
public class MarkdownPairedTag : MarkdownTag
{
    public bool IsClosedTag;
    
    public MarkdownPairedTag(MarkdownTagType tagType, int position, int length, bool isClosedTag = false) :
        base(tagType, position, length)
    {
        IsClosedTag = isClosedTag;
    }
}