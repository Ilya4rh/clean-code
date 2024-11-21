using Markdown.Converters;
using Markdown.Parsers;

namespace Markdown;

public class Md
{
    private readonly IParser parser;
    private readonly IConverter converter;

    public Md(IParser parser, IConverter converter)
    {
        this.parser = parser;
        this.converter = converter;
    }

    public string Render(string markDownString)
    {
        // TODO 
        throw new NotImplementedException();
    }
}