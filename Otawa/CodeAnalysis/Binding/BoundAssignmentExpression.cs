namespace Otawa.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(string name, BoundExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmantExpression;
        public override Type Type => Expression.Type;
        public string Name { get; }
        public BoundExpression Expression { get; }
    }
}
