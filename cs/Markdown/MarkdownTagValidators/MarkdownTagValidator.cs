using Markdown.MarkdownTags;

namespace Markdown.MarkdownTagValidators;

public class MarkdownTagValidator : IValidator
{
    public bool IsValidSingleTag(MarkdownTagType tagType, int positionOnLine, string line)
    {
        throw new NotImplementedException();
    }

    public bool IsValidPairedTag(
        MarkdownTagType tagType, 
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine, 
        string line,
        MarkdownTagType? externalTag = null)
    {
        throw new NotImplementedException();
    }
}