using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using static EZCodeLanguage.Parser;

namespace EZCodeLanguage
{
    public class Interpreter
    {
        public static Interpreter Instance { get; private set; }
        public event EventHandler OutputWrote;
        public event EventHandler OutputCleared;
        private string[] _output = [];
        public string[] Output
        {
            get => _output;
            internal set
            {
                _output = value ?? [];
                if (_output.Length == 0 && OutputCleared != null)
                {
                    OutputCleared.Invoke(this, EventArgs.Empty);
                }
                else if (OutputWrote != null)
                {
                    OutputWrote.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private string? Input = null;
        public enum EZInputType { Console, InputMethod }
        public EZInputType InputType { get; set; } = EZInputType.Console;
        public void SetInput(string input, EZInputType inputType = EZInputType.InputMethod)
        {
            InputType = inputType;
            Input = input;
        }
        internal string GetInput()
        {
            string input;
            if (InputType == EZInputType.Console) input = Console.ReadLine();
            else
            {
                while (Input == null)
                {
                    Task.Delay(100);
                }
                input = Input;
            }
            Input = null;
            return input;
        }
        public bool ContinuedForBreakpoint = false;
        public Parser parser { get; private set; }
        public EZHelp EZHelp { get; private set; }
        public Interpreter(Parser parser, Debug.Breakpoint[]? breakpoints = null)
        {
            Instance = this;
            StackTrace = new Stack<string>();
            this.parser = parser;
            EZHelp = new EZHelp(this);
            Methods = parser.Methods.ToArray();
            Classes = parser.Classes.ToArray();
            Breakpoints = breakpoints;

            for (int i = 0; i < Classes.Length; i++)
            {
                if (Classes[i].Properties == null) continue;
                for (int j = 0; j < Classes[i].Properties.Length; j++)
                {
                    Var var = Classes[i].Properties[j];
                    if (var.DataType.ObjectClass == null)
                    {
                        var.DataType = DataType.GetType(var.Line.Value.Split(" ")[0], Classes);
                        if (var.DataType.ObjectClass == null)
                        {
                            var.DataType.ObjectClass = Classes[i];
                        }
                    }
                    if (var.Value == null)
                    {
                        Parser p = new Parser(var.Line.Value, var.Line.FilePath);
                        Token[] parsertokens = p.Parse()[0].Tokens;
                        LineWithTokens lineWithTokens = new LineWithTokens(parsertokens.Skip(0).ToArray(), var.Line);
                        object obj = null;
                        try { obj = GetValue(lineWithTokens.Tokens.Skip(var.Line.Value.Contains("new :") ? 4 : 3).ToArray()); } catch { }
                        var.Value = string.IsNullOrEmpty(obj.ToString()) ? null : obj;
                    }
                }
                for (int j = 0; j < Classes[i].GetTypes.Length; j++)
                {
                    GetValueMethod get = Classes[i].GetTypes[j];
                    if (get.DataType == null || get.DataType.ObjectClass == null)
                    {
                        get.DataType = DataType.GetType(get.Returns, Classes);
                    }
                }
            }
        }
        private bool MessageHandled = false;
        private bool LastIfWasTrue = true;
        private Exception? LastTryNotFailed = null;
        private bool yielded = false;
        private object? returned = null;
        private DataType? Returning = null;
        private int StackNumber = 0;
        internal bool hasexited = false;
        public CustomAssemblyLoadContext[] LoadedAssemblies = [];
        public Stack<string> StackTrace { get; private set; }
        public Exception[] Errors { get; private set; } = [];
        public Var[] Vars { get; set; } = [];
        public Method[] Methods { get; set; } = [];
        public Class[] Classes { get; set; }
        public Debug.Breakpoint[]? Breakpoints { get; set; }
        public Line CurrentLine { get; private set; }
        private bool StartMethodEntry = false;
        public void Interperate() => Interperate(parser.LinesWithTokens);
        public void Interperate(LineWithTokens[] LineTokens)
        {
            var temp_stack = new Stack<string>(StackTrace);

            Package.RemoveAllPackagesFromExecutionDirectory(AppDomain.CurrentDomain.BaseDirectory);

            if (Methods.Any(x => x.Name.ToLower() == "start") && !StartMethodEntry)
            {
                StartMethodEntry = true;
                LineWithTokens[] lines_with_include = LineTokens.Where(x => x.Tokens.Length > 1).Where(x => x.Tokens[0].Type == TokenType.Include).ToArray();
                LineWithTokens[] lines_with_exclude = LineTokens.Where(x => x.Tokens.Length > 1).Where(x => x.Tokens[0].Type == TokenType.Exclude).ToArray();
                if (lines_with_include.Length > 0)
                    Interperate(lines_with_include);
                if (lines_with_exclude.Length > 0)
                    Interperate(lines_with_exclude);
                LineWithTokens[] lines_with_global = LineTokens.Where(x => x.Tokens.Length > 1).Where(x => x.Tokens[0].Type == TokenType.Global).ToArray();
                if (lines_with_global.Length > 0)
                    Interperate(lines_with_global);

                Interperate(Methods.First(x => x.Name.ToLower() == "start").Lines);
            }
            else
            {
                foreach (LineWithTokens line in LineTokens) 
                {
                    if (hasexited) break;
                    if (line.Tokens.Length == 0 || (line.Tokens.Length == 1 && line.Tokens[0].Value is Class or Method))
                        continue;

                    var backup_vars = Vars.Select(x => new Var(x.Name, x.Value, x.Line, x.StackNumber, x.DataType, x.Required)).ToArray();
                    var backup_methods = Methods.Select(x => new Method(x.Name, x.Line, x.Settings, x.Lines, x.Parameters, x.Returns)).ToArray();
                    try
                    {
                        temp_stack = new Stack<string>(StackTrace);
                        SingleLine(line);
                    }
                    catch (Exception ex)
                    {
                        MessageHandled = false;
                        string stack = string.Join("\n\t", StackTrace.Reverse());
                        string message = ex.Message + ", StackTrace: \n\t" + (stack != "" ? stack : "Stack Empty");
                        Output = [.. Output, message];
                        Console.WriteLine(message);
                        Errors = Errors.Append(ex).ToArray();
                        StackTrace = temp_stack;
                        Vars = backup_vars;
                        Methods = backup_methods;
                        EZHelp.Error = null;
                    }
                }
            }

            // Unload each assembly
            foreach (var assembly in LoadedAssemblies)
            {
                assembly.Unload();
            }
            // Let the garbage collector collect to ensure assemblies are fully unloaded
            for (int i = 0; i < 3; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            // Exclude every package
            Package.RemoveAllPackagesFromExecutionDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }
        internal object? SingleLine(LineWithTokens line)
        {
            StackNumber++;
            try
            {
                string message = $"codeline: \"{line.Line.Value}\", file: \"{line.Line.FilePath}\", line: {line.Line.CodeLine}";
                bool duplicate_stack = false;
                try { duplicate_stack = StackTrace.First() != message; } catch { }
                if (!duplicate_stack) StackTrace.Push(message);
                CurrentLine = line.Line;

                while (Breakpoints != null && Debug.IsHit(line.Line, Breakpoints, this) && !ContinuedForBreakpoint)
                {
                    Task.Delay(100);
                }
                ContinuedForBreakpoint = false;

                Token FirstToken = line.Tokens.FirstOrDefault(new Token(TokenType.None, "", ""));
                object? Return = null;
                switch (FirstToken.Type)
                {
                    case TokenType.Include:
                    case TokenType.Exclude:
                        try
                        {
                            string combined_packages = string.Join(" ", line.Tokens.Skip(1).Select(x => x.StringValue));
                            string[] packages = combined_packages.Split(",").Select(x=>x.Trim()).ToArray();
                            PackageClass[] projects = new PackageClass[packages.Length];
                            bool include = FirstToken.StringValue == "include";

                            for (int i = 0; i < packages.Length; i++)
                                projects[i] = Package.GetPackageAsProject(packages[i]);

                            if (projects.Any(x => !string.IsNullOrEmpty(x.Configuration?.LibraryDirectory)))
                            {
                                string destination = AppDomain.CurrentDomain.BaseDirectory;
                                foreach (var project in projects)
                                {
                                    if (include)
                                        Package.AddPackageToExecutionDirectory(project, destination);
                                    else
                                        Package.RemovePackageFromExecutionDirectory(project, destination);
                                }
                            }

                            if (include)
                            {
                                parser = Package.ReturnParserWithPackages(parser, packages);
                                Methods = [.. Methods, .. parser.Methods];
                                Classes = [.. Classes, .. parser.Classes];
                            }
                            else
                            {
                                Parser except = Package.ReturnParserWithPackages(new Parser("", ""), packages);
                                parser = Package.RemovePackageFromParser(parser, except);
                                Methods = parser.Methods.ToArray();
                                Classes = parser.Classes.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error with \"{FirstToken.StringValue}\", Error Message:\"{ex.Message}\"");
                        }
                        break;

                    case TokenType.Identifier:
                        try
                        {
                            if (float.TryParse(FirstToken.StringValue, out float val) && line.Tokens.Length == 1)
                            {
                                Return = val;
                            }
                            else
                            {
                                switch (IsType(FirstToken.Value.ToString()!, out object? type))
                                {
                                    case IdentType.Var:
                                        Var var = type as Var;
                                        if (line.Tokens.Length > 1)
                                        {
                                            if (line.Tokens[1].Type == TokenType.Arrow)
                                            {
                                                var tok = line.Tokens.Skip(2).ToArray();
                                                var.Value = SingleLine(new LineWithTokens(tok, line.Line));
                                                Return = var.Value;
                                            }
                                            else
                                            {
                                                if (var.Value is Class || (var.DataType.ObjectClass != null && var.DataType.Type != DataType.Types._null))
                                                {
                                                    Class c = var.Value is Class ? var.Value as Class : var.DataType.ObjectClass;

                                                    if (c.Properties.Any(x => x.Name.ToLower() == "value"))
                                                        c.Properties.FirstOrDefault(x => x.Name.ToLower() == "value").Value ??= var.Value;
                                                    
                                                    if (c.Methods.Any(x => x.Name == line.Tokens[1].Value.ToString()))
                                                    {
                                                        Token[] backup_tokens = line.Tokens.Select(x => new Token(x.Type, x.Value, x.StringValue)).ToArray();
                                                        try
                                                        {
                                                            for (int i = 2; i < line.Tokens.Length; i++)
                                                            {
                                                                line.Tokens[i].Value = GetValue(line.Tokens[i].Value, var.DataType);
                                                                line.Tokens[i].StringValue = line.Tokens[i].Value is string or int or float or bool ? line.Tokens[i].Value.ToString() : line.Tokens[i].StringValue;
                                                                line.Tokens[i].Type = parser.SingleToken([line.Tokens[i].StringValue == line.Tokens[i].Value.ToString() ? line.Tokens[i].StringValue : line.Tokens[i].Value], 0, line.Tokens[i].StringValue).Type;
                                                            }
                                                        }
                                                        catch 
                                                        {
                                                            line.Tokens[2].Value = GetValue(string.Join(" ", line.Tokens.Skip(2).Select(x => x.StringValue)));
                                                            line.Tokens[2].StringValue = line.Tokens[2].Value.ToString();
                                                            line.Tokens = line.Tokens.Take(3).ToArray();
                                                        }
                                                        Method[] backupMethods = Methods;
                                                        Methods = c.Methods.Concat(Methods.Where(x => (x.Settings & Method.MethodSettings.Global) == Method.MethodSettings.Global)).ToArray();
                                                        Var[] backupVars = Vars.Concat(Vars.Where(x => x.Global)).ToArray();
                                                        Vars = c.Properties;
                                                        line.Tokens = line.Tokens.Skip(1).ToArray();
                                                        object value = SingleLine(line);
                                                        line.Tokens = backup_tokens;
                                                        Methods = backupMethods;
                                                        Vars = backupVars;
                                                        Return = value;
                                                    }
                                                    else if (c.Properties.FirstOrDefault(x => x.Name == line.Tokens[1].Value.ToString()) is Var v)
                                                    {
                                                        return v.Value;
                                                    }
                                                    else
                                                    {
                                                        throw new Exception($"Variable is trying to call a method \"{line.Tokens[1].Value}\" that doesn't exists");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception($"Variable type is undefined and can not call the method \"{line.Tokens[1].Value}\"");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Return = var.Value;
                                        }
                                        break;

                                    case IdentType.Class:
                                        Class cl = new Class(type as Class);
                                        if (line.Tokens.Length > 2 && line.Tokens[2].Type == TokenType.New)
                                        {
                                            string name = line.Tokens[1].Value.ToString();
                                            if (Vars.Any(x => x.Name == name))
                                            {
                                                var = Vars.FirstOrDefault(x => x.Name == name);
                                                if (var.Value is Class)
                                                {
                                                    if ((var.Value as Class).Name != cl.Name)
                                                        throw new Exception("Can not create a class instance with the same name as another class instance of another class");
                                                    else cl = (Class)var.Value;
                                                }
                                                else throw new Exception("Can not create a class instance with the same name as another class instance of another class");
                                            }
                                            else if (Methods.Any(x => x.Name == name) || Classes.Any(x => x.Name == name))
                                                throw new Exception("Can not create a new class instance with the same name as a method or class");
                                            else if (name == "exeption")
                                                throw new Exception("The name \"error\" is reserved for the fail statement error message");
                                            else
                                                var = new Var(name, cl, line.Line, stackNumber: StackNumber, type: new DataType(cl.TypeOf != null ? cl.TypeOf.Type : DataType.Types._object, cl, cl.Name));
                                            Vars = [.. Vars, var];

                                            if (line.Tokens.Length > 3)
                                            {
                                                if (line.Tokens[3].Type == TokenType.Colon)
                                                {
                                                    if (cl.Params != null)
                                                    {
                                                        ExplicitParams? p = new ExplicitParams(cl.Params.Pattern, cl.Params.Runs, cl.Params.Vars, cl.Params.All);
                                                        if (parser.ParamIsFound(line.Tokens.Select(x => x.Value).ToArray(), 4, out _))
                                                        {
                                                            if (p.All)
                                                            {
                                                                RunMethod run = new RunMethod(p.Runs.Runs, p.Runs.Parameters, p.Runs.ClassName, p.Runs.Tokens);
                                                                Method[] backupMethods = Methods;
                                                                Var[] backupVars = Vars, backupParams = p.Runs.Parameters.Select(x => new Var(x.Name, x.Value, x.Line, x.StackNumber, x.DataType, x.Required)).ToArray();
                                                                Vars = [.. cl.Properties.Concat(Vars.Where(x => x.Global)), .. Vars];
                                                                Methods = [.. cl.Methods.Concat(Methods.Where(x => (x.Settings & Method.MethodSettings.Global) == Method.MethodSettings.Global)), .. Methods];
                                                                Var v = run.Parameters.FirstOrDefault(x => x.Name == "PARAMS");
                                                                if (v is not null)
                                                                {
                                                                    if (v.Value is string) v.Value = string.Join(" ", line.Tokens.Select(x => x.Value).Skip(4));
                                                                    else
                                                                    {
                                                                        v.Value = GetValue(new Token(TokenType.Identifier, line.Tokens.Skip(4).ToArray(), string.Join(" ", line.Tokens.Select(x => x.StringValue))));
                                                                        v.DataType = DataType.TypeFromValue(v.Value.ToString(), Classes);
                                                                    }
                                                                }
                                                                MethodRun(run.Runs, run.Parameters);
                                                                p.Runs.Parameters = backupParams;
                                                                Methods = backupMethods;
                                                                Vars = backupVars;
                                                            }
                                                            else
                                                            {
                                                                throw new Exception($"Params format is not implemented in the current version of EZCode");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            throw new Exception($"The format pattern of the class, \"{p.Pattern}\" does not match this class instance");
                                                        }
                                                    }
                                                    else if (cl.Properties != null)
                                                    {
                                                        string all = string.Join(" ", line.Tokens.Skip(4).Select(x => x.Value));
                                                        string[] vals = all.Split(',').Select(x => x.Trim()).ToArray();
                                                        Var[] prop = [];
                                                        if (vals.Length > 0)
                                                        {
                                                            for (int i = 0; i < vals.Length; i++)
                                                            {
                                                                string[] whole = vals[i].Split(":");
                                                                if (whole.Length < 2) throw new Exception("Expected properties to be set by syntax \"PropertyName:Value\"");
                                                                string before = whole[0].Trim();
                                                                string after = whole[1].Trim();
                                                                if (cl.Properties.Any(x => x.Name == before))
                                                                {
                                                                    Var v = cl.Properties.FirstOrDefault(x => x.Name == before);
                                                                    after = GetValue(after).ToString();
                                                                    prop = [.. prop, new Var(v.Name, after, line.Line, stackNumber: StackNumber, type: v.DataType)];
                                                                }
                                                                else
                                                                {
                                                                    throw new Exception($"There is no \"{before}\" property in the \"{cl.Name}\" class");
                                                                }
                                                            }
                                                        }
                                                        (var.Value as Class).Properties = (var.Value as Class).Properties.Where(x => !prop.Any(y => y.Name == x.Name)).Concat(prop).ToArray();
                                                    }
                                                    else
                                                    {
                                                        throw new Exception($"Class \"{cl.Name}\" does not have any properties");
                                                    }
                                                }
                                                else if (line.Tokens[3].Type == TokenType.Arrow)
                                                {
                                                    line.Tokens = line.Tokens.Skip(4).ToArray();
                                                    var value = SingleLine(line);
                                                    if (value == null)
                                                    {
                                                        throw new Exception("Expected method that returns value");
                                                    }
                                                    if (var.Value is Class c && c.Properties.FirstOrDefault(x => x.Name.ToLower() == "value") is Var v)
                                                    {
                                                        v.Value = value;
                                                    }
                                                    else
                                                    {
                                                        var.Value = value;
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception("Expected colon or arrow to set instance properties");
                                                }
                                            }
                                            Return = var;
                                        }
                                        else if (line.Tokens.Length > 1)
                                        {
                                            Methods = [ .. cl.Methods, .. Methods];
                                            Vars = [ .. cl.Properties, .. Vars];
                                            line.Tokens = line.Tokens.Skip(1).ToArray();
                                            Return = SingleLine(line);
                                            Methods = Methods.Except(cl.Methods).ToArray();
                                            Vars = Vars.Except(cl.Properties).ToArray();
                                        }
                                        else
                                        {
                                            throw new Exception("Expected \"new\" keyword to declare instance of class");
                                        }
                                        break;

                                    case IdentType.Method:
                                        Method method = (type is Method m) ?
                                            new Method(m.Name, m.Line, m.Settings, m.Lines.Select(
                                                x => new LineWithTokens(x.Tokens.Select(y => new Token(
                                                    y.Type, y.Value, y.StringValue)).ToArray(), x.Line)).ToArray(), m.Parameters, m.Returns)! : new Method();

                                        Method.MethodSettings settings = method.Settings;
                                        bool nocol = (settings & Method.MethodSettings.NoCol) != 0;
                                        Var[] vars = [];
                                        if (line.Tokens.Length != 1)
                                        {
                                            if (!nocol && line.Tokens[1].Type != TokenType.Colon)
                                            {
                                                throw new Exception("Expected \":\" identifier to set method perameters");
                                            }

                                            if (method.Parameters != null && method.Parameters.Length > 0)
                                            {
                                                int next = nocol ? 0 : 1;
                                                string all;
                                                object?[] vals;

                                                try
                                                {
                                                    all = string.Join(" ", line.Tokens.Select((x, y) => x.Value is RunMethod r ? "@:|" + y.ToString() + "{}" : x.StringValue).Skip(next + 1));
                                                    string[] all_parts = all.Split(',');
                                                    if (all_parts.Length > method.Parameters.Select(x => x.Required).ToArray().Length && !method.Parameters.Any(x => x.IsParams))
                                                        throw new Exception($"Expects {(method.Parameters.Any(x => x.Required) ? "at least" : "")} {(method.Parameters.Any(x => x.Required) ? method.Parameters.Select(x => x.Required).ToArray().Length : method.Parameters.Length)} parameter for method \"{method.Name}\" but {all_parts.Length} were given");

                                                    all_parts = GetIsParamParameters(all_parts, method);

                                                    vals = all_parts.Select(x => x.Trim()).Select( (selectValue, selectIndex) => 
                                                        selectValue.StartsWith("@:|") && selectValue.EndsWith("{}") ? 
                                                        line.Tokens[int.Parse( selectValue.Substring(3, selectValue.Length - 5)) ].Value : 
                                                        GetValue(selectValue, method.Parameters[selectIndex].DataType) )
                                                        .Where(x => x.ToString() != "").ToArray();
                                                }
                                                catch (Exception e)
                                                {
                                                    if (e.Message.StartsWith("Expects "))
                                                        throw new Exception(e.Message);
                                                    throw new Exception("Error getting values for method paramters");
                                                }
                                                if (vals.FirstOrDefault(x => x is RunMethod) is RunMethod r)
                                                {
                                                    for (int i = 0; i < r.Parameters.Length; i++)
                                                    {
                                                        r.Parameters[i].Value = GetValue(r.Parameters[i], r.Parameters[i].DataType);
                                                    }
                                                }
                                                if (vals.Length > 0)
                                                {
                                                    for (int i = 0; i < method.Parameters.Length; i++)
                                                    {
                                                        if (!method.Parameters[i].Required && vals.Length - 1 < i)
                                                            continue;
                                                        object value = method.Parameters[i].IsParams ? string.Join(",", vals.Skip(i)) : vals[i];
                                                        vars = [.. vars, new Var(method.Parameters[i].Name, value, line.Line, stackNumber: StackNumber, type: method.Parameters[i].DataType, required: method.Parameters[i].Required)];
                                                    }
                                                    if (vars.Where(x => x.Required).ToArray().Length != method.Parameters.Where(x => x.Required).ToArray().Length)
                                                    {
                                                        throw new Exception("Not all parameters are set");
                                                    }
                                                }
                                                else
                                                {
                                                    if (line.Tokens.Length > 1)
                                                    {
                                                        throw new Exception("Method does not require any parameters");
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (method.Parameters != null && method.Parameters.Where(x => x.Required).ToArray().Length > 0)
                                            {
                                                throw new Exception($"Method \"{method.Name}\" expects parameters");
                                            }
                                        }

                                        Return = MethodRun(method, vars);
                                        break;

                                    default:
                                    case IdentType.Other:
                                        throw new Exception("The identifier \"" + FirstToken.Value.ToString() + "\" does not exist in this current context");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"{line.Line.Value}\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.RunExec:
                        try
                        {
                            CSharpMethod method = FirstToken.Value as CSharpMethod;
                            StackTrace.Push($"csharp-method: \"{(method.Path != "" ? method.Path : "Null")}\", file: \"{line.Line.FilePath}\", line: {line.Line.CodeLine}");
                            object obj = Reflect(method!);
                            StackTrace.TryPop(out _);
                            Return = obj;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception($"Error with \"runexec\" in C# method. Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Undefined:
                        try
                        {
                            Token tokenName = line.Tokens[1];
                            object? value = null;
                            if (line.Tokens.Length > 2)
                            {
                                if (line.Tokens[2].Type == TokenType.Arrow)
                                {
                                    line.Tokens = line.Tokens.Skip(3).ToArray();
                                    value = SingleLine(line);
                                    if (value == null)
                                    {
                                        throw new Exception("Expected method that returns value");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Expected arrow identifier \"=>\" to set value");
                                }
                            }
                            Var var = new Var(tokenName.Value.ToString(), value, line.Line, stackNumber: StackNumber, type: null);
                            Vars = [.. Vars, var];
                            Return = var;
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"undefined\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Throw:
                        string err_message = "";
                        try
                        {
                            line.Tokens = line.Tokens.Skip(1).Prepend(new Token(TokenType.Return, "return", "return")).ToArray();
                            err_message = SingleLine(line).ToString();
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"throw\", Error Message:\"{ex.Message}\"");
                        }
                        if (err_message != "")
                            throw new Exception(err_message);
                        break;
                    case TokenType.Dispose:
                        try
                        {
                            var tokens = line.Tokens.Skip(1).ToArray();
                            var variablesTokens = tokens.Where(x => x.Type != TokenType.Comma).ToArray();
                            var variables = new Var[0];
                            for (int i = 0; i < variablesTokens.Length; i++)
                            {
                                var variable = Vars.FirstOrDefault(x => x.Name == variablesTokens[i].StringValue);

                                if (variables is null)
                                    throw new Exception($"Variable \"{variablesTokens[i].StringValue}\" does not exist and cannot be disposed");

                                variables = [.. variables, variable];
                            }
                            Vars = Vars.Except(variables).ToArray();
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"dispose\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Return:
                        try
                        {
                            line.Tokens = line.Tokens.Skip(1).ToArray();
                            Return = SingleLine(line) ?? GetValue(line.Tokens);
                            returned = Return;
                        }
                        catch (Exception ex)
                        {
                            string stringReturns = string.Join(" ", line.Tokens.Select(x => x.StringValue));
                            if (stringReturns != string.Empty && ex.Message.Contains($"Error Message:\"The identifier \"{line.Tokens.First().StringValue}\" does not exist in this current context\""))
                            {
                                try
                                {
                                    Return = GetValue(stringReturns);
                                    returned = Return;
                                }
                                catch
                                {
                                    if (MessageHandled) throw;
                                    MessageHandled = true;
                                    throw new Exception($"Error with \"return\", Error Message:\"{ex.Message}\"");
                                }
                            }
                            else
                            {
                                if (MessageHandled) throw;
                                MessageHandled = true;
                                throw new Exception($"Error with \"return\", Error Message:\"{ex.Message}\"");
                            }
                        }
                        break;
                    case TokenType.Global:
                        try
                        {
                            line.Tokens = line.Tokens.Skip(1).ToArray();
                            Return = SingleLine(line);
                            if (Return is not Var)
                                throw new Exception("Expected variable declaration after use of \"global\" keyword");
                            else (Return as Var).Global = true;
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"global\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.If:
                        try
                        {
                            Statement? statement = FirstToken.Value as Statement;
                            Argument[]? arguments = statement!.Argument!.Args() ?? throw new Exception($"Problem with argument \"{statement.Argument.StringValue}\"");
                            string[] parts = [];
                            for (int i = 0; i < arguments.Length; i++)
                            {
                                Argument arg = arguments[i];
                                bool? istrue = ArgumentIsTrue(arg);
                                parts = [.. parts, istrue.ToString()];
                                if (arg.ArgAdd != Argument.ArgAdds.None) parts = [.. parts, arg.ArgAdd.ToString()];
                            }
                            bool run = EZHelp.Evaluate(string.Join(" ", parts));
                            if (run)
                            {
                                LastIfWasTrue = true;
                                RunStatement(statement, out _);
                            }
                            else
                            {
                                LastIfWasTrue = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"if\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Elif:
                        try
                        {
                            if (LastIfWasTrue) break;
                            Statement? statement = FirstToken.Value as Statement;
                            Argument[]? arguments = statement!.Argument!.Args() ?? throw new Exception($"Problem with argument \"{statement.Argument.StringValue}\"");
                            string[] parts = [];
                            for (int i = 0; i < arguments.Length; i++)
                            {
                                Argument arg = arguments[i];
                                bool? istrue = ArgumentIsTrue(arg);
                                parts = [.. parts, istrue.ToString()];
                                if (arg.ArgAdd != Argument.ArgAdds.None) parts = [.. parts, arg.ArgAdd.ToString()];
                            }
                            bool run = EZHelp.Evaluate(string.Join(" ", parts));
                            if (run)
                            {
                                LastIfWasTrue = true;
                                RunStatement(statement, out _);
                            }
                            else
                            {
                                LastIfWasTrue = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"elif\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Else:
                        try
                        {
                            if (LastIfWasTrue) break;
                            Statement? statement = FirstToken.Value as Statement;
                            LastIfWasTrue = true;
                            RunStatement(statement, out _);
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"else\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Loop:
                        try
                        {
                            Statement? statement = FirstToken.Value as Statement;
                            Argument[]? arguments = statement!.Argument!.Args() ?? throw new Exception($"Problem with argument \"{statement.Argument.StringValue}\"");

                            object TryFloat = GetValue(statement.Argument.Tokens, DataType.GetType("float", Classes));
                            float? floatParse = EZHelp.Try<float?>(() => float.Parse(TryFloat.ToString()!));
                            if (floatParse != null) 
                            {
                                if (int.TryParse(floatParse.ToString(), out int parse))
                                {
                                    for (int i = 0; i < parse; i++)
                                    {
                                        RunStatement(statement, out bool broke);
                                        if (broke) break;
                                    }
                                }
                                else throw new Exception("Expected integer for loop count");
                            }
                            else
                            {
                                bool run;
                                do
                                {
                                    string[] parts = [];
                                    for (int i = 0; i < arguments.Length; i++)
                                    {
                                        Argument arg = arguments[i];
                                        bool? istrue = ArgumentIsTrue(arg);
                                        parts = [.. parts, istrue.ToString()];
                                        if (arg.ArgAdd != Argument.ArgAdds.None) parts = [.. parts, arg.ArgAdd.ToString()];
                                    }
                                    run = EZHelp.Evaluate(string.Join(" ", parts));
                                    if (run)
                                    {
                                        RunStatement(statement, out bool broke);
                                        if (broke) break;
                                    }
                                } while (run);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"loop\", Error Message:\"{ex.Message}\"");
                        }
                        break;
                    case TokenType.Try:
                        try
                        {
                            Statement? statement = FirstToken.Value as Statement;
                            LastTryNotFailed = null;
                            RunStatement(statement, out _);
                        }
                        catch (Exception ex)
                        {
                            EZHelp.Error = null;
                            LastTryNotFailed = ex;
                        }
                        break;
                    case TokenType.Fail:
                        try
                        {
                            if (LastTryNotFailed == null) break;
                            Var exception = new Var("error", LastTryNotFailed.Message, CurrentLine, stackNumber: StackNumber, type: DataType.GetType("str", Classes));
                            Vars = [.. Vars, exception];
                            Statement? statement = FirstToken.Value as Statement;
                            LastTryNotFailed = null;
                            RunStatement(statement, out _);
                            Vars = Vars.Where(x => x.Name != "error").ToArray();
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with \"fail\", Error Message:\"{ex.Message}\"");
                        }
                        break;

                    case TokenType.True:
                    case TokenType.False:
                        try
                        {
                            if (line.Tokens.Length == 1)
                            {
                                Return = bool.Parse(FirstToken.Value.ToString());
                            }
                            else
                            {
                                line.Tokens[0].Type = TokenType.Identifier;
                                Return = SingleLine(line);
                            }
                        }
                        catch (Exception ex)
                        {
                            if (MessageHandled) throw;
                            MessageHandled = true;
                            throw new Exception($"Error with boolean, Error Message:\"{ex.Message}\"");
                        }
                        break;
                }
                if (!duplicate_stack)
                    StackTrace.TryPop(out _);
                return Return;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        private string[] GetIsParamParameters(string[] all_parts, Method method)
        {
            if (method.Parameters.Any(x => x.IsParams))
            {
                string[] new_parts = [];
                for (int j = 0; j < method.Parameters.Length; j++)
                {
                    if (method.Parameters[j].IsParams)
                    {
                        new_parts = [.. new_parts, string.Join(",", all_parts.Skip(j)).Replace(" ,", ",")];
                        break;
                    }
                    new_parts = [.. new_parts, all_parts[j]];
                }
                all_parts = new_parts;
            }
            return all_parts;
        }
        private object? RunStatement(Statement statement, out bool broke)
        {
            statement = new Statement(statement.Type, statement.Line, statement.InBrackets.Select(x => new LineWithTokens(x.Tokens.Select(y => new Token(y.Type, y.Value, y.StringValue)).ToArray(), x.Line)).ToArray());
            broke = false;
            object? result = null;
            foreach (LineWithTokens line in statement.InBrackets)
            {
                result = SingleLine(new LineWithTokens(line));

                if (hasexited) break;
                if (yielded)
                {
                    yielded = false;
                    broke = true;
                    break;
                }

                if (line.Tokens.Length == 0) 
                    continue;
                
                if (line.Tokens[0].Type == TokenType.Yield)
                {
                    yielded = true;
                    if (line.Tokens.Length > 1)
                    {
                        if (line.Tokens[1].Type == TokenType.Break)
                        {
                            broke = true;
                            break;
                        }
                        else throw new Exception("Expected the \"break\" keyword after yield");
                    }
                    else
                    {
                        throw new Exception("Expected token after yield. yield is a modifier and can not be by itself");
                    }
                }
                if (line.Tokens[0].Type == TokenType.Break)
                {
                    broke = true;
                    break;
                }
            }
            StackTrace.TryPop(out _);
            return result;
        }
        private bool? ArgumentIsTrue(Argument argument)
        {
            LineWithTokens line = new LineWithTokens(argument.Tokens, argument.Line);
            object? result = null;
            string exc = "";
            try 
            {
                result = SingleLine(line) ?? throw new Exception(); 
            }
            catch 
            {
                exc = EZHelp.Error;
                EZHelp.Error = null;
                string[] before = line.Tokens.Select(x => x.Value.ToString()).ToArray();
                result = GetValue(argument.Tokens, DataType.GetType("bool", Classes));
                if (before.SequenceEqual(result.ToString().Split(' ')))
                {
                    try { result = EZHelp.Expression(result.ToString()); } catch { }
                }
            }
            if (result is Class c)
                result = Argument.EvaluateTerm(c.Properties.FirstOrDefault(x => x.Name.ToLower() == "value").Value.ToString());
            if (result == null)
                result = argument.StringValue;
            bool? term = Argument.EvaluateTerm(result.ToString());
            if (term == null) throw new Exception($"Expected the argument section's method \"{argument.StringValue}\" to return boolean{(exc != "" ? $", Message: \"{exc}\"" : "")}");
            return term;
        }
        private enum IdentType { Var, Class, Method, Other }
        private IdentType IsType(string token, out object? type)
        {
            if (Vars.Any(x => x.Name == token))
            {
                type = Vars.FirstOrDefault(x => x.Name == token)!;
                return IdentType.Var;
            }
            if (Methods.Any(x => x.Name == token))
            {
                type = Methods.FirstOrDefault(x => x.Name == token)!;
                return IdentType.Method;
            }
            if (Classes.Any(x => x.Name == token))
            {
                type = Classes.FirstOrDefault(x => x.Name == token)!;
                return IdentType.Class;
            }
            else if (Classes.Any(x => x.Aliases.Any(y => y == token)))
            {
                type = Classes.FirstOrDefault(x => x.Aliases.Any(y => y == token));
                return IdentType.Class;
            }
            type = null;
            return IdentType.Other;
        }
        internal object? MethodRun(Method method, Var[]? parameters)
        {
            StackTrace.Push($"method: {method.Name}, file: {method.Line.FilePath}, line: {method.Line.CodeLine}");
            parameters ??= [];
            LineWithTokens[] lines = method.Lines;
            Var[] vvars = method.Parameters ?? [];

            if (method.Returns != null)
                method.Returns.ObjectClass ??= Classes.FirstOrDefault(x => x.Name == method.Returns?.StringType, method.Returns?.ObjectClass);

            for (int i = 0; i < vvars.Length; i++)
            {
                if (vvars[i].DataType != null)
                    vvars[i].DataType.ObjectClass ??= Classes.FirstOrDefault(x => x.Name == vvars[i].DataType?.StringType, vvars[i].DataType?.ObjectClass);

                if (!vvars[i].Required && parameters.Length - 1 < i)
                {
                    parameters = parameters.Append(new Var(vvars[i].Name, vvars[i].Value, vvars[i].Line, vvars[i].StackNumber, vvars[i].DataType, vvars[i].Required)).ToArray();
                    continue;
                }
                if (ParameterNotEqual(vvars[i], parameters[i]) && vvars[i].DataType != DataType.UnSet)
                {
                    throw new Exception($"Method parameters do not match. Error in method: \"{method.Name}\" parameter: \"{vvars[i].Name}\" inputted value: \"{parameters[i].Value}\"");
                }

                parameters[i].Name = vvars[i].Name;
            }
            Var[] overlap = [];
            Var? v;
            do
            {
                v = Vars.FirstOrDefault(x => parameters.Any(y => y.Name == x.Name), null);
                if (v != null)
                {
                    overlap = [.. overlap, v];
                    Vars = Vars.Where(x => x != v).ToArray();
                    break;
                }
            }
            while (v != null);
            Vars = [.. Vars, .. parameters];
            object? result = null;
            var _returning = Returning != null ? new DataType(Returning.Type, Returning.ObjectClass, Returning.StringType) : null;
            var _returned = returned;
            Returning = method.Returns;
            returned = null;
            foreach (LineWithTokens line in lines)
            {
                result = SingleLine(line);

                if (hasexited) break;
                if (line.Tokens.Length > 0 && line.Tokens[0].Type == TokenType.Return || returned != null)
                    break;
            }
            result ??= returned;
            Returning = _returning;
            returned = _returned;
            Vars = [.. Vars, .. overlap];
            Vars = Vars.Except(parameters).ToArray();

            StackTrace.TryPop(out _);
            return result;
        }
        internal bool ParameterNotEqual(Var v1, Var v2)
        {
            DataType.Types t1 = v1.DataType.Type;
            DataType.Types t2 = v2.DataType.Type;

            if (t1 == DataType.Types._float && t2 == DataType.Types._int)
                return false;
            if (t1 == DataType.Types._int && t2 == DataType.Types._uint)
                return false;
            if (t1 == DataType.Types._int && t2 == DataType.Types._long)
                return false;
            if (t1 == DataType.Types._float && t2 == DataType.Types._decimal)
                return false;
            if (t1 == DataType.Types._float && t2 == DataType.Types._double)
                return false;

            return t1 != t2;
        }
        internal int getValueLoopIndex = 0;
        internal object GetValue(object obj, DataType? type = null, string arraySeperator = " ")
        {
            if (obj.GetType().IsArray)
            {
                object[] a = (object[])obj;
                string all = "";
                if (a.Length > 0)
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        if (a[i] is null) continue;

                        if (a[i].GetType().IsArray)
                        {
                            return (a[i] as object[])[0];
                        }
                        all += GetValue(a[i], type, arraySeperator) + (i < a.Length - 1 ? arraySeperator : "");
                    }
                }
                return all;
            }
            else if (obj is Class)
            {
                if (type == null && Returning == null)
                    throw new Exception("Can not get value of class instance");
                Var var = new Var("Intermediate Var for getting value", obj as Class, CurrentLine, stackNumber: StackNumber, type: type ?? Returning);
                return GetValue(var, type, arraySeperator);
            }
            else if (obj is string || obj is Var)
            {
                Var? var;
                if (obj is Var) var = obj as Var;
                else var = Vars.FirstOrDefault(x => x.Name == obj.ToString());
                if (var != null)
                {
                    bool contains_gets = var.Value is Class c ? c.GetTypes != null && c.GetTypes.Length != 0 : false;
                    if (type != null && var.DataType != null)
                    {
                        if (var.DataType.Type == DataType.Types._null || var.DataType.ObjectClass == null)
                        {
                            return var.Value ?? "";
                        }
                        else if (var.DataType.ObjectClass == null && (var.DataType.Type == DataType.Types._null || var.DataType.Type == DataType.Types._object))
                        {
                            throw new Exception($"Error with DataType of class instance. Variable \"{var.Name}\" has an explicit data type that is not connected to a class");
                        }
                        else if (var.DataType.ObjectClass != null && !(var.DataType.Type == DataType.Types._null || var.DataType.Type == DataType.Types._object))
                        {
                            obj = var.Value;
                            int i = 0;
                            while (obj is Class)
                            {
                                i++; if (i > 50) break;
                                obj = (var.Value as Class).Properties.FirstOrDefault(x => x.Name == "Value", (var.Value as Class).Properties.FirstOrDefault(x => x.Name == "value", null)).Value;
                            }
                            try
                            {
                                if (obj == null) throw new Exception();
                                switch (var.DataType.Type)
                                {
                                    case DataType.Types._string: return obj.ToString();
                                    case DataType.Types._int: return int.Parse(obj.ToString());
                                    case DataType.Types._float: return float.Parse(obj.ToString());
                                    case DataType.Types._bool: return bool.Parse(obj.ToString());
                                    case DataType.Types._char: return char.Parse(obj.ToString());
                                    case DataType.Types._double: return double.Parse(obj.ToString());
                                    case DataType.Types._decimal: return decimal.Parse(obj.ToString());
                                    case DataType.Types._long: return long.Parse(obj.ToString());
                                    case DataType.Types._uint: return uint.Parse(obj.ToString());
                                    case DataType.Types._ulong: return ulong.Parse(obj.ToString());
                                    default: throw new Exception("Error finding datatype of class instance. This may be an interpreter error or the code for the class is faulty");
                                }
                            }
                            catch
                            {
                                if (getValueLoopIndex < 50)
                                {
                                    try
                                    {
                                        getValueLoopIndex++;
                                        return GetValue(var.Value, type, arraySeperator);
                                    }
                                    catch { }
                                }
                                else
                                {
                                    getValueLoopIndex = 0;
                                    return obj;
                                }
                                throw new Exception("Error returning correct value");
                            }
                        }
                        else if (var.Value is Class && var.DataType.ObjectClass != null && (var.DataType.Type == DataType.Types._object || contains_gets))
                        {
                            GetValueMethod[] get = (var.Value as Class).GetTypes;
                            if (get != null && get.Length != 0)
                            {
                                int t = -1;
                                for (int i = 0; i < get.Length; i++)
                                {
                                    if (get[i].DataType.ObjectClass != null && type.ObjectClass != null &&
                                        get[i].DataType.ObjectClass.Name == type.ObjectClass.Name || get[i].DataType.Type == type.Type)
                                        t = i;
                                }
                                if (t != -1)
                                {
                                    var backup_vars = Vars.Select(x => new Var(x.Name, x.Value, x.Line, x.StackNumber, x.DataType, x.Required)).ToArray();
                                    var backup_methods = Methods.Select(x => new Method(x.Name, x.Line, x.Settings, x.Lines, x.Parameters, x.Returns)).ToArray();
                                    Vars = (var.Value as Class).Properties.Concat(Vars.Where(x => x.Global)).ToArray();
                                    Methods = (var.Value as Class).Methods.Concat(Methods.Where(x => (x.Settings & Method.MethodSettings.Global) == Method.MethodSettings.Global)).ToArray();
                                    object? result = MethodRun(get[t].Method, null);
                                    Vars = backup_vars;
                                    Methods = backup_methods;
                                    if (result != null)
                                        return result;
                                    else throw new Exception($"The Class of the instance does not contain a \"get\" method for the expected datatype \"{type.Type.ToString().Remove(0, 1)}\"");
                                }
                            }
                            if (var.DataType.Type == DataType.Types._object && type.Type == DataType.Types._null)
                            {
                                obj = var.Value;
                                if (obj is Class _c)
                                {
                                    Var? v = _c.Properties.FirstOrDefault(x => x.Name.Equals("value", StringComparison.CurrentCultureIgnoreCase));
                                    obj = v != null ? v.Value : var.Value;
                                }
                                return obj;
                            }
                            else if (var.DataType == type || var.DataType.Type == type.Type)
                            {
                                if (var.Value is Class)
                                {
                                    if (getValueLoopIndex < 50)
                                    {
                                        try
                                        {
                                            getValueLoopIndex++;
                                            return GetValue(var.Value, type, arraySeperator);
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        getValueLoopIndex = 0;
                                        return obj;
                                    }
                                    throw new Exception("Error returning correct value");
                                }
                                return var.Value;
                            }
                            else
                            {
                                throw new Exception($"The Class of the instance does not contain a \"get\" method for the expected datatype \"{type.Type.ToString().Remove(0, 1)}\"");
                            }
                        }
                        else if (var.Value != null)
                        {
                            return GetValue(var.Value, var.DataType, arraySeperator) ?? obj;
                        }
                        else
                        {
                            throw new Exception("Error finding datatype of class instance. This may be an interpreter error or the code for the class is faulty");
                        }
                    }
                    else
                    {
                        return GetValue(var.Value, var.DataType, arraySeperator) ?? obj;
                    }
                }
                else if (obj.ToString().Contains(':'))
                {
                    string firstpart = obj.ToString().Split(':')[0].ToString().Trim();
                    if (Vars.Any(x => x.Name == firstpart))
                    {
                        string property_name = string.Join(":", obj.ToString().Split(':').Skip(1)).Trim();
                        if (Vars.FirstOrDefault(x => x.Name == firstpart).Value is Class c)
                        {
                            if (c.Properties.FirstOrDefault(x => x.Name == property_name) is Var v)
                            {
                                return v.Value;
                            }
                            else if (c.Methods.FirstOrDefault(x => x.Name == property_name) is Method m)
                            {
                                Methods = [.. c.Methods, .. Methods];
                                Vars = [ .. c.Properties, .. Vars];
                                Parser parser = new Parser(property_name, m.Line.FilePath);
                                Token[] parsertokens = parser.Parse()[0].Tokens;
                                LineWithTokens lineWithTokens = new LineWithTokens(parsertokens, CurrentLine);
                                obj = SingleLine(lineWithTokens);
                                Methods = Methods.Except(c.Methods).ToArray();
                                Vars = Vars.Except(c.Properties).ToArray();
                                return obj;
                            }
                            else
                            {
                                throw new Exception($"Could not find the property \"{property_name}\" in the class instance \"{firstpart}\"");
                            }
                        }
                        else if (Vars.FirstOrDefault(x => x.Name == firstpart).Value is RunMethod m)
                        {
                            if (m.Parameters.FirstOrDefault(x => x.Name == property_name) is Var v)
                            {
                                return v.Value;
                            }
                            else
                            {
                                throw new Exception($"Could not find the property \"{property_name}\" in the class instance \"{firstpart}\"");
                            }
                        }
                        else
                        {
                            throw new Exception($"Variable \"{firstpart}\" is not a class instance");
                        }
                    }
                    else if (Classes.FirstOrDefault(x => x.Name == firstpart) is Class c)
                    {
                        string property_name = string.Join(":", obj.ToString().Split(':').Skip(1)).Trim();
                        if (c.Properties.FirstOrDefault(x => x.Name == property_name) is Var v)
                        {
                            return v.Value;
                        }
                        if (c.Methods.FirstOrDefault(x => x.Name == property_name) is Method m)
                        {
                            if (m.Parameters.Select(x => x.Required).ToArray().Length != 0) 
                                throw new Exception($"Method \"{property_name}\" can not have any required parameters if being called like a property");
                            
                            return MethodRun(m, []);
                        }
                        else
                        {
                            throw new Exception($"Class path \"{obj}\" is not correct");
                        }
                    }
                }
                if (obj.ToString().Split(' ').Length > 0)
                {
                    string input = obj.ToString();
                    string[] parts = input.Split(' ').Select(x => x.Trim()).ToArray();
                    if (parts.Length > 1)
                    {
                        string first = parts.First();
                        string second = parts[1];
                        if (Vars.FirstOrDefault(x => x.Name == first) is Var v)
                        {
                            if (v.Value is Class c)
                            {
                                if (c.Properties != null && c.Properties.FirstOrDefault(x => x.Name == second) is Var s)
                                {
                                    if (parts.Length < 3)
                                    {
                                        obj = s.Value;
                                    }
                                } 
                                else if (c.Methods != null && c.Methods.FirstOrDefault(x => x.Name == second) is Method m)
                                {
                                    DoClass(c);
                                }
                                else if (c.Classes != null && c.Classes.FirstOrDefault(x => x.Name == second) is Class _c)
                                {
                                    DoClass(_c);
                                }
                            }
                            else if (v.Value is not null)
                            {
                                obj = v.Value;
                            }
                        }
                        else if (Methods.FirstOrDefault(x => x.Name == first) is Method m)
                        {
                            DoMethod(m, 0);
                        }
                        else if (Classes.FirstOrDefault(x => x.Name == first) is Class c)
                        {
                            DoClass(c, 0);
                        }
                        void DoMethod(Method m, int skip = 1)
                        {
                            Parser parser = new Parser(input, m.Line.FilePath);
                            Token[] parsertokens = parser.Parse()[0].Tokens;
                            LineWithTokens lineWithTokens = new LineWithTokens(parsertokens.Skip(skip).ToArray(), CurrentLine);
                            obj = SingleLine(lineWithTokens);
                        }
                        void DoClass(Class c, int skip = 1)
                        {
                            Methods = [ .. c.Methods, .. Methods];
                            Vars = [ .. c.Properties, .. Vars];
                            Parser parser = new Parser(input, c.Line.FilePath);
                            Token[] parsertokens = parser.Parse()[0].Tokens;
                            LineWithTokens lineWithTokens = new LineWithTokens(parsertokens.Skip(skip).ToArray(), CurrentLine);
                            obj = SingleLine(lineWithTokens);
                            Methods = Methods.Except(c.Methods).ToArray();
                            Vars = Vars.Except(c.Properties).ToArray();
                        }
                    }
                }
            }
            else if (obj is RunMethod run)
            {
                if (run.ClassName != "" && run.ClassName != null)
                {
                    Class cl = new Class(Classes.FirstOrDefault(x => x.Name == run.ClassName));
                    Method[] backupMethods = Methods;
                    Var[] backupVars = Vars;
                    Var[] backupRunParameters = run.Parameters.Select(x => new Var(x.Name, x.Value, x.Line, x.StackNumber, x.DataType, x.Required, x.Global, x.IsParams)).ToArray();
                    Vars = [ .. cl.Properties, .. Vars];
                    Methods = [ .. cl.Methods, .. Methods];

                    for (int i = 0; i < run.Parameters.Length; i++)
                    {
                        string[] parts = run.Parameters[i].Value.ToString().Split(" ");
                        for (int j = 0; j < parts.Length; j++)
                        {
                            parts[j] = GetValue(parts[j], DataType.GetType("str", Classes), arraySeperator).ToString();
                        }
                        run.Parameters[i].Value = string.Join(" ", parts);
                    }
                    Vars = Vars.Except(backupVars).ToArray();
                    Methods = Methods.Except(backupMethods).ToArray();
                    object o = MethodRun(run.Runs, run.Parameters);
                    cl.Properties = cl.Properties.Select(x => 
                    { 
                        if (x.Value == null && Vars.FirstOrDefault(y => y.Name == x.Name) is Var v) 
                            return new Var(x.Name, v.Value, x.Line, x.StackNumber, x.DataType, x.Required, x.Global, x.IsParams); 
                        else return x; 
                    }).ToArray();

                    try { o = GetValue(cl, type, arraySeperator); } catch { }

                    Methods = backupMethods;
                    Vars = backupVars;
                    run.Parameters = backupRunParameters;
                    return o;
                }
                else
                {
                    return MethodRun(run.Runs, run.Parameters);
                }
            }
            else if (obj is Token t)
            {
                return GetValue(t.Value, type, arraySeperator);
            }
            else if (obj is LineWithTokens lwt)
            {
                return GetValue(lwt.Tokens, type, arraySeperator);
            }
            return obj;
        }
        internal object Reflect(CSharpMethod method)
        {
            if (method.IsVar)
            {
                string value = Vars.FirstOrDefault(x => x.Name == method.Path).Value.ToString();
                Line[] l = [new Line(value, 0, method.Line.FilePath)];
                string[] r = { };
                object[] o = parser.SplitParts(ref l, 0, 0, ref r, out _, out _);
                if (o.Length > 1) throw new Exception("Error with reflection properties");
                method = o[0] as CSharpMethod;
            }

            object val = InvokeMethod(method.Path, method.Params != null ? method.Params.Select(x => x).ToArray() : [], EZHelp, out var assembly);
            LoadedAssemblies = LoadedAssemblies.Append(assembly).Where(x => x != null).Distinct().ToArray();

            return val;
        }
        public static object? InvokeMethod(string methodPath, object[] parameters, EZHelp e, out CustomAssemblyLoadContext? assemblyContext)
        {
            assemblyContext = null;
            // Split the method files into type and method name
            string[] pathParts = methodPath.Split('.');
            if (pathParts.Length < 2)
            {
                throw new ArgumentException("Invalid method path");
            }
            if (pathParts.Length >= 2 && pathParts[1].Equals("dll"))
            {
                // If the method path contains an assemblyContext name, load the assemblyContext
                string assemblyPath = pathParts[0] + "." + pathParts[1];
                string subdirectory = pathParts[0]; // Name of the subdirectory is first part of namespace
                string relativeAssemblyPath = Path.Combine(subdirectory, assemblyPath); // Combine subdirectory path with assemblyContext name
                string fullAssemblyPath = Path.GetFullPath(relativeAssemblyPath); // Get the absolute path of the assembly

                // Get Assemblies
                assemblyContext = new CustomAssemblyLoadContext();
                Assembly assembly = assemblyContext.LoadFromAssemblyPath(fullAssemblyPath);

                // Get the type name
                string typeName = string.Join(".", pathParts.Skip(2).Take(pathParts.Length - 3));
                
                // Get the method name
                string methodName = pathParts.Last();

                // Get the type from the assemblyContext
                Type type = assembly.GetType(typeName);
                if (type == null)
                {
                    throw new ArgumentException($"Type \"{typeName}\" not found in assembly \"{fullAssemblyPath}\"");
                }

                // Get the method from the type
                MethodInfo methodInfo = type.GetMethod(methodName);
                if (methodInfo == null)
                {
                    throw new ArgumentException($"Method \"{methodName}\" not found in type \"{typeName}\"");
                }

                // If the method is static, no need to create an instance
                if (methodInfo.IsStatic)
                {
                    // Invoke the method with the specified parameters
                    object result = methodInfo.Invoke(null, parameters);
                    return result;
                }
                else
                {
                    // Create an instance of the type
                    object instance = Activator.CreateInstance(type);

                    // Invoke the non-static method with the specified parameters
                    object result = methodInfo.Invoke(instance, parameters);
                    return result;
                }
            }
            else
            {
                // Get the type from the full type name
                string typeName = string.Join(".", pathParts.Take(pathParts.Length - 1));
                // Include the assemblyContext information for types in the System namespace
                if (typeName.StartsWith("System."))
                {
                    typeName += ", mscorlib";
                }
                Type type = Type.GetType(typeName);
                if (type == null)
                {
                    throw new ArgumentException($"Type \"{typeName}\" not found");
                }

                // Get the method name from the files
                string methodName = pathParts.Last();

                // Get the parameters as types
                Type[] parameterTypes = parameters.Select(p => p.GetType()).ToArray();

                // Find the method in the type
                MethodInfo methodInfo = type.GetMethod(methodName, parameterTypes);
                if (methodInfo == null)
                {
                    throw new ArgumentException($"Method \"{methodName}\" not found in type \"{typeName}\"");
                }

                // If the method is static, no need to create an instance
                if (methodInfo.IsStatic)
                {
                    // Invoke the method with the specified parameters
                    object result = methodInfo.Invoke(null, parameters);
                    return result;
                }
                else
                {
                    // Create an instance of the type
                    object instance = Activator.CreateInstance(type);
                    if (typeName == "EZCodeLanguage.EZHelp") instance = e;

                    // Invoke the non-static method with the specified parameters
                    try
                    {
                        object result = methodInfo.Invoke(instance, parameters);
                        return result;
                    }
                    catch
                    {
                        string? message = EZHelp.Error;
                        throw new Exception(message ?? $"Error occured in \"{methodPath}\"");
                    }
                }
            }
        }
        public class CustomAssemblyLoadContext: AssemblyLoadContext
        {
            public CustomAssemblyLoadContext() : base(isCollectible: true) { }
            protected override Assembly Load(AssemblyName assemblyName) => null;
        }
    }
}