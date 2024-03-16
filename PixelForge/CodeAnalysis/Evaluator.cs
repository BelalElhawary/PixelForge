using PixelForge.CodeAnalysis.Binding;
using PixelForge.CodeAnalysis.Syntax;

namespace PixelForge.CodeAnalysis
{

    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression root)
        {
            if (root is BoundLiteralExpression n)
                return (int)n.Value;

            if (root is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);
                switch (u.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                }

            }

            if (root is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                var op = b.OperatorKind;

                switch (op)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    case BoundBinaryOperatorKind.Exponentiation:
                        return (int)MathF.Pow(left, right);
                    default:
                        throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                }
            }

            throw new Exception($"Unexpected node {root.Kind}");
        }

    }
}