using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.TokensParsers;

public class TokensParser : ITokensParser
{
    private readonly IMarkdownTagValidator validator;

    public TokensParser(IMarkdownTagValidator validator)
    {
        this.validator = validator;
    }

    public IEnumerable<MarkdownTag> ParserMarkdownTags(IEnumerable<IToken> paragraphOfTokens)
    {
        var tagTokens = GetTagTokens(paragraphOfTokens).ToList();

        for (var i = 0; i < tagTokens.Count; i++)
        {
             
        }

        return [];
    }

    private static IEnumerable<TagToken> GetTagTokens(IEnumerable<IToken> tokens)
    {
        return tokens
            .Where(token => token.Type == TokenType.Tag)
            .Select(token => (token as TagToken)!);
    }
}