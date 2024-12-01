using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

public class ItalicsTagValidatorTests
{
    private IValidator validator;
    private readonly MarkdownTagType tagType = MarkdownTagType.Italics;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        validator = new MarkdownTagValidator();
    }
    
    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceAfterOpenTag()
    {
        var line = "_ Hello!_";

        var isValidTag = validator.IsValidTag(tagType, 0, line, 8);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceBeforeCloseTag()
    {
        var line = "_Hello! _";
       
        var isValidTag = validator.IsValidTag(tagType, 0, line, 8);

        isValidTag.Should().BeFalse();
    }

    [TestCase(3, 10, "Hel_lo, wo_rld!")]
    [TestCase(0, 10, "_Hello, wo_rld!")]
    [TestCase(2, 14, "He_llo, world!_")]
    public void IsValidTag_ShouldBeFalse_WhenTagsInDifferentWords(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(2, 4, "12_3_")]
    [TestCase(0, 4, "_123_56")]
    public void IsValidTag_ShouldBeFalse_WhenTagInsideTheTextWithNumbers(
        int positionOpenTag, 
        int positionCloseTag, 
        string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(1, 7, @"\_Hello_")]
    [TestCase(0, 7, @"_Hello\_")]
    public void IsValidTag_ShouldBeFalse_WhenIsEscapedTag(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(3, 9, @"\\\_Hello_")]
    [TestCase(0, 11, @"_Hello\\\\\_")]
    public void IsValidTag_ShouldBeFalse_WhenIsOddNumberOfEscapeChars(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldBeTrue_WhenTagsAtTheBeginningAndEndOfTheLine()
    {
        var line = "_Hello, world!_";
        
        var isValidTag = validator.IsValidTag(tagType, 0, line, 14);

        isValidTag.Should().BeTrue();
    }

    [TestCase(0, 3, "_He_llo, world!")]
    [TestCase(2, 6, "Ma_rkd_own")]
    [TestCase(2, 7, "He_llo,_ world!")]
    public void IsValidTag_ShouldBeTrue_WhenTagsInOneWord(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }

    [TestCase(4, 10, @"\\\\_Hello_")]
    [TestCase(0, 8, @"_Hello\\_")]
    public void IsValidTag_ShouldBeTrue_WhenIsEvenNumberOfEscapeChars(
        int positionOpenTag, 
        int positionCloseTag,
        string line)
    {
        var isValidTag = validator.IsValidTag(tagType, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }
}