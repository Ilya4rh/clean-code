namespace Markdown.MarkdownTags;

public class MarkdownBoldTag : MarkdownTag
{
    // Закрывающий тэг текущего
    private MarkdownTag? closeTag;
    
    public MarkdownBoldTag(int position, MarkdownTag? closeTag = null) : base(position)
    {
        this.closeTag = closeTag;
    }

    public override int Length { get; }
    
    public override bool IsValid(string markdownLine)
    {
        throw new NotImplementedException();
    }
}