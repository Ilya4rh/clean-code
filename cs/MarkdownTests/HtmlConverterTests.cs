using FluentAssertions;
using Markdown.Converters;
using Markdown.MarkdownTags;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class HtmlConverterTests
{
    private IHtmlConverter htmlConverter;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        htmlConverter = new HtmlConverter();
    }
    
    [Test]
    public void Convert_ReturnOriginalString_WhenWereNoTags()
    {
        var markdownLine = "Здесь нет тегов";

        var htmlLine = htmlConverter.Convert([], markdownLine);

        htmlLine.Should().Be(markdownLine);
    }
    
    [Test]
    public void Convert_ShouldWrapLineWithHeadingTags_WhenHeadingTagProvided()
    {
        var markdownLine = "# Текст с заголовком";
        var tags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Heading, 0, 1)
        };

        var htmlLine = htmlConverter.Convert(tags, markdownLine);

        htmlLine.Should().Be(@"\<h1>Текст с заголовком\</h1>");
    }

    [Test]
    public void Convert_ShouldApplyBoldTag_WhenBoldTagProvided()
    {
        var markdownLine = "Текст с __полужирным__ тегом";
        var tags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Bold, 8, 2),
            new(MarkdownTagType.Bold, 20, 2, true)
        };

        var htmlLine = htmlConverter.Convert(tags, markdownLine);

        htmlLine.Should().Be(@"Текст с \<strong>полужирным\</strong> тегом");
    }

    [Test]
    public void Convert_ShouldApplyItalicsTag_WhenItalicsTagProvided()
    {
        var markdownLine = "Текст с _курсивом_";
        var tags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Italics, 8, 1),
            new(MarkdownTagType.Italics, 17, 1, true)
        };

        var htmlLine = htmlConverter.Convert(tags, markdownLine);

        htmlLine.Should().Be(@"Текст с \<em>курсивом\</em>");
    }

    [Test]
    public void Convert_ShouldHandleMultipleTags_WhenOneWordContainsAllTags()
    {
        var markdownLine = "# ___Привет___";
        var tags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Heading, 0, 1),
            new(MarkdownTagType.Bold, 2, 2),
            new(MarkdownTagType.Italics, 4, 1),
            new(MarkdownTagType.Italics, 11, 1, true),
            new(MarkdownTagType.Bold, 12, 2, true)
        };
        
        var htmlLine = htmlConverter.Convert(tags, markdownLine);

        htmlLine.Should().Be(@"\<h1>\<strong>\<em>Привет\</em>\</strong>\</h1>");
    }

    [Test]
    public void Convert_ShouldHandleNestedTags_WhenItalicsTagsInBoldTags()
    {
        var markdownLine = "__Тут _курсив_ внутри полужирного__";
        var tags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Bold, 0, 2),
            new(MarkdownTagType.Italics, 6, 1),
            new(MarkdownTagType.Italics, 13, 1, true),
            new(MarkdownTagType.Bold, 33, 2, true)
        };
        
        var htmlLine = htmlConverter.Convert(tags, markdownLine);

        htmlLine.Should().Be(@"\<strong>Тут \<em>курсив\</em> внутри полужирного\</strong>");
    }
}