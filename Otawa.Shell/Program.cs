using Otawa;
using Otawa.CodeAnalysis;
using Otawa.CodeAnalysis.Syntax;

internal static class Program
{
    private static void Main()
    {
        var variables = new Dictionary<VariableSymbol, object?>();

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
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(variables);
            var diagnostics = result.Diagnostics;


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
                    Console.WriteLine(result.Value);
                }
            }
            else
            {
                if (errors)
                {
                    foreach (var diagnostic in diagnostics)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);
                        Console.WriteLine();

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