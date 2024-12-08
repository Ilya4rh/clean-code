using Markdown.MarkdownTags;
using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public class PairedMarkdownTagTokens : IPairedMarkdownTagTokens
{
    public PairedMarkdownTagTokens(MarkdownTagType tagType, TagToken opening, TagToken closing)
    {
        TagType = tagType;
        Opening = opening;
        Closing = closing;
    }

    public MarkdownTagType TagType { get; }
    
    public TagToken Opening { get; }
    
    public TagToken Closing { get; }
    
    public bool IsIntersect(IPairedMarkdownTagTokens pairedMarkdownTagTokens)
    {
        if (IsExternalFor(pairedMarkdownTagTokens) || pairedMarkdownTagTokens.IsExternalFor(this))
        {
            return false;
        }

        return (pairedMarkdownTagTokens.Opening.PositionInTokens < Closing.PositionInTokens &&
                Closing.PositionInTokens < pairedMarkdownTagTokens.Closing.PositionInTokens) || 
               (Opening.PositionInTokens < pairedMarkdownTagTokens.Closing.PositionInTokens && 
                pairedMarkdownTagTokens.Closing.PositionInTokens < Closing.PositionInTokens);
    }

    public bool IsExternalFor(IPairedMarkdownTagTokens pairedMarkdownTagTokens)
    {
        return Opening.PositionInTokens < pairedMarkdownTagTokens.Opening.PositionInTokens &&
               pairedMarkdownTagTokens.Closing.PositionInTokens < Closing.PositionInTokens;
    }

    public static bool TryCreate(
        TagToken opening,
        TagToken closing,
        out PairedMarkdownTagTokens? pairedMarkdownTagTokens)
    {
        if (opening.Content != closing.Content)
        {
            pairedMarkdownTagTokens = null;
            return false;
        }

        switch (opening.Content)
        {
            case BoldTagToken.BoldTokenContent:
                pairedMarkdownTagTokens = new PairedMarkdownTagTokens(MarkdownTagType.Bold, opening, closing);
                return true;
            case ItalicsTagToken.ItalicsTokenContent:
                pairedMarkdownTagTokens = new PairedMarkdownTagTokens(MarkdownTagType.Italics, opening, closing);
                return true;
            default:
                pairedMarkdownTagTokens = null;
                return false;
        }
    }
}