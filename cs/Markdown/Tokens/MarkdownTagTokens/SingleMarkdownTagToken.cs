﻿using Markdown.MarkdownTags;
using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public class SingleMarkdownTagToken : ISingleMarkdownTagToken
{
    public SingleMarkdownTagToken(MarkdownTagType tagType, TagToken token)
    {
        Token = token;
        TagType = tagType;
    }

    public TagToken Token { get; }
    
    public MarkdownTagType TagType { get; }
}