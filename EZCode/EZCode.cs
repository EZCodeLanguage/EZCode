using System.Text.RegularExpressions;

namespace EZCodeLanguage
{
    public class EZCode
    {
        public class Line
        {
            public string code { get; set; }
            public int line { get; set; }
            public Line(string code, int line)
            {
                this.code = code;
                this.line = line;
            }
        }
        public class Token
        {
            public TokenType Type { get; set; }
            public object Value { get; set; }
            public Line Line {  get; set; }
            public Token(TokenType type, object value, Line line)
            {
                Type = type;
                Value = value; 
                Line = line;
            }
        }
        class Argument
        {
            public Token[] Tokens { get; set; }
            public Line Line { get; set; }
            public string Value { get; set; }
            public Argument(Token[] tokens, Line line, string value)
            {
                Tokens = tokens;
                Line = line;
                Value = value;
            }
            public bool GetValue()
            {
                bool check = false;
                return check;
            }
        }
        class Statement
        {
            public Argument? Argument { get; set; }
            public Line Line { get; set; }
            public LineWithTokens[] InBrackets { get; set; }
            public static string[] Types = ["if", "loop", "else", "try", "fail"];
            public static string[] ConditionalTypes = ["if", "loop"];
            public static string[] NonConditionalTypes = ["else", "try", "fail"];
            public string Type { get; set; }
            public Statement(string type, Line line, LineWithTokens[] insides, Argument? argument = null)
            {
                Type = type;
                Line = line;
                InBrackets = insides;
                if (ConditionalTypes.Contains(type))
                    Argument = argument;
            }
            
        }
        class LineWithTokens
        {
            public Line Line { get; set; }
            public Token[] Tokens { get; set; }
            public LineWithTokens(Token[] tokens, Line line)
            {
                Line = line;
                Tokens = tokens;
            }
        }
        public enum TokenType
        {
            None,
            NewLine,
            Comment,
            Var,
            Equals,
            Not,
            GreaterThan,
            LessThan,
            Colon,
            Arrow,
            DataType,
            OpenParenth,
            CloseParenth,
            OpenBracket,
            CloseBracket,
            OpenCurlyBracket,
            CloseCurlyBracket,
            If,
            Argument,
            Bool,
            Text,
            Number,
            Identifier
        }
        public string Code { get; set; }
        public EZCode() { }
        public EZCode(string code)
        {
            Code = code;
        }
        public Token[] Tokens = Array.Empty<Token>();
        public Token[] Tokenize(string code)
        {
            return Tokens = TokenArray(code);
        }
        private static Token[] TokenArray(string code)
        {
            List<Token> tokens = new List<Token>();
            Line[] Lines = SplitLine(code);
            string[] lines_strings = Lines.Select(x => x.code).ToArray();

            for (int i = 0; i < Lines.Length; i++)
            {
                Line line = Lines[i];
                string line_string = line.code;
                string[] parts = SplitParts(lines_strings, i, out int continues);
                bool oneLine = false;
                for (int j = 0; j < parts.Length; j++)
                {
                    Token token = SingleToken(Lines, i, parts, j, out bool stops, out oneLine);
                    if (token.Type != TokenType.None) tokens.Add(token);
                    if (stops) continue;
                }
                i += continues;
                if (oneLine) tokens.Add(new Token(TokenType.NewLine, Environment.NewLine, line));
            }
            return tokens.ToArray();
        }
        private static Token SingleToken(Line[] Lines, int lineIndex, string[] parts, int partIndex, out bool stops, out bool sameLine)
        {
            Line line = Lines[lineIndex];
            string part = parts[partIndex];
            stops = false;
            sameLine = true;
            TokenType tokenType = TokenType.None;
            switch (part.ToLower())
            {
                default: tokenType = TokenType.Identifier; break;
                case "//": tokenType = TokenType.Comment; stops = true; break;
                case "=": tokenType = TokenType.Equals; break;
                case "var": tokenType = TokenType.Var; break;
                case "!": tokenType = TokenType.Not; break;
                case ">": tokenType = TokenType.GreaterThan; break;
                case "<": tokenType = TokenType.LessThan; break;
                case "=>": tokenType = TokenType.Arrow; break;
                case ":": tokenType = TokenType.Colon; break;
                case "(": tokenType = TokenType.OpenParenth; break;
                case ")": tokenType = TokenType.CloseParenth; break;
                case "[": tokenType = TokenType.OpenBracket; break;
                case "]": tokenType = TokenType.CloseBracket; break;
                case "{": tokenType = TokenType.OpenCurlyBracket; break;
                case "}": tokenType = TokenType.CloseCurlyBracket; break;
                case "if": tokenType = TokenType.If; break;
                case "false": case "true": tokenType = TokenType.Bool; break;
            }
            if (part.StartsWith("@")) tokenType = TokenType.DataType;
            try
            {
                if (parts[partIndex - 1] == "if")
                {
                    Token[] argTokens = TokenArray(part);
                    Argument argument = new Argument(argTokens, line, part);
                    return new Token(TokenType.Argument, argument, line);
                }
            }
            catch { }

            return new Token(tokenType, part, line);
        }
        private static string[] SplitParts(string[] lines, int index, out int continues)
        {
            char[] chars = [ ' ', '(', ')', '[', ']', '{', '}', '!', '=', '@', ':', '>', '<' ];
            string line = lines[index];
            string[] parts = SplitWithDelimiters(line, chars).Where(x => x != "" && x != " ").ToArray();
            continues = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                try
                {
                    if (parts[i] == "-" && parts[i + 1] == ">")
                    {
                        parts[i] = "->";
                        List<string> l = [.. parts];
                        l.RemoveAt(i + 1);
                        parts = [.. l];
                        if (i == parts.Length - 1)
                        {
                            parts = parts.Append(lines[i + 1]).ToArray();
                            continues++;
                        }
                    }
                    else if (parts[i] == "=" && parts[i + 1] == ">")
                    {
                        parts[i] = "=>";
                        List<string> l = [.. parts];
                        l.RemoveAt(i + 1);
                        parts = [.. l];
                    }
                    else if (parts[i] == "/" && parts[i + 1] == "/")
                    {
                        parts[i] = "//";
                        List<string> l = [.. parts];
                        l.RemoveAt(i + 1);
                        parts = [.. l];
                    }
                    else if (parts[i - 1] == "if")
                    {
                        if
                    }
                }
                catch
                {

                }
            }
            return parts;
        }
        private static string[] SplitWithDelimiters(string input, char[] delimiters)
        {
            string pattern = $"({string.Join("|", delimiters.Select(c => Regex.Escape(c.ToString())))})";
            return Regex.Split(input, pattern);
        }
        private static Line[] SplitLine(string code)
        {
            Line[] lines = Array.Empty<Line>();
            int i = 0;
            foreach (var item in code.Split(Environment.NewLine).Select(s => s.Trim()).ToArray())
            {
                lines = lines.Append(new Line(item, i)).ToArray();
                i++;
            };
            return lines;
        }
    }
}