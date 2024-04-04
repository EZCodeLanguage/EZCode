using static EZCodeLanguage.Parser;
using static EZCodeLanguage.Interpreter;
using System.Data;

namespace EZCodeLanguage
{
    public class EZHelp
    {
        public Interpreter Interpreter { get; set; }
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
        public string Format(object _text) => Format(_text, "'");
        public string Format(object _text, object _char)
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
                    format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(name, DataType.GetType("str", Interpreter.Classes, Interpreter.Containers)).ToString());
                }
                else
                {
                    if (Interpreter.parser.WatchIsFound([name], 0, out ExplicitWatch watch, out _))
                    {
                        object val = Interpreter.GetValue(watch.Runs, DataType.GetType("str", Interpreter.Classes, Interpreter.Containers));
                        format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(val, new DataType(DataType.Types._string, null)).ToString());
                    }
                }
            }

            return format;
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
        public object ObjectParse(object obj, object type, bool to_string)
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
                    DataType data = DataType.GetType(type.ToString(), Interpreter.Classes, Interpreter.Containers);
                    if (Interpreter.Vars.Any(x => x.Name == n)) Interpreter.Vars.FirstOrDefault(x => x.Name == n).DataType = data;
                    obj = Interpreter.GetValue(n, data);
                } while (obj != o);
            }
            catch { }
            if (!to_string)
            {
                if (int.TryParse(obj.ToString(), out int i)) return i;
                if (float.TryParse(obj.ToString(), out float f)) return f;
                try { obj = Operate(obj.ToString(), false); return obj; } catch { }
                try { obj = Evaluate(obj.ToString()); return obj; } catch { }
            }
            return obj;
        }
        public bool Evaluate(string expression)
        {
            expression = expression.Replace("||", "or").Replace("&&", "and").Replace("&", "and").Replace("|", "or");
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(bool));

            DataRow row = table.NewRow();
            row["expression"] = DBNull.Value;
            table.Rows.Add(row);

            table.Columns["expression"].Expression = expression;

            return (bool)row["expression"];
        }
        public float Operate(string expression) => Operate(expression, true);
        public float Operate(string expression, bool object_parse)
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
        public bool Expression(string expression)
        {
            string[] parts = SplitWithDelimiters(ObjectParse(expression, "str").ToString(), ['-', '+', '=', '*', '/', '%', '&', '|', '!', ' ']).Where(x => x != "" && x != " ").ToArray();
            expression = "";
            foreach (string e in parts)
            {
                expression += ObjectParse(e, "str").ToString() + " ";
            }
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
            } catch { return null; }
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
            object[] values = [v1, v2, v3];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = ObjectParse(values[i], "str");
            }
            string all = string.Join(" ", values.Select(x => x.ToString()));
            return Evaluate(all);
        }
        public bool Compare(object v1, object v2)
        {
            return Compare(v1, v2, "");
        }
        public bool Compare(object v1)
        {
            return Compare(v1, "", "");
        }
        public string StringParse(object v) => ObjectParse(v, "str").ToString();
        public bool BoolParse(object v) => bool.Parse(ObjectParse(v, "bool").ToString());
        public float FloatParse(object v) => float.Parse(ObjectParse(v, "float").ToString());
        public int IntParse(object v) => int.Parse(ObjectParse(v, "int").ToString());
        public int RunEZCodeWithPackage(string code, string package)
        {
            code = ObjectParse(code, "str").ToString() + Environment.NewLine;
            package = ObjectParse(package, "str").ToString();
            Parser parser = new Parser(string.Join(" ", code));
            parser = Package.ReturnParserWithPackages(parser, [package]);
            parser.Parse();
            Interpreter interpreter = new Interpreter($"{Interpreter.WorkingFile}(instance running from inside file)", parser);
            return interpreter.Interperate();
        }
        public int RunEZCode(string code)
        {
            code = ObjectParse(code, "str").ToString();
            Parser parser = new Parser(string.Join(" ", code));
            parser.Parse();
            Interpreter interpreter = new Interpreter($"{Interpreter.WorkingFile}(instance running from inside file)", parser);
            return interpreter.Interperate();
        }
    }
}
