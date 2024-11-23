namespace Markdown.MarkdownTags;

public abstract class MarkdownTag
{
    // Тип markdown тэга
    public MarkdownTagType TagType;
    // Позиция тэга в строке
    public int Position;
    
    public MarkdownTag(MarkdownTagType tagType, int position)
    {
        TagType = tagType;
        Position = position;
    }

    /// <summary>
    /// Длина тэга
    /// </summary>
    public abstract int Length { get; }
}