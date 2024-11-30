namespace Markdown.MarkdownTags;

public class MarkdownTag
{
    // Тип markdown тэга
    public readonly MarkdownTagType TagType;
    // Позиция тэга в строке
    public readonly int Position;
    // Длина тэга
    public readonly int Length;
    // Если тэг закрывающий
    public readonly bool IsClosedTag;
    
    public MarkdownTag(MarkdownTagType tagType, int position, int length, bool isClosedTag = false)
    {
        TagType = tagType;
        Position = position;
        Length = length;
        IsClosedTag = isClosedTag;
    }
}