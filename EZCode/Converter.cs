using NAudio.MediaFoundation;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace EZCode.Converter
{
    public class Converter
    {
        public enum ProgrammingLanguage
        {
            Python,
        }
        private static readonly ProgrammingLanguage Default = ProgrammingLanguage.Python;
        public ProgrammingLanguage Language { get; set; } = Default;
        public string Code { get; set; } = "";
        public Converter() { }
        public Converter(string code, ProgrammingLanguage language)
        {
            Code = code;
            Language = language;
        }
        public Converter(string code)
        {
            Code = code;
        }
        public Converter(ProgrammingLanguage language)
        {
            Language = language;
        }
        public static class ImportAndDefine
        {
            // contains
            public static bool ContainsMethod = false;

            // import
            public static bool Time = false;
            public static bool DateTime = false;
            public static bool OS = false;
            public static bool Sys = false;
            public static bool Random = false;
            public static bool Socket = false;
            public static bool Platform = false;

            // def
            public static bool ReadFile = false;
            public const string READFILE = "def read_file(file_path):\r\n    try:\r\n        with open(file_path, 'r') as file:\r\n            content = file.read()\r\n            return content\r\n    except FileNotFoundError:\r\n        return f\"File not found: {file_path}\"";
            public static bool WriteFile = false;
            public const string WRITEFILE = "def write_file(content, file_path):\r\n    try:\r\n        with open(file_path, 'w') as file:\r\n           file.write(content)\r\n           return file_exists(file_path)\r\n    except:\r\n        return False";
            private static bool _validatePathFile = false;
            public static bool ValidatePathFile
            {
                get => _validatePathFile;
                set
                {
                    _validatePathFile = value;
                    OS = true;
                }
            }
            public const string VALIDATEPATHFILE = "def is_valid_path(file_path):\r\n    return os.path.exists(file_path) and os.path.isfile(file_path)";
            public static bool CreateFile = false;
            public const string CREATEFILE = "def create_file(file_path):\r\n    try:\r\n        with open(file_path, 'w'):\r\n            pass\r\n            return True\r\n    except:\r\n        return False";
            public static bool FileExists = false;
            public const string FILEEXISTS = "def file_exists(file_path):\r\n    return os.path.exists(file_path)";
            private static bool _deletefile = false;
            public static bool DeleteFile
            {
                get => _deletefile;
                set
                {
                    _deletefile = value;
                    OS = true;
                }
            }
            public const string DELETEFILE = "def delete_file(file_path):\r\n    try:\r\n        os.remove(file_path)\r\n        return True\r\n    except FileNotFoundError:\r\n        return False";

            public static bool IsNumber = false;
            public const string ISNUMBER = "def is_number(value):\r\n    try:\r\n        float(value)\r\n        return True\r\n    except:\r\n        return False";

            public static bool Is64Bit
            {
                get => _is64Bit;
                set
                {
                    _is64Bit = value;
                    OS = true;
                }
            }
            private static bool _is64Bit = false;
            public const string IS64BIT = "def is_windows_64bit():\r\n  if 'PROCESSOR_ARCHITEW6432' in os.environ:\r\n    return True\r\n  return os.environ['PROCESSOR_ARCHITECTURE'].endswith('64')";
        }
        public string Convert(string code) => Convert(code, Language);
        public string Convert(ProgrammingLanguage language) => Convert(Code, language);
        public string Convert(ProgrammingLanguage language, string code) => Convert(code, language);
        private static string Convert(string code, ProgrammingLanguage language)
        {
            string converted = "";
            string[] lines = code.Split(Environment.NewLine).Where(x => x != "").Select(y => y.Trim()).ToArray();
            List<EZCodeObject> objects = new List<EZCodeObject>();
            int tab = 0;
            foreach (string _line in lines)
            {
                string con = "";
                string line = _line;
                Actions action = getAction(ref line, objects);

                if (language == ProgrammingLanguage.Python)
                {
                    con = ConvertLinePython(action, line, ref tab);
                }

                converted += con + Environment.NewLine;
            }

            if (language == ProgrammingLanguage.Python && ImportAndDefine.ContainsMethod)
            {
                string firstMethod = lines.FirstOrDefault(x => getAction(x, objects) == Actions.Method, "method Start");
                firstMethod = ConvertLinePython(Actions.Method, firstMethod, ref tab).Split(" ")[1].Replace(":", "");
                converted += Environment.NewLine + firstMethod + Environment.NewLine;
            }

            if (ImportAndDefine.ReadFile) converted = ImportAndDefine.READFILE + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.WriteFile) converted = ImportAndDefine.WRITEFILE + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.ValidatePathFile) converted = ImportAndDefine.VALIDATEPATHFILE + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.CreateFile) converted = ImportAndDefine.CREATEFILE + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.FileExists) converted = ImportAndDefine.FILEEXISTS + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.DeleteFile) converted = ImportAndDefine.DELETEFILE + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.IsNumber) converted = ImportAndDefine.ISNUMBER + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Is64Bit) converted = ImportAndDefine.IS64BIT + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Sys) converted = "import sys" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.OS) converted = "import os" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Time) converted = "import time" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Time) converted = "import datetime" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Random) converted = "import random" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Socket) converted = "import socket" + Environment.NewLine + Environment.NewLine + converted;
            if (ImportAndDefine.Platform) converted = "import platform" + Environment.NewLine + Environment.NewLine + converted;

            return converted;
        }
        private static string ConvertLinePython(Actions action, string line, ref int _tab)
        {
            try
            {
                int tab = _tab;
                string[] w = line.Split(' ').Where(x => x != "").Select(y => y.Trim()).ToArray();
                switch (action)
                {
                    case Actions.None:
                        line = w != new string[0] ? $"# could not convert '{string.Join(" ", w)}' to python" : "";
                        break;
                    case Actions.Comment:
                        line = "# " + string.Join(" ", w).Remove(0, 2);
                        break;
                    case Actions.CreateVariable:
                        if (w[2] == ":")
                        {

                        }
                        else
                        {
                            line = $"{w[1]} = {returnValue(w, 2)}";
                        }
                        break;
                    case Actions.SetVar:
                        if (w[2] == ":")
                        {

                        }
                        else
                        {
                            string first = $"{w[0]} {w[1]}= ";
                            line = first + returnValue(w, 2);
                        }
                        break;
                    case Actions.Print:
                        line = $"print({returnValue(w, 1)})";
                        break;
                    case Actions.Clear:
                        ImportAndDefine.OS = true;
                        line = "os.system('cls')";
                        break;
                    case Actions.ClearConditional:
                        ImportAndDefine.OS = true;
                        line = $"if {returnBoolean(w, 1)}: os.system('cls')";
                        break;
                    case Actions.AwaitMiliseconds:
                        ImportAndDefine.Time = true;
                        try
                        {
                            line = $"time.sleep({float.Parse(w[1]) / 1000})";
                        }
                        catch
                        {
                            line = $"time.sleep({w[1]} / 1000)";
                        }
                        break;
                    case Actions.AwaitConditional:
                        ImportAndDefine.Time = true;
                        try
                        {
                            line = $"while {returnBoolean(w, 1)}: time.sleep({float.Parse(w[1]) / 1000})";
                        }
                        catch
                        {
                            line = $"time.sleep({w[1]} / 1000)";
                        }
                        break;
                    case Actions.ReadFile:
                        ImportAndDefine.ReadFile = true;
                        line = $"read_file({returnValue(w, 2)})";
                        break;
                    case Actions.WriteFile:
                        ImportAndDefine.WriteFile = true;
                        line = $"write_file({returnValue(w[2])}, {returnValue(w, 3)})";
                        break;
                    case Actions.ValidPathFile:
                        ImportAndDefine.ValidatePathFile = true;
                        line = $"is_valid_path({returnValue(w, 2)})";
                        break;
                    case Actions.CreateFile:
                        ImportAndDefine.CreateFile = true;
                        line = $"create_file({returnValue(w, 2)})";
                        break;
                    case Actions.ExistsFile:
                        ImportAndDefine.FileExists = true;
                        line = $"file_exists({returnValue(w, 2)})";
                        break;
                    case Actions.DeleteFile:
                        ImportAndDefine.DeleteFile = true;
                        line = $"delete_file({returnValue(w, 2)})";
                        break;
                    case Actions.StopAll:
                        ImportAndDefine.OS = true;
                        line = $"delete_file({returnValue(w, 2)})";
                        break;
                    case Actions.StopReturn:
                        line = $"return";
                        break;
                    case Actions.StopRestart:
                        line = $"# right now, 'stop restart' can not be converted to python.";
                        break;
                    case Actions.NewList:
                        line = $"{w[1]} = {returnArray(string.Join(" ", w.Skip(4)).Split(","))}";
                        break;
                    case Actions.AddList:
                        line = $"{w[1]}.append({returnValue(w[3])})";
                        break;
                    case Actions.EqualsList:
                        line = $"{w[1]}[{w[3]}] = {returnValue(w[4])}";
                        break;
                    case Actions.ClearList:
                        line = $"{w[1]} = []";
                        break;
                    case Actions.RemoveList:
                        if (int.TryParse(w[3], out _)) line = $"{w[1]}.pop({returnValue(w[3])})";
                        else line = $"{w[1]}.remove({returnValue(w[3])})";
                        break;
                    case Actions.DestroyList:
                        line = $"{w[1]} = [] # can't destroy in python";
                        break;
                    case Actions.SplitList:
                        line = $"{w[1]} = {returnValue(w[3])}.split({returnValue(w[4])})";
                        break;
                    case Actions.Method:
                        ImportAndDefine.ContainsMethod = true;
                        string[] newW = w.Skip(3).Select(x => x.Replace(":", "=").Replace(",", "")).ToArray();
                        for (int i = 0; i < newW.Length; i++)
                        {
                            string before = newW[i].Split("=")[0];
                            string after = newW[i].Split("=")[1];
                            newW[i] = before + "=" + returnValue(after);
                        }
                        string parameters = string.Join(", ", newW);
                        line = $"def {w[1]}({parameters}):";
                        tab += 1;
                        break;
                    case Actions.EndMethod:
                        line = "";
                        tab -= 1;
                        break;
                }
                string tabs = string.Join("", Enumerable.Repeat("    ", _tab));
                _tab = tab;
                return tabs + line;
            }
            catch
            {
                return "";
            }
        }
        private static string returnArray(string[] w)
        {
            string val = "[";
            w = w.Select(x => x.Trim()).ToArray();
            for (int i = 0; i < w.Length; i++)
            {
                val += returnValue(w, i, false) + ", ";
            }
            if(val.EndsWith(", ")) val = val.Remove(val.Length - 2, 2);
            return val + "]";
        }
        private static string returnBoolean(string[] w, int index)
        {
            return string.Join(" ", w.Skip(index)).Replace("?(", "").Replace(")?", "");
        }
        public static string returnValue(string val, bool specialChars = true)
        {
            return returnValue(new[] { val }, 0, false, specialChars);
        }
        public static string returnValue(string[] w, int index, bool all = true, bool specialChars = true)
        {
            try
            {
                return float.Parse(w[index]).ToString();
            }
            catch
            {
                string inside = all ? string.Join(" ", w.Skip(index)) : w[index];
                string incase = inside.Contains("\"") ? "'" : "\"";
                string val = incase + inside + incase;
                return specialChars ? val.SpecialChars() : val;
            }
        }
        private enum Actions
        {
            None,
            Comment,
            CreateVariable,
            SetVar,
            Print,
            Clear,
            ClearConditional,
            AwaitMiliseconds,
            AwaitConditional,
            ReadFile,
            WriteFile,
            ValidPathFile,
            CreateFile,
            ExistsFile,
            DeleteFile,
            StopAll,
            StopReturn,
            StopRestart,
            NewList,
            AddList,
            EqualsList,
            ClearList,
            RemoveList,
            DestroyList,
            SplitList,
            Method,
            EndMethod,
            Loop,
            Else,
            If,
            CallMethod,
            ConsoleInput,
            KeyInput,
            SpecificKeyInput,
            MousePositionInput,
            SpecificMousePositionInput,
            MouseWheelInput,
            RawMouseWheelInput,
            MouseButtonInput,
            SpecificMouseButtonInput,
            Math,

            NewWindow,//
            ChangeWindow,//
            ClearWindow,//
            CloseWindow,//
            OpenWindow,//
            DisplayWindow,//
            DestroyWindow,//
            CreateControl,//
            Destroy,//
            DestroyConditional,//
            Intersects,//
            NewGroup,//
            AddGroup,//
            EqualsGroup,//
            RemoveGroup,//
            ClearGroup,//
            DestroyGroup,//
            DestroyAllGroup,//
            ChangeGroup,//
            StopAllSound,//
            VolumeSound,//
            NewSound,//
            PlaySound,//
            PlayLoopSound,//
            DestroySound,//
            StopSound,//
            FontEvent,//
            TextEvent,//
            ClickEvent,//
            HoverEvent,//
            MoveEvent,//
            BackColorEvent,//
            ForeColorEvent,//
            ImageEvent,//
            ImageLayoutEvent,//
            FocusedEvent,//
            ControlAddedEvent,//
            ControlRemovedEvent,//
            DefocusedEvent,//
            CloseEvent,//
            OpenEvent,//
            EnabledChangedEvent,//
            KeyDownEvent,//
            KeyUpEvent,//
            KeyPressEvent,//
            ResizeEvent,//
            ResizeStartEvent,//
            ResizeEndEvent,//
            BringToFront,//
            BringToBack,//
        }
        private static Actions getAction(string line, List<EZCodeObject> objects)
        {
            string _line = line;
            return getAction(ref _line, objects);
        }
        private static Actions getAction(ref string line, List<EZCodeObject> objects)
        {
            string[] parts = line.Split(' ').Where(x => x != "").Select(y => y.Trim()).ToArray();
            string keyword = parts[0];
            if (keyword.StartsWith("//")) return Actions.Comment;
            switch (keyword)
            {
                case "window":
                    string type = parts[2];
                    switch (type)
                    {
                        case "new":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Window));
                            return Actions.NewWindow;
                        case "change":
                            return Actions.ChangeWindow;
                        case "clear":
                            return Actions.ClearWindow;
                        case "close":
                            return Actions.CloseWindow;
                        case "open":
                            return Actions.OpenWindow;
                        case "display":
                            return Actions.DisplayWindow;
                        case "destroy":
                            return Actions.DestroyWindow;

                    }
                    break;
                case "loop":
                    return Actions.Loop;
                case "else":
                    return Actions.Else;
                case "if":
                    return Actions.If;
                case "print":
                    return Actions.Print;
                case "math":
                    return Actions.Math;
                case "shape":
                case "label":
                case "textbox":
                case "button":
                    objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Control, keyword));
                    return Actions.CreateControl;
                case "clear":
                    break;
                case "destroy":
                    if (parts.Length > 2)
                        return Actions.DestroyConditional;
                    else return Actions.Destroy;
                case "await":
                    try
                    {
                        float.Parse(parts[1]);
                        return Actions.AwaitMiliseconds;
                    }
                    catch
                    {
                        return Actions.AwaitConditional;
                    }
                case "var":
                    objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Var));
                    return Actions.CreateVariable;
                case "global":
                    switch (parts[1])
                    {
                        case "var":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Var));
                            return Actions.CreateVariable;
                        case "list":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.List));
                            return Actions.NewList;
                        case "group":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Group));
                            return Actions.NewGroup;
                        case "shape":
                        case "label":
                        case "textbox":
                        case "button":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Control, parts[1]));
                            return Actions.CreateControl;
                        case "instance":
                            switch (parts[2])
                            {
                                case "shape":
                                case "label":
                                case "textbox":
                                case "button":
                                    objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Control, parts[2]));
                                    return Actions.CreateControl;
                            }
                            break;
                    }
                    break;
                case "instance":
                    switch (parts[1])
                    {
                        case "shape":
                        case "label":
                        case "textbox":
                        case "button":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Control, parts[1]));
                            return Actions.CreateControl;
                    }
                    break;
                case "intersects":
                    return Actions.Intersects;
                case "file":
                    string output = "";
                    type = parts[1].Trim();
                    int endindex = 0;
                    switch (type)
                    {
                        case "read":
                            return Actions.ReadFile;
                        case "write":
                            return Actions.WriteFile;
                        case "validpath":
                            return Actions.ValidPathFile;
                        case "play":
                            return Actions.None;
                        case "playproj":
                            return Actions.None;
                        case "create":
                            return Actions.CreateFile;
                        case "exists":
                            return Actions.ExistsFile;
                        case "delete":
                            return Actions.DeleteFile;
                    }
                    break;
                case "stop":
                    type = parts[1].Trim();
                    switch (type)
                    {
                        case "all":
                            return Actions.StopAll;
                        case "return":
                            return Actions.StopReturn;
                        case "restart":
                            return Actions.StopRestart;
                    }
                    break;
                case "input":
                    type = parts[1].Trim();
                    output = "";
                    switch (type)
                    {
                        case "console":
                            return Actions.ConsoleInput;
                        case "key":
                            if (parts[parts.Length - 1] == "key") return Actions.SpecificKeyInput;
                            else return Actions.KeyInput;
                        case "mouse":
                            switch (parts[2].Trim())
                            {
                                case "position":
                                    switch (parts.Length - 1 < 3 ? "" : parts[3].Trim())
                                    {
                                        case "x":
                                        case "X":
                                            return Actions.SpecificMousePositionInput;
                                        case "y":
                                        case "Y":
                                            return Actions.SpecificMousePositionInput;
                                        case "":
                                            return Actions.MousePositionInput;
                                    }
                                    break;
                                case "wheel":
                                    switch (parts.Length - 1 < 3 ? "" : parts[3].Trim().ToLower())
                                    {
                                        case "":
                                            return Actions.MouseWheelInput;
                                        case "raw":
                                            return Actions.RawMouseWheelInput;
                                    }
                                    break;
                                case "button":
                                    switch (parts.Length - 1 < 3 ? "" : parts[3].Trim())
                                    {
                                        case "":
                                            return Actions.MouseButtonInput;
                                        default:
                                            return Actions.SpecificMouseButtonInput;
                                    }
                            }
                            break;
                    }
                    break;
                case "list":
                    type = parts[2];
                    switch (type)
                    {
                        case "new":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.List));
                            return Actions.NewList;
                        case "add":
                            return Actions.AddList;
                        case "equals":
                            return Actions.EqualsList;
                        case "clear":
                            return Actions.ClearList;
                        case "remove":
                            return Actions.RemoveList;
                        case "destroy":
                            return Actions.DestroyList;
                        case "split":
                            return Actions.SplitList;
                    }
                    break;
                case "group":
                    type = parts[2];
                    switch (type)
                    {
                        case "new":
                            objects.Add(new EZCodeObject(parts[1], EZCodeObject.EZType.Group));
                            return Actions.NewGroup;
                        case "add":
                            return Actions.AddGroup;
                        case "remove":
                            return Actions.RemoveGroup;
                        case "clear":
                            return Actions.ClearGroup;
                        case "destroy":
                            return Actions.DestroyGroup;
                        case "destroyall":
                            return Actions.DestroyAllGroup;
                        case "change":
                            return Actions.ChangeGroup;
                    }
                    break;
                case "sound":
                    string name = parts[1];
                    if (name == "stopall")
                        return Actions.StopAllSound;
                    else if (name == "volume")
                        return Actions.VolumeSound;
                    type = parts[2];
                    switch (type)
                    {
                        case "new":
                            return Actions.NewSound;
                        case "play":
                            return Actions.PlaySound;
                        case "playloop":
                            return Actions.PlayLoopSound;
                        case "destroy":
                            return Actions.DestroySound;
                        case "stop":
                            return Actions.StopSound;
                    }
                    break;
                case "event":
                    name = parts[1];
                    type = parts[2];
                    switch (type)
                    {
                        case "font":
                            return Actions.FontEvent;
                        case "text":
                            return Actions.TextEvent;
                        case "click":
                            return Actions.ClickEvent;
                        case "hover":
                            return Actions.HoverEvent;
                        case "move":
                            return Actions.MoveEvent;
                        case "backcolor":
                            return Actions.BackColorEvent;
                        case "forecolor":
                            return Actions.ForeColorEvent;
                        case "image":
                            return Actions.ImageEvent;
                        case "imagelayout":
                            return Actions.ImageLayoutEvent;
                        case "focused":
                            return Actions.FocusedEvent;
                        case "controladded":
                            return Actions.ControlAddedEvent;
                        case "controlremoved":
                            return Actions.ControlRemovedEvent;
                        case "defocused":
                            return Actions.DefocusedEvent;
                        case "close":
                            return Actions.CloseEvent;
                        case "open":
                            return Actions.OpenEvent;
                        case "enabledchanged":
                            return Actions.EnabledChangedEvent;
                        case "keydown":
                            return Actions.KeyDownEvent;
                        case "keyup":
                            return Actions.KeyUpEvent;
                        case "keypress":
                            return Actions.KeyPressEvent;
                        case "resize":
                            return Actions.ResizeEvent;
                        case "resizestart":
                            return Actions.ResizeStartEvent;
                        case "resizeend":
                            return Actions.ResizeEndEvent;
                    }
                    break;
                case "bringto":
                    string next = parts[1];
                    switch (next)
                    {
                        case "front":
                            return Actions.BringToFront;
                        case "back":
                            return Actions.BringToBack;
                    }
                    break;
                case "method":
                    return Actions.Method;
                case "endmethod":
                    return Actions.EndMethod;
                default:
                    if (parts.Length == 0) return Actions.None;
                    //vars
                    if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Var).FirstOrDefault() || parts[1] == ":" || parts[1] == "+" || parts[1] == "-" || parts[1] == "*" || parts[1] == "/")
                    {
                        return Actions.SetVar;
                    }
                    //controls
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Control).FirstOrDefault())
                    {
                        EZCodeObject obj = objects.FirstOrDefault(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Control);
                        line = $"{obj.Control_Type} " + line;
                        return getAction($"{line}", objects);
                    }
                    //groups
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Group).FirstOrDefault())
                    {
                        line = "group " + line;
                        return getAction($"{line}", objects);
                    }
                    //lists
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.List).FirstOrDefault())
                    {
                        line = "list " + line;
                        return getAction($"{line}", objects);
                    }
                    //windows
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Window).FirstOrDefault())
                    {
                        line = "window " + line;
                        return getAction($"{line}", objects);
                    }
                    //methods
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Method).FirstOrDefault())
                    {
                        return Actions.CallMethod;
                    }
                    break;
            }
            return Actions.None;
        }
        struct EZCodeObject
        {
            public enum EZType
            {
                Var,
                Control,
                List,
                Group,
                Window,
                Method,
            }
            public EZType Type { get; set; }
            public string Name { get; set; }
            public string Control_Type { get; set; }
            public EZCodeObject(string name, EZType type, string controlType = "")
            {
                Name = name;
                Type = type;
                Control_Type = controlType;
            }
        }
    }
    internal static class StringExtensions
    {
        public static string SpecialChars(this string val)
        {
            bool format = false;
            List<string> formats = new List<string>();
            List<string> texts = val.Split(" ").ToList();
            string text = "";

            for (int i = 0; i < texts.Count; i++)
            {
                string cr = texts[i];
                bool start = false, end = false;
                if (cr.StartsWith("\""))
                {
                    cr = cr.Remove(0, 1);
                    start = true;
                }
                if (cr.EndsWith("\""))
                {
                    cr = cr.Remove(texts[i].Length - 2, 1);
                    end = true;
                }
                string t = ColonResponse(cr);
                cr = t == "" ? cr : $"'{t}'";
                texts[i] = t != "" ? (start ? "\"" : "") + cr + (end ? "\"" : "") : texts[i];

                string txt = texts[i];
                bool switched = false;
                string sw_t = txt;
                txt = txt.Contains(@"\!") && !txt.Contains(@"\\!") ? txt.Replace(@"\!", string.Empty) : txt.Contains(@"\\!") ? txt.Replace(@"\\!", @"\!") : txt;
                txt = txt.Contains(@"\_") && !txt.Contains(@"\\_") ? txt.Replace(@"\_", " ") : txt.Contains(@"\\_") ? txt.Replace(@"\\_", @"\_") : txt;
                txt = txt.Contains(@"\;") && !txt.Contains(@"\\;") ? txt.Replace(@"\;", ":") : txt.Contains(@"\\;") ? txt.Replace(@"\\;", @"\;") : txt;
                txt = txt.Contains(@"\q") && !txt.Contains(@"\\q") ? txt.Replace(@"\q", "=") : txt.Contains(@"\\q") ? txt.Replace(@"\\q", @"\q") : txt;
                txt = txt.Contains(@"\c") && !txt.Contains(@"\\c") ? txt.Replace(@"\c", ",") : txt.Contains(@"\\c") ? txt.Replace(@"\\c", @"\c") : txt;
                txt = txt.Contains(@"\e") && !txt.Contains(@"\\e") ? txt.Replace(@"\e", "!") : txt.Contains(@"\\e") ? txt.Replace(@"\\e", @"\e") : txt;
                txt = txt.Contains(@"\$") && !txt.Contains(@"\\$") ? txt.Replace(@"\$", "|") : txt.Contains(@"\\$") ? txt.Replace(@"\\$", @"\$") : txt;
                txt = txt.Contains(@"\&") && !txt.Contains(@"\\&") ? txt.Replace(@"\&", ";") : txt.Contains(@"\\&") ? txt.Replace(@"\\&", @"\&") : txt;
                txt = txt.Contains(@"\p") && !txt.Contains(@"\\p") ? txt.Replace(@"\p", ".") : txt.Contains(@"\\p") ? txt.Replace(@"\\p", @"\p") : txt;
                txt = txt.Replace(@"\\(", @"\(");
                txt = txt.Replace(@")\\", @")\");

                if (txt.Contains(@"\-"))
                {
                    List<char> split = new List<char>();
                    bool _a = false;
                    for (int j = 0; j < txt.Length; j++)
                    {
                        if (_a)
                        {
                            _a = false;
                            continue;
                        }
                        else if ((txt[j] != '\\' && txt.Length - 1 > j && txt[j + 1] != '-') || txt.Length - 1 <= j)
                        {
                            split.Add(txt[j]);
                        }
                        else
                        {
                            _a = true;
                            try
                            {
                                split.RemoveAt(j - 1);
                            }
                            catch
                            {
                                texts[i - 1] = string.Join("", texts[i - 1].Take(texts[i - 1].Length - 1));
                            }
                        }
                    }
                    txt = string.Join("", split);
                }

                switched = sw_t == txt ? switched : true;
                if ((txt.StartsWith("'") && txt.EndsWith("'")) || System.Text.RegularExpressions.Regex.Matches(txt, "'").Count > 1)
                {
                    string[] varray = txt.Split("'");
                    for (int j = 0; j < varray.Length; j++)
                    {
                        if (j % 2 == 1)
                        {
                            format = true;
                            formats.Add(varray[j]);
                            varray[j] = "{}";
                        }
                    }
                    txt = string.Join("", varray);
                }
                txt = txt.Contains(@"\""") && !txt.Contains(@"\\""") ? txt.Replace(@"\""", "'") : txt.Contains(@"\\""") ? txt.Replace(@"\\""", @"\""") : txt;
                switched = sw_t == txt ? switched : true;
                txt = !switched && txt.Contains(@"\") && !txt.Contains(@"\\") ? txt.Replace(@"\", string.Empty) : !switched && txt.Contains(@"\\") ? txt.Replace(@"\\", @"\") : txt;

                text += txt;
                if (i < texts.Count - 1) text += " ";
            }
            if (format)
            {
                text += $".format({string.Join(", ", formats)})";
            }
            if (text.StartsWith("\"{}\".format(") && formats.Count == 1)
            {
                text = formats[0];
            }
            return text;
        }
        private static string ColonResponse(string value)
        {
            string[] ind = value.Split(':');
            if (value.StartsWith("system:"))
            {
                switch (ind[1])
                {
                    case "time":
                        Converter.ImportAndDefine.DateTime = true;
                        if (ind.Length == 2)
                        {
                            List<string> list = ind.ToList();
                            list.Add("now");
                            ind = list.ToArray();
                        }
                        switch (ind[2].ToLower())
                        {
                            case "today":
                                return "datetime.datetime.now().replace(hour=0, minute=0, second=0, microsecond=0).strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                            case "now":
                                return "datetime.datetime.now().strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                            case "utcnow":
                                return "datetime.datetime.utcnow().strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                            case "unixepoch":
                                return "1/1/1970 12:00:00 AM";
                            case "hour24":
                                return "datetime.datetime.now().strftime(\"%H\")";
                            case "hour":
                                return "datetime.datetime.now().strftime(\"%I %p\")";
                            case "minute":
                                return "datetime.datetime.now().strftime(\"%M\")";
                            case "second":
                                return "datetime.datetime.now().strftime(\"%S\")";
                            case "milisecond":
                                return "datetime.datetime.now().strftime(\"%f\")[:-3]";
                            case "nownormal":
                                return "datetime.datetime.now().strftime(\"%m/%d/%Y %I:%M %p\")";
                            case "now24":
                                return "datetime.datetime.now().strftime(\"%m/%d/%Y %H:%M\")";
                            case "date":
                                return "datetime.datetime.now().strftime(\"%m/%d/%Y\")";
                            case "datedash":
                                return "datetime.datetime.now().strftime(\"%m-%d-%Y\")";
                            case "month":
                                return "datetime.datetime.now().strftime(\"%B\")";
                            case "monthnumber":
                                return "datetime.datetime.now().strftime(\"%m\")";
                            case "day":
                                return "datetime.datetime.now().strftime(\"%d\")";
                            case "dayname":
                                return "datetime.datetime.now().strftime(\"%A\")";
                        }
                        break;
                    case "random":
                        switch (ind[2])
                        {
                            default:
                                Converter.ImportAndDefine.Random = true;
                                bool more = value.Contains("system:random:");
                                if (more)
                                {
                                    return $"random.randint({ind[2]}, {ind[3]})";
                                }
                                else
                                {
                                    Converter.ImportAndDefine.Sys = true;
                                    return "random.randint(0, sys.maximize)";
                                }
                            case "single":
                                return "random.random()";
                        }
                    case "isnumber":
                        Converter.ImportAndDefine.IsNumber = true;
                        return $"is_number({Converter.returnValue(ind, 2, false)})";
                    case "machine":
                        switch (ind[2].ToLower())
                        {
                            case "machinename":
                                Converter.ImportAndDefine.Socket = true;
                                return "socket.gethostname()";
                            case "osversion":
                                Converter.ImportAndDefine.Platform = true;
                                return "platform.platform()";
                            case "is64bitoperatingsystem":
                                Converter.ImportAndDefine.Is64Bit = true;
                                return "is_windows_64but()";
                            case "username":
                                Converter.ImportAndDefine.OS = true;
                                return "os.getlogin()";
                            case "workingset":
                                return "\"CAN_NOT_GET_WORKING_SET\"";
                            case "hasshutdownstarted":
                                return "\"CAN_NOT_GET_HAS_SHUT_DOWN_STARTED\"";
                        }
                        break;
                    case "currentfile":
                        Converter.ImportAndDefine.OS = true;
                        return "os.path.dirname(os.path.realpath(__file__))";
                    case "currentplaydirectory":
                        Converter.ImportAndDefine.OS = true;
                        return "os.getcwd()";
                    case "space":
                        return "\" \"";
                    case "newline":
                        return "\"\\n\"";
                    case "pipe":
                        return "\"|\"";
                    case "nothing":
                        return "\"\"";
                }
            }
            if (ind.Length > 1)
            {
                switch (ind[1].ToLower())
                {
                    case "length":
                        return ind[0] + ".__len__()";
                    case "contains":
                        return ind[0] + ".__contains__()";
                    default:
                        return $"{ind[0]}[{ind[1]}]";



                    case "visible":
                        break;
                    case "anchor":
                        break;
                    case "align":
                        break;
                    case "id":
                        break;
                    case "x":
                        break;
                    case "y":
                        break;
                    case "w":
                    case "width":
                        break;
                    case "h":
                    case "height":
                        break;
                    case "backcolor":
                    case "bc":
                    case "bg":
                        break;
                    case "backcolor-r":
                    case "bcr":
                        break;
                    case "backcolor-g":
                    case "bcg":
                        break;
                    case "backcolor-b":
                    case "bcb":
                        break;
                    case "t":
                    case "text":
                        break;
                    case "forecolor":
                    case "fg":
                    case "fc":
                        break;
                    case "forecolor-r":
                    case "fcr":
                        break;
                    case "forecolor-g":
                    case "fcg":
                        break;
                    case "forecolor-b":
                    case "fcb":
                        break;
                    case "click":
                        break;
                    case "font":
                        break;
                    case "fontname":
                        break;
                    case "fontsize":
                        break;
                    case "fontstyle":
                        break;
                    case "point":
                    case "points":
                        break;
                    case "auto":
                    case "autosize":
                        break;
                    case "multi":
                    case "multiline":
                        break;
                    case "wrap":
                    case "wordwrap":
                        break;
                    case "vertical":
                    case "verticalscrollbar":
                        break;
                    case "horizantal":
                    case "horizantalscrollbar":
                        break;
                    case "p":
                    case "poly":
                        break;
                    case "z":
                    case "zindex":
                        break;
                    case "focused":
                        break;
                    case "enabled":
                        break;
                    case "readonly":
                        break;
                    case "image":
                        break;
                    case "imagelayout":
                        break;
                    case "name":
                        break;
                    case "minimizebox":
                        break;
                    case "maximizebox":
                        break;
                    case "showicon":
                        break;
                    case "showintaskbar":
                        break;
                    case "icon":
                        break;
                    case "state":
                        break;
                    case "startposition":
                        break;
                    case "type":
                        break;
                    case "maxwidth":
                        break;
                    case "maxheight":
                        break;
                    case "minwidth":
                        break;
                    case "minheight":
                        break;
                    case "opacity":
                        break;
                }
            }

            return "";
        }
    }
}
