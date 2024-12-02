using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public abstract class TagToken : IToken
{
    public abstract MarkdownTagType TagType { get; }
    
    public TokenType Type => TokenType.Tag;

    public abstract string Content { get; }

    public abstract bool IsTagToken(string line, int position);
}