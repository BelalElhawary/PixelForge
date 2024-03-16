using PixelForge.CodeAnalysis;
using PixelForge.CodeAnalysis.Binding;
using PixelForge.CodeAnalysis.Syntax;

internal static class Program
{
    private static void Main()
    {
        bool tree = false;
        bool errors = true;
        bool nulls = false;
        bool evaluate = true;
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (line == "@tree")
            {
                tree = !tree;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(tree ? "Showing parse trees." : "Not showing parse trees.");
                Console.ResetColor();
                continue;
            }
            else if (line == "@errors")
            {
                errors = !errors;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(errors ? "Showing errors." : "Not showing errors.");
                Console.ResetColor();
                continue;
            }
            else if (line == "@nulls")
            {
                nulls = !nulls;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(nulls ? "Allowing nulls." : "Not allowing nulls.");
                Console.ResetColor();
                continue;
            }
            else if (line == "@eval")
            {
                evaluate = !evaluate;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(evaluate ? "Enable evaluating code." : "Disable evaluating code.");
                Console.ResetColor();
                continue;
            }

            var syntaxTree = SyntaxTree.Parse(line);
            var binder = new Binder();
            var boundExpression = binder.BindExpression(syntaxTree.Root);
            var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();


            if (tree)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrettyPrint(syntaxTree.Root);
            }


            if (!diagnostics.Any())
            {
                if (evaluate)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    var e = new Evaluator(boundExpression);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
            }
            else
            {
                if (errors)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (var diagnostic in diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
            }
            Console.ResetColor();
        }
    }

    private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
    {
        var marker = isLast ? "└───" : "├───";

        Console.Write(indent);
        Console.Write(marker);
        Console.Write(node.Kind);

        if (node is SyntaxToken t && t.Value != null)
        {
            Console.Write(" ");
            Console.Write(t.Value);
        }

        Console.WriteLine();

        indent += isLast ? "    " : "│   ";
        var lastChild = node.GetChildren().LastOrDefault();
        foreach (var child in node.GetChildren())
            PrettyPrint(child, indent, child == lastChild);
    }
}