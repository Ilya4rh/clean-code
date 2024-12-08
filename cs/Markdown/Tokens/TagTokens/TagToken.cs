﻿using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public abstract class TagToken : IToken
{
    public abstract MarkdownTagType MarkdownTagType { get; }
    
    public TokenType Type => TokenType.Tag;

    public abstract string Content { get; }
    
    public abstract int PositionInTokens { get; }

    public static bool TryCreateTagToken(string line, int positionInLine, int positionInTokens, out TagToken? tagToken)
    {
        if (BoldTagToken.IsBoldTagToken(line, positionInLine))
        {
            tagToken = new BoldTagToken(positionInTokens);
            return true;
        }

        if (ItalicsTagToken.IsItalicsTagToken(line, positionInLine))
        {
            tagToken = new ItalicsTagToken(positionInTokens);
            return true;
        }
        
        if (HeadingTagToken.IsHeadingTagToken(line, positionInLine))
        {
            tagToken = new HeadingTagToken(positionInTokens);
            return true;
        }

        tagToken = null;
        return false;
    }
}