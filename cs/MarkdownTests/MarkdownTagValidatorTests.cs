using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownTagValidatorTests
{
    private IValidator validator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        validator = new MarkdownTagValidator();
    }

    [TestCase(-1)]
    [TestCase(1000)]
    public void IsValidTag_ThrowArgumentException_WhenTagPositionIsInvalid(int tagPositionOnLine)
    {
        var line = "Hello, Markdown!";

        var action = () =>
        {
            _ = validator.IsValidTag(MarkdownTagType.Bold, tagPositionOnLine, line);
        };

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The 'positionOnLine' is less than zero or greater than the length of the string.");
    }

    [TestCase(-1)]
    [TestCase(1000)]
    public void IsValidTag_ThrowArgumentException_WhenPositionCloseTagIsInvalid(int positionCloseTagOnLine)
    {
        var line = "Hello, Markdown!";

        var action = () =>
        {
            _ = validator.IsValidTag(MarkdownTagType.Bold, 0, line, positionCloseTagOnLine);
        };

        action
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("The 'positionCloseTagOnLine' is less than zero or greater than the length of the string.");
    }

    #region HeadingTag
    
    [TestCase(4, "    # Heading")]
    [TestCase(10, "          # Heading")]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagHasSpacesInFrontAndPositionGreaterOrEqualToFour(
        int tagPosition, 
        string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Heading, tagPosition, line);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(1, "1#Hello")]
    [TestCase(2, @"\\#")]
    [TestCase(3, " H #")]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagHasTextInFront(int tagPosition, string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Heading, tagPosition, line);
        
        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenNoSpaceAfterHeadingTag()
    {
        var line = "#Hello";
        
        var isValidTag = validator.IsValidTag(MarkdownTagType.Heading, 0, line);
        
        isValidTag.Should().BeFalse();
    }
    
    [TestCase("# Привет, мир!")]
    [TestCase("# ")]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasPositionZeroAndSpace(string line)
    {
        var isValid = validator.IsValidTag(MarkdownTagType.Heading, 0, line);

        isValid.Should().BeTrue();
    }

    [Test]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagIsOnePerLine()
    {
        var line = "#";
        var positionOnLine = 0;
        
        var isValid = validator.IsValidTag(MarkdownTagType.Heading, positionOnLine, line);

        isValid.Should().BeTrue();
    }
    
    [TestCase(1, " # Hello")]
    [TestCase(2, "  # ")]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasSpacesInFront(int tagPosition, string line)
    {
        var isValid = validator.IsValidTag(MarkdownTagType.Heading, tagPosition, line);

        isValid.Should().BeTrue();
    }
    
    #endregion

    #region ItalicsTag

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceAfterOpenTag()
    {
        var line = "_ Hello!_";

        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, 0, line, 8);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHasSpaceBeforeCloseTag()
    {
        var line = "_Hello! _";
       
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, 0, line, 8);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenEmptyLineBetweenTags()
    {
        var line = "Hello, __ world!";
        
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, 7, line, 8);

        isValidTag.Should().BeFalse();
    }

    [TestCase(3, 10, "Hel_lo, wo_rld!")]
    [TestCase(0, 10, "_Hello, wo_rld!")]
    [TestCase(2, 14, "He_llo, world!_")]
    public void IsValidTag_ShouldBeFalse_WhenTagsInDifferentWords(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(2, 4, "12_3_")]
    [TestCase(0, 4, "_123_56")]
    public void IsValidTag_ShouldBeFalse_WhenTagInsideTheTextWithNumbers(
        int positionOpenTag, 
        int positionCloseTag, 
        string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(1, 7, @"\_Hello_")]
    [TestCase(0, 7, @"_Hello\_")]
    public void IsValidTag_ShouldBeFalse_WhenIsEscapedTag(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [TestCase(3, 9, @"\\\_Hello_")]
    [TestCase(0, 11, @"_Hello\\\\\_")]
    public void IsValidTag_ShouldBeFalse_WhenIsOddNumberOfEscapeChars(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValid_ShouldBeTrue_WhenTagsAtTheBeginningAndEndOfTheLine()
    {
        var line = "_Hello, world!_";
        
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, 0, line, 14);

        isValidTag.Should().BeTrue();
    }

    [TestCase(0, 3, "_He_llo, world!")]
    [TestCase(2, 6, "Ma_rkd_own")]
    [TestCase(2, 7, "He_llo,_ world!")]
    public void IsValidTag_ShouldBeTrue_WhenTagsInOneWord(int positionOpenTag, int positionCloseTag, string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }

    [TestCase(4, 10, @"\\\\_Hello_")]
    [TestCase(0, 8, @"_Hello\\_")]
    public void IsValidTag_ShouldBeTrue_WhenIsEvenNumberOfEscapeChars(
        int positionOpenTag, 
        int positionCloseTag,
        string line)
    {
        var isValidTag = validator.IsValidTag(MarkdownTagType.Italics, positionOpenTag, line, positionCloseTag);

        isValidTag.Should().BeTrue();
    }
    
    #endregion
}