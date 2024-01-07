using EZCode.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EZCode
{
    public class EasierCode
    {
        /// <summary>
        /// EZCode object EZR Code is referencing
        /// </summary>
        public EzCode EZCode { get; set; }
        /// <summary>
        /// Easier Code Given
        /// </summary>
        public string EZRCode { get; set; }
        /// <summary>
        /// Translated Code. Set as <see cref="EzCode.Code"/>
        /// </summary>
        public string Code
        {
            get => Code;
            set
            {
                Code = value;
                EZCode.Code = value;
            }
        }
        public EasierCode()
        {
            EZCode = new EzCode();
            EZCode.Initialize();
            EZCode.Code ??= "";
        }
        public EasierCode(EzCode ezcode)
        {
            EZCode = ezcode;
            EZCode.Code ??= "";
        }
        public EasierCode(string code)
        {
            EZCode = new EzCode()
            {
                Code = code
            };
        }
        public string Translate(string InputCode)
        {
            try
            {
                string[] lines = InputCode.Split(Environment.NewLine).Select(x => x.Trim()).Where(y => !y.Equals("") && !y.StartsWith("//")).ToArray();
                for (int i = 0; i < lines.Length; i++)
                {
                    EZCode.codeLine++;
                    string line = lines[i];
                    string[] words = line.Split(" ").Select(x => x.Trim()).Where(y => !y.Equals("")).ToArray();
                    string first = words[0], second = words[1];
                    switch(first)
                    {
                        case "add":
                            try
                            {
                                string third = words[2], fourth = words[3];
                                if (third == "to")
                                {
                                    Code += $"{second} + {fourth}";
                                }
                                else
                                {
                                    Error("Expected 'to' for English syntax 'Add X to Var'");
                                }
                            }
                            catch
                            {
                                EZCode.ErrorText(words, EzCode.ErrorTypes.normal, first);
                            }
                            break;
                        default:
                            Error($"Keyword {first} could not be found.");
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
            EZCode.ErrorText(new string[0], EzCode.ErrorTypes.custom, custom:error);
        }
    }
}