using Markdown.MarkdownTags;

namespace Markdown.Parsers;

public interface IParser
{
    /// <summary>
    /// Метод, который парсит все тэги в строке.
    /// Найденные тэги будут добавляться в лист, чтобы была возможность получить доступ к уже добавленным(внешним) тэгам.
    /// </summary>
    /// <param name="markdownLineParagraph"> Абзац строки </param>
    /// <returns> Список тэгов </returns>
    IEnumerable<MarkdownTag> ParseMarkdownTags(string markdownLineParagraph);
}