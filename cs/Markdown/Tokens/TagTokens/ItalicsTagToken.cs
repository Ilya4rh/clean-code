using Markdown.MarkdownTags;

namespace Markdown.Tokens.TagTokens;

public class ItalicsTagToken : TagToken
{
    public ItalicsTagToken(int positionInTokens)
    {
        PositionInTokens = positionInTokens;
    }

    private const string ItalicsTag = "_";

    public override MarkdownTagType TagType => MarkdownTagType.Italics;

    public override string Content => ItalicsTag;
    
    public override int PositionInTokens { get; }

    public static bool IsItalicsTagToken(string line, int position)
    {
        return line[position].ToString() == ItalicsTag;
    }
}