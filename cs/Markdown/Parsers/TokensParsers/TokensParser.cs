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
        var positionAddedTokens = new HashSet<int>();
        IPairedMarkdownTagTokens? previousPaired = null;
        var tags = new List<MarkdownTag>();

        for (var i = 0; i < tagTokens.Count; i++)
        {
            if (!positionAddedTokens.Add(i)) continue;
            
            if (TryCreateSingleMarkdownTag(paragraphOfTokens, tagTokens[i], out var markdownTag))
            {
                tags.Add(markdownTag!);
                continue;
            }

            for (var j = i + 1; j < tagTokens.Count; j++)
            {
                if (positionAddedTokens.Contains(j)) continue;
                
                if (!PairedMarkdownTagTokens.TryCreate(tagTokens[i], tagTokens[j], out var currentPaired) || 
                    !IsValidPairedMarkdownTagTokens(paragraphOfTokens, previousPaired, currentPaired!))
                    continue;

                positionAddedTokens.Add(j);
                
                if (previousPaired != null && previousPaired.IsIntersect(currentPaired!))
                {
                    previousPaired = null;
                    continue;
                }
                if (previousPaired != null) tags.AddRange(previousPaired.ConvertToMarkdownTags());
                
                previousPaired = currentPaired;
            }
        }

        if (previousPaired != null) tags.AddRange(previousPaired.ConvertToMarkdownTags());
        
        return tags;
    }

    private bool TryCreateSingleMarkdownTag(
        List<IToken> paragraphOfTokens, 
        TagToken tagToken,
        out MarkdownTag? markdownTag)
    {
        if (!SingleMarkdownTagToken.TryCreate(tagToken, out var singleMarkdownTagToken) || 
            !validator.IsValidSingleTag(paragraphOfTokens, singleMarkdownTagToken!))
        {
            markdownTag = null;
            return false;
        }
            
        markdownTag = singleMarkdownTagToken!.ConvertToMarkdownTag();
        return true;
    }

    private bool IsValidPairedMarkdownTagTokens(
        List<IToken> paragraphOfTokens,
        IPairedMarkdownTagTokens? previousPaired,
        IPairedMarkdownTagTokens currentPaired)
    {
        MarkdownTagType? externalTagType = null;
        
        if (previousPaired != null && previousPaired.IsExternalFor(currentPaired))
        {
            externalTagType = previousPaired.TagType;
        }

        return validator.IsValidPairedTag(paragraphOfTokens, currentPaired, externalTagType);
    }

    private static IEnumerable<TagToken> GetTagTokens(IEnumerable<IToken> tokens)
    {
        return tokens
            .Where(token => token.Type == TokenType.Tag)
            .Select(token => (token as TagToken)!);
    }
}