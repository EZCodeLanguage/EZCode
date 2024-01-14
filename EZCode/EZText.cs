using System.Collections.Generic;
using System.Security.Principal;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
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
                InputCode = InputCode.Replace(".", "|");
                string[] lines = InputCode.Split(new[] { '\n', '|' }).Select(x => x.Trim()).Where(y => !y.Equals("")).Select(z=>z
                    .Replace("is equal to", "equals")
                    .Replace(" equals ", " = ")
                    .Replace("is greater than or equal to", ">=")
                    .Replace("is less than or equal to", "<=")
                    .Replace("is greater than", ">")
                    .Replace("is less than", "<")
                    .Replace(", then ", " then ")
                    .Replace(" then ", " : ")
                    .Replace("{p}", ".")
                    ).ToArray();
                for (int i = 0; i < lines.Length; i++)
                {
                    CodeLine++;
                    string line = lines[i];
                    EZText e = StaticTranslate(line);
                    Errors = e.Errors;
                    Code += e.Code + Environment.NewLine;
                }
            }
            catch
            {

            }
            return Code;
        }
        static EZText StaticTranslate(string line)
        {
            EZText e = new EZText();
            string Code = "";
            try
            {
                string[] words = line.Split(" ").Select(x => x.Trim()).Where(y => !y.Equals("")).ToArray();
                string first = words[0].ToLower();
                switch (first)
                {
                    case "add":
                    case "subtract":
                    case "multiply":
                    case "divide":
                        try
                        {
                            string second = words[1], third = words[2].ToLower(), fourth = words[3];

                            if (first == "add" && third == "to" && fourth == "list")
                            {
                                Code += $"{words[4]} add {words[1]}";
                                break;
                            }
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
                                Error("Expected 'to' for English syntax 'Add X to Var'", e);
                            }
                            else if (first == "subtract" && third != "from")
                            {
                                Error("Expected 'to' for English syntax 'Subtract X from Var'", e);
                            }
                            else if (by && third != "by")
                            {
                                Error("Expected 'by' for English syntax 'Multiply Var by X'", e);
                            }
                            else
                            {
                                Error("Expected 'to', 'by', or 'from' for English Syntax, 'Multiply Var by X', 'Subtract X from Var', or 'Add X to Var'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
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
                            words[0] = words[0].ToLower();
                            string newline = string.Join(" ", words).Split(":")[0];
                            string nextcode = StaticTranslate(string.Join(" ", words).Split(":")[1]).Code;
                            Code += newline + ": " + nextcode;
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "write":
                        try
                        {
                            if (words[words.Length - 3] == "to" && words[words.Length - 2] == "the" && words[words.Length - 1] == "console")
                            {
                                Code += $"print {string.Join(" ", words.Skip(1).Take(words.Length - 4))}";
                            }
                            else if (words[words.Length - 3] == "to" && words[words.Length - 2] == "file")
                            {
                                Code += $"file write {string.Join(" ", words.Skip(1).Take(words.Length - 4))} {words[words.Length - 1]}";
                            }
                            else
                            {
                                Error("Expected 'to the console' or 'to the file URL' at the end of 'write' for syntax, 'write text to the console' or 'write text to the file URL'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "create":
                        try
                        {
                            if (words[1].ToLower() == "a")
                            {
                                if (words[3].ToLower() == "named")
                                {
                                    switch (words[2].ToLower())
                                    {
                                        case "variable":
                                            if (words.Length > 4)
                                            {
                                                if (words[5].ToLower() == "with" && words[6].ToLower() == "the" && words[7].ToLower() == "value")
                                                {
                                                    if (words[8] == "of" && words.Length > 10)
                                                    {
                                                        // intersects
                                                        if (words[9] == "if" && words[11] == "intersects")
                                                        {
                                                            Code += $"var {words[4]} : intersects {words[10]} {words[12]}";
                                                        }

                                                        // file
                                                        if (words[9] == "if" && words[10] == "the" && words[11] == "file" && words[13] == "exists")
                                                        {
                                                            Code += $"var {words[4]} : file exists {words[12]}";
                                                        }
                                                        if (words[9] == "the" && words[10] == "contents" && words[11] == "of" && words[12] == "the" && words[13] == "file")
                                                        {
                                                            Code += $"var {words[4]} : file read {words[14]}";
                                                        }
                                                        if (words[9] == "if" && words[10] == "the" && words[11] == "file" && words[12] == "path" && words[14] == "is" && words[15] == "valid")
                                                        {
                                                            Code += $"var {words[4]} : file validpath {words[13]}";
                                                        }

                                                        // input
                                                        if (words[9] == "the" && words[10] == "console" && words[11] == "input")
                                                        {
                                                            Code += $"var {words[4]} : input console";
                                                        }
                                                        if (words[9] == "the" && words[10] == "current" && (words[11] == "keys" || words[11] == "key") && words[12] == "being" && words[13] == "pressed")
                                                        {
                                                            Code += $"var {words[4]} : input key";
                                                        }
                                                        if (words[9] == "if" && words[11] == "is" && words[12] == "being" && words[13] == "pressed")
                                                        {
                                                            Code += $"var {words[4]} : input key {words[10]}";
                                                        }
                                                        if (words[9] == "the" && words[10] == "current" && words[11] == "mouse" && (words[12] == "button" || words[12] == "buttons") && words[13] == "being" && words[14] == "pressed")
                                                        {
                                                            Code += $"var {words[4]} : input mouse button";
                                                        }
                                                        if (words[9] == "if" && words[10] == "the" && words[12] == "mouse" && words[13] == "button" && words[14] == "is" && words[15] == "being" && words[16] == "pressed")
                                                        {
                                                            Code += $"var {words[4]} : input mouse button {words[11]}";
                                                        }
                                                        if (words[9] == "the" && words[10] == "current" && words[11] == "mouse" && words[12] == "position")
                                                        {
                                                            Code += $"var {words[4]} : input mouse position";
                                                        }
                                                        if (words[9] == "the" && words[10] == "current" && words[12] == "mouse" && words[13] == "position")
                                                        {
                                                            Code += $"var {words[4]} : input mouse position {words[11]}";
                                                        }
                                                        if (words[9] == "the" && words[10] == "current" && words[11] == "mouse" && words[12] == "wheel" && words[13] == "state")
                                                        {
                                                            Code += $"var {words[4]} : input mouse wheel";
                                                        }
                                                    }
                                                    else if (words[8] == "of" && words.Length <= 10)
                                                    {
                                                        Code += $"var {words[4]} {words[9]}";
                                                    }
                                                    else
                                                    {
                                                        Code += $"var {words[4]} {words[8]}";
                                                    }
                                                }
                                                else
                                                {
                                                    Error("Incorrect Syntax. Expected 'Create a variable named Name with the value X'", e);
                                                }
                                            }
                                            else
                                            {
                                                Code += $"var {words[4]}";
                                            }
                                            break;
                                        case "list":
                                            if (words.Length > 4)
                                            {
                                                if (words[5].ToLower() == "with" && words[6].ToLower() == "the" && words[7].ToLower() == "value")
                                                {
                                                    Code += $"list {words[4]} new : {words[8]}";
                                                }
                                                if (words[5].ToLower() == "with" && words[6].ToLower() == "the" && words[7].ToLower() == "values")
                                                {
                                                    Code += $"list {words[4]} new : {string.Join(" ", words.Skip(8))}";
                                                }
                                                else
                                                {
                                                    Error("Incorrect Syntax. Expected 'Create a list named Name with the value X' or 'with the values X, Y, Z'", e);
                                                }
                                            }
                                            else
                                            {
                                                Code += $"list {words[4]} new";
                                            }
                                            break;
                                        default:
                                            Error("Expected 'variable' for syntax, 'Create a variable/list named X'", e);
                                            break;
                                    }
                                }
                                else
                                {
                                    Error("Expected 'named' after create for syntax, 'Create a variable named name with the value X'", e);
                                }
                            }
                            else if (words[1].ToLower() == "the" && words[2] == "file")
                            {
                                Code += $"file create {string.Join(" ", words.Skip(3))}";
                            }
                            else
                            {
                                Error("Expected 'a' after create for syntax, 'Create a variable named name with the value X'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "delete":
                        try
                        {
                            if (words[1].ToLower() == "the" && words[2] == "file")
                            {
                                Code += $"file delete {string.Join(" ", words.Skip(3))}";
                            }
                            else
                            {
                                Error("Expected 'the file' after 'delete' for syntax 'delete the file URL", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "display":
                        try
                        {
                            if (words[words.Length - 2].ToLower() == "via" && words[words.Length - 1].ToLower() == "messagebox")
                            {
                                Code += $"messagebox {words[words.Length - 3]} {string.Join(" ", words.Skip(1).Take(words.Length - 7)).Replace(" ", "\\_")}";
                            }
                            else
                            {
                                Error("Expected 'with messagebox' at the end of 'display' for syntax, 'display text with the title, title via messagebox'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "clear":
                        try
                        {
                            if (words[1] == "the" && words[2] == "console")
                            {
                                Code += "clear";
                            }
                            else if (words[1] == "the" && words[2] == "list" && words.Length == 4)
                            {
                                Code += $"{words[3]} clear";
                            }
                            else
                            {
                                Error("Incorrect syntax. Expected 'clear the console' or 'clear the list name'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "stop":
                        try
                        {
                            if (words[1] == "the" && words[2] == "program")
                            {
                                Code += "stop all";
                            }
                            else if (words[1] == "the" && words[2] == "file")
                            {
                                Code += "stop file";
                            }
                            else
                            {
                                Error("Expected 'stop the program' to clear the console", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "wait":
                        try
                        {
                            if (words[2].ToLower() == "miliseconds")
                            {
                                Code += $"await {words[1]}";
                            }
                            else if (words[1] == "until")
                            {
                                if (words[3].ToLower() == "is" && words[4].ToLower() == "true")
                                {
                                    Code = $"await {words[2]}";
                                }
                                else
                                {
                                    Error("Expected 'is true' after the variable for syntax, 'wait until X is true'", e);
                                }
                            }
                            else
                            {
                                Error("Incorrect Synrax. Update to 'wait X miliseconds' or 'wait until X is true'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "replace":
                        try
                        {
                            if (words[1].ToLower() == "index" && words[3].ToLower() == "with" && words[5].ToLower() == "in" && words[6].ToLower() == "list")
                            {
                                Code += $"{words[7]} equals {words[2]} {words[4]}";
                                break;
                            }
                            else
                            {
                                Error("Incorrect Syntax, 'replace index with value in list name", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "remove":
                        try
                        {
                            if (words[2].ToLower() == "in" && words[3].ToLower() == "list")
                            {
                                Code += $"{words[4]} remove {words[1]}";
                                break;
                            }
                            else
                            {
                                Error("Incorrect Syntax, 'remove index/value in list name", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "destroy":
                        try
                        {
                            if (words.Length == 2)
                            {
                                Code += $"{words[1]} destroy";
                            }
                            else
                            {
                                Error("Incorrect Syntax, 'destroy ListName", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    case "play":
                        try
                        {
                            if (words[1] == "project")
                            {
                                Code += $"file playproj {string.Join(" ", words.Skip(2))}";
                            }
                            else if (words[1] == "file")
                            {
                                Code += $"file play {string.Join(" ", words.Skip(2))}";
                            }
                            else
                            {
                                Error("Expected 'project' or 'file' for syntax 'play project/file URL'", e);
                            }
                        }
                        catch
                        {
                            Error($"Error occured with '{first}'", e);
                        }
                        break;
                    default:
                        if (words.Length > 1)
                        {
                            try
                            {
                                first = words[0];
                                string second = words[1], third = words[2];
                                string next = string.Join(" ", words.Skip(2));
                                if (second == "=" && (next.Contains("neg(") || next.Contains("sq(") || next.Contains("sqr(") || next.Contains("round(") || next.Contains("pow(") || next.Contains("clamp(") || next.Contains("sum(") || next.Contains("avg(") || next.Contains("min(") || next.Contains("max(") || next.Contains("pi()")))
                                {
                                    Code += $"{first} : math {next}";
                                    break;
                                }
                                else if (second == "=" && words.Length == 3)
                                {
                                    Code += $"{first} = {third}";
                                    break;
                                }
                                else if (second == "=" && words[3] == "split")
                                {
                                    string value = words[2];
                                    if (words[3] == "split" && words[4] == "by")
                                    {
                                        string splitter = words[5];

                                        Code += $"{first} split {value} {splitter}";
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                Error("Unexpected Error", e);
                            }
                        }
                        if (!first.StartsWith("//")) Error($"Keyword {first} could not be found", e);
                        else Code += line;
                        break;
                }
            }
            catch
            {
                Error("Unhandled Error Occured", e);
            }
            e.Code = Code;
            return e;
        }
        public static void Error(string error, EZText eZ)
        {
            eZ.Errors = eZ.Errors.Append($"{error} in EZText line {eZ.CodeLine}").ToArray();
        }
    }
}