using Markdown.Converters;
using Markdown.Parsers.MarkdownLineParsers;
using Markdown.Parsers.TokensParsers;

namespace Markdown;

public class Md
{
    private readonly IMarkdownLineParser markdownLineParser;
    private readonly ITokensParser tokensParser;
    private readonly IHtmlConverter htmlConverter;

    public Md(IMarkdownLineParser markdownLineParser, IHtmlConverter htmlConverter, ITokensParser tokensParser)
    {
        this.markdownLineParser = markdownLineParser;
        this.htmlConverter = htmlConverter;
        this.tokensParser = tokensParser;
    }

    public string Render(string markdownLine)
    {
        var paragraphs = markdownLineParser.ParseMarkdownTextIntoParagraphs(markdownLine);
        
        var htmlParagraphs = (from paragraph in paragraphs
            select markdownLineParser.ParseParagraphForTokens(paragraph)
            into tokens
            select tokens.ToList()
            into paragraphOfTokens
            let tags = tokensParser.ParserMarkdownTags(paragraphOfTokens)
            select htmlConverter.Convert(paragraphOfTokens, tags)).ToList();

        return string.Join(Environment.NewLine, htmlParagraphs);
    }
}