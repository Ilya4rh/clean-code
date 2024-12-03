using FluentAssertions;
using Markdown.Parsers.MarkdownLineParsers;
using Markdown.Tokens;
using Markdown.Tokens.CommonTokens;
using Markdown.Tokens.TagTokens;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownLineParserTests
{
    private readonly MarkdownLineParser markdownLineParser = new MarkdownLineParser();

    [Test]
    public void ParseParagraphForTokens_ShouldDefineHeadingTagToken()
    {
        const string paragraph = "#";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new HeadingTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineBoldTagToken()
    {
        const string paragraph = "__";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new BoldTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineItalicsTagToken()
    {
        const string paragraph = "_";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new ItalicsTagToken(0)]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineDigitToken()
    {
        const string paragraph = "1";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new DigitToken("1")]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineSpaceToken()
    {
        const string paragraph = " ";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new SpaceToken()]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineScreeningToken()
    {
        const string paragraph = "\\";
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo([new EscapeToken()]);
    }
    
    [Test]
    public void ParseParagraphForTokens_ShouldDefineTextToken()
    {
        const string paragraph = "Abc!";
        var expectedTokens = new List<IToken>
        {
            new TextToken("A"), 
            new TextToken("b"), 
            new TextToken("c"), 
            new TextToken("!")
        };
        
        var paragraphOfTokens = markdownLineParser.ParseParagraphForTokens(paragraph);

        paragraphOfTokens.Should().BeEquivalentTo(expectedTokens);
    }
}