using System.Text;
using System.Text.RegularExpressions;

namespace EZCodeLanguage
{
    public class Parser
    {
        public class Token
        {
            public TokenType Type { get; set; }
            public object Value { get; set; }
            public string StringValue { get; set; } 
            public Token(TokenType type, object value, string stringValue)
            {
                Type = type;
                Value = value;
                StringValue = stringValue;
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
        public class Argument
        {
            public enum ArgAdds { And, Or, None }
            public ArgAdds ArgAdd = ArgAdds.None;
            public Token[] Tokens { get; set; }
            public Line Line { get; set; }
            public string Value { get; set; }
            public Argument(Token[] tokens, Line line, string value, ArgAdds argAdds = ArgAdds.None)
            {
                Tokens = tokens;
                Line = line;
                Value = value;
                ArgAdd = argAdds;
            }
            public Argument[]? Args()
            {
                Argument[] arguments = [];
                Token[] tokens = [];
                for (int i = 0; i < Tokens.Length; i++)
                {
                    if (Tokens[i].Type == TokenType.And)
                    {
                        arguments = [.. arguments, new Argument(tokens, Line, string.Join(" ", tokens.Select(x=>x.Value)), ArgAdds.And)];
                        tokens = [];
                        continue;
                    }
                    else if (Tokens[i].Type == TokenType.Or)
                    {
                        arguments = [.. arguments, new Argument(tokens, Line, string.Join(" ", tokens.Select(x => x.Value)), ArgAdds.Or)];
                        tokens = [];
                        continue;
                    }
                    else
                    {
                        tokens = [.. tokens, Tokens[i]];
                    }
                }
                arguments = [.. arguments, new Argument(tokens, Line, string.Join(" ", tokens.Select(x => x.Value)))];

                return arguments;
            }
            public static bool? EvaluateTerm(string input)
            {
                switch (input.ToLower())
                {
                    case "true": case "y": case "yes": case "1": return true;
                    case "false": case "n": case "no": case "0": return false;
                    default: return null;
                }
            }
        }
        public class Statement
        {
            public Argument? Argument { get; set; }
            public Line Line { get; set; }
            public LineWithTokens[] InBrackets { get; set; }
            public static string[] Types = ["if", "elif", "else", "loop", "try", "fail"];
            public static string[] ConditionalTypes = ["if", "elif", "loop"];
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
        public class DataType
        {
            public enum Types {
                _null,
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
            public Container? ObjectContainer { get; set; }
            public DataType(Types type, Class? _class, Container? container = null)
            {
                Type = type;
                ObjectClass = _class;
                ObjectContainer = container;
            }
            public DataType() { }
            public static DataType UnSet = new DataType() { Type = Types._null };
            public static DataType GetType(string param, Class[] classes, Container[] containers)
            {
                Types types = new();
                Class _class = null;
                Container container = null;
                param = param.Replace("@", "");
                switch (param)
                {
                    case "string": case "str": types = Types._string; break;
                    case "int": types = Types._int; break;
                    case "float": types = Types._float; break;
                    case "bool": types = Types._bool; break;
                    case "null": types = Types._null; break;
                    default: types = Types._object; break;
                }
                _class = classes.FirstOrDefault(x => x.Name == param, null);
                if (_class == null) container = containers.FirstOrDefault(x => x.Name == param, null);

                return new(types, _class, container);
            }
            public static DataType TypeFromValue(string value, Class[] classes, Container[] containers)
            {
                if (float.TryParse(value, out _) && !int.TryParse(value, out _)) return GetType("float", classes, containers);
                if (int.TryParse(value, out _)) return GetType("int", classes, containers);
                if (bool.TryParse(value, out _)) return GetType("bool", classes, containers);
                return GetType("string", classes, containers);
            }
        }
        public class ExplicitWatch
        {
            public string Pattern { get; set; }
            public Var[]? Vars { get; private set; }
            public RunMethod Runs { get; set; }
            public ExplicitWatch(string format, RunMethod run, Var[] vars)
            {
                format ??= "";
                Pattern = format.Replace("{", "(?<").Replace("}", ">\\w+)");
                Runs = run;
                Vars = vars;
            }
            public bool IsFound(string input, Class[] classes, Container[] containers)
            {
                Match match = Regex.Match(input, Pattern);
                if (match.Success && input == match.Groups[0].ToString())
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
                        vars = vars.Append(new Var(Vars[i].Name, capturedValues[i], Vars[i].Line, type:(type != "" ? DataType.GetType(type, classes, containers) : DataType.UnSet))).ToArray();
                    }
                    Runs.Parameters = vars;
                    return true;
                }
                return false;
            }
        }
        public class ExplicitParams
        {
            public bool All { get; set; } = false;
            public string Pattern { get; set; }
            public Var[]? Vars { get; private set; }
            public RunMethod Runs { get; set; }
            public ExplicitParams(string format, RunMethod run, Var[] vars, bool all = false)
            {
                Pattern = format;
                Runs = run;
                Vars = vars;
                All = all;
            }
            public bool IsFound(string input, Class[] classes, Container[] containers)
            {
                ExplicitWatch watch = new ExplicitWatch(Pattern, Runs, Vars);
                return All || watch.IsFound(Regex.Escape(input), classes, containers);
            }
        }
        public class RunMethod
        {
            public string? ClassName { get; set; }
            public Method Runs { get; set; }
            public Var[]? Parameters { get; set; }
            public Token[] Tokens { get; set; }
            public RunMethod(Method method, Var[] vars, string? classname, Token[] tokens)
            {
                Runs = method;
                Parameters = vars;
                ClassName = classname;
                Tokens = tokens;
            }
        }
        public class Method
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
        public class GetValueMethod
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
        public class Class
        {
            public GetValueMethod[]? GetTypes { get; set; }
            public ExplicitWatch[] WatchFormat { get; set; }
            public DataType? TypeOf { get; set; }
            public ExplicitParams? Params { get; set; }
            public string Name { get; set; }
            public Line Line { get; set; }
            public Method[]? Methods { get; set; }
            public Var[]? Properties { get; set; }
            public Class[]? Classes { get; set; }
            public DataType[] InsideOf { get; set; }
            public string[] Aliases { get; set; }
            public int Length { get; set; }
            public Class() { }
            public Class(string name, Line line, Method[]? methods = null, Var[]? properties = null, ExplicitWatch[]? watchForamt = null, ExplicitParams? explicitParams = null,
                DataType? datatype = null, GetValueMethod[]? getValue = null, Class[]? classes = null, DataType[]? insideof = null, int length = 0, string[]? aliases = null)
            {
                Name = name;
                Line = line;
                Methods = methods ?? [];
                Properties = properties ?? [];
                WatchFormat = watchForamt ?? [];
                Params = explicitParams;
                TypeOf = datatype;
                GetTypes = getValue ?? [];
                Classes = classes ?? [];
                InsideOf = insideof ?? [];
                Length = length;
                Aliases = aliases ?? [];
            }
            public Class(Class cl)
            {
                Name = cl.Name;
                Line = cl.Line;
                Methods = cl.Methods?.Select(m => new Method(m.Name, m.Line, m.Settings, m.Lines, m.Params, m.Returns)).ToArray() ?? Array.Empty<Method>();
                Properties = cl.Properties?.Select(p => new Var(p.Name, p.Value, p.Line, p.StackNumber, p.DataType, p.Required)).ToArray() ?? Array.Empty<Var>();
                WatchFormat = cl.WatchFormat?.Select(w => new ExplicitWatch(w.Pattern, w.Runs, w.Vars)).ToArray() ?? Array.Empty<ExplicitWatch>();
                Params = cl.Params;
                TypeOf = cl.TypeOf;
                GetTypes = cl.GetTypes?.Select(t => new GetValueMethod(t.DataType, t.Method)).ToArray() ?? Array.Empty<GetValueMethod>();
                Classes = cl.Classes?.Select(c => new Class(c)).ToArray() ?? Array.Empty<Class>();
                InsideOf = cl.InsideOf?.Select(i => new DataType(i.Type, i.ObjectClass, i.ObjectContainer)).ToArray() ?? Array.Empty<DataType>();
                Length = cl.Length;
                Aliases = cl.Aliases;
            }
        }
        public class Var
        {
            public string? Name { get; set; }
            public object? Value { get; set; }
            public DataType? DataType { get; set; }
            public Line Line { get; set; }
            public bool Required { get; set; }
            public int StackNumber = 0;
            public Var() { }
            public Var(string? name, object? value, Line line, int stackNumber = 0, DataType? type = null, bool optional = true)
            {
                type ??= DataType.UnSet;
                Name = name;
                Value = value;
                Line = line;
                DataType = type;
                Required = optional;
                StackNumber = stackNumber;
            }
        }
        public class Container
        {
            public Container() { }
            public Container(string name, Class[] classes, Line line)
            {
                Name = name;
                Classes = classes;
                Line = line;
            }
            public string Name { get; set; }
            public Class[] Classes { get; set; }
            public Line Line { get; set; }
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
            public LineWithTokens(LineWithTokens line)
            {
                Line = line.Line;
                Tokens = line.Tokens;
            }
        }
        public class CSharpMethod(string path, string[]? @params, bool isVar)
        {
            public string Path { get; set; } = path;
            public string[]? Params { get; set; } = @params;
            public bool IsVar { get; set; } = isVar;
            public override string ToString()
            {
                return $"{Path}";
            }
        }
        public class CSharpDataType(string path, string type)
        {
            public string Path { get; set; } = path;
            public string Type { get; set; } = type;
        }
        public enum TokenType
        {
            None,
            Null,
            Comment,
            Comma,
            QuestionMark,
            Colon,
            Arrow,
            DataType,
            OpenCurlyBracket,
            CloseCurlyBracket,
            New,
            If,
            Else,
            Elif,
            Loop,
            Try,
            Fail,
            Argument,
            Identifier,
            Undefined,
            Class,
            Explicit,
            Alias,
            Watch,
            Params,
            TypeOf,
            InsideOf,
            NoCol,
            Method,
            Match,
            Container,
            Break,
            Yield,
            Return,
            Get,
            And,
            Not,
            Or,
            Make,
            Is,
            RunExec,
            EZCodeDataType,
            Include,
            Exclude,
        }
        public static char[] Delimeters = [' ', '{', '}', '@', ':', ',', '?'];
        public string Code { get; set; }
        internal bool commentBlock = false;
        public List<Class> Classes = [];
        public List<Container> Containers = [];
        public List<Method> Methods = [];
        public LineWithTokens[] Tokens = Array.Empty<LineWithTokens>();
        public Parser() { }
        public Parser(string code)
        {
            Code = code;
        }
        public LineWithTokens[] Tokenize() => Tokens = Tokenize(Code);
        public LineWithTokens[] Tokenize(string code) => Tokens = TokenArray(code).Where(x => x.Line.Value.ToString() != "").ToArray();
        private LineWithTokens[] TokenArray(string code, bool insideClass = false)
        {
            List<LineWithTokens> withTokens = new List<LineWithTokens>();
            Line[] Lines = SplitLine(code);

            for (int i = 0; i < Lines.Length; i++)
            {
                List<Token> tokens;
                Line line = Lines[i];
                int continues = 0, arrow, ar = 0;
                do
                {
                    tokens = new List<Token>();
                    arrow = 0;
                    string[] stringParts = SplitParts(ref Lines, i, ar, out _, out _, insideClass, true).Select(x=>x.ToString()).ToArray();
                    object[] parts = SplitParts(ref Lines, i, ar, out continues, out arrow, insideClass);
                    for (int j = 0; j < parts.Length; j++)
                    {
                        Token token = SingleToken(parts, j, stringParts.Length > j ? stringParts[j] : "", out bool stops);
                        if (token.Type != TokenType.None && token.Type != TokenType.Comment) tokens.Add(token);
                        if (stops) continue;
                    }
                    ar = arrow;

                    withTokens.Add(new(tokens.ToArray(), line));
                }
                while (arrow != 0);
                i += continues;
                line.CodeLine += 1;
            }

            return withTokens.ToArray();
        }
        internal Token SingleToken(object[] parts, int partIndex, string stringPart, out bool stops)
        {
            stops = false;
            TokenType tokenType = TokenType.None;
            if (parts[partIndex] is string)
            {
                string part = parts[partIndex] as string;
                switch (part)
                {
                    default: tokenType = TokenType.Identifier; break;
                    case "!": case "not": tokenType = TokenType.Not; break;
                    case "&": case "&&": case "and": tokenType = TokenType.And; break;
                    case "|": case "||": case "or": tokenType = TokenType.Or; break;
                    case "//": tokenType = TokenType.Comment; stops = true; break;
                    case "=>": tokenType = TokenType.Arrow; break;
                    case ":": tokenType = TokenType.Colon; break;
                    case "{": tokenType = TokenType.OpenCurlyBracket; break;
                    case "}": tokenType = TokenType.CloseCurlyBracket; break;
                    case "?": tokenType = TokenType.QuestionMark; break;
                    case ",": tokenType = TokenType.Comma; break;
                    case "undefined": tokenType = TokenType.Undefined; break;
                    case "explicit": tokenType = TokenType.Explicit; break;
                    case "watch": tokenType = TokenType.Watch; break;
                    case "params": tokenType = TokenType.Params; break;
                    case "insideof": tokenType = TokenType.InsideOf; break;
                    case "typeof": tokenType = TokenType.TypeOf; break;
                    case "alias": tokenType = TokenType.Alias; break;
                    case "nocol": tokenType = TokenType.NoCol; break;
                    case "method": tokenType = TokenType.Method; break;
                    case "return": tokenType = TokenType.Return; break;
                    case "break": tokenType = TokenType.Break; break;
                    case "yield": tokenType = TokenType.Yield; break;
                    case "is": tokenType = TokenType.Is; break;
                    case "get": tokenType = TokenType.Get; break;
                    case "new": tokenType = TokenType.New; break;
                    case "make": tokenType = TokenType.Make; break;
                    case "null": tokenType = TokenType.Null; parts[partIndex] = ""; break;
                }
                if (part.StartsWith("//")) tokenType = TokenType.Comment;
                if (part.StartsWith('@')) tokenType = TokenType.DataType;
            }
            else if (parts[partIndex] is Statement)
            {
                Statement part = (Statement)parts[partIndex];

                switch (part.Type)
                {
                    case "if": tokenType = TokenType.If; break;
                    case "elif": tokenType = TokenType.Elif; break;
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
            else if (parts[partIndex] is RunMethod)
            {
                tokenType = TokenType.Match;
            }
            else if (parts[partIndex] is Method)
            {
                tokenType = TokenType.Method;
            }
            else if (parts[partIndex] is CSharpMethod)
            {
                tokenType = TokenType.RunExec;
            }
            else if (parts[partIndex] is CSharpDataType)
            {
                tokenType = TokenType.EZCodeDataType;
            }
            else if (parts[partIndex] is Container)
            {
                tokenType = TokenType.Container;
            }

            return new Token(tokenType, parts[partIndex], stringPart);
        }
        public object[] SplitParts(ref Line[] lines, int lineIndex, int partStart, out int continues, out int arrow, bool insideClass = false, bool tostring = false)
        {
            string line = lines[lineIndex].Value;
            object[] parts = SplitWithDelimiters(line, Delimeters).Where(x => x != "" && x != " ").Select(x => (object)x).Skip(partStart).ToArray();
            string[] partsSpaces = line.Split(" ").Where(x => x != "" && x != " ").ToArray();
            continues = 0; arrow = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                try
                {
                    if (parts[i].ToString() == "->")
                    {
                        if (i == parts.Length - 1)
                        {
                            parts = parts.Append(lines[i + 1].Value).ToArray();
                            continues++;
                        }
                        else
                        {
                            arrow = i + 1 + partStart;
                            return parts.Take(i).ToArray();
                        }
                    }
                    else if (parts[i].ToString() == "@")
                    {
                        parts[i] = "@" + parts[i + 1];
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }
                    else if (parts[i].ToString().StartsWith("//"))
                    {
                        parts[i] = string.Join(" ", parts.Skip(i));
                        parts = parts.ToList().Where((item, index) => index <= i).ToArray();
                        break;
                    }
                    else if (parts[i].ToString().EndsWith("*/") && !tostring)
                    {
                        commentBlock = false;
                        parts = parts.ToList().Where((item, index) => index != i).ToArray();
                    }
                    else if (parts[i].ToString().StartsWith("/*") || (commentBlock && !tostring))
                    {
                        commentBlock = true;
                        parts = parts.ToList().Where((item, index) => index != i).ToArray();
                        i--;
                    }
                    if (tostring) continue;
                    else if (Statement.Types.Contains(parts[i]))
                    {
                        Statement statement = SetStatement(ref lines, lineIndex, i);
                        parts = [statement];
                    }
                    else if (parts[i].ToString() == "class")
                    {
                        string name = parts[i + 1].ToString();

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
                        List<Line> propertyLine = new List<Line>();
                        Var[]? paramVars = null;
                        bool paramAll = false;
                        Token[] paramTokens = [], watchTokens = [];
                        DataType[] insideof = [];
                        Class[] classes = [];
                        int length = 0;
                        string[] alias = [];
                        for (int j = lineIndex + 1, skip = 0; j < lines.Length; j++, skip -= skip > 0 ? 1 : 0)
                        {
                            Line bracketLine = lines[j];
                            LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value, true)[0];
                            if (bracketLineTokens.Tokens.Length > 0 && bracketLineTokens.Tokens[0].Value.ToString() == "class")
                            {
                                bracketLineTokens = TokenArray(string.Join(Environment.NewLine, lines.Select(x => x.Value).Skip(j)), true)[0];
                                skip += (bracketLineTokens.Tokens[0].Value as Class).Length + 1;
                            }
                            if (skip == 0)
                            {
                                bool ismethod = false, isproperty = false, isexplicit = false, iswatch = false, isparam = false,
                                    istypeof = false, isinsideof = false, isget = false, isclass = false, isalias = false;
                                for (int k = 0; k < bracketLineTokens.Tokens.Length; k++)
                                {
                                    bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + k;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.New || bracketLineTokens.Tokens[k].Type == TokenType.Undefined) isproperty = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Method) ismethod = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Explicit) isexplicit = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Get) isget = true;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.Class) isclass = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.Watch) iswatch = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.Params) isparam = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.TypeOf) istypeof = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.InsideOf) isinsideof = true;
                                    if (isexplicit && bracketLineTokens.Tokens[k].Type == TokenType.Alias) isalias = true;
                                }
                                if (isproperty)
                                {
                                    propertyTokens.Add(bracketLineTokens.Tokens);
                                    propertyLine.Add(lines[j]);
                                }
                                else if (ismethod)
                                {
                                    Method method = SetMethod(lines, j);
                                    methods = methods.Append(method).ToArray();
                                    skip += method.Lines.Length;
                                }
                                else if (iswatch)
                                {
                                    watchFormats = [.. watchFormats, string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value))];
                                    watchNames = [.. watchNames, bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString()];
                                    Token[] varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                    watchVars.Add(GetVarsFromParameter(varTokens, lines[j]));
                                    watchTokens = bracketLineTokens.Tokens;
                                }
                                else if (isparam)
                                {
                                    paramFormat = string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value));
                                    paramName = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString();
                                    Token[] varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                    paramVars = GetVarsFromParameter(varTokens, lines[j]);
                                    paramTokens = bracketLineTokens.Tokens;
                                    paramAll = paramFormat == "";
                                }
                                else if (istypeof)
                                {
                                    Token token = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray()[0];
                                    if (token.Type == TokenType.EZCodeDataType)
                                    {
                                        string type = (token.Value as CSharpDataType).Type;
                                        typeOf = DataType.GetType(type, Classes.ToArray(), Containers.ToArray());
                                    }
                                }
                                else if (isinsideof)
                                {
                                    Token token = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray()[0];
                                    if (token.Type == TokenType.DataType)
                                    {
                                        insideof = [.. insideof, DataType.GetType(token.Value.ToString(), Classes.ToArray(), Containers.ToArray())];
                                    }
                                }
                                else if (isclass)
                                {
                                    classes = [.. classes, bracketLineTokens.Tokens[0].Value as Class];
                                }
                                else if (isget)
                                {
                                    GetValueMethod getVal = SetGetVal(lines, j);
                                    getValueMethods = [.. getValueMethods, getVal];
                                    skip += getVal.Method.Lines.Length;
                                }
                                else if (isalias)
                                {
                                    alias = [.. alias, bracketLineTokens.Tokens[2].Value.ToString()];
                                }
                            }
                            if (bracketLine.Value.Contains('{'))
                                curleyBrackets++;
                            if (bracketLine.Value.Contains('}'))
                                curleyBrackets--;
                            string code = bracketLineTokens.Line.Value;
                            l.Remove(bracketLine);
                            length = j;
                            if (curleyBrackets == 0)
                                break;
                        }
                        lines = [.. l];
                        for (int j = 0; j < watchFormats.Length; j++)
                            explicitWatch = [.. explicitWatch, new ExplicitWatch(watchFormats[j], new(methods.FirstOrDefault(x => x.Name == watchNames[j], null), watchVars[j] != null ? watchVars[j] : null, name, watchTokens), watchVars[j])];
                        if (paramName != "")
                            explicitParams = new ExplicitParams(paramFormat, new RunMethod(methods.FirstOrDefault(x => x.Name == paramName, null), paramVars != null ? paramVars : null, name, paramTokens), paramVars, paramAll);
                        for (int j = 0; j < propertyTokens.Count; j++)
                            properties = [.. properties, SetVar(propertyLine[j], propertyTokens[j])];

                        Class @class = new(name, lines[lineIndex], methods, properties, explicitWatch, explicitParams, typeOf, getValueMethods, classes, insideof, length, alias);
                        if (Classes.Any(x => x.Name == name) != false)
                        {
                            Class oc = Classes.FirstOrDefault(x => x.Name == name);
                            oc = @class;
                        }
                        else
                        {
                            Classes.Add(@class);
                        }
                        parts = [@class];
                    }
                    else if (WatchIsFound(parts, i, out ExplicitWatch? watch, out int skip_match))
                    {
                        parts[i] = new RunMethod(watch.Runs.Runs, watch.Runs.Parameters, watch.Runs.ClassName, watch.Runs.Tokens);
                        parts = parts.Take(i + 1).Concat(parts.Skip(i + skip_match + 1)).ToArray();
                    }
                    else if (!insideClass && parts[i].ToString() == "method")
                    {
                        Method method = SetMethod(ref lines, lineIndex);
                        parts = [method];
                        if (Methods.Any(x => x.Name == method.Name) != false)
                        {
                            Method om = Methods.FirstOrDefault(x => x.Name == method.Name);
                            om = method;
                        }
                        else
                        {
                            Methods.Add(method);
                        }
                    }
                    else if (parts[i].ToString() == "make")
                    {
                        string[] both = string.Join(" ", partsSpaces.Skip(1)).Split("=>").Select(x => x.Trim()).ToArray();
                        string take = both[0];
                        string replace = both.Length == (0 | 1) ? "" : both[1];
                        int next = lineIndex + 1;
                        string takeMulti = "", replaceMulti = "";
                        if (take.Trim() == "{")
                        {
                            // Handle multi-line matching
                            StringBuilder multiLineTake = new StringBuilder();
                            int braceCount = 1;

                            for (int j = next; j < lines.Length; j++)
                            {
                                braceCount -= lines[j].Value.Trim().StartsWith('}') ? 1 : 0;

                                if (braceCount == 0)
                                    break;

                                multiLineTake.AppendLine(lines[j].Value.ToString());

                                next++;
                            }
                            if (!lines[next].Value.Contains("=>"))
                            {
                                return null;
                            }
                            else
                            {
                                replace = lines[next].Value.Split("=>")[1];
                            }

                            next++;
                            take = multiLineTake.ToString();
                            takeMulti = take.Split(['\n', '\r']).Select(x => x.Trim()).ToArray()[0];
                        }
                        if (replace.Trim() == "{")
                        {
                            // Handle multi-line matching
                            StringBuilder multiLineTake = new StringBuilder();
                            int braceCount = 1;

                            for (int j = next; j < lines.Length; j++)
                            {
                                braceCount -= lines[j].Value.Trim().StartsWith('}') ? 1 : 0;

                                if (braceCount == 0)
                                    break;

                                multiLineTake.AppendLine(lines[j].Value.ToString());
                                next++;
                            }

                            next++;
                            replace = multiLineTake.ToString();
                            replaceMulti = replace.Split('\n').Select(x => x.Trim()).ToArray()[0];
                        }
                        string[] takeLines = take.Split("\n").Select(x => x.Trim()).Where(y => y != "").ToArray();
                        int line_removes = 0, lr = -1;
                        var _LINES = lines.ToList();

                        for (int j = lineIndex + 1; j < lines.Length; j++)
                        {
                            if (next > lineIndex + 1)
                            {
                                lr = lr == -1 ? j : lr;
                                line_removes++;
                                next--;
                                continue;
                            }
                            string input = lines[j].Value.ToString();
                            if (takeMulti != "")
                            {
                                string rep = replace;
                                if (MakeMatch(input, takeLines[0], ref rep, out string output, false))
                                {
                                    bool match = false;
                                    Line[] takes = [new Line(output, j)];
                                    for (int k = 1; k < takeLines.Length; k++)
                                    {
                                        match = MakeMatch(lines[j + k].Value.ToString(), takeLines[k], ref rep, out string o, false);
                                        if (!match) goto OuterLoop;
                                        takes = takes.Append(new Line(o, j + k)).ToArray();
                                    }
                                    _LINES.RemoveRange(takes[0].CodeLine, takes[takes.Length - 1].CodeLine - takes[0].CodeLine + 1);
                                    rep = replace;
                                    bool m = MakeMatchMulti(string.Join(Environment.NewLine, takes.Select(x => x.Value.ToString())), take, ref rep, out string val, true);
                                    var vals = val.Split(Environment.NewLine).Where(x => x != "").ToArray();
                                    for (int k = vals.Length - 1; k >= 0; k--)
                                    {
                                        _LINES.Insert(takes[0].CodeLine, new Line(vals[k], takes[0].CodeLine + k));
                                    }
                                }
                            }
                            else
                            {
                                string _ref = replace;
                                if (MakeMatch(input, take, ref _ref, out string output, true))
                                {
                                    if (replaceMulti != "")
                                    {
                                        bool m = MakeMatchMulti(input, take, ref _ref, out string val, true);
                                        var vals = val.Split(Environment.NewLine).Where(x => x != "").ToArray();
                                        _LINES.RemoveAt(j);
                                        for (int k = vals.Length - 1; k >= 0; k--)
                                        {
                                            _LINES.Insert(j, new Line(vals[k], j + k));
                                        }
                                    }
                                    else
                                    {
                                        lines[j].Value = output;
                                    }
                                }
                            }

                        OuterLoop:
                            continue;
                        }
                        for (int j = 0; j < line_removes; j++)
                            _LINES.RemoveAt(lr);
                        lines = _LINES.ToArray();
                        parts = ["make"];
                    }
                    else if (parts[i].ToString().StartsWith("EZCodeLanguage.EZCode.DataType("))
                    {
                        string part = parts[i].ToString();
                        int ch = Array.IndexOf(part.ToCharArray(), '(');
                        string path = part[..ch];
                        string type = part.Substring(ch + 1, part.Length - ch - 2).Replace("\"", "");

                        parts[i] = new CSharpDataType(path, type);
                    }
                    else if (parts[i].ToString() == "runexec")
                    {
                        string path = "";
                        string[]? vars = null;
                        int skip = 1;
                        path = parts[i + 1].ToString();
                        if (parts.Length - 1 > 2 && parts[i + 2].ToString() == "~>")
                        {
                            skip++;
                            string[] str = parts.Select(x => x.ToString()).Skip(3 + i).ToArray();
                            skip += str.Length;
                            string all = string.Join(" ", str);
                            str = all.Split(",").Select(x => x.Trim()).ToArray();
                            vars = [];
                            for (int j = 0; j < str.Length; j++)
                            {
                                vars = [.. vars, str[j]];
                            }
                        }
                        for (int j = 0; j <= skip; j++)
                        {
                            parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                        }
                        parts[i] = new CSharpMethod(path, vars, path.Contains('\''));
                    }
                    else if (parts[i].ToString() == "container")
                    {
                        string name = parts[i + 1].ToString();
                        Class[] classes = [];
                        Line nextLine = lines[lineIndex + 1];
                        bool sameLineBracket = nextLine.Value.StartsWith('{');
                        List<Line> l = [.. lines];
                        int curleyBrackets = sameLineBracket ? 0 : 1;
                        for (int j = lineIndex + 1; j < lines.Length; j++)
                        {
                            Line bracketLine = lines[j];
                            if (bracketLine.Value.Contains('{'))
                                curleyBrackets++;
                            if (bracketLine.Value.Contains('}'))
                                curleyBrackets--;

                            classes = classes.Append(Classes.FirstOrDefault(x => x.Name == bracketLine.Value)).ToArray();

                            l.Remove(bracketLine);
                            if (curleyBrackets == 0)
                                break;
                        }
                        lines = [.. l];
                        parts = [new Container(name, classes, lines[lineIndex])];
                    }
                }
                catch
                {

                }
            }
            return parts;
        }
        private static bool MakeMatch(string input, string pattern, ref string replace, out string output, bool format)
        {
            string newPat = pattern.Replace("\\{", "\\<[[>").Replace("\\}", "\\<]]>").Replace("{", "(?<").Replace("}", ">\\S+)").Replace("\\<[[>", "\\{").Replace("\\<]]>", "\\}");
            Match match = Regex.Match(input, newPat);
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                for (int k = 1; k < groups.Count; k++)
                {
                    string placeholder = $"{{{groups[k].Name}}}";
                    string capturedValue = groups[k].Value;
                    replace = replace.Replace(placeholder, capturedValue);
                }
                var l = replace.Split(Environment.NewLine);
                for (int i = 0; i < l.Length; i++)
                    l[i] = string.Join(" ", l[i].Split(" ").Select(x => x.Replace("\\\\{", "\\<[[>").Replace("\\\\}", "\\<]]>").Replace("\\{", "{").Replace("\\}", "}").Replace("\\<[[>", "\\{").Replace("\\<]]>", "\\}")));
                replace = string.Join(Environment.NewLine, l);
                if (format) output = input.Substring(0, match.Index) + replace + input.Substring(match.Index + match.Length);
                else output = input;
                return true;
            }
            output = "";
            return false;
        }
        private static bool MakeMatchMulti(string input, string pattern, ref string replace, out string output, bool format)
        {
            string[] inputLines = input.Split('\n').Select(x => x.Trim()).ToArray();
            string[] patternLines = pattern.Split('\n').Select(x => x.Trim()).ToArray();
            output = "";

            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                string lineOutput;
                bool lineMatch = MakeMatch(line, patternLines[i], ref replace, out lineOutput, format);

                if (lineMatch)
                {
                    output = lineOutput;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        internal bool WatchIsFound(object[] parts, int index, out ExplicitWatch? watch, out int skip)
        {
            skip = 0;
            watch = null;
            string match = "";
            for (int i = index; i < parts.Length; i++)
            {
                string add = parts[i].ToString();
                match = (match + " " + add).Trim();
                for (int j = 0; j < Classes.Count; j++)
                {
                    for (int k = 0; k < Classes[j].WatchFormat.Length; k++)
                    {
                        if (Classes[j].WatchFormat[k].IsFound(match, Classes.ToArray(), Containers.ToArray()))
                        {
                            watch = Classes[j].WatchFormat[k];
                            return true;
                        }
                    }
                }
                skip++;
            }

            return false;
        }
        internal bool ParamIsFound(object[] parts, int index, out ExplicitParams? param)
        {
            param = null;

            string match = "";
            for (int i = index; i < parts.Length; i++)
            {
                string add = parts[i].ToString();
                match = (match + " " + add).Trim();
                for (int j = 0; j < Classes.Count; j++)
                {
                    if (Classes[j].Params == null) continue;
                    if (Classes[j].Params.IsFound(match, Classes.ToArray(), Containers.ToArray()))
                    {
                        param = Classes[j].Params;
                        return true;
                    }
                }
            }
            return false;
        }
        private Var[] GetVarsFromParameter(Token[] tokens, Line line)
        {
            line.CodeLine += 1;
            if (tokens.Length > 1 && tokens.Select(x => x.Type).Contains(TokenType.Colon))
            {
                tokens = tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).ToArray();
                string[] all = string.Join("", tokens.Select(x=>x.Value.ToString())).Split(",");
                Var[] vars = [];
                for (int i = 0; i < all.Length ; i++)
                {
                    DataType? type = null;
                    string name = all[i];
                    string[] sides = all[i].Split(":");
                    if(sides.Length > 1)
                    {
                        type = DataType.GetType(sides[0], Classes.ToArray(), Containers.ToArray());
                        name = sides[1];
                    }
                    vars = [.. vars, new Var(name, null, line, type:type)];
                }
                return vars;
            }
            else
            {
                return [];
            }
        }
        private Statement SetStatement(ref Line[] lines, int lineIndex, int partIndex) => SetStatement(ref lines, lineIndex, partIndex, out List<Line> removes);
        private Statement SetStatement(ref Line[] lines, int lineIndex, int partIndex, out List<Line> removes)
        {
            removes = [];
            string line = lines[lineIndex].Value;
            object[] parts = SplitWithDelimiters(line, Delimeters).Where(x => x != "" && x != " ").Select(x => (object)x).ToArray();
            string[] partsSpaces = line.Split(" ").Where(x => x != "" && x != " ").ToArray();
            LineWithTokens[] lineWithTokens = [];
            Argument? argument = null;
            bool sameLine = false, brackets = false;
            string val = string.Join(" ", partsSpaces.Skip(1).TakeWhile(x => x != ":" && x != "{"));
            Token[] argTokens = [];
            int end = 0;
            for (int j = 1; j < parts.Length; j++)
            {
                if (parts[j].ToString() == ":")
                {
                    if (brackets)
                        brackets = false;
                    sameLine = true;
                    end = parts.Length - j;
                }
                if (parts[j].ToString() == "{")
                {
                    if (sameLine)
                        sameLine = false;
                    brackets = true;
                    end = parts.Length - j;
                }
                argTokens = argTokens.Append(SingleToken(parts, j, parts[partIndex].ToString(), out _)).ToArray();
            }
            argTokens = argTokens.Take(argTokens.Length - end).ToArray();
            if (argTokens.FirstOrDefault(new Token(TokenType.None, "", "")).Value.ToString() == "runexec")
            {
                Line[] l = [new Line(lines[lineIndex].Value, lines[lineIndex].CodeLine)];
                l[0].Value = string.Join(" ", argTokens.Select(x => x.Value.ToString()));
                object[] objects = SplitParts(ref l, 0, 0, out _, out _);
                Token[] t = [];
                for (int i = 0; i < objects.Length; i++)
                    t = [.. t, SingleToken(objects, i, parts[partIndex].ToString(), out _)];
                argTokens = t;
            }
            if (Statement.ConditionalTypes.Contains(parts[partIndex]))
            {
                argument = new Argument(argTokens, lines[lineIndex], val);
            }
            if (sameLine)
            {
                string v = string.Join(" ", partsSpaces.SkipWhile(x => x != ":").Skip(1));
                LineWithTokens inLineTokens = TokenArray(v)[0];
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
                    removes.Add(nextLine);
                    lines = [.. l];
                }
                else
                {
                    List<Line> l = [.. lines];
                    int curleyBrackets = sameLineBracket ? 0 : 1;
                    string code = "";
                    for (int i = lineIndex + 1; i < lines.Length; i++)
                    {
                        Line bracketLine = lines[i];
                        LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value)[0];
                        bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + i;
                        if (bracketLine.Value.Contains('{'))
                            curleyBrackets++;
                        if (bracketLine.Value.Contains('}'))
                            curleyBrackets--;
                        removes.Add(bracketLine);
                        l.Remove(bracketLine);
                        if (curleyBrackets == 0)
                            break;
                        code += bracketLine.Value + Environment.NewLine;
                    }
                    lineWithTokens = Tokenize(code);
                    lineWithTokens = lineWithTokens.Select(x => { x.Line.CodeLine++; return x; }).ToArray();
                    if (l.Last().Value.ToString() == "}")
                    {
                        removes.Add(l[l.Count - 1]);
                        l.RemoveAt(l.Count - 1);
                        lineWithTokens = lineWithTokens.Where((x, y) => y != l.Count - 1).ToArray();
                    }
                    if (lineWithTokens[0].Line.Value == "{") lineWithTokens = lineWithTokens.Where((x, y) => y != 0).ToArray();
                    lines = [.. l];
                }
            }
            for (int i = 0; i < lineWithTokens.Length; i++)
                lineWithTokens[i].Line.CodeLine += 1;
            return new Statement(parts[partIndex].ToString(), lines[lineIndex], lineWithTokens, argument);
        }
        private Method SetMethod(Line[] lines, int index) => SetMethod(ref lines, index);
        private Method SetMethod(ref Line[] lines, int index)
        {
            Line line = lines[index];
            line.CodeLine += 1;

            Method.MethodSettings settings =
                (line.Value.Contains("static ") ? Method.MethodSettings.Static : Method.MethodSettings.None) |
                (line.Value.Contains("nocol ") ? Method.MethodSettings.NoCol : Method.MethodSettings.None);
            DataType? returns = null;
            Var[]? param = [];
            Token[] fistLineTokens = TokenArray(string.Join(" ", line.Value.Split(" ").SkipWhile(x => x != "method").Skip(1)))[0].Tokens;
            string name = fistLineTokens[0].Value.ToString()!;
            bool ret = false, req = true;
            for (int i = 1; i < fistLineTokens.Length; i++)
            {
                Token token = fistLineTokens[i];
                if (ret)
                {
                    if(token.Type == TokenType.DataType)
                    {
                        returns = DataType.GetType(token.Value.ToString()!, Classes.ToArray(), Containers.ToArray());
                        break;
                    }
                }
                if (token.Type == TokenType.Arrow)
                {
                    ret = true;
                    continue;
                }

                if (token.Type == TokenType.QuestionMark)
                {
                    req = false;
                    continue;
                }

                if ((token.Type == TokenType.Comma || i == 1) && token.Type != TokenType.Arrow && token.Type != TokenType.OpenCurlyBracket)
                {
                    bool pTypeDef = fistLineTokens[i + 1].Type == TokenType.DataType;
                    string pName = "";
                    DataType pType = DataType.UnSet;
                    if(pTypeDef)
                    {
                        pType = DataType.GetType(fistLineTokens[i + 1].Value.ToString()!, Classes.ToArray(), Containers.ToArray());
                        if (fistLineTokens[i + 2].Type == TokenType.Colon)
                        {
                            pName = fistLineTokens[i + 3].Value.ToString()!;
                        }
                    }
                    else
                    {
                        pName = fistLineTokens[i + 1].Value.ToString()!;
                    }
                    param = param.Append(new Var(pName, null, line, type:pType, optional:req)).ToArray();
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
                try
                {
                    if (Statement.Types.Contains(bracketLineTokens[0].Value))
                    {
                        lines = l.ToArray();
                        Statement statement = SetStatement(ref lines, Array.IndexOf(lines, bracketLine), 0, out List<Line> removes);
                        for (int j = 0; j < removes.Count; j++) l.Remove(removes[j]);
                        bracketLineTokens = [SingleToken([statement], 0, string.Join(" ", statement.Argument.Tokens.Select(x=>x.StringValue)), out _)];
                    }
                } catch { }
                if (bracketLine.Value.Contains('{'))
                    curleyBrackets++;
                if (bracketLine.Value.Contains('}'))
                    curleyBrackets--;
                lineWithTokens = [.. lineWithTokens, new LineWithTokens(bracketLineTokens, bracketLine)];
                l.Remove(bracketLine);
                if (curleyBrackets == 0)
                    break;
            }
            if (lineWithTokens.Last().Line.Value.ToString() == "}")
            {
                lineWithTokens.ToList().RemoveAt(lineWithTokens.Length - 1);
                lineWithTokens = lineWithTokens.Where((x, y) => y != lineWithTokens.Length - 1).ToArray();
            }
            if (lineWithTokens[0].Line.Value == "{") lineWithTokens = lineWithTokens.Where((x, y) => y != 0).ToArray();
            lines = [.. l];

            for (int i = 0; i < lineWithTokens.Length; i++)
                lineWithTokens[i].Line.CodeLine += 1;
            return new Method(name, line, settings, lineWithTokens, param, returns);
        }
        private GetValueMethod SetGetVal(Line[] lines, int index)
        {
            Line line = new Line(string.Join(" ", lines[index].Value.Split(" ").Prepend("method")), index);
            line.CodeLine += 1;
            Line[] liness = lines.Select((x, y) => y == index ? line : x).ToArray();
            Method method = SetMethod(liness, index);
            method.Line = lines[index];
            method.Name = null;
            GetValueMethod getValue = new GetValueMethod(method.Returns, method);
            return getValue;
        }
        private Var? SetVar(Line line, Token[] tokens)
        {
            line.CodeLine += 1;
            Var? var = null;
            if (tokens[0].Type == TokenType.Identifier)
            {
                if (tokens[1].Type == TokenType.Identifier)
                {
                    if (tokens[2].Type == TokenType.New)
                    {
                        if (tokens.Length > 3)
                        {
                            if (tokens[3].Type == TokenType.Colon)
                            {
                                var = new(tokens[1].Value.ToString(), string.Join(" ", tokens.Skip(4).Select(x => x.Value)), line);
                            }
                        }
                        else
                        {
                            var = new(tokens[1].Value.ToString(), tokens[2], line);
                        }
                    }
                }
            }
            if (tokens[0].Type == TokenType.Undefined)
            {
                if (tokens[1].Type == TokenType.Identifier)
                {
                    var = new(tokens[1].Value.ToString(), null, line);
                }
            }
            return var;
        }
        internal static string[] SplitWithDelimiters(string input, char[] delimiters)
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