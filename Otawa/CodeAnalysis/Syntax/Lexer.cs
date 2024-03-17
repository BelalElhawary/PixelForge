namespace Otawa.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private DiagnosticBag _diagnostics = new();
        public Lexer(string text)
        {
            _text = text;
        }

        public DiagnosticBag Diagnostics => _diagnostics;
        private char Current => Peek(0);
        private char Lookahead => Peek(1);
        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];
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
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, length), text, typeof(int));

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
                    return new SyntaxToken(SyntaxKind.CircumflexAccentToken, _position++, "^", null);
                case '!':
                    if (Lookahead == '=')
                    {
                        var token = new SyntaxToken(SyntaxKind.BangEqualsToken, _position, "!=", null);
                        _position += 2;
                        return token;
                    }
                    else
                        return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
                case '=':
                    if (Lookahead == '=')
                    {
                        var token = new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position, "==", null);
                        _position += 2;
                        return token;
                    }
                    else
                        return new SyntaxToken(SyntaxKind.EqualsToken, _position++, "=", null);
            }

            if (!char.IsWhiteSpace(Current))
                _diagnostics.ReportBadCharacter(_position, Current);

            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}