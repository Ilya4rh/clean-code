namespace Markdown.Tokens.CommonTokens;

public class EscapeToken : CommonToken
{
    public override TokenType Type => TokenType.Screening;

    public override string Content => @"\";
}