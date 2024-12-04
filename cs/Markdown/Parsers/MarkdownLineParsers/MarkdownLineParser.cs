using System.Text;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public class MarkdownLineParser : IMarkdownLineParser
{
    public IEnumerable<IToken> ParseParagraphForTokens(string markdownLineParagraph)
    {
        var previousTokenPosition = -1;
        var previousTokenLength = -1;
        
        for (var i = 0; i < markdownLineParagraph.Length; i++)
        {
            if (i < previousTokenLength + previousTokenPosition)
                continue;
            
            previousTokenPosition = i;
            var symbol = markdownLineParagraph[i];
                
            if (TagToken.TryCreateTagToken(markdownLineParagraph, i, out var tagToken))
            {
                yield return tagToken!;
                previousTokenLength = tagToken!.Content.Length;
                continue;
            }

            var commonTokenType = CommonToken.GetCommonTokenType(symbol);
            var tokenContent = symbol.ToString();
            
            if (commonTokenType == TokenType.Text)
                tokenContent += CompleteTextTokenContent(markdownLineParagraph, i + 1, commonTokenType);

            var commonToken = CommonToken.CreateCommonToken(commonTokenType, tokenContent);
            yield return commonToken;
            previousTokenLength = commonToken.Content.Length;
        }
    }

    private static string CompleteTextTokenContent(string line, int position, TokenType tokenType)
    {
        var content = new StringBuilder();
        
        for (; position < line.Length; position++)
        {
            var currentTokenType = CommonToken.GetCommonTokenType(line[position]);
            
            if (tokenType != currentTokenType || TagToken.TryCreateTagToken(line, position, out _))
                break;

            content.Append(line[position]);
        }

        return content.ToString();
    }
    
    public IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownText)
    {
        var paragraphs = markdownText.Split(Environment.NewLine);

        foreach (var paragraph in paragraphs)
        {
            if (!string.IsNullOrWhiteSpace(paragraph))
                yield return paragraph;
        }
    }
}