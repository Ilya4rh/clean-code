using Markdown.MarkdownTags;

namespace Markdown.MarkdownTagValidators;

public interface IMarkdownTagValidator
{
    /// <summary>
    /// Проверяет валидность тэга 
    /// </summary>
    /// <param name="tagType"> Тип тэга, который проверяется</param>
    /// <param name="positionOnLine"> Позиция тэга в строке </param>
    /// <param name="line"> Строка, в которой находится тэг </param>
    /// <param name="positionCloseTagOnLine"> Позиция закрывающего тэга, если он парный </param>
    /// <param name="externalTagType"> Тип внешнего тэга, для текущего </param>
    /// <returns></returns>
    bool IsValidTag(
        MarkdownTagType tagType, 
        int positionOnLine, 
        string line,
        int? positionCloseTagOnLine = null, 
        MarkdownTagType? externalTagType = null);
}