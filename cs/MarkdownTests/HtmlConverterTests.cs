using FluentAssertions;
using Markdown.Converters;
using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class HtmlConverterTests
{
    private IHtmlConverter htmlConverter;
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        htmlConverter = new HtmlConverter();
    }

    [TestCaseSource(nameof(_testCases))]
    public void Test(TestCase testCase, Expected expected)
    {
        var htmlSting = htmlConverter.Convert(testCase.TokensInParagraph, testCase.TagsInParagraph);

        htmlSting.Should().Be(expected.HtmlString);
    }

    private static IEnumerable<TestCaseData> _testCases = new[]
    {
        new TestCaseData
            (
                new TestCase
                {
                    TokensInParagraph = 
                    [
                        new TextToken("Здесь"),
                        new SpaceToken(),
                        new TextToken("нет"),
                        new SpaceToken(),
                        new TextToken("тегов")
                    ],
                    TagsInParagraph = []
                },
                new Expected
                {
                    HtmlString = "Здесь нет тегов"
                })
            .SetName("01. Начальная строка: 'Здесь нет тегов'. В строке отсутствуют теги."),
        
        new TestCaseData
            (
                new TestCase
                {
                    TokensInParagraph = 
                    [
                        new HeadingTagToken(0),
                        new SpaceToken(),
                        new TextToken("Текст"),
                        new SpaceToken(),
                        new TextToken("с"),
                        new SpaceToken(),
                        new TextToken("заголовком")
                    ],
                    TagsInParagraph = [new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading)]
                },
                new Expected
                {
                    HtmlString = "<h1>Текст с заголовком</h1>"
                })
            .SetName("02. Начальная строка: '# Текст с заголовком'. В строке единственный тег-заголовок."),
        
        new TestCaseData
            (
                new TestCase
                {
                    TokensInParagraph = 
                    [
                        new TextToken("Текст"),
                        new SpaceToken(),
                        new TextToken("с"),
                        new SpaceToken(),
                        new BoldTagToken(4),
                        new TextToken("полужирным"),
                        new BoldTagToken(6),
                        new SpaceToken(),
                        new TextToken("тегом")
                    ],
                    TagsInParagraph = 
                    [
                        new MarkdownTag(new BoldTagToken(4), MarkdownTagType.Bold),
                        new MarkdownTag(new BoldTagToken(6), MarkdownTagType.Bold, true),
                    ]
                },
                new Expected
                {
                    HtmlString = "Текст с <strong>полужирным</strong> тегом"
                })
            .SetName("03. Начальная строка: 'Текст с __полужирным__ тегом'. В строке единственный тег-полужирный."),
        
        new TestCaseData
            (
                new TestCase
                {
                    TokensInParagraph = 
                    [
                        new TextToken("Текст"),
                        new SpaceToken(),
                        new TextToken("с"),
                        new SpaceToken(),
                        new ItalicsTagToken(4),
                        new TextToken("курсивом"),
                        new ItalicsTagToken(6),
                    ],
                    TagsInParagraph = 
                    [
                        new MarkdownTag(new ItalicsTagToken(4), MarkdownTagType.Italics),
                        new MarkdownTag(new ItalicsTagToken(6), MarkdownTagType.Italics, true),
                    ]
                },
                new Expected
                {
                    HtmlString = "Текст с <em>курсивом</em>"
                })
            .SetName("04. Начальная строка: 'Текст с _курсивом_'. В строке единственный тег-курсив."),
        
        new TestCaseData
            (
                new TestCase
                {
                    TokensInParagraph = 
                    [
                        new HeadingTagToken(0),
                        new SpaceToken(),
                        new TextToken("Заголовок"),
                        new SpaceToken(),
                        new BoldTagToken(4),
                        new TextToken("с"),
                        new SpaceToken(),
                        new ItalicsTagToken(7),
                        new TextToken("разными"),
                        new ItalicsTagToken(9),
                        new SpaceToken(),
                        new TextToken("символами"),
                        new BoldTagToken(12),
                    ],
                    TagsInParagraph = 
                    [
                        new MarkdownTag(new HeadingTagToken(0), MarkdownTagType.Heading),
                        new MarkdownTag(new BoldTagToken(4), MarkdownTagType.Bold),
                        new MarkdownTag(new BoldTagToken(12), MarkdownTagType.Bold, true),
                        new MarkdownTag(new ItalicsTagToken(7), MarkdownTagType.Italics),
                        new MarkdownTag(new ItalicsTagToken(9), MarkdownTagType.Italics, true),
                    ]
                },
                new Expected
                {
                    HtmlString = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
                })
            .SetName("05. Начальная строка: '# Заголовок __с _разными_ символами__'. В строке присутствуют все типы тегов."),
    };

    public record TestCase
    {
        public required IEnumerable<IToken> TokensInParagraph { get; init; }
        
        public required IEnumerable<MarkdownTag> TagsInParagraph { get; init; }
    }

    public record Expected
    {
        public required string HtmlString { get; init; }
    }
}