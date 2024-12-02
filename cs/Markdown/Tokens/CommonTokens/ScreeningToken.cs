namespace Markdown.Tokens.CommonTokens;

public class ScreeningToken : CommonToken
{
    public override TokenType Type => TokenType.Screening;

    public override string Content => @"\";
}