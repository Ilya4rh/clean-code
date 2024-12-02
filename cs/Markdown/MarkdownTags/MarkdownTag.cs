using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public abstract class MarkdownTag
{
    // // Тип markdown тэга
    // public abstract MarkdownTagType TagType { get; }
    // // Позиция тэга в строке
    // public abstract int Position { get; }
    // // Длина тэга
    // public abstract int Length { get; }
    public abstract TagToken TagToken { get; }
    // Если тэг закрывающий
    public abstract bool IsClosedTag { get; }

    public static MarkdownTag? CreateMarkdownTag(TagToken tagToken, bool isClosedTag = false)
    {
        return tagToken.MarkdownTagType switch
        {
            MarkdownTagType.Italics => new MarkdownItalicsTag(tagToken, isClosedTag),
            MarkdownTagType.Bold => new MarkdownBoldTag(tagToken, isClosedTag),
            MarkdownTagType.Heading => new MarkdownHeadingTag(tagToken),
            _ => null
        };
    }
}