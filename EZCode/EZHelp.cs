using static EZCodeLanguage.Parser;
using static EZCodeLanguage.Interpreter;
using System.Data;

namespace EZCodeLanguage
{
    public class EZHelp
    {
        public Interpreter Interpreter { get; set; }
        public static string? Error = null;
        public EZHelp(Interpreter interpreter)
        {
            Interpreter = interpreter;
        }
        public EZHelp() { }

        // Main Package:
        //     include main
        public void Print(string text)
        {
            text = Format(text.ToString());
            Interpreter.Output = [.. Interpreter.Output, text.ToString()];
            Console.WriteLine(text);
        }
        public void Clear()
        {
            Console.Clear();
            Interpreter.Output = [];
        }
        public string Input()
        {
            string input = Interpreter.ConsoleInput();
            return input;
        }
        public string Format(object _text) => Format(_text, "'");
        public string Format(object _text, object _char)
        {
            try
            {
                char c = char.Parse(_char.ToString());
                string text = ObjectParse(_text.ToString(), "str", true).ToString();
                string format = "";
                char[] chars = text.ToCharArray();
                bool open = true, backslash = false;
                Range[] ranges = [];
                int current = 0;
                for (int i = 0, count = 0; i < chars.Length; i++, count++)
                {
                    if (backslash)
                    {
                        int minus = 0;
                        backslash = false;
                        chars[i] =
                            chars[i] == '\\' ? '\\' :
                            chars[i] == 'n' ? '\n' :
                            chars[i] == 'r' ? '\r' :
                            chars[i] == 'c' ? ',' :
                            chars[i] == 'p' ? '.' :
                            chars[i] == '"' ? '\'' :
                            chars[i] == '\'' ? '"' :
                            chars[i] == '_' ? ' ' :
                            chars[i] == 'L' ? '{' :
                            chars[i] == 'R' ? '}' :
                            chars[i] == 'q' ? '?' :
                            chars[i] == 'a' ? '@' :
                            chars[i] == ';' ? ':' :
                            chars[i] == 's' ? ';' :
                            chars[i];
                        if (chars[i] == '!')
                        {
                            chars = chars.ToList().Where((x, y) => y != i).ToArray();
                            minus++;
                        }
                        count--;
                        i--;
                        chars = chars.ToList().Where((x, y) => y != i).ToArray();
                        i -= minus;

                        if (minus == 0)
                            format += chars[i];
                        continue;
                    }
                    else if (chars[i] == '\\')
                    {
                        backslash = true;
                        continue;
                    }
                    if (chars[i] == c)
                    {
                        chars = chars.ToList().Where((x, y) => y != i).ToArray();
                        open = !open;
                        count--;
                        if (open)
                        {
                            ranges[current].Count = count;
                            current++;
                        }
                        else
                        {
                            ranges = [.. ranges, new Range()];
                            ranges[current].Start = i;
                            count = 0;
                        }
                        i--;
                        continue;
                    }
                    format += chars[i];
                }
                for (int i = ranges.Length - 1; i >= 0; i--)
                {
                    Range range = ranges[i];
                    string name = format.Substring(range.Start, range.Count),
                        instance_name = name;
                    if (name.Contains(':'))
                    {
                        instance_name = name.Split(':')[0].Trim();
                    }
                    if (Interpreter.Vars.Any(x => x.Name == instance_name))
                    {
                        format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(name, DataType.GetType("str", Interpreter.Classes)).ToString());
                    }
                    else
                    {
                        Interpreter.parser.WatchIsFound([name], 0, out ExplicitWatch watch, out _);

                        object val = Interpreter.GetValue(watch != null ? watch.Runs : name, DataType.GetType("str", Interpreter.Classes));
                        format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(val, new DataType(DataType.Types._string, null, "str")).ToString());
                    }
                }

                return format;
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        struct Range(int start, int end)
        {
            public int Start = start;
            public int Count = end;
        }
        public string StringEmpty()
        {
            return string.Empty;
        }
        public object ObjectParse(object obj, object type) => ObjectParse(obj, type, false);
        public object ObjectParse(object obj, object type, bool to_string, string arraySeperator = " ", bool returnNull = false)
        {
            try
            {
                if (obj.ToString().StartsWith("{") && obj.ToString().EndsWith("}"))
                {
                    obj = obj.ToString().Substring(1, obj.ToString().Length - 2).Trim();
                }
                try
                {
                    object o = obj;
                    do
                    {
                        string n = obj.ToString();
                        o = obj;
                        DataType data = DataType.GetType(type.ToString(), Interpreter.Classes);
                        if (Interpreter.Vars.Any(x => x.Name == n)) Interpreter.Vars.FirstOrDefault(x => x.Name == n).DataType = data;
                        obj = Interpreter.GetValue(n, data, arraySeperator);
                    } while (obj != o);
                }
                catch when (returnNull) { return null; }
                if (!to_string)
                {
                    if (int.TryParse(obj.ToString(), out int i)) return i;
                    if (float.TryParse(obj.ToString(), out float f)) return f;
                    try { obj = Operate(obj.ToString(), false); return obj; } catch { }
                    try { obj = Evaluate(obj.ToString()); return obj; } catch { }
                }
                return obj;
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public bool Evaluate(string expression)
        {
            try
            {
                expression = expression.Replace("||", "or").Replace("&&", "and").Replace("&", "and").Replace("|", "or").Replace("!=", "<>");
                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(bool));

                DataRow row = table.NewRow();
                row["expression"] = DBNull.Value;
                table.Rows.Add(row);

                table.Columns["expression"].Expression = expression;

                return (bool)row["expression"];
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public float Operate(string expression) => Operate(expression, true);
        public float Operate(string expression, bool object_parse)
        {
            try
            {

                string[] parts = SplitWithDelimiters(object_parse ? (ObjectParse(expression, "str").ToString()) : expression, ['-', '+', '=', '*', '/', '%', '&', '|', '!', ' ']).Where(x => x != "" && x != " ").ToArray();
                expression = "";
                foreach (string e in parts)
                {
                    expression += object_parse ? (ObjectParse(e, "float").ToString() + " ") : e + " ";
                }
                DataTable table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return float.Parse((string)row["expression"]);
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public bool Expression(string expression)
        {
            char[] symbols = ['-', '+', '=', '*', '/', '%', '&', '|', '!', ' '];
            string[] parts = SplitWithDelimiters(ObjectParse(expression, "str", true).ToString(), symbols).Where(x => x != "" && x != " ").ToArray();
            bool allIsText = false;
            for (int i = 0; i < parts.Length; i++)
            {
                string e = parts[i];
                if ((new[] { "=", ">", "<", "=", "!=", "!", ">=", "<=" }).Any(x => x == e))
                {
                    if (parts[i] == "!" && parts.Length > i + 1 && parts[i + 1] == "=")
                    {
                        parts[i] = "!=";
                        parts = parts.ToList().Where((item, index) => index != i + 1).ToArray();
                    }

                    if (!allIsText && !(float.TryParse(parts[i + 1], out _) || bool.TryParse(parts[i + 1], out _) || symbols.Any(x => x.ToString() == parts[i + 1])))
                    {
                        parts[i - 1] = $"'{StringParse(parts[i - 1])}'";
                    }
                    continue;
                }
                if ((new[] { "&", "|", "and", "or", "&&", "||" }).Any(x => x == e))
                {
                    allIsText = false;
                    continue;
                }
                bool isText = !(float.TryParse(e, out _) || bool.TryParse(e, out _) || symbols.Any(x => x.ToString() == e)) || allIsText;
                if (isText) allIsText = true;
                e = !isText ? StringParse(e) : $"'{StringParse(e)}'";
                parts[i] = e;
            }

            expression = string.Join(" ", parts);
            return Evaluate(expression.Trim());
        }
        public string StringExpression(string expression)
        {
            string[] parts = ObjectParse(expression, "str").ToString().Split(['+']).Where(x => x != "" && x != " ").Select(e => ObjectParse(e.Trim(), "str").ToString()).ToArray();
            return string.Join("", parts);
        }
        public object? StringMod(object x, object y, object mod)
        {
            try
            {
                string m = mod.ToString(),
                    a = ObjectParse(x.ToString(), "str", true).ToString(),
                    b = ObjectParse(y.ToString(), "str", true).ToString();
                switch (m)
                {
                    case "+": return a + b;
                    case "-":
                        if (int.TryParse(b, out int res))
                        {
                            a = string.Join("", a.ToCharArray().Take(a.Length - res));
                        }
                        else
                        {
                            a = a.Replace(b, "");
                        }
                        return a;
                    case "*":
                        if (int.TryParse(b, out res))
                        {
                            string _a = a;
                            for (int i = 0; i < res; i++)
                            {
                                _a += a;
                            }
                            a = _a;
                        }
                        else
                        {
                            throw new Exception("Expcted number to multiply by");
                        }
                        return a;
                    case "/":
                        if (int.TryParse(b, out res))
                        {
                            int len = a.Length;
                            int newl = len / res;
                            a = string.Join("", a.ToCharArray().Take(a.Length - newl));
                        }
                        else
                        {
                            throw new Exception("Expcted number to divide by");
                        }
                        return a;
                    default: return null;
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public object Add(object x, object y)
        {
            try
            {
                float a = float.Parse(ObjectParse(x.ToString(), "float").ToString());
                float b = float.Parse(ObjectParse(y.ToString(), "float").ToString());
                return a + b;
            }
            catch
            {
                string a = ObjectParse(x.ToString(), "str").ToString();
                string b = ObjectParse(y.ToString(), "str").ToString();
                return a + b;
            }
        }
        public object Subtract(object x, object y)
        {
            try
            {
                float a = float.Parse(ObjectParse(x.ToString(), "float").ToString());
                float b = float.Parse(ObjectParse(y.ToString(), "float").ToString());
                return a - b;
            }
            catch
            {
                string a = ObjectParse(x.ToString(), "str").ToString();
                string b = ObjectParse(y.ToString(), "str").ToString();
                if (int.TryParse(b, out int res))
                {
                    a = string.Join("", a.ToCharArray().Take(a.Length - res));
                }
                else
                {
                    a = a.Replace(b, "");
                }
                return a;
            }
        }
        public object Multiply(object x, object y)
        {
            try
            {
                float a = float.Parse(ObjectParse(x.ToString(), "float").ToString());
                float b = float.Parse(ObjectParse(y.ToString(), "float").ToString());
                return a * b;
            }
            catch
            {
                string a = ObjectParse(x.ToString(), "str").ToString();
                string b = ObjectParse(y.ToString(), "str").ToString();
                if (int.TryParse(b, out int res))
                {
                    string _a = a;
                    for (int i = 0; i < res; i++)
                    {
                        _a += a;
                    }
                    a = _a;
                }
                else
                {
                    throw new Exception("Expcted number to multiply by");
                }
                return a;
            }
        }
        public object Divide(object x, object y)
        {
            try
            {
                float a = float.Parse(ObjectParse(x.ToString(), "float").ToString());
                float b = float.Parse(ObjectParse(y.ToString(), "float").ToString());
                return a / b;
            }
            catch
            {
                string a = ObjectParse(x.ToString(), "str").ToString();
                string b = ObjectParse(y.ToString(), "str").ToString();
                if (int.TryParse(b, out int res))
                {
                    int len = a.Length;
                    int newl = len / res;
                    a = string.Join("", a.ToCharArray().Take(a.Length - newl));
                }
                else
                {
                    throw new Exception("Expcted number to divide by");
                }
                return a;
            }
        }
        public bool Compare(object v1, object v2, object v3)
        {
            try
            {
                object[] values = [v1, v2, v3];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = ObjectParse(values[i], "str");
                }

                string all = string.Join(" ", values.Select(x => x.ToString()));
                try
                {
                    return Expression(all);
                }
                catch
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public bool Compare(object v1, object v2)
        {
            return Compare(v1, v2, "");
        }
        public bool Compare(object v1)
        {
            return Compare(v1, "", "");
        }
        public string StringParse(object v) => ObjectParse(v, "str", true).ToString();
        public string CharParse(object v) => ObjectParse(v, "char", true).ToString();
        public bool BoolParse(object v) => bool.Parse(ObjectParse(v, "bool").ToString());
        public float FloatParse(object v) => float.Parse(ObjectParse(v, "float").ToString());
        public int IntParse(object v) => int.Parse(ObjectParse(v, "int").ToString());
        public bool SameType(object a, object b)
        {
            if (a is Var va && b is Var vb)
            {
                if (va.Value is Class ca && vb.Value is Class cb)
                {
                    return ca.Name == cb.Name;
                }
                else
                {
                    return va.Value.GetType() == vb.Value.GetType();
                }
            }
            else
            {
                return a.GetType() == b.GetType();
            }
        }
        public int StringLength(object str) => StringParse(str).Length;
        public void RunEZCode(string code)
        {
            try
            {
                code = ObjectParse(code, "str").ToString();
                Parser parser = new Parser(string.Join(" ", code), "(instance running from inside file)");
                parser.Parse();
                Interpreter interpreter = new Interpreter(parser);
                interpreter.Interperate();
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public bool IsType(object obj, object type)
        {
            try
            {
                type = StringParse(type);
                if (obj.ToString().StartsWith("{") && obj.ToString().EndsWith("}"))
                {
                    obj = obj.ToString().Substring(1, obj.ToString().Length - 2).Trim();
                    if (Interpreter.Vars.FirstOrDefault(x => x.Name == obj.ToString()) is Var var) obj = var;
                    else
                    {
                        throw new Exception($"Could not find variable \"{obj}\"");
                    }
                    if (!var.Value.ToString().StartsWith('#'))
                    {
                        throw new Exception($"For IsType method, use \"#varName\" with # sign");
                    }
                    else
                    {
                        if (var.Value is Class c)
                        {
                            c.Properties.First(x => x.Name.ToString() == "value").Value = var.Value.ToString().Remove(0, 1);
                        }
                        else
                        {
                            var.Value = var.Value.ToString().Remove(0, 1);
                        }
                    }
                    if (Interpreter.Vars.FirstOrDefault(x => x.Name == var.Value.ToString()) is Var v2) obj = v2;
                    else
                    {
                        throw new Exception($"Could not find variable \"{var.Value}\"");
                    }
                }
                if (obj is Var v)
                {
                    if (v.Value is Class c)
                    {
                        return c.Name == type.ToString();
                    }
                    else if (v.DataType.ObjectClass is Class cl)
                    {
                        return cl.Name == type.ToString();
                    }
                    else
                    {
                        throw new Exception($"Variable \"{v.Name}\" is not defined");
                    }
                }
                else if (obj is Class c)
                {
                    return c.Name == type.ToString();
                }
                else
                {
                    throw new Exception($"Object \"{obj}\" is not a variable");
                }
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public object ArrayParse(object array, object separator) => ObjectParse(array, "list").ToString().Split(StringParse(separator)).Select(x => x.Trim()).ToArray();
        public string ArrayStringParse(object array) => ObjectParse(array, "list", to_string: false, arraySeperator: ", ").ToString();
        public int ArrayLength(object array)
        {
            try
            {
                string sep = "|\\@@@@@__~>=>=//\\@@@@@@######{}}{sd\\___gpgdfpsg14702580690, ";
                return ObjectParse(array, "list", to_string: false, arraySeperator: sep).ToString().Split(sep).Length;
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public object ArrayAppend(object array, object appends, object separator)
        {
            try
            {
                string sep = "|\\@@@@@__~>=>=//\\:@@@@@@#:#####{}}{sd\\___gpgdfpsg14702580690, ";
                var tempA = ObjectParse(array, "list", to_string: false, arraySeperator: sep, returnNull: true);
                object[] a = (tempA != null) ? tempA.ToString().Split(sep) : [];
                object[] b = (object[])ArrayParse(appends, separator);
                return a.Concat(b).ToArray();
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public object ArrayIndex(object array, object index)
        {
            try
            {
                string sep = "|\\@@@@@__~>=>=//\\:@@@@@@#:#####{}}{sd\\___gpgdfpsg14702580690, ";
                return ObjectParse(array, "list", to_string: false, arraySeperator: sep).ToString().Split(sep)[int.Parse(StringParse(index))];
            }
            catch (Exception e)
            {
                Error = e.Message;
                throw;
            }
        }
        public float MathFunc(object obj, object op)
        {
            try
            {
                obj = ObjectParse(obj, "str");
                bool obj2_is_null = !obj.ToString().Contains(",");
                object obj1 = obj;
                if (!obj2_is_null) obj1 = obj.ToString().Split(",")[0];
                object? obj2 = null;
                obj1 = ObjectParse(obj1, "float");
                if (!obj2_is_null) obj2 = ObjectParse(obj.ToString().Split(",")[1], "float");
                op = StringParse(op);

                if (!float.TryParse(obj1.ToString(), out float val1))
                {
                    throw new Exception("Could not parse 'obj' to type 'float'");
                }
                if (!float.TryParse(!obj2_is_null ? obj2.ToString() : "", out float val2))
                {
                    if (!obj2_is_null)
                    {
                        throw new Exception("Could not parse 'obj' to type 'float'");
                    }
                }
                string operation = string.Join("", op.ToString().ToLower().ToCharArray().Select((x, y) => { if (y == 0) return char.Parse(x.ToString().ToUpper()); else return x; }));
                object[] parameters = [val1];
                if (!obj2_is_null) parameters = [.. parameters, val2];

                return float.Parse(InvokeMethod($"System.MathF.{operation}", parameters, this, out _).ToString());
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                throw;
            }
        }
        public float Average(object obj)
        {
            object[] array = ArrayParse(obj, ",") as object[];
            float[] numbers = array.Select((x, y) => float.Parse(ObjectParse(array[y], "float").ToString())).ToArray();
            float all = numbers.Sum();
            return all / numbers.Length;
        }
        public float Pi() => MathF.PI;
        public float E() => MathF.E;
        public float Clamp(object _val, object _min, object _max)
        {
            float val = FloatParse(_val);
            float min = FloatParse(_min);
            float max = FloatParse(_max);

            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }
        public float Power(object _val, object _pow)
        {
            float val = FloatParse(_val);
            float pow = FloatParse(_pow);
            return MathF.Pow(val, pow);
        }
        public int RandomNumber(object _min, object _max)
        {
            Random random = new Random();
            int min = IntParse(_min);
            int max = IntParse(_max);
            return random.Next(min, max);
        }
        public string Trim(object text) => StringParse(text).Trim();
        public string ToLower(object text) => StringParse(text).ToLower();
        public string ToUpper(object text) => StringParse(text).ToUpper();
        public string Replace(object text, object older, object newwer) => StringParse(text).Replace(StringParse(older), StringParse(newwer));
        public string Substring(object text, object index, object length) => StringParse(text).Substring(IntParse(index), IntParse(length));
        public bool StartsWith(object text, object val) => StringParse(text).StartsWith(StringParse(val));
        public bool EndsWith(object text, object val) => StringParse(text).EndsWith(StringParse(val));
        public bool RegexMatch(object _text, object _match)
        {
            string input = StringParse(_text);
            string oattern = StringParse(_match);
            var match = System.Text.RegularExpressions.Regex.Match(input, oattern);
            return match.Success;
        }
        public static string FixDirPath(string path) => path.Replace("/", "\\").Replace(" : \\", ":\\");
        public static string FixUrlPath(string path)
        {
            string replacedPath = path.Replace("\\", "/").Replace(" : ", ":");
            return !(replacedPath.StartsWith("https://") || replacedPath.StartsWith("http://")) ? ("https://" + replacedPath) : replacedPath;
        }
        public string FileRead(object _path)
        {
            try
            {
                string path = StringParse(_path);
                path = FixDirPath(path);
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
                throw ThrowError(e);
            }
        }
        public void FileWrite(object _path, object _text)
        {
            try
            {
                string path = StringParse(_path);
                string text = StringParse(_text);
                path = FixDirPath(path);
                File.WriteAllText(path, text);
            }
            catch (Exception e)
            {
                throw ThrowError(e);
            }
        }
        public void FileCreate(object _path)
        {
            try
            {
                string path = StringParse(_path);
                path = FixDirPath(path);
                File.Create(path).Close();
            }
            catch (Exception e)
            {
                throw ThrowError(e);
            }
        }
        public void FileDelete(object _path)
        {
            try
            {
                string path = StringParse(_path);
                path = FixDirPath(path);
                File.Delete(path);
            }
            catch (Exception e)
            {
                throw ThrowError(e);
            }
        }
        public void Exit() => Interpreter.hasexited = true;
        public object? EnvironmentOS(object _property, object? _val1, object? _val2)
        {
            try
            {
                string? property = Try(() => StringParse(_property).ToLower(), () => throw ThrowError(new Exception("Error occured while getting property name from parameter")))?.ToString();
                string? val1 = _val1 != null ? Try(() => StringParse(_val1))?.ToString() : null;
                string? val2 = _val2 != null ? Try(() => StringParse(_val2))?.ToString() : null;
                switch (property)
                {
                    case "newline": return Environment.NewLine;
                    case "osversion": return Environment.OSVersion;
                    case "processpath": return Environment.ProcessPath;
                    case "is64bitprocess": return Environment.Is64BitProcess;
                    case "processid": return Environment.ProcessId;
                    case "commandline": return Environment.CommandLine;
                    case "currentdirectory": return Environment.CurrentDirectory;
                    case "currentmanagedthreadid": return Environment.CurrentManagedThreadId;
                    case "exitcode": return Environment.ExitCode;
                    case "hasshutdownstarted": return Environment.HasShutdownStarted;
                    case "is64bitoperatingsystem": return Environment.Is64BitOperatingSystem;
                    case "isprivilegedprocess": return Environment.IsPrivilegedProcess;
                    case "machinename": return Environment.MachineName;
                    case "processorcount": return Environment.ProcessorCount;
                    case "stacktrace": return Environment.StackTrace;
                    case "systemdirectory": return Environment.SystemDirectory;
                    case "systempagesize": return Environment.SystemPageSize;
                    case "tickcount": return Environment.TickCount;
                    case "tickcount64": return Environment.TickCount64;
                    case "userdomainname": return Environment.UserDomainName;
                    case "userinteractive": return Environment.UserInteractive;
                    case "username": return Environment.UserName;
                    case "version": return Environment.Version;
                    case "workingset": return Environment.WorkingSet;
                    case "getcommandlineargs": return Environment.GetCommandLineArgs();
                    case "getenvironmentvariables": return Environment.GetEnvironmentVariables();
                    case "getlogicaldrives": return Environment.GetLogicalDrives();
                }
                if (val1 != null)
                {
                    Enum.TryParse(val1, out Environment.SpecialFolder fval1);
                    int.TryParse(val1, out int ival1);
                    switch (property)
                    {
                        case "exit": Environment.Exit(ival1); return null;
                        case "expandenvironmentvariables": return Environment.ExpandEnvironmentVariables(val1);
                        case "failfast": Environment.FailFast(val1); return null;
                        case "getenvironmentvariable": return Environment.GetEnvironmentVariable(val1);
                        case "getfolderpath": return Environment.GetFolderPath(fval1);
                    }
                    if (val2 != null)
                    {
                        switch (property)
                        {
                            case "equals": return Equals(val1, val2);
                            case "referenceequals": return ReferenceEquals(val1, val2);
                            case "setenvironmentvariable": Environment.SetEnvironmentVariable(val1, val2); return null;
                        }
                    }
                }

                throw new Exception($"The environment property '{property}' does not exist");
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }

        // EZHelp static functions:
        public static T GetParameter<T>(object obj, Type type) => GetParameter<T>(obj, type.Name);
        public static T GetParameter<T>(object obj, string type)
        {
            EZHelp e = new EZHelp(Instance);

            return (T)e.ObjectParse(obj, type);
        }
        public static Exception ThrowError(Exception exception)
        {
            Error = exception.Message;
            throw exception;
        }
        public static object? Try(Func<object> function)
        {
            try
            {
                return function();
            }
            catch
            {
                return null;
            }
        }
        public static object? Try(Func<object> function, Func<object> handle)
        {
            try
            {
                return function();
            }
            catch
            {
                return handle();
            }
        }

        // Time Package:
        //     include time
        public string DateTimeNow(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.Now.ToString(format);
                }
                else
                {
                    return DateTime.Now.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public DateTime DateTimeExtract(object _t1)
        {

            try
            {
                if (_t1.ToString().StartsWith("{") && _t1.ToString().EndsWith("}"))
                    _t1 = _t1.ToString().Substring(1, _t1.ToString().Length - 2).Trim();

                Var? vart1 = Interpreter?.Vars?.FirstOrDefault(x => x.Name == _t1.ToString());
                Class? classt1 = (Class)Try(() => vart1?.Value as Class);
                DateTime t1;

                if (classt1 is not null)
                {
                    t1 = new DateTime(
                        year: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "year").Value.ToString()),
                        month: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "month").Value.ToString()),
                        day: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "day").Value.ToString()),
                        hour: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "hour").Value.ToString()),
                        minute: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "minute").Value.ToString()),
                        second: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "second").Value.ToString()),
                        millisecond: int.Parse(classt1.Properties.FirstOrDefault(x => x.Name == "milisecond").Value.ToString())
                        );
                }
                else
                {
                    if (vart1.Value is Var)
                    {
                        var interVar = (Var)vart1.Value;
                        var contains = Interpreter.Vars.Any(x => x.Name == interVar.Name);
                        if (!contains)
                            Interpreter.Vars = [.. Interpreter.Vars, interVar];

                        var result = DateTimeExtract(interVar.Name);

                        if (!contains)
                            Interpreter.Vars = Interpreter.Vars.Where(x => x.Name != interVar.Name).ToArray();

                        return result;
                    }

                    t1 = DateTime.Parse(vart1.Value.ToString());
                }
                return t1;
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public int DateTimeCompare(object _t1, object _t2)
        {
            try
            {

                DateTime
                    t1 = DateTimeExtract(_t1),
                    t2 = DateTimeExtract(_t2);

                return DateTime.Compare(t1, t2);
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public object DateTimeParses(object _val)
        {
            try
            {
                string val = StringParse(_val);
                return DateTime.Parse(val);
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public int DateTimeTakeYear(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Year;
        }
        public int DateTimeTakeMonth(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Month;
        }
        public int DateTimeTakeDay(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Day;
        }
        public int DateTimeTakeHour(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Hour;
        }
        public int DateTimeTakeMinute(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Minute;
        }
        public int DateTimeTakeSecond(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Second;
        }
        public int DateTimeTakeMilisecond(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.Millisecond;
        }
        public string DateTimeUtcNow(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.UtcNow.ToString(format);
                }
                else
                {
                    return DateTime.UtcNow.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public string DateTimeToday(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.Today.ToString(format);
                }
                else
                {
                    return DateTime.Today.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public string DateTimeMaxValue(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.MaxValue.ToString(format);
                }
                else
                {
                    return DateTime.MaxValue.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public string DateTimeMinValue(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.MinValue.ToString(format);
                }
                else
                {
                    return DateTime.MinValue.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public string DateTimeUnixEpoch(object? _format)
        {
            try
            {
                string? format = Try(() => StringParse(_format))?.ToString();
                if (format is not null)
                {
                    return DateTime.UnixEpoch.ToString(format);
                }
                else
                {
                    return DateTime.UnixEpoch.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public int DateTimeDaysInMonth(object _year, object _month)
        {
            try
            {
                int year = IntParse(_year);
                int month = IntParse(_month);

                return DateTime.DaysInMonth(year, month);
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public string DateTimeFormat(object _format, object _time)
        {
            try
            {
                string format = StringParse(_format);
                DateTime dateTime = DateTimeExtract(_time);

                return dateTime.ToString(format);
            }
            catch (Exception ex)
            {
                throw ThrowError(ex);
            }
        }
        public bool DateTimeIsLeepYear(object _time)
        {
            try
            {
                DateTime dateTime = DateTimeExtract(_time);
                return DateTime.IsLeapYear(dateTime.Year);
            }
            catch
            {
                int year = IntParse(_time);
                return DateTime.IsLeapYear(year);
            }
        }
        public bool DateTimeIsDaylightSavingsTime(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.IsDaylightSavingTime();
        }
        public int DateTimeDayOfYear(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.DayOfYear;
        }
        public string DateTimeDayOfWeek(object _time)
        {
            DateTime dateTime = DateTimeExtract(_time);
            return dateTime.DayOfWeek.ToString();
        }
    }
}
