using Antlr.Runtime.Tree;

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
        public PythonImportAndDefine PythonImport = new PythonImportAndDefine();
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
        public class PythonImportAndDefine
        {
            // __init__
            public bool Init { get => INIT != "def __init__(self):\r\n"; }
            public string INIT = "def __init__(self):\r\n";
            private bool init_keyboard = false;
            public bool Init_keyboard
            {
                get
                {
                    return init_keyboard;
                }
                set
                {
                    if (value && !init_keyboard)
                    {
                        INIT += "    self.input_key = set()\r\n    self.input_key_lock = threading.Lock()\r\n    self.start_keyboard_listener()\r\n";
                    }
                    init_keyboard = value;
                }
            }
            private bool init_mouse_button = false;
            public bool Init_mouse_button
            {
                get
                {
                    return init_mouse_button;
                }
                set
                {
                    if (value && !init_mouse_button)
                    {
                        INIT += "    self.input_mouse_buttons = set()\r\n    self.input_mouse_buttons_lock = threading.Lock()\r\n    self.start_mouse_button_listener()\r\n";
                    }
                    init_mouse_button = value;
                }
            }
            private bool init_mouse_wheel = false;
            public bool Init_mouse_wheel
            {
                get
                {
                    return init_mouse_wheel;
                }
                set
                {
                    if (value && !init_mouse_wheel)
                    {
                        INIT += "    self.wheel_state = 0\r\n    self.wheel_state_raw = 0\r\n    self.start_mouse_wheel_listener()\r\n";
                    }
                    init_mouse_wheel = value;
                }
            }

            // contains
            public bool ContainsMethod = false;
            public bool ContainsWindow = false;
            public string firstWindow = "";

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
            public bool TKinter = false;
            public bool Enum = false;

            // def
            public bool ReadFile = false;
            public readonly string READFILE = "def read_file(self, file_path):\r\n    try:\r\n        with open(file_path, 'r') as file:\r\n            content = file.read()\r\n            return content\r\n    except FileNotFoundError:\r\n        return f\"File not found: {file_path}\"";
            public bool WriteFile = false;
            public readonly string WRITEFILE = "def write_file(self, content, file_path):\r\n    try:\r\n        with open(file_path, 'w') as file:\r\n            file.write(content)\r\n            return self.file_exists(file_path)\r\n    except:\r\n        return False";
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
            public readonly string VALIDATEPATHFILE = "def is_valid_path(self, file_path):\r\n    return os.path.exists(file_path) and os.path.isfile(file_path)";
            public bool CreateFile = false;
            public readonly string CREATEFILE = "def create_file(self, file_path):\r\n    try:\r\n        with open(file_path, 'w'):\r\n            pass\r\n            return True\r\n    except:\r\n        return False";
            public bool FileExists = false;
            public readonly string FILEEXISTS = "def file_exists(self, file_path):\r\n    return os.path.exists(file_path)";
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
            public readonly string DELETEFILE = "def delete_file(self, file_path):\r\n    try:\r\n        os.remove(file_path)\r\n        return True\r\n    except FileNotFoundError:\r\n        return False";

            public bool IsNumber = false;
            public readonly string ISNUMBER = "def is_number(self, value):\r\n    try:\r\n        float(value)\r\n        return True\r\n    except:\r\n        return False";

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
            public readonly string IS64BIT = "def is_windows_64bit(self):\r\n    if 'PROCESSOR_ARCHITEW6432' in os.environ:\r\n        return True\r\n    return os.environ['PROCESSOR_ARCHITECTURE'].endswith('64')";

            public bool KeysPressed
            {
                get => _keysPressed;
                set
                {
                    _keysPressed = value;
                    Pynput = true;
                    Threading = true;
                    Init_keyboard = true;
                }
            }
            private bool _keysPressed = false;
            public readonly string KEYSPRESSED = "def on_press(self, key):\r\n    with self.input_key_lock:\r\n        self.input_key.add(key)\r\n\r\ndef on_release(self, key):\r\n    with self.input_key_lock:\r\n        self.input_key.remove(key)\r\n\r\ndef start_keyboard_listener(self):\r\n    keyboard_listener = pynput.keyboard.Listener(\r\n        on_press=self.on_press, on_release=self.on_release\r\n    )\r\n    keyboard_listener.start()\r\n\r\ndef get_all_keys(self):\r\n    with self.input_key_lock:\r\n        return list(self.input_key)\r\n\r\ndef get_specific_key(self, key):\r\n    with self.input_key_lock:\r\n        for k in self.input_key:\r\n            k_str = str(k)\r\n            key_str = f\"'{str(key)}'\"\r\n            if k_str == key_str:\r\n                return True\r\n        return False";

            public bool MouseButton
            {
                get => _mouseButton;
                set
                {
                    _mouseButton = value;
                    Pynput = true;
                    Threading = true;
                    Init_mouse_button = true;
                }
            }
            private bool _mouseButton = false;
            public readonly string MOUSEBUTTON = "def on_mouse_click(self, x, y, button, pressed):\r\n    with self.input_mouse_buttons_lock:\r\n        if pressed:\r\n            self.input_mouse_buttons.add(button)\r\n        else:\r\n            self.input_mouse_buttons.remove(button)\r\n\r\ndef get_all_mouse_buttons(self):\r\n    val = []\r\n    with self.input_mouse_buttons_lock:\r\n        if pynput.mouse.Button.left in self.input_mouse_buttons:\r\n            val.append(\"left\")\r\n        if pynput.mouse.Button.right in self.input_mouse_buttons:\r\n            val.append(\"right\")\r\n        if pynput.mouse.Button.middle in self.input_mouse_buttons:\r\n            val.append(\"middle\")\r\n    return val\r\n\r\ndef is_left_mouse_button_pressed(self):\r\n    with self.input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.left in self.input_mouse_buttons\r\n\r\ndef is_right_mouse_button_pressed(self):\r\n    with self.input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.right in self.input_mouse_buttons\r\n\r\ndef is_middle_mouse_button_pressed(self):\r\n    with self.input_mouse_buttons_lock:\r\n        return pynput.mouse.Button.middle in self.input_mouse_buttons\r\n\r\ndef get_specific_mouse_button(self, button):\r\n    if button.lower() == \"left\":\r\n        button_enum = pynput.mouse.Button.left\r\n    elif button.lower() == \"right\":\r\n        button_enum = pynput.mouse.Button.right\r\n    elif button.lower() == \"middle\":\r\n        button_enum = pynput.mouse.Button.middle\r\n    else:\r\n        raise ValueError(\"Invalid mouse button specified\")\r\n\r\n    with self.input_mouse_buttons_lock:\r\n        return button_enum in self.input_mouse_buttons\r\n\r\ndef start_mouse_button_listener(self):\r\n    mouse_listener = pynput.mouse.Listener(on_click=self.on_mouse_click)\r\n    mouse_listener.start()";

            public bool WheelState
            {
                get => _wheelState;
                set
                {
                    _wheelState = value;
                    Pynput = true;
                    Init_mouse_wheel = true;
                }
            }
            
            private bool _wheelState = false;
            public readonly string WHEELSTATE = "def on_scroll(self, x, y, dx, dy):\r\n    self.wheel_state_raw = dy\r\n    if dy > 0:\r\n        self.wheel_state = 1\r\n    elif dy < 0:\r\n        self.wheel_state = -1\r\n    else:\r\n        self.wheel_state = 0\r\n\r\ndef start_mouse_wheel_listener(self):\r\n    mouse_listener = pynput.mouse.Listener(on_scroll=self.on_scroll)\r\n    mouse_listener.start()";
            public bool Clamp = false;
            public readonly string CLAMP = "def clamp(self, _val, _min, _max):\r\n    return max(_min, min(_val, _max))";
            public bool Average = false;
            public readonly string AVERAGE = "def average(self, *args):\r\n    if not args:\r\n        return 0\r\n    return sum(args) / len(args)";
            public bool Window
            {
                get => _window;
                set
                {
                    _window = value;
                    TKinter = true;
                    Enum = true;
                    ContainsWindow = true;
                }
            }
            private bool _window = false;
            public readonly string WINDOW = "class Window:\r\n    class Control:\r\n        class Type(Enum):\r\n            Shape = 1\r\n            Label = 2\r\n            Textbox = 3\r\n            Button = 4\r\n\r\n        class Properties:\r\n            def __init__(self):\r\n                self.X = 0\r\n                self.Y = 0\r\n                self.Width = 0\r\n                self.Height = 0\r\n                self.Text = \"\"\r\n\r\n        def __init__(self, control_type=Type.Shape):\r\n            self.type = control_type\r\n            self.properties = self.Properties()\r\n            self.X = self.properties.X\r\n            self.widget = None \r\n\r\n        def change(self, x=-1, y=-1, width=-1, height=-1, title=\"\"):\r\n            if x != -1: self.properties.X = x\r\n            if y != -1: self.properties.Y = y\r\n            if width != -1: self.properties.Width = width\r\n            elif width == -1 and self.type == EZCodeConversion.Window.Control.Type.Shape: self.properties.Width = 50\r\n            elif width == -1 and self.type == EZCodeConversion.Window.Control.Type.Button: self.properties.Width = 75\r\n            elif width == -1 and self.type == EZCodeConversion.Window.Control.Type.Textbox: self.properties.Width = 75\r\n            if height != -1: self.properties.Height = height\r\n            elif height == -1 and self.type == EZCodeConversion.Window.Control.Type.Shape: self.properties.Height = 50\r\n            elif height == -1 and self.type == EZCodeConversion.Window.Control.Type.Button: self.properties.Height = 25\r\n            elif height == -1 and self.type == EZCodeConversion.Window.Control.Type.Textbox: self.properties.Height = 50\r\n            if title != \"\": self.properties.Text = title\r\n    def __init__(self):\r\n        self.window = None\r\n        self.X = 0\r\n        self.Y = 0\r\n        self.Width = 600\r\n        self.Height = 400\r\n        self.Title = \"tk\"\r\n        self.Icon = \"\"\r\n        self.controls = []\r\n        self.waitDisplayControls = []\r\n\r\n    def open(self):\r\n        self.window = tkinter.Tk()\r\n        self.change()\r\n        for control in self.waitDisplayControls:\r\n            self.display_control(control)\r\n        self.waitDisplayControls = []\r\n\r\n    def close(self):\r\n        self.window = tkinter.Tk().destroy()\r\n        self.window = None\r\n\r\n    def destroy(self):\r\n        self.window = tkinter.Tk().destroy()\r\n        self.window = None\r\n        self.controls = []\r\n        self.waitDisplayControls = []\r\n\r\n    def start_loop(self):\r\n        try: self.window.mainloop()\r\n        except: None\r\n        \r\n    def change(self, x=0, y=0, width=600, height=400, title=\"\", icon=\"\", default=\"\"):\r\n        if x != 0: self.X = x\r\n        if y != 0: self.Y = y\r\n        if width != 600: self.Width = width\r\n        if height != 400: self.Height = height\r\n        if title != \"\": self.Title = title\r\n        if icon != \"\": self.Icon = icon\r\n\r\n        try:\r\n            self.window.geometry(f\"{self.Width}x{self.Height}+{self.X}+{self.Y}\")\r\n            self.window.title(self.Title)\r\n            self.window.iconbitmap(self.Icon)\r\n        except:\r\n            None\r\n\r\n    def display_controls(self, *args):\r\n        if self.window == None:\r\n            for control in args:\r\n                self.waitDisplayControls.append(control)\r\n            return\r\n        for control in args:\r\n            self.controls.append(control)\r\n            self.display_control(control)\r\n    \r\n    def display_control(self, control):\r\n            if control.type == EZCodeConversion.Window.Control.Type.Shape:\r\n                shape = tkinter.Canvas(self.window, width=control.properties.Width, height=control.properties.Height, bg=\"blue\")\r\n                shape.place(x=control.properties.X, y=control.properties.Y)\r\n                control.widget = shape\r\n            elif control.type == EZCodeConversion.Window.Control.Type.Label:\r\n                label = tkinter.Label(self.window, text=control.properties.Text)\r\n                label.place(x=control.properties.X, y=control.properties.Y)\r\n                control.widget = label\r\n            elif control.type == EZCodeConversion.Window.Control.Type.Textbox:\r\n                textbox = tkinter.Entry(self.window, width=control.properties.Width)\r\n                textbox.place(x=control.properties.X, y=control.properties.Y)\r\n                textbox.delete(0, tkinter.END)\r\n                textbox.insert(0, control.properties.Text)\r\n                control.widget = textbox\r\n            elif control.type == EZCodeConversion.Window.Control.Type.Button:\r\n                button = tkinter.Button(self.window, text=control.properties.Text)\r\n                button.place(x=control.properties.X, y=control.properties.Y)\r\n                control.widget = button\r\n\r\n    def clear_controls(self):\r\n        for control in self.controls:\r\n            control.widget.destroy()";
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
            PythonImport = new PythonImportAndDefine();
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
                converted = "ezcode_conversion = EZCodeConversion()" + Environment.NewLine + converted + (PythonImport.ContainsWindow ? Environment.NewLine + $"{PythonImport.firstWindow}.start_loop()" : "");

                ProgramFiles = new ProgramFile[]
                {
                    new ProgramFile("Main.py", "from EZCodeConversion import EZCodeConversion" + Environment.NewLine + PreRequisites(true) +Environment.NewLine + converted),
                };
                if (PreRequisites().Trim() != "") ProgramFiles = ProgramFiles.Append(new ProgramFile("EZCodeConversion.py", PreRequisites())).ToArray();
                if (PythonImport.ContainsMethod)
                {
                    int ttab = 0; 
                    string firstMethod = lines.FirstOrDefault(x => getAction(x, objects) == Actions.Method, "method Start");
                    firstMethod = ConvertLinePython(Actions.Method, firstMethod, objects, ref ttab).Split(" ").Select(x=>x.Trim()).ToArray()[1].Replace(":", "");
                    converted += Environment.NewLine + firstMethod + Environment.NewLine;
                }

                converted = "# Your converted EZCode" + Environment.NewLine + converted;
                string pre = PreRequisites();
                if (pre.Trim() != "") converted = "# Python prerequisites" + Environment.NewLine + pre + converted;
            }

            if (converted.StartsWith(Environment.NewLine)) converted = converted.Remove(0, Environment.NewLine.Length);

            return converted;
        }
        private string PreRequisites(bool justImports = false)
        {
            string pre = "";
            if (Language == ProgrammingLanguage.Python)
            {
                if (!justImports)
                {
                    if (PythonImport.ReadFile) pre = PythonImport.READFILE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.WriteFile) pre = PythonImport.WRITEFILE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.ValidatePathFile) pre = PythonImport.VALIDATEPATHFILE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.CreateFile) pre = PythonImport.CREATEFILE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.FileExists) pre = PythonImport.FILEEXISTS + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.DeleteFile) pre = PythonImport.DELETEFILE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.IsNumber) pre = PythonImport.ISNUMBER + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.Is64Bit) pre = PythonImport.IS64BIT + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.Clamp) pre = PythonImport.CLAMP + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.Average) pre = PythonImport.AVERAGE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.KeysPressed) pre = PythonImport.KEYSPRESSED + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.MouseButton) pre = PythonImport.MOUSEBUTTON + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.WheelState) pre = PythonImport.WHEELSTATE + Environment.NewLine + Environment.NewLine + pre;
                    if (PythonImport.Window) pre = PythonImport.WINDOW + Environment.NewLine + Environment.NewLine + pre;

                    if (PythonImport.Init) pre = PythonImport.INIT + Environment.NewLine + Environment.NewLine + pre;
                    if (pre != "" && pre != "ezcode_conversion = EZCodeConversion()" + Environment.NewLine)
                    {
                        string[] p = pre.Split(Environment.NewLine);
                        pre = "class EZCodeConversion:" + Environment.NewLine; ;
                        for (int i = 0; i < p.Length; i++)
                        {
                            pre += (p[i] == "ezcode_conversion = EZCodeConversion()" ? "" : "    ") + p[i] + Environment.NewLine;
                        }
                    }
                    pre = Environment.NewLine + pre;
                }
                if (PythonImport.Sys) pre = "import sys" + Environment.NewLine + pre;
                if (PythonImport.OS) pre = "import os" + Environment.NewLine + pre;
                if (PythonImport.Time) pre = "import time" + Environment.NewLine + pre;
                if (PythonImport.DateTime) pre = "import datetime" + Environment.NewLine + pre;
                if (PythonImport.Random) pre = "import random" + Environment.NewLine + pre;
                if (PythonImport.Socket) pre = "import socket" + Environment.NewLine + pre;
                if (PythonImport.Platform) pre = "import platform" + Environment.NewLine + pre;
                if (PythonImport.Math) pre = "import math" + Environment.NewLine + pre;
                if (PythonImport.Threading) pre = "import threading" + Environment.NewLine + pre;
                if (PythonImport.Enum) pre = "from enum import Enum" + Environment.NewLine + pre;
                if (PythonImport.Keyboard) pre = "import keyboard # use 'pip install keyboard' to install this module" + Environment.NewLine + pre;
                if (PythonImport.Pyatuogui) pre = "import pyautogui # use 'pip install pyautogui' to install this module" + Environment.NewLine + pre;
                if (PythonImport.Pynput) pre = "import pynput # use 'pip install pynput' to install this module" + Environment.NewLine + pre;
                if (PythonImport.TKinter) pre = "import tkinter # use 'pip install tkinter' to install this module" + Environment.NewLine + pre;
            }
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
                        PythonImport.OS = true;
                        line = "os.system('cls')";
                        break;
                    case Actions.ClearConditional:
                        PythonImport.OS = true;
                        line = $"if {returnBoolean(w, 1)}: os.system('cls')";
                        break;
                    case Actions.AwaitMiliseconds:
                        PythonImport.Time = true;
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
                        PythonImport.Time = true;
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
                        PythonImport.ReadFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.read_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.WriteFile:
                        PythonImport.WriteFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.write_file({returnValue(w[2])}, {returnValue(w, 3)})";
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
                            PythonImport.ValidatePathFile = true;
                        }
                        line = $"ezcode_conversion.is_valid_path({returnValue(w, 2)})";
                        break;
                    case Actions.CreateFile:
                        PythonImport.CreateFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.create_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.ExistsFile:
                        PythonImport.FileExists = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.file_exists({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.DeleteFile:
                        PythonImport.DeleteFile = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.delete_file({returnValue(w, 2)})";
                        }
                        break;
                    case Actions.StopAll:
                        PythonImport.OS = true;
                        line = $"ezcode_conversion.delete_file({returnValue(w, 2)})";
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
                        PythonImport.ContainsMethod = true;
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
                        PythonImport.KeysPressed = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.get_all_keys()";
                        }
                        break;
                    case Actions.SpecificKeyInput:
                        PythonImport.KeysPressed = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.get_specific_key({returnValue(w[2])})";
                        }
                        break;
                    case Actions.MousePositionInput:
                        PythonImport.Pyatuogui = true;
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
                        PythonImport.Pyatuogui = true;
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
                        PythonImport.WheelState = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.wheel_state{(Actions.RawMouseWheelInput == action ? "_raw" : "")}";
                        }
                        break;
                    case Actions.MouseButtonInput:
                        PythonImport.MouseButton = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.get_all_mouse_buttons()";
                        }
                        break;
                    case Actions.SpecificMouseButtonInput:
                        PythonImport.MouseButton = true;
                        if (line.Contains(" => ") || line.Contains(" : "))
                        {
                            string nl = $"var {line.Split(" ")[line.Split(" ").Length - 1]} : {string.Join(" ", line.Split(" ").TakeWhile(x => x != ":" && x != "=>"))}";
                            nl = ConvertLinePython(Actions.CreateVariable, nl, objects, ref _t);
                            line = nl;
                        }
                        else
                        {
                            line = $"ezcode_conversion.get_specific_mouse_button({returnValue(w[3])})";
                        }
                        break;
                    case Actions.NewWindow:
                        PythonImport.Window = true;
                        PythonImport.firstWindow = PythonImport.firstWindow == "" ? w[1] : "";
                        line = $"{w[1]} = ezcode_conversion.Window()";
                        string change = returnProperties(w, false);
                        line += Environment.NewLine + change;
                        break;
                    case Actions.ChangeWindow:
                        line = returnProperties(w, false);
                        break;
                    case Actions.OpenWindow:
                        line = $"{w[1]}.open()";
                        break;
                    case Actions.CloseWindow:
                        line = $"{w[1]}.close()";
                        break;
                    case Actions.DestroyWindow:
                        line = $"""
                        {w[1]}.destroy()
                        {w[1]} = None
                        """;
                        break;
                    case Actions.ClearWindow:
                        line = $"{w[1]}.clear()";
                        break;
                    case Actions.DisplayWindow:
                        line = $"{w[1]}.display_controls({w[3]})";
                        break;
                    case Actions.CreateControl:
                        line = $"{w[1]} = ezcode_conversion.Window.Control(control_type=EZCodeConversion.Window.Control.Type.{w[0].FisrtLetterToUpper()})" + Environment.NewLine + returnProperties(w, true);
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
        private string returnProperties(string[] w, bool control)
        {
            string change = "";
            string propertyString = string.Join(" ", w.Skip(control ? 2 : 4));
            string[] properties = propertyString.Split(",").Select(x => x.Trim()).ToArray();
            change = $"{w[1]}.change(";
            string _default = "";
            foreach (string property in properties)
            {
                string[] splitProp = property.Split(":");
                string name = splitProp[0].ToLower();
                string value = splitProp[1];
                switch (name)
                {
                    case "x":
                    case "y":
                        change += name + "=" + (value == "0" && !control ? "1" : control && value == "-1" ? "-2" : value);
                        break;
                    case "w":
                    case "width":
                        change += "width=" + (value == "600" ? "601" : control && value == "-1" ? "-2" : value);
                        break;
                    case "h":
                    case "height":
                        change += "height=" + (value == "400" ? "401" : control && value == "-1" ? "-2" : value);
                        break;
                    case "t":
                    case "text":
                        change += $"title=\"{(value == "" ? " " : value)}\"";
                        break;
                    case "icon":
                        change += $"icon={returnValue(value.Split(" "), 0)}";
                        break;
                    default:
                        if (_default == "") _default += "|";
                        _default += $" can not convert '{name}' |";
                        break;
                }
                change += $", ";
            }
            change += $"{(_default == "" ? "" : $"default=\"{_default}\"")}";
            change = change.Replace(", , , ", ", ").Replace(", , ", ", ");
            if (change.EndsWith(", ")) change = change.Remove(change.Length - 2, 2);
            change += ")";
            return change;
        }
        private string returnMath(string value)
        {
            if (!value.EndsWith(")") && System.Text.RegularExpressions.Regex.Matches(value, @"\)").Count == 1 && System.Text.RegularExpressions.Regex.Matches(value, @"\(").Count == 1)
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
                PythonImport.Math = true;
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
                PythonImport.Clamp = true;

                string eq = value.Replace("clamp(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");

                float ineq1 = float.Parse(eqboth[0].Trim());
                float ineq2 = float.Parse(eqboth[1].Trim());
                float ineq3 = float.Parse(eqboth[2].Trim());
                return $"ezcode_conversion.clamp({ineq1}, {ineq2}, {ineq3})";
            }
            else if (value.StartsWith("sum("))
            {
                string eq = value.Replace("sum(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"sum({string.Join(", ", eqboth)})";
            }
            else if (value.StartsWith("avg("))
            {
                PythonImport.Average = true;
                string eq = value.Replace("avg(", "").Replace(")", "");
                string[] eqboth = eq.Split(",");
                return $"ezcode_conversion.average({string.Join(", ", eqboth)})";
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

            NewWindow,
            ChangeWindow,
            ClearWindow,
            CloseWindow,
            OpenWindow,
            DisplayWindow,
            DestroyWindow,
            CreateControl,
            Destroy,
            DestroyConditional,
            Intersects,
            BringToFront,
            BringToBack,
            NewGroup,
            AddGroup,
            EqualsGroup,
            RemoveGroup,
            ClearGroup,
            DestroyGroup,
            DestroyAllGroup,
            ChangeGroup,
            StopAllSound,
            VolumeSound,
            NewSound,
            PlaySound,
            PlayLoopSound,
            DestroySound,
            StopSound,
            FontEvent,
            TextEvent,
            ClickEvent,
            HoverEvent,
            MoveEvent,
            BackColorEvent,
            ForeColorEvent,
            ImageEvent,
            ImageLayoutEvent,
            FocusedEvent,
            ControlAddedEvent,
            ControlRemovedEvent,
            DefocusedEvent,
            CloseEvent,
            OpenEvent,
            EnabledChangedEvent,
            KeyDownEvent,
            KeyUpEvent,
            KeyPressEvent,
            ResizeEvent,
            ResizeStartEvent,
            ResizeEndEvent,
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

        public static string FisrtLetterToUpper(this string input)
        {
            string result = "";

            if (!string.IsNullOrEmpty(input))
            {
                result = char.ToUpper(input[0]) + input.Substring(1);
            }
            return result;
        }
        private static string ColonResponse(string value, Converter converter)
        {
            string[] ind = value.Split(':');
            if (value.StartsWith("system:"))
            {
                switch (ind[1])
                {
                    case "time":
                        converter.PythonImport.DateTime = true;
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
                                converter.PythonImport.Random = true;
                                bool more = value.Contains("system:random:");
                                if (more)
                                {
                                    if (converter.Language == Converter.ProgrammingLanguage.Python)
                                        return $"random.randint({ind[2]}, {ind[3]})";
                                    else return "";
                                }
                                else
                                {
                                    converter.PythonImport.Sys = true;
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
                        converter.PythonImport.IsNumber = true;
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return $"ezcode_conversion.is_number({converter.returnValue(ind, 2, false)})";
                        else return "";
                    case "machine":
                        switch (ind[2].ToLower())
                        {
                            case "machinename":
                                converter.PythonImport.Socket = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "socket.gethostname()";
                                else return "";
                            case "osversion":
                                converter.PythonImport.Platform = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "platform.platform()";
                                else return "";
                            case "is64bitoperatingsystem":
                                converter.PythonImport.Is64Bit = true;
                                if (converter.Language == Converter.ProgrammingLanguage.Python)
                                    return "ezcode_conversion.is_windows_64bit()";
                                else return "";
                            case "username":
                                converter.PythonImport.OS = true;
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
                        converter.PythonImport.OS = true;
                        if (converter.Language == Converter.ProgrammingLanguage.Python)
                            return "os.path.dirname(os.path.realpath(__file__))";
                        else return "";
                    case "currentplaydirectory":
                        converter.PythonImport.OS = true;
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
