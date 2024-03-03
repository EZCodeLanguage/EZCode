using EZCodeLanguage;
using System.Diagnostics;
using Newtonsoft.Json; 

string path = "Code.ezcode";
string code = File.ReadAllText(path);
string file = Path.GetFullPath(path);

#if false

// print token tree

Tokenizer tokenizer = new Tokenizer(code);
tokenizer.Tokenize();

string save = JsonConvert.SerializeObject(tokenizer, Formatting.Indented);

File.WriteAllText("Code.eztokens", save);

Tokenizer save_test = JsonConvert.DeserializeObject<Tokenizer>(save);
Console.WriteLine("Serialization/Deserialization Successful\n");
Console.WriteLine(save);
#else
// run ezcode

Stopwatch stopwatch = Stopwatch.StartNew();
Tokenizer tokenizer = new Tokenizer();
Tokenizer.LineWithTokens[] tokens = tokenizer.Tokenize(code);
List<Tokenizer.Token[]> tokenTypes = [];
for (int i = 0; i < tokens.Length; i++)
{
    tokenTypes.Add(tokens[i].Tokens);
}
string tokenString = "";
for (int i = 0; i < tokenTypes.Count; i++)
{
    for (int j = 0; j < tokenTypes[i].Length; j++)
    {
        if (tokenTypes[i][j].Type == Tokenizer.TokenType.Identifier)
            tokenString += $"'{tokenTypes[i][j].Value}' ";
        else
            tokenString += tokenTypes[i][j].Type.ToString() + " ";
    }
    tokenString += "\n";    
}

string ch = "⁚";
code = string.Join("\n", code.Split("\n").Select((x, y) => x = $"{(y + 1 < 10 ? "0" : "")}{y + 1} {ch}  {x}").Select(x => x.Replace("\t", "    ").Replace("    ", $"  {ch} ")));
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

Interpreter interpreter = new Interpreter(file, tokenizer);
interpreter.Interperate();


stopwatch.Stop();
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine(len + "\n" + "Tokenize Miliseconds:" + Omili.ToString() + "\nInterperate Miliseconds:" + mili.ToString() + "\nOverall Miliseconds:" + (Omili + mili).ToString());
#endif