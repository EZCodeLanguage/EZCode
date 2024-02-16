using System.Text.RegularExpressions;

namespace EZCodeLanguage.Tokenizer
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
            public static string[] Types = ["if", "loop", "else", "try", "fail", "elif"];
            public static string[] ConditionalTypes = ["if", "loop", "elif"];
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
                        vars = vars.Append(new Var(Vars[i].Name, capturedValues[i], Vars[i].Line, type != "" ? DataType.GetType(type, classes) : DataType.UnSet)).ToArray();
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
        class RunMethod
        {
            public Method Runs { get; set; }
            public Var[]? Parameters { get; set; }
            public RunMethod(Method method, Var[] vars)
            {
                Runs = method;
                Parameters = vars;
            }
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
        class CSharpMethod(string path, Var[]? @params, bool isVar)
        {
            public string Path { get; set; } = path;
            public Var[]? Params { get; set; } = @params;
            public bool IsVar { get; set; } = isVar;
        }
        class CSharpDataType(string path, string type)
        {
            public string Path { get; set; } = path;
            public string Type { get; set; } = type;
        }
        public enum TokenType
        {
            None,
            Comment,
            Comma,
            EqualTo,
            Not,
            GreaterThan,
            GreaterThanOrEqualTo,
            LessThan,
            LessThanOrEqualTo,
            Nothing,
            Colon,
            Arrow,
            DataType,
            OpenCurlyBracket,
            CloseCurlyBracket,
            New,
            If,
            Else,
            Loop,
            Try,
            Fail,
            Argument,
            Identifier,
            Undefined,
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
            Match,
            Container,
            Return,
            Get,
            And,
            Make,
            Is,
            RunExec,
            EZCodeDataType,
            Include,
            Exclude,
        }
        public char[] Delimeters = [];
        public string Code { get; set; }
        private List<Class> Classes = [];
        private List<Method> Methods = [];
        public LineWithTokens[] Tokens = Array.Empty<LineWithTokens>();
        public EZCode() { }
        public EZCode(string code)
        {
            Code = code;
        }
        public LineWithTokens[] Tokenize(string code)
        {
            return Tokens = TokenArray(code).Where(x => x.Line.Value.ToString() != "").ToArray();
        }
        private LineWithTokens[] TokenArray(string code, bool insideClass = false)
        {
            List<LineWithTokens> withTokens = new List<LineWithTokens>();
            Line[] Lines = SplitLine(code);

            for (int i = 0; i < Lines.Length; i++)
            {
                List<Token> tokens = new List<Token>();
                Line line = Lines[i];
                object[] parts = SplitParts(ref Lines, i, out int continues, insideClass);
                for (int j = 0; j < parts.Length; j++)
                {
                    Token token = SingleToken(parts, j, out bool stops);
                    if (token.Type != TokenType.None) tokens.Add(token);
                    if (stops) continue;
                }
                i += continues;

                withTokens.Add(new(tokens.ToArray(), line));
            }
            return withTokens.ToArray();
        }
        private Token SingleToken(object[] parts, int partIndex) =>
            SingleToken(parts, partIndex, out bool stops);
        private Token SingleToken(object[] parts, int partIndex, out bool stops)
        {
            stops = false;
            TokenType tokenType = TokenType.None;
            if (parts[partIndex] is string)
            {
                string part = parts[partIndex] as string;
                switch (part.ToLower())
                {
                    default: tokenType = TokenType.Identifier; break;
                    case "!": case "not": tokenType = TokenType.Not; break;
                    case "&": case "and": tokenType = TokenType.And; break;
                    case "//": tokenType = TokenType.Comment; stops = true; break;
                    case "=>": tokenType = TokenType.Arrow; break;
                    case ":": tokenType = TokenType.Colon; break;
                    case "{": tokenType = TokenType.OpenCurlyBracket; break;
                    case "}": tokenType = TokenType.CloseCurlyBracket; break;
                    case ",": tokenType = TokenType.Comma; break;
                    case "undefined": tokenType = TokenType.Undefined; break;
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
                    case "is": tokenType = TokenType.Is; break;
                    case "get": tokenType = TokenType.Get; break;
                    case "new": tokenType = TokenType.New; break;
                    case "make": tokenType = TokenType.Make; break;
                    case "\\!": tokenType = TokenType.Nothing; break;
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

            return new Token(tokenType, parts[partIndex]);
        }
        private object[] SplitParts(ref Line[] lines, int lineIndex, out int continues, bool insideClass = false)
        {
            Delimeters = [' ', '{', '}', '@', ':', ','];
            string line = lines[lineIndex].Value;
            object[] parts = SplitWithDelimiters(line, Delimeters).Where(x => x != "" && x != " ").Select(x => (object)x).ToArray();
            string[] partsSpaces = line.Split(" ").Where(x => x != "" && x != " ").ToArray();
            continues = 0;
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
                    else if (Statement.Types.Contains(parts[i]))
                    {
                        Statement statement = SetStatement(ref lines, lineIndex, i);
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
                        List<Line> propertyLine = new List<Line>();
                        Var[]? paramVars = null;
                        for (int j = lineIndex + 1, skip = 0; j < lines.Length; j++, skip -= skip > 0 ? 1 : 0)
                        {
                            Line bracketLine = lines[j];
                            LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value, true)[0];
                            if (skip == 0)
                            {
                                bool ismethod = false, isproperty = false, isexplicit = false, iswatch = false, isparam = false, istypeof = false, isget = false;
                                for (int k = 0; k < bracketLineTokens.Tokens.Length; k++)
                                {
                                    bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + k;
                                    if (bracketLineTokens.Tokens[k].Type == TokenType.New || bracketLineTokens.Tokens[k].Type == TokenType.Undefined) isproperty = true;
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
                                    if (token.Type == TokenType.EZCodeDataType)
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
                            properties = [.. properties, SetVar(propertyLine[j], propertyTokens[j])];

                        Class @class = new(name, lines[lineIndex], methods, settings, properties, explicitWatch, explicitParams, typeOf, getValueMethods);
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
                    else if (WatchIsFound(parts, i, out ExplicitWatch? watch))
                    {
                        parts[i] = watch.Runs;
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
                        string replace = both[1];

                        for (int j = i + 1; j < lines.Length; j++)
                        {
                            Match match = Regex.Match(lines[j].Value.ToString(), take.Replace("{", "(?<").Replace("}", ">\\w+)"));
                            if (match.Success)
                            {
                                GroupCollection groups = match.Groups;
                                string transformedValue = replace;

                                for (int k = 1; k < groups.Count; k++)
                                {
                                    string placeholder = $"{{{groups[k].Name}}}";
                                    string capturedValue = groups[k].Value;
                                    transformedValue = transformedValue.Replace(placeholder, capturedValue);
                                }

                                lines[j].Value = lines[j].Value.ToString().Substring(0, match.Index) + transformedValue +
                                     lines[j].Value.ToString().Substring(match.Index + match.Length);
                            }
                        }
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
                        Var[]? vars = null;
                        int skip = 1;
                        if (parts[i + 1].ToString() == "=>")
                        {
                            skip++;
                            path = parts[i + 2].ToString();
                            if (parts.Length - 1 > 3 && parts[i + 3].ToString() == "~>")
                            {
                                skip++;
                                string[] vals = parts.Select(x => x.ToString()).Skip(4 + i).ToArray();
                                skip += vals.Length;
                                vals = vals.Where(x => x != ",").ToArray();
                                vars = [];
                                for (int j = 0; j < vals.Length; j++)
                                {
                                    vars = [.. vars, new Var(null, vals[j], lines[lineIndex])];
                                }
                            }
                        }
                        for (int j = 0; j <= skip; j++)
                        {
                            parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                        }
                        parts[i] =  new CSharpMethod(path, vars, path.Contains('\''));
                    }
                }
                catch
                {

                }
            }
            return parts;
        }
        private bool WatchIsFound(object[] parts, int index, out ExplicitWatch? watch)
        {
            watch = null;

            for (int i = 0; i < Classes.Count; i++)
            {
                for (int j = 0; j < Classes[i].WatchFormat.Length; j++)
                {
                    if (Classes[i].WatchFormat[j].IsFound(parts[index].ToString(), Classes.ToArray()))
                    {
                        watch = Classes[i].WatchFormat[j];
                        return true;
                    }
                } 
            }

            return false;
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
                    string name = all[i];
                    string[] sides = all[i].Split(":");
                    if(sides.Length > 1)
                    {
                        type = DataType.GetType(sides[0], Classes.ToArray());
                        name = sides[1];
                    }
                    vars = [.. vars, new Var(name, null, line, type)];
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
            if (Statement.ConditionalTypes.Contains(parts[partIndex]))
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

                    argTokens = argTokens.Append(SingleToken(parts, j)).ToArray();
                }
                argument = new(argTokens, lines[lineIndex], val);
            }
            if (sameLine)
            {
                string val = string.Join(" ", partsSpaces.SkipWhile(x => x != ":").Skip(1));
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
                    removes.Add(nextLine);
                    lines = [.. l];
                }
                else
                {
                    List<Line> l = [.. lines];
                    int curleyBrackets = sameLineBracket ? 0 : 1;
                    for (int i = lineIndex + 1; i < lines.Length; i++)
                    {
                        Line bracketLine = lines[i];
                        LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value)[0];
                        bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + i;
                        if (bracketLine.Value.Contains('{'))
                            curleyBrackets++;
                        if (bracketLine.Value.Contains('}'))
                            curleyBrackets--;
                        string code = bracketLineTokens.Line.Value;
                        lineWithTokens = [.. lineWithTokens, new(bracketLineTokens.Tokens, bracketLine)];
                        removes.Add(bracketLine);
                        l.Remove(bracketLine);
                        if (curleyBrackets == 0)
                            break;
                    }
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
            return new Statement(parts[partIndex].ToString(), lines[lineIndex], lineWithTokens, argument);
        }
        private Method SetMethod(Line[] lines, int index) => SetMethod(ref lines, index);
        private Method SetMethod(ref Line[] lines, int index)
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

                if ((token.Type == TokenType.Comma || i == 1) && token.Type != TokenType.Arrow && token.Type != TokenType.OpenCurlyBracket)
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
                try
                {
                    if (Statement.Types.Contains(bracketLineTokens[0].Value))
                    {
                        lines = l.ToArray();
                        Statement statement = SetStatement(ref lines, Array.IndexOf(lines, bracketLine), 0, out List<Line> removes);
                        for (int j = 0; j < removes.Count; j++) l.Remove(removes[j]);
                        bracketLineTokens = [SingleToken([statement], 0)];
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
            if (l.Last().Value.ToString() == "}")
            {
                l.RemoveAt(l.Count - 1);
                lineWithTokens = lineWithTokens.Where((x, y) => y != l.Count - 1).ToArray();
            }
            if (lineWithTokens[0].Line.Value == "{") lineWithTokens = lineWithTokens.Where((x, y) => y != 0).ToArray();
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