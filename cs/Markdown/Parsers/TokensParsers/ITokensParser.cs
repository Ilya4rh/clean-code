using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown.Parsers.TokensParsers;

public interface ITokensParser
{
    IEnumerable<MarkdownTag> ParserMarkdownTags(List<IToken> paragraphOfTokens);
}