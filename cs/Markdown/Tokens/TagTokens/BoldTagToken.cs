﻿using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class BoldTagToken : TagToken
{
    public BoldTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    private const string BoldTag = "_";
    
    public override MarkdownTagType TagType => MarkdownTagType.Bold;

    public override string Content => BoldTag;
    
    public override int PositionInTokens { get; }

    public static bool IsBoldTagToken(string line, int position)
    {
        if (line.Length < BoldTag.Length)
            return false;

        return line.Substring(position, BoldTag.Length) == BoldTag;
    }
}