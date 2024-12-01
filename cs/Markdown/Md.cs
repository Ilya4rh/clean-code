using Markdown.Converters;
using Markdown.Parsers;

namespace Markdown;

public class Md
{
    private readonly IParser parser;
    private readonly IConverter converter;

    public Md(IParser parser, IConverter converter)
    {
        this.parser = parser;
        this.converter = converter;
    }

    public string Render(string markdownLine)
    {
        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(markdownLine);
        var htmlParagraphs = new List<string>();
        
        foreach (var paragraph in paragraphs)
        {
            var tagsInParagraph = parser.ParseMarkdownTags(paragraph);
            var htmlLine = converter.Convert(tagsInParagraph, paragraph);
            htmlParagraphs.Add(htmlLine);
        }

        return string.Join('\n', htmlParagraphs);
    }
}