using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

[TestFixture]
public class HeadingTagValidatorTests
{
    private IValidator validator;
    private readonly MarkdownTagType tagType = MarkdownTagType.Heading;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        validator = new MarkdownTagValidator();
    }
    
    [TestCase(4, "    # Heading")]
    [TestCase(10, "          # Heading")]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagHasSpacesInFrontAndPositionGreaterOrEqualToFour(
        int tagPosition, 
        string line)
    {
        var isValidTag = validator.IsValidTag(tagType, tagPosition, line);

        isValidTag.Should().BeFalse();
    }
    
    [TestCase(1, "1#Hello")]
    [TestCase(2, @"\\#")]
    [TestCase(3, " H #")]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagHasTextInFront(int tagPosition, string line)
    {
        var isValidTag = validator.IsValidTag(tagType, tagPosition, line);
        
        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenNoSpaceAfterHeadingTag()
    {
        var line = "#Hello";
        
        var isValidTag = validator.IsValidTag(tagType, 0, line);
        
        isValidTag.Should().BeFalse();
    }
    
    [TestCase("# Привет, мир!")]
    [TestCase("# ")]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasPositionZeroAndSpace(string line)
    {
        var isValid = validator.IsValidTag(tagType, 0, line);

        isValid.Should().BeTrue();
    }

    [Test]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagIsOnePerLine()
    {
        var line = "#";
        var positionOnLine = 0;
        
        var isValid = validator.IsValidTag(tagType, positionOnLine, line);

        isValid.Should().BeTrue();
    }
    
    [TestCase(1, " # Hello")]
    [TestCase(2, "  # ")]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasSpacesInFront(int tagPosition, string line)
    {
        var isValid = validator.IsValidTag(tagType, tagPosition, line);

        isValid.Should().BeTrue();
    }
}