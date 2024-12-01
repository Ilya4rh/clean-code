using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Parsers;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownLineParserTests
{
    private IParser parser;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        parser = new MarkdownLineParser(new MarkdownTagValidator());
    }
    
    [Test]
    public void ParseMarkdownTags_ReturnHeadingTagType_WhenParagraphBeginsWithHeadingTag()
    {
        var paragraphLine = "# Hello, world!";
        var expectedTag = new MarkdownTag(MarkdownTagType.Heading, 0, 1);

        var tags = parser.ParseMarkdownTags(paragraphLine);
        
        tags.First().Should().BeEquivalentTo(expectedTag);
    }
    
    [Test]
    public void ParseMarkdownTags_ReturnItalicsTagsType_WhenParagraphContainsItalicsTagOnly()
    {
        var paragraphLine = "_Hello, world!_";
        var expectedTags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Italics, 0, 1),
            new(MarkdownTagType.Italics, 14, 1, true),
        };
            
        var tags = parser.ParseMarkdownTags(paragraphLine);

        tags.Should().BeEquivalentTo(expectedTags);
    }
    
    [Test]
    public void ParseMarkdownTags_ReturnBoldTagsType_WhenParagraphContainsBoldTagOnly()
    {
        var paragraphLine = "__Hello, world!__";
        var expectedTags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Bold, 0, 2),
            new(MarkdownTagType.Bold, 15, 2, true),
        };
            
        var tags = parser.ParseMarkdownTags(paragraphLine);

        tags.Should().BeEquivalentTo(expectedTags);
    }

    [Test]
    public void ParseMarkdownTags_ReturnItalicsTagsOnly_WhenInsideBoldTags()
    {
        var paragraphLine = "Внутри _одинарного __двойное__ не_ работает.";
        var expectedTags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Italics, 7, 1),
            new(MarkdownTagType.Italics, 33, 1, true),
        };
        
        var tags = parser.ParseMarkdownTags(paragraphLine);

        tags.Should().BeEquivalentTo(expectedTags);
    }
    
    [Test]
    public void ParseMarkdownTags_ShouldBeEmpty_WhenTagsOverlap()
    {
        var paragraphLine =
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";

        var tags = parser.ParseMarkdownTags(paragraphLine);

        tags.Should().BeEmpty();
    }

    [Test]
    public void ParseMarkdownTags_ShouldBeEmpty_WhenTagsDoNotHavePair()
    {
        var paragraphLine = "__Hello, world!_";

        var tags = parser.ParseMarkdownTags(paragraphLine);

        tags.Should().BeEmpty();
    }

    [Test]
    public void ParseMarkdownTags_ParsedAllTags_WhenAllTagsIsValid()
    {
        var paragraphLine = "# Hello, __markdown _is_ difficult__, i am __very__ _tried!_";

        var tags = parser.ParseMarkdownTags(paragraphLine).ToArray();

        tags.Length.Should().Be(9);
    }

    [Test]
    public void ParseMarkdownTags_ParsedAllTags_WhenOneWordContainsAllTags()
    {
        var paragraphLine = "# ___Hello___";
        var expectedTags = new List<MarkdownTag>
        {
            new(MarkdownTagType.Heading, 0, 1),
            new(MarkdownTagType.Bold, 2, 2),
            new(MarkdownTagType.Italics, 4, 1),
            new(MarkdownTagType.Italics, 10, 1, true),
            new(MarkdownTagType.Bold, 11, 2, true)
        };
        
        var tags = parser.ParseMarkdownTags(paragraphLine).ToArray();

        tags.Should().BeEquivalentTo(expectedTags);
    }

    [Test]
    public void ParseMarkdownTextIntoParagraphs_ReturnTwoParagraphs_WhenTextContainsTwoEmptyLines()
    {
        var text = "_Первый абзац_ \n\n _Второй абзац_";

        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("_Первый абзац_", "_Второй абзац_");
    }
    
    [Test]
    public void ParseMarkdownTextIntoParagraphs_ReturnMultiLineParagraph_WhenTextContainsEmptyLines()
    {
        var text = "_Первый абзац_ \n _Второй абзац_";

        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("_Первый абзац_ _Второй абзац_");
    }

    [Test]
    public void ParseMarkdownTextIntoParagraphs_ReturnThreeParagraphs_WhenHeadingTagInText()
    {
        var text = "Первый абзац\n # Второй абзац \n Третий абзац";
        
        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("Первый абзац", "# Второй абзац", "Третий абзац");
    }
    
    [Test]
    public void ParseMarkdownTextIntoParagraphs_ReturnTwoParagraphs_WhenInvalidHeadingTagInText()
    {
        var text = "Первый абзац\n     # Второй абзац \n\n Третий абзац";
        
        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("Первый абзац # Второй абзац", "Третий абзац");
    }

    [Test]
    public void ParseMarkdownTextIntoParagraphs_ReturnTwoParagraphs_WhenLineContainsTwoSpaces()
    {
        var text = "_Первый абзац_  \n _Второй абзац_";

        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("_Первый абзац_", "_Второй абзац_");
    }

    [Test]
    public void ParseMarkdownTextIntoParagraphs_SkipEmptyLines_WhenTextContainsFourEmptyLines()
    {
        var text = "Первый абзац \n\n\n\n Второй абзац";
        
        var paragraphs = parser.ParseMarkdownTextIntoParagraphs(text);
        
        paragraphs.Should().BeEquivalentTo("Первый абзац", "Второй абзац");
    }
}