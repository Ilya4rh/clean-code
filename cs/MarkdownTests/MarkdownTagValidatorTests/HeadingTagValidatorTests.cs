using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

[TestFixture]
public class HeadingTagValidatorTests
{
    private readonly MarkdownTagValidator markdownTagValidator = new();
    private const MarkdownTagType TagType = MarkdownTagType.Heading;

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagTokenPositionIsNotZero()
    {
        var paragraphOfTokens = new List<IToken> { new TextToken("A"), new HeadingTagToken(1)  };
        
        var isValidTag = markdownTagValidator.IsValidTag(paragraphOfTokens, new HeadingTagToken(1));
        
        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenNoSpaceAfterHeadingTag()
    {
        var paragraphOfTokens = new List<IToken> { new HeadingTagToken(0), new TextToken("A") };
        
        var isValidTag = markdownTagValidator.IsValidTag(paragraphOfTokens, new HeadingTagToken(0));
        
        isValidTag.Should().BeFalse();
    }
    
    [Test]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasPositionZeroAndSpace()
    {
        var paragraphOfTokens = new List<IToken>
        {
            new HeadingTagToken(0), new SpaceToken(), new TextToken("A")
        };
        
        var isValid = markdownTagValidator.IsValidTag(paragraphOfTokens, new HeadingTagToken(0));

        isValid.Should().BeTrue();
    }
}