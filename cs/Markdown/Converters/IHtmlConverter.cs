using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown.Converters;

public interface IHtmlConverter
{
    string Convert(IEnumerable<IToken> tokensInParagraph, IEnumerable<MarkdownTag> tagsInParagraph);
}