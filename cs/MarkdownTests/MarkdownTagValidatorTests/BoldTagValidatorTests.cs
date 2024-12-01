using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

[TestFixture]
public class BoldTagValidatorTests
{
    private IMarkdownTagValidator markdownTagValidator;
    private readonly MarkdownTagType tagType = MarkdownTagType.Bold;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        markdownTagValidator = new MarkdownTagValidator();
    }
    
    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceAfterOpenTag()
    {
        var line = "__ Hello!__";

        var isValidTag = markdownTagValidator.IsValidTag(tagType, 0, line, 9);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceBeforeCloseTag()
    {
        var line = "__Hello! __";
       
        var isValidTag = markdownTagValidator.IsValidTag(tagType, 0, line, 9);

        isValidTag.Should().BeFalse();
    }
    
    [Test]
    public void IsValidTag_ShouldBeFalse_WhenEmptyLineBetweenTags()
    {
        var line = "Hello, ____ world!";
        
        var isValidTag = markdownTagValidator.IsValidTag(tagType, 7, line, 9);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(3, 11, "Hel__lo, wo__rld!")]
    [TestCase(0, 11, "__Hello, wo__rld!")]
    [TestCase(2, 15, "He__llo, world!__")]
    public void IsValidTag_ShouldBeFalse_WhenTagsInDifferentWords(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(2, 5, "12__3__")]
    [TestCase(0, 5, "__123__56")]
    public void IsValidTag_ShouldBeFalse_WhenTagInsideTheTextWithNumbers(
        int positionOpenTag, 
        int positionCloseTag, 
        string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(1, 8, @"\__Hello__")]
    [TestCase(0, 8, @"__Hello\__")]
    public void IsValidTag_ShouldBeFalse_WhenIsEscapedTag(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(3, 10, @"\\\__Hello__")]
    [TestCase(0, 12, @"__Hello\\\\\__")]
    public void IsValidTag_ShouldBeFalse_WhenIsOddNumberOfEscapeChars(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenExternalTagIsItalics()
    {
        var line = "_Hello, __big__ world_";

        var isValidTag = markdownTagValidator.IsValidTag(tagType, 8, line, 13, MarkdownTagType.Italics);

        isValidTag.Should().BeFalse();
    }
    
    [Test]
    public void IsValid_ShouldBeTrue_WhenTagsAtTheBeginningAndEndOfTheLine()
    {
        var line = "__Hello, world!__";
        
        var isValidTag = markdownTagValidator.IsValidTag(tagType, 0, line, 15);

        isValidTag.Should().BeTrue();
    }

    [TestCase(0, 4, "__He__llo, world!")]
    [TestCase(2, 7, "Ma__rkd__own")]
    [TestCase(2, 8, "He__llo,__ world!")]
    public void IsValidTag_ShouldBeTrue_WhenTagsInOneWord(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }

    [TestCase(4, 11, @"\\\\__Hello__")]
    [TestCase(0, 9, @"__Hello\\__")]
    public void IsValidTag_ShouldBeTrue_WhenIsEvenNumberOfEscapeChars(
        int positionOpenTag, 
        int positionCloseTag,
        string line)
    {
        var isValidTag = markdownTagValidator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }
}