﻿using FluentAssertions;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests.MarkdownTagValidatorTests;

public class MarkedListTagValidatorTests
{
    private readonly MarkdownTagValidator markdownTagValidator = new();

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenHeadingTagTokenPositionIsNotZero()
    {
        var paragraphOfTokens = new List<IToken> { new TextToken("A"), new MarkedListTagToken(1)  };
        
        var isValidTag = markdownTagValidator.IsValidSingleTag(
            paragraphOfTokens, 
            new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(1)));
        
        isValidTag.Should().BeFalse();
    }

    [Test]
    public void IsValidTag_ShouldBeFalse_WhenNoSpaceAfterHeadingTag()
    {
        var paragraphOfTokens = new List<IToken> { new MarkedListTagToken(0), new TextToken("A") };
        
        var isValidTag = markdownTagValidator.IsValidSingleTag(
            paragraphOfTokens, 
            new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(0)));
        
        isValidTag.Should().BeFalse();
    }
    
    [Test]
    public void IsValidTag_ShouldBeTrue_WhenHeadingTagHasPositionZeroAndSpace()
    {
        var paragraphOfTokens = new List<IToken>
        {
            new MarkedListTagToken(0), new SpaceToken(), new TextToken("A")
        };
        
        var isValid = markdownTagValidator.IsValidSingleTag(
            paragraphOfTokens,
            new SingleMarkdownTagToken(MarkdownTagType.MarkedList, new MarkedListTagToken(0)));

        isValid.Should().BeTrue();
    }
}