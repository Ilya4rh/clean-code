using Markdown.Tokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public interface IMarkdownLineParser
{
    IEnumerable<IToken> ParseParagraphForTokens(string markdownText);

    IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownLine);
}