﻿using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public class MarkdownTagValidator : IMarkdownTagValidator
{
    public bool IsValidSingleTag(
        List<IToken> paragraphOfTokens, 
        ISingleMarkdownTag singleTag)
    {
        return singleTag.Token.MarkdownTagType switch
        {
            MarkdownTagType.Heading => IsValidHeadingTag(singleTag.Token, paragraphOfTokens),
            _ => false
        };
    }

    public bool IsValidPairedTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTags pairedTags, 
        MarkdownTagType? externalTagType = null)
    {
        return pairedTags.OpeningToken.MarkdownTagType switch
        {
            MarkdownTagType.Italics => IsValidItalicsTag(paragraphOfTokens, pairedTags),
            MarkdownTagType.Bold => IsValidBoldTag(paragraphOfTokens, pairedTags, externalTagType),
            _ => false
        };
    }
    
    private static bool IsValidHeadingTag(TagToken tagToken, List<IToken> paragraphOfTokens)
    {
        var position = tagToken.PositionInTokens;

        if (paragraphOfTokens.Count < 2)
            return false;

        return position == 0 && paragraphOfTokens[position + 1].Type == TokenType.Space;
    }

    private static bool IsValidItalicsTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTags pairedTags)
    {
        return IsValidPairedTags(paragraphOfTokens, pairedTags);
    }

    private static bool IsValidBoldTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTags pairedTags,
        MarkdownTagType? externalTagType)
    {
        if (pairedTags.OpeningToken.PositionInTokens + 1 == pairedTags.ClosingToken.PositionInTokens)
            return false;
        if (externalTagType is MarkdownTagType.Italics)
            return false;

        return IsValidPairedTags(paragraphOfTokens, pairedTags);
    }
    
    private static bool IsValidPairedTags( 
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTags pairedTags)
    {
        var positionOpening = pairedTags.OpeningToken.PositionInTokens;
        var positionClosing = pairedTags.ClosingToken.PositionInTokens;
        
        // Подумать о выносе этих метод из класса
        if (IsSpaceAfterTagToken(paragraphOfTokens, positionOpening) || 
            IsSpaceBeforeTagToken(paragraphOfTokens, positionClosing))
            return false;
        if (IsTagsInDifferentWords(paragraphOfTokens, positionOpening, positionClosing))
            return false;
        if (IsTagsInsideTheTextWithNumbers(paragraphOfTokens, positionOpening, positionClosing))
            return false;
        if (IsEscapedTag(paragraphOfTokens, positionOpening) ||
            IsEscapedTag(paragraphOfTokens, positionClosing))
            return false;
        
        return true;
    }

    private static bool IsSpaceAfterTagToken(List<IToken> paragraphOfTokens, int tagTokenPosition)
    {
        if (tagTokenPosition == paragraphOfTokens.Count - 1)
            return false;
        
        var nextToken = paragraphOfTokens[tagTokenPosition + 1];

        return nextToken.Type == TokenType.Space;
    }
    
    private static bool IsSpaceBeforeTagToken(List<IToken> paragraphOfTokens, int tagTokenPosition)
    {
        if (tagTokenPosition == 0)
            return false;
        
        var previousToken = paragraphOfTokens[tagTokenPosition - 1];

        return previousToken.Type == TokenType.Space;
    }

    private static bool IsTagsInDifferentWords(
        List<IToken> paragraphOfTokens, 
        int positionOpeningTagToken, 
        int positionClosingTagToken)
    {
        if (positionOpeningTagToken == 0 && 
            positionClosingTagToken == paragraphOfTokens.Count - 1)
            return false;

        if (IsSpaceBeforeTagToken(paragraphOfTokens, positionOpeningTagToken) &&
            IsSpaceAfterTagToken(paragraphOfTokens, positionClosingTagToken)) 
            return false;

        var numberOfTokensBetweenTags = positionClosingTagToken - positionOpeningTagToken;

        var tokensBetweenTags = paragraphOfTokens.GetRange(positionOpeningTagToken, numberOfTokensBetweenTags);
        
        return tokensBetweenTags.Any(token => token.Type == TokenType.Space);
    }

    private static bool IsTagsInsideTheTextWithNumbers(
        List<IToken> paragraphOfTokens, 
        int positionOpeningTagToken, 
        int positionClosingTagToken)
    {

        if (positionOpeningTagToken != 0 && IsTagOnNumber(paragraphOfTokens, positionOpeningTagToken))
            return true;
        
        if (positionClosingTagToken != paragraphOfTokens.Count - 1 && 
            IsTagOnNumber(paragraphOfTokens, positionClosingTagToken))
            return true;

        return false;
    }

    private static bool IsTagOnNumber(List<IToken> paragraphOfTokens, int tagTokenPosition)
    {
        return paragraphOfTokens[tagTokenPosition - 1].Type == TokenType.Digit && 
               paragraphOfTokens[tagTokenPosition + 1].Type == TokenType.Digit;
    }

    private static bool IsEscapedTag(List<IToken> paragraphOfTokens, int tagPosition)
    {
        if (tagPosition == 0)
            return false;

        var previousToken = paragraphOfTokens[tagPosition - 1];
        
        if (tagPosition == 1 && previousToken.Type == TokenType.Escape)
            return true;

        return 
            previousToken.Type == TokenType.Escape && paragraphOfTokens[tagPosition - 2].Type != TokenType.Escape;
    }
}