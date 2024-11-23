using Markdown.MarkdownTags;

namespace Markdown.MarkdownTagValidators;

public interface IValidator
{
    /// <summary>
    /// Проверяет валидность одиночного тэга
    /// </summary>
    /// <param name="tagType"> Тип тэга </param>
    /// <param name="positionOnLine"> Позиция тэга в строке </param>
    /// <param name="line"> Строка, в которой находится тэг </param>
    /// <returns></returns>
    bool IsValidSingleTag(MarkdownTagType tagType, int positionOnLine, string line);

    /// <summary>
    /// Проверяет валидность парного тэга
    /// </summary>
    /// <param name="tagType"> Тип тэга </param>
    /// <param name="positionOpenTagOnLine"> Позиция открывающего тэга в строке </param>
    /// <param name="positionCloseTagOnLine"> Позиция закрывающего тэга в строке </param>
    /// <param name="line"> Строк, в которой находятся тэги </param>
    /// <param name="externalTagType"> Тип внешнего тэга, для текущего </param>
    /// <returns></returns>
    bool IsValidPairedTag(
        MarkdownTagType tagType, 
        int positionOpenTagOnLine, 
        int positionCloseTagOnLine, 
        string line,
        MarkdownTagType? externalTagType = null);
}