namespace Markdown.MarkdownTags;

/// <summary>
/// Одиночный markdown тэг
/// </summary>
public class MarkdownSingleTag : MarkdownTag
{
    public MarkdownSingleTag(MarkdownTagType tagType, int position) : base(tagType, position)
    {
    }

    public override int Length { get; }
}