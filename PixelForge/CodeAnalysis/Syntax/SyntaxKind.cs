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
        CircumflexAccentToken,
        OpenParenToken,
        CloseParenToken,
        IdentifierToken,
        BangToken,


        // Keywords
        FalseKeyword,
        TrueKeyword,
        AndKeyword,
        OrKeyword,

        // Expression
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,

    }
}