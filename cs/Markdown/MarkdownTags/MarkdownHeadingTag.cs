namespace Markdown.MarkdownTags;

public class MarkdownHeadingTag : MarkdownTag
{
    public MarkdownHeadingTag(int position) : 
        base(position)
    {
    }

    public override int Length { get; }
    
    public override bool IsValid(string markdownLine)
    {
        throw new NotImplementedException();
    }
}