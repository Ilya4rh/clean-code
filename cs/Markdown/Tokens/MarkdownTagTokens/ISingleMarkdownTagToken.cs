using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public interface ISingleMarkdownTagToken : IMarkdownTagToken
{
    TagToken Token { get; }
}