﻿using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public interface IPairedMarkdownTags : IMarkdownTag
{
    TagToken OpeningToken { get; }
    
    TagToken ClosingToken { get; }

    bool IsIntersect(IPairedMarkdownTags pairedMarkdownTags);

    bool IsExternalFor(IPairedMarkdownTags pairedMarkdownTags);
}