using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class SingleMarkdownTag : ISingleMarkdownTag
{
    public SingleMarkdownTag(TagToken token)
    {
        Token = token;
    }

    public TagToken Token { get; }
}