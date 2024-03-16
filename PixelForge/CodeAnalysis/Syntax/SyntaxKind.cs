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
        BangToken,
        CircumflexAccentToken,
        EqualsEqualsToken,
        BangEqualsToken,
        OpenParenToken,
        CloseParenToken,
        IdentifierToken,



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