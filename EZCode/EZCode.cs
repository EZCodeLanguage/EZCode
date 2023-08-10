using Groups;
using NCalc;
using Objects;
using Sound;
using System.Data;
using System.Drawing;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Variables;
using Group = Groups.Group;
using Player = Sound.Player;
using Types = Variables.Ivar.Types;

namespace EZCode
{
    /// <summary>
    /// This is the Official EZCode Source Code. See Version '<seealso cref="Version"/>'
    /// </summary>
    public class EZCode
    {
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
        private List<Label> labels = new List<Label>();
        /// <summary>
        /// List for textboxes
        /// </summary>
        private List<TextBox> textboxes = new List<TextBox>();
        /// <summary>
        /// List for buttons
        /// </summary>
        private List<Button> buttons = new List<Button>();
        /// <summary>
        /// List for gameobjects
        /// </summary>
        private List<GObject> gameObjects = new List<GObject>();
        /// <summary>
        /// List for variables
        /// </summary>
        private List<Var> vars = new List<Var>();
        /// <summary>
        /// List of Groups
        /// </summary>
        private List<Group> Group = new List<Group>();
        /// <summary>
        /// List of all controls
        /// </summary>
        List<Control> AllControls = new List<Control>();
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
        bool playing { get; set; }
        /// <summary>
        /// string array for naming violations
        /// </summary>
        public string[] UnusableNames = new string[] { "await", "button", "print", "group", "clear", "write", "stop",
            "event", "textbox", "multiLine", "object", "image", "label", "font", "move", "scale", "color", "intersects",
            "var", "input", "list", "file", "sound", "if", "//", "#create", "#suppress", "#", "system:", "?", "=", "!",
            ">", "<", "+", "-", "|", "\\", ",", "@", "#", "$", "%", "^", "&", "*", "(", ")", "/", "~", "`", ".", ":", ";" };
        /// <summary>
        /// char array for unusable names that can't even be used once in the name
        /// </summary>
        public char[] UnusableContains = new char[] { '?', '=', '!', ':', '>', '<', '|', '\\', '#', '(', ')' };
        /// <summary>
        /// The character tht seperates each line of code. Automatically { '\n', '|' } but this can be added to if needed 
        /// </summary>
        public char[] seperatorChars = new char[] { '\n', '|' };
        /// <summary>
        /// List of keys being pressed
        /// Needs to have Key_Down and Key_Up event connected to KeyInput_Down and KeyInput_Up
        /// </summary>
        public HashSet<Keys> Keys = new HashSet<Keys>();

        /// <summary>
        /// Initializes the EZCode Player with the provided parameters.
        /// </summary>
        /// <param name="_space">The Control used as the output space. Only needed if the code includes visual output, like 'object' or 'button'</param>
        /// <param name="_directory">The directory path where the file is located. Only needed if the code referenses another file locally using the '~/' character.</param>
        /// <param name="_Console">The RichTextbox that has the error color is wanted.</param>
        public void Initialize(string _directory = "NOTHING", Control _space = null, RichTextBox _console = null, bool _showFileWithErrors = true, bool _showStartAndEnd = true, bool _clearConsole = true)
        {
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
            Object,
            Textbox,
            Label,
            Button
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
            Group.Clear();
            gameObjects.Clear();
            labels.Clear();
            textboxes.Clear();
            buttons.Clear();
            if (ClearConsole) RichConsole.Clear();
            if (showStartAndEnd) AddText("Build Start");
            Code = code;
            string output = string.Empty;
            string[] lines = code.Split(seperatorChars);
            for (int i = 0; i < lines.Length; i++)
            {
                if (!playing) return output;
                UpdateControlVariables();
                codeLine = i + 1;
                string[] task = await PlaySwitch(lines[i].Split(new char[] { ' ' }));
                if (bool.Parse(task[1]) == false) i = lines.Length - 1;
                output += task[0];
                ConsoleText = output;
            }
            playing = false;
            StopAllSounds();
            if (showStartAndEnd) AddText("Build Ended");
            return output;
        }
        string returnOutput;
        async Task<string[]> PlaySwitch(string[]? _parts = null, string jumpsto = "")
        {
            try
            {
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
                    case "print":
                        try
                        {
                            string text = getString_value(parts, 1, true)[0];
                            AddText(text, false);
                            returnOutput = text;
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // PRINT
                        break;
                    case "object":
                    case "label":
                    case "textbox":
                    case "button":
                        try
                        {
                            GObject? obj = new GObject();
                            Label? lab = new Label();
                            TextBox? txb = new TextBox();
                            Button? btn = new Button();
                            controlType controltype =
                                keyword == "object" ? controlType.Object :
                                keyword == "label" ? controlType.Label :
                                keyword == "textbox" ? controlType.Textbox :
                                keyword == "button" ? controlType.Button :
                                controlType.None;
                            Control control = createControl(parts, controltype);
                            obj = control is GObject ? control as GObject : null;
                            lab = control is Label ? control as Label : null;
                            txb = control is TextBox ? control as TextBox : null;
                            btn = control is Button ? control as Button : null;
                            if (obj == null && lab == null && txb == null && btn == null)
                            {
                                throw new Exception("ERROR");
                            }
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
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
                            else if (BoolCheck(parts, 1))
                            {
                                ConsoleText = string.Empty;
                                RichConsole.Clear();
                            }
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // CLEAR
                        break;
                    case "destroy":
                        try
                        {
                            string name = parts[1];
                            Control? control = getControl(name);
                            if (control == null)
                            {
                                returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                            }
                            if (parts.Length - 1 >= 2 && !BoolCheck(parts, 2))
                            {
                                return new string[] { returnOutput, "true" };
                            }
                            Space.Controls.Remove(control);
                            switch (control.AccessibleName)
                            {
                                case "object":
                                    gameObjects.Remove(control as GObject);
                                    break;
                                case "button":
                                    buttons.Remove(control as Button);
                                    break;
                                case "label":
                                    labels.Remove(control as Label);
                                    break;
                                case "textbox":
                                    textboxes.Remove(control as TextBox);
                                    break;
                                default:
                                    returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, name);
                                    break;
                            }
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // DESTROY
                        break;
                    case "await":
                        try
                        {
                            if (!parts[1].Trim().Contains("?"))
                            {
                                float[] values = find_value(parts, 1, 0);
                                await Task.Delay((int)values[0]);
                            }
                            else
                            {
                                bool check = QMarkCheck(parts, 1)[0] == 0 ? false : true;
                                while (!check && playing)
                                {
                                    check = QMarkCheck(parts, 1)[0] == 0 ? false : true;
                                    await Task.Delay(150);
                                }
                            }
                        }
                        catch
                        {
                            AddText($"An error occured with '{keyword}' in line {codeLine}", true);
                        } // AWAIT
                        break;
                    case "var":
                        try
                        {
                            Var var = CreateVar(parts, allowJump: true).Result;
                            vars.Add(var);
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // VAR
                        break;
                    case "intersects":
                        try
                        {
                            Control? get_1 = getControl(parts[1].Trim(), controlType.None);
                            Control? get_2 = getControl(parts[2].Trim(), controlType.None);

                            if (get_1 == null)
                            {
                                returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, parts[1]);
                            }
                            if (get_2 == null)
                            {
                                returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, parts[2]);
                            }

                            Rectangle rect1 = new Rectangle();
                            Rectangle rect2 = new Rectangle();

                            switch (get_1.AccessibleName)
                            {
                                case "object":
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
                                    returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, parts[1]);
                                    break;
                            }
                            switch (get_2.AccessibleName)
                            {
                                case "object":
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
                                    returnOutput += returnOutput += ErrorText(parts, ErrorTypes.missingControl, keyword, parts[2]);
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
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
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
                                        returnOutput += returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"The path given is invalid for '{keyword}' in line {codeLine}");
                                    }
                                    break;
                                case "play":
                                    string[] file_p = await getFile(parts, 2);
                                    string code = File.ReadAllText(file_p[0]);
                                    endindex = int.Parse(file_p[1]);
                                    string[] lines = code.Split(seperatorChars);
                                    for (int i = 0; i < lines.Length; i++)
                                    {
                                        if (!playing) return new string[] { output, stillInFile.ToString() };
                                        codeLine = i + 1;
                                        UpdateControlVariables();
                                        string[] task = await PlaySwitch(lines[i].Split(new char[] { ' ' }));
                                        if (bool.Parse(task[1]) == false) i = lines.Length - 1;
                                        output += task[0];
                                    }
                                    break;
                                default:
                                    returnOutput += returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'read' 'write' 'path' or 'play' in line {codeLine}");
                                    break;
                            }
                            if (jumpTo) return new string[] { output, stillInFile.ToString() };
                            returnOutput += SetVKeyword(parts, endindex, keyword, output, Types.File);
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // FILE
                        break;
                    case "stop":
                        try
                        {
                            string type = parts[1].Trim();
                            switch (type)
                            {
                                case "all":
                                    playing = false;
                                    if (showStartAndEnd) AddText("Build Stopped");
                                    break;
                                case "file":
                                    stillInFile = false;
                                    break;
                                default:
                                    returnOutput += returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'all' or 'file' in line {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
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
                                                    output = $"({MousePosition.X}, {MousePosition.Y})";
                                                    break;
                                                default:
                                                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'X' or 'Y' for '{keyword}' in line {codeLine}");
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
                                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'console' 'key' or 'mouse' for '{keyword}' in line {codeLine}");
                                    break;
                            }
                            if (jumpTo) return new string[] { output, stillInFile.ToString() };
                            SetVKeyword(parts, index, keyword, output, des);
                        }
                        catch
                        {
                            returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // INPUT
                        break;
                    case "list":
                        try
                        {
                            string name = parts[1];
                            string type = parts[2];
                            List<string> varray = new List<string>();
                            switch (type)
                            {
                                case "new":
                                    string[] colon = getString_value(parts, 3, true);
                                    string values = colon[0];
                                    if (!values.StartsWith(":") && values != "")
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the list in line {codeLine}");
                                    }
                                    else if (values == "")
                                    {

                                    }
                                    else if (values.StartsWith(":"))
                                    {
                                        string allv = values.Remove(0, 1);
                                        varray = allv.Split(",").Select(x=>x.Trim()).ToList();
                                    }
                                    Var var = CreateVar(parts, 1, alreadyarray: varray.ToArray(), allowJump: true).Result;
                                    vars.Add(var);
                                    break;
                                case "add":
                                    Var var_a = getVar(name);
                                    if (var_a.isSet && var_a.isArray())
                                    {
                                        varray = var_a.array.ToList();
                                        string[] colon_ = getString_value(parts, 3, true);
                                        string values_ = colon_[0];
                                        if (!values_.StartsWith(":") && values_ == "")
                                        {
                                            returnOutput += ErrorText(parts, ErrorTypes.custom, $"Expected values set the list in line {codeLine}");
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
                                        var_a.set(array: varray.ToArray());
                                    }
                                    else if (!var_a.isArray())
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in line {codeLine}");
                                    }
                                    else
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                                    }
                                    break;
                                case "equals":
                                    Var var_c = getVar(name);
                                    if (var_c.isSet && var_c.isArray())
                                    {
                                        varray = var_c.array.ToList();
                                        float[] indexes = find_value(parts, 3, 0);
                                        int index = (int)indexes[0];
                                        string[] colon__ = getString_value(parts, (int)indexes[1], true);
                                        string values__ = colon__[0];
                                        if (values__ == "")
                                        {
                                            returnOutput += ErrorText(parts, ErrorTypes.custom, $"Expected ':' to set values to the list in line {codeLine}");
                                        }
                                        else
                                        {
                                            varray[index] = values__.Trim();
                                        }
                                        var_c.array = varray.ToArray();
                                    }
                                    else if (!var_c.isArray())
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in line {codeLine}");
                                    }
                                    else
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                                    }
                                    break;
                                case "clear":
                                    Var var_ca = getVar(name);
                                    if (var_ca.isSet && var_ca.isArray())
                                    {
                                        var_ca.array = new string[] { };
                                    }
                                    else if (!var_ca.isArray())
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected a list variable in line {codeLine}");
                                    }
                                    else
                                    {
                                        returnOutput += ErrorText(parts, ErrorTypes.missingVar, keyword, name);
                                    }
                                    break;
                                default:
                                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new' 'add' 'equals' or 'clear' in line {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            returnOutput += returnOutput += ErrorText(parts, ErrorTypes.normal, keyword);
                        } // LIST
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
                                        returnOutput += ErrorText(parts, ErrorTypes.missingSound, keyword, name);
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
                                        returnOutput += ErrorText(parts, ErrorTypes.missingSound, keyword, name);
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
                                        returnOutput += ErrorText(parts, ErrorTypes.missingSound, keyword, name);
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
                                        returnOutput += ErrorText(parts, ErrorTypes.missingSound, keyword, name);
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
                                        returnOutput += ErrorText(parts, ErrorTypes.missingSound, keyword, name);
                                    }
                                    break;
                                default:
                                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'new' 'destroy' 'play' 'stop' or 'volume' in line {codeLine}");
                                    break;
                            }
                        }
                        catch
                        {
                            returnOutput += ErrorText(parts,ErrorTypes.normal, keyword);
                        } // SOUND
                        break;
                    default:
                        try
                        {
                            if (!keyword.StartsWith("//") && !getVar(keyword).isSet && keyword != "" && !keyword.StartsWith("#"))
                            {
                                returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Could not find a keyword or variable named '{keyword}' in line {codeLine}");
                            }
                            else if(keyword == "#" || keyword == "#create".ToLower() || keyword == "#suppress".ToLower())
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
                                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'error' keyword after '#suppress' in line {codeLine}");
                                        }
                                        break;
                                    case "#create":
                                        if (parts[index] != "error")
                                        {
                                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'error' keyword after '#create' in line {codeLine}");
                                        }
                                        string[] strings = getString_value(parts, index + 1, true);
                                        if (strings[0] == "")
                                        {
                                            returnOutput += ErrorText(parts, ErrorTypes.unkown);
                                        }
                                        else
                                        {
                                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: strings[0]);
                                        }
                                        break;
                                }
                            }
                            else if (getVar(keyword).isSet)
                            {
                                Var var = getVar(keyword);
                            }
                        }
                        catch
                        {
                            returnOutput += ErrorText(parts, ErrorTypes.unkown);
                        }
                        break;
                }
                returnOutput += returnOutput.Equals("") ? "" : "\n";
                senttext = ""; sent = false;
                return new string[] { returnOutput, stillInFile.ToString() };
            }
            catch
            {
                returnOutput += ErrorText(_parts, ErrorTypes.unkown) + "\n";
                return new string[] { returnOutput, "true" };
            }
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
        bool BoolCheck(string[] parts, int index, bool oposite = false)
        {
            bool check = false;

            if (parts[index].Contains("?"))
            {
                check = QMarkCheck(parts, index)[0] == 1;
                if (oposite) check = !check;
            }
            else
            {
                Var var = getVar(parts[index]);
                if (var.isBool() || var.isNumber())
                {
                    check = var.returnBool();
                }
                else if (!var.isSet && var.returnBool(parts[index]) != null)
                {
                    check = var.returnBool(parts[index]) == true;
                }
                else
                {
                    ErrorText(parts, ErrorTypes.custom, custom: $"Expected a boolean variable in line {codeLine}");
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
                    check = IfCheck(value);
                }
                else
                {
                    throw new Exception("");
                }

                return new int[] { check ? 1 : 0, index };
            }
            catch
            {
                ErrorText(parts, ErrorTypes.custom, custom: $"There was an error with the bool check in line {codeLine}");
                return new int[] { };
            }
        }
        bool IfCheck(string inners)
        {
            string[] parts = inners.Split(new char[] { ' ' });
            string[] innerparts = getString_value(parts.Prepend(" ").ToArray(), 0, true);
            string expression = string.Join(" ", innerparts[0].Split(" "));
            bool check = EvaluateExpression(expression);
            return check;
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
                        errorText = $"Expected new variable name after '=>' for '{keyword}' in line {codeLine}";
                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: errorText);
                    }
                    break;
                case ":":
                    try
                    {
                        Var? var0 = SetVar(text, parts[switchIndex + 1].Trim(), parts, Description);
                    }
                    catch
                    {
                        errorText = $"Expected variable name after ':' for '{keyword}' in line {codeLine}";
                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: errorText);
                    }
                    break;
                case "":
                    break;
                default:
                    errorText = $"Expected '=>' or ':' for '{keyword}' in line {codeLine}";
                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: errorText);
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
                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Can not find a variable named '{name}' in line {codeLine}");
                    break;
            }

            vars[index] = var;
            return var;
        }
        async Task<Var> CreateVar(string[] parts, int index = 1, bool reuse = true, string? alreadyVal = null, bool allowJump = false, string[]? alreadyarray = null, Types description = Types.None)
        {
            string name = parts[index].Trim();

            if(UnusableNames.Contains(name) || UnusableContains.Any(unusable => name.Contains(unusable)) || name.Contains(":") || name.Contains("|") || name.Contains("\\") || name.Trim() == "")
            {
                returnOutput += ErrorText(parts, ErrorTypes.violation, name: name);
            }

            Var var = new Var(name);
            var.Description = description;

            if (reuse && vars.Select(x => x.Name).Contains(name))
            {
                var = vars.FirstOrDefault(x => x.Name == name);
            }
            else if (!reuse && vars.Select(x => x.Name).Contains(name))
            {
                returnOutput += ErrorText(parts, ErrorTypes.alreadyMember, "Variable", name);
            }
            if (alreadyVal == null && alreadyarray == null)
            {
                string stringvalue = "";
                int? intval = null;

                if (allowJump && parts[index + 1] == ":")
                {
                    try
                    {
                        string str = string.Join(' ', parts.Skip(index + 2));
                        if (str.Trim() == "")
                        {
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected an assigned value after ':' in line {codeLine}");
                        }

                        string[] strings = await PlaySwitch(jumpsto: str);
                        stringvalue = strings[0];
                    }
                    catch
                    {
                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"There was an error setting the variable to the correct value in Line {codeLine}");
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
                        string[] gotstring1 = getString_value(parts, index + 1);
                        stringvalue = gotstring1[0];
                        index = int.Parse(gotstring1[1]);
                    }
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
                                        returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Minumum can not be greater or equal to the max in 'system:random' line {codeLine}");
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
                    /*case "litteral":
                        if (value.Contains("system:litteral:"))
                        {
                            string[] strings = getString_value(ind, 2, false, false, false, false);
                            return strings[0];
                        }
                        else
                        {
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected ':' after 'system:litteral' in line {codeLine}");
                        }
                        break;*/
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
            else if (AllControls.Select(x => x.Name).Contains(ind[0]))
            {
                Control control = AllControls.FirstOrDefault(x => x.Name == ind[0]);
                switch (ind[1])
                {
                    case "x":
                        value = control.Left.ToString();
                        break;
                    case "y":
                        value = control.Top.ToString();
                        break;
                    case "width":
                        value = control.Width.ToString();
                        break;
                    case "height":
                        value = control.Height.ToString();
                        break;
                    case "r":
                        value = control.BackColor.R.ToString();
                        break;
                    case "g":
                        value = control.BackColor.G.ToString();
                        break;
                    case "b":
                        value = control.BackColor.B.ToString();
                        break;
                    case "text":
                        if (control is not GObject)
                            value = control.Text.ToString();
                        else
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text' property in line " + codeLine);
                        break;
                    case "text-r":
                        if (control is not GObject)
                            value = control.ForeColor.R.ToString();
                        else
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-r' property in line " + codeLine);
                        break;
                    case "text-g":
                        if (control is not GObject)
                            value = control.ForeColor.G.ToString();
                        else
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-g' property in line " + codeLine);
                        break;
                    case "text-b":
                        if (control is not GObject)
                            value = control.ForeColor.B.ToString();
                        else
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-b' property in line " + codeLine);
                        break;
                    case "click":
                        if (control is Button)
                            value = control.AccessibleDescription.Split("\n")[0];
                        else
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: "Only Buttons have a 'click' property in line " + codeLine);
                        break;
                }
            }
            else if (vars.Select(x => x.Name).Contains(ind[0])) 
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
                            returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Expected an array variable in line {codeLine}");
                        }
                        break;
                    default:
                        float number = find_value(ind, 1, 0)[0];
                        value = var.array[(int)number];
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
        void UpdateControlVariables()
        {
            AllControls.AddRange(gameObjects); AllControls.AddRange(buttons);
            AllControls.AddRange(labels); AllControls.AddRange(textboxes);
            /*foreach(Control control in AllControls)
            {
                for (int i = 0; i < vars.Count; i++)
                {
                    if (vars[i].Name == $"{control.Name}:x") vars[i].number = control.Left;
                    //else if (vars[i].Name == $"{control.Name}:y") vars[i].number = control.Top;
                    else if (vars[i].Name == $"width") vars[i].number = control.Width;
                    else if (vars[i].Name == $"height") vars[i].number = control.Height;
                    else if (vars[i].Name == $"r") vars[i].number = control.BackColor.R;
                    else if (vars[i].Name == $"g") vars[i].number = control.BackColor.G;
                    else if (vars[i].Name == $"b") vars[i].number = control.BackColor.B;

                    if(control.AccessibleName == "button" || control.AccessibleName == "label" || control.AccessibleName == "textbox")
                    {
                        if (vars[i].Name == $"{control.Name}:text-r") vars[i].number = control.ForeColor.R;
                        else if (vars[i].Name == $"{control.Name}:text-g") vars[i].number = control.ForeColor.G;
                        else if (vars[i].Name == $"{control.Name}:text-b") vars[i].number = control.ForeColor.B;
                        else if (vars[i].Name == $"{control.Name}:text") vars[i].text = control.Text;
                    }
                    if(control.AccessibleName == "button")
                    {
                        if (vars[i].Name == $"{control.Name}:click") vars[i].number = int.Parse(control.AccessibleDescription.Split("\n")[0]);
                        control.AccessibleDescription = "0\nnull";
                    }
                }
            }*/
        }
        Control? getControl(string name, controlType controltype = controlType.None)
        {
            Control? control = new Control();

            switch (controltype)
            {
                case controlType.None:
                    control = AllControls.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Object:
                    control = gameObjects.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Label:
                    control = labels.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Textbox:
                    control = textboxes.FirstOrDefault(x => x.Name == name);
                    break;
                case controlType.Button:
                    control = buttons.FirstOrDefault(x => x.Name == name);
                    break;
            }

            return control;
        }
        Control createControl(string[] parts, controlType controltype, int index = 1)
        {
            string name = parts[index];

            int violation = AllControls.Select(x => x.Name).Contains(name) == true ? 1 : UnusableNames.Contains(name) ? 2 : UnusableContains.Any(unusable => name.Contains(unusable)) ? 2 : 0;

            string type = controltype == controlType.Button ? "button" :
                controltype == controlType.Object ? "object" : 
                controltype == controlType.Label ? "label" : 
                controltype == controlType.Textbox ? "textbox" : 
                "AN_INTERNAL_ERROR";

            switch (violation)
            {
                case 1:
                    returnOutput += ErrorText(parts, ErrorTypes.alreadyMember, "Control", name);
                    break;
                case 2:
                    returnOutput += ErrorText(parts, ErrorTypes.violation, name: name);
                    break;
            }

            if (controltype == controlType.Object)
            {
                GObject control = new GObject();
                float[] getpoints = find_value(parts, index + 1, 4);
                int points = (int)getpoints[0];
                index = (int)getpoints[1];
                if (points < 3)
                {
                    returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"A minumum of 3 points required for the object {name} in line {codeLine}");
                }
                else if (points == 3) control = new GObject(GObject.Type.Triangle);
                else if (points == 4) control = new GObject(GObject.Type.Square);
                else control = new GObject(GObject.Type.Polygon, points);
                try
                {
                    float[] v = find_value(parts, index, 0);
                    int x = (int)v[0];
                    float[] v1 = find_value(parts, (int)v[1], 0);
                    int y = (int)v1[0];
                    float[] v2 = find_value(parts, (int)v1[1], 50);
                    int scaleX = (int)v2[0];
                    float[] v3 = find_value(parts, (int)v2[1], 50);
                    int scaleY = (int)v3[0];
                    float[] v4 = find_value(parts, (int)v3[1], 0);
                    int r = (int)v4[0];
                    float[] v5 = find_value(parts, (int)v4[1], 0);
                    int g = (int)v5[0];
                    float[] v6 = find_value(parts, (int)v5[1], 0);
                    int b = (int)v6[0];
                    control.Left = x;
                    control.Top = y;
                    control.Width = scaleX;
                    control.Height = scaleY;
                    control.BackColor = Color.FromArgb(r, g, b);
                    control.Name = name;
                    control.AccessibleName = type;
                }
                catch
                {
                    returnOutput += ErrorText(parts, ErrorTypes.normal, type);
                }

                Space.Controls.Add(control);
                gameObjects.Add(control);

                return control;
            }
            else if (controltype == controlType.Label)
            {
                Label control = new Label();
                control.Name = name;
                control.AccessibleName = type;
                control.AutoSize = true;

                try
                {
                    string[] t = getString_value(parts, index + 1);
                    control.Text = t[0];
                    float[] v = find_value(parts, int.Parse(t[1]), 0);
                    int x = (int)v[0];
                    float[] v1 = find_value(parts, (int)v[1], 0);
                    int y = (int)v1[0];
                    float[] v4 = find_value(parts, (int)v1[1], 0);
                    int r = (int)v4[0];
                    float[] v5 = find_value(parts, (int)v4[1], 0);
                    int g = (int)v5[0];
                    float[] v6 = find_value(parts, (int)v5[1], 0);
                    int b = (int)v6[0];
                    control.Left = x;
                    control.Top = y;
                    control.ForeColor = Color.FromArgb(r, g, b);
                }
                catch
                {
                    returnOutput += ErrorText(parts, ErrorTypes.normal, type);
                }

                Space.Controls.Add(control);
                labels.Add(control);

                return control;
            }
            else if (controltype == controlType.Textbox)
            {
                TextBox control = new TextBox();
                control.Name = name;
                control.AccessibleName = type;

                try
                {
                    string[] txt = getString_value(parts, index + 1);
                    control.Text = txt[0];
                    float[] v = find_value(parts, int.Parse(txt[1]), 0);
                    int x = (int)v[0];
                    float[] v1 = find_value(parts, (int)v[1], 0);
                    int y = (int)v1[0];
                    float[] v2 = find_value(parts, (int)v1[1], 100);
                    int scaleX = (int)v2[0];
                    float[] v3 = find_value(parts, (int)v2[1], 25);
                    int scaleY = (int)v3[0];
                    float[] v4 = find_value(parts, (int)v3[1], 0);
                    int r = (int)v4[0];
                    float[] v5 = find_value(parts, (int)v4[1], 0);
                    int g = (int)v5[0];
                    float[] v6 = find_value(parts, (int)v5[1], 0);
                    int b = (int)v6[0];
                    if (parts.Length - 1 >= v6[1])
                    {
                        string t = parts[(int)v6[1]];

                        for (int j = 0; j < vars.Count; j++)
                        {
                            if (vars[j].Name == t)
                            {
                                t = vars[j].value();
                            }
                        }
                        if (t == "yes" || t == "Yes" || t == "1" || t == "true" || t == "True")
                        {
                            control.Multiline = true;
                        }
                        if (t == "no" || t == "No" || t == "0" || t == "false" || t == "False")
                        {
                            control.Multiline = false;
                        }
                    }
                    control.Left = x;
                    control.Top = y;
                    control.Width = scaleX;
                    control.Height = scaleY;
                    control.ForeColor = Color.FromArgb(r, g, b);
                }
                catch
                {
                    returnOutput += ErrorText(parts, ErrorTypes.normal, type);
                }

                Space.Controls.Add(control);
                textboxes.Add(control);

                return control;
            }
            else if (controltype == controlType.Button)
            {
                Button control = new Button();
                control.Name = name;
                control.AccessibleName = type;
                control.Click += InGameButton_Click;

                try
                {
                    string[] txt = getString_value(parts, index + 1);
                    control.Text = txt[0];
                    float[] v = find_value(parts, int.Parse(txt[1]), 0);
                    int x = (int)v[0];
                    float[] v1 = find_value(parts, (int)v[1], 0);
                    int y = (int)v1[0];
                    float[] v2 = find_value(parts, (int)v1[1], 100);
                    int scaleX = (int)v2[0];
                    float[] v3 = find_value(parts, (int)v2[1], 25);
                    int scaleY = (int)v3[0];
                    float[] v4 = find_value(parts, (int)v3[1], 255);
                    int r = (int)v4[0];
                    float[] v5 = find_value(parts, (int)v4[1], 255);
                    int g = (int)v5[0];
                    float[] v6 = find_value(parts, (int)v5[1], 255);
                    int b = (int)v6[0];
                    control.Left = x;
                    control.Top = y;
                    control.Width = scaleX;
                    control.Height = scaleY;
                    control.BackColor = Color.FromArgb(r, g, b);
                }
                catch
                {
                    returnOutput += ErrorText(parts, ErrorTypes.normal, type);
                }

                Space.Controls.Add(control);
                buttons.Add(control);
                return control;
            }
            return null;
        }
        private void InGameButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.AccessibleDescription = "1\nnull";
            if (button.AccessibleDescription.Split(Environment.NewLine).Length < 2) return;
            if (button.AccessibleDescription.Split(Environment.NewLine)[1] != "null")
            {

            }
        }
        string[] getString_value(string[] parts, int next, bool all = false, bool useVar = true, bool useEquation = true, bool useRaw = true, string def = "")
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
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\!") && !texts[i].Contains(@"\\!") ? texts[i].Replace(@"\!", string.Empty) : texts[i].Contains(@"\\!") ? texts[i].Replace(@"\\!", @"\!") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\_") && !texts[i].Contains(@"\\_") ? texts[i].Replace(@"\_", " ") : texts[i].Contains(@"\\_") ? texts[i].Replace(@"\\_", @"\_") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\;") && !texts[i].Contains(@"\\;") ? texts[i].Replace(@"\;", "|") : texts[i].Contains(@"\\;") ? texts[i].Replace(@"\\;", @"\;") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\=") && !texts[i].Contains(@"\\=") ? texts[i].Replace(@"\=", "=") : texts[i].Contains(@"\\=") ? texts[i].Replace(@"\\=", @"\=") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\c") && !texts[i].Contains(@"\\c") ? texts[i].Replace(@"\c", ",") : texts[i].Contains(@"\\c") ? texts[i].Replace(@"\\c", @"\c") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\e!") && !texts[i].Contains(@"\\e!") ? texts[i].Replace(@"\e!", "!") : texts[i].Contains(@"\\!") ? texts[i].Replace(@"\\e!", @"\e!") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Replace(@"\\(", @"\(");
                        switched = sw_t == texts[i] ? switched : true;
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
            value = ColonResponse(value, parts);

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
                returnOutput += ErrorText(parts, ErrorTypes.custom, custom: $"Syntax error in line {codeLine}. Expected ')' to end equation.");
            }

            return new float[] { float.Parse(value), next };
        }
        float[] find_value(string[] parts, int next, int def, bool? var = true, bool colon = true)
        {
            float v = def;
            if (parts.Length - 1 >= next)
            {
                try
                {
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

        /// <summary>
        /// Stops the code currently being played
        /// </summary>
        public void Stop()
        {
            if (showStartAndEnd && playing)
                AddText("Build Stopped");
            playing = false;
        }
        /// <summary>
        /// Sets the Console Input to the inputted text
        /// </summary>
        /// <param name="text">Text Input</param>
        public void ConsoleInput(string text)
        {
            if (!playing && text == "help")
            {
                string help = @"Need help? Look over the list below. If you still need help, please look go to the official EZCode website: https://ez-code.web.app";
                AddText(help);
            }
            if (!playing) return;
            senttext = text;
            sent = true;
        }
        /// <summary> Sets the Key Input to the inputted key for the keydown </summary>
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
        /// Sets the Mouse Input to the inputted Button for the MouseDown
        /// </summary>
        public void MouseInput_Down(MouseEventArgs e) {
            mouseButtons.Add(e.Button);
        }
        public void MouseInput_Down(object sender, MouseEventArgs e) { MouseInput_Down(e); }
        /// <summary>
        /// Sets the Mouse Input to the inputted Button for the MouseUp
        /// </summary>
        public void MouseInput_Up(MouseEventArgs e) { 
            mouseButtons.Remove(e.Button); 
        }
        public void MouseInput_Up(object sender, MouseEventArgs e) { MouseInput_Up(e); }
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
            unkown,
            custom
        }
        /// <summary>
        /// Add an error to the output
        /// </summary>
        /// <param name="parts">To check to see if there is an overide in the line</param>
        /// <param name="error">The type of error for uniform output</param>
        /// <param name="keyword">keyword, for error type</param>
        /// <param name="name">name, for the error type</param>
        /// <param name="custom">cutom error, this makes the 'name' and 'keyword' parameters not needed</param>
        public string ErrorText(string[] parts, ErrorTypes error, string keyword = "keyword", string name = "name", string custom = "")
        {
            string text = 
                error == ErrorTypes.unkown ? $"An error occured in line {codeLine}" :
                error == ErrorTypes.normal ? $"An error occured with '{keyword}' in line {codeLine}" :
                error == ErrorTypes.violation ? $"Naming violation in line {codeLine}. '{name}' can not be used as a name" : 
                error == ErrorTypes.missingControl ? $"Could not find a Control named '{name}' in line {codeLine}" :
                error == ErrorTypes.missingVar ? $"Could not find a Variable named '{name}' in line {codeLine}" :
                error == ErrorTypes.missingSound ? $"Could not find a Sound Player named '{name}' in line {codeLine}" :
                error == ErrorTypes.alreadyMember ? $"Naming violation in line {codeLine}. There is already a '{keyword}' named '{name}'" :
                error == ErrorTypes.custom ? custom : "An Error Occured, We don't know why. If it helps, it was on line " + codeLine;

            if ((parts.Contains("#suppress") && parts.Contains("error")) || (parts.Contains("#") && parts.Contains("suppress") && parts.Contains("error"))) return "";
            if (showFileInError)
            {
                text = ScriptDirectory != "" ? $"{ScriptDirectory}: {text}" : text;
            }
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

            returnOutput += ErrorText(new string[] { }, ErrorTypes.unkown);
        }
    }
}