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
        
        
        if (tagType == MarkdownTagType.Heading)
            return IsValidHeadingTag(positionOnLine, line);
        
        throw new NotImplementedException();
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

        return positionOnLine == 0 || line[positionOnLine + 1] == ' ';
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