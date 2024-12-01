using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.MarkdownTags;
using Markdown.Parsers;
using Moq;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownRenderTests
{
    private Md md;
    private readonly Mock<IParser> parserMock = new();
    private readonly Mock<IConverter> converterMock = new();

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        md = new Md(parserMock.Object, converterMock.Object);
    }

    [Test]
    public void Render_ShouldReturnConvertedHtml_WhenInputContainsSingleParagraph()
    {
        var markdownLine = "# Обычная строка с одним тегом";
        var htmlLine = @"\<h1>Обычная строка с одним тегом\</h1>";
        parserMock
            .Setup(parser => parser.ParseMarkdownTextIntoParagraphs(It.IsAny<string>()))
            .Returns([markdownLine]);
        parserMock
            .Setup(parser => parser.ParseMarkdownTags(It.IsAny<string>()))
            .Returns([new MarkdownTag(MarkdownTagType.Heading, 0, 1)]);
        converterMock
            .Setup(converter => converter.Convert(It.IsAny<IEnumerable<MarkdownTag>>(), It.IsAny<string>()))
            .Returns(htmlLine);

        var renderedString = md.Render(markdownLine);

        renderedString.Should().Be(htmlLine);
    }

    [Test]
    public void Render_ShouldReturnConvertedHtml_WhenInputContainsSeveralParagraphs()
    {
        var markdownLine = "Абзац1 \n\n _Абзац2_ \n __Принадлежит2__";
        var paragraphs = new List<string> { "Абзац1", "_Абзац2_ Принадлежит2" };
        var tags = new List<MarkdownTag>
            { new(MarkdownTagType.Italics, 0, 1), new(MarkdownTagType.Italics, 7, 1, true) };
        var htmlSecondParagraph = @"\<em>_Абзац2_ Принадлежит2\</em>";
        parserMock
            .Setup(parser => parser.ParseMarkdownTextIntoParagraphs(It.IsAny<string>()))
            .Returns(paragraphs);
        parserMock
            .Setup(parser => parser.ParseMarkdownTags(paragraphs[0]))
            .Returns([]);
        parserMock
            .Setup(parser => parser.ParseMarkdownTags(paragraphs[1]))
            .Returns(tags);
        converterMock
            .Setup(converter => converter.Convert(Array.Empty<MarkdownTag>(), paragraphs[0]))
            .Returns(paragraphs[0]);
        converterMock
            .Setup(converter => converter.Convert(tags, paragraphs[1]))
            .Returns(htmlSecondParagraph);
        
        var renderedString = md.Render(markdownLine);

        renderedString.Should().Be("Абзац1\n\\<em>_Абзац2_ Принадлежит2\\</em>");
    }
}