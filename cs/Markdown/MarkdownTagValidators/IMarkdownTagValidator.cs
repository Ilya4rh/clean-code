using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public interface IMarkdownTagValidator
{
    /// <summary>
    /// Проверяет валидность тэга 
    /// </summary>
    /// <param name="paragraphOfTokens"></param>
    /// <param name="openingTagToken"></param>
    /// <param name="closingTagToken"></param>
    /// <param name="externalTagType"> Тип внешнего тэга, для текущего </param>
    /// <returns></returns>
    public bool IsValidTag(
        List<IToken> paragraphOfTokens,
        TagToken openingTagToken,
        TagToken? closingTagToken = null,
        MarkdownTagType? externalTagType = null);
}