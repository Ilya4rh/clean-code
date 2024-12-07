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
        if (IsExternalFor(pairedMarkdownTags) || pairedMarkdownTags.IsExternalFor(this))
        {
            return false;
        }

        return (pairedMarkdownTags.OpeningToken.PositionInTokens < ClosingToken.PositionInTokens &&
               ClosingToken.PositionInTokens < pairedMarkdownTags.ClosingToken.PositionInTokens) || 
               (OpeningToken.PositionInTokens < pairedMarkdownTags.ClosingToken.PositionInTokens && 
                pairedMarkdownTags.ClosingToken.PositionInTokens < ClosingToken.PositionInTokens);
    }

    public bool IsExternalFor(IPairedMarkdownTags pairedMarkdownTags)
    {
        return OpeningToken.PositionInTokens < pairedMarkdownTags.OpeningToken.PositionInTokens &&
               pairedMarkdownTags.ClosingToken.PositionInTokens < ClosingToken.PositionInTokens;
    }
}