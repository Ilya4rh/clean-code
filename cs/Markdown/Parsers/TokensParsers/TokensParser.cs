using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.TokensParsers;

public class TokensParser : ITokensParser
{
    private readonly IMarkdownTagValidator validator;

    public TokensParser(IMarkdownTagValidator validator)
    {
        this.validator = validator;
    }

    public IEnumerable<MarkdownTag> ParserMarkdownTags(List<IToken> paragraphOfTokens)
    {
        var tagTokens = GetTagTokens(paragraphOfTokens).ToList();
        
        var singleTags = ParseSingleTags(tagTokens, paragraphOfTokens);
        var pairedTags = ParsePairedTags(tagTokens, paragraphOfTokens);

        var tags = new List<MarkdownTag>();
        tags.AddRange(singleTags);
        tags.AddRange(pairedTags);

        return tags;
    }

    private IEnumerable<MarkdownTag> ParseSingleTags(
        IEnumerable<TagToken> tagTokens, 
        List<IToken> paragraphOfTokens)
    {
        foreach (var tagToken in tagTokens)
        {
            if (!SingleMarkdownTagToken.TryCreate(tagToken, out var singleMarkdownTagToken))
            {
                continue;
            }
            
            if (validator.IsValidSingleTag(paragraphOfTokens, singleMarkdownTagToken!))
            {
                yield return new MarkdownTag(tagToken, singleMarkdownTagToken!.TagType);
            }
        }
    }

    private IEnumerable<MarkdownTag> ParsePairedTags(
        List<TagToken> tagTokens,
        List<IToken> paragraphOfTokens)
    {
        var positionAddedTokens = new HashSet<int>();
        PairedMarkdownTagTokens? previousPairedTags = null;

        for (var i = 0; i < tagTokens.Count; i++)
        {
            if (!positionAddedTokens.Add(i))
                continue;

            for (var j = i + 1; j < tagTokens.Count; j++)
            {
                if (positionAddedTokens.Contains(j))
                    continue;

                var isComplete = TryCompleteValidPairedMarkdownTagTokens(
                    paragraphOfTokens,
                    previousPairedTags,
                    tagTokens[i],
                    tagTokens[j],
                    out var currentPairedTags);

                if (previousPairedTags == null)
                {
                    if (isComplete)
                    {
                        previousPairedTags = currentPairedTags;
                        positionAddedTokens.Add(j);
                        break;
                    }
                    continue;
                }

                if (isComplete && previousPairedTags.IsIntersect(currentPairedTags!))
                {
                    previousPairedTags = null;
                    continue;
                }
                
                yield return new MarkdownTag(previousPairedTags.Opening, previousPairedTags.TagType);
                yield return new MarkdownTag(previousPairedTags.Closing, previousPairedTags.TagType, true);
                previousPairedTags = currentPairedTags;
                positionAddedTokens.Add(j);
                break;
            }
        }

        if (previousPairedTags == null) 
            yield break;
        
        yield return new MarkdownTag(previousPairedTags.Opening, previousPairedTags.TagType);
        yield return new MarkdownTag(previousPairedTags.Closing, previousPairedTags.TagType, true);
    }

    private bool TryCompleteValidPairedMarkdownTagTokens(
        List<IToken> paragraphOfTokens,
        PairedMarkdownTagTokens? previousPairedTags,
        TagToken openingTagToken,
        TagToken closingTagToken,
        out PairedMarkdownTagTokens? pairedMarkdownTagTokens)
    {
        if (!PairedMarkdownTagTokens.TryCreate(openingTagToken, closingTagToken, out pairedMarkdownTagTokens))
        {
            return false;
        }

        MarkdownTagType? externalTagType =
            previousPairedTags != null && previousPairedTags.IsExternalFor(pairedMarkdownTagTokens!)
                ? previousPairedTags.TagType
                : null;

        if (!validator.IsValidPairedTag(paragraphOfTokens, pairedMarkdownTagTokens!, externalTagType))
            return false;
        
        return true;
    }
    
    private static IEnumerable<TagToken> GetTagTokens(IEnumerable<IToken> tokens)
    {
        return tokens
            .Where(token => token.Type == TokenType.Tag)
            .Select(token => (token as TagToken)!);
    }
}