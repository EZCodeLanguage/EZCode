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
        public void Print(string text)
        {
            text = Format(text.ToString());
            Interpreter.Output = [.. Interpreter.Output, text.ToString()];
            Console.WriteLine(text);
        }
        public string Format(string text)
        {
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
                if (backslash)
                {
                    backslash = false;
                    chars[i] = 
                        chars[i] == '\\' ? '\\' :
                        chars[i] == 'n' ? '\n' :
                        chars[i] == 'c' ? ',' :
                        chars[i];
                    if (chars[i] == '!')
                    {
                        chars = chars.ToList().Where((x, y) => y != i).ToArray();
                    }
                    count--;
                    i--;
                    chars = chars.ToList().Where((x, y) => y != i).ToArray();
                }
                else if (chars[i] == '\\')
                {
                    backslash = true;
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
                    format = format.Remove(range.Start, range.Count).Insert(range.Start, Interpreter.Vars.FirstOrDefault(x => x.Name == name).Value.ToString());
                }
                else
                {
                    if (Interpreter.tokenizer.WatchIsFound([name], 0, out ExplicitWatch watch))
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
        public string StringParse(object str)
        {
            return Format(str.ToString()!);
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
        public float Add(object x, object y)
        {
            float a = float.Parse(Format(x.ToString()));
            float b = float.Parse(Format(y.ToString()));
            return a + b;
        }
    }
}
