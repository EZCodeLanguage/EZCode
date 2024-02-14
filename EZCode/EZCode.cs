using System.Text.RegularExpressions;

namespace EZCodeLanguage
{
    public class EZCode
    {
        public class Token
        {
            public TokenType Type { get; set; }
            public object Value { get; set; }
            public Token(TokenType type, object value)
            {
                Type = type;
                Value = value; 
            }
        }
        public class Line
        {
            public string Value { get; set; }
            public int CodeLine { get; set; }
            public Line(string code, int line)
            {
                this.Value = code;
                this.CodeLine = line;
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
                {
                    Argument = argument;
                }
            }
            
        }
        class DataType
        {
            public enum Types {
                NotSet,
                _object,
                _string,
                _int,
                _float,
                _bool,
                _char,
                _double,
                _decimal,
                _long,
                _uint,
                _ulong,
            }
            public Types Type { get; set; }
            public Class? ObjectClass { get; set; }
            public DataType(Types type, Class? _class)
            {
                Type = type;
                ObjectClass = _class;
            }
            public DataType() { }
            public static DataType UnSet = new DataType() { Type = Types.NotSet };
            public static DataType GetType(string param, Class[] classes)
            {
                Types types = new();
                Class _class = new();
                switch (param)
                {
                    case "@string": case "@str": types = Types._string; break;
                    case "@int": types = Types._int; break;
                    case "@float": types = Types._float; break;
                    case "@bool": types = Types._bool; break;
                    default: 
                        types = Types._object; 
                        _class = classes.FirstOrDefault(x=>x.Name == param.Replace("@", ""), null); 
                        break;
                }
                return new(types, _class);
            }
        }
        class ExplicitWatch
        {
            public string Pattern { get; set; }
            public Var[]? Vars { get; private set; }
            public RunMethod Runs { get; set; }
            public ExplicitWatch(string format, RunMethod run, Var[] vars)
            {
                Pattern = format.Replace("{", "(?<").Replace("}", ">\\w+)");
                Runs = run;
                Vars = vars;
            }
            public bool IsFound(string[] parts, int index, Class[] classes) => IsFound(parts, index, classes, out int cont);
            public bool IsFound(string[] parts, int index, Class[] classes, out int continues)
            {
                continues = 0;
                for (int i = parts.Length - 1; i >= index; i--)
                {
                    string input = string.Join("", parts.Take(i));
                    continues = i - index;
                    if (IsFound(input, classes)) return true;
                }
                return false;
            }
            public bool IsFound(string input, Class[] classes)
            {
                Match match = Regex.Match(input, Pattern);
                if (match.Success)
                {
                    GroupCollection groups = match.Groups;
                    int groupCount = groups.Count;
                    string[] capturedValues = new string[groupCount - 2];
                    for (int i = 1; i < groupCount - 1; i++)
                    {
                        capturedValues[i - 1] = groups[i].Value;
                    }
                    Var[] vars = [];
                    for (int i = 0; i < capturedValues.Length; i++)
                    {
                        string name = capturedValues[i], type = "";
                        if (capturedValues[i].Contains(":"))
                        {
                            type = capturedValues[i].Split(":")[0];
                            name = capturedValues[i].Split(":")[1];
                        }
                        vars = vars.Append(new Var(name, capturedValues[i], Vars[i].Line, type != "" ? DataType.GetType(type, classes) : DataType.UnSet)).ToArray();
                    }
                    Runs.Parameters = vars;
                    return true;
                }
                return false;
            }
        }
        class ExplicitParams
        {
            public string Format { get; set; }
            public Var[]? Vars { get; private set; }
            public RunMethod Runs { get; set; }
            public ExplicitParams(string format, RunMethod run, Var[] vars)
            {
                Format = format;
                Runs = run;
                Vars = vars;
            }
        }
        class RunMethod(Method method, Var[]? vars)
        {
            public Method Runs = method;
            public Var[]? Parameters = vars;
        }
        class Method
        {
            public string Name { get; set; }
            public Line Line { get; set; }
            [Flags] public enum MethodSettings
            {
                None = 0,
                Static = 1,
                NoCol = 2,
            }
            public MethodSettings Settings { get; set; }
            public LineWithTokens[] Lines { get; set; }
            public DataType? Returns { get; set; } 
            public Var[]? Params { get; set; }
            public Method(string name, Line line, MethodSettings methodSettings, LineWithTokens[] lines, Var[]? param = null, DataType? returns = null)
            {
                Name = name;
                Line = line;
                Settings = methodSettings;
                Lines = lines;
                Params = param;
                Returns = returns;
            }
            public Method() { }
        }
        class GetValueMethod
        {
            public DataType DataType { get; set; }
            public Method Method { get; set; }
            public GetValueMethod() { }
            public GetValueMethod(DataType dataType, Method method)
            {
                DataType = dataType; 
                Method = method;
            }
        }
        class Class
        {
            public GetValueMethod[]? GetTypes { get; set; }
            public ExplicitWatch[] WatchFormat { get; set; }
            public DataType? TypeOf { get; set; }
            public ExplicitParams Params { get; set; }
            public string Name { get; set; }
            public Line Line { get; set; }
            public Method[]? Methods { get; set; }
            public Var[]? Properties { get; set; }
            [Flags] public enum ClassSettings
            {
                None = 0,
                Static = 1,
                Semi = 2,
                Ontop = 4
            }
            public ClassSettings Settings { get; set; }
            public Class() { }
            public Class(string name, Line line, Method[]? methods = null, ClassSettings settings = ClassSettings.None, Var[]? properties = null, ExplicitWatch[]? watchForamt = null, ExplicitParams? explicitParams = null, DataType? datatype = null, GetValueMethod[]? getValue = null)
            {
                Name = name;
                Line = line;
                Methods = methods ?? [];
                Settings = settings;
                Properties = properties ?? [];
                WatchFormat = watchForamt ?? [];
                Params = explicitParams;
                TypeOf = datatype;
                GetTypes = getValue ?? [];
            }

        }
        class Var
        {
            public string? Name { get; set; }
            public object? Value { get; set; }
            public DataType DataType { get; set; }
            public Line Line { get; set; }
            public Var() { }
            public Var(string? name, object? value, Line line, DataType? type = null)
            {
                type ??= DataType.UnSet;
                Name = name;
                Value = value;
                Line = line;
                DataType = type;
            }
            public object? GetFromType(DataType data)
            {
                object? value = null;
                switch (data.Type)
                {
                    case DataType.Types._object:
                        break;
                    case DataType.Types._string:
                        break;
                    case DataType.Types._int:
                        break;
                    case DataType.Types._float:
                        break;
                    case DataType.Types._bool:
                        break;
                    case DataType.Types._char:
                        break;
                    case DataType.Types._double:
                        break;
                    case DataType.Types._decimal:
                        break;
                    case DataType.Types._long:
                        break;
                    case DataType.Types._uint:
                        break;
                    case DataType.Types._ulong:
                        break;
                }
                return value;
            }
        }
        public class LineWithTokens
        {
            public Line Line { get; set; }
            public Token[] Tokens { get; set; }
            public LineWithTokens() { }
            public LineWithTokens(Token[] tokens, Line line)
            {
                Line = line;
                Tokens = tokens;
            }
        }
        public enum TokenType
        {
            None,
            Comment,
            Comma,
            Equals,
            Not,
            GreaterThan,
            LessThan,
            Nothing,
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
            Else,
            Loop,
            Try,
            Fail,
            Argument,
            Identifier,
            Var,
            Class,
            Static,
            Explicit,
            Watch,
            Params,
            TypeOf,
            InsideOf,
            Semi,
            Ontop,
            NoCol,
            Method,
            Container,
            Return,
            Get,
            EZCODEMETHOD
        }
        public string Code { get; set; }
        private List<Class> Classes = [];
        private List<Var> Vars = [];
        private List<Method> Methods = [];
        public LineWithTokens[] Tokens = Array.Empty<LineWithTokens>();
        public EZCode() { }
        public EZCode(string code)
        {
            Code = code;
        }
        public LineWithTokens[] Tokenize(string code)
        {
            return Tokens = TokenArray(code);
        }
        private LineWithTokens[] TokenArray(string code)
        {
            List<LineWithTokens> withTokens = new List<LineWithTokens>();
            Line[] Lines = SplitLine(code);

            for (int i = 0; i < Lines.Length; i++)
            {
                List<Token> tokens = new List<Token>();
                Line line = Lines[i];
                object[] parts = SplitParts(ref Lines, i, out int continues);
                for (int j = 0; j < parts.Length; j++)
                {
                    Token token = SingleToken(Lines, i, parts, j, out bool stops);
                    if (token.Type != TokenType.None) tokens.Add(token);
                    if (stops) continue;
                }
                i += continues;

                withTokens.Add(new(tokens.ToArray(), line));
            }
            return withTokens.ToArray();
        }
        private Token SingleToken(Line[] Lines, int lineIndex, object[] parts, int partIndex) =>
            SingleToken(Lines, lineIndex, parts, partIndex, out bool stops);
        private Token SingleToken(Line[] Lines, int lineIndex, object[] parts, int partIndex, out bool stops)
        {
            stops = false;
            TokenType tokenType = TokenType.None;
            if (parts[partIndex] is string)
            {
                string part = parts[partIndex] as string;
                switch (part.ToLower())
                {
                    default: tokenType = TokenType.Identifier; break;
                    case "//": tokenType = TokenType.Comment; stops = true; break;
                    case "=": tokenType = TokenType.Equals; break;
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
                    case ",": tokenType = TokenType.Comma; break;
                    case "var": tokenType = TokenType.Var; break;
                    case "static": tokenType = TokenType.Static; break;
                    case "explicit": tokenType = TokenType.Explicit; break;
                    case "watch": tokenType = TokenType.Watch; break;
                    case "params": tokenType = TokenType.Params; break;
                    case "insideof": tokenType = TokenType.InsideOf; break;
                    case "typeof": tokenType = TokenType.TypeOf; break;
                    case "semi": tokenType = TokenType.Semi; break;
                    case "ontop": tokenType = TokenType.Ontop; break;
                    case "nocol": tokenType = TokenType.NoCol; break;
                    case "method": tokenType = TokenType.Method; break;
                    case "container": tokenType = TokenType.Container; break;
                    case "return": tokenType = TokenType.Return; break;
                    case "get": tokenType = TokenType.Get; break;
                    case "\\!": tokenType = TokenType.Nothing; break;
                }
                if (part.StartsWith("//")) tokenType = TokenType.Comment;
                if (part.StartsWith('@')) tokenType = TokenType.DataType;
                if (part.StartsWith("EZCodeLanguage.EZHelp."))
                    tokenType = TokenType.EZCODEMETHOD;
            }
            else if (parts[partIndex] is Statement)
            {
                Statement part = (Statement)parts[partIndex];
                switch (part.Type)
                {
                    case "if": tokenType = TokenType.If; break;
                    case "else": tokenType = TokenType.Else; break;
                    case "loop": tokenType = TokenType.Loop; break;
                    case "try": tokenType = TokenType.Try; break;
                    case "fail": tokenType = TokenType.Fail; break;
                }
            }
            else if (parts[partIndex] is Class)
            {
                tokenType = TokenType.Class;
            }

            return new Token(tokenType, parts[partIndex]);
        }
        private object[] SplitParts(ref Line[] lines, int lineIndex, out int continues)
        {
            char[] chars = [ ' ', '(', ')', '[', ']', '{', '}', '!', '=', '@', ':', '>', '<', ',' ];
            string line = lines[lineIndex].Value;
            object[] parts = SplitWithDelimiters(line, chars).Where(x => x != "" && x != " " ).ToArray();
            string[] partsSpaces = line.Split(" ").Where(x => x != "" && x != " ").ToArray();
            continues = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                try
                {
                    if (parts[i].ToString() == "\\" && parts[i + 1].ToString() == "!")
                    {
                        parts[i] = "\\!";
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }
                    if (parts[i].ToString() == "-" && parts[i + 1].ToString() == ">")
                    {
                        parts[i] = "->";
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                        if (i == parts.Length - 1)
                        {
                            parts = parts.Append(lines[i + 1].Value).ToArray();
                            continues++;
                        }
                    }
                    else if (parts[i].ToString() == "=" && parts[i + 1].ToString() == ">")
                    {
                        parts[i] = "=>";
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }
                    else if (parts[i].ToString() == "@")
                    {
                        parts[i] = "@" + parts[i + 1];
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }
                    else if (parts[i].ToString() == "//")
                    {
                        parts[i] = string.Join(" ", parts.Skip(i));
                        parts = parts.ToList().Where((item, index) => index <= i).ToArray();
                        break;
                    }
                    else if (parts[i].ToString()!.StartsWith("EZCodeLanguage.EZHelp.") && parts[i + 1].ToString() == "(")
                    {
                        int end = Array.IndexOf(parts, ")", i);
                        int dif = end - i;
                        object[] np = parts.Skip(i).Take(dif + 1).ToArray();
                        parts[i] = string.Join("", np);

                        List<object> l = new List<object>(parts);
                        for (int j = i + 1; j < end + 1; j++)
                        {
                            l.RemoveAt(i + 1);
                        }
                        parts = l.ToArray();
                    }
                    else if (Statement.Types.Contains(parts[i]))
                    {
                        LineWithTokens[] lineWithTokens = [];
                        Argument? argument = null;
                        bool sameLine = false, brackets = false;
                        if (Statement.ConditionalTypes.Contains(parts[i]))
                        {
                            string val = string.Join(" ", partsSpaces.Skip(1).TakeWhile(x => x != ":" && x != "{"));
                            Token[] argTokens = [];
                            for (int j = 1; j < parts.Length; j++)
                            {
                                if (parts[j].ToString() == ":")
                                {
                                    sameLine = true;
                                    break;
                                }
                                if (parts[j].ToString() == "{")
                                {
                                    brackets = true;
                                    break;
                                }

                                argTokens = argTokens.Append(SingleToken(lines, lineIndex, parts, j)).ToArray();
                            }
                            argument = new(argTokens, lines[lineIndex], val);
                        }
                        if (sameLine)
                        {
                            string val = string.Join(" ", partsSpaces.SkipWhile(x=>x != ":").Skip(1));
                            LineWithTokens inLineTokens = TokenArray(val)[0];
                            string code = inLineTokens.Line.Value;
                            Line endline = new(code, lines[lineIndex].CodeLine);
                            lineWithTokens = [new LineWithTokens(inLineTokens.Tokens, endline)];
                        }
                        else
                        {
                            Line nextLine = lines[lineIndex + 1];
                            bool sameLineBracket = nextLine.Value.StartsWith('{');
                            if (!brackets && !sameLineBracket)
                            {
                                LineWithTokens nextLineTokens = TokenArray(nextLine.Value)[0];
                                nextLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + 1;
                                string code = nextLineTokens.Line.Value;
                                lineWithTokens = [new(nextLineTokens.Tokens, nextLine)];
                                List<Line> l = [.. lines];
                                l.Remove(nextLine);
                                lines = [.. l];
                            }
                            else
                            {
                                List<Line> l = [.. lines];
                                int curleyBrackets = sameLineBracket ? 0 : 1;
                                for (int j = lineIndex + 1; j < lines.Length; j++)
                                {
                                    Line bracketLine = lines[j];
                                    LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value)[0];
                                    bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + j;
                                    if (bracketLine.Value.Contains('{'))
                                        curleyBrackets++;
                                    if (bracketLine.Value.Contains('}'))
                                        curleyBrackets--;
                                    string code = bracketLineTokens.Line.Value;
                                    lineWithTokens = [.. lineWithTokens, new(bracketLineTokens.Tokens, bracketLine)];
                                    l.Remove(bracketLine);
                                    if (curleyBrackets == 0)
                                        break;
                                }
                                lines = [.. l];
                            }
                        }
                        Statement statement = new(parts[i].ToString(), lines[lineIndex], lineWithTokens, argument);
                        parts = [statement];
                    }
                    else if (parts[i].ToString() == "class")
                    {
                        string name = parts[i + 1].ToString();

                        Class.ClassSettings settings =
                            (line.Contains("static ") ? Class.ClassSettings.Static : Class.ClassSettings.None) |
                            (line.Contains("ontop ") ? Class.ClassSettings.Ontop : Class.ClassSettings.None) |
                            (line.Contains("semi ") ? Class.ClassSettings.Semi : Class.ClassSettings.None);

                        Var[] properties = [];
                        Method[] methods = [];
                        GetValueMethod[] getValueMethods = [];
                        ExplicitWatch[]? explicitWatch = [];
                        ExplicitParams? explicitParams = null;
                        DataType? typeOf = null;
                        Line nextLine = lines[lineIndex + 1];
                        bool sameLineBracket = nextLine.Value.StartsWith('{');
                        List<Line> l = [.. lines];
                        int curleyBrackets = sameLineBracket ? 0 : 1;
                        string[] watchFormats = [], watchNames = [];
                        string paramFormat = "", paramName = "";
                        List<Var[]> watchVars = new List<Var[]>();
                        List<Token[]> propertyTokens = new List<Token[]>();
                        Var[]? paramVars = null;
                        for (int j = lineIndex + 1, skip = 0; j < lines.Length; j++, skip -= skip > 0 ? 1 : 0)
                        {
                            Line bracketLine = lines[j];
                            LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value)[0];
                            if (skip == 0)
                            {
                                bool ismethod = false, isproperty = false, isexplicit = false, iswatch = false, isparam = false, istypeof = false, isget = false;
                                for (int k = 0; k < bracketLineTokens.Tokens.Length; k++)
                                {
                                    bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + k;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Var) isproperty = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Method) ismethod = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Explicit) isexplicit = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Get) isget = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.Watch) iswatch = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.Params) isparam = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.TypeOf) istypeof = true;
                                }
                                if (isproperty)
                                {
                                    propertyTokens.Add(bracketLineTokens.Tokens);
                                }
                                else if (ismethod)
                                {
                                    Method method = SetMethod(lines, j);
                                    methods = [.. methods, method];
                                    skip += method.Lines.Length;
                                }
                                else if (iswatch)
                                {
                                    watchFormats = [.. watchFormats, string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value))];
                                    watchNames = [.. watchNames, bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString()];
                                    Token[] varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                    watchVars.Add(GetVarsFromParameter(varTokens, lines[j]));
                                }
                                else if (isparam)
                                {
                                    paramFormat = string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value));
                                    paramName = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString();
                                    Token[] varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                    paramVars = GetVarsFromParameter(varTokens, lines[j]);
                                }
                                else if (istypeof)
                                {
                                    Token token = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray()[0];
                                    if (token.Type == TokenType.EZCODEMETHOD)
                                    {
                                        string[] exp = token.Value.ToString().Split(['(', ')', '.']).Where(x => x != "").ToArray();
                                        string type = "@" + exp[exp.Length - 1].Remove(0, 1);
                                        type = type.Remove(type.Length - 1, 1);
                                        typeOf = DataType.GetType(type, Classes.ToArray());
                                    }
                                }
                                else if (isget)
                                {
                                    GetValueMethod getVal = SetGetVal(lines, j);
                                    getValueMethods = [.. getValueMethods, getVal];
                                    skip += getVal.Method.Lines.Length;
                                }
                            }
                            if (bracketLine.Value.Contains('{'))
                                curleyBrackets++;
                            if (bracketLine.Value.Contains('}'))
                                curleyBrackets--;
                            string code = bracketLineTokens.Line.Value;
                            l.Remove(bracketLine);
                            if (curleyBrackets == 0)
                                break;
                        }
                        lines = [.. l];
                        for (int j = 0; j < watchFormats.Length; j++)
                            explicitWatch = [.. explicitWatch, new ExplicitWatch(watchFormats[j], new(methods.FirstOrDefault(x => x.Name == watchNames[j], null), null), watchVars[j])];
                        if (paramName != "")
                            explicitParams = new ExplicitParams(paramFormat, new(methods.FirstOrDefault(x => x.Name == paramName, null), null), paramVars);
                        for (int j = 0; j < propertyTokens.Count; j++)
                            properties = [.. properties, SetVar(lines[j], propertyTokens[j])];

                        Class @class = new(name, lines[lineIndex], methods, settings, properties, explicitWatch, explicitParams, typeOf, getValueMethods);
                        Classes.Add(@class);
                        parts = [@class];
                    }
                    else if (Classes.Any(x => x.WatchFormat.Any(x => x.IsFound(parts as string[] ?? [], i, Classes.ToArray()))))
                    {
                        Class c = Classes.FirstOrDefault(x => x.WatchFormat.Any(x => x.IsFound(parts as string[] ?? [], i, Classes.ToArray())));
                        ExplicitWatch watch = c.WatchFormat.FirstOrDefault(x => x.IsFound(parts as string[] ?? [], i, Classes.ToArray(), out int continues));
                        parts[i] = watch.Runs;
                        i += continues;
                    }
                }
                catch
                {

                }
            }
            return parts;
        }
        private Var[] GetVarsFromParameter(Token[] tokens, Line line)
        {
            if (tokens.Length > 1 && tokens.Select(x => x.Type).Contains(TokenType.Colon))
            {
                tokens = tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).ToArray();
                string[] all = string.Join("", tokens.Select(x=>x.Value.ToString())).Split(",");
                Var[] vars = [];
                for (int i = 0; i < all.Length ; i++)
                {
                    DataType? type = null;
                    string value = all[i];
                    string[] sides = all[i].Split(":");
                    if(sides.Length > 1)
                    {
                        type = DataType.GetType(sides[0], Classes.ToArray());
                        value = sides[1];
                    }
                    vars = [.. vars, new Var(null, value, line)];
                }
                return vars;
            }
            else
            {
                return new Var[0];
            }
        }
        private Method SetMethod(Line[] lines, int index)
        {
            Line line = lines[index];

            Method.MethodSettings settings =
                (line.Value.Contains("static ") ? Method.MethodSettings.Static : Method.MethodSettings.None) |
                (line.Value.Contains("nocol ") ? Method.MethodSettings.NoCol : Method.MethodSettings.None);
            DataType? returns = null;
            Var[]? param = [];
            Token[] fistLineTokens = TokenArray(string.Join(" ", line.Value.Split(" ").SkipWhile(x => x != "method").Skip(1)))[0].Tokens;
            string name = fistLineTokens[0].Value.ToString()!;
            bool ret = false;
            for (int i = 1; i < fistLineTokens.Length; i++)
            {
                Token token = fistLineTokens[i];
                if (ret)
                {
                    if(token.Type == TokenType.DataType)
                    {
                        returns = DataType.GetType(token.Value.ToString()!, Classes.ToArray());
                        break;
                    }
                }
                if (token.Type == TokenType.Arrow)
                {
                    ret = true;
                    continue;
                }

                if (token.Type == TokenType.Comma || i == 1)
                {
                    bool pTypeDef = fistLineTokens[i + 1].Type == TokenType.DataType;
                    string pName = "";
                    DataType pType = DataType.UnSet;
                    if(pTypeDef)
                    {
                        pType = DataType.GetType(fistLineTokens[i + 1].Value.ToString()!, Classes.ToArray());
                        if (fistLineTokens[i + 2].Type == TokenType.Colon)
                        {
                            pName = fistLineTokens[i + 3].Value.ToString()!;
                        }
                    }
                    else
                    {
                        pName = fistLineTokens[i + 1].Value.ToString()!;
                    }
                    param = param.Append(new Var(pName, null, line, pType)).ToArray();
                }
            }

            LineWithTokens[] lineWithTokens = [];
            Line nextLine = lines[index + 1];
            bool sameLineBracket = nextLine.Value.StartsWith('{');
            List<Line> l = [.. lines];
            int curleyBrackets = sameLineBracket ? 0 : 1;
            for (int i = index + 1; i < lines.Length; i++)
            {
                Line bracketLine = lines[i];
                Token[] bracketLineTokens = TokenArray(bracketLine.Value)[0].Tokens;
                if (bracketLine.Value.Contains('{'))
                    curleyBrackets++;
                if (bracketLine.Value.Contains('}'))
                    curleyBrackets--;
                lineWithTokens = [.. lineWithTokens, new LineWithTokens(bracketLineTokens, bracketLine)];
                l.Remove(bracketLine);
                if (curleyBrackets == 0)
                    break;
            }
            lines = [.. l];
            return new Method(name, line, settings, lineWithTokens, param, returns);
        }
        private GetValueMethod SetGetVal(Line[] lines, int index)
        {
            Line line = new Line(string.Join(" ", lines[index].Value.Split(" ").Prepend("name").Prepend("method")), index);
            Line[] liness = lines.Select((x, y) => y == index ? line : x).ToArray();
            Method method = SetMethod(liness, index);
            method.Line = lines[index];
            method.Name = null;
            GetValueMethod getValue = new GetValueMethod(method.Returns, method);
            return getValue;
        }
        private Var? SetVar(Line line, Token[] tokens)
        {
            Var? var = null;
            if (tokens[0].Type == TokenType.Var)
            {
                if (tokens[1].Type == TokenType.Identifier)
                {
                    if (tokens[2].Type == TokenType.Arrow)
                    {
                        var = new(tokens[1].Value.ToString(), string.Join(" ", tokens.Skip(3).Select(x => x.Value)).ToArray(), line);
                    }
                    else
                    {
                        var = new(tokens[1].Value.ToString(), tokens[2], line);
                    }
                }
            }
            return var;
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