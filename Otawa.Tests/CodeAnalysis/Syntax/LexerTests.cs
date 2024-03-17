using Otawa.CodeAnalysis.Syntax;

namespace Otawa.Tests.CodeAnalysis.Syntax;

public class LexerTests
{
    [Theory]
    [MemberData(nameof(GetTokensData))]
    public void Lexer_Lexes_Token(SyntaxKind kind, string text)
    {
        var tokens = SyntaxTree.ParseTokens(text);

        var token = Assert.Single(tokens);
        Assert.Equal(kind, token.Kind);
        Assert.Equal(text, token.Text);
    }

    [Theory]
    [MemberData(nameof(GetTokenPairsData))]
    public void Lexer_Lexes_TokenPairs(SyntaxKind t1kind, string t1text, SyntaxKind t2kind, string t2text)
    {
        var text = t1text + t2text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();

        Assert.Equal(2, tokens.Length);

        Assert.Equal(tokens[0].Text, t1text);
        Assert.Equal(tokens[1].Text, t2text);
        Assert.Equal(tokens[0].Kind, t1kind);
        Assert.Equal(tokens[1].Kind, t2kind);
    }

    public static IEnumerable<object[]> GetTokensData()
    {
        foreach (var t in GetTokens())
            yield return new object[] { t.kind, t.text };
    }

    public static IEnumerable<object[]> GetTokenPairsData()
    {
        foreach (var t in GetTokenPairs())
            yield return new object[] { t.t1kind, t.t1text, t.t2kind, t.t2text };
    }

    private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
    {
        return [
            (SyntaxKind.PlusToken, "+"),
            (SyntaxKind.MinusToken, "-"),
            (SyntaxKind.SlashToken, "/"),
            (SyntaxKind.StarToken, "*"),
            (SyntaxKind.BangToken, "!"),
            (SyntaxKind.EqualsToken, "="),
            (SyntaxKind.CircumflexAccentToken, "^"),
            (SyntaxKind.EqualsEqualsToken, "=="),
            (SyntaxKind.BangEqualsToken, "!="),
            (SyntaxKind.OpenParenToken, "("),
            (SyntaxKind.CloseParenToken, ")"),
            (SyntaxKind.FalseKeyword, "false"),
            (SyntaxKind.TrueKeyword, "true"),
            (SyntaxKind.AndKeyword, "and"),
            (SyntaxKind.OrKeyword, "or"),

            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "abc"),
            (SyntaxKind.IdentifierToken, "_a"),
            (SyntaxKind.IdentifierToken, "a_"),
            (SyntaxKind.IdentifierToken, "_1"),
            (SyntaxKind.IdentifierToken, "a1"),
            (SyntaxKind.IdentifierToken, "a1b2c3"),
            (SyntaxKind.NumberToken, "1"),
            (SyntaxKind.NumberToken, "1230"),
        ];
    }

    private static bool RequiresSeparator(SyntaxKind t1kind, SyntaxKind t2kind)
    {
        var t1IsKeyword = t1kind.ToString().EndsWith("Keyword");
        var t2IsKeyword = t2kind.ToString().EndsWith("Keyword");

        if (t1kind == SyntaxKind.IdentifierToken || t2kind == SyntaxKind.IdentifierToken)
            return true;

        if (t1kind == SyntaxKind.NumberToken || t2kind == SyntaxKind.NumberToken)
            return true;

        if (t1IsKeyword && t2IsKeyword)
            return true;

        if (t1IsKeyword && t2kind == SyntaxKind.IdentifierToken)
            return true;

        if (t1kind == SyntaxKind.IdentifierToken && t2IsKeyword)
            return true;

        if (t1kind == SyntaxKind.BangToken || t2kind == SyntaxKind.EqualsToken)
            return true;

        if (t1kind == SyntaxKind.BangToken || t2kind == SyntaxKind.EqualsEqualsToken)
            return true;

        if (t1kind == SyntaxKind.EqualsToken || t2kind == SyntaxKind.EqualsToken)
            return true;

        if (t1kind == SyntaxKind.EqualsToken || t2kind == SyntaxKind.EqualsEqualsToken)
            return true;

        return false;
    }

    private static IEnumerable<(SyntaxKind t1kind, string t1text, SyntaxKind t2kind, string t2text)> GetTokenPairs()
    {
        foreach (var t1 in GetTokens())
        {
            foreach (var t2 in GetTokens())
            {
                if (!RequiresSeparator(t1.kind, t2.kind))
                    yield return (t1.kind, t1.text, t2.kind, t2.text);
            }
        }
    }
}