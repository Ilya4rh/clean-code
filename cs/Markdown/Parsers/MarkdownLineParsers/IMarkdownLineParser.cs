using Markdown.Tokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public interface IMarkdownLineParser
{
    IEnumerable<IToken> ParseMarkdownLineForTokens(string markdownText);
}