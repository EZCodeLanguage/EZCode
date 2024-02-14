using EZCodeLanguage;
using System.Diagnostics;

Stopwatch stopwatch = Stopwatch.StartNew();
    //semi class str {
    //    explicit watch ""(.*? {text})"" => set : text
    //    explicit watch .*'(.*? {val})'.* => set : val
    //    explicit typeof => ezcodelanguage.ezhelp.datatype("string")
    //    explicit params (text) => set : text
    //    var value \!
    //    get => @int {
    //        return ezcodelanguage.ezhelp.string.parse(value)
    //    }
    //    get => @bool {
    //        return ezcodelanguage.ezhelp.bool.parse(value)
    //    }
    //    get => @float {
    //        return ezcodelanguage.ezhelp.float.parse(value)
    //    }
    //    method set : text {
    //        value => format : text
    //    }
    //    nocol method + : @str:text {
    //        value = ezcodelanguage.ezhelp.addstring(value, text)
    //    }
    //    static method format : text => @str {
    //        return ezcodelanguage.ezhelp.formatstring(text)
    //    }
    //}
    //static ontop class program {
    //    method print : @str:text {
    //        ezcodelanguage.ezhelp.print(text)
    //    }
    //}
    //// declare a variable from the class 'str' and set it to 'hello world'
    //str text new : hello world
    //text + ""text""
    //// print 'text' (hello world) to the console
    //print 'text'
EZCode ezcode = new EZCode();
string code = """
    class str {
        explicit watch ""(.*? {text})"" => set : text
    }
    print ""this is some text""
    """;
EZCode.LineWithTokens[] tokens = ezcode.Tokenize(code);
List<EZCode.TokenType[]> tokenTypes = [];
for (int i = 0; i < tokens.Length; i++)
{
    tokenTypes.Add(tokens[i].Tokens.Select(x => x.Type).ToArray());
}
string tokenString = "";
for (int i = 0; i < tokenTypes.Count; i++)
{
    for (int j = 0; j < tokenTypes[i].Length; j++)
    {
        tokenString += tokenTypes[i][j].ToString() + " ";
    }
    tokenString += "\n";
}

stopwatch.Stop();
long mili = stopwatch.ElapsedMilliseconds;
Console.WriteLine("Miliseconds:" + mili.ToString() + "\n--------------------\n\n\n--------------------\n" +
    code + "\n--------------------\n" +
    tokenString
    );