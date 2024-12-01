using Markdown.MarkdownTags;

namespace Markdown.Converters;

public interface IHtmlConverter
{
    string Convert(IEnumerable<MarkdownTag> tagsInLine, string markdownLine);
}