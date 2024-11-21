using Markdown.MarkdownTags;

namespace Markdown.Parsers;

public interface IParser
{
    List<MarkdownTag> ParseMarkdownTags(string markdownLine);
}