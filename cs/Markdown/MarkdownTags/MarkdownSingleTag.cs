namespace Markdown.MarkdownTags;

/// <summary>
/// Одиночный markdown тэг
/// </summary>
public class MarkdownSingleTag : MarkdownTag
{
    public MarkdownSingleTag(MarkdownTagType tagType, int position, int length) : base(tagType, position, length)
    {
    }
}