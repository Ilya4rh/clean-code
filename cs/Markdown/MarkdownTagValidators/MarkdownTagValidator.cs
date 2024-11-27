using Markdown.MarkdownTags;

namespace Markdown.MarkdownTagValidators;

public class MarkdownTagValidator : IValidator
{
    public bool IsValidTag(
        MarkdownTagType tagType, 
        int positionOnLine, 
        string line, 
        int? positionCloseTagOnLine = null,
        MarkdownTagType? externalTagType = null)
    {
        if (positionOnLine < 0 || positionOnLine >= line.Length)
        {
            throw new ArgumentException(
                "The 'positionOnLine' is less than zero or greater than the length of the string.");
        }

        if (positionCloseTagOnLine != null && (positionCloseTagOnLine < 0 || positionCloseTagOnLine >= line.Length))
        {
            throw new ArgumentException(
                "The 'positionCloseTagOnLine' is less than zero or greater than the length of the string.");
        }

        return tagType switch
        {
            MarkdownTagType.Heading => IsValidHeadingTag(positionOnLine, line),
            MarkdownTagType.Italics when positionCloseTagOnLine != null 
                => IsValidItalicsPairedTag(positionOnLine, positionCloseTagOnLine.Value, line),
            MarkdownTagType.Bold when positionCloseTagOnLine != null => 
                IsValidBoldTag(positionOnLine, positionCloseTagOnLine.Value, line, externalTagType),
            _ => throw new NotImplementedException()
        };
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

    private static bool IsValidItalicsPairedTag(
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine,
        string line)
    {
        if (line[positionOpenTagOnLine + 1] == ' ')
            return false;
        if (line[positionCloseTagOnLine - 1] == ' ')
            return false;
        if (positionOpenTagOnLine + 1 == positionCloseTagOnLine)
            return false;
        if (IsTagsInDifferentWords(positionOpenTagOnLine, positionCloseTagOnLine, 2, line))
            return false;
        if (IsTagsInsideTheTextWithNumbers(positionOpenTagOnLine, positionCloseTagOnLine, 1, line))
            return false;
        if (IsEscapedTag(positionOpenTagOnLine, positionCloseTagOnLine, line))
            return false;
        
        return true;
    }

    private static bool IsValidBoldTag(
        int positionOpenTagOnLine,
        int positionCloseTagOnLine,
        string line,
        MarkdownTagType? externalTagType)
    {
        if (line[positionOpenTagOnLine + 2] == ' ')
            return false;
        if (line[positionCloseTagOnLine - 1] == ' ')
            return false;
        if (positionOpenTagOnLine + 2 == positionCloseTagOnLine)
            return false;
        if (IsTagsInDifferentWords(positionOpenTagOnLine, positionCloseTagOnLine, 2, line))
            return false;
        if (IsTagsInsideTheTextWithNumbers(positionOpenTagOnLine, positionCloseTagOnLine, 2, line))
            return false;
        if (IsEscapedTag(positionOpenTagOnLine, positionCloseTagOnLine, line))
            return false;
        if (externalTagType is MarkdownTagType.Italics)
            return false;
        
        return true;
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
            line[positionCloseTagOnLine + tagLength] == ' ' && positionCloseTagOnLine != line.Length - 1) return false;
        
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
    
    // private bool IsValidPairedTag(
    //     MarkdownTagType tagType, 
    //     int positionOpenTagOnLine, 
    //     int positionCloseTagOnLine, 
    //     string line,
    //     MarkdownTagType? externalTag = null)
    // {
    //     throw new NotImplementedException();
    // }
}