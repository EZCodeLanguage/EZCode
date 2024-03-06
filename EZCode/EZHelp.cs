using static EZCodeLanguage.Tokenizer;
using static EZCodeLanguage.Interpreter;

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
        public void Print(string text, string ch)
        {
            text = Format(text.ToString(), ch);
            Interpreter.Output = [.. Interpreter.Output, text.ToString()];
            Console.WriteLine(text);
        }
        public string Format(object _text, object _f)
        {
            char f = char.Parse(_f.ToString());
            string text = _text.ToString();
            if(text.StartsWith("{")  && text.EndsWith("}"))
            {
                DataType type = DataType.GetType("@str", Interpreter.Classes, Interpreter.Containers);
                Interpreter.Vars.FirstOrDefault(x => x.Name == text.Substring(1, text.Length - 2).Trim()).DataType = type;
                text = Interpreter.GetValue(text.Substring(1, text.Length - 2).Trim(), type).ToString()!;
            }
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
                if (chars[i] == f)
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
                    if (Interpreter.tokenizer.WatchIsFound([name], 0, out ExplicitWatch watch, out _))
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
        public object ObjectParse(object obj, object type)
        {
            if (obj.ToString().StartsWith("{") && obj.ToString().EndsWith("}"))
            {
                DataType data = DataType.GetType(type.ToString(), Interpreter.Classes, Interpreter.Containers);
                Interpreter.Vars.FirstOrDefault(x => x.Name == obj.ToString().Substring(1, obj.ToString().Length - 2).Trim()).DataType = data;
                obj = Interpreter.GetValue(obj.ToString().Substring(1, obj.ToString().Length - 2).Trim(), data); 
            }
            if (int.TryParse(obj.ToString(), out int i)) obj = i;
            if (float.TryParse(obj.ToString(), out float f)) obj = f;
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
        public object Add(object x, object y)
        {
            if (x.ToString().StartsWith("{") && x.ToString().EndsWith("}"))
                x = Interpreter.GetValue(x.ToString().Substring(1, x.ToString().Length - 2).Trim(), new DataType(DataType.Types._float, null));
            if (y.ToString().StartsWith("{") && y.ToString().EndsWith("}"))
                y = Interpreter.GetValue(y.ToString().Substring(1, y.ToString().Length - 2).Trim(), new DataType(DataType.Types._float, null));
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
        public bool Compare(object v1, object v2, object v3)
        {
            object[] values = [v1, v2, v3];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].ToString().StartsWith("{") && values[i].ToString().EndsWith("}"))
                {
                    DataType data = DataType.GetType("str", Interpreter.Classes, Interpreter.Containers);
                    Interpreter.Vars.FirstOrDefault(x => x.Name == values[i].ToString().Substring(1, values[i].ToString().Length - 2).Trim()).DataType = data;
                    values[i] = Interpreter.GetValue(values[i].ToString().Substring(1, values[i].ToString().Length - 2).Trim(), data);
                }
            }
            string all = string.Join(" ", values.Select(x => x.ToString()));
            return Evaluate(all);
        }
    }
}
