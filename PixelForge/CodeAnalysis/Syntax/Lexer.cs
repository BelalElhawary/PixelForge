namespace PixelForge.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();
        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;
        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out int value))
                    _diagnostics.Add($"The number {text} isn't valid Int32");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);

                return new SyntaxToken(kind, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenToken, _position++, ")", null);
                case '^':
                    return new SyntaxToken(SyntaxKind.CircumflexAccent, _position++, "^", null);
            }

            if (!char.IsWhiteSpace(Current))
                _diagnostics.Add($"ERROR: bad character input: '{Current}' at '{_position}'");

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}