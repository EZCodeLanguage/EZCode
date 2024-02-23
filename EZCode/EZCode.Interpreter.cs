using System.Reflection;
using static EZCodeLanguage.Tokenizer;
using static System.Net.Mime.MediaTypeNames;

namespace EZCodeLanguage
{
    public class Interpreter
    {
        public Tokenizer tokenizer { get; set; }
        public string WorkingFile { get; set; }
        public EZHelp EZHelp { get; private set; }
        public Interpreter(string file, Tokenizer tokenizer)
        {
            WorkingFile = file;
            this.tokenizer = tokenizer;
            EZHelp = new EZHelp(this);
            Methods = tokenizer.Methods.ToArray();

            for (int i = 0; i < Classes.Length; i++)
            {
                if (Classes[i].Properties == null) continue;
                for (int j = 0; j < Classes[i].Properties.Length; j++)
                {
                    Var var = Classes[i].Properties[j];
                    if (var.DataType.ObjectClass == null)
                    {
                        var.DataType = DataType.GetType(var.Line.Value.Split(" ")[0], Classes, Containers);
                        if (var.DataType.ObjectClass == null && var.DataType.ObjectContainer == null)
                        {
                            var.DataType.ObjectClass = Classes[i];
                        }
                    }
                }
            }
        }
        public string[] Output { get; internal set; } = [];
        public Exception[] Errors { get; private set; } = [];
        public Var[] Vars { get; set; } = [];
        public Method[] Methods { get; set; } = [];
        public Class[] Classes { get => tokenizer.Classes.ToArray(); }
        public Container[] Containers { get => tokenizer.Containers.ToArray(); }
        public int Interperate() => Interperate(tokenizer.Tokens);
        public int Interperate(LineWithTokens[] LineTokens)
        {
            int endcode = 0;

            foreach (LineWithTokens line in LineTokens)
            {
                try
                {
                    SingleLine(line);
                }
                catch (Exception ex)
                {
                    string message = ex.Message + ". Line: " + line.Line.CodeLine;
                    if (ex is EZException)
                    {
                        EZException ez = ex as EZException;
                        message = "\"" + ez.StackTrace + "\" ~> " + message;
                        message += " (" + ez.Id + ")";
                    }
                    Console.WriteLine(message);
                    Errors = Errors.Append(ex).ToArray();
                }
            }

            return endcode;
        }
        private object? SingleLine(LineWithTokens line)
        {
            string deb = "";
            foreach (var t in line.Tokens)
            {
                if (t.Value is RunMethod r)
                {
                    deb += r.Runs.Name;
                }
                else if (t.Value is Var v)
                {
                    deb += v.Name;
                }
                else if (t.Value is ExplicitWatch e)
                {
                    deb += e.Runs.ClassName;
                }
                else if (t.Value is Class)
                {
                    continue;
                }
                else if (t.Value is Method)
                {
                    continue;
                }
                else
                {
                    deb += t.Value.ToString();
                }
                deb += " ";
            }
            Console.WriteLine(line.Line.CodeLine + " -> " + line.Line.Value + "   ->   " + deb);
            Token FirstToken = line.Tokens.First();
            switch (FirstToken.Type)
            {
                case TokenType.Identifier:
                    try
                    {
                        if (Methods.Any(x => x.Name == FirstToken.Value.ToString()))
                        {
                            Method method = Methods.FirstOrDefault(x => x.Name == FirstToken.Value.ToString());
                            Method.MethodSettings settings = method.Settings;
                            bool nocol = (settings & Method.MethodSettings.NoCol) != 0;
                            int next = nocol ? 0 : 1;
                            if (!nocol && line.Tokens[1].Type != TokenType.Colon)
                            {
                                throw new Exception("Expected ':' identifier to set method perameters");
                            }
                            Var[] vars = [];

                            if (method.Params != null && method.Params.Length > 0)
                            {
                                string all = string.Join(" ", line.Tokens.Skip(next + 1).Select(x => GetValue(x)));
                                string[] vals = all.Split(',').Select(x => x.Trim()).ToArray();
                                if (vals.Length > 0)
                                {
                                    for (int i = 0; i < method.Params.Length; i++)
                                    {
                                        vars = [.. vars, new Var(method.Params[i].Name, vals[i], line.Line, method.Params[i].DataType)];
                                    }
                                    if (vars.Length != method.Params.Select(x => x.Required).ToArray().Length)
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

                            return MethodRun(method, vars);
                        }
                        else if (Classes.Any(x => x.Name == FirstToken.Value.ToString()))
                        {
                            Class cl = new Class(Classes.FirstOrDefault(x => x.Name == FirstToken.Value.ToString()));
                            if (line.Tokens[2].Type == TokenType.New)
                            {
                                string name = line.Tokens[1].Value.ToString();
                                Var var = new Var(name, cl, line.Line, new DataType(cl.TypeOf != null ? cl.TypeOf.Type : DataType.Types._object, cl));
                                Vars = [.. Vars, var];

                                if (line.Tokens.Length > 3)
                                {
                                    if (line.Tokens[3].Type != TokenType.Colon)
                                        throw new Exception("Expected colon to set instance properties");
                                    ExplicitParams? p = cl.Params;
                                    if (p != null)
                                    {
                                        if (p.IsOverride)
                                        {
                                            RunMethod run = p.Runs;
                                            if (tokenizer.ParamIsFound(line.Tokens.Select(x => x.Value).ToArray(), 4, out _) == false)
                                                throw new Exception($"The format pattern of the class, '{p.Pattern}' does not match this class instance");
                                            Method[] backupMethods = Methods;
                                            Var[] backupVars = Vars;
                                            Vars = cl.Properties;
                                            Methods = cl.Methods;
                                            MethodRun(run.Runs, run.Parameters);
                                            Methods = backupMethods;
                                            Vars = backupVars;
                                        }
                                        else if (line.Tokens[4].Type == TokenType.Match)
                                        {
                                            RunMethod run = line.Tokens[4].Value as RunMethod;
                                            Method[] backupMethods = Methods;
                                            Var[] backupVars = Vars;

                                            int i = 0;
                                            while (run.ClassName != FirstToken.Value.ToString())
                                            {
                                                i++; if (i > 50) throw new Exception($"Infinite loop error in with declaring instance of class '{cl.Name}'. Unkown cause, might be Interperater error or faulty code");
                                                Class cc = Classes.FirstOrDefault(x => x.Name == run.ClassName);
                                                if (cc == null) throw new Exception("Error with finding class from name");
                                                Vars = cc.Properties;
                                                Methods = cc.Methods;
                                                object result = GetValue(run);
                                                Methods = backupMethods;
                                                Vars = backupVars;
                                                bool f = tokenizer.ParamIsFound([result is Var v ? v.Value : result], 0, out var param);
                                                if (f) 
                                                {
                                                    run = p.Runs;
                                                    break;
                                                }
                                                else
                                                {
                                                    throw new Exception($"The format pattern of the class, '{p.Pattern}' does not match this class instance");
                                                }
                                            } 
                                            Vars = cl.Properties;
                                            Methods = cl.Methods;
                                            MethodRun(run.Runs, run.Parameters);
                                            Methods = backupMethods;
                                            Vars = backupVars;
                                        }
                                        else
                                        {
                                            throw new Exception($"The format pattern of the class, '{p.Pattern}' does not match this class instance");
                                        }
                                    }
                                    else
                                    {
                                        string all = string.Join(" ", line.Tokens.Skip(4).Select(x => x.Value));
                                        string[] vals = all.Split(',').Select(x => x.Trim()).ToArray();
                                        Var[] vars = [];
                                        if (vals.Length > 0)
                                        {
                                            for (int i = 0; i < vals.Length; i++)
                                            {
                                                string[] whole = vals[i].Split(":");
                                                if (whole.Length < 2) throw new Exception("Expected properties to be set by syntax 'PropertyName:Value'"); 
                                                string before = whole[0];
                                                string after = whole[1];
                                                if (cl.Properties.Any(x => x.Name == before))
                                                {
                                                    Var v = cl.Properties.FirstOrDefault(x => x.Name == before);
                                                    after = GetValue(after).ToString();
                                                    vars = [.. vars, new Var(v.Name, after, line.Line, v.DataType)];
                                                }
                                                else
                                                {
                                                    throw new Exception($"There is no '{before}' property in the '{cl.Name}' class");
                                                }
                                            }
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                throw new Exception("Expected 'new' keyword to declare instance of class");
                            }
                        }
                        else if (Vars.Any(x => x.Name == FirstToken.Value.ToString()))
                        {
                            Var var = Vars.FirstOrDefault(x => x.Name == FirstToken.Value.ToString());
                            if (line.Tokens.Length == 1) return var.Value;
                            else if (line.Tokens[1].Type == TokenType.Arrow)
                            {
                                line.Tokens = line.Tokens.Skip(2).ToArray();
                                object value = SingleLine(line);
                                var.Value = value;
                            }
                            else
                            {
                                if (var.Value is Class || (var.DataType.ObjectClass != null && var.DataType.Type != DataType.Types.NotSet))
                                {
                                    Class cl = var.Value is Class ? var.Value as Class : var.DataType.ObjectClass;
                                    if (cl.Methods.Any(x => x.Name == line.Tokens[1].Value.ToString()))
                                    {
                                        Method[] backupMethods = Methods;
                                        Methods = cl.Methods;
                                        Var[] backupVars = Vars;
                                        Vars = cl.Properties;
                                        line.Tokens = line.Tokens.Skip(1).ToArray();
                                        object value = SingleLine(line);
                                        Methods = backupMethods;
                                        Vars = backupVars;

                                        var.Value = value;
                                    }
                                    else
                                    {
                                        throw new Exception($"Variable is trying to call a method '{line.Tokens[1].Value}' that doesn't exists");
                                    }
                                }
                                else
                                {
                                    throw new Exception($"Variable type is undefined and can not call the method '{line.Tokens[1].Value}'");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("Unexpected identifier '" + FirstToken.Value.ToString() + "' Expects methods, object delcaration, or existing variables");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new EZException($"Error with '{line.Line.Value}', Error Message:'{ex.Message}'", "ex21", $"{WorkingFile} line:{line.Line.CodeLine}", ex);
                    }
                    break;
                case TokenType.RunExec:
                    try
                    {
                        CSharpMethod method = FirstToken.Value as CSharpMethod;
                        return Reflect(method);
                    }
                    catch (Exception ex)
                    {
                        throw new EZException($"Error with 'runexec' and with C# method. The reason may be because the method may not exist. C# Error Message:'{ex.Message}'", "ex01", $"{WorkingFile} line:{line.Line.CodeLine}", ex);
                    }
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
                                throw new Exception("Expected arrow identifier '=>' to set value");
                            }
                        }
                        Vars = [.. Vars, new Var(tokenName.Value.ToString(), value, line.Line, null)];
                    }
                    catch (Exception ex)
                    {
                        throw new EZException($"Error with 'undefined', Error Message:'{ex.Message}'", "ex03", $"{WorkingFile} line:{line.Line.CodeLine}", ex);
                    }
                    break;
                case TokenType.Return:
                    try
                    {
                        line.Tokens = line.Tokens.Skip(1).ToArray();
                        return SingleLine(line);
                    }
                    catch (Exception ex)
                    {
                        throw new EZException($"Error with 'return', Error Message:'{ex.Message}'", "ex04", $"{WorkingFile} line:{line.Line.CodeLine}", ex);
                    }
                case TokenType.If:
                    break;
                case TokenType.Else:
                    break;
                case TokenType.Loop:
                    break;
                case TokenType.Try:
                    break;
                case TokenType.Fail:
                    break;
            }
            return null;
        }
        public object? MethodRun(Method method, Var[]? parameters)
        {
            Console.WriteLine("-- " + method.Name + (method.Returns != null ? (" => " + (method.Returns.ObjectClass != null ? method.Returns.ObjectClass.Name : method.Returns.Type)) : ""));
            LineWithTokens[] lines = method.Lines;
            parameters ??= [];
            Var[] vvars = method.Params ?? [];
            for (int i = 0; i < vvars.Length; i++)
            {
                if (vvars[i].DataType != parameters[i].DataType && vvars[i].DataType != DataType.UnSet)
                {
                    throw new Exception($"Method parameters do not match. Error in method: '{method.Name}' parameter: '{vvars[i].Name}' inputted value: '{parameters[i].Value}'");
                }
            }
            Vars = [.. Vars, .. parameters];
            object? result = null;
            foreach (LineWithTokens line in lines)
            {
                result = SingleLine(new LineWithTokens(line));

                if (line.Tokens[0].Type == TokenType.Return)
                    break;
            }
            Vars = Vars.Except(parameters).ToArray();

            return result;
        }
        public object GetValue(object obj, DataType? type = null)
        {
            if (obj is string)
            {
                Var? var = GetVar(obj.ToString()!);
                if (var != null)
                {
                    if (type != null && var.DataType != null)
                    {
                        if (var.DataType.ObjectClass == null && (var.DataType.Type == DataType.Types.NotSet || var.DataType.Type == DataType.Types._object))
                        {
                            throw new Exception($"Error with DataType of class instance. variable '{var.Name}' has an explicit data type that is not connected to a class");
                        }
                        else if (var.DataType.ObjectClass != null && var.DataType.Type == DataType.Types._object)
                        {
                            GetValueMethod[] get = var.DataType.ObjectClass.GetTypes;
                            if (get != null && get.Length != 0)
                            {
                                int t = -1;
                                for (int i = 0; i < get.Length; i++)
                                {
                                    if (get[i].DataType == type) t = i;
                                }
                                if (t != -1)
                                {
                                    object? result = MethodRun(get[t].Method, null);
                                    if (result != null)
                                        return result;
                                    else throw new Exception("The 'get' method for this class instance does not return a value");
                                }
                                else
                                {
                                    if (var.DataType == type)
                                    {
                                        return var.Value;
                                    }
                                    else
                                    {
                                        throw new Exception($"The Class of the instance does not contain a 'get' method for the expected datatype '{type.Type.ToString().Remove(0, 1)}'");
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception($"The Class of the instance does not contain a 'get' method for the expected datatype '{type.Type.ToString().Remove(0, 1)}'");
                            }
                        }
                        else if (var.DataType.ObjectClass != null && !(var.DataType.Type == DataType.Types.NotSet || var.DataType.Type == DataType.Types._object))
                        {
                            obj = var.Value;
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
                        else
                        {
                            throw new Exception("Error finding datatype of class instance. This may be an interpreter error or the code for the class is faulty");
                        }
                    }
                    else
                    {
                        return var.Value ?? obj;
                    }
                }
            }
            else if (obj is RunMethod run)
            {
                if (run.ClassName != "" && run.ClassName != null)
                {
                    Class cl = Classes.FirstOrDefault(x => x.Name == run.ClassName);
                    Method[] backupMethods = Methods;
                    Var[] backupVars = Vars;

                    Vars = cl.Properties;
                    Methods = cl.Methods;
                    object o = MethodRun(run.Runs, run.Parameters);
                    Methods = backupMethods;
                    Vars = backupVars;
                    return o;
                }
                else
                {
                    return MethodRun(run.Runs, run.Parameters);
                }
            }
            else if (obj is Token t)
            {
                return GetValue(t.Value);
            }
            return obj;
        }
        public object Reflect(CSharpMethod method)
        {
            Console.WriteLine("--  " + method.Path);
            if (method.IsVar)
            {
                string value = GetVar(method.Path).Value.ToString();
                Line[] l = [new Line(value, 0)];
                object[] o = tokenizer.SplitParts(ref l, 0, out _);
                if (o.Length > 1) throw new Exception();
                method = o[0] as CSharpMethod;
            }

            return InvokeMethod(method.Path, method.Params.Select(x => x).ToArray(), EZHelp);
        }
        public Var? GetVar(string name)
        {
            return Vars.FirstOrDefault(x => x.Name == name, null);
        }
        public static object? InvokeMethod(string methodPath, object[] parameters, EZHelp e)
        {
            // Split the method path into type and method name
            string[] pathParts = methodPath.Split('.');
            if (pathParts.Length < 2)
            {
                throw new ArgumentException("Invalid method path");
            }

            // Get the type from the full type name
            string typeName = string.Join(".", pathParts.Take(pathParts.Length - 1));
            // Include the assembly information for types in the System namespace
            if (typeName.StartsWith("System."))
            {
                typeName += ", mscorlib";
            }
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"Type '{typeName}' not found");
            }

            // Get the method name from the path
            string methodName = pathParts.Last();

            // Find the method in the type
            MethodInfo methodInfo = type.GetMethod(methodName, parameters.Select(p => p.GetType()).ToArray());
            if (methodInfo == null)
            {
                throw new ArgumentException($"Method '{methodName}' not found in type '{typeName}'");
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
                object result = methodInfo.Invoke(instance, parameters);
                return result;
            }
        }
    }
}
