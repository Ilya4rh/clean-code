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
    
    public MarkdownTagType TagType { get; }
    
    public static bool TryCreateSingleMarkdownTagToken(
        TagToken tagToken, 
        out SingleMarkdownTagToken? singleMarkdownTagToken)
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