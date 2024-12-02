using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public class MarkdownLineParser : IMarkdownLineParser
{
    public IEnumerable<IToken> ParseParagraphForTokens(string markdownLineParagraph)
    {
        for (var i = 0; i < markdownLineParagraph.Length; i++)
        {
            var symbol = markdownLineParagraph[i];
                
            if (TagToken.TryCreateTagToken(markdownLineParagraph, i, out var tagToken))
            {
                yield return tagToken!;

                MovePointer(tagToken!, ref i);
                    
                continue;
            }
                
            yield return CommonToken.CreateCommonToken(symbol);
        }
    }

    private static void MovePointer(TagToken tagToken, ref int pointer)
    {
        pointer += tagToken.Content.Length - 1;
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