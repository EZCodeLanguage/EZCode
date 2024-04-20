﻿using System.Text;
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
                        arguments = [.. arguments, new Argument(tokens, Line, string.Join(" ", tokens.Select(x => x.Value)), ArgAdds.And)];
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
                bool not = false;
                if (input.StartsWith("!"))
                {
                    input = input.Remove(0,1);
                    not = true;
                }
                switch (input.ToLower().Trim())
                {
                    case "true": case "y": case "yes": case "1": case "!": return true && !not;
                    case "false": case "n": case "no": case "0": return false || not;
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
                    string[] capturedValues = new string[groupCount - 1];
                    string[] types = new string[groupCount - 1];
                    for (int i = 1; i < groupCount; i++)
                    {
                        var val = groups[i].Name.Replace("AAA_AA_____hdfosdofdsof32472348923748923749823__________AIKSUDFGHAIKUDSFHAIKUDFGHIKSHDHUASDFH_AA_AAA__", "@").Replace("BBB_AA_____hdfosdofdsof32472348923748923749823__________AIKSUDFGHAIKUDSFHAIKUDFGHIKSHDHUASDFH_AA_B__", ":");
                        types[i - 1] = val.Split(":").Length == 2 ? val.Split(":")[0] : null;
                        capturedValues[i - 1] = groups[i].Value;
                    }
                    Var[] vars = [];
                    for (int i = 0; i < capturedValues.Length; i++)
                    {
                        string? type = types[i];
                        try
                        {
                            vars = vars.Append(new Var(Vars[i].Name, capturedValues[i], Vars[i].Line, type: (type != null ? DataType.GetType(type, classes, containers) : DataType.UnSet))).ToArray();
                        }
                        catch { }
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
                NoCol = 1,
                Global = 2
            }
            public MethodSettings Settings { get; set; }
            public LineWithTokens[] Lines { get; set; }
            public DataType? Returns { get; set; }
            public Var[]? Parameters { get; set; }
            public Method(string name, Line line, MethodSettings methodSettings, LineWithTokens[] lines, Var[]? param = null, DataType? returns = null)
            {
                Name = name;
                Line = line;
                Settings = methodSettings;
                Lines = lines;
                Parameters = param;
                Returns = returns;
            }
            public Method() { }
        }
        public class GetValueMethod
        {
            public DataType DataType { get; set; }
            public Method Method { get; set; }
            public string Returns { get; set; }
            public GetValueMethod() { }
            public GetValueMethod(DataType dataType, Method method, string returns)
            {
                Returns = returns;
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
                Methods = cl.Methods?.Select(m => new Method(m.Name, m.Line, m.Settings, m.Lines, m.Parameters, m.Returns)).ToArray() ?? Array.Empty<Method>();
                Properties = cl.Properties?.Select(p => new Var(p.Name, p.Value, p.Line, p.StackNumber, p.DataType, p.Required)).ToArray() ?? Array.Empty<Var>();
                WatchFormat = cl.WatchFormat?.Select(w => new ExplicitWatch(w.Pattern, w.Runs, w.Vars)).ToArray() ?? Array.Empty<ExplicitWatch>();
                Params = cl.Params;
                TypeOf = cl.TypeOf;
                GetTypes = cl.GetTypes?.Select(t => new GetValueMethod(t.DataType, t.Method, t.Returns)).ToArray() ?? Array.Empty<GetValueMethod>();
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
            public bool IsParams { get; set; }
            public bool Global { get; set; }
            public int StackNumber = 0;
            public Var() { }
            public Var(string? name, object? value, Line line, int stackNumber = 0, DataType? type = null, bool required = true, bool global = false, bool @params = false)
            {
                type ??= DataType.UnSet;
                Name = name;
                Value = value;
                Line = line;
                DataType = type;
                Required = required;
                StackNumber = stackNumber;
                Global = global;
                IsParams = @params;
            }
        }
        public class Container
        {
            public Container() { }
            public Container(string name, string[] classes, Line line)
            {
                Name = name;
                ClassNames = classes;
                Line = line;
            }
            public string Name { get; set; }
            public string[] ClassNames { get; set; }
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
            public override string ToString() => $"{Path}";
        }
        public class CSharpDataType(string path, string type)
        {
            public string Path { get; set; } = path;
            public string Type { get; set; } = type;
            public override string ToString() => $"{Path}(\"{Type}\")";
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
            RunExec,
            EZCodeDataType,
            Include,
            Global,
            True, 
            False
        }
        public static char[] Delimeters = [' ', '{', '}', '@', ':', ',', '?', '!'];
        public string Code { get; set; }
        internal bool commentBlock = false;
        public List<Class> Classes = [];
        public List<Container> Containers = [];
        public List<Method> Methods = [];
        public LineWithTokens[] LinesWithTokens = Array.Empty<LineWithTokens>();
        public Parser() { }
        public Parser(string code)
        {
            Code = code;
        }
        public LineWithTokens[] Parse() => LinesWithTokens = Parse(Code);
        public LineWithTokens[] Parse(string code) => LinesWithTokens = TokenArray(code).Where(x => x.Line.Value.ToString() != "").ToArray();
        private LineWithTokens[] TokenArray(string code, bool insideClass = false)
        {
            // Set the Code property to the parameter if it isn't null
            Code ??= code;
            // The LineWithTokens list that gets returned
            List<LineWithTokens> lineWithTokens = new List<LineWithTokens>();
            // Splits the code into lines
            Line[] Lines = SplitLine(code);

            // loops through each line
            for (int i = 0; i < Lines.Length; i++)
            {
                // Some empty variables before looping through tokens
                List<Token> tokens; // tokens in the line
                Line line = Lines[i]; // current line
                string[] stringParts = []; // Needed for the 'SplitParts' method. each token as a string value
                int continues = 0, // used with the '->' token to continue the line to the next line
                                   // These are used to check if the line contains the '->' and there is still code after it. 2 code lines in 1 line
                    arrow_output_index, // The output of the index after the '->' 
                    arrow_input_index = 0; // The input index for after the '->'
                do
                {
                    tokens = new List<Token>(); // sets tokens to empty
                    arrow_output_index = 0; // sets the arrow output to zero
                    // gets the parts split by spaces. Tokens as strings
                    stringParts = SplitParts(ref Lines, i, arrow_input_index, ref stringParts, out _, out _, insideClass, true).Select(x => x.ToString()).ToArray();
                    // gets the token's values
                    object[] parts = SplitParts(ref Lines, i, arrow_input_index, ref stringParts, out continues, out arrow_output_index, insideClass);
                    // loops through each part and creates the token from it
                    for (int j = 0; j < parts.Length; j++)
                    {
                        // Creates the appropriate token for the part
                        Token token = SingleToken(parts, j, stringParts.Length > j ? stringParts[j] : "");

                        // if the token is a comment or invalid, don't append the token to 'tokens'
                        if (token.Type == TokenType.None || token.Type == TokenType.Comment) continue;
                        tokens.Add(token);
                    }
                    arrow_input_index = arrow_output_index; // sets the arrow input to the arrow output

                    // adds the line with the 'tokens' to 'lineWithTokens' list
                    lineWithTokens.Add(new(tokens.ToArray(), line));
                }
                while (arrow_output_index != 0); // checks if there isn't anymore arrows spliting lines

                i += continues; // skips any lines that are apart of the line before it with the '->' token
                line.CodeLine += 1; // increments the line number by 1 so the first line is not 0
            }

            return lineWithTokens.ToArray();
        }
        internal Token SingleToken(object[] parts, int partIndex, string stringPart)
        {
            TokenType tokenType = TokenType.None; // the output token type

            // checks if the part is a string
            if (parts[partIndex] is string)
            {
                string part = parts[partIndex] as string; // the part from parts

                // sets the tokenType to the correct type
                switch (part)
                {
                    default: tokenType = TokenType.Identifier; break;
                    case "!": case "not": tokenType = TokenType.Not; break;
                    case "&": case "&&": case "and": tokenType = TokenType.And; break;
                    case "|": case "||": case "or": tokenType = TokenType.Or; break;
                    case "//": tokenType = TokenType.Comment; break;
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
                    case "global": tokenType = TokenType.Global; break;
                    case "get": tokenType = TokenType.Get; break;
                    case "new": tokenType = TokenType.New; break;
                    case "make": tokenType = TokenType.Make; break;
                    case "true": case "True": tokenType = TokenType.True; break;
                    case "false": case "False": tokenType = TokenType.False; break;
                    case "null": tokenType = TokenType.Null; parts[partIndex] = ""; break;
                }
                if (part.StartsWith("//")) tokenType = TokenType.Comment; // If the part starts with '//', it is comment
                if (part.StartsWith('@')) tokenType = TokenType.DataType; // If the part starts with '@', it is a datatype
            }
            // If the part is a statement 
            else if (parts[partIndex] is Statement)
            {
                Statement part = (Statement)parts[partIndex];

                // sets the tokenType to the type of statement 'part' is
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
            // If it is Class, set the part to the appropriate type
            else if (parts[partIndex] is Class)
            {
                tokenType = TokenType.Class;
            }
            // If it is RunMethod (A match), set the part to the appropriate type
            else if (parts[partIndex] is RunMethod)
            {
                tokenType = TokenType.Match;
            }
            // If it is Method, set the part to the appropriate type
            else if (parts[partIndex] is Method)
            {
                tokenType = TokenType.Method;
            }
            // If it is C# method, set the part to the appropriate type
            else if (parts[partIndex] is CSharpMethod)
            {
                tokenType = TokenType.RunExec;
            }
            // If it is C# datatype, set the part to the appropriate type
            else if (parts[partIndex] is CSharpDataType)
            {
                tokenType = TokenType.EZCodeDataType;
            }
            // If it is Container, set the part to the appropriate type
            else if (parts[partIndex] is Container)
            {
                tokenType = TokenType.Container;
            }

            return new Token(tokenType, parts[partIndex], stringPart);
        }
        public object[] SplitParts(ref Line[] lines, int lineIndex, int partStart, ref string[] stringParts, out int continues, out int arrow, bool insideClass = false, bool tostring = false)
        {
            // Current Line
            string line = lines[lineIndex].Value;
            // Get each token from the line. 'parts' is split by the token delimeters. 'partSpaces' is split by spaces
            object[] parts = SplitWithDelimiters(line, Delimeters).Where(x => x != "" && x != " ").Select(x => (object)x).Skip(partStart).ToArray();
            string[] partsSpaces = line.Split(" ").Where(x => x != "" && x != " ").ToArray();
            // Sets the out parameters to default
            continues = 0; arrow = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                try
                {
                    if (parts[i].ToString().EndsWith("*/") && !tostring)
                    {
                        // This needs to check before checking '\*'
                        // remove the current part and end comment block
                        commentBlock = false;
                        parts = parts.ToList().Where((item, index) => index != i).ToArray();
                    }
                    else if (parts[i].ToString().StartsWith("/*") || (commentBlock && !tostring))
                    {
                        // set comment block to true
                        commentBlock = true;
                        // remove this part from parts
                        parts = parts.ToList().Where((item, index) => index != i).ToArray();
                        /* decrement 'i' because removing index from parts.
                           This keeps loop from skipping index */
                        i--;
                    }
                    else if (parts[i].ToString() == "->")
                    {
                        // check if arrow is at the end of the line
                        if (i == parts.Length - 1)
                        {
                            // append next line to the current line
                            parts = parts.Append(lines[i + 1].Value).ToArray();
                            // increment skip line 
                            continues++;
                        }
                        else
                        {
                            // split the line into multiple linetokens
                            // calculate the new start of the part line
                            arrow = i + 1 + partStart;
                            // return parts only in the current part of the line
                            return parts.Take(i).ToArray();
                        }
                    }
                    else if (parts[i].ToString() == "@")
                    {
                        // append the next part to the current part
                        parts[i] = "@" + parts[i + 1];
                        // remove the next part
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }
                    else if (parts[i].ToString().StartsWith("//"))
                    {
                        // remove all parts after '//'
                        parts[i] = string.Join(" ", parts.Skip(i));
                        parts = parts.ToList().Where((item, index) => index <= i).ToArray();
                        break;
                    }
                    else if (tostring)
                    {
                        /* doesn't create class or method or whatever if 
                           just trying to get string version of parts */
                        continue;
                    }
                    else if (Statement.Types.Contains(parts[i]))
                    {
                        // gets statement and sets it to parts
                        Statement statement = SetStatement(ref lines, ref stringParts, lineIndex, i);
                        parts = [statement];
                    }
                    else if (parts[i].ToString() == "class")
                    {
                        string name = parts[i + 1].ToString();

                        // Class Properties
                        Var[] properties = []; // Class Properties
                        Method[] methods = []; // Class Methods
                        GetValueMethod[] getValueMethods = []; // Class's Gets
                        ExplicitWatch[]? explicitWatch = []; // Class's Explicit Watches
                        ExplicitParams? explicitParams = null; // Class's Explicit Params
                        DataType? typeOf = null; // Class's Explicit Typeof
                        Class[] classes = []; // Class's internal classes
                        DataType[] insideof = []; // Class's Explicit InsideOf for Containers
                        int length = 0; // Class's Length
                        string[] alias = []; // Class's Aliases

                        // Initialize a bunch of variables for looping through class 
                        Line nextLine = lines[lineIndex + 1]; // Next Line
                        bool sameLineBracket = nextLine.Value.StartsWith('{'); // if nextline starts with '{'
                        List<Line> l = [.. lines]; // A list version of lines that can be adjusted 
                        int curleyBrackets = sameLineBracket ? 0 : 1; // Curley Bracket count to look for end of class

                        /*
                         * These Variables are needed to for class properties by storing the data first, then setting 
                         * it after loop is finshed. This allows all values to be evaluated and stored before adding 
                         * them to class. Basically, in EZCode, removes the need to write certain parts of the class in order.
                         */

                        string[] watchFormats = [], watchNames = []; // temporary watch properties
                        List<Var[]> watchVars = new List<Var[]>(); // temo watch property
                        string paramFormat = "", paramName = ""; // temp param properties
                        Var[]? paramVars = null; // temp param property
                        bool paramAll = false; // temp param property
                        List<Token[]> propertyTokens = new List<Token[]>(); // temp Property property. Stores Class Property's Tokens
                        List<Line> propertyLine = new List<Line>(); // temp Property property. Stores Lines properties are on
                        Token[] paramTokens = [], watchTokens = []; // temp properties
                        for (int j = lineIndex + 1, skip = 0; j < lines.Length; j++, skip -= skip > 0 ? 1 : 0)
                        {
                            // gets current line in loop and the linewithtokens
                            Line bracketLine = lines[j];
                            LineWithTokens bracketLineTokens = TokenArray(bracketLine.Value, true)[0];
                            // checks if current line is the start of a subclass
                            if (bracketLineTokens.Tokens.Length > 0 && bracketLineTokens.Tokens[0].Value.ToString() == "class")
                            {
                                // gets LineWithTokens of the entire subclass
                                // the [0] index is there because we assume the only line is the class itself that has all the values in it
                                bracketLineTokens = TokenArray(string.Join(Environment.NewLine, lines.Select(x => x.Value).Skip(j)), true)[0];
                                // skips all of the next lines that are apart of the sub class
                                skip += (bracketLineTokens.Tokens[0].Value as Class).Length + 1;
                            }
                            if (skip == 0)
                            {
                                // variable to check what type of class property is being set in the current line
                                CurrentLineClassProperty lineType = CurrentLineClassProperty.none;
                                // check if current line property is also explicit
                                bool isexplicit = false;
                                for (int k = 0; k < bracketLineTokens.Tokens.Length; k++)
                                {
                                    // Incrementing each token's line by 1 to ensure there is no "line 0" and instead "line 1"
                                    bracketLineTokens.Line.CodeLine = lines[lineIndex].CodeLine + k;
                                    // Check current Line's type and set the right variable
                                    switch (bracketLineTokens.Tokens[k].Type)
                                    {
                                        case TokenType.New: case TokenType.Undefined: lineType = CurrentLineClassProperty.isproperty; continue;
                                        case TokenType.Method: lineType = CurrentLineClassProperty.ismethod; continue;
                                        case TokenType.Explicit: lineType = CurrentLineClassProperty.isexplicit; isexplicit = true; continue;
                                        case TokenType.Get: lineType = CurrentLineClassProperty.isget; continue;
                                        case TokenType.Class: lineType = CurrentLineClassProperty.isclass; continue;
                                        case TokenType.Watch: if (isexplicit) lineType = CurrentLineClassProperty.iswatch; continue;
                                        case TokenType.Params: if (isexplicit) lineType = CurrentLineClassProperty.isparam; continue;
                                        case TokenType.TypeOf: if (isexplicit) lineType = CurrentLineClassProperty.istypeof; continue;
                                        case TokenType.InsideOf: if (isexplicit) lineType = CurrentLineClassProperty.isinsideof; continue;
                                        case TokenType.Alias: if (isexplicit) lineType = CurrentLineClassProperty.isalias; continue;
                                    }
                                }
                                switch (lineType)
                                {
                                    case CurrentLineClassProperty.isproperty:
                                        // Add Property Values to temp property variables
                                        propertyTokens.Add(bracketLineTokens.Tokens);
                                        propertyLine.Add(lines[j]);
                                        break;
                                    case CurrentLineClassProperty.ismethod:
                                        // get method and append it to methods
                                        Method method = SetMethod(lines, ref stringParts, j);
                                        methods = methods.Append(method).ToArray();
                                        // skip the methods length
                                        skip += method.Lines.Length + 2;
                                        break;
                                    case CurrentLineClassProperty.iswatch:
                                        // get watch formats
                                        watchFormats = [.. watchFormats, string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value)).Replace("@", "AAA_AA_____hdfosdofdsof32472348923748923749823__________AIKSUDFGHAIKUDSFHAIKUDFGHIKSHDHUASDFH_AA_AAA__").Replace(":", "BBB_AA_____hdfosdofdsof32472348923748923749823__________AIKSUDFGHAIKUDSFHAIKUDFGHIKSHDHUASDFH_AA_B__")];
                                        // get the name of the method the watch calls
                                        watchNames = [.. watchNames, bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString()];
                                        // get the variable tokens for calling the method
                                        Token[] varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                        // append the watchformat to the temp var
                                        watchVars.Add(GetVarsFromParameter(varTokens, lines[j]));
                                        watchTokens = bracketLineTokens.Tokens;
                                        break;
                                    case CurrentLineClassProperty.isparam:
                                        // get param formats
                                        paramFormat = string.Join("", bracketLineTokens.Tokens.Skip(2).TakeWhile(x => x.Type != TokenType.Arrow).Select(x => x.Value));
                                        // get the name of the method the param calls
                                        paramName = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).TakeWhile(x => x.Type != TokenType.Colon).ToArray()[1].Value.ToString();
                                        // get the variable tokens for calling the method
                                        varTokens = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray();
                                        // set the param to the temp var
                                        paramVars = GetVarsFromParameter(varTokens, lines[j]);
                                        paramTokens = bracketLineTokens.Tokens;
                                        paramAll = paramFormat == "";
                                        break;
                                    case CurrentLineClassProperty.istypeof:
                                        // gets the tokens after the arrow (TYPE in this example) explicit typeof => TYPE
                                        Token token = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray()[0];
                                        // check if it is a correct datatype
                                        if (token.Type == TokenType.EZCodeDataType)
                                        {
                                            // set it to the type
                                            string type = (token.Value as CSharpDataType).Type;
                                            typeOf = DataType.GetType(type, Classes.ToArray(), Containers.ToArray());
                                        }
                                        else
                                        {
                                            // throw error
                                        }
                                        break;
                                    case CurrentLineClassProperty.isinsideof:
                                        // gets the tokens after the arrow 
                                        token = bracketLineTokens.Tokens.SkipWhile(x => x.Type != TokenType.Arrow).Skip(1).ToArray()[0];
                                        // check if it is a correct datatype
                                        if (token.Type == TokenType.DataType)
                                        {
                                            // set what container class is apart of
                                            insideof = [.. insideof, DataType.GetType(token.Value.ToString(), Classes.ToArray(), Containers.ToArray())];
                                        }
                                        else
                                        {
                                            // throw error
                                        }
                                        break;
                                    case CurrentLineClassProperty.isclass:
                                        // Add class to the subclasses
                                        classes = [.. classes, bracketLineTokens.Tokens[0].Value as Class];
                                        break;
                                    case CurrentLineClassProperty.isget:
                                        // get the get method
                                        GetValueMethod getVal = SetGetVal(lines, j);
                                        getValueMethods = [.. getValueMethods, getVal];
                                        // skip the method's length
                                        skip += getVal.Method.Lines.Length + 2;
                                        break;
                                    case CurrentLineClassProperty.isalias:
                                        // gets class alias
                                        alias = [.. alias, bracketLineTokens.Tokens[2].Value.ToString()];
                                        break;
                                }
                            }
                            // adjust curleyBracket
                            if (bracketLine.Value.Contains('{')) curleyBrackets++;
                            if (bracketLine.Value.Contains('}')) curleyBrackets--;
                            // remove line from Lines so it doesn't get tokenized after class has finished loop
                            l.Remove(bracketLine);
                            // set class length
                            length = j;
                            // check if class has ended
                            if (curleyBrackets == 0)
                                break;
                        }
                        // set Lines to the 'temp' line list
                        lines = [.. l];
                        // Create Watch, Params, and Properties for class based off of temp variables
                        for (int j = 0; j < watchFormats.Length; j++)
                        {
                            explicitWatch = [.. explicitWatch, new ExplicitWatch(watchFormats[j], new RunMethod(methods.FirstOrDefault(x => x.Name == watchNames[j], null), watchVars[j] ?? null, name, watchTokens), watchVars[j])];
                        }
                        if (paramName != "")
                        {
                            explicitParams = new ExplicitParams(paramFormat, new RunMethod(methods.FirstOrDefault(x => x.Name == paramName, null), paramVars ?? null, name, paramTokens), paramVars, paramAll);
                        }
                        for (int j = 0; j < propertyTokens.Count; j++)
                        {
                            properties = [.. properties, SetVar(propertyLine[j], propertyTokens[j])];
                        }

                        // Create class
                        Class _class = new(name, lines[lineIndex], methods, properties, explicitWatch, explicitParams, typeOf, getValueMethods, classes, insideof, length, alias);
                        // Check if class already exists
                        if (Classes.Any(x => x.Name == name) != false)
                        {
                            // If class already exists, append any new values to the end of it
                            Class cl = Classes.FirstOrDefault(x => x.Name == name);
                            _class = new Class()
                            {

                                Name = _class.Name,
                                Line = _class.Line,
                                Methods = [.. _class.Methods, .. cl.Methods?.Select(m => new Method(m.Name, m.Line, m.Settings, m.Lines, m.Parameters, m.Returns)).ToArray() ?? Array.Empty<Method>()],
                                Properties = [.. _class.Properties, .. cl.Properties?.Select(p => new Var(p.Name, p.Value, p.Line, p.StackNumber, p.DataType, p.Required)).ToArray() ?? Array.Empty<Var>()],
                                WatchFormat = [.. _class.WatchFormat, .. cl.WatchFormat?.Select(w => new ExplicitWatch(w.Pattern, w.Runs, w.Vars)).ToArray() ?? Array.Empty<ExplicitWatch>()],
                                Params = cl.Params ?? _class.Params,
                                TypeOf = cl.TypeOf ?? _class.TypeOf,
                                GetTypes = [.. _class.GetTypes, .. cl.GetTypes?.Select(t => new GetValueMethod(t.DataType, t.Method, t.Returns)).ToArray() ?? Array.Empty<GetValueMethod>()],
                                Classes = [.. _class.Classes, .. cl.Classes?.Select(c => new Class(c)).ToArray() ?? Array.Empty<Class>()],
                                InsideOf = [.. _class.InsideOf, .. cl.InsideOf?.Select(i => new DataType(i.Type, i.ObjectClass, i.ObjectContainer)).ToArray() ?? Array.Empty<DataType>()],
                                Length = cl.Length + _class.Length,
                                Aliases = [.. _class.Aliases, .. cl.Aliases]
                            };
                        }
                        else
                        {
                            // Else, Add it to the Class List
                            Classes.Add(_class);
                        }
                        // set part to class
                        parts = [_class];
                    }
                    else if (WatchIsFound(parts, i, out ExplicitWatch? watch, out int skip_match))
                    {
                        // get runmethod and put into parts
                        parts[i] = new RunMethod(watch.Runs.Runs, watch.Runs.Parameters, watch.Runs.ClassName, watch.Runs.Tokens);
                        // remove some of parts that go with runmethod
                        parts = parts.Take(i + 1).Concat(parts.Skip(i + skip_match + 1)).ToArray();
                        // make stringParts corelate with parts
                        // find string match of parts and remove found watch
                        var except = stringParts.Skip(i).Take(skip_match + 1).ToArray();
                        stringParts = stringParts.Except(except).ToArray();
                        // replace any commas with '\c' to keep from causing multiple parameters
                        except = except.Select(x => x.Replace(",", "\\c")).ToArray();
                        // create list and insert match into stringParts
                        var sp = stringParts.ToList();
                        sp.Insert(i, string.Join(" ", except));
                        // ser stringParts to temp list
                        stringParts = sp.ToArray();
                    }
                    else if (!insideClass && parts[i].ToString() == "method")
                    {
                        // Get method
                        Method method = SetMethod(ref lines, ref stringParts, lineIndex, out _);
                        parts = [method];
                        // Check if method already exists
                        if (Methods.Any(x => x.Name == method.Name) != false)
                        {
                            // If so, overide method
                            Method om = Methods.FirstOrDefault(x => x.Name == method.Name);
                            om = method;
                        }
                        else
                        {
                            // Else, add it to Method List
                            Methods.Add(method);
                        }
                    }
                    else if (parts[i].ToString() == "make")
                    {
                        // syntax,
                        /* 
                         * make A => B
                         * 
                         * make A {VAR} A => B {VAR} B
                         * 
                         *  make {
                         *      A {VAR}
                         *      A
                         *  } => {
                         *      B {VAR}
                         *      B
                         *  }
                         */

                        // Gets both sides of the make function
                        string[] both = string.Join(" ", partsSpaces.Skip(1)).Split("=>").Select(x => x.Trim()).ToArray();
                        // the take part
                        string take = both[0];
                        // the replace part
                        string replace = both.Length == (0 | 1) ? "" : both[1];
                        // the next line's index
                        int next = lineIndex + 1;
                        // if the make is multiline, these are the variabls to store them
                        string takeMulti = "", replaceMulti = "";
                        // if the take part starts with '{'
                        if (take.Trim() == "{")
                        {
                            // Handle multi-line matching
                            StringBuilder multiLineTake = new StringBuilder();
                            int braceCount = 1;

                            // Go through every line until it incounters a closing bracket
                            for (int j = next; j < lines.Length; j++)
                            {
                                braceCount -= lines[j].Value.Trim().StartsWith('}') ? 1 : 0;

                                if (braceCount == 0)
                                    break;

                                multiLineTake.AppendLine(lines[j].Value.ToString());

                                next++;
                            }
                            // if the last line of take does not contain '=>'
                            if (!lines[next].Value.Contains("=>"))
                            {
                                // throw error
                                return null;
                            }
                            else
                            {
                                // set replace to everything after '=>'. This is if the replace part is a single line.
                                replace = lines[next].Value.Split("=>")[1];
                            }

                            next++;
                            take = multiLineTake.ToString();
                            takeMulti = take.Split(['\n', '\r']).Select(x => x.Trim()).ToArray()[0];
                        }
                        // if the replace part starts with '{'
                        if (replace.Trim() == "{")
                        {
                            // Handle multi-line matching
                            StringBuilder multiLineTake = new StringBuilder();
                            int braceCount = 1;

                            // Go through every line until it incounters a closing bracket
                            for (int j = next; j < lines.Length; j++)
                            {
                                braceCount -= lines[j].Value.Trim().StartsWith('}') ? 1 : 0;

                                if (braceCount == 0)
                                    break;

                                multiLineTake.AppendLine(lines[j].Value.ToString());
                                next++;
                            }

                            // increment next
                            next++;
                            // set replace to the multi line
                            replace = multiLineTake.ToString();
                            replaceMulti = replace.Split('\n').Select(x => x.Trim()).ToArray()[0];
                        }
                        // create new array for all of the take lines
                        string[] takeLines = take.Split("\n").Select(x => x.Trim()).Where(y => y != "").ToArray();
                        // integer to remove lines if the take or replace is multi line so it doesn't get parsed later
                        int line_removes = 0, lr = -1;
                        // The temp lines
                        var _LINES = lines.ToList();

                        for (int j = lineIndex + 1; j < lines.Length; j++)
                        {
                            // remove line if the next variable is greater than the line index
                            if (next > lineIndex + 1)
                            {
                                lr = lr == -1 ? j : lr;
                                line_removes++;
                                next--;
                                continue;
                            }
                            string input = lines[j].Value.ToString(); // the string part of the line that will be replaced
                            // if the take is multi line
                            if (takeMulti != "")
                            {
                                string rep = replace; // temp replace that can be changed
                                // checks if input is a match
                                if (MakeMatch(input, takeLines[0], ref rep, out string output, false))
                                {
                                    bool match = false;
                                    Line[] takes = [new Line(output, j)];
                                    // checks if all of the lines in takemulti match
                                    for (int k = 1; k < takeLines.Length; k++)
                                    {
                                        match = MakeMatch(lines[j + k].Value.ToString(), takeLines[k], ref rep, out string o, false);
                                        // if it doesn't match, continue the outer loop
                                        if (!match) goto OuterLoop;
                                        // append takes 
                                        takes = takes.Append(new Line(o, j + k)).ToArray();
                                    }
                                    // Remove line from temp lines
                                    _LINES.RemoveRange(takes[0].CodeLine, takes[takes.Length - 1].CodeLine - takes[0].CodeLine + 1);
                                    // set rep back to replace
                                    rep = replace;
                                    // Make the multi match replace
                                    bool m = MakeMatchMulti(string.Join(Environment.NewLine, takes.Select(x => x.Value.ToString())), take, ref rep, out string val, true);

                                    // Insert the new converted lines at the correct spot
                                    var vals = val.Split(Environment.NewLine).Where(x => x != "").ToArray();
                                    for (int k = vals.Length - 1; k >= 0; k--)
                                    {
                                        _LINES.Insert(takes[0].CodeLine, new Line(vals[k], takes[0].CodeLine + k));
                                    }
                                }
                            }
                            else
                            {
                                // If take is a single line

                                string _ref = replace;
                                // If input is a match
                                if (MakeMatch(input, take, ref _ref, out string output, true))
                                {
                                    // if replace is multi line
                                    if (replaceMulti != "")
                                    {
                                        // Make replacement
                                        bool m = MakeMatchMulti(input, take, ref _ref, out string val, true);

                                        // Insert the new converted lines at the correct spot
                                        var vals = val.Split(Environment.NewLine).Where(x => x != "").ToArray();
                                        _LINES.RemoveAt(j);
                                        for (int k = vals.Length - 1; k >= 0; k--)
                                        {
                                            _LINES.Insert(j, new Line(vals[k], j + k));
                                        }
                                    }
                                    else
                                    {
                                        // replace line with outputs
                                        lines[j].Value = output;
                                    }
                                }
                            }

                        OuterLoop:
                            continue;
                        }
                        // Skip lines
                        for (int j = 0; j < line_removes; j++) _LINES.RemoveAt(lr);
                        // Set lines and part
                        lines = _LINES.ToArray();
                        parts = ["make"];
                    }
                    else if (parts[i].ToString().StartsWith("EZCodeLanguage.EZCode.DataType("))
                    {
                        // Example, EZCodeLanguage.EZCode.DataType("int")

                        // get entire part
                        string part = parts[i].ToString();
                        // get where '(' starts
                        int ch = Array.IndexOf(part.ToCharArray(), '(');
                        // gets the path from the part
                        string path = part[..ch];
                        // gets the type from inside the ""
                        string type = part.Substring(ch + 1, part.Length - ch - 2).Replace("\"", "");
                        // sets part to datattype
                        parts[i] = new CSharpDataType(path, type);
                    }
                    else if (parts[i].ToString() == "runexec")
                    {
                        // path of the CSharp Method
                        string path = parts[i + 1].ToString();
                        // parameters of the method
                        string[]? vars = null;
                        int skip = 1; // what to skip for the rest of the line
                        // checks if there are any parameters and if the arrow '~>' is used. Similar to ':' in normal methods
                        if (parts.Length - 1 > 2 && parts[i + 2].ToString() == "~>")
                        {
                            // increment skip
                            skip++;

                            string[] str = parts.Select(x => x.ToString()).Skip(3 + i).ToArray(); // get string array of all of the parameters
                            skip += str.Length; // skip according to the length of the parameters
                            string all = string.Join(" ", str); // get single string from 'str'
                            var paams = all.Split(",").Select(x => x.Trim()).ToArray(); // set 'parms' to 'all' split by commas. This is to get all parameters
                            vars = []; // sets the var array to empty
                            for (int j = 0; j < paams.Length; j++)
                            {
                                // adds the paramter to the array
                                vars = [.. vars, paams[j]];
                            }
                        }
                        for (int j = 0; j <= skip; j++)
                        {
                            // Remove the parts skipped by 'skip'
                            parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                        }
                        // Set part to CSharp method
                        parts[i] = new CSharpMethod(path, vars, path.Contains('\''));
                    }
                    else if (parts[i].ToString() == "container")
                    {
                        // gets all of the classes in the container
                        string name = parts[i + 1].ToString(); // name of container
                        string[] classNames = []; // all of the class names inside it
                        Line nextLine = lines[lineIndex + 1]; // the next line
                        bool sameLineBracket = nextLine.Value.StartsWith('{'); // if next line starts with '{'
                        List<Line> l = [.. lines]; // temp List<Line> for lines
                        int curleyBrackets = sameLineBracket ? 0 : 1; // variable to check when loop ends by looking at '{' '}'
                        for (int j = lineIndex + 1; j < lines.Length; j++)
                        {
                            Line bracketLine = lines[j];

                            // adjusts curleyBracket accordingly
                            if (bracketLine.Value.Contains('{')) curleyBrackets++;
                            if (bracketLine.Value.Contains('}')) curleyBrackets--;

                            // adds class name to list
                            classNames = [.. classNames, bracketLine.Value];

                            // removes current line and checks for end of container
                            l.Remove(bracketLine);
                            if (curleyBrackets == 0)
                                break;
                        }
                        // sets lines to temp 'l' to keep lines from inside container to be parsed
                        lines = [.. l];
                        // set part to container
                        parts = [new Container(name, classNames, lines[lineIndex])];
                    }
                }
                catch
                {

                }
            }
            return parts;
        }
        internal enum CurrentLineClassProperty
        {
            none,
            ismethod,
            isproperty, 
            isexplicit, 
            iswatch,
            isparam,
            istypeof, 
            isinsideof, 
            isget, 
            isclass, 
            isalias
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
            try
            {
                string match = "";
                for (int i = index; i < parts.Length; i++)
                {
                    string add = parts[i].ToString();
                    match = (match + " " + add).Trim();
                    for (int j = 0; j < Classes.Count; j++)
                    {
                        if (Classes[j].WatchFormat == null) continue;
                        for (int k = 0; k < Classes[j].WatchFormat.Length; k++)
                        {
                            if (Classes[j].WatchFormat[k].IsFound(match, Classes.ToArray(), Containers.ToArray()))
                            {
                                watch = new ExplicitWatch(Classes[j].WatchFormat[k].Pattern, Classes[j].WatchFormat[k].Runs, Classes[j].WatchFormat[k].Vars);
                                return true;
                            }
                        }
                    }
                    skip++;
                }
            }
            catch
            {

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
                    if (sides.Length > 1)
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
        private Statement SetStatement(ref Line[] lines, ref string[] strParts, int lineIndex, int partIndex)
        {
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
                argTokens = argTokens.Append(SingleToken(parts, j, parts[partIndex].ToString())).ToArray();
            }
            argTokens = argTokens.Take(argTokens.Length - end).ToArray();
            if (argTokens.FirstOrDefault(new Token(TokenType.None, "", "")).Value.ToString() == "runexec")
            {
                Line[] l = [new Line(lines[lineIndex].Value, lines[lineIndex].CodeLine)];
                l[0].Value = string.Join(" ", argTokens.Select(x => x.Value.ToString()));
                
                object[] objects = SplitParts(ref l, 0, 0, ref strParts, out _, out _);
                Token[] t = [];
                for (int i = 0; i < objects.Length; i++)
                    t = [.. t, SingleToken(objects, i, parts[partIndex].ToString())];
                argTokens = t;
            }
            if (Statement.ConditionalTypes.Contains(parts[partIndex]))
            {
                argTokens = TokenArray(string.Join(" ", argTokens.Select(x => x.Value.ToString())))[0].Tokens;
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
                        l.Remove(bracketLine);
                        if (curleyBrackets == 0)
                            break;
                        code += bracketLine.Value + Environment.NewLine;
                    }
                    lineWithTokens = Parse(code);
                    lineWithTokens = lineWithTokens.Select(x => { x.Line.CodeLine++; return x; }).ToArray();
                    if (lineWithTokens.Last().Line.Value.ToString() == "}")
                    {
                        lineWithTokens = lineWithTokens.Where((x, y) => y != lineWithTokens.Length - 1).ToArray();
                    }
                    if (lineWithTokens[0].Line.Value == "{") lineWithTokens = lineWithTokens.Where((x, y) => y != 0).ToArray();
                    lines = [.. l];
                }
            }
            for (int i = 0; i < lineWithTokens.Length; i++)
                lineWithTokens[i].Line.CodeLine += 1;
            return new Statement(parts[partIndex].ToString(), lines[lineIndex], lineWithTokens, argument);
        }
        private Method SetMethod(Line[] lines, ref string[] strParts, int index) => SetMethod(ref lines, ref strParts, index, out _);
        private Method SetMethod(Line[] lines, ref string[] strParts, int index, out string returns) => SetMethod(ref lines, ref strParts, index, out returns);
        private Method SetMethod(ref Line[] lines, ref string[] strParts, int index, out string returns)
        {
            // Get the current line
            Line line = lines[index];
            line.CodeLine += 1;
            returns = "";
            bool global = false;
            if (new string(line.Value.Trim().Prepend(':').ToArray()).Contains(":global ") || 
                new string(line.Value.Trim().Prepend(':').ToArray()).Contains(":nocol global ")) global = true;
            Method.MethodSettings settings =
                line.Value.Trim().StartsWith("nocol ") && !global ? Method.MethodSettings.NoCol :
                line.Value.Contains(" nocol ") && global ? Method.MethodSettings.NoCol | Method.MethodSettings.Global :
                global ? Method.MethodSettings.Global :
                Method.MethodSettings.None;
            DataType? _returns = null;
            Var[]? param = [];
            Token[] firstLineTokens = TokenArray(string.Join(" ", line.Value.Split(" ").SkipWhile(x => x != "method").Skip(1)))[0].Tokens;
            string name = firstLineTokens[0].Value.ToString()!;
            bool ret = false, req = true, para = false;
            for (int i = 1; i < firstLineTokens.Length; i++)
            {
                Token token = firstLineTokens[i];
                if (ret)
                {
                    if(token.Type == TokenType.DataType)
                    {
                        returns = token.Value.ToString();
                        _returns = DataType.GetType(token.Value.ToString()!, Classes.ToArray(), Containers.ToArray());
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
                    if (!para)
                    {
                        if (firstLineTokens[i + 1].Type == TokenType.QuestionMark)
                        {
                            req = false;
                            i++;
                        }
                        bool pTypeDef = firstLineTokens[i + 1].Type == TokenType.DataType;
                        string pName = "";
                        object? pVal = null;
                        DataType pType = DataType.UnSet;
                        if (pTypeDef)
                        {
                            pType = DataType.GetType(firstLineTokens[i + 1].Value.ToString()!, Classes.ToArray(), Containers.ToArray());
                            if (firstLineTokens[i + 2].Type == TokenType.Colon)
                            {
                                if (firstLineTokens[i + 3].Type == TokenType.Colon)
                                {
                                    pName = firstLineTokens[i + 4].Value.ToString()!;
                                    para = true;
                                    i += 4;
                                }
                                else
                                {
                                    pName = firstLineTokens[i + 3].Value.ToString()!;
                                    i += 3;
                                }
                            }
                            else i += 1;
                        }
                        else
                        {
                            pName = firstLineTokens[i + 1].Value.ToString()!;
                            i += 1;
                        }
                        if (firstLineTokens.Length - 1 > i + 1 && firstLineTokens[i + 1].Type == TokenType.Identifier)
                        {
                            pVal = string.Join(" ", firstLineTokens.Skip(i + 1).TakeWhile(x => x.Type == TokenType.Identifier).Select(x => x.Value));
                        }
                        param = param.Append(new Var(name: pName, value: pVal, line: line, type: pType, required: req, @params: para)).ToArray();
                    }
                    else
                    {
                        // throw error
                    }
                }
            }
            int start = index + 1;
            LineWithTokens[] lineWithTokens = [];
            Line nextLine = lines[start];
            bool sameLineBracket = nextLine.Value.StartsWith('{');
            int curleyBrackets = sameLineBracket ? 0 : 1;
            for (int i = start; i < lines.Length; i++)
            {
                Line bracketLine = lines[i];
                Token[] bracketLineTokens = TokenArray(bracketLine.Value)[0].Tokens;
                try
                {
                    if (Statement.Types.Contains(bracketLineTokens[0].Value))
                    {
                        Statement statement = SetStatement(ref lines, ref strParts, i, 0);
                        bracketLineTokens = [SingleToken([statement], 0, string.Join(" ", statement.Line.Value))];
                        if (bracketLine.Value.Contains("{") && !bracketLine.Value.Contains("}")) curleyBrackets--;
                    }
                } catch { }
                if (bracketLine.Value.Contains('{'))
                    curleyBrackets++;
                if (bracketLine.Value.Contains('}'))
                    curleyBrackets--;
                lineWithTokens = [.. lineWithTokens, new LineWithTokens(bracketLineTokens, bracketLine)];
                if (curleyBrackets == 0)
                    break;
            }
            for (int i = 0; i < lineWithTokens.Length; i++)
                lines = lines.Where((x, y) => y != start).ToArray();
            
            if (lineWithTokens.Last().Line.Value.ToString() == "}")
            {
                lineWithTokens = lineWithTokens.Where((x, y) => y != lineWithTokens.Length - 1).ToArray();
            }
            if (lineWithTokens[0].Line.Value == "{") lineWithTokens = lineWithTokens.Where((x, y) => y != 0).ToArray();

            for (int i = 0; i < lineWithTokens.Length; i++)
                lineWithTokens[i].Line.CodeLine += 1;
            return new Method(name, line, settings, lineWithTokens, param, _returns);
        }
        private GetValueMethod SetGetVal(Line[] lines, int index)
        {
            Line line = new Line(string.Join(" ", lines[index].Value.Split(" ").Prepend("method")), index);
            line.CodeLine += 1;
            Line[] liness = lines.Select((x, y) => y == index ? line : x).ToArray();
            string[] r = { };
            Method method = SetMethod(liness, ref r, index, out string returns);
            method.Line = lines[index];
            method.Name = null;
            GetValueMethod getValue = new GetValueMethod(method.Returns, method, returns);
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
            // Generates an empty Line array to return
            Line[] lines = Array.Empty<Line>();

            int index = 0; // index of loop
            string[] string_lines = code.Split('\n');
            string_lines = string_lines.Select(s => s.Trim()).ToArray(); // splits code by each line
            foreach (var item in string_lines)
            {
                // Append line to the array, 'lines'
                lines = lines.Append(new Line(item, index)).ToArray();
                index++; // increment the index by one
            }
            return lines;
        }
    }
}