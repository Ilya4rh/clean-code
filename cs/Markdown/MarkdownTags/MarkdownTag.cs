using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class MarkdownTag
{
    public MarkdownTag(TagToken token, bool isClosedTag)
    {
        Token = token;
        IsClosedTag = isClosedTag;
    }

    // Токен соответствующий данному тегу
    public TagToken Token { get; }
    
    // Если тэг закрывающий
    public bool IsClosedTag { get; }
}