using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public interface IPairedMarkdownTags
{
    TagToken OpeningToken { get; }
    
    TagToken ClosingToken { get; }

    bool IsIntersect(IPairedMarkdownTags pairedMarkdownTags);

    bool IsExternal(IPairedMarkdownTags pairedMarkdownTags);
}