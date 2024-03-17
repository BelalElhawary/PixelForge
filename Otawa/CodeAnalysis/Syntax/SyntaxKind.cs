namespace Otawa.CodeAnalysis.Syntax
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
        EqualsToken,
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
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssignmentExpression,
    }
}