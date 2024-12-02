using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.Parsers.MarkdownLineParsers;

public class MarkdownLineParser : IMarkdownLineParser
{
    public IEnumerable<IToken> ParseMarkdownLineForTokens(string markdownText)
    {
        var markdownLineParagraphs = ParseMarkdownTextIntoParagraphs(markdownText);

        foreach (var paragraph in markdownLineParagraphs)
        {
            for (var i = 0; i < paragraph.Length; i++)
            {
                var symbol = paragraph[i];
                
                if (TagToken.TryCreateTagToken(paragraph, i, out var tagToken))
                {
                    yield return tagToken!;

                    MovePointer(tagToken!, ref i);
                    
                    continue;
                }
                
                yield return CommonToken.CreateCommonToken(symbol);
            }
            
            yield return new NewLineToken();
        }
    }

    private static void MovePointer(TagToken tagToken, ref int pointer)
    {
        pointer += tagToken.Content.Length - 1;
    } 
    
    private static IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownLine)
    {
        var paragraphs = markdownLine.Split(Environment.NewLine);

        foreach (var paragraph in paragraphs)
        {
            if (!string.IsNullOrWhiteSpace(paragraph))
                yield return paragraph;
        }
    }
}