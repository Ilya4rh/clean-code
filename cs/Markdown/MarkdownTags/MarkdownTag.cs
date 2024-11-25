namespace Markdown.MarkdownTags;

public abstract class MarkdownTag
{
    // Тип markdown тэга
    public MarkdownTagType TagType;
    // Позиция тэга в строке
    public int Position;
    // Длина тэга
    public int Length;
    
    public MarkdownTag(MarkdownTagType tagType, int position, int length)
    {
        TagType = tagType;
        Position = position;
        Length = length;
    }
}