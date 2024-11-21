using Markdown.MarkdownTags;

namespace Markdown.Converters;

public interface IConverter
{
    string Convert(MarkdownTag markdownTag, string markdownLine);
}