using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class PairedMarkdownTags : IPairedMarkdownTags
{
    public PairedMarkdownTags(TagToken openingToken, TagToken closingToken)
    {
        OpeningToken = openingToken;
        ClosingToken = closingToken;
    }

    public TagToken OpeningToken { get; }
    public TagToken ClosingToken { get; }
    
    public bool IsIntersect(IPairedMarkdownTags pairedMarkdownTags)
    {
        throw new NotImplementedException();
    }

    public bool IsExternal(IPairedMarkdownTags pairedMarkdownTags)
    {
        throw new NotImplementedException();
    }
}