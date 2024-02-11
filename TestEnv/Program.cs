using EZCodeLanguage;

EZCode ezcode = new EZCode();

string code = """
    if ! false : var a => input keys
    """;

string[] tokens = ezcode.Tokenize(code).Select(x => x.Type.ToString()).ToArray();

Console.WriteLine(code + "\n--------------------\n--------------------\n" + 
    string.Join(", ", tokens)
    .Replace(", NewLine, ", "NewLine")
    .Replace("NewLine", "\n")
    .Replace(", \n", "\n")
    .Replace("\n, ", "\n"));