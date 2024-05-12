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
                        format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(val, new DataType(DataType.Types._string, null)).ToString());
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
                catch when(returnNull) { return null; }
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
        public static object GetParameter(object obj, string type)
        {
            EZHelp e = new EZHelp(Instance);

            return e.ObjectParse(obj, type);
        }
        public static Exception SetError(Exception exception)
        {
            Error = exception.Message;
            throw exception;
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
        public string ArrayStringParse(object array) => ObjectParse(array, "list", to_string:false, arraySeperator:", ").ToString();
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
                if (!obj2_is_null) parameters = [..parameters, val2];

                return float.Parse(InvokeMethod($"System.MathF.{operation}", parameters, this).ToString());
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
    }
}
