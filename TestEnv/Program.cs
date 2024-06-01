using EZCodeLanguage;
using System.Diagnostics;

/*
// set up files variables 
string path = "Code.ezcode";
string full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
string code = File.ReadAllText(path);
string file = Path.GetFullPath(path);

// start stopwatch
Stopwatch stopwatch = Stopwatch.StartNew();

// parse
Parser parser = new Parser(code, file);
parser.Parse();

// breakpoints
EZCodeLanguage.Debug.Breakpoint[] breakpoints = [
    //new EZCodeLanguage.Debug.Breakpoint(parser.LinesWithTokens[2].Line)
    ];

// print code and file
string ch = "⁚";
bool overHundred = code.Split("\n").Length > 100;
code = string.Join("\n", code.Split("\n").Select((x, y) => x = $"{(y + 1 < 10 ? (overHundred ? "  " : " ") : overHundred && y + 1 < 100 ? " " : "")}{y + 1} {ch}  {x}").Select(x => x.Replace("\t", "    ").Replace("    ", $"  {ch} ")));
int[] num = []; for (int i = 0; i < code.Split("\n").Length; i++) num = [.. num, code.Split("\n")[i].Length + 5];
string len = new string('-', num.Max());
code = "File:\t" + file + "\n" + len + "\n" + code;
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine(code + "\n" + len);

// set up stop watch
stopwatch.Stop();
long Omili = stopwatch.ElapsedMilliseconds;
stopwatch.Restart();

// start interpretation
Interpreter interpreter = new Interpreter(parser, breakpoints);
interpreter.Interperate();

//print time
stopwatch.Stop();
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine(len + "\n" + "Parser Miliseconds:" + Omili.ToString() + "\nInterperate Miliseconds:" + mili.ToString() + "\nOverall Miliseconds:" + (Omili + mili).ToString());
*/

// or
EZCode.RunProject("FirstProject.json", AppDomain.CurrentDomain.BaseDirectory);