using System.Text.RegularExpressions;

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
        public ImportAndDefine Import = new ImportAndDefine();
        public ProgramFile[] ProgramFiles = Array.Empty<ProgramFile>();
        public class ProgramFile
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public ProgramFile(string name, string content)
            {
                Name = name;
                Content = content;
            }
        }
        public class ImportAndDefine
        {
            // contains
            public bool ContainsMethod = false;

            // import
            public bool Time = false;
            public bool DateTime = false;
            public bool OS = false;
            public bool Sys = false;
            public bool Random = false;
            public bool Socket = false;
            public bool Platform = false;
            public bool Math = false;
            public bool Keyboard = false;
            public bool Pyatuogui = false;
            public bool Pynput = false;
            public bool Threading = false;

            // def
            public bool ReadFile = false;
            public readonly string READFILE = "def read_file(file_path):\r\n    try:\r\n        with open(file_path, 'r') as file:\r\n            content = file.read()\r\n            return content\r\n    except FileNotFoundError:\r\n        return f\"File not found: {file_path}\"";
            public bool WriteFile = false;
            public readonly string WRITEFILE = "def write_file(content, file_path):\r\n    try:\r\n        with open(file_path, 'w') as file:\r\n           file.write(content)\r\n           return file_exists(file_path)\r\n    except:\r\n        return False";
            private bool _validatePathFile = false;
            public bool ValidatePathFile
            {
                get => _validatePathFile;
                set
                {
                    _validatePathFile = value;
                    OS = true;
                }
            }
            public readonly string VALIDATEPATHFILE = "def is_valid_path(file_path):\r\n    return os.path.exists(file_path) and os.path.isfile(file_path)";
            public bool CreateFile = false;
            public readonly string CREATEFILE = "def create_file(file_path):\r\n    try:\r\n        with open(file_path, 'w'):\r\n            pass\r\n            return True\r\n    except:\r\n        return False";
            public bool FileExists = false;
            public readonly string FILEEXISTS = "def file_exists(file_path):\r\n    return os.path.exists(file_path)";
            private bool _deletefile = false;
            public bool DeleteFile
            {
                get => _deletefile;
                set
                {
                    _deletefile = value;
                    OS = true;
                }
            }
            public readonly string DELETEFILE = "def delete_file(file_path):\r\n    try:\r\n        os.remove(file_path)\r\n        return True\r\n    except FileNotFoundError:\r\n        return False";

            public bool IsNumber = false;
            public readonly string ISNUMBER = "def is_number(value):\r\n    try:\r\n        float(value)\r\n        return True\r\n    except:\r\n        return False";

            public bool Is64Bit
            {
                get => _is64Bit;
                set
                {
                    _is64Bit = value;
                    OS = true;
                }
            }
            private bool _is64Bit = false;
            public readonly string IS64BIT = "def is_windows_64bit():\r\n  if 'PROCESSOR_ARCHITEW6432' in os.environ:\r\n    return True\r\n  return os.environ['PROCESSOR_ARCHITECTURE'].endswith('64')";

            public bool KeysPressed
            {
                get => _keysPressed;
                set
                {
                    _keysPressed = value;
                    Pynput = true;
                    Threading = true;
                }
            }
            private bool _keysPressed = false;
            public readonly string KEYSPRESSED = "input_key = set()\r\ninput_key_lock = threading.Lock()\r\n\r\ndef on_press(key):\r\n    global input_key\r\n    with input_key_lock:\r\n        input_key.add(key)\r\n\r\ndef on_release(key):\r\n    with input_key_lock:\r\n        input_key.remove(key)\r\n        \r\nkeyboard_listener = pynput.keyboard.Listener(on_press=on_press, on_release=on_release)\r\nlistener_thread = threading.Thread(target=keyboard_listener.run)\r\nlistener_thread.start()\r\n\r\ndef get_all_keys():\r\n    with input_key_lock:\r\n        return list(input_key)\r\n\r\ndef get_specific_key(key):\r\n    with input_key_lock:\r\n        for k in input_key:\r\n            k = str(k)\r\n            key = f\"'{str(key)}'\"\r\n            if k == key:\r\n                return True\r\n        return False";

            public bool MouseButton
            {
                get => _mouseButton;
                set
                {
                    _mouseButton = value;
                    Pynput = true;
                    Threading = true;
                }
            }
            private bool _mouseButton = false;
            public readonly string MOUSEBUTTON = "input_mouse_buttons = set()\r\ninput_mouse_buttons_lock = threading.Lock()\r\n\r\ndef on_mouse_click(x, y, button, pressed):\r\n    global input_mouse_buttons\r\n    with input_mouse_buttons_lock:\r\n        if pressed:\r\n            input_mouse_buttons.add(button)\r\n        else:\r\n            input_mouse_buttons.remove(button)\r\n\r\ndef get_all_mouse_buttons():\r\n    val = []\r\n    with input_mouse_buttons_lock:\r\n        if pynput.mouse.Button.left in input_mouse_buttons:\r\n            val.append(\"left\")\r\n        if pynput.mouse.Button.right in input_mouse_buttons:\r\n            val.append(\"right\")\r\n        if pynput.mouse.Button.middle in input_mouse_buttons:\r\n            val.append(\"middle\")\r\n    return val\r\n\r\ndef is_left_mouse_button_pressed():\r\n    with input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.left in input_mouse_buttons\r\n\r\ndef is_right_mouse_button_pressed():\r\n    with input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.right in input_mouse_buttons\r\n\r\ndef is_middle_mouse_button_pressed():\r\n    with input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.middle in input_mouse_buttons\r\n\r\ndef get_specific_mouse_button(button):\r\n    if button.lower() == \"left\": button = pynput.mouse.Button.left\r\n    elif button.lower() == \"right\": pynput.mouse.Button.right\r\n    elif button.lower() == \"middle\": pynput.mouse.Button.middle\r\n    with input_mouse_buttons_lock:\r\n        return button in input_mouse_buttons\r\n\r\nmouse_listener = pynput.mouse.Listener(on_click=on_mouse_click)\r\nmouse_thread = threading.Thread(target=mouse_listener.run)\r\nmouse_thread.start()";

            public bool WheelState
            {
                get => _wheelState;
                set
                {
                    _wheelState = value;
                    Pynput = true;
                }
            }
            private bool _wheelState = false;
            public readonly string WHEELSTATE = "wheel_state = 0\r\nwheel_state_raw = 0\r\ndef on_scroll(x, y, dx, dy):\r\n    wheel_state_raw = dy\r\n    if dy > 0:\r\n        wheel_state = 1\r\n    elif dy < 0:\r\n        wheel_state = -1\r\n    else:\r\n        wheel_state = 0\r\n\r\nwith pynput.mouse.Listener(on_scroll=on_scroll) as listener:\r\n    listener.join()";
            
            public bool Clamp = false;
            public readonly string CLAMP = "def clamp(_val, _min, _max):\r\n    return max(_min, min(_val, _max))";
            public bool Average = false;
            public readonly string AVERAGE = "def average(*args):\r\n    if not args:\r\n        return 0\r\n    return sum(args) / len(args)";
        }
        public List<EZCodeObject> objects = new List<EZCodeObject>();
        public string Convert() => Convert(Code, Language);
        public string Convert(string code) => Convert(code, Language);
        public string Convert(ProgrammingLanguage language) => Convert(Code, language);
        public string Convert(string code, ProgrammingLanguage language, out ProgramFile[] programFiles)
        {
            Converter con = new Converter(code, language);
            string ret = con.Convert();
            programFiles = con.ProgramFiles;
            return ret;
        }
        public string Convert(string code, ProgrammingLanguage language)
        {
            Import = new ImportAndDefine();
            string converted = "";
            string[] lines = code.Split(Environment.NewLine).Where(x => x != "").Select(y => y.Trim()).ToArray();
            objects = new List<EZCodeObject>();
            int tab = 0;
            foreach (string _line in lines)
            {
                string con = "";
                string line = _line;
                Actions action = getAction(ref line, objects);

                if (language == ProgrammingLanguage.Python)
                {
                    con = ConvertLinePython(action, line, objects, ref tab);
                }

                converted += con + Environment.NewLine;
            }

            if (language == ProgrammingLanguage.Python)
            {
                ProgramFiles = new ProgramFile[]
                {
                    new ProgramFile("Main.py", converted)
                };
                if (PreRequisites().Trim() != "") ProgramFiles = ProgramFiles.Append(new ProgramFile("Main.py2", PreRequisites())).ToArray();
                if (Import.ContainsMethod)
                {
                    int ttab = 0; 
                    string firstMethod = lines.FirstOrDefault(x => getAction(x, objects) == Actions.Method, "method Start");
                    firstMethod = ConvertLinePython(Actions.Method, firstMethod, objects, ref ttab).Split(" ").Select(x=>x.Trim()).ToArray()[1].Replace(":", "");
                    converted += Environment.NewLine + firstMethod + Environment.NewLine;
                }
            }

            converted = "# Your converted python code" + Environment.NewLine + converted;
            string pre = PreRequisites();
            if (pre.Trim() != "") converted = "# Converted python prerequisites" + Environment.NewLine + pre + converted;

            if (converted.StartsWith(Environment.NewLine)) converted = converted.Remove(0, Environment.NewLine.Length);

            return converted;
        }
        private string PreRequisites()
        {
            string pre = "";
            if (Import.ReadFile) pre = Import.READFILE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.WriteFile) pre = Import.WRITEFILE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.ValidatePathFile) pre = Import.VALIDATEPATHFILE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.CreateFile) pre = Import.CREATEFILE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.FileExists) pre = Import.FILEEXISTS + Environment.NewLine + Environment.NewLine + pre;
            if (Import.DeleteFile) pre = Import.DELETEFILE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.IsNumber) pre = Import.ISNUMBER + Environment.NewLine + Environment.NewLine + pre;
            if (Import.Is64Bit) pre = Import.IS64BIT + Environment.NewLine + Environment.NewLine + pre;
            if (Import.Clamp) pre = Import.CLAMP + Environment.NewLine + Environment.NewLine + pre;
            if (Import.Average) pre = Import.AVERAGE + Environment.NewLine + Environment.NewLine + pre;
            if (Import.KeysPressed) pre = Import.KEYSPRESSED + Environment.NewLine + Environment.NewLine + pre;
            if (Import.MouseButton) pre = Import.MOUSEBUTTON + Environment.NewLine + Environment.NewLine + pre;
            if (Import.WheelState) pre = Import.WHEELSTATE + Environment.NewLine + Environment.NewLine + pre;
            pre = Environment.NewLine + pre;
            if (Import.Sys) pre = "import sys" + Environment.NewLine + pre;
            if (Import.OS) pre = "import os" + Environment.NewLine + pre;
            if (Import.Time) pre = "import time" + Environment.NewLine + pre;
            if (Import.DateTime) pre = "import datetime" + Environment.NewLine + pre;
            if (Import.Random) pre = "import random" + Environment.NewLine + pre;
            if (Import.Socket) pre = "import socket" + Environment.NewLine + pre;
            if (Import.Platform) pre = "import platform" + Environment.NewLine + pre;
            if (Import.Math) pre = "import math" + Environment.NewLine + pre;
            if (Import.Threading) pre = "import threading" + Environment.NewLine + pre;
            if (Import.Keyboard) pre = "import keyboard # use 'pip install keyboard' to install this module" + Environment.NewLine + pre;
            if (Import.Pyatuogui) pre = "import pyautogui # use 'pip install pyautogui' to install this module" + Environment.NewLine + pre;
            if (Import.Pynput) pre = "import pynput # use 'pip install pynput' to install this module" + Environment.NewLine + pre;
            return pre;
        }
        private string ConvertLinePython(Actions action, string line, List<EZCodeObject> objects, ref int _tab)
        {
            try
            {
                int tab = _tab;
                int _t = 0;
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
                            string nextline = string.Join(" ", w.Skip(3));
                            Actions _action = getAction(nextline, objects);
                            int t = 0;
                            nextline = ConvertLinePython(_action, nextline, objects, ref _t);
                            line = $"{w[1]} = {nextline}";
                        }
                        else
                        {
                            line = $"{w[1]} = {returnValue(w, 2)}";
                        }
                        break;
                    case Actions.SetVar:
                        if (w[1] == ":")
                        {
                            string nextline = string.Join(" ", w.Skip(2));
                            Actions _action = getAction(nextline, objects);
                            nextline = ConvertLinePython(_action, nextline, objects, ref tab);
                            line = $"{w[0]} = {nextline}";
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
                        Import.OS = true;
                        line = "os.system('cls')";
                        break;
                    case Actions.ClearConditional:
                        Import.OS = true;
                        line = $"if {returnBoolean(w, 1)}: os.system('cls')";
                        break;
                    case Actions.AwaitMiliseconds:
                        Import.Time = true;
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
                        Import.Time = true;
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
                        Import.ReadFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"read_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.WriteFile:
                        Import.WriteFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"write_file({returnValue(w[2])}, {returnValue(w, 3)})";
                        }
                        break;
                    case Actions.ValidPathFile:
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            Import.ValidatePathFile = true;
                        }
                        line = $"is_valid_path({returnValue(w, 2)})";
                        break;
                    case Actions.CreateFile:
                        Import.CreateFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"create_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.ExistsFile:
                        Import.FileExists = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"file_exists({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.DeleteFile:
                        Import.DeleteFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"delete_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.StopAll:
                        Import.OS = true;
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
                        Import.ContainsMethod = true;
                        string[] newW = string.Join(" ", w.Skip(3).Select(x => x.Replace(":", "="))).Split(",").Where(y => y != "").ToArray();
                        for (int i = 0; i < newW.Length; i++)
                        {
                            string before = newW[i].Split("=")[0];
                            string after = newW[i].Split("=")[1];
                            newW[i] = before + "=" + returnValue(after);
                        }
                        string parameters = string.Join(",", newW);
                        line = $"def {w[1]}({parameters}):";
                        tab ++;
                        break;
                    case Actions.EndMethod:
                        line = "";
                        tab--;
                        break;
                    case Actions.ClosingBracket:
                        line = "";
                        tab--;
                        break;
                    case Actions.OpenningBracket:
                        line = "";
                        break;
                    case Actions.If:
                        line = $"if {returnAgument(w, 1, objects, out int next)}:";
                        if (w.Length > next && !string.Join(" ", w).Trim().EndsWith("{"))
                        {
                            string nextLine = string.Join(" ", w.Skip(next));
                            Actions _action = getAction(nextLine, objects);
                            tab++;
                            line += Environment.NewLine + ConvertLinePython(_action, nextLine, objects, ref tab);
                            tab--;
                        }
                        else
                        {
                            tab++;
                        }
                        break;
                    case Actions.Loop:
                        next = 0;
                        line = $"while {returnAgument(w, 1, objects, out next)}:";
                        tab++;
                        break;
                    case Actions.Else:
                        line = $"else:";
                        if (w.Length > 2 && !string.Join(" ", w).Trim().EndsWith("{"))
                        {
                            string nextLine = string.Join(" ", w.Skip(2));
                            Actions _action = getAction(nextLine, objects);
                            tab++;
                            line += Environment.NewLine + ConvertLinePython(_action, nextLine, objects, ref tab);
                            tab--;
                        }
                        else
                        {
                            tab++;
                        }
                        break;
                    case Actions.CallMethod:
                        newW = string.Join(" ", w.Skip(2)).Split(",").Where(x => x != "").ToArray();
                        for (int i = 0; i < newW.Length; i++)
                        {
                            newW[i] = returnValue(newW[i]);
                        }
                        parameters = string.Join(", ", newW);
                        line = $"{w[0]}({parameters})";
                        break;
                    case Actions.Math:
                        string values = string.Join(" ", w.Skip(1).TakeWhile(x => x != ":" && x != "=>").Select(returnMath));
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            values = nl;
                        }
                        line = values;
                        break;
                    case Actions.ConsoleInput:
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = "input()";
                        }
                        break;
                    case Actions.KeyInput:
                        Import.KeysPressed = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"get_all_keys()";
                        }
                        break;
                    case Actions.SpecificKeyInput:
                        Import.KeysPressed = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"get_specific_key({returnValue(w[2])})";
                        }
                        break;
                    case Actions.MousePositionInput:
                        Import.Pyatuogui = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"pyautogui.position()";
                        }
                        break;
                    case Actions.SpecificMousePositionInput:
                        Import.Pyatuogui = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"pyautogui.position().{w[3]}";
                        }
                        break;
                    case Actions.MouseWheelInput:
                    case Actions.RawMouseWheelInput:
                        Import.WheelState = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"wheel_state{(Actions.RawMouseWheelInput == action ? "_raw" : "")}";
                        }
                        break;
                    case Actions.MouseButtonInput:
                        Import.MouseButton = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"get_all_mouse_buttons()";
                        }
                        break;
                    case Actions.SpecificMouseButtonInput:
                        Import.MouseButton = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"get_specific_mouse_button({returnValue(w[3])})";
                        }
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
        private string returnMath(string value)
        {
            if (!value.EndsWith(")") && Regex.Matches(value, @"\)").Count == 1 && Regex.Matches(value, @"\(").Count == 1)
                return value;

            if (value.StartsWith("abs("))
            {
                string eq = value.Replace("abs(", "").Replace(")", "");
                return $"abs({eq})";
            }
            else if (value.StartsWith("neg("))
            {
                string eq = value.Replace("neg(", "").Replace(")", "");
                return $"-{eq}";
            }
            else if (value.StartsWith("sq("))
            {
                string eq = value.Replace("sq(", "").Replace(")", "");
                return $"({eq} ** 2)";
            }
            else if (value.StartsWith("sqr("))
            {
                Import.Math = true;
                string eq = value.Replace("sqr(", "").Replace(")", "");
                return $"math.sqrt({eq})";
            }
            else if (value.StartsWith("round("))
            {
                string eq = value.Replace("round(", "").Replace(")", "");
                return $"round({eq})";
            }
            else if (value.StartsWith("pow("))
            {
                string eq = value.Replace("pow(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"({eqboth[0]} ** {eqboth[1]})";
            }
            else if (value.StartsWith("clamp("))
            {
                Import.Clamp = true;

                string eq = value.Replace("clamp(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");

                float ineq1 = float.Parse(eqboth[0].Trim());
                float ineq2 = float.Parse(eqboth[1].Trim());
                float ineq3 = float.Parse(eqboth[2].Trim());
                return $"clamp({ineq1}, {ineq2}, {ineq3})";
            }
            else if (value.StartsWith("sum("))
            {
                string eq = value.Replace("sum(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"sum({string.Join(", ", eqboth)})";
            }
            else if (value.StartsWith("avg("))
            {
                Import.Average = true;
                string eq = value.Replace("avg(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"average({string.Join(", ", eqboth)})";
            }
            else if (value.StartsWith("min("))
            {
                string eq = value.Replace("min(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"min({string.Join(", ", eqboth)})";
            }
            else if (value.StartsWith("max("))
            {
                string eq = value.Replace("max(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"max({string.Join(", ", eqboth)})";
            }
            else if (value.Equals("pi()"))
            {
                return "";
            }
            return value;
        }
        private string returnAgument(string[] _w, int index, List<EZCodeObject> objects, out int next)
        {
            string val = "";
            string[] w = _w.Skip(index).TakeWhile(x => x != ":" && x != "{").Select(y => y.Trim().Replace("?(", "").Replace(")?", "")).ToArray();
            for (int i = 0; i < w.Length; i++)
            {
                if (objects.FirstOrDefault(x => x.Type == EZCodeObject.EZType.Var && x.Name == w[i], null) != null)
                {
                    val += w[i] + " ";
                }
                else if (new[] { "=", "!", "!=", ">", "<", "<=", ">=", "&", "and", "or", "true", "false" }.Any(x => w[i] == x))
                {
                    val += w[i] + " ";
                }
                else
                {
                    val += returnValue(w, i, false) + " ";
                }
            }
            if (val.EndsWith(" ")) val = val.Remove(val.Length - 1, 1);
            next = _w.ToList().IndexOf(":") + 1;
            return val.Replace(" = ", " == ").Replace(" ! ", " not ").Replace("&", "and").Replace("true", "True").Replace("false", "False");
        }
        private string returnArray(string[] w)
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
        private string returnBoolean(string[] w, int index)
        {
            return string.Join(" ", w.Skip(index)).Replace("?(", "").Replace(")?", "");
        }
        public string returnValue(string val, bool specialChars = true)
        {
            return returnValue(new[] { val }, 0, false, specialChars);
        }
        public string returnValue(string[] w, int index, bool all = true, bool specialChars = true)
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
                return specialChars ? val.SpecialChars(this) : val;
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
            ClosingBracket,
            OpenningBracket,
            Loop,
            Else,
            If,
            CallMethod,
            Math,
            ConsoleInput,
            KeyInput,
            SpecificKeyInput,
            MousePositionInput,
            SpecificMousePositionInput,
            MouseWheelInput,
            RawMouseWheelInput,
            MouseButtonInput,
            SpecificMouseButtonInput,

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
                            if (parts[parts.Length - 1] == "key") return Actions.KeyInput;
                            else return Actions.SpecificKeyInput;
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
                                    switch (parts.TakeWhile(x => x != ":" && x != ":").ToArray().Length - 1 < 3 ? "" : parts[3].Trim())
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
                    if (parts.Length > 2)
                    {
                        string[] param = string.Join(" ", parts.Skip(3)).Split(",").Select(x=>x.Trim()).ToArray();
                        EZCodeObject[] vals = new EZCodeObject[0];
                        foreach (string val in param)
                        {
                            vals = vals.Append(new EZCodeObject(val.Split(":")[0], EZCodeObject.EZType.Var)).ToArray();
                        }
                        objects.AddRange(vals.ToList());
                    }
                    return Actions.Method;
                case "endmethod":
                    return Actions.EndMethod;
                case "}":
                    return Actions.ClosingBracket;
                case "{":
                    return Actions.OpenningBracket;
                default:
                    if (parts.Length == 0) return Actions.None;
                    //vars
                    if (parts.Length > 1 && (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Var).FirstOrDefault() || parts[1] == "+" || parts[1] == "-" || parts[1] == "*" || parts[1] == "/"))
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
                    else if (objects.Select(x => x.Name == keyword && x.Type == EZCodeObject.EZType.Method).FirstOrDefault() || parts.Length == 1)
                    {
                        return Actions.CallMethod;
                    }
                    else
                    {
                        if (parts.Length > 1 && parts[1] == ":" && parts.Contains(":"))
                        {
                            return Actions.CallMethod;
                        }
                    }
                    break;
            }
            return Actions.None;
        }
        public class EZCodeObject
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
        public static string SpecialChars(this string val, Converter converter)
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
                string t = ColonResponse(cr, converter);
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
        private static string ColonResponse(string value, Converter converter)
        {
            string[] ind = value.Split(':');
            if (value.StartsWith("system:"))
            {
                switch (ind[1])
                {
                    case "time":
                        converter.Import.DateTime = true;
                        if (ind.Length == 2)
                        {
                            List<string> list = ind.ToList();
                            list.Add("now");
                            ind = list.ToArray();
                        }
                        switch (ind[2].ToLower())
                        {
                            case "today":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().replace(hour=0, minute=0, second=0, microsecond=0).strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                                else return "";
                            case "now":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                                else return "";
                            case "utcnow":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.utcnow().strftime(\"%m/%d/%Y %I:%M:%S %p\")";
                                else return "";
                            case "unixepoch":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "1/1/1970 12:00:00 AM";
                                else return "";
                            case "hour24":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%H\")";
                                else return "";
                            case "hour":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%I %p\")";
                                else return "";
                            case "minute":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%M\")";
                                else return "";
                            case "second":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%S\")";
                                else return "";
                            case "milisecond":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%f\")[:-3]";
                                else return "";
                            case "nownormal":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m/%d/%Y %I:%M %p\")";
                                else return "";
                            case "now24":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m/%d/%Y %H:%M\")";
                                else return "";
                            case "date":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m/%d/%Y\")";
                                else return "";
                            case "datedash":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m-%d-%Y\")";
                                else return "";
                            case "month":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%B\")";
                                else return "";
                            case "monthnumber":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%m\")";
                                else return "";
                            case "day":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%d\")";
                                else return "";
                            case "dayname":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "datetime.datetime.now().strftime(\"%A\")";
                                else return "";
                        }
                        break;
                    case "random":
                        switch (ind[2])
                        {
                            default:
                                converter.Import.Random = true;
                                bool more = value.Contains("system:random:");
                                if (more)
                                {
                                    if (converter.Language == Converter.ProgrammingLanguage.Python)
                                        return $"random.randint({ind[2]}, {ind[3]})";
                                    else return "";
                                }
                                else
                                {
                                    converter.Import.Sys = true;
                                    if (converter.Language == Converter.ProgrammingLanguage.Python)
                                        return "random.randint(0, sys.maximize)";
                                    else return "";
                                }
                            case "single":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "random.random()";
                                else return "";
                        }
                    case "isnumber":
                        converter.Import.IsNumber = true;
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return $"is_number({converter.returnValue(ind, 2, false)})";
                        else return "";
                    case "machine":
                        switch (ind[2].ToLower())
                        {
                            case "machinename":
                                converter.Import.Socket = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "socket.gethostname()";
                                else return "";
                            case "osversion":
                                converter.Import.Platform = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "platform.platform()";
                                else return "";
                            case "is64bitoperatingsystem":
                                converter.Import.Is64Bit = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "is_windows_64but()";
                                else return "";
                            case "username":
                                converter.Import.OS = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "os.getlogin()";
                                else return "";
                            case "workingset":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "\"CAN_NOT_GET_WORKING_SET\"";
                                else return "";
                            case "hasshutdownstarted":
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "\"CAN_NOT_GET_HAS_SHUT_DOWN_STARTED\"";
                                else return "";
                        }
                        break;
                    case "currentfile":
                        converter.Import.OS = true;
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return "os.path.dirname(os.path.realpath(__file__))";
                        else return "";
                    case "currentplaydirectory":
                        converter.Import.OS = true;
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return "os.getcwd()";
                        else return "";
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
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return ind[0] + ".__len__()";
                        else return "";
                    case "contains":
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return ind[0] + ".__contains__()";
                        else return "";
                    default:
                        if (converter.objects.Select(x => x.Name).Contains(ind[0]))
                        {
                            if (converter.Language == Converter.ProgrammingLanguage.Python)
                                return $"{ind[0]}[{ind[1]}]";
                            else return "";
                        }
                        else return "";



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
