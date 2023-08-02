using Groups;
using NAudio.Wave;
using Objects;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Variables;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace EZCode
{
    /// <summary>
    /// This is the Official EZCode Source Code. Version 2.0.0
    /// </summary>
    public class EZCode
    {
        /// <summary>
        /// Directory of the script playing
        /// </summary>
        private string ScriptDirectory = "";
        /// <summary>
        /// Console input's bool to send
        /// </summary>
        private bool sent;
        /// <summary>
        /// The key that is currently being pressed
        /// </summary>
        private string keyPreview = "";
        /// <summary>
        /// The last key that is was pressed
        /// </summary>
        private string awaitKeyPreview = "";
        /// <summary>
        /// Text sent by the console's input
        /// </summary>
        private string senttext = "";
        /// <summary>
        /// Bool to decide if a key is down
        /// </summary>
        private bool keydown;
        /// <summary>
        /// Audio output player
        /// </summary>
        private WaveOutEvent? outputPlayer;
        /// <summary>
        /// The output color of an error
        /// </summary>
        public Color errorColor;
        /// <summary>
        /// The normal output color
        /// </summary>
        public Color normalColor;
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
        /// List variable lists
        /// </summary>
        private List<List<Var>> VarList = new List<List<Var>>();
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
            "var", "input", "list", "file", "sound", "if", "//", "#create", "#suppress", "#", "system:" };

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
            string[] lines = code.Split(new char[] { '\n', ';' });
            for (int i = 0; i < lines.Length; i++)
            {
                if (!playing) return output;
                UpdateControlVariables();
                codeLine = i + 1;
                string[] task = await PlaySwitch(lines[i].Split(new char[] { ' ' }));
                if (bool.Parse(task[1]) == false) i = lines.Length - 1;
            }
            playing = false;
            if (showStartAndEnd) AddText("Build Ended");
            return output;
        }
        async Task<string[]> PlaySwitch(string[]? parts = null, string jumpsto = "")
        {
            bool stillInFile = true;
            string returnOutput = string.Empty;
            string keyword = parts != null ? parts[0] : jumpsto.Split(new char[] { ' ' })[0];
            bool jumpTo = jumpsto == "" ? false : true;
            parts = parts != null ? parts : jumpsto.Split(new char[] { ' ' });
            switch (keyword)
            {
                case "print":
                    try
                        {
                            string text = getString_value(parts, 1, true)[0];
                            AddText(text);
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
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
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // CONTROL
                    break;
                case "clear":
                    try
                        {
                            ConsoleText = string.Empty;
                            RichConsole.Clear();
                        }
                        catch
                        {
                            ErrorText(parts, ErrorTypes.normal, keyword);
                        } // CLEAR CONSOLE
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
                            await Task.Delay((int)find_value(parts, 1, 0)[0]);
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
                                ErrorText(parts, ErrorTypes.missingControl, keyword, parts[1]);
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
                                ErrorText(parts, ErrorTypes.missingControl, keyword, parts[2]);
                                break;
                        }
                        string intersects = (rect1.IntersectsWith(rect2) == false ? 0 : 1).ToString();
                        if (jumpTo)
                        {
                            return new string[] { intersects, stillInFile.ToString() };
                        }
                        switch (parts[3])
                        {
                            case "=>":
                                Var var = CreateVar(parts, 4, false, intersects).Result;
                                vars.Add(var);
                                break;
                            case ":":
                                Var? var0 = SetVar(intersects, parts[4].Trim(), parts);
                                break;
                            default:
                                ErrorText(parts, ErrorTypes.custom, custom:$"Expected '=>' or ':' for {keyword} in line {codeLine}");
                                break;
                        }
                    }
                    catch
                    {
                        ErrorText(parts, ErrorTypes.normal, keyword);
                    } // INTERSECTS
                    break;
                case "file":
                    try
                    {
                        if (jumpTo)
                        {
                            return new string[] { "", stillInFile.ToString() };
                        }
                        switch (parts[3])
                        {
                            case "=>":
                                
                                break;
                            case ":":
                                
                                break;
                            default:
                                ErrorText(parts, ErrorTypes.custom, custom:$"Expected '=>' or ':' for {keyword} in line {codeLine}");
                                break;
                        }
                    }
                    catch
                    {
                        ErrorText(parts, ErrorTypes.normal, keyword);
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
                                ErrorText(parts, ErrorTypes.custom, custom: $"Expected 'all' or 'file' in line {codeLine}");
                                break;
                        }
                    }
                    catch
                    {
                        ErrorText(parts, ErrorTypes.normal, keyword);
                    } // STOP
                    break;
                default:
                    break;
            }
            return new string[] { returnOutput, stillInFile.ToString() };
        }
        Var SetVar(string alreadyVal, string name, string[] parts)
        {
            Var var = new Var(name);
            int index = 0;

            if (vars.Select(x => x.Name).Contains(name))
            {
                var = vars.FirstOrDefault(x => x.Name == name);
                index = vars.IndexOf(var);
                var.isSet = true;
            }
            else
            {
                var.isSet = false;
            }

            switch (var.isSet)
            {
                case true:
                    var.set(alreadyVal);
                    vars[index] = var;
                    break;
                case false:
                    ErrorText(parts, ErrorTypes.custom, custom: $"Can not find a variable named '{name}' in line {codeLine}");
                    break;
            }

            return var;
        }
        async Task<Var> CreateVar(string[] parts, int index = 1, bool reuse = true, string? alreadyVal = null, bool allowJump = false)
        {
            string name = parts[index].Trim();

            if(UnusableNames.Contains(name) || name.Contains(":"))
            {
                ErrorText(parts, ErrorTypes.violation, name: name);
            }

            Var var = new Var(name);

            if (reuse && vars.Select(x => x.Name).Contains(name))
            {
                var = vars.FirstOrDefault(x => x.Name == name);
            }
            else if (!reuse && vars.Select(x => x.Name).Contains(name))
            {
                ErrorText(parts, ErrorTypes.alreadyMember, "Variable", name);
            }
            if (alreadyVal == null)
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
                            ErrorText(parts, ErrorTypes.custom, custom: $"Expected an assigned value after ':' in line {codeLine}");
                        }

                        string[] strings = await PlaySwitch(jumpsto: str);
                        stringvalue = strings[0];
                    }
                    catch
                    {
                        ErrorText(parts, ErrorTypes.custom, custom: $"There was an error setting the variable to the correct value in Line {codeLine}");
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
            else
            {
                var.set(alreadyVal);
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
                    case "computer":
                        switch (ind[2])
                        {
                            case "name":
                                value = Environment.MachineName;
                                break;
                        }
                        break;
                    default: break;
                }
            }
            else
            {
                if (AllControls.Select(x => x.Name).Contains(ind[0]))
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
                                ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text' property in line " + codeLine);
                            break;
                        case "text-r":
                            if (control is not GObject)
                                value = control.ForeColor.R.ToString();
                            else
                                ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-r' property in line " + codeLine);
                            break;
                        case "text-g":
                            if (control is not GObject)
                                value = control.ForeColor.G.ToString();
                            else
                                ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-g' property in line " + codeLine);
                            break;
                        case "text-b":
                            if (control is not GObject)
                                value = control.ForeColor.B.ToString();
                            else
                                ErrorText(parts, ErrorTypes.custom, custom: "Objects can don't have a 'text-b' property in line " + codeLine);
                            break;
                        case "click":
                            if (control is Button)
                                value = control.AccessibleDescription.Split("\n")[0];
                            else
                                ErrorText(parts, ErrorTypes.custom, custom: "Only Buttons have a 'click' property in line " + codeLine);
                            break;
                    }
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

            int violation = AllControls.Select(x => x.Name).Contains(name) == true ? 1 : UnusableNames.Contains(name) ? 2 : 0;

            string type = controltype == controlType.Button ? "button" :
                controltype == controlType.Object ? "object" : 
                controltype == controlType.Label ? "label" : 
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

            if (controltype == controlType.Object)
            {
                GObject control = new GObject();
                float[] getpoints = find_value(parts, index + 1, 4);
                int points = (int)getpoints[0];
                index = (int)getpoints[1];
                if (points < 3)
                {
                    ErrorText(parts, ErrorTypes.custom, custom: $"A minumum of 3 points required for the object {name} in line {codeLine}");
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
                    ErrorText(parts, ErrorTypes.normal, type);
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
                    ErrorText(parts, ErrorTypes.normal, type);
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
                    ErrorText(parts, ErrorTypes.normal, type);
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
                    ErrorText(parts, ErrorTypes.normal, type);
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

            if (parts.Length - 1 >= next)
            {

                string s = !all ? parts[next] : string.Join(" ", parts.Skip(1).TakeWhile(part => part != "//"));
                string val = s;
                List<string> texts = s.Split(" ").ToList();

                if (useVar)
                    for (int i = 0; i < texts.Count; i++)
                        for (int j = 0; j < vars.Count; j++)
                            if (vars[j].Name == texts[i])
                                texts[i] = vars[j].value();

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
                else next++;

                string text = "";
                for (int i = 0; i < texts.Count; i++)
                {
                    if (useRaw)
                    {
                        bool switched = false;
                        string sw_t = texts[i];
                        texts[i] = texts[i].Contains(@"\n") && !texts[i].Contains(@"\\n") ? texts[i].Replace(@"\n", Environment.NewLine) : texts[i].Contains(@"\\n") ? texts[i].Replace(@"\\n", @"\n") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\!") && !texts[i].Contains(@"\\!") ? texts[i].Replace(@"\!", string.Empty) : texts[i].Contains(@"\\!") ? texts[i].Replace(@"\\!", @"\!") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Contains(@"\_") && !texts[i].Contains(@"\\_") ? texts[i].Replace(@"\_", " ") : texts[i].Contains(@"\\_") ? texts[i].Replace(@"\\_", @"\_") : texts[i];
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Replace(@"\\(", @"\(");
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = texts[i].Replace(@")\\", @")\");
                        switched = sw_t == texts[i] ? switched : true;
                        texts[i] = !switched && texts[i].Contains(@"\") && !texts[i].Contains(@"\\") ? texts[i].Replace(@"\", string.Empty) : !switched && texts[i].Contains(@"\\") ? texts[i].Replace(@"\\", @"\") : texts[i];
                    }
                    text += texts[i];
                    if (i < texts.Count - 1) text += " ";
                }
                val = text;

                value = val;
            }

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
                ErrorText(parts, ErrorTypes.custom, custom: $"Syntax error in line {codeLine}. Expected ')' to end equation.");
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
                string help = @"Need help? Look over this menu and if you still have questions, please look at the official EZCode website: https://ez-code.web.app";
                AddText(help);
            }
            if (!playing) return;
            senttext = text;
            sent = true;
        }
        /// <summary>
        /// Sets the Key Input to the inputted key for the keydown
        /// </summary>
        /// <param name="e">Key Event</param>
        public void KeyInput_Down(KeyEventArgs e)
        {
            if (!playing) return;
            keyPreview = e.KeyCode.ToString();
            awaitKeyPreview = e.KeyCode.ToString();
            keydown = true;
        }
        /// <summary>
        /// Sets the Key Input to the inputted key for the keyup
        /// </summary>
        public void KeyInput_Up(KeyEventArgs e)
        {
            if (!playing) return;
            keyPreview = "";
            keydown = false;
        }
        /// <summary>
        /// Sets the Key Input to the inputted key for the preview keydown
        /// </summary>
        public void KeyInput_PrevDown(PreviewKeyDownEventArgs e)
        {
            if (!playing) return;
            keyPreview = e.KeyCode.ToString();
            awaitKeyPreview = e.KeyCode.ToString();
            keydown = true;
        }
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
        public void ErrorText(string[] parts, ErrorTypes error, string keyword = "keyword", string name = "name", string custom = "")
        {
            string text = 
                error == ErrorTypes.unkown ? $"An error occured in line {codeLine}" :
                error == ErrorTypes.normal ? $"An error occured with '{keyword}' in line {codeLine}" :
                error == ErrorTypes.violation ? $"Naming violation in line {codeLine}. '{name}' can not be used as a name" : 
                error == ErrorTypes.missingControl ? $"Could not find a Control named '{name}' in line {codeLine}" :
                error == ErrorTypes.alreadyMember ? $"Naming violation in line {codeLine}. There is already a {keyword} named '{name}'" :
                error == ErrorTypes.custom ? custom : "An Error Occured, We don't know why. If it helps, it was on line " + codeLine;

            if ((parts.Contains("#suppress") && parts.Contains("error")) || (parts.Contains("#") && parts.Contains("suppress") && parts.Contains("error"))) return;
            if (showFileInError)
            {
                text = ScriptDirectory != "" ? $"{ScriptDirectory}: {text}" : text;
            }
            AddText(text, true, RichConsole, true);
        }
    }
}