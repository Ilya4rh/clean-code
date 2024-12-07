using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public interface IMarkdownTagValidator
{
    public bool IsValidSingleTag(
        List<IToken> paragraphOfTokens,
        ISingleMarkdownTag singleTag);

    public bool IsValidPairedTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTags pairedTags,
        MarkdownTagType? externalTagType = null);
}