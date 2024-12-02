using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public abstract class TagToken : IToken
{
    public abstract MarkdownTagType MarkdownTagType { get; }
    
    public TokenType Type => TokenType.Tag;

    public abstract string Content { get; }
    
    public abstract int PositionInTokens { get; }

    public static bool TryCreateTagToken(string line, int position, out TagToken? tagToken)
    {
        if (BoldTagToken.IsBoldTagToken(line, position))
        {
            tagToken = new BoldTagToken(position);
            return true;
        }

        if (ItalicsTagToken.IsItalicsTagToken(line, position))
        {
            tagToken = new ItalicsTagToken(position);
            return true;
        }
        
        if (HeadingTagToken.IsHeadingTagToken(line, position))
        {
            tagToken = new HeadingTagToken(position);
            return true;
        }

        tagToken = null;
        return false;
    }
    
    // public abstract bool IsTagToken(string line, int position);
}