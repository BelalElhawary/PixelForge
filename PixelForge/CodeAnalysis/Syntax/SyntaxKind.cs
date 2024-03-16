namespace PixelForge.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        NumberToken,
        // WhiteSpaceToken,
        PlusToken,
        MinusToken,
        SlashToken,
        StarToken,
        CircumflexAccent,
        OpenParenToken,
        CloseParenToken,

        // Expression
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
    }
}