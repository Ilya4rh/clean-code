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
}