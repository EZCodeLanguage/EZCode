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
        public string Format(object _text)
        {
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
                    backslash = false;
                    chars[i] = 
                        chars[i] == '\\' ? '\\' :
                        chars[i] == 'n' ? '\n' :
                        chars[i] == 'c' ? ',' :
                        chars[i] == 'p' ? '.' :
                        chars[i] == '"' ? '\'' :
                        chars[i] == '\'' ? '"' :
                        chars[i];
                    if (chars[i] == '!')
                    {
                        chars = chars.ToList().Where((x, y) => y != i).ToArray();
                    }
                    count--;
                    i--;
                    chars = chars.ToList().Where((x, y) => y != i).ToArray();

                    format += chars[i];
                    continue;
                }
                else if (chars[i] == '\\')
                {
                    backslash = true;
                    continue;
                }
                if (chars[i] == '\'')
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
                string name = format.Substring(range.Start, range.Count);
                if (Interpreter.Vars.Any(x => x.Name == name))
                {
                    format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.GetValue(name, DataType.GetType("str", Interpreter.Classes, Interpreter.Containers)).ToString());
                }
                else
                {
                    if (Interpreter.parser.WatchIsFound([name], 0, out ExplicitWatch watch, out _))
                    {
                        object val = Interpreter.MethodRun(watch.Runs.Runs, watch.Runs.Parameters);
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
                if (int.TryParse(obj.ToString(), out int i)) obj = i;
                if (float.TryParse(obj.ToString(), out float f)) obj = f;
            }
            return obj;
        }
        public bool Evaluate(string expression)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("expression", typeof(bool), expression);
            System.Data.DataRow row = table.NewRow();
            table.Rows.Add(row);
            return (bool)row["expression"];
        }
        public float Operate(string expression)
        {
            string[] parts = SplitWithDelimiters(ObjectParse(expression, "str").ToString(), ['-', '+', '=', '*', '/', '%', '&', '|', '!', ' ']).Where(x => x != "" && x != " ").ToArray();
            expression = "";
            foreach (string e in parts)
            {
                expression += ObjectParse(e, "float").ToString() + " ";
            }
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return float.Parse((string)row["expression"]);
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
            string all = string.Join(" ", values.Select(x => x.ToString()), true);
            return Evaluate(all);
        }
    }
}
