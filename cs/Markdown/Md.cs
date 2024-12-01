using Markdown.Converters;
using Markdown.Parsers;

namespace Markdown;

public class Md
{
    private readonly IMarkdownLineParser markdownLineParser;
    private readonly IHtmlConverter htmlConverter;

    public Md(IMarkdownLineParser markdownLineParser, IHtmlConverter htmlConverter)
    {
        this.markdownLineParser = markdownLineParser;
        this.htmlConverter = htmlConverter;
    }

    public string Render(string markdownLine)
    {
        var paragraphs = markdownLineParser.ParseMarkdownTextIntoParagraphs(markdownLine);
        var htmlParagraphs = new List<string>();
        
        foreach (var paragraph in paragraphs)
        {
            var tagsInParagraph = markdownLineParser.ParseMarkdownTags(paragraph);
            var htmlLine = htmlConverter.Convert(tagsInParagraph, paragraph);
            htmlParagraphs.Add(htmlLine);
        }

        return string.Join('\n', htmlParagraphs);
    }
}