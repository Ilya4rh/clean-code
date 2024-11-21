namespace Markdown.MarkdownTags;

public abstract class MarkdownTag
{
    // Позиция тэга в строке
    private int position;
    
    public MarkdownTag(int position)
    {
        this.position = position;
    }

    /// <summary>
    /// Длина тэга
    /// </summary>
    public abstract int Length { get; }
    
    /// <summary>
    /// Проверка валидности тэга
    /// </summary>
    /// <returns></returns>
    public abstract bool IsValid(string markdownLine);
}