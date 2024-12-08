using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public interface IPairedMarkdownTagTokens : IMarkdownTagToken
{
    TagToken Opening { get; }
    
    TagToken Closing { get; }

    bool IsIntersect(IPairedMarkdownTagTokens pairedMarkdownTagTokens);

    bool IsExternalFor(IPairedMarkdownTagTokens pairedMarkdownTagTokens);
}