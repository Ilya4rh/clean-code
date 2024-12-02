using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public class MarkdownTagValidator : IMarkdownTagValidator
{
    public bool IsValidTag(
        List<IToken> paragraphOfTokens, 
        TagToken openingTagToken, 
        TagToken? closingTagToken = null,
        MarkdownTagType? externalTagType = null)
    {
        return openingTagToken.MarkdownTagType switch
        {
            MarkdownTagType.Heading => IsValidHeadingTag(openingTagToken, paragraphOfTokens),
            MarkdownTagType.Italics => false,
            MarkdownTagType.Bold => false,
            _ => false
        };
    }
    
    
    private static bool IsValidHeadingTag(TagToken tagToken, List<IToken> paragraphOfTokens)
    {
        var position = tagToken.PositionInTokens;

        if (paragraphOfTokens.Count < 2)
            return false;

        return position == 0 && paragraphOfTokens[position + 1].Type == TokenType.Space;
    }
    
    private static bool IsValidHeadingTag(int positionOnLine, string line)
    {
        switch (positionOnLine)
        {
            case >= 4:
                return false;
            case > 0:
            {
                for (var i = 0; i < positionOnLine; i++)
                {
                    if (line[i] != ' ')
                        return false;
                }

                break;
            }
        }

        return positionOnLine + 1 >= line.Length || line[positionOnLine + 1] == ' ';
    }

    private static bool IsValidItalicsTag(
        int positionOpenTagOnLine,
        int positionCloseTagOnLine,
        string line)
    {
        return IsValidPairedTag(positionOpenTagOnLine, positionCloseTagOnLine, line, 1);
    }

    private static bool IsValidBoldTag(
        int positionOpenTagOnLine,
        int positionCloseTagOnLine,
        string line,
        MarkdownTagType? externalTagType)
    {
        if (positionOpenTagOnLine + 2 == positionCloseTagOnLine)
            return false;
        if (externalTagType is MarkdownTagType.Italics)
            return false;

        return IsValidPairedTag(positionOpenTagOnLine, positionCloseTagOnLine, line, 2);
    }
    
    private static bool IsValidPairedTag( 
        int positionOpenTagOnLine,
        int positionCloseTagOnLine,
        string line,
        int tagLength)
    {
        if (IsTagNotHaveSpaces(positionOpenTagOnLine, positionCloseTagOnLine, line, tagLength))
            return false;
        if (IsTagsInDifferentWords(positionOpenTagOnLine, positionCloseTagOnLine, tagLength, line))
            return false;
        if (IsTagsInsideTheTextWithNumbers(positionOpenTagOnLine, positionCloseTagOnLine, tagLength, line))
            return false;
        
        return !IsEscapedTag(positionOpenTagOnLine, positionCloseTagOnLine, line);
    }

    private static bool IsTagNotHaveSpaces(int positionOpenTag, int positionCloseTag, string line, int tagLength)
    {
        return line[positionOpenTag + tagLength] == ' ' || line[positionCloseTag-1] == ' ';
    }

    private static bool IsTagsInDifferentWords(
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine, 
        int tagLength, 
        string line)
    {
        if (positionOpenTagOnLine == 0 && positionCloseTagOnLine + tagLength == line.Length)
            return false;

        if (positionOpenTagOnLine != 0 && line[positionOpenTagOnLine - 1] == ' ' &&
            positionCloseTagOnLine != line.Length - tagLength && line[positionCloseTagOnLine + tagLength] == ' ') 
            return false;
        
        var lineBetweenTags = 
            line.Substring(positionOpenTagOnLine, positionCloseTagOnLine - positionOpenTagOnLine);
        
        return lineBetweenTags.Contains(' ');
    }

    private static bool IsTagsInsideTheTextWithNumbers(
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine,
        int tagLength,
        string line)
    {

        if (positionOpenTagOnLine != 0 &&
            IsTagOnNumber(line[positionOpenTagOnLine - 1], line[positionOpenTagOnLine + tagLength]))
            return true;
        
        if (positionCloseTagOnLine + tagLength != line.Length && 
            IsTagOnNumber(line[positionCloseTagOnLine - 1], line[positionCloseTagOnLine + tagLength]))
            return true;

        return false;
    }

    private static bool IsTagOnNumber(char symbolToLeftOfTag, char symbolToRightOfTag)
    {
        var numbers = new HashSet<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        return numbers.Contains(symbolToLeftOfTag) && numbers.Contains(symbolToRightOfTag);
    }

    private static bool IsEscapedTag(int positionOpenTagOnLine, int positionCloseTagOnLine, string line)
    {
        int numberOfEscape;
        
        if (positionOpenTagOnLine > 0 && line[positionOpenTagOnLine - 1] == '\\')
        {
            numberOfEscape = CalculateNumberOfEscapeChar(positionOpenTagOnLine - 1, line);
            
            if (numberOfEscape % 2 != 0)
                return true;
        }

        if (line[positionCloseTagOnLine - 1] != '\\') 
             return false;
        
        numberOfEscape = CalculateNumberOfEscapeChar(positionCloseTagOnLine - 1, line);
        
        return numberOfEscape % 2 != 0;
    }

    private static int CalculateNumberOfEscapeChar(int positionFirstEscapeChar, string line)
    {
        var numberOfEscape = 1;

        if (positionFirstEscapeChar == 0)
            return numberOfEscape;

        for (var i = positionFirstEscapeChar - 1; i >= 0; i--)
        {
            if (line[i] != '\\')
                break;

            numberOfEscape++;
        }

        return numberOfEscape;
    }
}