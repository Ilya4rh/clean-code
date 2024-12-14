using System.Text;
using Markdown.HtmlTags;
using Markdown.MarkdownTags;
using Markdown.Tokens;

namespace Markdown.Converters;

public class HtmlConverter : IHtmlConverter
{
    public string Convert(IEnumerable<IToken> tokensInParagraph, IEnumerable<MarkdownTag> tagsInParagraph)
    {
        var tagsSortingByPosition = tagsInParagraph
            .OrderBy(tag => tag.Token.PositionInTokens)
            .ToArray();
        var tokens = tokensInParagraph.ToArray();
        var htmlString = new StringBuilder();
        var firstTag = tagsSortingByPosition.FirstOrDefault();
        var previousTag = firstTag;
        var previousTagPosition = firstTag == null ? tokens.Length : firstTag.Token.PositionInTokens;

        htmlString.Append(ConvertCommonTokensToString(tokens, 0, previousTagPosition));

        if (firstTag != null)
            htmlString.Append(GetHtmlTag(firstTag) ?? "");

        for (var i = 1; i < tagsSortingByPosition.Length; i++)
        {
            var currentTag = tagsSortingByPosition[i];
            var commonTokensStartIndex = CalculateCommonTokensStartIndex(previousTag!);
            var commonTokensOnString =
                ConvertCommonTokensToString(tokens, commonTokensStartIndex, currentTag.Token.PositionInTokens);
            
            htmlString.Append(commonTokensOnString).Append(GetHtmlTag(currentTag) ?? "");
            previousTag = currentTag;
        }

        if (previousTag != null)
        {
            var commonTokensStartIndex = CalculateCommonTokensStartIndex(previousTag);
            htmlString.Append(ConvertCommonTokensToString(tokens, commonTokensStartIndex, tokens.Length));
        }

        if (firstTag is { TagType: MarkdownTagType.Heading })
            htmlString.Append(GetHtmlTag(firstTag, true));
        
        return htmlString.ToString();
    }

    private static string ConvertCommonTokensToString(IToken[] tokensInParagraph, int startIndex, int endIndex)
    {
        var tokensOnString = new StringBuilder();

        for (var i = startIndex; i < endIndex; i++)
        {
            tokensOnString.Append(tokensInParagraph[i].Content);
        }

        return tokensOnString.ToString();
    }

    private static int CalculateCommonTokensStartIndex(MarkdownTag tagBeforeCommonTokens)
    {
        if (tagBeforeCommonTokens.TagType == MarkdownTagType.Heading)
            return tagBeforeCommonTokens.Token.PositionInTokens + 2;
        
        return tagBeforeCommonTokens.Token.PositionInTokens + 1;
    }
        
    private static string? GetHtmlTag(MarkdownTag markdownTag, bool isNeedClosingTagForSingle = false)
    {
        return markdownTag.TagType switch
        {
            MarkdownTagType.Heading when isNeedClosingTagForSingle => HtmlTagContentConstants.HeadingClosing,
            MarkdownTagType.Heading => HtmlTagContentConstants.HeadingOpening,
            MarkdownTagType.Bold when markdownTag.IsClosedTag => HtmlTagContentConstants.BoldClosing,
            MarkdownTagType.Bold => HtmlTagContentConstants.BoldOpening,
            MarkdownTagType.Italics when markdownTag.IsClosedTag => HtmlTagContentConstants.ItalicsClosing,
            MarkdownTagType.Italics => HtmlTagContentConstants.ItalicsOpening,
            _ => null
        };
    }
}