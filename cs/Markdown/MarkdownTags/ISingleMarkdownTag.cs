using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public interface ISingleMarkdownTag : IMarkdownTag
{
    TagToken Token { get; }
}