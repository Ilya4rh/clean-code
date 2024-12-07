using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown.Parsers.TokensParsers;

public interface ITokensParser
{
    IEnumerable<IMarkdownTag> ParserMarkdownTags(List<IToken> paragraphOfTokens);
}