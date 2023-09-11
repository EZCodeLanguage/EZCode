﻿using EZCode.GControls;
using EZCode.Variables;
using EZCode.Windows;
using NCalc;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Group = EZCode.Groups.Group;
using Player = Sound.Player;
using Types = EZCode.Variables.Ivar.Types;

namespace EZCode
{
    /// <summary>
    /// This is the Official EZCode Source Code. See Version '<seealso cref="Version"/>'
    /// </summary>
    public class EzCode
    {
        #region Variables_and_Initializers
        /// <summary>
        /// Directory of the script playing
        /// </summary>
        public string Version { get; } = "2.0.0";
        /// <summary>
        /// Directory of the script playing
        /// </summary>
        public string ScriptDirectory { get; set; }
        /// <summary>
        /// Console input's bool to send
        /// </summary>
        private bool sent;
        /// <summary>
        /// Text sent by the console's input
        /// </summary>
        private string senttext = "";
        /// <summary>
        /// Bool to decide if a key is down
        /// </summary>
        public HashSet<MouseButtons> mouseButtons = new HashSet<MouseButtons>();
        /// <summary>
        /// Position of the mouse
        /// </summary>
        public Point MousePosition = new Point();
        /// <summary>
        /// Delta of the Mouse Wheel
        /// </summary>
        public int mouseWheel
        {
            get
            {
                int a = _mouseWheel;
                _mouseWheel = 0;
                return a;
            }
            set
            {
                _mouseWheel = value;
            }
        }
        int _mouseWheel;
        /// <summary>
        /// The output color of an error
        /// </summary>
        public Color errorColor;
        /// <summary>
        /// The normal output color
        /// </summary>
        public Color normalColor;
        /// <summary>
        /// List for Audio output players
        /// </summary>
        private List<Player> sounds = new List<Player>();
        /// <summary>
        /// List for Labels
        /// </summary>
        private List<GLabel> labels = new List<GLabel>();
        /// <summary>
        /// List for textboxes
        /// </summary>
        private List<GTextBox> textboxes = new List<GTextBox>();
        /// <summary>
        /// List for buttons
        /// </summary>
        private List<GButton> buttons = new List<GButton>();
        /// <summary>
        /// List for gameobjects
        /// </summary>
        private List<GShape> shapes = new List<GShape>();
        /// <summary>
        /// List for windows
        /// </summary>
        private List<Window> windows = new List<Window>();
        /// <summary>
        /// List for variables
        /// </summary>
        private List<Var> vars = new List<Var>();
        /// <summary>
        /// List of Objects
        /// </summary>
        private List<object> objects = new List<object>();
        /// <summary>
        /// List of Groups
        /// </summary>
        private List<Group> groups = new List<Group>();
        /// <summary>
        /// List of all controls
        /// </summary>
        public List<Control> AllControls
        {
            get
            {
                List<Control> c = new List<Control>();
                c.AddRange(shapes);
                c.AddRange(buttons);
                c.AddRange(labels);
                c.AddRange(textboxes);
                return c;
            }
        }
        /// <summary>
        /// Shows the file directory whenever an error occurs. Recommended for debugging.
        /// </summary>
        public bool showFileInError = true;
        /// <summary>
        /// Shows the 'Build Started' and 'Build Ended' console values.
        /// </summary>
        public bool showStartAndEnd = true;
        /// <summary>
        /// Clear the console after each build
        /// </summary>
        public bool ClearConsole = true;
        /// <summary>
        /// Clear the Visual output after the build
        /// </summary>
        public bool ClearAfterBuild = true;
        int _codeLine;
        /// <summary>
        /// Current line that is being executed
        /// </summary>
        public int codeLine
        {
            get
            {
                return _codeLine;
            }
            set
            {
                _codeLine = value;
            }
        }
        /// <summary>
        /// RichTextbox for the console
        /// </summary>
        private RichTextBox RichConsole;

        string _ConsoleText;
        /// <summary>
        /// The output console
        /// </summary>
        public string ConsoleText
        {
            get
            {
                return _ConsoleText;
            }
            set
            {
                _ConsoleText = value;
            }
        }
        /// <summary>
        /// The output space. usually a panel or picturebox
        /// </summary>
        Control Space { get; set; }
        /// <summary>
        /// Text to be executed
        /// </summary>
        string Code { get; set; }
        /// <summary>
        /// Bool to decide if the script is playing
        /// </summary>
        public bool playing
        {
            get
            {
                return _pplaying;
            }
            private set
            {
                _pplaying = value;
                returnOutput = "";
                devDisplay = true;
                devportal = 0;
                ifmany = 0;
                lastif = true;
                loopmany = 0;
                foreach (Window Window in windows)
                {
                    Window.Close();
                }
                if (_pplaying && showStartAndEnd)
                    AddText("Build Start");
                else if(!_pplaying && showStartAndEnd)
                    AddText("Build Ended");
            }
        }
        private bool _pplaying;
        /// <summary>
        /// string array for naming violations
        /// </summary>
        public string[] UnusableNames = new string[] { "await", "button", "print", "group", "clear", "write", "stop", "DEVPORTAL",
            "event", "textbox", "multiLine", "shape", "image", "label", "font", "move", "scale", "color", "intersects", "var", "if",
            "input", "list", "file", "sound", "if", "//", "#create", "#suppress", "#", "system:", "?", "=", "!", ">", "<", "+", "loop",
            "-", "|", "\\", ",", "@", "#", "$", "%", "^", "&", "*", "(", ")", "/", "~", "`", ".", ":", ";", "window", "system", "messagebox" };
        /// <summary>
        /// char array for unusable names that can't even be used once in the name
        /// </summary>
        public char[] UnusableContains = new char[] { '?', '=', '!', ':', '>', '<', '|', '\\', '#', '(', ')', '&', '^', '*', '+', '-', '/', '{', '}' };
        /// <summary>
        /// The character tht seperates each line of code. Automatically { '\n', '|' } but this can be added to if needed 
        /// </summary>
        public char[] seperatorChars = new char[] { '\n', '|' };
        /// <summary>
        /// List of keys being pressed
        /// Needs to have Key_Down and Key_Up event connected to KeyInput_Down and KeyInput_Up
        /// </summary>
        public HashSet<Keys> Keys = new HashSet<Keys>();

        private string SegmentSeperator = "segment";
        /// <summary>
        /// Refreshes screen when a control is changed/created
        /// </summary>
        public bool RefreshOnControl = false;

        public bool InPanel { get; set; }

        /// <summary>
        /// Initializes the EZCode Player with the provided parameters.
        /// </summary>
        /// <param name="_space">The Control used as the output space. Only needed if the code includes visual output, like 'object' or 'button'</param>
        /// <param name="_directory">The directory path where the file is located. Only needed if the code referenses another file locally using the '~/' character.</param>
        /// <param name="_console">The RichTextbox that has the error color is wanted.</param>
        public void Initialize(bool inpanel = false, string _directory = "NOTHING", Control _space = null, RichTextBox _console = null, bool _showFileWithErrors = true, bool _showStartAndEnd = true, bool _clearConsole = true)
        {
            InPanel = inpanel;
            Space = _space;
            RichConsole = _console;
            ScriptDirectory = _directory != "NOTHING" ? _directory : "";
            showFileInError = _showFileWithErrors;
            showStartAndEnd = _showStartAndEnd;
            ClearConsole = _clearConsole;
        }
        /// <summary>
        /// Enum for the different Controls
        /// </summary>
        enum controlType
        {
            None,
            Shape,
            Textbox,
            GLabel,
            GButton
        }
        /// <summary>
        /// Plays EZCode code. Make sure you have initialized first.
        /// </summary>
        /// <param name="code">The string representing the EZCode content to be played.</param>
        public async Task<string> Play(string code)
        {
            if (playing) return "";
            playing = true;
            AllControls.Clear();
            Space.Controls.Clear();
            vars.Clear();
            groups.Clear();
            shapes.Clear();
            labels.Clear();
            textboxes.Clear();
            buttons.Clear();
            windows.Clear();
            objects.Clear();
            if (ClearConsole) RichConsole.Clear();
            if (showStartAndEnd) AddText("Build Started");
            Code = code;
            string output = string.Empty;
            List<string> lines = code.Split(seperatorChars).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (!playing) return output;
                codeLine = i + 1;
                List<string> parts = lines[i].Split(new char[] { ' ' }).ToList();
                for (int j = 0; j < parts.Count; j++)
                {
                    if (parts[j].Trim() == "->")
                    {
                        try
                        {
                            parts.RemoveAt(j);
                            parts.AddRange(lines[i + 1].Split(' '));
                            lines.RemoveAt(i + 1);
                        }
                        catch
                        {

                        }
                    }
                }
                string[] task = await PlaySwitch(parts.ToArray(), "", lines.ToArray(), i);
                if (bool.Parse(task[1]) == false) i = lines.Count - 1;
                output += task[0];
                ConsoleText = output;
            }
            playing = false;
            StopAllSounds();
            return output;
        }

        #endregion

        #region EZCode_Script_Player
        string returnOutput = "";
        bool devDisplay = true, lastif = true;
        int devportal = 0, ifmany = 0, loopmany = 0;
        async Task<string[]> PlaySwitch(string[]? _parts = null, string jumpsto = "", string[]? splitcode = null, int currentindex = 0)
        {
            try
            {
                foreach (GButton button in buttons) button.isclick = 0;
                if (ifmany > 0 || loopmany > 0)
                {
                    if (ifmany > 0) ifmany--;
                    if (loopmany > 0) loopmany--;
                    return new[] { returnOutput, "true" };
                }
                string[]? parts = _parts;
                parts = parts == null ? parts : parts.Where(x => x != "").ToArray();
                parts = parts != null ? parts : jumpsto.Split(new char[] { ' ' });
                _parts ??= parts;
                _parts = _parts != null ? _parts.Select(part => string.IsNullOrEmpty(part) ? " " : part).ToArray() : _parts;
                parts ??= new string[] { };
                bool stillInFile = true;
                returnOutput = string.Empty;
                parts = parts.Select(x => x.Trim()).ToArray();
                string keyword = parts.Length == 0 ? "" : parts != null ? parts[0] : jumpsto.Split(new char[] { ' ' })[0];
                bool jumpTo = jumpsto == "" ? false : true;
                switch (keyword)
                {
                    case "window":
                        try
                        {
                            DoWindow(parts, 1, keyword);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    case "loop":
                        try
                        {
                            int looptime = -1;
                            bool? loop = null;

                            loop = BoolCheck(parts, 1, false);
                            if (loop == null)
                            {
                                looptime = (int)find_value(parts, 1, 0)[0];
                            }

                            List<string> everythingafter = new List<string>();
                            everythingafter.AddRange(splitcode.Skip(currentindex));
                            everythingafter = everythingafter.Select(x => x.Trim()).ToList();
                            string[] returnned = ExtractContent(everythingafter.ToArray(), parts);
                            
                            int tr = 0;
                            int oldcodeline = codeLine;
                            while (looptime != -1 ? tr < looptime : loop == true && playing)
                            {
                                tr++;

                                int changeable = oldcodeline;
                                string[] lines = returnned;
                                for (int i = 0; i < lines.Length; i++)
                                {

                                    if (!playing) { i = lines.Length; continue; };
                                    changeable = oldcodeline + i;
                                    codeLine = changeable;
                                    string[] task = await PlaySwitch(lines[i].Split(new char[] { ' ' }), "", lines, i);
                                    if (bool.Parse(task[1]) == false) i = lines.Length - 1;
                                }

                                loop = BoolCheck(parts, 1, false);
                            }

                            codeLine = oldcodeline;
                            loopmany = returnned.Length - 1;
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    case "else":
                        try
                        {
                            bool containscolon = parts[1] == ":";
                            if(containscolon)
                            {
                                string[] wholearray = parts.ToArray();
                                string[] after = string.Join(" ", wholearray).Split(" : ");
                                string code = after.Length == 2 ? after[1].Trim() : "";
                                int indexof = Array.IndexOf(wholearray, ":");
                                bool anythingafter = indexof != wholearray.Length - 1;
                                bool startswithbracket = code.StartsWith("{");
                                List<string> everythingafter = new List<string>();
                                everythingafter.AddRange(splitcode.Skip(currentindex));
                                everythingafter = everythingafter.Select(x => x.Trim()).ToList();
                                string nextline = "";
                                for (int i = 1; i < everythingafter.Count && nextline == ""; i++) if (everythingafter[i] != "") nextline = everythingafter[i];
                                bool inline = anythingafter && !startswithbracket;
                                bool onnextline = !inline && !startswithbracket && !nextline.Trim().StartsWith("{") && nextline != "";
                                if (inline) // execute line after if
                                {
                                    if (!lastif) //TRUE
                                    {
                                        string[] result = await PlaySwitch(code.Split(" "));
                                    }
                                    else //FALSE
                                    {
                                        // Nothing
                                    }
                                }
                                else if (onnextline) // execute next line
                                {
                                    if (!lastif) //TRUE
                                    {
                                        // Nothing
                                    }
                                    else //FALSE
                                    {
                                        ifmany = 1;
                                    }
                                }
                                else // execute everything inside { }
                                {
                                    string[] returnned = ExtractContent(everythingafter.ToArray(), parts);
                                    if (!lastif) //TRUE
                                    {
                                        // Nothing
                                    }
                                    else //FALSE
                                    {
                                        ifmany = returnned.Length - (startswithbracket ? 2 : 1);
                                    }
                                }
                            }
                            else
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' for '{keyword}' in {SegmentSeperator} {codeLine}");
                            }
                            lastif = true;
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    case "if":
                        try
                        {
                            bool check = false;
                            string[] QMarkValuesArray = getString_value(parts, 1, true, true, false, true);
                            string[] wholearray = QMarkValuesArray[0].Split(" ");
                            string[] arrayed = wholearray.TakeWhile(x => x != ":").ToArray();
                            int number = 0;
                            for (int i = 0; i < wholearray.Length; i++) number = wholearray[i].Equals(":") ? number + 1 : number;
                            if (number == 1)
                            {
                                for (int i = 0; i < arrayed.Length; i++)
                                {
                                    try
                                    {
                                        // Try Convert to number
                                        float[] floats = find_value(arrayed, i, 0);
                                        arrayed[i] = floats[0].ToString();
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            // Try Convert to string
                                            string[] strings = getString_value(arrayed, i);
                                            arrayed[i] = strings[0];
                                        }
                                        catch
                                        {
                                            // Nothing
                                        }
                                    }
                                    if (Var.staticReturnBool(arrayed[i]) != null)
                                    {
                                        arrayed[i] = Var.staticReturnBool(arrayed[i]).ToString();
                                    }
                                }
                                string values = string.Join(" ", arrayed).ToLower();
                                check = IfCheck(values);
                            }
                            else if (number == 0)
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' for '{keyword}' in {SegmentSeperator} {codeLine}");
                            }
                            else if (number > 1)
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected only one ':' for '{keyword}' in {SegmentSeperator} {codeLine}");
                            }
                            wholearray = wholearray.Where(x => x != "").ToArray();
                            string[] after = string.Join(" ", wholearray).Split(" : ");
                            string code = after.Length == 2 ? after[1].Trim() : "";
                            int indexof = Array.IndexOf(wholearray, ":");
                            bool anythingafter = indexof != wholearray.Length - 1;
                            bool startswithbracket = code.StartsWith("{");
                            List<string> everythingafter = new List<string>();
                            everythingafter.AddRange(splitcode.Skip(currentindex));
                            everythingafter = everythingafter.Select(x => x.Trim()).ToList();
                            string nextline = "";
                            for (int i = 1; i < everythingafter.Count && nextline == ""; i++) if(everythingafter[i] != "") nextline = everythingafter[i];
                            bool inline = anythingafter && !startswithbracket;
                            bool onnextline = !inline && !startswithbracket && !nextline.Trim().StartsWith("{") && nextline != "";
                            if (inline) // execute line after if
                            {
                                if (check) //TRUE
                                {
                                    string[] result = await PlaySwitch(code.Split(" "));
                                }
                                else //FALSE
                                {
                                    // Nothing
                                }
                            }
                            else if (onnextline) // execute next line
                            {
                                if (check) //TRUE
                                {
                                    // Nothing
                                }
                                else //FALSE
                                {
                                    ifmany = 1;
                                }
                            }
                            else // execute everything inside { }
                            {
                                string[] returnned = ExtractContent(everythingafter.ToArray(), parts);
                                if (check) //TRUE
                                {
                                    // Nothing
                                }
                                else //FALSE
                                {
                                    ifmany = returnned.Length - (startswithbracket ? 2 : 1);
                                }
                            }
                            lastif = check;
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    case "messagebox":
                        try
                        {
                            string[] title = getString_value(parts, 1);
                            string text = getString_value(parts, int.Parse(title[1]))[0];
                            MessageBox.Show(text, title[0]);                            
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // MESSAGEBOX
                        break;
                    case "print":
                        try
                        {
                            string text = getString_value(parts, 1, true)[0];
                            AddText(text, false);
                            returnOutput = text;
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // PRINT
                        break;
                    case "DEVPORTAL":
                        try
                        {
                            devportal = devportal == 0 ? 1 : 2;
                            string output = "";
                            string next = parts[1];
                            if (devportal == 1)
                            {
                                output = "Entered into Devportal";
                                if (next != "!display") output += "\n";
                            }
                            if (next == "display")
                            {
                                devDisplay = true;
                                output += "Set Dev Portal Display to true";
                            }
                            if (next == "!display")
                            {
                                devDisplay = false;
                            }
                            if (next == "refreshonchange")
                            {
                                RefreshOnControl = true;
                                output += "Set Dev Portal Refresh on Update to true";
                            }
                            if (next == "!refreshonchange")
                            {
                                RefreshOnControl = false;
                                output += "Set Dev Portal Refresh on Update to false";
                            }
                            if (next == "seperatingchars")
                            {
                                string addedchar = parts[2];
                                List<char> each = seperatorChars.ToList();
                                foreach (char c in addedchar)
                                {
                                    each.Add(c);
                                }
                                seperatorChars = each.ToArray();
                            }
                            if (next == "resetseperatingchars")
                            {
                                seperatorChars = new char[] { '\n', '|' };
                            }
                            if (devDisplay)
                            {
                                AddText(output);
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    case "shape":
                    case "label":
                    case "textbox":
                    case "button":
                        try
                        {
                            await DoControl(parts, keyword, 1);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // CONTROL
                        break;
                    case "clear":
                        try
                        {
                            if (parts.Length - 1 == 0 || (parts.Length - 1 > 0 && parts[1] == ""))
                            {
                                ConsoleText = string.Empty;
                                RichConsole.Clear();
                            }
                            else if ((bool)BoolCheck(parts, 1))
                            {
                                ConsoleText = string.Empty;
                                RichConsole.Clear();
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // CLEAR
                        break;
                    case "destroy":
                        try
                        {
                            string name = parts[1];
                            Control? control = getControl(name);
                            if (control == null)
                            {
                                ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                            }
                            if (parts.Length - 1 >= 2 && BoolCheck(parts, 2) == false)
                            {
                                return new string[] { returnOutput, "true" };
                            }
                            control.Dispose();
                            switch (control.AccessibleName)
                            {
                                case "shape":
                                    shapes.Remove(control as GShape);
                                    break;
                                case "button":
                                    buttons.Remove(control as GButton);
                                    break;
                                case "label":
                                    labels.Remove(control as GLabel);
                                    break;
                                case "textbox":
                                    textboxes.Remove(control as GTextBox);
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                                    break;
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // DESTROY
                        break;
                    case "await":
                        try
                        {
                            if (IsNumericString(parts[1].Trim()))
                            {
                                float[] values = find_value(parts, 1, 0);
                                await Task.Delay((int)values[0]);
                            }
                            else
                            {
                                bool check = BoolCheck(parts, 1) == true;
                                while (!check && playing)
                                {
                                    check = BoolCheck(parts, 1) == true;
                                    await Task.Delay(100);
                                }
                            }
                        }
                        catch
                        {
                            AddText($"An error occured with '{keyword}' in {SegmentSeperator} {codeLine}", true);
                        } // AWAIT
                        break;
                    case "var":
                        try
                        {
                            Var var = await CreateVar(parts, allowJump: true);
                            vars.Add(var);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // VAR
                        break;
                    case "intersects":
                        try
                        {
                            Control? get_1 = getControl(parts[1].Trim(), controlType.None);
                            Control? get_2 = getControl(parts[2].Trim(), controlType.None);

                            if (get_1 == null)
                            {
                                ErrorText(parts, ErrorTypes.missingControl, keyword, parts[1]);
                            }
                            if (get_2 == null)
                            {
                                ErrorText(parts, ErrorTypes.missingControl, keyword, parts[2]);
                            }

                            Rectangle rect1 = new Rectangle();
                            Rectangle rect2 = new Rectangle();

                            switch (get_1.AccessibleName)
                            {
                                case "shape":
                                    rect1 = get_1.Bounds;
                                    break;
                                case "button":
                                    rect1 = get_1.Bounds;
                                    break;
                                case "label":
                                    rect1 = get_1.Bounds;
                                    break;
                                case "textbox":
                                    rect1 = get_1.Bounds;
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.missingControl, keyword, parts[1]);
                                    break;
                            }
                            switch (get_2.AccessibleName)
                            {
                                case "shape":
                                    rect2 = get_2.Bounds;
                                    break;
                                case "button":
                                    rect2 = get_2.Bounds;
                                    break;
                                case "label":
                                    rect2 = get_2.Bounds;
                                    break;
                                case "textbox":
                                    rect2 = get_2.Bounds;
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.missingControl, keyword, parts[2]);
                                    break;
                            }
                            string intersects = (rect1.IntersectsWith(rect2) == false ? 0 : 1).ToString();
                            if (jumpTo)
                            {
                                return new string[] { intersects, stillInFile.ToString() };
                            }
                            returnOutput += SetVKeyword(parts, 3, keyword, intersects, Types.Bool);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // INTERSECTS
                        break;
                    case "file":
                        try
                        {
                            string output = "";
                            string type = parts[1].Trim();
                            int endindex = 0;
                            switch (type)
                            {
                                case "read":
                                    string[] file_r = await getFile(parts, 2);
                                    if (!File.Exists(file_r[0])) throw new Exception($"File not found in {SegmentSeperator} {codeLine}");
                                    output = File.ReadAllText(file_r[0]);
                                    endindex = int.Parse(file_r[1]);
                                    break;
                                case "write":
                                    string stuff = getString_value(parts, 2)[0];
                                    string[] file_w = await getFile(parts, 3, 2);
                                    bool success = false;
                                    try
                                    {
                                        string direc = Path.GetDirectoryName(file_w[0]);
                                        if (!Directory.Exists(direc)) Directory.CreateDirectory(direc);
                                        File.WriteAllText(file_w[0], stuff);
                                        success = true;
                                    }
                                    catch
                                    {
                                        success = false;
                                    }
                                    endindex = int.Parse(file_w[1]);
                                    output = success == true ? "1" : "0";
                                    break;
                                case "path":
                                    string[] _strings = await getFile(_parts, parts.ToList().IndexOf("file") + 2);
                                    output = _strings[0];
                                    endindex = parts.ToList().IndexOf(_strings[0].Split(" ")[_strings[0].Split(" ").Length - 1]);
                                    if (!validpathcheck(output))
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"The path given is invalid for '{keyword}' in {SegmentSeperator} {codeLine}");
                                    }
                                    break;
                                case "play":
                                    string[] file_p = await getFile(parts, 2);
                                    if (!File.Exists(file_p[0])) throw new Exception($"File not found in {SegmentSeperator} {codeLine}");
                                    string code = File.ReadAllText(file_p[0]);
                                    endindex = int.Parse(file_p[1]);
                                    string[] lines = code.Split(seperatorChars);
                                    string tempscript = ScriptDirectory;
                                    int templine = codeLine;
                                    ScriptDirectory = file_p[0];
                                    for (int i = 0; i < lines.Length; i++)
                                    {
                                        if (!playing) continue;
                                        codeLine = i + 1;
                                        string[] task = await PlaySwitch(lines[i].Split(new char[] { ' ' }), "", lines, 0);
                                        if (bool.Parse(task[1]) == false) i = lines.Length - 1;
                                        output += task[0];
                                    }
                                    ScriptDirectory = tempscript;
                                    codeLine = templine;
                                    if (jumpTo)
                                    {
                                        return new string[] { output, stillInFile.ToString() };
                                    }
                                    break;
                                case "create":
                                    {
                                        string[] file_ = await getFile(parts, 2);
                                        if (File.Exists(file_[0])) throw new Exception($"File already exists in {SegmentSeperator} {codeLine}");
                                        File.Create(file_[0]).Close();
                                        output = File.Exists(file_[0]) ? "1" : "0";
                                        endindex = int.Parse(file_[1]);
                                    }
                                    break;
                                case "delete":
                                    {
                                        string[] file_ = await getFile(parts, 2);
                                        if (!File.Exists(file_[0])) throw new Exception($"File not found in {SegmentSeperator} {codeLine}");
                                        File.Delete(file_[0]);
                                        output = !File.Exists(file_[0]) ? "1" : "0";
                                        endindex = int.Parse(file_[1]);
                                    }
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'read' 'write' 'path' or 'play' in {SegmentSeperator} {codeLine}");
                                    break;
                            }
                            if (jumpTo) return new string[] { output, stillInFile.ToString() };
                            returnOutput += SetVKeyword(parts, endindex, keyword, output, Types.File);
                        }
                        catch (Exception ex)
                        {
                            if(ex.Message == $"File not found in {SegmentSeperator} {codeLine}") ErrorText(parts, ErrorTypes.custom, custom: ex.Message);
                            else ErrorText(parts, ErrorTypes.normal, keyword);
                        } // FILE
                        break;
                    case "stop":
                        try
                        {
                            string type = parts[1].Trim();
                            if (parts.Length - 1 >= 2 && !(bool)BoolCheck(parts, 2))
                            {
                                return new string[] { returnOutput, "true" };
                            }
                            switch (type)
                            {
                                case "all":
                                    playing = false;
                                    break;
                                case "file":
                                    stillInFile = false;
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'all' or 'file' in {SegmentSeperator} {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // STOP
                        break;
                    case "input":
                        try
                        {
                            string type = parts[1].Trim();
                            string output = "";
                            Types des = Types.None;
                            int index = 0;
                            switch (type)
                            {
                                case "console":
                                    if (!jumpTo)
                                    {
                                        while (!sent && playing)
                                        {
                                            await Task.Delay(200);
                                        }
                                    }
                                    output = senttext;
                                    index = 2;
                                    break;
                                case "key":
                                    switch (parts.Length - 1 < 2 ? "" : parts[2].Trim())
                                    {
                                        case "":
                                            output = string.Join(",", Keys);
                                            break;
                                        default:
                                            output = Keys.Select(x => x.ToString()).FirstOrDefault(y => y == parts[2].Trim()) != null ? "1" : "0";
                                            break;
                                    }
                                    break;
                                case "mouse":
                                    switch (parts[2].Trim())
                                    {
                                        case "position":
                                            switch (parts.Length - 1 < 3 ? "" : parts[3].Trim())
                                            {
                                                case "x":
                                                case "X":
                                                    output = MousePosition.X.ToString();
                                                    break;
                                                case "y":
                                                case "Y":
                                                    output = MousePosition.Y.ToString();
                                                    break;
                                                case "":
                                                    output = $"{MousePosition.X}, {MousePosition.Y}";
                                                    break;
                                                default:
                                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'X' or 'Y' for '{keyword}' in {SegmentSeperator} {codeLine}");
                                                    break;
                                            }
                                            break;
                                        case "wheel":
                                            switch (parts.Length - 1 < 3 ? "" : parts[3].Trim().ToLower())
                                            {
                                                case "":
                                                    output = Math.Sign(mouseWheel).ToString();
                                                    break;
                                                case "raw":
                                                    output = mouseWheel.ToString();
                                                    break;
                                                default:
                                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'X' or 'Y' for '{keyword}' in {SegmentSeperator} {codeLine}");
                                                    break;
                                            }
                                            break;
                                        case "button":
                                            switch (parts.Length - 1 < 3 ? "" : parts[3].Trim())
                                            {
                                                case "":
                                                    output = string.Join(",", mouseButtons);
                                                    break;
                                                default:
                                                    output = mouseButtons.Select(x => x.ToString()).FirstOrDefault(y => y == parts[3].Trim()) != null ? "1" : "0";
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'console' 'key' or 'mouse' for '{keyword}' in {SegmentSeperator} {codeLine}");
                                    break;
                            }
                            if (jumpTo) return new string[] { output, stillInFile.ToString() };
                            SetVKeyword(parts, index, keyword, output, des);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // INPUT
                        break;
                    case "list":
                        try
                        {
                            DoList(parts, 1, keyword);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // LIST
                        break;
                    case "group":
                        try
                        {
                            DoGroup(parts, 1, keyword);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // GROUP
                        break;
                    case "sound":
                        try
                        {
                            string[] namearray = getString_value(parts, 1, false, false, true, false);
                            string name = namearray[0];
                            if(name == "stopall")
                            {
                                StopAllSounds();
                                return new string[] { returnOutput, stillInFile.ToString() };
                            }
                            string type = parts[int.Parse(namearray[1])];
                            switch (type)
                            {
                                case "new":
                                    string[] strings = await getFile(parts, 3);
                                    string filepath = strings[0];
                                    sounds.Add(new Player(name, filepath));
                                    break;
                                case "play":
                                    Player player = await GetPlayer(name);
                                    if(player.Name != "")
                                    {
                                        player.Play();
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                case "playloop":
                                    Player player_ = await GetPlayer(name);
                                    if (player_.Name != "")
                                    {
                                        player_.PlayLooping();
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                case "destroy":
                                    Player player__ = await GetPlayer(name);
                                    if (player__.Name != "")
                                    {
                                        player__.Stop();
                                        sounds.Remove(player__);
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                case "stop":
                                    Player player___ = await GetPlayer(name);
                                    if (player___.Name != "")
                                    {
                                        player___.Stop();
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                case "volume":
                                    Player player____ = await GetPlayer(name);
                                    if (player____.Name != "")
                                    {
                                        player____.Volume = find_value(parts, 3, 0)[0];
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new' 'destroy' 'play' 'stop' or 'volume' in {SegmentSeperator} {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            ErrorText(parts,ErrorTypes.normal, keyword);
                        } // SOUND
                        break;
                    case "event":
                        try
                        {
                            string name = parts[1];
                            
                            if (getControl(name) != null)
                            {
                                Control? control = getControl(name);
                                GShape gShape = new GShape();
                                GLabel gLabel = new GLabel();
                                GButton gButton = new GButton();
                                GTextBox gTextBox = new GTextBox();
                                switch (control.AccessibleName)
                                {
                                    case "shape":
                                        gShape = (control as GShape);
                                        break;
                                    case "button":
                                        gButton = (control as GButton);
                                        break;
                                    case "label":
                                        gLabel = (control as GLabel);
                                        break;
                                    case "textbox":
                                        gTextBox = (control as GTextBox);
                                        break;
                                    default:
                                        ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                                        break;
                                }
                                string type = parts[2];
                                string[] file_r = await getFile(parts, 3);
                                string file = file_r[0];
                                switch (type)
                                {
                                    case "click":
                                        gShape.click = file;
                                        gButton.click = file;
                                        gLabel.click = file;
                                        gTextBox.click = file;
                                        gShape.Click += G_click;
                                        gLabel.Click += G_click;
                                        gTextBox.Click += G_click;
                                        break;
                                    case "hover":
                                        gShape.mousehover = file;
                                        gButton.mousehover = file;
                                        gLabel.mousehover = file;
                                        gTextBox.mousehover = file;
                                        control.MouseHover += G_mousehover;
                                        break;
                                    case "move":
                                        gShape.move = file;
                                        gButton.move = file;
                                        gLabel.move = file;
                                        gTextBox.move = file;
                                        control.LocationChanged += G_move;
                                        break;
                                    case "scale":
                                        gShape.scale = file;
                                        gButton.scale = file;
                                        gLabel.scale = file;
                                        gTextBox.scale = file;
                                        control.SizeChanged += G_scale;
                                        break;
                                    case "backcolor":
                                        gShape.backcolor = file;
                                        gButton.backcolor = file;
                                        gLabel.backcolor = file;
                                        gTextBox.backcolor = file;
                                        control.BackColorChanged += G_backcolor;
                                        break;
                                    case "forecolor":
                                        gShape.forecolor = file;
                                        gButton.forecolor = file;
                                        gLabel.forecolor = file;
                                        gTextBox.forecolor = file;
                                        control.ForeColorChanged += G_forecolor;
                                        break;
                                    case "image":
                                        gShape.image = file;
                                        gButton.image = file;
                                        gLabel.image = file;
                                        gTextBox.image += file;
                                        control.BackgroundImageChanged += G_image;
                                        break;
                                    case "imagelayout":
                                        gShape.imagelayout = file;
                                        gButton.imagelayout = file;
                                        gLabel.imagelayout = file;
                                        gTextBox.imagelayout = file;
                                        control.BackgroundImageLayoutChanged += G_imagetype;
                                        break;
                                    case "font":
                                        gShape.font = file;
                                        gButton.font = file;
                                        gLabel.font = file;
                                        gTextBox.font = file;
                                        control.FontChanged += G_font;
                                        break;
                                    case "text":
                                        gShape.text = file;
                                        gButton.text = file;
                                        gLabel.text = file;
                                        gTextBox.text = file;
                                        control.TextChanged += G_text;
                                        break;
                                    default:
                                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'click' 'hover' 'move' 'scale' 'backcolor' 'forecolor' 'image' 'imagetype' 'font' or 'text' for 'event' in {SegmentSeperator} {codeLine}");
                                        break;
                                }
                            }
                            else if (getWindow(name) != null)
                            {
                                Window window = getWindow(name);
                                string type = parts[2];
                                string[] file_r = await getFile(parts, 3);
                                string file = file_r[0];
                                switch (type)
                                {
                                    case "click":
                                        window.click = file;
                                        window.MouseHover += G_click;
                                        break;
                                    case "hover":
                                        window.mousehover = file;
                                        window.MouseHover += G_mousehover;
                                        break;
                                    case "move":
                                        window.move = file;
                                        window.LocationChanged += G_move;
                                        break;
                                    case "backcolor":
                                        window.backcolor = file;
                                        window.BackColorChanged += G_backcolor;
                                        break;
                                    case "forecolor":
                                        window.forecolor = file;
                                        window.ForeColorChanged += G_forecolor;
                                        break;
                                    case "image":
                                        window.image = file;
                                        window.BackgroundImageChanged += G_image;
                                        break;
                                    case "imagelayout":
                                        window.imagelayout = file;
                                        window.BackgroundImageLayoutChanged += G_imagetype;
                                        break;
                                    case "font":
                                        window.font = file;
                                        window.FontChanged += G_font;
                                        break;
                                    case "text":
                                        window.text = file;
                                        window.TextChanged += G_text;
                                        break;
                                    case "scroll":
                                        window.scroll = file;
                                        window.Scroll += G_scroll;
                                        break;
                                    case "focus":
                                        window.focused = file;
                                        window.GotFocus += G_focused;
                                        break;
                                    case "controladded":
                                        window.focused = file;
                                        window.ControlAdded += G_ctroladded;
                                        break;
                                    case "controlremoved":
                                        window.controlremoved = file;
                                        window.ControlRemoved += G_controlremoved;
                                        break;
                                    case "defocused":
                                        window.defocused = file;
                                        window.defocused += G_defocused;
                                        break;
                                    case "close":
                                        window.close = file;
                                        window.FormClosed += G_close;
                                        break;
                                    case "open":
                                        window.open = file;
                                        window.Shown += G_open;
                                        break;
                                    case "enabledchanged":
                                        window.enabledchanged = file;
                                        window.EnabledChanged += G_enabledchanged;
                                        break;
                                    case "keydown":
                                        window.keydown = file;
                                        window.KeyDown += G_keydown;
                                        break;
                                    case "keyup":
                                        window.keyup = file;
                                        window.KeyUp += G_keyup;
                                        break;
                                    case "keypress":
                                        window.keypress = file;
                                        window.KeyPress += G_press;
                                        break;
                                    case "resize":
                                        window.resized = file;
                                        window.Resize += G_resize;
                                        break;
                                    case "resizestart":
                                        window.resizedstart = file;
                                        window.ResizeBegin += G_resizedstart;
                                        break;
                                    case "resizeend":
                                        window.resizedend = file;
                                        window.ResizeEnd += G_resizedend;
                                        break;
                                    default:
                                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'click' 'hover' 'move' 'scale' 'backcolor' 'forecolor' 'image' 'imagetype' 'font' or 'text' for 'event' in {SegmentSeperator} {codeLine}");
                                        break;
                                }
                            }
                            else
                            {
                                ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                            }
                        } 
                        catch 
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // EVENT
                        break;
                    case "bringto":
                        try
                        {
                            string next = parts[1];
                            Control? control = getControl(parts[2]);
                            if (control == null)
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Could not find Control named '{parts[2]}' in {SegmentSeperator} {codeLine}");
                            }
                            switch (next)
                            {
                                case "front":
                                    {
                                        control.BringToFront();
                                    }
                                    break;
                                case "back":
                                    {
                                        control.SendToBack();
                                    }
                                    break;
                                default:
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'front' or 'back' in {SegmentSeperator} {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        }
                        break;
                    default:
                        try
                        {
                            if (keyword == "#" || keyword == "#create".ToLower() || keyword == "#suppress".ToLower())
                            {
                                int index = keyword == "#create".ToLower() ? 1 :
                                    keyword == "#" && parts[1] == "create".ToLower() ? 2 :
                                    keyword == "#" && parts[1] == "suppress".ToLower() ? 2 :
                                    keyword == "#suppress".ToLower() ? 1 :
                                    0;
                                keyword = keyword == "#create".ToLower() ? keyword.ToLower() :
                                    keyword == "#" && parts[1] == "create".ToLower() ? "#create" :
                                    keyword == "#" && parts[1] == "suppress".ToLower() ? "#suppress" :
                                    keyword == "#suppress".ToLower() ? keyword.ToLower() :
                                    keyword;
                                switch (keyword)
                                {
                                    case "#suppress":
                                        if (parts[index] != "error")
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'error' keyword after '#suppress' in {SegmentSeperator} {codeLine}");
                                        }
                                        break;
                                    case "#create":
                                        if (parts[index] != "error")
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'error' keyword after '#create' in {SegmentSeperator} {codeLine}");
                                        }
                                        string[] strings = getString_value(parts, index + 1, true);
                                        if (strings[0] == "")
                                        {
                                            ErrorText(parts, ErrorTypes.unkown);
                                        }
                                        else
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: strings[0]);
                                        }
                                        break;
                                }
                            }
                            else if (vars.Select(x => x.Name).Contains(keyword))
                            {
                                Var var = getVar(keyword);
                                if (!var.isArray()) var = await VarManipulation(var, parts, 1);
                                else var = DoList(parts, 0, keyword);
                            }
                            else if (AllControls.Select(x => x.Name).Contains(keyword))
                            {
                                Control control = getControl(keyword);
                                control = await DoControl(parts, control.AccessibleName, 0);
                            }
                            else if (groups.Select(x => x.Name).Contains(keyword))
                            {
                                Group group = getGroup(keyword);
                                group = DoGroup(parts, 0, keyword);
                            }
                            else if (windows.Select(x => x.Name).Contains(keyword))
                            {
                                Window Window = getWindow(keyword);
                                Window = await DoWindow(parts, 0, keyword);
                            }
                            else if (!keyword.StartsWith("//") && !keyword.StartsWith("{") && !keyword.StartsWith("}") && keyword != "")
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Could not find a keyword or variable named '{keyword}' in {SegmentSeperator} {codeLine}");
                            }
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.unkown);
                        }
                        break;
                }
                returnOutput += returnOutput.Equals("") ? "" : "\n";
                senttext = ""; sent = false;
                return new string[] { returnOutput, stillInFile.ToString() };
            }
            catch
            {
                returnOutput += ErrorText(_parts, ErrorTypes.unkown, returnoutput:false) + "\n";
                return new string[] { returnOutput, "true" };
            }
        }
        static bool IsNumericString(string input)
        {
            // Use a regular expression to match a string containing only numeric characters
            return Regex.IsMatch(input, @"^\d+$");
        }
        async Task<Window> DoWindow(string[] parts, int index, string keyword)
        {
            Window? Window = null;
            string name = parts[index];
            string type = parts[index + 1];
            switch (type)
            {
                case "new":
                    {
                        Window = getWindow(name);
                        if (Window == null)
                        {
                            Window = new Window(name);
                            Window.KeyDown += KeyInput_Down;
                            Window.KeyUp += KeyInput_Up;
                            Window.MouseWheel += MouseInput_Wheel;
                            Window.MouseMove += MouseInput_Move;
                            Window.MouseDown += MouseInput_Down;
                            Window.MouseUp += MouseInput_Up;
                            windows.Add(Window);
                            Window = await changeWindow(Window, parts, index + 2);
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.alreadyMember, keyword, name);
                        }
                    }
                    break;
                case "change":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            Window = await changeWindow(Window, parts, index + 2);
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                case "clear":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            foreach (Control control in Window.Controls)
                            {
                                control.Dispose();
                            }
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                case "close":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            Window.Close();
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                case "open":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            Window.Show();
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                case "display":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            Control? control = getControl(parts[index + 2]);
                            if (control != null)
                            {
                                Window.Controls.Add(control);
                            }
                            else
                            {
                                Group group = getGroup(parts[index + 2]);
                                if (group.isSet)
                                {
                                    Window.Controls.AddRange(group.Controls.ToArray());
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Could not find a Control or Group named '{parts[index + 2]}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                case "destroy":
                    {
                        Window = getWindow(name);
                        if (Window != null)
                        {
                            Window.Close();
                            windows.Remove(Window);
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingWindow, keyword, name);
                        }
                    }
                    break;
                default:
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new', 'display', 'change', 'close', 'open', or 'destroy' for '{name}' in {SegmentSeperator} {codeLine}");
                    }
                    break;
            }
            return Window;
        }
        async Task<Window> changeWindow(Window window, string[] parts, int index)
        {
            if(parts.Length - 1 >= index)
            {
                if (parts[index] == ":")
                {
                    string[] strings = string.Join(" ", parts.SkipWhile(x=>x !=  ":").Skip(1).ToArray()).Split(",");
                    foreach (string p in strings)
                    {
                        string[] values = p.Split(':');
                        string[] before = getString_value(values, 0);
                        string[] after = getString_value(values, int.Parse(before[1]));
                        switch (before[0].Trim().ToLower())
                        {
                            case "focus":
                                {
                                    if ((bool)BoolCheck(after, 0)) window.Focus();
                                }
                                break;
                            case "enable":
                                {
                                    window.Enabled = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "minwidth":
                                {
                                    float[] floats = find_value(after, 0, 600);
                                    window.MinimumSize = new Size((int)floats[0], window.MinimumSize.Height);
                                }
                                break;
                            case "minheight":
                                {
                                    float[] floats = find_value(after, 0, 600);
                                    window.MinimumSize = new Size(window.MinimumSize.Width, (int)floats[0]);
                                }
                                break;
                            case "maxwidth":
                                {
                                    float[] floats = find_value(after, 0, 600);
                                    window.MaximumSize = new Size((int)floats[0], window.MaximumSize.Height);
                                }
                                break;
                            case "maxheight":
                                {
                                    float[] floats = find_value(after, 0, 400);
                                    window.MaximumSize = new Size(window.MaximumSize.Width, (int)floats[0]);
                                }
                                break;
                            case "width":
                                {
                                    float[] floats = find_value(after, 0, 600);
                                    window.Width = (int)floats[0];
                                }
                                break;
                            case "height":
                                {
                                    float[] floats = find_value(after, 0, 400);
                                    window.Height = (int)floats[0];
                                }
                                break;
                            case "left":
                                {
                                    float[] floats = find_value(after, 0, window.Left);
                                    window.Left = (int)floats[0];
                                }
                                break;
                            case "top":
                                {
                                    float[] floats = find_value(after, 0, window.Top);
                                    window.Top = (int)floats[0];
                                }
                                break;
                            case "opacity":
                                {
                                    float[] floats = find_value(after, 0, 1);
                                    if (floats[0] >= 0 && floats[0] <= 1) 
                                        window.Opacity = floats[0];
                                    else ErrorText(parts, ErrorTypes.custom, custom: $"Expected opacity value to be withen 0 and 1 in {SegmentSeperator} {codeLine}");
                                }
                                break;
                            case "text":
                                {
                                    window.Text = after[0];
                                }
                                break;
                            case "autosize":
                                {
                                    window.AutoSize = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "minimizebox":
                                {
                                    window.MinimizeBox = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "maximizebox":
                                {
                                    window.MaximizeBox = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "showicon":
                                {
                                    window.ShowIcon = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "showintaskbar":
                                {
                                    window.ShowInTaskbar = (bool)BoolCheck(after, 0);
                                }
                                break;
                            case "icon":
                                {
                                    string[] icon = await getFile(after, 0);
                                    try
                                    {
                                        Icon i = new Icon(icon[0]);
                                        window.Icon = i;
                                    }
                                    catch
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"An error occured setting the icon to '{window.Name}' in {SegmentSeperator} {codeLine}");
                                    }
                                }
                                break;
                            case "state":
                                {
                                    string state = after[0];
                                    if (state == "maximized")
                                    {
                                        window.WindowState = FormWindowState.Maximized;
                                    }
                                    if (state == "minimized")
                                    {
                                        window.WindowState = FormWindowState.Minimized;
                                    }
                                    if (state == "normal")
                                    {
                                        window.WindowState = FormWindowState.Normal;
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                break;
                            case "type":
                                {
                                    string type = after[0];
                                    if (type == "none")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.None;
                                    }
                                    else if (type == "fixed3d")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.Fixed3D;
                                    }
                                    else if (type == "fixeddialog")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.FixedDialog;
                                    }
                                    else if (type == "fixedsingle")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.FixedSingle;
                                    }
                                    else if (type == "fixedtoolwindow")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                                    }
                                    else if (type == "sizable")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.Sizable;
                                    }
                                    else if (type == "sizabletoolwindow")
                                    {
                                        window.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                break;
                            case "startposition":
                                {
                                    string type = after[0];
                                    if (type == "manual")
                                    {
                                        window.StartPosition = FormStartPosition.Manual;
                                    }
                                    else if (type == "center")
                                    {
                                        window.StartPosition = FormStartPosition.CenterScreen;
                                    }
                                    else if (type == "defaultbounds")
                                    {
                                        window.StartPosition = FormStartPosition.WindowsDefaultBounds;
                                    }
                                    else if (type == "default")
                                    {
                                        window.StartPosition = FormStartPosition.WindowsDefaultLocation;
                                    }
                                    else
                                    {
                                        throw new Exception();
                                    }
                                }
                                break;
                            case "backcolor":
                                {
                                    window.BackColor = returncolor(parts, after, 0, window.BackColor);
                                }
                                break;
                            case "forecolor":
                                {
                                    window.ForeColor = returncolor(parts, after, 0, window.ForeColor);
                                }
                                break;
                            case "font":
                                {
                                    string all = string.Join("", after[0].Split(" ")).Trim();
                                    bool thing2 = all.StartsWith("[") && all.EndsWith("]#suppresserror".ToLower());
                                    string fontType = "Segoe UI";
                                    int fontSize = 9;
                                    FontStyle fontStyle = FontStyle.Regular;
                                    if ((all.StartsWith("[") && all.EndsWith("]")) || thing2)
                                    {
                                        if (!thing2) all = all.Substring(1, all.Length - 2);
                                        else all = all.Substring(1, all.Length - 1).Replace("]#suppresserror", "");
                                        string[] seperator = all.Split(";");
                                        if (seperator.Length == 3)
                                        {
                                            if (IsRealFont(seperator[0]))
                                            {
                                                fontType = seperator[0];
                                            }
                                            else
                                            {
                                                ErrorText(parts, ErrorTypes.custom, custom: $"'{fontType}' is not a valid font. Try 'Arial' or go to https://learn.mcrosoft.com for more inWindowation about supported WinWindows fonts. Exception for '{window.Name}' in line {SegmentSeperator} {codeLine}");
                                            }
                                            try
                                            {
                                                float[] floats = find_value(seperator, 1, -1);
                                                fontSize = (int)floats[0];
                                                if (fontSize < 0) new Exception("");
                                            }
                                            catch
                                            {
                                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected a number greater greater than zero for font size value in {SegmentSeperator} {codeLine}");
                                            }
                                            if (Enum.TryParse(seperator[2], out FontStyle parsedFontStyle))
                                            {
                                                fontStyle = parsedFontStyle;
                                            }
                                            else
                                            {
                                                ErrorText(parts, ErrorTypes.custom, custom: $"'{seperator[2]}' is not a valid font style. Valid styles are: {string.Join(", ", Enum.GetNames(typeof(FontStyle)))}. Exception for '{window.Name}' in line {SegmentSeperator} {codeLine}");
                                            }
                                        }
                                        else
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Requires 3 values for font in {SegmentSeperator} {codeLine}");
                                        }
                                    }
                                    else if (parts.Length != 1)
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected '[' and ']' for font value in {SegmentSeperator} {codeLine}");
                                    }
                                    window.Font = new Font(fontType, fontSize, fontStyle);
                                }
                                break;
                            default:
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected a correct property name for '{window.Name}' in {SegmentSeperator} {codeLine}");
                                break;
                        }
                    }
                }
                else
                {
                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' to initialize values to set in {SegmentSeperator} {codeLine}");
                }
            }
            return window;
        }
        Window? getWindow(string name)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].Name == name)
                {
                    return windows[i];
                }
            }
            return null;
        }
        string[] ExtractContent(string[] _input, string[] parts)
        {
            string input = string.Join(Environment.NewLine, _input);
            int depth = 0;
            int startIndex = -1;
            bool insideBrackets = false;

            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                if (currentChar == '{')
                {
                    if (depth == 0)
                    {
                        startIndex = i;
                    }
                    depth++;
                }
                else if (currentChar == '}')
                {
                    depth--;
                    if (depth == 0 && startIndex >= 0)
                    {
                        int endIndex = i + 1; // Include the closing bracket
                        return input.Substring(startIndex, endIndex - startIndex).Trim().Split(Environment.NewLine);
                    }
                }
            }

            // If no closing bracket is found, throw an exception
            ErrorText(parts, ErrorTypes.custom, custom: $"Expected closing bracket for '{parts[0]}' in {SegmentSeperator} {codeLine}");
            return new string[0];
        }
        int GetZIndex(Control control)
        {
            Control parent = control.Parent;

            if (parent != null)
            {
                for (int i = 0; i < parent.Controls.Count; i++)
                {
                    if (parent.Controls[i] == control)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
        Var DoList(string[] parts, int _index, string keyword)
        {
            Var var = new Var("");
            var.isSet = false;
            string name = parts[_index];
            string type = parts[_index + 1];
            List<string> varray = new List<string>();
            switch (type)
            {
                case "new":
                    string[] colon = getString_value(parts, _index + 2, true);
                    string values = colon[0];
                    if (!values.StartsWith(":") && values != "")
                    {
                        ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the list in {SegmentSeperator} {codeLine}");
                    }
                    else if (values == "")
                    {

                    }
                    else if (values.StartsWith(":"))
                    {
                        string allv = values.Remove(0, 1);
                        varray = allv.Split(",").Select(x => x.Trim()).ToList();
                    }
                    var = CreateVar(parts, _index, alreadyarray: varray.ToArray(), allowJump: true).Result;
                    vars.Add(var);
                    break;
                case "add":
                    var = getVar(name);
                    if (var.isSet && var.isArray())
                    {
                        varray = var.array.ToList();
                        string[] colon_ = getString_value(parts, _index + 2, true);
                        string values_ = colon_[0];
                        if (!values_.StartsWith(":") && values_ == "")
                        {
                            ErrorText(parts, ErrorTypes.custom, $"Expected values set the list in {SegmentSeperator} {codeLine}");
                        }
                        else if (!values_.Contains(":"))
                        {
                            varray.Add(values_.Trim());
                        }
                        else if (values_.StartsWith(":"))
                        {
                            string allv_ = values_.Remove(0, 1);
                            varray.AddRange(allv_.Split(",").Select(x => x.Trim()));
                        }
                        var.set(array: varray.ToArray());
                    }
                    else if (!var.isArray())
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in {SegmentSeperator} {codeLine}");
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                    }
                    break;
                case "equals":
                    var = getVar(name);
                    if (var.isSet && var.isArray())
                    {
                        varray = var.array.ToList();
                        float[] indexes = find_value(parts, _index + 2, 0);
                        int index = (int)indexes[0];
                        string[] colon__ = getString_value(parts, (int)indexes[1], true);
                        string values__ = colon__[0];
                        if (values__ == "")
                        {
                            ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the list in {SegmentSeperator} {codeLine}");
                        }
                        else
                        {
                            varray[index] = values__.Trim();
                        }
                        var.array = varray.ToArray();
                    }
                    else if (!var.isArray())
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in {SegmentSeperator} {codeLine}");
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                    }
                    break;
                case "clear":
                    var = getVar(name);
                    if (var.isSet && var.isArray())
                    {
                        var.array = new string[0];
                    }
                    else if (!var.isArray())
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in {SegmentSeperator} {codeLine}");
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                    }
                    break;
                case "destroy":
                    var = getVar(name);
                    if (var.isSet && var.isArray())
                    {
                        vars.Remove(var);
                    }
                    else if (!var.isArray())
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in {SegmentSeperator} {codeLine}");
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                    }
                    break;
                default:
                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new' 'add' 'equals' 'destroy' or 'clear' in {SegmentSeperator} {codeLine}");
                    break;
            }
            return var;
        }
        Group DoGroup(string[] parts, int _index, string keyword)
        {
            Group group = new Group("");
            group.isSet = false;
            string name = parts[_index];
            string type = parts[_index + 1];
            List<Control> varray = new List<Control>();
            switch (type)
            {
                case "new":
                    string[] colon = getString_value(parts, _index + 2, true);
                    string values = colon[0];
                    group = new Group(name);
                    if (!values.StartsWith(":") && values != "")
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' to set values to the Group in {SegmentSeperator} {codeLine}");
                    }
                    else if (values == "")
                    {

                    }
                    else if (values.StartsWith(":"))
                    {
                        string[] allv = values.Remove(0, 1).Split(",").Select(x => x.Trim()).ToArray();
                        for (int i = 0; i < allv.Length; i++)
                        {
                            varray.Add(getControl(allv[i]));
                        }
                    }
                    group.set(varray);
                    groups.Add(group);
                    break;
                case "add":
                    Group group_a = getGroup(name);
                    if (group_a.isSet)
                    {
                        varray = group_a.Controls;
                        string[] colon_ = getString_value(parts, _index + 2, true);
                        string values_ = colon_[0];
                        if (!values_.StartsWith(":") && values_ == "")
                        {
                            ErrorText(parts, ErrorTypes.custom, $"Expected values add to the Group in {SegmentSeperator} {codeLine}");
                        }
                        else if (!values_.Contains(":"))
                        {
                            varray.Add(getControl(values_.Trim()));
                        }
                        else if (values_.StartsWith(":"))
                        {
                            string[] allv = values_.Remove(0, 1).Split(",").Select(x => x.Trim()).ToArray();
                            for (int i = 0; i < allv.Length; i++)
                            {
                                varray.Add(getControl(allv[i]));
                            };
                        }
                        group_a.set(varray);
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                    }
                    break;
                case "equals":
                    Group group_e = getGroup(name);
                    if (group_e.isSet)
                    {
                        varray = group_e.Controls;
                        float[] indexes = find_value(parts, _index + 2, -1);
                        int index = (int)indexes[0];
                        string[] colon__ = getString_value(parts, (int)indexes[1], true);
                        string values__ = colon__[0];
                        if (values__ == "")
                        {
                            ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the Group in {SegmentSeperator} {codeLine}");
                        }
                        else
                        {
                            varray[index] = getControl(values__.Trim());
                        }
                        group_e.Controls = varray;
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                    }
                    break;
                case "remove":
                    Group group_r = getGroup(name);
                    if (group_r.isSet)
                    {
                        varray = group_r.Controls;
                        float[]? indexes = null;
                        int? index = null;
                        Control? rem = null;
                        try
                        {
                            indexes = find_value(parts, _index, -1);
                            index = (int)indexes[0];
                        }
                        catch
                        {
                            rem = getControl(parts[_index]);
                        }
                        if (index == null && rem == null)
                        {
                            ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the Group in {SegmentSeperator} {codeLine}");
                        }
                        else
                        {
                            if (index != null)
                            {
                                varray.RemoveAt((int)index);
                            }
                            else if (rem != null)
                            {
                                varray.Remove(rem);
                            }
                        }
                        group_r.set(varray);
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                    }
                    break;
                case "clear":
                    Group group_c = getGroup(name);
                    if (group_c.isSet)
                    {
                        group_c.clear();
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                    }
                    break;
                case "destroy":
                    Group group_d = getGroup(name);
                    if (group_d.isSet)
                    {
                        groups.Remove(group_d);
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                    }
                    break;
                case "destroyall":
                    {
                        Group g = getGroup(name);
                        if (g.isSet)
                        {
                            groups.Remove(g);
                            foreach(Control c in g.Controls)
                            {
                                c.Dispose();
                            }
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.missingGroup, keyword, name);
                        }
                    }
                    break;
                case "change":
                    try
                    {
                        bool abs = (bool)BoolCheck(parts, _index + 2);
                        Group group1 = getGroup(name);
                        if (group1.isSet)
                        {
                            group1.Absolute = abs;
                            Control control = new Control();
                            control.Text = group1.Text;
                            control.Top = group1.Y;
                            control.Width = group1.Width;
                            control.Height = group1.Height;
                            control.BackColor = Color.FromArgb(group1.bgR, group1.bgG, group1.bgB);
                            control.ForeColor = Color.FromArgb(group1.fcR, group1.fcG, group1.fcB);
                            control.Text = group1.Text;

                            control = Change(control, parts, _index + 3, true, false, false, true);

                            group1.X = control.Left;
                            group1.Y = control.Top;
                            group1.Width = control.Width;
                            group1.Height = control.Height;
                            group1.bgR = control.BackColor.R;
                            group1.bgG = control.BackColor.G;
                            group1.bgB = control.BackColor.B;
                            group1.fcR = control.ForeColor.R;
                            group1.fcG = control.ForeColor.G;
                            group1.fcB = control.ForeColor.B;
                            group1.Text = control.Text;
                        }
                    }
                    catch
                    {
                        ErrorText(parts, ErrorTypes.normal, keyword);
                    }
                    break;
                default:
                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new' 'add' 'equals' 'remove' 'change' or 'clear' in {SegmentSeperator} {codeLine}");
                    break;
            }
            return group;
        }
        async Task<Var> VarManipulation(Var var, string[] parts, int index)
        {
            try
            {
                string mid = parts[index];
                switch (mid)
                {
                    case "=":
                    case "+":
                    case "-":
                        {
                            try
                            {
                                float[] floats = find_value(parts, index + 1, 0);
                                string next = floats[0].ToString();
                                if (var.isNumber())
                                {
                                    var.Multiply(mid, next);
                                }
                                else if (var.isString())
                                {
                                    string[] strings = getString_value(parts, index + 1);
                                    var.Change(mid, strings[0]);
                                }
                                else
                                {
                                    new Exception("Expected Number or Text Variable");
                                }
                            }
                            catch
                            {
                                if (!var.isNumber() && !var.isArray())
                                {
                                    string[] strings = getString_value(parts, index + 1, true);
                                    var.Change(mid, strings[0]);
                                }
                                else if (var.isNumber() && mid == "=")
                                {
                                    string[] strings = getString_value(parts, index + 1);
                                    var.set(strings[0]);
                                }
                                else
                                {
                                    new Exception("Expected Number or Text Variable");
                                }
                            }
                        }
                        break;
                    case "*":
                    case "/":
                        {
                            float[] floats = find_value(parts, index + 1, 0);
                            string next = floats[0].ToString();
                            if (var.isNumber())
                            {
                                var.Multiply(mid, next);
                            }
                            else
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected a Number Variable in {SegmentSeperator} {codeLine}");
                            }
                        }
                        break;
                    case ":":
                        {
                            try
                            {
                                var = await CreateVar(parts, 0, true, allowJump:true);
                            }
                            catch
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"There was an error setting the variable to the correct value in {SegmentSeperator} {codeLine}");
                            }
                        }
                        break;
                    default:
                        {
                            if (var.returnBool(var.value()) != null && BoolCheck(parts, index, false) != null)
                                var.set(BoolCheck(parts, index) == true ? "1" : "0");
                            else
                                ErrorText(parts, ErrorTypes.custom, custom: $"There was an Error with changing '{var.Name}'. Expected '+', '-', '*', '/', '=', or ':' in {SegmentSeperator} {codeLine}");
                        }
                        break;
                }
            }
            catch
            {
                ErrorText(parts, ErrorTypes.custom, custom: $"There was an Error with changing '{var.Name}' in {SegmentSeperator} {codeLine}");
            }
            return var;
        }
        async Task<Control> DoControl(string[] parts, string contype, int index)
        {
            GShape? obj = new GShape();
            GLabel? lab = new GLabel();
            GTextBox? txb = new GTextBox();
            GButton? btn = new GButton();
            controlType controltype =
                contype == "shape" ? controlType.Shape :
                contype == "label" ? controlType.GLabel :
                contype == "textbox" ? controlType.Textbox :
                contype == "button" ? controlType.GButton :
                controlType.None;
            Control control = await createControl(parts, controltype, index);
            obj = control is GShape ? control as GShape : null;
            lab = control is GLabel ? control as GLabel : null;
            txb = control is GTextBox ? control as GTextBox : null;
            btn = control is GButton ? control as GButton : null;
            if (obj == null && lab == null && txb == null && btn == null)
            {
                throw new Exception();
            }
            return control;
        }
        Group getGroup(string name)
        {
            Group group = new Group(name);
            group.isSet = false;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].Name == name)
                {
                    group = groups[i];
                    group.isSet = true;
                }
            }
            return group;
        } 
        void StopAllSounds()
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                sounds[i].Stop();
            }
        }
        async Task<Player> GetPlayer(string name) 
        {
            Player player = new Player();
            for (int i = 0; i < sounds.Count; i++)
            {
                if(name == sounds[i].Name)
                {
                    player = sounds[i];
                }
            }
            return player;
        }
        bool? BoolCheck(string[] parts, int index, bool error = true)
        {
            bool? check = null;

            if (parts[index].Contains("?"))
            {
                check = QMarkCheck(parts, index)[0] == 1;
            }
            else
            {
                Var var = getVar(parts[index].Trim());
                if (var.isBool() || var.isNumber())
                {
                    check = var.returnBool();
                }
                else if (!var.isSet && Var.staticReturnBool(parts[index].Trim()) != null)
                {
                    check = Var.staticReturnBool(parts[index].Trim()) == true;
                }
                else if (var.isSet && var.returnBool() != null)
                {
                    check = var.returnBool();
                }
                else
                {
                    if (error) ErrorText(parts, ErrorTypes.custom, custom: $"Expected a boolean variable in {SegmentSeperator} {codeLine}");
                }
            }
            return check;
        }
        Var getVar(string name)
        {
            Var var = new Var("");
            var.isSet = false;
            for (int i = 0; i < vars.Count; i++)
            {
                if (vars[i].Name == name)
                {
                    var = vars[i];
                }
            }
            return var;
        }
        int[] QMarkCheck(string[] parts, int index)
        {
            try
            {
                bool check = false;
                parts = getString_value(parts, index, true);
                parts = parts[0].Split(' ');
                string value = "";
                bool insideBlock = false;

                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts.Length == 1)
                    {
                        value = parts[i].Replace("?(", "").Replace(")?", "");

                    }
                    else if (parts[i].StartsWith("?("))
                    {
                        if (parts[i].Length > 2) value += parts[i].Replace("?(", "") + " ";
                        insideBlock = true;
                        continue;
                    }

                    if (insideBlock)
                    {
                        if (parts[i].EndsWith(")?"))
                        {
                            if (parts[i].Length > 1) value += parts[i].Replace(")?", "");
                            insideBlock = false;
                        }
                        else
                        {
                            value += parts[i] + (parts.Length - 1 > i ? " " : "");
                        }
                    }
                }
                index = parts.ToList().IndexOf(value.Split(" ")[value.Split(" ").Length - 1]);

                if (value != "")
                {
                    string[] values = value.Split(" ");
                    for (int i = 0; i < values.Length; i++)
                    {
                        try
                        {
                            float[] floats = find_value(values, i, 0);
                            values[i] = floats[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                string[] strings = getString_value(values, i);
                                values[i] = strings[0];
                            }
                            catch
                            {
                                // Nothing
                            }
                        }
                    }
                    check = IfCheck(string.Join(" ", values));
                }
                else
                {
                    throw new Exception("");
                }

                return new int[] { check ? 1 : 0, index };
            }
            catch
            {
                ErrorText(parts, ErrorTypes.custom, custom: $"There was an error with the bool check in {SegmentSeperator} {codeLine}");
                return new int[] { };
            }
        }
        bool IfCheck(string inners)
        {
            // Use regular expressions to split based on "and", "&", and "or" with optional spaces around them
            string[] parts = Regex.Split(inners, @"\s*(and|&|or)\s*", RegexOptions.IgnoreCase);

            bool overallResult = true;  // Initialize to true; we'll use it as a default for "or" operations
            string lastOperator = null; // Maintain the last operator

            for (int i = 0; i < parts.Length; i++)
            {
                string expression = parts[i].Trim();
                bool partResult = EvaluateExpression(expression);

                if (i == 0)
                {
                    // First part doesn't have a preceding operator; use its result as the initial overall result
                    overallResult = partResult;
                }
                else
                {
                    // Check the preceding operator to determine how to combine results
                    string precedingOperator = lastOperator;

                    if (precedingOperator.Equals("and", StringComparison.OrdinalIgnoreCase) || precedingOperator == "&")
                    {
                        if (parts[i] != "and" && parts[i] != "&")
                            overallResult = overallResult && partResult;
                    }
                    else if (precedingOperator.Equals("or", StringComparison.OrdinalIgnoreCase))
                    {
                        if (parts[i] != "or")
                            overallResult = overallResult || partResult;
                    }
                }

                // Update the lastOperator variable
                lastOperator = parts[i].Trim();
            }

            return overallResult;
        }
        bool EvaluateExpression(string expression)
        {
            try
            {
                Expression expr = new Expression(expression);
                return (bool)expr.Evaluate(); ;
            }
            catch
            {
                string[] sides = expression.Split(new char[] { '=', '!' });
                char? mid = expression.Contains("=") ? '=' : expression.Contains('!') ? '!' : null;
                if (sides.Length != 2 || mid == null)
                {
                    return false;
                }
                bool equals = sides[0].Trim() == sides[1].Trim();
                bool midequal = mid == '=';
                return equals && midequal ? true :
                    !equals && !midequal ? true : 
                    false;
            }
        }
        bool validpathcheck(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (string.IsNullOrWhiteSpace(path) || path.Length < 3)
            {
                return false;
            }

            if (!driveCheck.IsMatch(path.Substring(0, 3)))
            {
                return false;
            }
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
            {
                return false;
            }

            return true;
        }
        async Task<string[]> getFile(string[] parts, int index, int skip = 1, string seperator = " ", bool vars = false)
        {
            string[] get = getString_value(parts, index, true, vars, false, false);
            get[0] = string.Join(" ", get[0].Split(" ").Skip(skip));
            string[] file_s = new string[] { };
            if (get[0].Contains(" => "))
            {
                file_s = get[0].Split(" => ");
            }
            else if (get[0].Contains(" : "))
            {
                file_s = get[0].Split(" : ");
            }
            else
            {
                file_s = get;
            }
            if (!get[0].Contains("\\") && !get[0].Contains("/"))
            {
                string[] val = getString_value(parts, index, false, true, true, false);
                file_s[0] = val[0];
                index = int.Parse(val[1]) - 1;
            }
            else
            {
                index = file_s == get ? int.Parse(get[1]) : int.Parse(get[1]) - 1;
            }
            if (file_s[0].StartsWith("~/"))
            {
                FileInfo fileinfo = new FileInfo(ScriptDirectory);
                DirectoryInfo directoryinfo = fileinfo.Directory;
                string filename = file_s[0].Replace("~/", "");
                string path = Path.Combine(directoryinfo.FullName + "\\" + filename);
                file_s[0] = path;
            }
            string final = file_s == get ? file_s[0] : string.Join(seperator, file_s.Take(file_s.Length - 1));
            return new string[] { final, index.ToString() };
        }
        string SetVKeyword(string[] parts, int switchIndex, string keyword, string text, Types Description = Types.None)
        {
            if (parts[switchIndex - 1] == "=>" || parts[switchIndex - 1] == ":" && (parts[switchIndex - 2] != "=>" || parts[switchIndex - 2] != ":"))
                switchIndex--;
            else if ((parts.Length > switchIndex + 1) && (parts[switchIndex + 1] == "=>" || parts[switchIndex + 1] == ":") && (parts[switchIndex] != "=>" || parts[switchIndex] != ":"))
                switchIndex++;
            if ((parts[switchIndex].Contains("\\") || parts[switchIndex].Contains("/") || vars.Select(x=>x.Name).Contains(parts[switchIndex])) && (parts[switchIndex - 1] != "=>" || parts[switchIndex - 1] != ":") && (!parts.Contains(":") && !parts.Contains("=>")))
                parts[switchIndex] = "";
            string errorText = "";
            switch (parts.Length - 1 >= switchIndex ? parts[switchIndex] : "")
            {
                case "=>":
                    try
                    {
                        Var var = CreateVar(parts, switchIndex + 1, false, text, description: Description).Result;
                        vars.Add(var);
                    }
                    catch
                    {
                        errorText = $"Expected new variable name after '=>' for '{keyword}' in {SegmentSeperator} {codeLine}";
                        ErrorText(parts, ErrorTypes.custom, custom: errorText);
                    }
                    break;
                case ":":
                    try
                    {
                        Var? var0 = SetVar(text, parts[switchIndex + 1].Trim(), parts, Description);
                    }
                    catch
                    {
                        errorText = $"Expected variable name after ':' for '{keyword}' in {SegmentSeperator} {codeLine}";
                        ErrorText(parts, ErrorTypes.custom, custom: errorText);
                    }
                    break;
                case "":
                    break;
                default:
                    errorText = $"Expected '=>' or ':' for '{keyword}' in {SegmentSeperator} {codeLine}";
                    ErrorText(parts, ErrorTypes.custom, custom: errorText);
                    break;
            }
            return errorText;
        }
        Var SetVar(string alreadyVal, string name, string[] parts, Types description = Types.None)
        {
            Var var = new Var(name);
            int index = 0;

            if (vars.Select(x => x.Name).Contains(name))
            {
                var = vars.FirstOrDefault(x => x.Name == name);
                index = vars.IndexOf(var);
                var.isSet = true;
                var.Description = description;
            }
            else
            {
                var.isSet = false;
            }

            switch (var.isSet)
            {
                case true:
                    var.set(alreadyVal);
                    break;
                case false:
                    ErrorText(parts, ErrorTypes.custom, custom: $"Can not find a variable named '{name}' in {SegmentSeperator} {codeLine}");
                    break;
            }

            vars[index] = var;
            return var;
        }
        async Task<Var> CreateVar(string[] parts, int index = 1, bool reuse = true, string? alreadyVal = null, bool allowJump = false, string[]? alreadyarray = null, Types description = Types.None)
        {
            string name = parts[index].Trim();

            if(UnusableNames.Contains(name) || IsNumericString(name) || UnusableContains.Any(unusable => name.Contains(unusable)) || name.Contains(":") || name.Contains("|") || name.Contains("\\") || name.Trim() == "")
            {
                ErrorText(parts, ErrorTypes.violation, name: name);
            }

            Var var = new Var(name);
            var.Description = description;

            if (reuse && vars.Select(x => x.Name).Contains(name))
            {
                var = vars.FirstOrDefault(x => x.Name == name);
            }
            else if (!reuse && vars.Select(x => x.Name).Contains(name))
            {
                ErrorText(parts, ErrorTypes.alreadyMember, "Variable", name);
            }
            if (alreadyVal == null && alreadyarray == null)
            {
                string stringvalue = "";
                int? intval = null;

                try
                {
                    if (allowJump && parts[index + 1] == ":")
                    {
                        try
                        {
                            string str = string.Join(' ', parts.Skip(index + 2));
                            if (str.Trim() == "")
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected an assigned value after ':' in {SegmentSeperator} {codeLine}");
                            }

                            string[] strings = await PlaySwitch(jumpsto: str);
                            stringvalue = strings[0];
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.custom, custom: $"There was an error setting the variable to the correct value in {SegmentSeperator} {codeLine}");
                        }
                    }
                    else
                    {
                        try
                        {
                            float[] intValue = find_value(parts, index + 1, 0);
                            intval = (int)intValue[0];
                            index = (int)intValue[1];
                        }
                        catch
                        {
                            string[] gotstring1 = getString_value(parts, index + 1, true);
                            stringvalue = gotstring1[0];
                            index = int.Parse(gotstring1[1]);
                        }
                    }
                }
                catch
                {
                    stringvalue = "";
                }
                var.set(intval != null ? intval.ToString() : stringvalue);
            }
            else if(alreadyarray == null)
            {
                var.set(alreadyVal);
            }
            else if(alreadyarray != null)
            {
                var.set(array: alreadyarray);
            }
            var.isSet = true;

            return var;
        }
        string ColonResponse(string value, string[] parts = null)
        {
            string[] ind = value.Split(':');
            if (value.StartsWith("system:"))
            {
                switch (ind[1])
                {
                    case "random":
                        switch (ind[2])
                        {
                            case "int":
                                bool more = value.Contains("system:random:int:");
                                if (more)
                                {
                                    float[] fl1 = find_value(ind, 3, 0, true, false);
                                    float[] fl2 = find_value(ind, (int)fl1[1], 0, true, false);
                                    int v1 = (int)fl1[0];
                                    int v2 = (int)fl2[0];
                                    if (v1 >= v2)
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"Minumum can not be greater or equal to the max in 'system:random' line {codeLine}");
                                        return "0";
                                    }
                                    Random rand = new Random();
                                    value = rand.Next(v1, v2).ToString();
                                }
                                else
                                {
                                    Random rand = new Random();
                                    value = rand.Next().ToString();
                                }
                                break;
                            case "single":
                                Random rand0 = new Random();
                                value = rand0.NextSingle().ToString();
                                break;
                        }
                        break;
                    case "number":
                        {
                            string[] strings = getString_value(ind, 2, usecolon:false);
                            string name = strings[0];
                            Var var = getVar(name);
                            if (var.isSet)
                            {
                                value = var.isNumber().ToString();
                            }
                            else
                            {
                                bool number = false;
                                try
                                {
                                    int.Parse(name);
                                    number = true;
                                }
                                catch
                                {
                                    number = false;
                                }
                                value = number.ToString();
                            }
                        }
                        break;
                    case "machine":
                        switch (ind[2])
                        {
                            case "MachineName":
                                value = Environment.MachineName.ToString();
                                break;
                            case "OSVersion":
                                value = Environment.OSVersion.ToString();
                                break;
                            case "Is64BitOperatingSystem":
                                value = Environment.Is64BitOperatingSystem.ToString();
                                break;
                            case "UserName":
                                value = Environment.UserName.ToString();
                                break;
                            case "WorkingSet":
                                value = Environment.WorkingSet.ToString();
                                break;
                            case "HasShutdownStarted":
                                value = Environment.HasShutdownStarted.ToString();
                                break;
                            case "CurrentDirectory":
                                value = Environment.CurrentDirectory.ToString();
                                break;
                        }
                        break;
                    case "litteral":
                        if (value.Contains("system:litteral:"))
                        {
                            string[] strings = getString_value(ind, 2, false, false, false, false, "", false);
                            return strings[0];
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' after 'system:litteral' in {SegmentSeperator} {codeLine}");
                        }
                        break;
                    case "space":
                        value = " ";
                        break;
                    case "newline":
                        value = "\n";
                        break;
                    case "pipe":
                        value = "|";
                        break;
                    case "nothing":
                        value = "";
                        break;
                    default: break;
                }
            }
            else if (AllControls.Select(x => x.Name).Contains(ind[0]) && ind.Length > 1)
            {
                Control control = AllControls.FirstOrDefault(x => x.Name == ind[0]);
                switch (ind[1].ToLower())
                {
                    case "x":
                        value = control.Left.ToString();
                        break;
                    case "y":
                        value = control.Top.ToString();
                        break;
                    case "w":
                    case "width":
                        value = control.Width.ToString();
                        break;
                    case "h":
                    case "height":
                        value = control.Height.ToString();
                        break;
                    case "backcolor":
                    case "bc":
                        value = $"{control.BackColor.R}, {control.BackColor.G}, {control.BackColor.B}";
                        break;
                    case "backcolor-r":
                    case "bcr":
                        value = control.BackColor.R.ToString();
                        break;
                    case "backcolor-g":
                    case "bcg":
                        value = control.BackColor.G.ToString();
                        break;
                    case "backcolor-b":
                    case "bcb":
                        value = control.BackColor.B.ToString();
                        break;
                    case "t":
                    case "text":
                        if (control is not GShape)
                            value = control.Text.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Shapes don't have a '{ind[1]}' property in {SegmentSeperator} " + codeLine);
                        break;
                    case "forecolor":
                    case "fc":
                        value = $"{control.ForeColor.R}, {control.ForeColor.G}, {control.ForeColor.B}";
                        break;
                    case "forecolor-r":
                    case "fcr":
                        if (control is not GShape)
                            value = control.ForeColor.R.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: "Shapes can don't have a 'text-r' property in {SegmentSeperator} " + codeLine);
                        break;
                    case "forecolor-g":
                    case "fcg":
                        if (control is not GShape)
                            value = control.ForeColor.G.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: "Shapes can don't have a 'text-g' property in {SegmentSeperator} " + codeLine);
                        break;
                    case "forecolor-b":
                    case "fcb":
                        if (control is not GShape)
                            value = control.ForeColor.B.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: "Shapes can don't have a 'text-b' property in {SegmentSeperator} " + codeLine);
                        break;
                    case "click":
                        if (control is GButton a)
                            value = a.isclick.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: "Only Buttons have a 'click' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "font":
                        value = $"{control.Font.Name}, {control.Font.Size}, {control.Font.Style}";
                        break;
                    case "fontname":
                        value = control.Font.Name;
                        break;
                    case "fontsize":
                        value = control.Font.Size.ToString();
                        break;
                    case "fontstyle":
                        value = control.Font.Style.ToString();
                        break;
                    case "point":
                    case "points":
                        if(control is GShape gs)
                            value = string.Join(",", gs.Points);
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Shapes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "auto":
                    case "autosize":
                        if (control is GLabel l)
                            value = l.AutoSize.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Labels have 'autosize' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "multi":
                    case "multiline":
                        if (control is GTextBox t)
                            value = t.Multiline == true ? "1" : "0";
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Textboxes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "wrap":
                    case "wordwrap":
                        if (control is GTextBox tt)
                            value = tt.WordWrap == true ? "1" : "0";
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Textboxes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "vertical":
                    case "verticalscrollbar":
                        if (control is GTextBox ttt)
                            value = ttt.ScrollBars == ScrollBars.Vertical || ttt.ScrollBars == ScrollBars.Both ? "1" : "0";
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Textboxes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "horizantal":
                    case "horizantalscrollbar":
                        if (control is GTextBox tttt)
                            value = tttt.ScrollBars == ScrollBars.Vertical || tttt.ScrollBars == ScrollBars.Both ? "1" : "0";
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Textboxes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "p":
                    case "poly":
                        if (control is GShape g)
                            value = g.Poly.ToString();
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Shapes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    case "z":
                    case "zindex":
                        value = GetZIndex(control).ToString();
                        break;
                    case "focused":
                            value = control.Focused ? "1" : "0";
                        break;
                    case "enabled":
                            value = control.Enabled ? "1" : "0";
                        break;
                    case "readonly":
                        if (control is GTextBox rt)
                            value = rt.ReadOnly ? "1" : "0";
                        else
                            ErrorText(parts, ErrorTypes.custom, custom: $"Only Textboxes have '{ind[1]}' value in {SegmentSeperator} " + codeLine);
                        break;
                    default:
                        ErrorText(parts, ErrorTypes.custom, custom: $"'{ind[0]}' is not a valid value for '{control.Name}' in {SegmentSeperator} {codeLine}");
                        break;
                }
            }
            else if (windows.Select(x => x.Name).Contains(ind[0]) && ind.Length > 1)
            {
                Window window = getWindow(ind[0]);
                switch (ind[1])
                {
                    case "autosize":
                        {
                            value = window.AutoSize.ToString();
                        }
                        break;
                    case "minimizebox":
                        {
                            value = window.MinimizeBox.ToString();
                        }
                        break;
                    case "maximizebox":
                        {
                            value = window.MaximizeBox.ToString();
                        }
                        break;
                    case "showicon":
                        {
                            value = window.ShowIcon.ToString();
                        }
                        break;
                    case "showintaskbar":
                        {
                            value = window.ShowInTaskbar.ToString();
                        }
                        break;
                    case "icon":
                        {
                            value = window.Icon.ToString();
                        }
                        break;
                    case "state":
                        {
                            value = window.WindowState.ToString();
                        }
                        break;
                    case "position":
                        {
                            value = window.StartPosition.ToString();
                        }
                        break;
                    case "type":
                        {
                            value = window.FormBorderStyle.ToString();
                        }
                        break;
                    case "maxwidth":
                        {
                            value = window.MaximumSize.Width.ToString();
                        }
                        break;
                    case "maxheight":
                        {
                            value = window.MaximumSize.Height.ToString();
                        }
                        break;
                    case "minwidth":
                        {
                            value = window.MinimumSize.Width.ToString();
                        }
                        break;
                    case "minheight":
                        {
                            value = window.MinimumSize.Height.ToString();
                        }
                        break;
                    case "width":
                        {
                            value = window.Width.ToString();
                        }
                        break;
                    case "height":
                        {
                            value = window.Width.ToString();
                        }
                        break;
                    case "left":
                        {
                            value = window.Left.ToString();
                        }
                        break;
                    case "top":
                        {
                            value = window.Top.ToString();
                        }
                        break;
                    case "opacity":
                        {
                            value = window.Opacity.ToString();
                        }
                        break;
                    case "backcolor":
                    case "bc":
                        value = $"{window.BackColor.R}, {window.BackColor.G}, {window.BackColor.B}";
                        break;
                    case "backcolor-r":
                    case "bcr":
                        value = window.BackColor.R.ToString();
                        break;
                    case "backcolor-g":
                    case "bcg":
                        value = window.BackColor.G.ToString();
                        break;
                    case "backcolor-b":
                    case "bcb":
                        value = window.BackColor.B.ToString();
                        break;
                    case "t":
                    case "text":
                        value = window.Text.ToString();
                        break;
                    case "forecolor":
                    case "fc":
                        value = $"{window.ForeColor.R}, {window.ForeColor.G}, {window.ForeColor.B}";
                        break;
                    case "forecolor-r":
                    case "fcr":
                        value = window.ForeColor.R.ToString();
                        break;
                    case "forecolor-g":
                    case "fcg":
                        value = window.ForeColor.G.ToString();
                        break;
                    case "forecolor-b":
                    case "fcb":
                        value = window.ForeColor.B.ToString();
                        break;
                    case "font":
                        value = $"{window.Font.Name}, {window.Font.Size}, {window.Font.Style}";
                        break;
                    case "fontname":
                        value = window.Font.Name;
                        break;
                    case "fontsize":
                        value = window.Font.Size.ToString();
                        break;
                    case "fontstyle":
                        value = window.Font.Style.ToString();
                        break;
                    case "focused":
                            value = window.Focused ? "1" : "0";
                        break;
                    case "enabled":
                            value = window.Enabled ? "1" : "0";
                        break;
                    default:
                        ErrorText(parts, ErrorTypes.custom, custom: $"'{ind[0]}' is not a valid value for '{window.Name}' in {SegmentSeperator} {codeLine}");
                        break;
                }
            }
            else if (vars.Select(x => x.Name).Contains(ind[0]) && ind.Length > 1) 
            {
                Var var = getVar(ind[0]);
                switch (ind[1])
                {
                    case "length":
                        value = var.Description == Types.Array ? var.array.Length.ToString() : var.text.Length.ToString();
                        if(ind.Length > 2 && var.isArray())
                        {
                            float _number = find_value(ind, 2, 0)[0];
                            value = var.array[(int)_number].Length.ToString();
                        }
                        else if (ind.Length > 2 && !var.isArray())
                        {
                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected an array variable in {SegmentSeperator} {codeLine}");
                        }
                        break;
                    case "contains":
                        if (ind.Length > 2)
                        {
                            string v = ind[2];
                            value = var.value().Contains(v) ? "1" : "0";
                        }
                        else
                        {
                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected a value to check in '{ind[0]}:{ind[1]}' for in {SegmentSeperator} {codeLine}");
                        }
                        break;
                    default:
                        float number = find_value(ind, 1, 0)[0];
                        value = var.array[(int)number];
                        break;
                }
            }
            else if (groups.Select(x => x.Name).Contains(ind[0]) && ind.Length > 1) 
            {
                Group group = getGroup(ind[0]);
                switch (ind[1])
                {
                    case "length":
                        value = group.Controls.Count.ToString();
                        break;
                    default:
                        float number = find_value(ind, 1, 0)[0];
                        value = group.Controls[(int)number].Name;
                        break;
                }
            }
            else if (ind.Length > 1 && ind[0].Split(",").Length > 1) 
            {
                string[] varray = ind[0].Split(new char[] { ',' });
                switch (ind[1])
                {
                    case "length":
                        value = varray.Length.ToString();
                        break;
                    default:
                        float number = find_value(ind, 1, 0)[0];
                        value = varray[(int)number];
                        break;
                }
            }

            return value;
        }
        Control? getControl(string name, controlType controltype = controlType.None)
        {
            Control? control = new Control();

            switch (controltype)
            {
                case controlType.None:
                    control = AllControls.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Shape:
                    control = shapes.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.GLabel:
                    control = labels.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Textbox:
                    control = textboxes.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.GButton:
                    control = buttons.FirstOrDefault(x => x.Name == name);
                    break;
            }

            return control;
        }
        async Task<Control> createControl(string[] parts, controlType controltype, int index = 1, bool overwrite = true)
        {
            string name = parts[index];

            int violation = overwrite ? 0 : AllControls.Select(x => x.Name).Contains(name) == true ? 1 : UnusableNames.Contains(name) ? 2 : UnusableContains.Any(unusable => name.Contains(unusable)) ? 2 : 0;
            violation = IsNumericString(name) ? 2 : violation;

            string type = controltype == controlType.GButton ? "button" :
                controltype == controlType.Shape ? "shape" : 
                controltype == controlType.GLabel ? "label" : 
                controltype == controlType.Textbox ? "textbox" : 
                "AN_INTERNAL_ERROR";

            switch (violation)
            {
                case 1:
                    ErrorText(parts, ErrorTypes.alreadyMember, "Control", name);
                    break;
                case 2:
                    ErrorText(parts, ErrorTypes.violation, name: name);
                    break;
            }

            if (controltype == controlType.Shape)
            {
                GShape control = new GShape();
                if (getControl(name) != null && getControl(name).AccessibleName == type && overwrite) control = getControl(name) as GShape;
                else if (getControl(name) != null && getControl(name).AccessibleName != type && overwrite) ErrorText(parts, ErrorTypes.custom, custom: $"Expected '{getControl(name).AccessibleName}' in {SegmentSeperator} {codeLine}");
                try
                {
                    control.Name = name;
                    control.BackColor = Color.Black;
                    control = Change(control, parts, index + 1, false, false, false, false, true) as GShape;
                    control.Name = name;
                    control.AccessibleName = type;
                }
                catch
                {
                    ErrorText(parts, ErrorTypes.normal, type);
                }

                if (getControl(name) == null && overwrite)
                {
                    if(InPanel) Space.Controls.Add(control);
                    shapes.Add(control);
                }

                return control;
            }
            else if (controltype == controlType.GLabel)
            {
                GLabel control = new GLabel();
                if (getControl(name) != null && getControl(name).AccessibleName == type && overwrite) control = getControl(name) as GLabel;
                else if (getControl(name) != null && getControl(name).AccessibleName != type && overwrite) ErrorText(parts, ErrorTypes.custom, custom: $"Expected '{getControl(name).AccessibleName}' in {SegmentSeperator} {codeLine}");
                control.Name = name;
                control.AccessibleName = type;

                try
                {
                    control = Change(control, parts, index + 1) as GLabel;
                }
                catch
                {
                    ErrorText(parts, ErrorTypes.normal, type);
                }
                if (getControl(name) == null && overwrite)
                {
                    if (InPanel) Space.Controls.Add(control);
                    labels.Add(control);
                }
                return control;
            }
            else if (controltype == controlType.Textbox)
            {
                GTextBox control = new GTextBox();
                if (getControl(name) != null && getControl(name).AccessibleName == type && overwrite) control = getControl(name) as GTextBox;
                else if (getControl(name) != null && getControl(name).AccessibleName != type && overwrite) ErrorText(parts, ErrorTypes.custom, custom: $"Expected '{getControl(name).AccessibleName}' in {SegmentSeperator} {codeLine}");
                control.Name = name;
                control.AccessibleName = type;

                try
                {
                    control = Change(control, parts, index + 1) as GTextBox;
                }
                catch
                {
                    ErrorText(parts, ErrorTypes.normal, type);
                }

                if (getControl(name) == null && overwrite)
                {
                    if (InPanel) Space.Controls.Add(control);
                    textboxes.Add(control);
                }

                return control;
            }
            else if (controltype == controlType.GButton)
            {
                GButton control = new GButton();
                if (getControl(name) != null && getControl(name).AccessibleName == type && overwrite) control = getControl(name) as GButton;
                else if(getControl(name) != null && getControl(name).AccessibleName != type && overwrite) ErrorText(parts, ErrorTypes.custom, custom: $"Expected '{getControl(name).AccessibleName}' in {SegmentSeperator} {codeLine}");
                control.Name = name;
                control.AccessibleName = type;
                control.Click += GButtonClick;

                try
                {
                    control = Change(control, parts, index + 1) as GButton;
                }
                catch
                {
                    ErrorText(parts, ErrorTypes.normal, type);
                }

                if (getControl(name) == null && overwrite)
                {
                    if (InPanel) Space.Controls.Add(control);
                    buttons.Add(control);
                }
                return control;
            }
            return null;
        }
        Control Change(Control _control, string[] _parts, int index, bool text = true, bool white = true, bool nfifty = true, bool allzero = false, bool sides = false)
        {
            string[] parts = string.Join(" ", _parts.Skip(index)).Split(",");
            Control control = _control;
            string[]? txt = null;
            float[]? getpoints = null;
            int points = 0;
            control.BringToFront();
            if(RefreshOnControl) Space.Refresh();
            if (text)
            {
                txt = getString_value(parts, 0);
                txt[0] = !txt[0].StartsWith("text:") ? _control.Text : txt[0];
                control.Text = txt[0];
            }
            else if (sides)
            { 
                try
                {
                    if (control is GShape gs)
                    {
                        float[] floats = find_value(parts, 0, 4);
                        int poly = (int)floats[0];
                        getpoints = floats;
                        points = poly;
                        if (poly < 3)
                        {
                            ErrorText(parts, ErrorTypes.custom, custom: $"A minumum of 3 points required for the shape '{control.Name}' in {SegmentSeperator} {codeLine}");
                        }
                        else if (poly == 3) gs.type = GShape.Type.Triangle;
                        else if (poly == 4) gs.type = GShape.Type.Square;
                        else
                        {
                            gs.type = GShape.Type.Polygon;
                            gs.Poly = poly;
                        }
                        gs.Refresh();
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'shape' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                    }
                }
                catch
                {
                    control = control is GShape gss ? gss : new GShape(GShape.Type.Square);
                }
            }
            try
            {
                /* Old_Thing
                parts = parts.Length == 0 || (parts.Length == 1 && parts[0] == "") ? new string[] { "0" } : parts;
                float[] v = find_value(parts, points != 0 ? (int)getpoints[1] : txt != null ? int.Parse(txt[1]) : 0, 0, overide:0);
                int x = (int)v[0];
                float[] v1 = find_value(parts, (int)v[1], 0, overide: 0);
                int y = (int)v1[0];
                float[] v2 = find_value(parts, (int)v1[1], allzero ? 0 : nfifty ? 75 : 50, overide: 0);
                int width = (int)v2[0];
                float[] v3 = find_value(parts, (int)v2[1], allzero ? 0 : nfifty ? 25 : 50, overide: 0);
                int height = (int)v3[0];
                Color bc = control.BackColor;
                if(parts.Length - 1 >= (int)v3[1]) bc = returncolor(_parts, parts, (int)v3[1], control.BackColor, allzero ? 0 : sides ? 0 : 255);
                Color fc = control.ForeColor;
                if(parts.Length - 1 >= (int)v3[1] + 1) fc = returncolor(_parts, parts, (int)v3[1] + 1, control.ForeColor, allzero ? 0 : sides ? 255 : 0);
                int v9 = (int)v3[1] + 2;
                if (control is GTextBox con)
                {
                    if (parts.Length - 1 >= v9)
                    {
                        con.Multiline = (bool)BoolCheck(parts, v9);
                    }
                    if (parts.Length - 1 >= v9 + 1)
                    {
                        bool tr = (bool)BoolCheck(parts, v9 + 1);
                        con.WordWrap = tr;
                        con.AcceptsReturn = !tr;
                        con.AcceptsTab = !tr;
                        con.AllowDrop = !tr;
                    }
                    if (parts.Length - 1 >= v9 + 2)
                    {
                        ScrollBars scroll = con.ScrollBars;
                        bool tr = (bool)BoolCheck(parts, v9 + 2);
                        con.ScrollBars = 
                            scroll == ScrollBars.None && tr ? ScrollBars.Vertical :
                            scroll == ScrollBars.Horizontal && tr ? ScrollBars.Both :
                            scroll == ScrollBars.Both && !tr ? ScrollBars.Horizontal:
                            scroll;
                    }
                    if (parts.Length - 1 >= v9 + 3)
                    {
                        ScrollBars scroll = con.ScrollBars;
                        bool tr = (bool)BoolCheck(parts, v9 + 3);
                        con.ScrollBars =
                            scroll == ScrollBars.None && tr ? ScrollBars.Horizontal :
                            scroll == ScrollBars.Vertical && tr ? ScrollBars.Both :
                            scroll == ScrollBars.Both && !tr ? ScrollBars.Horizontal :
                            scroll;
                    }
                    if (parts.Length - 1 >= v9 + 4)
                    {
                        con.PasswordChar = (parts[(int)(v9 + 4)]).Trim().ToCharArray()[0];
                    }
                }
                if (control is GShape gs)
                {
                    if (parts.Length - 1 >= v9)
                    {
                        gs = custompoints(parts, v9, gs);
                    }
                }
                if (control is GLabel lb)
                {
                    if (parts.Length - 1 >= v9)
                    {
                        control.AutoSize = (bool)BoolCheck(parts, v9);
                    }
                }
                control.Left = x;
                control.Top = y;
                control.Width = width;
                control.Height = height;
                control.BackColor = bc;
                control.ForeColor = fc;
                */
                if (!(parts.Length == 0 || (parts.Length == 1 && parts[0] == ""))) throw new Exception();
            }
            catch
            {
                int x = control.Left,
                    y = control.Top,
                    w = allzero ? 0 : nfifty ? 75 : control.Width,
                    h = allzero ? 0 : nfifty ? 25 : control.Height;
                foreach(string p in parts)
                {
                    string[] values = p.Split(':');
                    string[] before = getString_value(values, 0);
                    string[] after = getString_value(values, int.Parse(before[1]));
                    switch(before[0].Trim().ToLower())
                    {
                        case "focus":
                            {
                                if ((bool)BoolCheck(after, 0)) control.Focus();
                            }
                            break;
                        case "readonly":
                            {
                                if (control is GTextBox g)
                                    g.ReadOnly = (bool)BoolCheck(after, 0);
                                else
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'textbox' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                            }
                            break;
                        case "enable":
                            {
                                control.Enabled = (bool)BoolCheck(after, 0);
                            }
                            break;
                        case "font":
                            {
                                string all = string.Join(" ", after[0].Split(" ")).Trim();
                                bool thing2 = all.StartsWith("[") && all.EndsWith("]#suppresserror".ToLower());
                                string fontType = "Segoe UI";
                                int fontSize = 9;
                                FontStyle fontStyle = FontStyle.Regular;
                                if ((all.StartsWith("[") && all.EndsWith("]")) || thing2)
                                {
                                    if (!thing2) all = all.Substring(1, all.Length - 2);
                                    else all = all.Substring(1, all.Length - 1).Replace("]#suppresserror", "");
                                    string[] seperator = all.Split(";");
                                    for (int i = 0; i < seperator.Length; i++)
                                    {
                                        string[] strings = getString_value(seperator, i);
                                        seperator[i] = strings[0];
                                    }
                                    if (seperator.Length == 3)
                                    {
                                        if (IsRealFont(seperator[0]))
                                        {
                                            fontType = seperator[0];
                                        }
                                        else
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Exception for '{control.Name}' in line {SegmentSeperator} {codeLine}. '{seperator[0]}' is not a valid font. Try 'Arial' or go to https://learn.mcrosoft.com for more inWindowation about supported WinWindows fonts.");
                                        }
                                        try
                                        {
                                            float[] floats = find_value(seperator, 1, -1);
                                            fontSize = (int)floats[0];
                                            if (fontSize < 0) new Exception("");
                                        }
                                        catch
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected a number greater greater than zero for font size value in {SegmentSeperator} {codeLine}");
                                        }
                                        if (Enum.TryParse(char.ToUpper(seperator[2][0]) + seperator[2].Substring(1).ToLower(), out FontStyle parsedFontStyle))
                                        {
                                            fontStyle = parsedFontStyle;
                                        }
                                        else
                                        {
                                            ErrorText(parts, ErrorTypes.custom, custom: $"Exception for '{control.Name}' in line {SegmentSeperator} {codeLine}. '{seperator[2]}' is not a valid font style. Valid styles are: {string.Join(", ", Enum.GetNames(typeof(FontStyle)))}.");
                                        }
                                    }
                                    else
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"Requires 3 values for font in {SegmentSeperator} {codeLine}");
                                    }
                                }
                                else if (parts.Length != 1)
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected '[' and ']' for font value in {SegmentSeperator} {codeLine}");
                                }
                                control.Font = new Font(fontType, fontSize, fontStyle);
                            }
                            break;
                        case "point":
                        case "points":
                            {
                                if (control is GShape gs)
                                {
                                    gs = custompoints(after, 0, gs);
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'shape' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        case "auto":
                        case "autosize":
                            {
                                control.AutoSize = (bool)BoolCheck(after, 0);
                            }
                            break;
                        case "multi":
                        case "multiline":
                            {
                                if (control is GTextBox tb)
                                {
                                    tb.Multiline = (bool)BoolCheck(after, 0);
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'textbox' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        case "wrap":
                        case "wordwrap":
                            {
                                if (control is GTextBox tb)
                                {
                                    bool tr = (bool)BoolCheck(after, 0);
                                    tb.WordWrap = tr;
                                    tb.AcceptsReturn = !tr;
                                    tb.AcceptsTab = !tr;
                                    tb.AllowDrop = !tr;
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'textbox' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        case "vertical":
                        case "verticalscrollbar":
                            {
                                if (control is GTextBox tb)
                                {
                                    ScrollBars scroll = tb.ScrollBars;
                                    bool tr = (bool)BoolCheck(after, 0);
                                    tb.ScrollBars =
                                        scroll == ScrollBars.None && tr ? ScrollBars.Vertical :
                                        scroll == ScrollBars.Horizontal && tr ? ScrollBars.Both :
                                        scroll == ScrollBars.Both && !tr ? ScrollBars.Horizontal :
                                        scroll;
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'textbox' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        case "horizantal":
                        case "horizantalscrollbar":
                            {
                                if (control is GTextBox tb)
                                {
                                    ScrollBars scroll = tb.ScrollBars;
                                    bool tr = (bool)BoolCheck(after, 0);
                                    tb.ScrollBars =
                                        scroll == ScrollBars.None && tr ? ScrollBars.Horizontal :
                                        scroll == ScrollBars.Vertical && tr ? ScrollBars.Both :
                                        scroll == ScrollBars.Both && !tr ? ScrollBars.Vertical :
                                        scroll;
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'textbox' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        case "t":
                        case "text":
                            {
                                control.Text = after[0];
                            }
                            break;
                        case "x":
                            {
                                float[] floats = find_value(after, 0, 0);
                                x = (int)floats[0];
                            }
                            break;
                        case "y":
                            {
                                float[] floats = find_value(after, 0, 0);
                                y = (int)floats[0];
                            }
                            break;
                        case "h":
                        case "height":
                            {
                                float[] floats = find_value(after, 0, 50);
                                h = (int)floats[0];
                            }
                            break;
                        case "w":
                        case "width":
                            {
                                float[] floats = find_value(after, 0, 50);
                                w = (int)floats[0];
                            }
                            break;
                        case "bc":
                        case "backcolor":
                            {
                                control.BackColor = returncolor(_parts, after, 0, control.BackColor, allzero ? 0 : sides ? 0 : 255);
                            }
                            break;
                        case "fc":
                        case "forecolor":
                            {
                                control.ForeColor = returncolor(_parts, after, 0, control.ForeColor, allzero ? 0 : sides ? 255 : 0);
                            }
                            break;
                        case "poly":
                        case "p":
                            {
                                if(control is GShape gs)
                                {
                                    float[] floats = find_value(after, 0, 4);
                                    int poly = (int)floats[0];
                                    if (poly < 3)
                                    {
                                        ErrorText(parts, ErrorTypes.custom, custom: $"A minumum of 3 points required for the shape '{control.Name}' in {SegmentSeperator} {codeLine}");
                                    }
                                    else if (poly == 3) gs.type = GShape.Type.Triangle;
                                    else if (poly == 4) gs.type = GShape.Type.Square;
                                    else
                                    {
                                        gs.type = GShape.Type.Polygon;
                                        gs.Poly = poly;
                                    }
                                    gs.Refresh();
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'shape' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                                }
                            }
                            break;
                        default:
                            ErrorText(_parts, ErrorTypes.custom, custom: $"Expected one of the following values 'x', 'y', 'w', 'h', 'bc', or 'fc' for '{control.Name}' in {SegmentSeperator} {codeLine}");
                            break;
                    }
                }
                control.Left = x;
                control.Top = y;
                control.Width = w;
                control.Height = h;
            }
            return control;
        }
        bool IsRealFont(string fontName)
        {
            foreach (FontFamily fontFamily in FontFamily.Families)
            {
                if (fontFamily.Name.Equals(fontName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        Color returncolor(string[] allparts, string[] parts, int index, Color c, int def = -1)
        {
            int r = def == -1 ? c.R : def,
                g = def == -1 ? c.G : def,
                b = def == -1 ? c.B : def;
            try
            {
                string[] strings = getString_value(parts, index);
                string all = string.Join("", strings[0].Split(" ")).Trim();
                bool thing2 = all.StartsWith("[") && all.EndsWith("]#suppresserror".ToLower());
                if ((all.StartsWith("[") && all.EndsWith("]")) || thing2)
                {
                    if(!thing2) all = all.Substring(1, all.Length - 2);
                    else all = all.Substring(1, all.Length - 1).Replace("]#suppresserror", "");
                    string[] seperator = all.Split(";");
                    if (seperator.Length == 3)
                    {
                        float[] v1 = find_value(seperator, 0, r);
                        r = (int)v1[0];
                        float[] v2 = find_value(seperator, (int)v1[1], g);
                        g = (int)v2[0];
                        float[] v3 = find_value(seperator, (int)v2[1], b);
                        b = (int)v3[0];
                    }
                    else
                    {
                        ErrorText(allparts, ErrorTypes.custom, custom: $"Requires 3 values for color in {SegmentSeperator} {codeLine}");
                    }
                }
                else if (all.Equals("transparent"))
                {
                    c = Color.Transparent;
                }
                else if(parts.Length != 1)
                {
                    ErrorText(allparts, ErrorTypes.custom, custom: $"Expected '[' and ']' for color value in {SegmentSeperator} {codeLine}");
                }
                c = Color.FromArgb(r, g, b);
            }
            catch
            {
                c = Color.FromArgb(0, 0, 0);
            }
            return c;
        }
        GShape custompoints(string[] parts, int index, GShape gs)
        {
            string[] strings = getString_value(parts, index);
            string all = string.Join("", strings[0].Split(" ")).Trim();
            if (all.StartsWith("[") && all.EndsWith("]"))
            {
                try
                {
                    all = all.Substring(1, all.Length - 2);
                    List<PointF> ppoints = new List<PointF>();
                    string[] seperator = all.Split(";");
                    if (seperator.Length > 2)
                    {
                        foreach (string _point in seperator)
                        {
                            if (_point.StartsWith("(") && _point.EndsWith(")"))
                            {
                                string point = _point.Substring(1, _point.Length - 2);
                                string[] polysides = point.Split("*");
                                if (polysides.Length == 2)
                                {
                                    ppoints.Add(new PointF(float.Parse(polysides[0]), float.Parse(polysides[1])));
                                }
                                else
                                {
                                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected 2 values for a single point in points value in {SegmentSeperator} {codeLine}");
                                }
                            }
                            else
                            {
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected '(' and ')' for points value in {SegmentSeperator} {codeLine}");
                            }
                        }
                    }
                    else
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"A minumum of 3 points required for the shape '{gs.Name}' in {SegmentSeperator} {codeLine}");
                    }
                    gs.Points = ppoints.ToArray();
                }
                catch
                {
                    gs.Points = new PointF[] { };
                }
            }
            else
            {
                ErrorText(parts, ErrorTypes.custom, custom: $"Expected '[' and ']' for points value in {SegmentSeperator} {codeLine}");
            }
            gs.type = GShape.Type.Custom;
            gs.Refresh();
            return gs;
        }
        string[] getString_value(string[] parts, int next, bool all = false, bool useVar = true, bool useEquation = true, bool useRaw = true, string def = "", bool usecolon = true)
        {
            string value = def;
            string val = "";

            if (parts.Length - 1 >= next)
            {

                string s = !all ? parts[next] : string.Join(" ", parts.Skip(next).TakeWhile(part => part != "//"));
                val = s;
                List<string> texts = s.Split(" ").ToList();

                if (useEquation && s.Contains(@"\(") && !s.Contains(@"\\("))
                {
                    for (int i = 0; i < texts.Count; i++)
                    {
                        next++;
                        if (texts[i].StartsWith(@"\(") && !s.StartsWith(@"\\("))
                        {
                            string brackets = "";
                            int started = i;
                            int count = 1;
                            int ended = 1;
                            for (int l = i; l < texts.Count; l++)
                            {
                                if (ended == 1)
                                {
                                    count++;
                                    brackets += texts[l];
                                    if (l < texts.Count - 1) brackets += " ";
                                }
                                if (texts[l].EndsWith(@")\") && !s.EndsWith(@")\\"))
                                {
                                    ended = 2;
                                }
                            }
                            if (ended != 0)
                            {
                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                string result = SolveEquation(equation);
                                texts[started] = result;

                                texts.RemoveRange(started + 1, count - 2);
                            }
                        }
                    }
                }
                else 
                {
                    if (!all)
                    {
                        next++;
                    }
                    else
                    {
                        for (int i = next; i < s.Split(" ").Length; i++)
                        {
                            next++;
                        }
                    }
                }

                string text = "";
                for (int i = 0; i < texts.Count; i++)
                {
                    bool switched = false;
                    string sw_t = texts[i];
                    if (useRaw)
                    {
                        texts[i] = texts[i].Contains(@"\n") && !texts[i].Contains(@"\\n") ? texts[i].Replace(@"\n", Environment.NewLine) : texts[i].Contains(@"\\n") ? texts[i].Replace(@"\\n", @"\n") : texts[i];
                        texts[i] = texts[i].Contains(@"\!") && !texts[i].Contains(@"\\!") ? texts[i].Replace(@"\!", string.Empty) : texts[i].Contains(@"\\!") ? texts[i].Replace(@"\\!", @"\!") : texts[i];
                        texts[i] = texts[i].Contains(@"\_") && !texts[i].Contains(@"\\_") ? texts[i].Replace(@"\_", " ") : texts[i].Contains(@"\\_") ? texts[i].Replace(@"\\_", @"\_") : texts[i];
                        texts[i] = texts[i].Contains(@"\;") && !texts[i].Contains(@"\\;") ? texts[i].Replace(@"\;", "|") : texts[i].Contains(@"\\;") ? texts[i].Replace(@"\\;", @"\;") : texts[i];
                        texts[i] = texts[i].Contains(@"\=") && !texts[i].Contains(@"\\=") ? texts[i].Replace(@"\=", "=") : texts[i].Contains(@"\\=") ? texts[i].Replace(@"\\=", @"\=") : texts[i];
                        texts[i] = texts[i].Contains(@"\c") && !texts[i].Contains(@"\\c") ? texts[i].Replace(@"\c", ",") : texts[i].Contains(@"\\c") ? texts[i].Replace(@"\\c", @"\c") : texts[i];
                        texts[i] = texts[i].Contains(@"\e") && !texts[i].Contains(@"\\e") ? texts[i].Replace(@"\e", "!") : texts[i].Contains(@"\\e") ? texts[i].Replace(@"\\e", @"\e") : texts[i];
                        texts[i] = texts[i].Contains(@"\$") && !texts[i].Contains(@"\\$") ? texts[i].Replace(@"\$", ":") : texts[i].Contains(@"\\$") ? texts[i].Replace(@"\\$", @"\$") : texts[i];
                        texts[i] = texts[i].Contains(@"\&") && !texts[i].Contains(@"\\&") ? texts[i].Replace(@"\&", ";") : texts[i].Contains(@"\\&") ? texts[i].Replace(@"\\&", @"\&") : texts[i];
                        texts[i] = texts[i].Replace(@"\\(", @"\(");
                        texts[i] = texts[i].Replace(@")\\", @")\");
                        switched = sw_t == texts[i] ? switched : true;
                    }
                    if (useVar && ((texts[i].StartsWith("'") && texts[i].EndsWith("'")) || Regex.Matches(texts[i], "'").Count > 1))
                    {
                        string[] varray = texts[i].Split("'");
                        for (int j = 0; j < varray.Length; j++)
                        {
                            if (j % 2 == 1)
                            {
                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (varray[j] == vars[k].Name) varray[j] = vars[k].value();
                                }
                            }
                        }
                        texts[i] = string.Join("", varray);
                    }
                    if (useRaw)
                    {
                        texts[i] = texts[i].Contains(@"\""") && !texts[i].Contains(@"\\""") ? texts[i].Replace(@"\""", "'") : texts[i].Contains(@"\\""") ? texts[i].Replace(@"\\""", @"\""") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = !switched && texts[i].Contains(@"\") && !texts[i].Contains(@"\\") ? texts[i].Replace(@"\", string.Empty) : !switched && texts[i].Contains(@"\\") ? texts[i].Replace(@"\\", @"\") : texts[i];
                    }
                    text += texts[i];
                    if (i < texts.Count - 1) text += " ";
                }
                val = text;
            }

            value = val;
            if (usecolon) value = ColonResponse(value, parts);

            return new string[] { value, next.ToString() };
        }
        float[] getEquationWithNext(string value, int a, string[] parts)
        {
            StringBuilder brackets = new StringBuilder();
            int ended = 0, next = 0;

            if (value.StartsWith("("))
            {
                ended = 1;
                for (int l = a; l < parts.Length; l++)
                {
                    if (ended == 1)
                    {
                        next++;
                        brackets.Append(parts[l]);
                        if (l < parts.Length - 1) brackets.Append(" ");
                    }
                    if (parts[l].EndsWith(")"))
                    {
                        ended = 2;
                    }
                }
            }
            if (ended != 0)
            {
                string equation = brackets.ToString().TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                string result = SolveEquation(equation);
                value = result;
            }
            else if (ended == 1)
            {
                ErrorText(parts, ErrorTypes.custom, custom: $"Syntax error in {SegmentSeperator} {codeLine}. Expected ')' to end equation.");
            }

            return new float[] { float.Parse(value), next };
        }
        float[] find_value(string[] parts, int next, int def, bool? var = true, bool colon = true, int overide = -1)
        {
            float v = def;
            if (parts.Length - 1 >= next || overide != -1)
            {
                try
                {
                    next = parts.Length - 1 >= next ? next : overide;
                    string s = parts[next];
                    if (var == true)
                    {
                        for (int j = 0; j < vars.Count; j++)
                        {
                            if (vars[j].Name == parts[next] && vars[j].isNumber())
                            {
                                s = vars[j].value();
                            }
                        }
                    }
                    if (colon) s = ColonResponse(s, parts);
                    v = int.Parse(s);
                    next++;
                }
                catch
                {
                    float[] result = getEquationWithNext(parts[next], next, parts);
                    v = result[0];
                    next += (int)result[1];
                }
            }
            return new float[] { v, next };
        }
        string SolveEquation(string equation)
        {
            try
            {
                // Replace variables with their values
                foreach (Var variable in vars)
                {
                    equation = equation.Replace(variable.Name, variable.value());
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("expression", typeof(string), equation);
                DataRow row = dt.NewRow();
                dt.Rows.Add(row);
                object result = row["expression"];
                return result.ToString();
            }
            catch
            {
                return "Error: Unable to solve the equation.";
            }
        }
        #endregion

        #region Control_Events
        async void GButtonClick(object sender, EventArgs e)
        {
            GButton button = (GButton)sender;
            button.isclick = 1;
            if (button.click != "" && button.click != null)
            {
                G_click(sender, e);
            }
        }
        enum eventType
        {
            click,
            mousehover,
            move,
            scale,
            backcolor,
            forecolor,
            text,
            image,
            imagelayout,
            font,
            scroll,
            focused,
            controladded,
            controlremoved,
            defocused,
            close,
            open,
            enabledchanged,
            keydown,
            keyup,
            keypress,
            resized,
            resizestart,
            resizeend
        }
        private void G_click(object sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.click);
        }
        private void G_mousehover(object sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.mousehover);
        }
        private void G_move(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.move);
        }
        private void G_scale(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.scale);
        }
        private void G_backcolor(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.backcolor);
        }
        private void G_forecolor(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.forecolor);
        }
        private void G_text(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.text);
        }
        private void G_font(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.font);
        }
        private void G_image(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.image);
        }
        private void G_imagetype(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.imagelayout);
        }
        private void G_scroll(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.scroll);
        }
        private void G_focused(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.focused);
        }
        private void G_ctroladded(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.controladded);
        }
        private void G_controlremoved(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.controlremoved);
        }
        private void G_defocused(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.defocused);
        }
        private void G_close(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.close);
        }
        private void G_open(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.open);
        }
        private void G_enabledchanged(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.enabledchanged);
        }
        private void G_keydown(object? sender, KeyEventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.keydown);
        }
        private void G_keyup(object? sender, KeyEventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.keyup);
        }
        private void G_press(object? sender, KeyPressEventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.keypress);
        }
        private void G_resize(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.resized);
        }
        private void G_resizedstart(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.resizestart);
        }
        private void G_resizedend(object? sender, EventArgs e)
        {
            PlayScriptFromEvent(sender, eventType.resizeend);
        }
        private async void PlayScriptFromEvent(object sender, eventType eventtype)
        {
            if (!playing) return;
            Window? w = sender.ToString().Contains(nameof(w)) ? sender as Window : sender is Window ? sender as Window : null;
            GShape? s = sender.ToString().Contains(nameof(s)) ? sender as GShape : sender is GShape ? sender as GShape : null;
            GLabel? l = sender.ToString().Contains(nameof(l)) ? sender as GLabel : sender is GLabel ? sender as GLabel : null;
            GButton? b = sender.ToString().Contains(nameof(b)) ? sender as GButton : sender is GButton ? sender as GButton : null;
            GTextBox? t = sender.ToString().Contains(nameof(t)) ? sender as GTextBox : sender is GTextBox ? sender as GTextBox : null;
            string file = "";
            switch (eventtype)
            {
                case eventType.click: file = s != null ? s.click : l != null ? l.click : b != null ? b.click : t != null ? t.click : w != null ? w.click : "null"; break;
                case eventType.mousehover: file = s != null ? s.mousehover : l != null ? l.mousehover : b != null ? b.mousehover : t != null ? t.mousehover : w != null ? w.mousehover : "null"; break;
                case eventType.text: file = s != null ? s.text : l != null ? l.text : b != null ? b.text : t != null ? t.text : w != null ? w.text : "null"; break;
                case eventType.move: file = s != null ? s.move : l != null ? l.move : b != null ? b.move : t != null ? t.move : w != null ? w.move : "null"; break;
                case eventType.scale: file = s != null ? s.scale : l != null ? l.scale : b != null ? b.scale : t != null ? t.scale : "null"; break;
                case eventType.backcolor: file = s != null ? s.backcolor : l != null ? l.backcolor : b != null ? b.backcolor : t != null ? t.backcolor : w != null ? w.backcolor : "null"; break;
                case eventType.forecolor: file = s != null ? s.forecolor : l != null ? l.forecolor : b != null ? b.forecolor : t != null ? t.forecolor : w != null ? w.forecolor : "null"; break;
                case eventType.font: file = s != null ? s.font : l != null ? l.font : b != null ? b.font : t != null ? t.font : w != null ? w.forecolor : "null"; break;
                case eventType.image: file = s != null ? s.image : l != null ? l.image : b != null ? b.image : t != null ? t.image : w != null ? w.image : "null"; break;
                case eventType.imagelayout: file = s != null ? s.imagelayout : l != null ? l.imagelayout : b != null ? b.imagelayout : t != null ? t.imagelayout : w != null ? w.imagelayout : "null"; break;
                case eventType.scroll: file = w.scroll; break;
                case eventType.focused: file = w.focused; break;
                case eventType.controladded: file = w.controladded; break;
                case eventType.controlremoved: file = w.controlremoved; break;
                case eventType.defocused: file = w.defocused; break;
                case eventType.close: file = w.close; break;
                case eventType.open: file = w.open; break;
                case eventType.enabledchanged: file = w.enabledchanged; break;
                case eventType.keydown: file = w.keydown; break;
                case eventType.keyup: file = w.keyup; break;
                case eventType.resized: file = w.resized; break;
                case eventType.resizestart: file = w.resizedstart; break;
                case eventType.resizeend: file = w.resizedend; break;
            }
            await PlaySwitch(jumpsto: $"file play {file}");
        }
        #endregion

        #region Public_Helpers

        /// <summary>
        /// Stops the code currently being played
        /// </summary>
        public void Stop()
        {
            _pplaying = false;
            returnOutput = "";
            devDisplay = true;
            devportal = 0;
            ifmany = 0;
            lastif = true;
            loopmany = 0;
            foreach (Window Window in windows)
            {
                Window.Close();
            }
        }
        /// <summary>
        /// Sets the Console Input to the inputted text
        /// </summary>
        /// <param name="text">Text Input</param>
        public void ConsoleInput(string text)
        {
            if (!playing && text == "help")
            {
                string help = @"Need help? Please go to the official EZCode website: https://ez-code.web.app";
                AddText(help);
            }
            if (!playing) return;
            senttext = text;
            sent = true;
        }
        /// <summary> 
        /// Sets the Key Input to the inputted key for the keydown
        /// </summary>
        public void KeyInput_Down(KeyEventArgs e) {
            Keys.Add(e.KeyCode);
        }
        public void KeyInput_Down(object sender, KeyEventArgs e) { KeyInput_Down(e); }
        /// <summary>
        /// Sets the Key Input to the inputted key for the keyup
        /// </summary>
        public void KeyInput_Up(KeyEventArgs e) {
            Keys.Remove(e.KeyCode);
        }
        public void KeyInput_Up(object sender, KeyEventArgs e) { KeyInput_Up(e); }
        /// <summary>
        /// Gets The Mouse Position
        /// </summary>
        public void MouseInput_Move(MouseEventArgs e) {
            MousePosition = Cursor.Position;
        }
        public void MouseInput_Move(object sender, MouseEventArgs e) { MouseInput_Move(e); }
        /// <summary>
        /// Sets the Mouse Input to the inputted GButton for the MouseDown
        /// </summary>
        public void MouseInput_Down(MouseEventArgs e) {
            mouseButtons.Add(e.Button);
        }
        public void MouseInput_Down(object sender, MouseEventArgs e) { MouseInput_Down(e); }
        /// <summary>
        /// Sets the Mouse Input to the inputted GButton for the MouseUp
        /// </summary>
        public void MouseInput_Up(MouseEventArgs e) { 
            mouseButtons.Remove(e.Button); 
        }
        public void MouseInput_Up(object sender, MouseEventArgs e) { MouseInput_Up(e); }
        /// <summary>
        /// Sets the Mouse Wheel Input to the delta of the mouse as: -1, 0, or 1
        /// </summary>
        public void MouseInput_Wheel(MouseEventArgs e)
        {
            mouseWheel = e.Delta;
        }
        public void MouseInput_Wheel(object sender, MouseEventArgs e) { MouseInput_Wheel(e); }
        /// <summary>
        /// Put this in OnTextChange. This decides to scroll to end, sets the Normal/Error Colors, and sets the RichTextbox.
        /// </summary>
        /// <param name="Console">The RichTextbox that has the Console Output</param>
        /// <param name="output">Normal Output Color</param>
        /// <param name="error">Error Color</param>
        /// <param name="scrollToEnd">Scrolls to the end of the RichTextbox.</param>
        /// <param name="showFileError">Show File Directory in error</param>
        public RichTextBox ScrollToEnd(bool scrollToEnd, Color? output = null, Color? error = null, RichTextBox? Console = null, bool? showFileError = null)
        {
            normalColor = error != null ? (Color)output : Color.Black;
            errorColor = output != null ? (Color)error : Color.Red;
            RichConsole = Console != null ? Console : RichConsole;
            showFileInError = showFileError != null ? (showFileError == true ? true : false) : showFileInError;
            Console = Console != null ? Console : RichConsole;
            if (scrollToEnd == true)
            {
                Console.ScrollToCaret();
                Console.SelectionStart = Console.TextLength;
            }
            return Console;
        }
        /// <summary>
        /// Adds text to the console
        /// </summary>
        /// <param name="text">Text being added</param>
        /// <param name="error">If the output is an error</param>
        /// <param name="control">Output of RichTextbox.</param>
        /// <param name="newLine">Automatic Newline </param>
        public void AddText(string text, bool error = false, RichTextBox? control = null, bool? newLine = true)
        {
            text = string.Join(" ", text.Split(" ").TakeWhile(x => x != "#suppress").TakeWhile(y=>y != "#create"));
            text = newLine == true ? text + Environment.NewLine : text;
            ConsoleText += text;
            RichConsole = control != null ? control : RichConsole;
            if (RichConsole != null)
            {
                if(RichConsole.Text.Length + 100 > RichConsole.MaxLength)
                {
                    RichConsole.Text = "";
                }
                RichConsole.SelectionStart = RichConsole.TextLength;
                RichConsole.SelectionLength = 0;
                RichConsole.SelectionColor = error ? errorColor : normalColor;
                RichConsole.AppendText(text);
                RichConsole.SelectionColor = RichConsole.ForeColor;
            }
        }
        /// <summary>
        /// Enum for different errors
        /// </summary>
        public enum ErrorTypes
        {
            none,
            violation,
            normal,
            missingControl,
            alreadyMember,
            missingVar,
            missingSound,
            missingGroup,
            missingWindow,
            unkown,
            custom
        }
        /// <summary>
        /// Add an error to the output
        /// </summary>
        /// <param name="parts">To check to see if there is an overide in the line</param>
        /// <param name="error">The type of error for uniWindow output</param>
        /// <param name="keyword">keyword, for error type</param>
        /// <param name="name">name, for the error type</param>
        /// <param name="custom">cutom error, this makes the 'name' and 'keyword' parameters not needed</param>
        public string ErrorText(string[] parts, ErrorTypes error, string keyword = "keyword", string name = "name", string custom = "An Error Occured", bool returnoutput = true)
        {
            string text = 
                error == ErrorTypes.unkown ? $"An error occured in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.normal ? $"An error occured with '{keyword}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.violation ? $"Naming violation in {SegmentSeperator} {codeLine}. '{name}' can not be used as a name" : 
                error == ErrorTypes.missingControl ? $"Could not find a Control named '{name}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.missingVar ? $"Could not find a Variable named '{name}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.missingSound ? $"Could not find a Sound Player named '{name}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.missingGroup ? $"Could not find a Group named '{name}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.missingWindow ? $"Could not find a Window named '{name}' in {SegmentSeperator} {codeLine}" :
                error == ErrorTypes.alreadyMember ? $"Naming violation in {SegmentSeperator} {codeLine}. There is already a '{keyword}' named '{name}'" :
                error == ErrorTypes.custom ? custom : "An Error Occured, We don't know why. If it helps, it was on line " + codeLine;

            if ((parts.Contains("#suppress") && parts.Contains("error")) || (parts.Contains("#") && parts.Contains("suppress") && parts.Contains("error"))) return "";
            if (showFileInError)
            {
                text = ScriptDirectory != "" ? $"{ScriptDirectory}: {text}" : text;
            }
            if (returnoutput) returnOutput += text; 
            AddText(text, true, RichConsole, true);
            return text;
        }
        /// <summary>
        /// Set's The Script's Directory
        /// </summary>
        /// <param name="scriptDirectory">The file path that is playing</param>
        /// <returns></returns>
        public bool SetScriptDirectory(string scriptDirectory)
        {
            if (validpathcheck(scriptDirectory))
            {
                ScriptDirectory = scriptDirectory;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Give the window an Unhandled exception event and put this method in there.
        /// Put this in next to the Initialized line:
        /// AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;
        /// This assumes that ezcode has already been declared:
        /// EZCode.EZCode ezcode = new EZCode.EZCode();
        /// </summary>
        public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            ErrorText(new string[] { }, ErrorTypes.unkown);
        }
        #endregion
    }
}