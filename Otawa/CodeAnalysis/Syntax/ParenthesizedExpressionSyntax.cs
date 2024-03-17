namespace Otawa.CodeAnalysis.Syntax
{
    public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParen, ExpressionSyntax expression, SyntaxToken closedParen)
        {
            OpenParen = openParen;
            Expression = expression;
            ClosedParen = closedParen;
        }

        public SyntaxToken OpenParen { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken ClosedParen { get; }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParen;
            yield return Expression;
            yield return ClosedParen;
        }
    }
}