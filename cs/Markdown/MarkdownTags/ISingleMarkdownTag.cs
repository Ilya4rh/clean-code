using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public interface ISingleMarkdownTag
{
    TagToken Token { get; }
}