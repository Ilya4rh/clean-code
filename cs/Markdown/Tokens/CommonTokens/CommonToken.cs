namespace Markdown.Tokens.CommonTokens;

public abstract class CommonToken : IToken
{
    public abstract TokenType Type { get; }
    public abstract string Content { get; }

    public static CommonToken CreateCommonToken(char symbol)
    {
        if (char.IsDigit(symbol))
            return new DigitToken(symbol.ToString());
        if (char.IsWhiteSpace(symbol))
            return new SpaceToken();
        if (symbol == '\\')
            return new EscapeToken();

        return new TextToken(symbol.ToString());
    }
}