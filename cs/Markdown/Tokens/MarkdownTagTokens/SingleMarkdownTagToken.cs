using Markdown.MarkdownTags;
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
    
    public MarkdownTag ConvertToMarkdownTag()
    {
        return new MarkdownTag(Token, TagType);
    }

    public MarkdownTagType TagType { get; }
    
    public static bool TryCreate(TagToken tagToken, out SingleMarkdownTagToken? singleMarkdownTagToken)
    {
        if (tagToken.Content == HeadingTagToken.HeadingTokenContent)
        {
            singleMarkdownTagToken = new SingleMarkdownTagToken(MarkdownTagType.Heading, tagToken);
            return true;
        }

        singleMarkdownTagToken = null;
        return false;
    }
}