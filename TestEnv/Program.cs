using EZCodeLanguage;
using System.Diagnostics;

string path = "Code.ezcode";
string full_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
string code = File.ReadAllText(path);
string file = Path.GetFullPath(path);

#if false

// print token tree


Parser parser = new Parser(code);

string save = Package.SaveCache(full_path, parser);

Package.SaveCache(Package.GetPackageFile("Main"), parser);

Parser save_test = Package.OpenCache(full_path, Package.GetPackageFile("Main"));

Console.WriteLine("Serialization/Deserialization Successful\n");
Console.WriteLine(save);
#else
// run ezcode

Stopwatch stopwatch = Stopwatch.StartNew();
Parser parser = new Parser();
Parser.LineWithTokens[] tokens = parser.Tokenize(code);
List<Parser.Token[]> tokenTypes = [];
for (int i = 0; i < tokens.Length; i++)
{
    tokenTypes.Add(tokens[i].Tokens);
}
string tokenString = "";
for (int i = 0; i < tokenTypes.Count; i++)
{
    for (int j = 0; j < tokenTypes[i].Length; j++)
    {
        if (tokenTypes[i][j].Type == Parser.TokenType.Identifier)
            tokenString += $"'{tokenTypes[i][j].Value}' ";
        else
            tokenString += tokenTypes[i][j].Type.ToString() + " ";
    }
    tokenString += "\n";    
}

string ch = "⁚";
bool overHundred = code.Split("\n").Length > 100;
code = string.Join("\n", code.Split("\n").Select((x, y) => x = $"{(y + 1 < 10 ? (overHundred ? "  " : " ") : overHundred && y + 1 < 100 ? " " : "")}{y + 1} {ch}  {x}").Select(x => x.Replace("\t", "    ").Replace("    ", $"  {ch} ")));
int[] num = []; for (int i = 0; i < code.Split("\n").Length; i++) num = [.. num, code.Split("\n")[i].Length + 5];
string len = new string('-', num.Max());
code = "File:\t" + file + "\n" + len + "\n" + code;
Console.OutputEncoding = System.Text.Encoding.UTF8;
stopwatch.Stop();
long Omili = stopwatch.ElapsedMilliseconds;
Console.WriteLine(code + "\n" + len
    // tokenString + "\n" + len + "\n"
    );

stopwatch.Restart();

Interpreter interpreter = new Interpreter(file, parser);
interpreter.Interperate();


stopwatch.Stop();
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine(len + "\n" + "Tokenize Miliseconds:" + Omili.ToString() + "\nInterperate Miliseconds:" + mili.ToString() + "\nOverall Miliseconds:" + (Omili + mili).ToString());
#endif