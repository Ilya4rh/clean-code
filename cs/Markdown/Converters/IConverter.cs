using Markdown.MarkdownTags;

namespace Markdown.Converters;

public interface IConverter
{
    string Convert(IEnumerable<MarkdownTag> tagsInLine, string markdownLine);
}