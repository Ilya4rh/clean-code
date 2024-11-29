namespace Markdown.MarkdownTags;

public class MarkdownTag
{
    // Тип markdown тэга
    public MarkdownTagType TagType;
    // Позиция тэга в строке
    public int Position;
    // Длина тэга
    public int Length;
    // Если тэг закрывающий
    public bool IsClosedTag;
    
    public MarkdownTag(MarkdownTagType tagType, int position, int length, bool isClosedTag = false)
    {
        TagType = tagType;
        Position = position;
        Length = length;
        IsClosedTag = isClosedTag;
    }
}