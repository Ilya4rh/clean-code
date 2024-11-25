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
        throw new NotImplementedException();
    }

    private bool IsValidSingleTag(MarkdownTagType tagType, int positionOnLine, string line)
    {
        throw new NotImplementedException();
    }

    private bool IsValidPairedTag(
        MarkdownTagType tagType, 
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine, 
        string line,
        MarkdownTagType? externalTag = null)
    {
        throw new NotImplementedException();
    }
}