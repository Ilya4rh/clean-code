using System.Text;
using Markdown.MarkdownTags;

namespace Markdown.Converters;

public class HtmlConverter : IConverter
{
    public string Convert(IEnumerable<MarkdownTag> tagsInLine, string markdownLine)
    {
        var tagsSortingByPosition = tagsInLine
            .OrderBy(tag => tag.Position)
            .ToArray();

        if (tagsSortingByPosition.Length == 0)
            return markdownLine;
        
        var startIndexOfStringFragment = 0;
        var htmlLine = new StringBuilder();
        
        foreach (var tag in tagsSortingByPosition)
        {
            var htmlTag = GetHtmlTag(tag);
            
            if (tag.Position == 0)
            {
                var positionNextChar = tag.Position + tag.Length;
                startIndexOfStringFragment =
                    tag.TagType == MarkdownTagType.Heading ? positionNextChar + 1 : positionNextChar;
                htmlLine.Append(htmlTag);
                continue;
            }

            var lineFragmentLength = tag.Position - startIndexOfStringFragment;
            var lineFragment = markdownLine.AsSpan(startIndexOfStringFragment, lineFragmentLength);
            htmlLine.Append(lineFragment).Append(htmlTag);
            startIndexOfStringFragment = tag.Position + tag.Length;
        }

        if (startIndexOfStringFragment != markdownLine.Length)
            htmlLine.Append(markdownLine.AsSpan(startIndexOfStringFragment,
                markdownLine.Length - startIndexOfStringFragment));
        
        if (tagsSortingByPosition[0].TagType == MarkdownTagType.Heading)
            htmlLine.Append(GetHtmlTag(tagsSortingByPosition[0], true));
        
        return htmlLine.ToString();
    }

    private static string GetHtmlTag(MarkdownTag markdownTag, bool isNeedClosingTagForSingle = false)
    {
        return markdownTag.TagType switch
        {
            MarkdownTagType.Heading when isNeedClosingTagForSingle => "\\</h1>",
            MarkdownTagType.Heading => "\\<h1>",
            MarkdownTagType.Bold when markdownTag.IsClosedTag => "\\</strong>",
            MarkdownTagType.Bold => "\\<strong>",
            MarkdownTagType.Italics when markdownTag.IsClosedTag => "\\</em>",
            MarkdownTagType.Italics => "\\<em>",
            _ => ""
        };
    }
}