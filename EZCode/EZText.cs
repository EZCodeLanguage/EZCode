using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EZCode
{
    public class EZText
    {
        /// <summary>
        /// EZCode object EZR Code is referencing
        /// </summary>
        public EzCode ezCode { get; set; }
        /// <summary>
        /// Easier Code Given
        /// </summary>
        public string ezText { get; set; } = "";
        /// <summary>
        /// Translated EZCode
        /// </summary>
        public string Code { get; set; } = "";
        /// <summary>
        /// Current line of EZText
        /// </summary>
        public int CodeLine { get; set; } = 0;
        /// <summary>
        /// Errors in EZText
        /// </summary>
        public string[] Errors { get; set; } = new string[0];
        public EZText()
        {
            ezCode = new EzCode();
            ezCode.Initialize();
            ezCode.Code ??= "";
        }
        public EZText(EzCode ezcode)
        {
            ezCode = ezcode;
            ezCode.Code ??= "";
        }
        public EZText(string code)
        {
            ezCode = new EzCode()
            {
                Code = code
            };
        }
        public string Translate() => Translate(ezCode.Code, ezCode.codeLine);
        public string Translate(string InputCode) => Translate(InputCode, ezCode.codeLine);
        public string Translate(int codeLine) => Translate(ezCode.Code, codeLine);
        public string Translate(string InputCode, int _codeLine)
        {
            try
            {
                CodeLine = _codeLine;
                string[] lines = InputCode.Split(Environment.NewLine).Select(x => x.Trim()).Where(y => !y.Equals("") && !y.StartsWith("//")).Select(z=>z
                    .Replace("is equal to", "equals")
                    .Replace(" equals ", " = ")
                    .Replace("is greater than or equal to", ">=")
                    .Replace("is less than or equal to", "<=")
                    .Replace("is greater than", ">")
                    .Replace("is less than", "<")
                    .Replace(", then ", " then ")
                    .Replace(" then ", " : ")
                    ).ToArray();
                for (int i = 0; i < lines.Length; i++)
                {
                    CodeLine++;
                    string line = lines[i];
                    string[] words = line.Split(" ").Select(x => x.Trim()).Where(y => !y.Equals("")).ToArray();
                    string first = words[0].ToLower();
                    switch(first)
                    {
                        case "add":
                        case "subtract":
                        case "multiply":
                        case "divide":
                            try
                            {
                                string second = words[1], third = words[2].ToLower(), fourth = words[3];

                                bool by = first == "multiply" || first == "divide" ? true : false;

                                if (!by && (third == "to" || third == "from"))
                                {
                                    Code += $"{fourth} {(first == "add" ? "+" : first == "subtract" ? "-" : "")} {second}";
                                }
                                else if (by && third == "by")
                                {
                                    Code += $"{second} {(first == "multiply" ? "*" : first == "divide" ? "/" : "")} {fourth}";
                                }
                                else if (first == "add" && third != "to")
                                {
                                    Error("Expected 'to' for English syntax 'Add X to Var'");
                                }
                                else if (first == "subtract" && third != "from")
                                {
                                    Error("Expected 'to' for English syntax 'Subtract X from Var'");
                                }
                                else if (by && third != "by")
                                {
                                    Error("Expected 'by' for English syntax 'Multiply Var by X'");
                                }
                                else
                                {
                                    Error("Expected 'to', 'by', or 'from' for English Syntax, 'Multiply Var by X', 'Subtract X from Var', or 'Add X to Var'");
                                }
                            }
                            catch
                            {
                                Error($"Error occured with '{first}'");
                            }
                            break;
                        case "if":
                            try
                            {
                                if (words[1] == "not" && words[2] == ":")
                                {
                                    words[0] = "else";
                                    List<string> nw = words.ToList();
                                    nw.RemoveAt(1);
                                    words = nw.ToArray();
                                }
                                Code += string.Join(" ", words);
                            }
                            catch
                            {
                                Error($"Error occured with '{first}'");
                            }
                            break;
                        default:
                            if (words.Length > 1)
                            {
                                string second = words[1];
                                if (second == "=")
                                {
                                    string third = words[2];
                                    Code += $"{first} = {third}";
                                    break;
                                }
                            }
                            Error($"Keyword {first} could not be found");
                            Code += "// Error Occured with tranlateing this line: " + line;
                            break;
                    }
                    Code += Environment.NewLine;
                }
            }
            catch
            {
                Error("Unhandled Error Occured");
            }
            return Code;
        }
        public void Error(string error)
        {
            Errors = Errors.Append($"{error} in EZText line {CodeLine}").ToArray();
        }
    }
}