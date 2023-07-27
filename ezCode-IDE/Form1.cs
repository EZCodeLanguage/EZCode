using NAudio.Wave;
using Objects;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;
using System.Xml.Linq;

namespace ezCode
{
    public partial class Form1 : Form
    {
        enum multimids
        {
            plus,
            minus,
            add,
            subtract,
            multiply,
            divide,
            equals,
            greater,
            less,
            not
        }
        bool debugWait;
        bool autosave;
        bool debugging;
        string _File;
        bool saved;
        bool playing;
        bool sent;
        int codeLine = 0;
        int mc;
        string keyPreview;
        string awaitKeyPreview;
        string senttext;
        bool keydown;
        List<Open_File> openfiles = new List<Open_File>();
        WaveOutEvent outputPlayer;
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        List<Label> labels = new List<Label>();
        List<TextBox> textboxes = new List<TextBox>();
        List<Button> buttons = new List<Button>();
        List<GObject> gameObjects = new List<GObject>();
        List<Var> vars = new List<Var>();
        List<List<Var>> VarList = new List<List<Var>>();
        List<Group> Group = new List<Group>();

        public Form1(string file)
        {
            InitializeComponent();
            autoopenproject();
            string filePath = "C:\\Users\\Public\\Temp\\ezcode\\autosave.txt";

            if (File.Exists(filePath))
            {
                string on = File.ReadAllText(filePath);
                if (on == "On")
                {
                    autosave = true;
                    console.AddText("Autosave On \n", false);
                }
            }

            _File = file;
            saved = true;

            txt.Focus();

            if (_File != "NOTHING")
            {
                try
                {
                    StreamReader streamReader = new StreamReader(_File);
                    txt.Text = streamReader.ReadToEnd();
                    streamReader.Close();

                    string[] filenames = file.Split("\\");
                    string filename = filenames[filenames.Length - 1];

                    openfiles.Add(new Open_File(filename, file));
                    listBox1.Items.Add(filename);
                }
                catch
                {
                    MessageBox.Show("Could not open the document");
                }
            }
        }
        /*public if_check ifs;
        public struct if_check
        {
            public int num { get; set; }
            public bool istrue { get; set; }
            private List<string> _lines;
            public List<string> lines
            {
                get
                {
                    return _lines;
                }
                set
                {
                    _lines = value;
                }
            }
            public static List<string> SetIf(string[] _lines, int index)
            {
                List<string> l = new List<string>();
                bool a = false;
                for (int i = index + 1; i < _lines.Length; i++)
                {
                    if (_lines[i].Trim() == "endIf")
                    {
                        a = true;
                        break;
                    }
                    if (!a) l.Add(_lines[i]);
                }
                return l;
            }
            public if_check(int number, bool itrue, List<string> lines_)
            {
                num = number;
                istrue = itrue;
                lines = lines_;
            }
        }*/
        private async Task PlayAsync(string text)
        {
            string code = text;
            string[] lines = code.Split(Environment.NewLine);

            List<string> loopCode = new List<string>(); // Create a list to store the loop code

            bool breaked = false;
            bool hasEnded = false;
            int endl = 0;
            int stackNow = 0;
            bool iftrue = true;

            for (int w = 0; w < lines.Length; w++)
            {
                try { progressBar1.Value = w + 1; }
                catch { progressBar1.Value = progressBar1.Maximum; }
                if (!playing) return;
                if (debugging)
                {
                    int _1 = comboBox1.SelectedIndex;
                    int _2 = comboBox2.SelectedIndex;
                    CurrentLineTxt.Text = lines[w];
                    listBox2.Items.Clear();
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        comboBox1.Items.Add(gameObjects[i].Name);
                    }
                    for (int i = 0; i < labels.Count; i++)
                    {
                        comboBox2.Items.Add(labels[i].Name);
                    }
                    for (int i = 0; i < vars.Count; i++)
                    {
                        listBox2.Items.Add(vars[i].Name + "  =  " + vars[i].value());
                    }
                    comboBox1.SelectedIndex = _1;
                    comboBox2.SelectedIndex = _2;
                    for (int i = 0; i < gameObjects.Count; i++)
                    {
                        if (i == comboBox1.SelectedIndex)
                        {
                            xpos1.Text = gameObjects[i].Left.ToString();
                            ypos1.Text = gameObjects[i].Top.ToString();
                            xscale1.Text = gameObjects[i].Width.ToString();
                            yscale1.Text = gameObjects[i].Height.ToString();
                        }
                    }
                    for (int i = 0; i < labels.Count; i++)
                    {
                        if (i == comboBox2.SelectedIndex)
                        {
                            xpos2.Text = labels[i].Left.ToString();
                            ypos2.Text = labels[i].Top.ToString();
                            xscale2.Text = labels[i].Text.ToString();
                            yscale2.Text = labels[i].Font.ToString();
                        }
                    }
                    while (debugWait)
                    {
                        await Task.Delay(100);
                    }
                }
                debugWait = true;
                codeLine = w + 1;
                string[] part = lines[w].Trim().Split(' ').ToArray();
                if (part[0] == "loop")
                {
                    int stackTotal = 1;
                    hasEnded = false;
                    breaked = false;
                    // Get the number of times to loop
                    int loopTimes = 0;
                    bool iss = false;
                    try
                    {
                        foreach (Var v in vars)
                        {
                            if (v.Name == part[1])
                            {
                                try
                                {
                                    iss = true;
                                    loopTimes = int.Parse(v.value());
                                }
                                catch
                                {
                                    console.AddText("An error occured, 'loop' wasn't formatted correctly line " + codeLine + Environment.NewLine, true);
                                }
                            }
                        }
                        if (!iss) loopTimes = int.Parse(part[1]);
                    }
                    catch
                    {
                        console.AddText("An error occured, couldn't get 'loop' value in line " + codeLine + Environment.NewLine, true);
                    }
                    // Store the loop code in the list
                    loopCode.Clear();

                    bool containsEnd = false;

                    for (int k = w + 1; k < lines.Length; k++)
                    {
                        if (lines[k].Trim() == "end")
                        {
                            if (!containsEnd) endl = k + 1;
                            containsEnd = true;
                        }
                    }

                    if (!containsEnd)
                    {
                        console.AddText("loop doesn't have and 'end'" + Environment.NewLine, true);
                        return;
                    }

                    for (int k = w + 1; k < lines.Length; k++)
                    {
                        string[] innerParts = lines[k].Split(' ');
                        int st = stackNow;
                        // Check if the current line is an "endloop" statement
                        if (innerParts[0] == "loop")
                        {
                            //stackTotal++;
                        }
                        else if (innerParts[0] == "if" && innerParts[innerParts.Length - 1] == ":")
                        {
                            //stackTotal++;
                        }
                        else if (innerParts[0] == "end")
                        {
                            //st++;
                            if (/*st == stackTotal*/ true)
                            {
                                endl = w;
                                w = k; //Jump back to the line after the endloop statement
                                hasEnded = true;
                                break; // Break out of the loop
                            }
                        }/*
                        if (innerParts[0] == "return")
                        {
                            w = k; //Jump back to the line after the endloop statement
                            break;
                        }
                        else if (innerParts[0] == "break")
                        {
                            breaked = true;
                            w = endl + 1;
                            break; // Break out of the loop
                        }*/
                        else
                        {
                            if (!hasEnded && !breaked) loopCode.Add(lines[k]); // Add the current line to the loop code list
                        }
                    }

                    // Loop through the specified number of times
                    for (int j = 0; j < loopTimes; j++)
                    {
                        // Execute the code in the loop code list
                        foreach (string loopLine in loopCode)
                        {
                            if (!breaked && playing) await ExecuteLine(loopLine);
                        }
                    }
                }
                else
                {
                    // Execute the code on the current line
                    await ExecuteLine(lines[w]);
                }
                // using filePath list name : values,from,that,script |or| using filePath var name ValueFromScript
                async Task ExecuteLine(string line)
                {
                    List<string> parts = line.Trim().Split(' ').ToList();
                    int i = 0;
                    if (parts[0] == "endIf")
                    {
                        try
                        {
                            iftrue = true;
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    }
                    if (!iftrue) return;
                    if (parts[i] == "print")
                    {
                        try
                        {
                            string text = "";
                            bool done = false;
                            for (int j = 1; j < parts.Count; j++)
                            {
                                if (parts[j].Contains("//")) done = true;
                                text += !done ? parts[j] : string.Empty;
                                if (j < parts.Count - 1 && !done) text += " ";
                            }

                            bool isVar = false;
                            string val = text;
                            List<string> texts = text.Split(" ").ToList();
                            int ended = 0;
                            string brackets = "";
                            int started = 0;
                            int count = 0;

                            for (int j = 0; j < texts.Count; j++)
                            {
                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == texts[j])
                                    {
                                        isVar = true;
                                        texts[j] = vars[k].value();
                                    }
                                }
                                if (texts[j].StartsWith(@"\("))
                                {
                                    ended = 1;
                                    count = 1;
                                    started = j;
                                    for (int l = j; l < texts.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            count++;
                                            brackets += texts[l];
                                            if (l < texts.Count - 1) brackets += " ";
                                        }
                                        if (texts[l].EndsWith(@")\"))
                                        {
                                            ended = 2;
                                        }
                                    }
                                }
                            }
                            if (ended != 0)
                            {
                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                string result = SolveEquation(equation);
                                texts[started] = result;

                                int endIndex = started + count;
                                if (endIndex < texts.Count)
                                {
                                    texts.RemoveRange(started + 1, count - 2);
                                }
                                else
                                {
                                    texts.RemoveRange(started + 1, texts.Count - (started + 1));
                                }
                            }
                            text = "";
                            for (int j = 0; j < texts.Count; j++)
                            {
                                text += texts[j];
                                if (j < texts.Count - 1) text += " ";
                            }
                            val = text;

                            if (!isVar)
                            {
                                val = val.Replace(@"\n", Environment.NewLine);
                                val = val.Replace(@"\_", " ");
                                val = val.Replace(@"\!", string.Empty);

                                console.AddText(val + Environment.NewLine, false);
                                return;
                            }
                            else
                            {
                                console.AddText(text + Environment.NewLine, false);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("There was an error with 'print' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // print text
                    else if (parts[i] == "printRaw")
                    {
                        try
                        {
                            string text = "";
                            for (int j = 1; j < parts.Count; j++)
                            {
                                text += parts[j];
                                if (j < parts.Count - 1) text += " ";
                            }
                            console.AddText(text + "\n", false);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'printRaw' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // printRaw text
                    else if (parts[i] == "group")
                    {
                        try
                        {
                            if (parts[2] == "add")
                            {
                                Group g = new Group("");
                                bool found = false;
                                for (int j = 0; j < Group.Count; j++)
                                {
                                    if (Group[j].Name == parts[1])
                                    {
                                        found = true;
                                        g = Group[j];
                                    }
                                }
                                if (!found)
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a group named '" + parts[1] + "' in 'Group' in line " + codeLine + " \n", true);
                                    return;
                                }
                                string name = parts[4];
                                bool er = true;
                                if (parts[3] == "button")
                                {
                                    Button b = new Button();
                                    for (int j = 0; j < buttons.Count; j++)
                                    {
                                        if (buttons[j].Name == name)
                                        {
                                            b = buttons[j];
                                            b.AccessibleName = name;
                                            er = false;
                                        }
                                    }
                                    if (er)
                                    {
                                        int.Parse("error");
                                    }
                                    g.Buttons.Add(b);
                                }
                                else if (parts[3] == "label")
                                {
                                    Label b = new Label();
                                    for (int j = 0; j < labels.Count; j++)
                                    {
                                        if (labels[j].Name == name)
                                        {
                                            b = labels[j];
                                            b.AccessibleName = name;
                                            er = false;
                                        }
                                    }
                                    if (er)
                                    {
                                        int.Parse("error");
                                    }
                                    g.Labels.Add(b);
                                }
                                else if (parts[3] == "object")
                                {
                                    GObject b = new GObject(GObject.Type.Square);
                                    for (int j = 0; j < gameObjects.Count; j++)
                                    {
                                        if (gameObjects[j].Name == name)
                                        {
                                            b = gameObjects[j];
                                            b.AccessibleName = name;
                                            er = false;
                                        }
                                    }
                                    if (er)
                                    {
                                        int.Parse("error");
                                    }
                                    g.Objects.Add(b);
                                }
                                else if (parts[3] == "textbox")
                                {
                                    TextBox b = new TextBox();
                                    for (int j = 0; j < textboxes.Count; j++)
                                    {
                                        if (textboxes[j].Name == name)
                                        {
                                            b = textboxes[j];
                                            er = false;
                                        }
                                    }
                                    if (er)
                                    {
                                        int.Parse("error");
                                    }
                                    g.Textboxes.Add(b);
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Expected the modifier 'button', 'label', 'textbox, or 'button' in 'Group' in line " + codeLine + " \n", true);
                                    return;
                                }
                            }
                            else if (parts[2] == "change")
                            {
                                Group g = new Group("");
                                bool found = false;
                                for (int j = 0; j < Group.Count; j++)
                                {
                                    if (Group[j].Name == parts[1])
                                    {
                                        found = true;
                                        g = Group[j];
                                    }
                                }
                                if (!found)
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a group named '" + parts[1] + "' in 'Group' in line " + codeLine + " \n", true);
                                    return;
                                }
                                bool abs = false;
                                bool rel = false;
                                if (parts[3] == "relative" || parts[3] == "rel") rel = true;
                                if (parts[3] == "absolute" || parts[3] == "abs") abs = true;
                                if (!abs && !rel)
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Expected 'absoulute' or 'relative' in 'Group' in line " + codeLine + " \n", true);
                                    return;
                                }
                                if (parts[4] == "move")
                                {
                                    int[] x = find_value(parts, 5, 0);
                                    int[] y = find_value(parts, x[1], 0);
                                    for (int j = 0; j < g.Buttons.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Buttons[j].Left = x[0];
                                            g.Buttons[j].Top = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Buttons[j].Left += x[0];
                                            g.Buttons[j].Top += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Objects.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Objects[j].Left = x[0];
                                            g.Objects[j].Top = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Objects[j].Left += x[0];
                                            g.Objects[j].Top += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Labels.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Labels[j].Left = x[0];
                                            g.Labels[j].Top = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Labels[j].Left += x[0];
                                            g.Labels[j].Top += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Textboxes.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Textboxes[j].Left = x[0];
                                            g.Textboxes[j].Top = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Textboxes[j].Left += x[0];
                                            g.Textboxes[j].Top += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                }
                                else if (parts[4] == "scale")
                                {
                                    int[] x = find_value(parts, 5, 0);
                                    int[] y = find_value(parts, x[1], 0);
                                    for (int j = 0; j < g.Buttons.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Buttons[j].Width = x[0];
                                            g.Buttons[j].Height = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Buttons[j].Width += x[0];
                                            g.Buttons[j].Height += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Objects.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Objects[j].Width = x[0];
                                            g.Objects[j].Height = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Objects[j].Width += x[0];
                                            g.Objects[j].Height += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Labels.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Labels[j].Width = x[0];
                                            g.Labels[j].Height = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Labels[j].Width += x[0];
                                            g.Labels[j].Height += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                    for (int j = 0; j < g.Textboxes.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            g.Textboxes[j].Width = x[0];
                                            g.Textboxes[j].Height = y[0];
                                        }
                                        else if (rel)
                                        {
                                            g.Textboxes[j].Width += x[0];
                                            g.Textboxes[j].Height += y[0];
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                    }
                                }
                                else if (parts[4] == "color")
                                {
                                    int r = 0;
                                    int g_ = 0;
                                    int b = 0;
                                    int[] __r = find_value(parts, 5, 0);
                                    int[] __g = find_value(parts, __r[1], 0);
                                    int[] __b = find_value(parts, __g[1], 0);
                                    for (int j = 0; j < g.Buttons.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            r = __r[0];
                                            g_ = __g[0];
                                            b = __b[0];
                                        }
                                        else if (rel)
                                        {
                                            r = __r[0] + g.Buttons[j].BackColor.R;
                                            g_ = __g[0] + g.Buttons[j].BackColor.G;
                                            b = __b[0] + g.Buttons[j].BackColor.B;
                                            if (r > 255) r = 255;
                                            if (g_ > 255) g_ = 255;
                                            if (b > 255) b = 255;
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                        g.Buttons[j].BackColor = Color.FromArgb(r, g_, b);
                                    }
                                    for (int j = 0; j < g.Objects.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            r = __r[0];
                                            g_ = __g[0];
                                            b = __b[0];
                                        }
                                        else if (rel)
                                        {
                                            r = __r[0] + g.Objects[j].BackColor.R;
                                            g_ = __g[0] + g.Objects[j].BackColor.G;
                                            b = __b[0] + g.Objects[j].BackColor.B;
                                            if (r > 255) r = 255;
                                            if (g_ > 255) g_ = 255;
                                            if (b > 255) b = 255;
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                        g.Objects[j].BackColor = Color.FromArgb(r, g_, b);
                                    }
                                    for (int j = 0; j < g.Labels.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            r = __r[0];
                                            g_ = __g[0];
                                            b = __b[0];
                                        }
                                        else if (rel)
                                        {
                                            r = __r[0] + g.Labels[j].ForeColor.R;
                                            g_ = __g[0] + g.Labels[j].ForeColor.G;
                                            b = __b[0] + g.Labels[j].ForeColor.B;
                                            if (r > 255) r = 255;
                                            if (g_ > 255) g_ = 255;
                                            if (b > 255) b = 255;
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                        g.Labels[j].ForeColor = Color.FromArgb(r, g_, b);
                                    }
                                    for (int j = 0; j < g.Textboxes.Count; j++)
                                    {
                                        if (abs)
                                        {
                                            r = __r[0];
                                            g_ = __g[0];
                                            b = __b[0];
                                        }
                                        else if (rel)
                                        {
                                            r = __r[0] + g.Textboxes[j].ForeColor.R;
                                            g_ = __g[0] + g.Textboxes[j].ForeColor.G;
                                            b = __b[0] + g.Textboxes[j].ForeColor.B;
                                            if (r > 255) r = 255;
                                            if (g_ > 255) g_ = 255;
                                            if (b > 255) b = 255;
                                        }
                                        else
                                        {
                                            int thisisanerror = int.Parse("error");
                                        }
                                        g.Textboxes[j].ForeColor = Color.FromArgb(r, g_, b);
                                    }
                                }
                            }
                            else if (parts[1] == "new")
                            {
                                Group.Add(new Group(parts[2]));
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Expected 'new', 'change', or 'add' in 'Group' in line " + codeLine + " \n", true);
                                return;
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'Group' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // group groupName add mid name |or| group new name |or| group groupName change abs/rel mid v1 v2 v3
                    else if (parts[i] == "consoleClear")
                    {
                        try
                        {
                            console.Clear();
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'consoleClear' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // consoleClear
                    else if (parts[i] == "write")
                    {
                        try
                        {
                            string labelName = parts[i + 1];
                            string labelText = "";
                            for (int j = 2; j < parts.Count; j++)
                            {
                                labelText += parts[j];
                                if (j < parts.Count - 1) labelText += " ";
                            }
                            string val = labelText;
                            Label label = new Label();
                            label.AccessibleName = "error";
                            TextBox textBox = new TextBox();
                            textBox.AccessibleName = "error";
                            for (int j = 0; j < labels.Count; j++)
                            {
                                if (labels[j].Name == labelName)
                                {
                                    label = labels[j];
                                }
                            }
                            for (int j = 0; j < textboxes.Count; j++)
                            {
                                if (textboxes[j].Name == labelName)
                                {
                                    textBox = textboxes[j];
                                }
                            }
                            bool isvar = false;

                            List<string> texts = labelText.Split(" ").ToList();
                            int ended = 0;
                            string brackets = "";
                            int started = 0;
                            int count = 0;

                            for (int j = 0; j < texts.Count; j++)
                            {
                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == texts[j])
                                    {
                                        isvar = true;
                                        texts[j] = vars[k].value();
                                    }
                                }
                                if (texts[j].StartsWith(@"\("))
                                {
                                    ended = 1;
                                    count = 1;
                                    started = j;
                                    for (int l = j; l < texts.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            count++;
                                            brackets += texts[l];
                                            if (l < texts.Count - 1) brackets += " ";
                                        }
                                        if (texts[l].EndsWith(@")\"))
                                        {
                                            ended = 2;
                                        }
                                    }
                                }
                            }
                            if (ended != 0)
                            {
                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                string result = SolveEquation(equation);
                                texts[started] = result;

                                int endIndex = started + count;
                                if (endIndex < texts.Count)
                                {
                                    texts.RemoveRange(started + 1, count - 2);
                                }
                                else
                                {
                                    texts.RemoveRange(started + 1, texts.Count - (started + 1));
                                }
                            }
                            labelText = "";
                            for (int j = 0; j < texts.Count; j++)
                            {
                                labelText += texts[j];
                                if (j < texts.Count - 1) labelText += " ";
                            }
                            val = labelText;
                            if (label.AccessibleName == "error" && textBox.AccessibleName == "error")
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a label or txtbox named " + labelName + "\n", true);
                                return;
                            }
                            else if (label.AccessibleName != "error" && textBox.AccessibleName == "error")
                            {
                                if (!isvar)
                                {
                                    val = val.Replace(@"\n", Environment.NewLine);
                                    val = val.Replace(@"\_", " ");
                                    val = val.Replace(@"\!", string.Empty);

                                    label.Text = val;
                                }
                                else
                                {
                                    label.Text = labelText;
                                }
                            }
                            else if (label.AccessibleName == "error" && textBox.AccessibleName != "error")
                            {
                                if (!isvar)
                                {
                                    val = val.Replace(@"\n", Environment.NewLine);
                                    val = val.Replace(@"\_", " ");
                                    val = val.Replace(@"\!", string.Empty);

                                    textBox.Text = val;
                                }
                                else
                                {
                                    textBox.Text = labelText;
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'write' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // write labelName text
                    else if (parts[i] == "writeRaw")
                    {
                        try
                        {
                            string labelName = parts[i + 1];
                            string labelText = "";
                            for (int j = 2; j < parts.Count; j++)
                            {
                                labelText += parts[j];
                                if (j < parts.Count - 1) labelText += " ";
                            }
                            string val = string.Empty;
                            Label label = new Label();
                            label.AccessibleName = "error";
                            TextBox textBox = new TextBox();
                            textBox.AccessibleName = "error";
                            for (int j = 0; j < labels.Count; j++)
                            {
                                if (labels[j].Name == labelName)
                                {
                                    label = labels[j];
                                }
                            }
                            for (int j = 0; j < textboxes.Count; j++)
                            {
                                if (textboxes[j].Name == labelName)
                                {
                                    textBox = textboxes[j];
                                }
                            }

                            if (label.AccessibleName == "error" && textBox.AccessibleName == "error")
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a label or txtbox named " + labelName + "\n", true);
                                return;
                            }
                            else if (label.AccessibleName != "error" && textBox.AccessibleName == "error")
                            {
                                label.Text = labelText;
                            }
                            else if (label.AccessibleName == "error" && textBox.AccessibleName != "error")
                            {
                                textBox.Text = labelText;
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'write' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // writeRaw labelName text
                    else if (parts[i] == "button")
                    {
                        try
                        {
                            string name = parts[i + 1];

                            Button b = new Button();
                            b.Left = 0;
                            b.Top = 0;
                            b.Name = name;
                            b.FlatStyle = FlatStyle.Flat;
                            b.FlatAppearance.BorderSize = 1;

                            try
                            {
                                if(parts.Count > 2)
                                {
                                    string text = parts[2];

                                    text = text.Replace(@"\n", Environment.NewLine);
                                    text = text.Replace(@"\_", " ");
                                    text = text.Replace(@"\!", string.Empty);

                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == text)
                                        {
                                            text = vars[j].value();
                                        }
                                    }
                                    b.Text = text;
                                }
                                int[] v = find_value(parts, 3, 0);
                                int x = v[0];
                                int[] v1 = find_value(parts, v[1], 0);
                                int y = v1[0];
                                int[] v2 = find_value(parts, v1[1], 100);
                                int scaleX = v2[0];
                                int[] v3 = find_value(parts, v2[1], 25);
                                int scaleY = v3[0];
                                int[] v4 = find_value(parts, v3[1], 211);
                                int r = v4[0];
                                int[] v5 = find_value(parts, v4[1], 211);
                                int g = v5[0];
                                int[] v6 = find_value(parts, v5[1], 211);
                                int _b = v6[0];
                                b.Left = x;
                                b.Top = y;
                                b.Width = scaleX;
                                b.Height = scaleY;
                                b.BackColor = Color.FromArgb(r, g, _b);
                            }
                            catch
                            {
                                int.Parse("error");
                            }

                            Space.Controls.Add(b);
                            buttons.Add(b);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'button' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // button name text
                    else if (parts[i] == "buttonClick")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string file = "";
                            for (int j = 2; j < parts.Count; j++)
                            {
                                file += parts[j];
                                if (j < parts.Count - 1) file += " ";
                            }
                            bool iss = false;
                            Button b = new Button();

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == text)
                                {
                                    text = vars[j].value();
                                }
                                if (vars[j].Name == file)
                                {
                                    file = vars[j].value();
                                }
                            }

                            for (int j = 0; j < buttons.Count; j++)
                            {
                                if (buttons[j].Name == name)
                                {
                                    iss = true;
                                    b = buttons[j];
                                }
                            }
                            if (!iss)
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a button named " + name + " in line " + codeLine + " \n", true);
                                return;
                            }


                            if (file.Contains("~/"))
                            {
                                string[] dp = _File.Split(@"\");
                                string directory = "";
                                for (int j = 0; j < dp.Length; j++)
                                {
                                    if (j < dp.Length - 1)
                                    {
                                        directory += dp[j] + @"\\";
                                    }
                                }
                                directory += file.Remove(0, 2);
                                file = directory;
                            }

                            if (!File.Exists(file))
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find the file specified in line " + codeLine + ": " + file + " \n", true);
                                return;
                            }

                            b.Click += InGameButtonClicked;

                            b.AccessibleDescription = file;
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'buttonClcik' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // buttonClick buttonname PathtoFile
                    else if (parts[i] == "textbox")
                    {
                        try
                        {
                            string name = parts[i + 1];

                            TextBox tb = new TextBox();
                            tb.Left = 0;
                            tb.Top = 0;
                            tb.Name = name;

                            try
                            {
                                if (parts.Count > 2)
                                {
                                    string text = parts[2];

                                    text = text.Replace(@"\n", Environment.NewLine);
                                    text = text.Replace(@"\_", " ");
                                    text = text.Replace(@"\!", string.Empty);

                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == text)
                                        {
                                            text = vars[j].value();
                                        }
                                    }
                                    tb.Text = text;
                                }
                                int[] v = find_value(parts, 3, 0);
                                int x = v[0];
                                int[] v1 = find_value(parts, v[1], 0);
                                int y = v1[0];
                                int[] v2 = find_value(parts, v1[1], 100);
                                int scaleX = v2[0];
                                int[] v3 = find_value(parts, v2[1], 25);
                                int scaleY = v3[0];
                                int[] v4 = find_value(parts, v3[1], 0);
                                int r = v4[0];
                                int[] v5 = find_value(parts, v4[1], 0);
                                int g = v5[0];
                                int[] v6 = find_value(parts, v5[1], 0);
                                int _b = v6[0];
                                if (parts.Count - 1 >= v6[1])
                                {
                                    string t = parts[v6[1]];

                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == t)
                                        {
                                            t = vars[j].value();
                                        }
                                    }
                                    if (t == "yes" || t == "Yes" || t == "1" || t == "true" || t == "True")
                                    {
                                        tb.Multiline = true;
                                    }
                                    if (t == "no" || t == "No" || t == "0" || t == "false" || t == "False")
                                    {
                                        tb.Multiline = false;
                                    }
                                }
                                tb.Left = x;
                                tb.Top = y;
                                tb.Width = scaleX;
                                tb.Height = scaleY;
                                tb.ForeColor = Color.FromArgb(r, g, _b);
                            }
                            catch
                            {
                                int.Parse("error");
                            }

                            Space.Controls.Add(tb);
                            textboxes.Add(tb);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'textbox' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // textbox name text
                    else if (parts[i] == "multiLine")
                    {
                        try
                        {
                            string tb = parts[i + 1];
                            string t = parts[i + 2];
                            TextBox textBox = new TextBox();
                            textBox.AccessibleName = "error";
                            for (int j = 0; j < textboxes.Count; j++)
                            {
                                if (textboxes[j].Name == tb)
                                {
                                    textBox = textboxes[j];
                                }
                            }

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == t)
                                {
                                    t = vars[j].value();
                                }
                            }

                            if (textBox.AccessibleName == "error")
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a txtbox named " + tb + "\n", true);
                                return;
                            }
                            else
                            {
                                if (t == "yes" || t == "Yes" || t == "1" || t == "true" || t == "True")
                                {
                                    textBox.Multiline = true;
                                }
                                if (t == "no" || t == "No" || t == "0" || t == "false" || t == "False")
                                {
                                    textBox.Multiline = false;
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'write' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // multLine textboxname value
                    else if (parts[i] == "object")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            int points = 0;
                            bool isVar = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == parts[i + 2] && vars[j].isNumber())
                                {
                                    points = Convert.ToInt16(vars[j].number);
                                    isVar = true;
                                }
                            }
                            string brackets = "";
                            int ended = 0;
                            if (parts[2].StartsWith(@"("))
                            {
                                ended = 1;
                                List<string> texts = new List<string>();
                                for (int l = 2; l < parts.Count; l++)
                                {
                                    texts.Add(parts[l]);
                                }
                                for (int l = 0; l < texts.Count; l++)
                                {
                                    if (ended == 1)
                                    {
                                        brackets += texts[l];
                                        if (l < texts.Count - 1) brackets += " ";
                                    }
                                    if (texts[l].EndsWith(@")"))
                                    {
                                        ended = 2;
                                    }
                                }
                            }
                            if (ended != 0)
                            {
                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                string result = SolveEquation(equation);
                                points = (int)float.Parse(result);
                            }
                            else
                            {
                                if (!isVar) points = Convert.ToInt16(parts[i + 2]);
                            }

                            GObject go = new GObject(GObject.Type.Square);

                            if (points < 3)
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("A minumum of 3 points required for the object " + name + "\n", true);
                            }
                            else if (points == 3) go = new GObject(GObject.Type.Triangle);
                            else if (points == 4) go = new GObject(GObject.Type.Square);
                            else go = new GObject(GObject.Type.Polygon, points);

                            try
                            {
                                int[] v = find_value(parts, 3, 0);
                                int x = v[0];
                                int[] v1 = find_value(parts, v[1], 0);
                                int y = v1[0];
                                int[] v2 = find_value(parts, v1[1], 50);
                                int scaleX = v2[0];
                                int[] v3 = find_value(parts, v2[1], 50);
                                int scaleY = v3[0];
                                int[] v4 = find_value(parts, v3[1], 0);
                                int r = v4[0];
                                int[] v5 = find_value(parts, v4[1], 0);
                                int g = v5[0];
                                int[] v6 = find_value(parts, v5[1], 0);
                                int _b = v6[0];
                                go.Left = x;
                                go.Top = y;
                                go.Width = scaleX;
                                go.Height = scaleY;
                                go.BackColor = Color.FromArgb(r, g, _b);
                            }
                            catch
                            {
                                int.Parse("error");
                            }

                            go.Name = name;

                            Space.Controls.Add(go);
                            gameObjects.Add(go);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'object' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // object name sides
                    else if (parts[i] == "image")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string file = "";
                            for (int j = 3; j < parts.Count; j++)
                            {
                                file += parts[j];
                                if (j < parts.Count - 1) file += " ";
                            }
                            string type = parts[i + 2].Trim();

                            if (file.Contains("~/"))
                            {
                                string[] dp = _File.Split(@"\");
                                string directory = "";
                                for (int j = 0; j < dp.Length; j++)
                                {
                                    if (j < dp.Length - 1)
                                    {
                                        directory += dp[j] + @"\\";
                                    }
                                }
                                directory += file.Remove(0, 2);
                                file = directory;
                            }

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == parts[i + 2])
                                {
                                    file = vars[j].value();
                                }
                            }

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == parts[i + 2])
                                {
                                    type = vars[j].value();
                                }
                            }

                            GObject go = new GObject(GObject.Type.Square);
                            go.AccessibleName = "ERROR";

                            for (int j = 0; j < gameObjects.Count; j++)
                            {
                                if (gameObjects[j].Name == name)
                                {
                                    go = gameObjects[j];
                                    go.AccessibleName = "";
                                }
                            }

                            if (go.AccessibleName == "ERROR")
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find an object named '" + name + "' in line " + codeLine + " \n", true);
                                return;
                            }

                            try
                            {
                                ImageLayout imageLayout = ImageLayout.Stretch;
                                if (type == "center") imageLayout = ImageLayout.Center;
                                else if (type == "none") imageLayout = ImageLayout.None;
                                else if (type == "zoom") imageLayout = ImageLayout.Zoom;
                                else if (type == "tile") imageLayout = ImageLayout.Tile;
                                else if (type == "stretch") imageLayout = ImageLayout.Stretch;
                                go.BackgroundImageLayout = imageLayout;
                                go.BackgroundImage = System.Drawing.Image.FromFile(file);
                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("There was an error reading the image '" + file + "' in line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'object' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // image name fit PathToFile
                    else if (parts[i] == "label")
                    {
                        try
                        {
                            string name = parts[i + 1];

                            Label label = new Label();
                            label.AutoSize = true;
                            label.Left = 0;
                            label.Top = 0;
                            label.Name = name;
                            label.Text = name;

                            try
                            {
                                if (parts.Count > 2)
                                {
                                    string text = parts[2];

                                    text = text.Replace(@"\n", Environment.NewLine);
                                    text = text.Replace(@"\_", " ");
                                    text = text.Replace(@"\!", string.Empty);

                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == text)
                                        {
                                            text = vars[j].value();
                                        }
                                    }
                                    label.Text = text;
                                }
                                int[] v = find_value(parts, 3, 0);
                                int x = v[0];
                                int[] v1 = find_value(parts, v[1], 0);
                                int y = v1[0];
                                int[] v2 = find_value(parts, v1[1], 0);
                                int r = v2[0];
                                int[] v3 = find_value(parts, v2[1], 0);
                                int g = v3[0];
                                int[] v4 = find_value(parts, v3[1], 0);
                                int b = v4[0];
                                label.Left = x;
                                label.Top = y;
                                label.ForeColor = Color.FromArgb(r, g, b);
                            }
                            catch
                            {
                                int.Parse("error");
                            }

                            Space.Controls.Add(label);
                            labels.Add(label);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'label' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // label name
                    else if (parts[i] == "font")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string style = parts[i + 2];
                            string size = parts[i + 3];
                            string brackets = "";
                            int ended = 0;
                            List<string> texts = new List<string>();
                            for (int l = 3; l < parts.Count; l++)
                            {
                                texts.Add(parts[l]);
                            }

                            Label go = new Label();
                            go.AccessibleName = "error";

                            for (int j = 0; j < labels.Count; j++)
                            {
                                if (labels[j].Name == name)
                                {
                                    go = labels[j];
                                    go.AccessibleName = name;
                                }
                            }
                            if (go.AccessibleName != "error")
                            {
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == style)
                                    {
                                        style = vars[j].value();
                                    }
                                }
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == size)
                                    {
                                        size = vars[j].value();
                                    }
                                }
                                if (size.StartsWith(@"("))
                                {
                                    ended = 1;
                                    for (int l = 0; l < texts.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            brackets += texts[l];
                                            if (l < texts.Count - 1) brackets += " ";
                                        }
                                        if (texts[l].EndsWith(@")"))
                                        {
                                            ended = 2;
                                        }
                                    }
                                }
                                if (ended != 0)
                                {
                                    string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                    string result = SolveEquation(equation);
                                    size = result;
                                }
                                FontStyle styl = new FontStyle();
                                if (style == "bold") styl = FontStyle.Bold;
                                else if (style == "italic") styl = FontStyle.Italic;
                                else if (style == "underline") styl = FontStyle.Underline;
                                else if (style == "strikeout") styl = FontStyle.Strikeout;
                                else if (style == "regular") styl = FontStyle.Regular;
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with 'font' there is no font style called " + style + "\n", true);
                                }
                                int siz = int.Parse(size);
                                SetFont(go, name, siz, styl);
                            }
                            else
                            {
                                TextBox tb = new TextBox();
                                tb.AccessibleName = "error";

                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == name)
                                    {
                                        tb = textboxes[j];
                                        tb.AccessibleName = name;
                                    }
                                }
                                if (tb.AccessibleName != "error")
                                {
                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == style)
                                        {
                                            style = vars[j].value();
                                        }
                                    }
                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == size)
                                        {
                                            size = vars[j].value();
                                        }
                                    }
                                    if (size.StartsWith(@"("))
                                    {
                                        ended = 1;
                                        for (int l = 0; l < texts.Count; l++)
                                        {
                                            if (ended == 1)
                                            {
                                                brackets += texts[l];
                                                if (l < texts.Count - 1) brackets += " ";
                                            }
                                            if (texts[l].EndsWith(@")"))
                                            {
                                                ended = 2;
                                            }
                                        }
                                    }
                                    if (ended != 0)
                                    {
                                        string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                        string result = SolveEquation(equation);
                                        size = result;
                                    }
                                    FontStyle styl = new FontStyle();
                                    if (style == "bold") styl = FontStyle.Bold;
                                    else if (style == "italic") styl = FontStyle.Italic;
                                    else if (style == "underline") styl = FontStyle.Underline;
                                    else if (style == "strikeout") styl = FontStyle.Strikeout;
                                    else if (style == "regular") styl = FontStyle.Regular;
                                    else
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("There was an error with 'font' there is no font style called " + style + "\n", true);
                                    }
                                    int siz = int.Parse(size);
                                    SetFont(tb, name, siz, styl);
                                }
                                else
                                {
                                    Button b = new Button();
                                    b.AccessibleName = "error";

                                    for (int j = 0; j < buttons.Count; j++)
                                    {
                                        if (buttons[j].Name == name)
                                        {
                                            b = buttons[j];
                                            b.AccessibleName = name;
                                        }
                                    }
                                    if (b.AccessibleName != "error")
                                    {
                                        for (int j = 0; j < vars.Count; j++)
                                        {
                                            if (vars[j].Name == style)
                                            {
                                                style = vars[j].value();
                                            }
                                        }
                                        for (int j = 0; j < vars.Count; j++)
                                        {
                                            if (vars[j].Name == size)
                                            {
                                                size = vars[j].value();
                                            }
                                        }
                                        if (size.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < texts.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += texts[l];
                                                    if (l < texts.Count - 1) brackets += " ";
                                                }
                                                if (texts[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            size = result;
                                        }
                                        FontStyle styl = new FontStyle();
                                        if (style == "bold") styl = FontStyle.Bold;
                                        else if (style == "italic") styl = FontStyle.Italic;
                                        else if (style == "underline") styl = FontStyle.Underline;
                                        else if (style == "strikeout") styl = FontStyle.Strikeout;
                                        else if (style == "regular") styl = FontStyle.Regular;
                                        else
                                        {
                                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                            console.AddText("There was an error with 'font' there is no font style called " + style + "\n", true);
                                        }
                                        int siz = int.Parse(size);
                                        SetFont(b, name, siz, styl);
                                    }
                                    else
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find a label, textbox, or button named " + name + "\n", true);
                                    }
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'font' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // font labelName fontStyle size
                    else if (parts[i] == "move")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            Point point = new Point(0, 0);
                            string brackets = "";
                            int ended = 0;
                            int endindex = 0;
                            List<string> textsA = new List<string>();
                            for (int l = 2; l < parts.Count; l++)
                            {
                                textsA.Add(parts[l]);
                            }
                            bool aV = false, bV = false;
                            try
                            {
                                int a = 0;
                                int b = 0;
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == parts[i + 2] && vars[j].isNumber())
                                    {
                                        a = Convert.ToInt16(vars[j].number);
                                        aV = true;
                                    }
                                }
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == parts[i + 3] && vars[j].isNumber())
                                    {
                                        b = Convert.ToInt16(vars[j].number);
                                        bV = true;
                                    }
                                }
                                if (!aV)
                                {
                                    try
                                    {
                                        a = Convert.ToInt16(parts[i + 2]);
                                    }
                                    catch
                                    {
                                        if (parts[2].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsA.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsA[l];
                                                    if (l < textsA.Count - 1) brackets += " ";
                                                }
                                                if (textsA[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            a = (int)float.Parse(result);
                                        }
                                    }
                                }
                                if (!bV)
                                {
                                    try
                                    {
                                        b = Convert.ToInt16(textsA[endindex + 1]);
                                    }
                                    catch
                                    {
                                        string newended = textsA[endindex + 1];
                                        List<string> textsB = new List<string>();
                                        for (int l = endindex + 1; l < textsA.Count; l++)
                                        {
                                            textsB.Add(textsA[l]);
                                        }
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsB.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsB[l];
                                                    if (l < textsB.Count - 1) brackets += " ";
                                                }
                                                if (textsB[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            b = (int)float.Parse(result);
                                        }
                                    }
                                }

                                point = new Point(a, b);

                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error with 'move' the vector was not formatted correctly in line " + codeLine + "\n", true);
                            }

                            GObject go = new GObject(GObject.Type.Square);
                            go.AccessibleName = "error";
                            Label la = new Label();
                            la.AccessibleName = "error";
                            TextBox tb = new TextBox();
                            tb.AccessibleName = "error";
                            Button bt = new Button();
                            bt.AccessibleName = "error";

                            for (int j = 0; j < labels.Count; j++)
                            {
                                if (labels[j].Name == name)
                                {
                                    la = labels[j];
                                    la.AccessibleName = name;
                                }
                            }
                            if (la.AccessibleName == "error")
                            {
                                for (int j = 0; j < gameObjects.Count; j++)
                                {
                                    if (gameObjects[j].Name == name)
                                    {
                                        go = gameObjects[j];
                                        go.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName == "error" && go.AccessibleName == "error")
                            {
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == name)
                                    {
                                        tb = textboxes[j];
                                        tb.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName == "error" && go.AccessibleName == "error")
                            {
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == name)
                                    {
                                        tb = textboxes[j];
                                        tb.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName == "error" && go.AccessibleName == "error" && tb.AccessibleName == "error")
                            {
                                for (int j = 0; j < buttons.Count; j++)
                                {
                                    if (buttons[j].Name == name)
                                    {
                                        bt = buttons[j];
                                        bt.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName != "error")
                            {
                                la.Location = point;
                            }
                            else if (go.AccessibleName != "error")
                            {
                                go.Location = point;
                            }
                            else if (tb.AccessibleName != "error")
                            {
                                tb.Location = point;
                            }
                            else if (bt.AccessibleName != "error")
                            {
                                bt.Location = point;
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find an object, label, textbox, or button named " + name + "\n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'move' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // move object/label_Name x y 
                    else if (parts[i] == "scale")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            Point point = new Point(0, 0);
                            string brackets = "";
                            int ended = 0;
                            int endindex = 0;
                            List<string> textsA = new List<string>();
                            for (int l = 2; l < parts.Count; l++)
                            {
                                textsA.Add(parts[l]);
                            }
                            bool aV = false, bV = false;
                            try
                            {
                                int a = 0;
                                int b = 0;
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == parts[i + 2] && vars[j].isNumber())
                                    {
                                        a = Convert.ToInt16(vars[j].number);
                                        aV = true;
                                    }
                                    if (vars[j].Name == parts[i + 3] && vars[j].isNumber())
                                    {
                                        b = Convert.ToInt16(vars[j].number);
                                        bV = true;
                                    }
                                }
                                if (!aV)
                                {
                                    try
                                    {
                                        a = Convert.ToInt16(parts[i + 2]);
                                    }
                                    catch
                                    {
                                        if (parts[2].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsA.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsA[l];
                                                    if (l < textsA.Count - 1) brackets += " ";
                                                }
                                                if (textsA[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            a = (int)float.Parse(result);
                                        }
                                    }
                                }
                                if (!bV)
                                {
                                    try
                                    {
                                        b = Convert.ToInt16(textsA[endindex + 1]);
                                    }
                                    catch
                                    {
                                        string newended = textsA[endindex + 1];
                                        List<string> textsB = new List<string>();
                                        for (int l = endindex + 1; l < textsA.Count; l++)
                                        {
                                            textsB.Add(textsA[l]);
                                        }
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsB.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsB[l];
                                                    if (l < textsB.Count - 1) brackets += " ";
                                                }
                                                if (textsB[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            b = (int)float.Parse(result);
                                        }
                                    }
                                }

                                point = new Point(a, b);

                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error with 'scale' the vector was not formatted correctly in line " + codeLine + " \n", true);
                            }

                            GObject go = new GObject(GObject.Type.Square);
                            go.AccessibleName = "error";
                            TextBox tb = new TextBox();
                            tb.AccessibleName = "error";
                            Button bt = new Button();
                            bt.AccessibleName = "error";

                            for (int j = 0; j < gameObjects.Count; j++)
                            {
                                if (gameObjects[j].Name == name)
                                {
                                    go = gameObjects[j];
                                    go.AccessibleName = name;
                                }
                            }
                            if (go.AccessibleName != "error")
                            {
                                go.Size = new Size(point.X, point.Y);
                            }
                            else
                            {
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == name)
                                    {
                                        tb = textboxes[j];
                                        tb.AccessibleName = name;
                                    }
                                }
                                //if (tb.Multiline) tb.Size = new Size(point.X, point.Y);
                                //else console.AddText("The textbox " + name + " is not multi lined\n", true);
                                if (tb.AccessibleName == "error")
                                {
                                    for (int j = 0; j < buttons.Count; j++)
                                    {
                                        if (buttons[j].Name == name)
                                        {
                                            bt = buttons[j];
                                            bt.AccessibleName = name;
                                        }
                                    }
                                    if (bt.AccessibleName == "error") console.AddText("Could not find an object or textbox named " + name + "\n", true);
                                    bt.Size = new Size(point.X, point.Y);
                                }
                                tb.Size = new Size(point.X, point.Y);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'scale' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // scale objectName x y
                    else if (parts[i] == "color")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            Color color = Color.Black;
                            string brackets = "";
                            int ended = 0;
                            int endindex = 0;
                            List<string> textsA = new List<string>();
                            for (int l = 2; l < parts.Count; l++)
                            {
                                textsA.Add(parts[l]);
                            }
                            bool aV = false, bV = false, cV = false;
                            try
                            {
                                float a = 0;
                                float b = 0;
                                float c = 0;
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == parts[i + 2] && vars[j].isNumber())
                                    {
                                        a = float.Parse(vars[j].value());
                                        aV = true;
                                    }
                                    if (vars[j].Name == parts[i + 3] && vars[j].isNumber())
                                    {
                                        b = float.Parse(vars[j].value());
                                        bV = true;
                                    }
                                    if (vars[j].Name == parts[i + 4] && vars[j].isNumber())
                                    {
                                        c = float.Parse(vars[j].value());
                                        cV = true;
                                    }
                                }
                                if (!aV)
                                {
                                    try
                                    {
                                        a = Convert.ToInt16(parts[i + 2]);
                                    }
                                    catch
                                    {
                                        if (parts[2].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsA.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsA[l];
                                                    if (l < textsA.Count - 1) brackets += " ";
                                                }
                                                if (textsA[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            a = (int)float.Parse(result);
                                        }
                                    }
                                }
                                List<string> textsB = new List<string>();
                                for (int l = endindex + 1; l < textsA.Count; l++)
                                {
                                    textsB.Add(textsA[l]);
                                }
                                if (!bV)
                                {
                                    try
                                    {
                                        b = Convert.ToInt16(textsA[endindex + 1]);
                                    }
                                    catch
                                    {
                                        string newended = textsA[endindex + 1];
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsB.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsB[l];
                                                    if (l < textsB.Count - 1) brackets += " ";
                                                }
                                                if (textsB[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            b = (int)float.Parse(result);
                                        }
                                    }
                                }
                                if (!cV)
                                {
                                    try
                                    {
                                        c = Convert.ToInt16(textsB[endindex + 1]);
                                    }
                                    catch
                                    {
                                        string newended = textsB[endindex + 1];
                                        List<string> textsC = new List<string>();
                                        for (int l = endindex + 1; l < textsB.Count; l++)
                                        {
                                            textsC.Add(textsB[l]);
                                        }
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsC.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsC[l];
                                                    if (l < textsC.Count - 1) brackets += " ";
                                                }
                                                if (textsC[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            c = (int)float.Parse(result);
                                        }
                                    }
                                }

                                color = Color.FromArgb((int)a, (int)b, (int)c);

                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error with 'color' the rgb color was not formatted correctly in line " + codeLine + "\n", true);
                            }

                            GObject go = new GObject(GObject.Type.Square);
                            go.AccessibleName = "error";
                            Label la = new Label();
                            la.AccessibleName = "error";
                            TextBox tb = new TextBox();
                            tb.AccessibleName = "error";
                            Button bt = new Button();
                            bt.AccessibleName = "error";

                            for (int j = 0; j < labels.Count; j++)
                            {
                                if (labels[j].Name == name)
                                {
                                    la = labels[j];
                                    la.AccessibleName = name;
                                }
                            }
                            if (la.AccessibleName == "error")
                            {
                                for (int j = 0; j < gameObjects.Count; j++)
                                {
                                    if (gameObjects[j].Name == name)
                                    {
                                        go = gameObjects[j];
                                        go.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName == "error" && go.AccessibleName == "error")
                            {
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == name)
                                    {
                                        tb = textboxes[j];
                                        tb.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName == "error" && go.AccessibleName == "error" && tb.AccessibleName == "error")
                            {
                                for (int j = 0; j < buttons.Count; j++)
                                {
                                    if (buttons[j].Name == name)
                                    {
                                        bt = buttons[j];
                                        bt.AccessibleName = name;
                                    }
                                }
                            }
                            if (la.AccessibleName != "error")
                            {
                                la.ForeColor = color;
                            }
                            else if (go.AccessibleName != "error")
                            {
                                go.BackColor = color;
                            }
                            else if (tb.AccessibleName != "error")
                            {
                                tb.ForeColor = color;
                            }
                            else if (bt.AccessibleName != "error")
                            {
                                bt.BackColor = color;
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find an object or label named " + name + "\n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'move' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // color object/label_Name r g b
                    else if (parts[i] == "await")
                    {
                        try
                        {
                            string time = parts[i + 1];
                            string brackets = "";
                            int ended = 0;
                            List<string> texts = new List<string>();
                            for (int l = 1; l < parts.Count; l++)
                            {
                                texts.Add(parts[l]);
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == time)
                                {
                                    time = vars[j].value();
                                }
                            }
                            if (time.StartsWith(@"("))
                            {
                                ended = 1;
                                for (int l = 0; l < texts.Count; l++)
                                {
                                    if (ended == 1)
                                    {
                                        brackets += texts[l];
                                        if (l < texts.Count - 1) brackets += " ";
                                    }
                                    if (texts[l].EndsWith(@")"))
                                    {
                                        ended = 2;
                                    }
                                }
                            }
                            if (ended != 0)
                            {
                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                string result = SolveEquation(equation);
                                time = result;
                            }
                            await Task.Delay(Convert.ToInt16(time));
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'await' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // await miliseconds
                    else if (parts[i] == "var")
                    {
                        try
                        {
                            string name = parts[1];
                            string value = parts[2];

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == value)
                                {
                                    value = vars[j].value();
                                }
                            }
                            Var var = new Var(name);

                            value = value.Replace(@"\n", Environment.NewLine);
                            value = value.Replace(@"\_", " ");
                            value = value.Replace(@"\!", string.Empty);

                            if (value == "ConsoleInput")
                            {
                                // Wait for the user to press the "Send" button
                                float qq = 0;
                                while (!sent)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for console input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                var.set(senttext);
                                var.isSet = true;
                                var.stack = stackNow;

                                // Reset the "sent" flag
                                sent = false;
                                senttext = string.Empty;
                            }
                            else if (value == "IntersectsWith")
                            {
                                string a = parts[3];
                                string b = parts[4];
                                string intersects;

                                GObject A = new GObject(GObject.Type.Square);
                                A.AccessibleName = "Error";
                                GObject B = new GObject(GObject.Type.Square);
                                B.AccessibleName = "Error";

                                for (int j = 0; j < gameObjects.Count; j++)
                                {
                                    if (a == gameObjects[j].Name)
                                    {
                                        A = gameObjects[j];
                                        A.AccessibleName = "";
                                    }
                                    if (b == gameObjects[j].Name)
                                    {
                                        B = gameObjects[j];
                                        B.AccessibleName = "";
                                    }
                                }
                                if (A.AccessibleName != "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the objects: '" + a + "' and '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName == "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName != "" && B.AccessibleName == "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + a + "' in line " + codeLine + " \n", true);
                                    return;
                                }

                                if (A.Bounds.IntersectsWith(B.Bounds))
                                {
                                    intersects = "1";
                                }
                                else
                                {
                                    intersects = "0";
                                }

                                // Create the variable with the user's input as the value
                                var.set(intersects);
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "Random")
                            {
                                string mi = parts[3];
                                string ma = parts[4];
                                int min = 0;
                                int max = 0;
                                string brackets = "";
                                int ended = 0;
                                int endindex = 0;
                                List<string> textsA = new List<string>();
                                for (int l = 3; l < parts.Count; l++)
                                {
                                    textsA.Add(parts[l]);
                                }
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (mi == vars[j].Name)
                                    {
                                        mi = vars[j].value();
                                    }
                                    if (ma == vars[j].Name)
                                    {
                                        ma = vars[j].value();
                                    }
                                }
                                try
                                {
                                    try
                                    {
                                        min = int.Parse(mi);
                                    }
                                    catch
                                    {
                                        if (parts[3].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsA.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsA[l];
                                                    if (l < textsA.Count - 1) brackets += " ";
                                                }
                                                if (textsA[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            min = (int)float.Parse(result);
                                        }
                                    }
                                    try
                                    {
                                        max = int.Parse(ma);
                                    }
                                    catch
                                    {
                                        string newended = textsA[endindex + 1];
                                        List<string> textsB = new List<string>();
                                        for (int l = endindex + 1; l < textsA.Count; l++)
                                        {
                                            textsB.Add(textsA[l]);
                                        }
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsB.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsB[l];
                                                    if (l < textsB.Count - 1) brackets += " ";
                                                }
                                                if (textsB[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            max = (int)float.Parse(result);
                                        }
                                    }
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with the minumim and maximum of Random in line " + codeLine + " \n", true);
                                }

                                Random rand = new Random();
                                int rnd = rand.Next(min, max);
                                // Create the variable with the user's input as the value
                                var.set(rnd.ToString());
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "KeyInput")
                            {
                                // Create the variable with the user's input as the value
                                var.set(keyPreview);
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "KeyDown")
                            {
                                // Create the variable with the user's input as the value
                                var.set(keydown == false ? "0" : "1");
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "AwaitKeyInput")
                            {
                                float qq = 0;
                                while (!keydown)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for key input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                var.set(keyPreview);
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            /*else if (value == "MousePosition")
                            {
                                string vector = parts[3];
                                float pos = 1111.111100015684465464864f;
                                if(vector == "x")
                                {
                                    pos = MousePosition.X;
                                }
                                else if(vector == "y")
                                {
                                    pos = MousePosition.Y;
                                }
                                else
                                {
                                    console.AddText("There was an error with the vector: " + vector + ". Expected 'x' or 'y' in line " + codeLine + " \n", true);
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(pos.ToString());
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "MouseClick")
                            {
                                float pos = 1111.111100015684465464864f;
                                if(mc == 0)
                                {
                                    pos = 0;
                                }
                                else if(mc == 1)
                                {
                                    pos = 1;
                                }
                                else if(mc == 2)
                                {
                                    pos = 2;
                                }
                                else if(mc == 3)
                                {
                                    pos = 3;
                                }
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(pos.ToString());
                                var.isSet = true;

                                vars.Add(var);

                                mc = 0;
                            }*/
                            else if (value == "ReadFile")
                            {
                                string file = "";
                                for (int j = 3; j < parts.Count; j++)
                                {
                                    file += parts[j];
                                    if (j < parts.Count - 1) file += " ";
                                }
                                if (file.Contains("~/"))
                                {
                                    string[] dp = _File.Split(@"\");
                                    string directory = "";
                                    for (int j = 0; j < dp.Length; j++)
                                    {
                                        if (j < dp.Length - 1)
                                        {
                                            directory += dp[j] + @"\\";
                                        }
                                    }
                                    directory += file.Remove(0, 2);
                                    file = directory;
                                }
                                string val = "";
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == file)
                                    {
                                        file = vars[j].value();
                                    }
                                }
                                try
                                {
                                    val = File.ReadAllText(file);
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error reading the file: " + file + " In line " + codeLine + " \n", true);
                                }

                                // Create the variable with the user's input as the value
                                var.set(val);
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "FromTextBox")
                            {
                                bool found = false;
                                string val = "";
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == parts[3])
                                    {
                                        found = true;
                                        val = textboxes[j].Text;
                                    }
                                }
                                if (!found)
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a textbox named " + parts[3] + " In line " + codeLine + " \n", true);
                                    return;
                                }

                                // Create the variable with the user's input as the value
                                var.set(val);
                                var.isSet = true;
                                var.stack = stackNow;
                            }
                            else if (value == "FromList")
                            {
                                bool isSet = false;
                                string listN = parts[3];
                                List<Var> list = new List<Var>();

                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[j][0].Name == listN)
                                    {
                                        list = VarList[j];
                                        isSet = true;
                                    }
                                }
                                if (isSet)
                                {
                                    bool isVar = false;
                                    string number = parts[4];
                                    number = getEquation(number, 4, parts);
                                    int numberValue = 0;
                                    for (int k = 0; k < vars.Count; k++)
                                    {
                                        if (vars[k].Name == number.Trim() && vars[k].isNumber())
                                        {
                                            numberValue = (int)vars[k].number;
                                            isVar = true;
                                        }
                                    }
                                    if (!isVar) numberValue = Convert.ToInt16(number);

                                    // Create the variable with the user's input as the value
                                    var.set(list[numberValue].value());
                                    var.isSet = true;
                                    var.stack = stackNow;
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a list by the inputted name in " + codeLine + " \n", true);
                                }
                            }
                            else
                            {
                                string brackets = "";
                                int ended = 0;
                                List<string> texts = new List<string>();
                                for (int l = 2; l < parts.Count; l++)
                                {
                                    texts.Add(parts[l]);
                                }
                                if (value.StartsWith(@"("))
                                {
                                    ended = 1;
                                    for (int l = 0; l < texts.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            brackets += texts[l];
                                            if (l < texts.Count - 1) brackets += " ";
                                        }
                                        if (texts[l].EndsWith(@")"))
                                        {
                                            ended = 2;
                                        }
                                    }
                                }
                                if (ended != 0)
                                {
                                    string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                    string result = SolveEquation(equation);
                                    value = result;
                                }
                                // Create a new variable with the specified name and value
                                var.set(value);
                                var.isSet = true;
                                var.stack = stackNow;
                            }

                            bool first = true;
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == var.Name)
                                {
                                    first = false;
                                    vars[j] = var;
                                }
                            }
                            if (first) vars.Add(var);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("There was an error with 'var' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // var name value |or| var name FromList listName varNumber 
                    else if (parts[i] == "varRaw")
                    {
                        try
                        {
                            string name = parts[1];
                            string value = parts[2];

                            if (value == "ConsoleInput")
                            {
                                // Wait for the user to press the "Send" button
                                float qq = 0;
                                while (!sent)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for console input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(senttext);
                                var.isSet = true;

                                vars.Add(var);

                                // Reset the "sent" flag
                                sent = false;
                                senttext = string.Empty;
                            }
                            else if (value == "KeyDown")
                            {
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(keydown == false ? "0" : "1");
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "IntersectsWith")
                            {
                                string a = parts[3];
                                string b = parts[4];
                                string intersects;

                                GObject A = new GObject(GObject.Type.Square);
                                A.AccessibleName = "Error";
                                GObject B = new GObject(GObject.Type.Square);
                                B.AccessibleName = "Error";

                                for (int j = 0; j < gameObjects.Count; j++)
                                {
                                    if (a == gameObjects[j].Name)
                                    {
                                        A = gameObjects[j];
                                        A.AccessibleName = "";
                                    }
                                    if (b == gameObjects[j].Name)
                                    {
                                        B = gameObjects[j];
                                        B.AccessibleName = "";
                                    }
                                }
                                if (A.AccessibleName != "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the objects: '" + a + "' and '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName == "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName != "" && B.AccessibleName == "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + a + "' in line " + codeLine + " \n", true);
                                    return;
                                }

                                if (A.Bounds.IntersectsWith(B.Bounds))
                                {
                                    intersects = "1";
                                }
                                else
                                {
                                    intersects = "0";
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(intersects);
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "Random")
                            {
                                string mi = parts[3];
                                string ma = parts[4];
                                int min = 0;
                                int max = 0;
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (mi == vars[j].Name)
                                    {
                                        mi = vars[j].value();
                                    }
                                    if (ma == vars[j].Name)
                                    {
                                        ma = vars[j].value();
                                    }
                                }
                                try
                                {
                                    min = int.Parse(mi);
                                    max = int.Parse(ma);
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with the minumim and maximum: '" + mi + "," + ma + "' in line " + codeLine + " \n", true);
                                }

                                Random rand = new Random();
                                int rnd = rand.Next(min, max);
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(rnd.ToString());
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "KeyInput")
                            {
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(keyPreview);
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "AwaitKeyInput")
                            {
                                float qq = 0;
                                while (!keydown)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for key input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(keyPreview);
                                var.isSet = true;

                                vars.Add(var);
                            }
                            /*else if (value == "MousePosition")
                            {
                                string vector = parts[3];
                                float pos = 1111.111100015684465464864f;
                                if (vector == "x")
                                {
                                    pos = MousePosition.X;
                                }
                                else if (vector == "y")
                                {
                                    pos = MousePosition.Y;
                                }
                                else
                                {
                                    console.AddText("There was an error with the vector: " + vector + ". Expected 'x' or 'y' in line " + codeLine + " \n", true);
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(pos.ToString());
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "MouseClick")
                            {
                                float pos = 1111.111100015684465464864f;
                                if (mc == 0)
                                {
                                    pos = 0;
                                }
                                else if (mc == 1)
                                {
                                    pos = 1;
                                }
                                else if (mc == 2)
                                {
                                    pos = 2;
                                }
                                else if (mc == 3)
                                {
                                    pos = 3;
                                }
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(pos.ToString());
                                var.isSet = true;

                                vars.Add(var);

                                mc = 0;
                            }*/
                            else if (value == "ReadFile")
                            {
                                string file = "";
                                for (int j = 3; j < parts.Count; j++)
                                {
                                    file += parts[j];
                                    if (j < parts.Count - 1) file += " ";
                                }
                                if (file.Contains("~/"))
                                {
                                    string[] dp = _File.Split(@"\");
                                    string directory = "";
                                    for (int j = 0; j < dp.Length; j++)
                                    {
                                        if (j < dp.Length - 1)
                                        {
                                            directory += dp[j] + @"\\";
                                        }
                                    }
                                    directory += file.Remove(0, 2);
                                    file = directory;
                                }
                                string val = "";
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == file)
                                    {
                                        file = vars[j].value();
                                    }
                                }
                                try
                                {
                                    val = File.ReadAllText(file);
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error reading the file: " + file + " In line " + codeLine + " \n", true);
                                }

                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(val);
                                var.isSet = true;

                                vars.Add(var);
                            }
                            else if (value == "FromList")
                            {
                                bool isSet = false;
                                string listN = parts[3];
                                List<Var> list = new List<Var>();

                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[j][0].Name == listN)
                                    {
                                        list = VarList[j];
                                        isSet = true;
                                    }
                                }
                                if (isSet)
                                {
                                    bool isVar = false;
                                    string number = parts[4];
                                    int numberValue = 0;
                                    for (int k = 0; k < vars.Count; k++)
                                    {
                                        if (vars[k].Name == number.Trim() && vars[k].isNumber())
                                        {
                                            numberValue = (int)vars[k].number;
                                            isVar = true;
                                        }
                                    }
                                    if (!isVar) numberValue = Convert.ToInt16(number);

                                    // Create the variable with the user's input as the value
                                    Var var = new Var(name);
                                    var.set(list[numberValue].value());
                                    var.isSet = true;

                                    vars.Add(var);
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a list by the inputted name in " + codeLine + " \n", true);
                                }
                            }
                            else
                            {
                                // Create a new variable with the specified name and value
                                Var var = new Var(name);
                                var.set(value);
                                var.isSet = true;

                                vars.Add(var);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("There was an error with 'var' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // varRaw name value |or| varRaw name FromList listName varNumber 
                    else if (parts[i] == "varInput")
                    {
                        try
                        {
                            string name = parts[1];
                            string value = parts[2];
                            Var var = new Var(name);
                            var.isSet = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (var.Name == vars[j].Name)
                                {
                                    var = vars[j];
                                    var.isSet = true;
                                }
                            }
                            if (!var.isSet)
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a variable named '" + name + "' in line " + codeLine + "\n", true);
                                return;
                            }

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == value)
                                {
                                    value = vars[j].value();
                                }
                            }

                            // Check if the name is "ConsoleInput"
                            if (value == "Console")
                            {
                                // Wait for the user to press the "Send" button
                                float qq = 0;
                                while (!sent)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for console input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                var.set(senttext);
                                var.isSet = true;

                                // Reset the "sent" flag
                                sent = false;
                                senttext = string.Empty;
                            }
                            else if (value == "Key")
                            {
                                // Create the variable with the user's input as the value
                                var.set(keyPreview);
                                var.isSet = true;
                            }
                            else if (value == "KeyDown")
                            {
                                // Create the variable with the user's input as the value
                                var.set(keydown == false ? "0" : "1");
                                var.isSet = true;
                            }
                            else if (value == "StickyKey")
                            {
                                // Create the variable with the user's input as the value
                                var.set(awaitKeyPreview);
                                var.isSet = true;
                            }
                            else if (value == "AwaitKey")
                            {
                                float qq = 0;
                                while (!keydown)
                                {
                                    qq += .1f;
                                    //console.AddText("waiting for key input in line " + codeLine + ": " + qq + Environment.NewLine);
                                    await Task.Delay(100);
                                }

                                // Create the variable with the user's input as the value
                                var.set(keyPreview);
                                var.isSet = true;
                                keydown = false;
                            }
                            /*else if (value == "MousePosition")
                            {
                                string vector = parts[3];
                                float pos = 1111.111100015684465464864f;
                                if (vector == "x")
                                {
                                    pos = MousePosition.X;
                                }
                                else if (vector == "y")
                                {
                                    pos = MousePosition.Y;
                                }
                                else
                                {
                                    console.AddText("There was an error with the vector: " + vector + ". Expected 'x' or 'y' in line " + codeLine + " \n", true);
                                }

                                // Create the variable with the user's input as the value
                                var.set(pos.ToString());
                            }
                            else if (value == "MouseClick")
                            {
                                float pos = 1111.111100015684465464864f;
                                if (mc == 0)
                                {
                                    pos = 0;
                                }
                                else if (mc == 1)
                                {
                                    pos = 1;
                                }
                                else if (mc == 2)
                                {
                                    pos = 2;
                                }
                                else if (mc == 3)
                                {
                                    pos = 3;
                                }
                                // Create the variable with the user's input as the value
                                var.set(pos.ToString());

                                mc = 0;
                            }*/
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("There was an error with 'varInput' Line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("There was an error with 'varInput' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // varInput varName INPUT
                    /*else if (parts[i] == "varSet")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string mid = parts[i + 2];
                            string value = parts[i + 3];

                            value = value.Replace(@"\n", Environment.NewLine);
                            value = value.Replace(@"\_", " ");
                            value = value.Replace(@"\!", string.Empty);

                            Var var = new Var(name);
                            var.isSet = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == var.Name)
                                {
                                    var = vars[j];
                                    var.isSet = true;
                                }
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == mid)
                                {
                                    mid = vars[j].value();
                                }
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == value)
                                {
                                    value = vars[j].value();
                                }
                            }

                            if (!var.isSet)
                            {
                                console.AddText("Could not find a variable named '" + name + "' in line " + codeLine + "\n", true);
                                return;
                            }

                            if (!var.isNumber())
                            {
                                var.stringChange(value, mid);

                                if(!var.isSet) 
                                {
                                    console.AddText("Their was an error with '" + name + "' the called variable is not a number and cannot be divided or multiplied. Line " + codeLine + " \n", true);
                                    return;
                                }
                            }
                            else
                            {
                                var.change(mid, value);

                                if (!var.isSet)
                                {
                                    console.AddText("Their was an error with 'varSet' in line " + codeLine + " \n", true);
                                    return;
                                }
                            }

                            for (int k = 0; k < vars.Count; k++)
                            {
                                if (vars[k].Name == var.Name)
                                {
                                    vars[k] = var;
                                }
                            }

                        }
                        catch
                        {
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // varSet varName (+,-,=,*,/) mulitplier */
                    else if (parts[i] == "varEquals")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string value = parts[i + 2];

                            Var var = new Var(name);
                            var.isSet = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == var.Name)
                                {
                                    var = vars[j];
                                    var.isSet = true;
                                }
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == value)
                                {
                                    value = vars[j].value();
                                }
                            }
                            if (!var.isSet)
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a variable named '" + name + "' in line " + codeLine + "\n", true);
                                return;
                            }


                            if (value == "ReadFile")
                            {
                                string file = "";
                                for (int j = 3; j < parts.Count; j++)
                                {
                                    file += parts[j];
                                    if (j < parts.Count - 1) file += " ";
                                }
                                if (file.Contains("~/"))
                                {
                                    string[] dp = _File.Split(@"\");
                                    string directory = "";
                                    for (int j = 0; j < dp.Length; j++)
                                    {
                                        if (j < dp.Length - 1)
                                        {
                                            directory += dp[j] + @"\\";
                                        }
                                    }
                                    directory += file.Remove(0, 2);
                                    file = directory;
                                }
                                string val = "";
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == file)
                                    {
                                        file = vars[j].value();
                                    }
                                }
                                try
                                {
                                    val = File.ReadAllText(file);
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error reading the file: " + file + " In line " + codeLine + " \n", true);
                                }

                                var.set(val);
                            }
                            else if (value == "FromTextBox")
                            {
                                bool found = false;
                                string val = "";
                                for (int j = 0; j < textboxes.Count; j++)
                                {
                                    if (textboxes[j].Name == parts[3])
                                    {
                                        found = true;
                                        val = textboxes[j].Text;
                                    }
                                }
                                if (!found)
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a textbox named " + parts[3] + " In line " + codeLine + " \n", true);
                                    return;
                                }

                                var.set(val);
                            }
                            else if (value == "IntersectsWith")
                            {
                                string a = parts[3];
                                string b = parts[4];
                                string intersects;

                                GObject A = new GObject(GObject.Type.Square);
                                A.AccessibleName = "Error";
                                GObject B = new GObject(GObject.Type.Square);
                                B.AccessibleName = "Error";

                                for (int j = 0; j < gameObjects.Count; j++)
                                {
                                    if (a == gameObjects[j].Name)
                                    {
                                        A = gameObjects[j];
                                        A.AccessibleName = "";
                                    }
                                    if (b == gameObjects[j].Name)
                                    {
                                        B = gameObjects[j];
                                        B.AccessibleName = "";
                                    }
                                }
                                if (A.AccessibleName != "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the objects: '" + a + "' and '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName == "" && B.AccessibleName != "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + b + "' in line " + codeLine + " \n", true);
                                    return;
                                }
                                else if (A.AccessibleName != "" && B.AccessibleName == "")
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find the object: '" + a + "' in line " + codeLine + " \n", true);
                                    return;
                                }

                                if (A.Bounds.IntersectsWith(B.Bounds))
                                {
                                    intersects = "1";
                                }
                                else
                                {
                                    intersects = "0";
                                }

                                // Create the variable with the user's input as the value
                                var.set(intersects);
                            }
                            else if (value == "FromList")
                            {
                                bool isSet = false;
                                string listN = parts[3];
                                List<Var> list = new List<Var>();

                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[j][0].Name == listN)
                                    {
                                        list = VarList[j];
                                        isSet = true;
                                    }
                                }
                                if (isSet)
                                {
                                    bool isVar = false;
                                    string number = parts[4];
                                    number = getEquation(number, 4, parts);
                                    int numberValue = 0;
                                    for (int k = 0; k < vars.Count; k++)
                                    {
                                        if (vars[k].Name == number.Trim() && vars[k].isNumber())
                                        {
                                            numberValue = (int)vars[k].number;
                                            isVar = true;
                                        }
                                    }
                                    if (!isVar) numberValue = Convert.ToInt16(number);

                                    // Create the variable with the user's input as the value
                                    var.set(list[numberValue].value());
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Could not find a list by the inputted name in " + codeLine + " \n", true);
                                }
                            }
                            else if (value == "Random")
                            {
                                string mi = parts[3];
                                string ma = parts[4];
                                int min = 0;
                                int max = 0;
                                string brackets = "";
                                int ended = 0;
                                int endindex = 0;
                                List<string> textsA = new List<string>();
                                for (int l = 3; l < parts.Count; l++)
                                {
                                    textsA.Add(parts[l]);
                                }
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (mi == vars[j].Name)
                                    {
                                        mi = vars[j].value();
                                    }
                                    if (ma == vars[j].Name)
                                    {
                                        ma = vars[j].value();
                                    }
                                }
                                try
                                {
                                    try
                                    {
                                        min = int.Parse(mi);
                                    }
                                    catch
                                    {
                                        if (parts[3].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsA.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsA[l];
                                                    if (l < textsA.Count - 1) brackets += " ";
                                                }
                                                if (textsA[l].EndsWith(@")"))
                                                {
                                                    if (endindex == 0) endindex = l;
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            min = (int)float.Parse(result);
                                        }
                                    }
                                    try
                                    {
                                        max = int.Parse(ma);
                                    }
                                    catch
                                    {
                                        string newended = textsA[endindex + 1];
                                        List<string> textsB = new List<string>();
                                        for (int l = endindex + 1; l < textsA.Count; l++)
                                        {
                                            textsB.Add(textsA[l]);
                                        }
                                        ended = 0;
                                        brackets = "";
                                        if (newended.StartsWith(@"("))
                                        {
                                            ended = 1;
                                            for (int l = 0; l < textsB.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    brackets += textsB[l];
                                                    if (l < textsB.Count - 1) brackets += " ";
                                                }
                                                if (textsB[l].EndsWith(@")"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                        if (ended != 0)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            max = (int)float.Parse(result);
                                        }
                                    }
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with the minumim and maximum of Random in line " + codeLine + " \n", true);
                                }

                                Random rand = new Random();
                                int rnd = rand.Next(min, max);
                                // Create the variable with the user's input as the value
                                var.set(rnd.ToString());
                            }

                            for (int k = 0; k < vars.Count; k++)
                            {
                                if (vars[k].Name == var.Name)
                                {
                                    vars[k] = var;
                                }
                            }

                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // varEquals varName Modfier valueA(maybe) valueB(maybe)
                    /*else if (parts[i] == "varAddText")
                    {
                        try
                        {
                            string name = parts[i + 1];
                            string value = parts[i + 2];
                            string v;
                            Var var = new Var(name);
                            var.isSet = false;

                            Var val = new Var(value);
                            val.isSet = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == var.Name)
                                {
                                    var = vars[j];
                                    var.isSet = true;
                                }
                            }

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == val.Name)
                                {
                                    val = vars[j];
                                    val.isSet = true;
                                }
                            }

                            if (!var.isSet)
                            {
                                console.AddText("Could not find a variable named '" + name + "' in line " + codeLine + "\n", true);
                                return;
                            }

                            if (var.isNumber())
                            {
                                console.AddText("Their was an error with '" + name + "' the called variable is not text. Line " + codeLine + " \n", true);
                                return;
                            }

                            if (val.isSet) v = val.value();
                            else v = value;

                            var.stringChange(v, "add");

                            if (!var.isSet)
                            {
                                console.AddText("Their was an error with 'varAddText' in line " + codeLine + " \n", true);
                                return;
                            }

                            for (int k = 0; k < vars.Count; k++)
                            {
                                if (vars[k].Name == var.Name)
                                {
                                    vars[k] = var;
                                }
                            }

                        }
                        catch
                        {
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // varAddText varName text*/
                    else if (parts[i] == "list")
                    {
                        try
                        {
                            string next = parts[i + 1];
                            if (next == "new")
                            {
                                string name = parts[i + 2];
                                string mid = parts[i + 3];
                                string valuesRaw = "";
                                for (int j = 4; j < parts.Count; j++)
                                {
                                    valuesRaw += parts[j].ToString();
                                }
                                if (mid == ":")
                                {
                                    List<string> values = valuesRaw.Split(",").ToList();
                                    for (int j = 0; j < values.Count; j++)
                                    {
                                        string brackets = "";
                                        int ended = 0;
                                        if (values[j].StartsWith(@"("))
                                        {
                                            ended = 1;
                                            brackets = values[j];
                                            if (values[j].EndsWith(@")"))
                                            {
                                                ended = 2;
                                            }

                                        }
                                        if (ended == 2)
                                        {
                                            string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                            string result = SolveEquation(equation);
                                            values[j] = result;
                                        }
                                        else if (ended == 1)
                                        {
                                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                            console.AddText("Syntax error in line " + codeLine + ". Expected ')' to end equation \n", true);
                                        }
                                        values[j] = values[j].Replace(@"\n", Environment.NewLine);
                                        values[j] = values[j].Replace(@"\_", " ");
                                        values[j] = values[j].Replace(@"\!", string.Empty);
                                    }

                                    bool allNumbers = true;
                                    bool allText = true;
                                    List<Var> varList = new List<Var>();

                                    for (int j = -1; j < values.Count; j++)
                                    {
                                        if (j == -1)
                                        {
                                            Var var = new Var(name);
                                            var.text = name;
                                            varList.Add(var);

                                        }
                                        else
                                        {
                                            string value = values[j];
                                            for (int k = 0; k < vars.Count; k++)
                                            {
                                                if (vars[k].Name == value)
                                                {
                                                    value = vars[k].value();
                                                }
                                            }
                                            Var var = new Var(j.ToString());
                                            var.set(value);
                                            if (!var.isNumber()) allNumbers = false;
                                            if (var.isNumber()) allText = false;
                                            varList.Add(var);

                                            Var v = var;
                                            v.isSet = true;
                                            v.Name = name + ":" + j;
                                            vars.Add(v);
                                        }
                                    }

                                    if (!allNumbers && !allText)
                                    {
                                        //varList.Clear();
                                        //console.AddText("Their was an error in line " + codeLine + ". Mixed value types \n", true);
                                        //return;
                                    }

                                    VarList.Add(varList);
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their was an error in line " + codeLine + ". Expected ':' to initiate values in the list \n", true);
                                }
                            }
                            else if (next == "add")
                            {
                                string name = parts[i + 2];
                                string value = parts[i + 3];

                                value = getEquation(value, 3, parts);

                                value = value.Replace(@"\n", Environment.NewLine);
                                value = value.Replace(@"\_", " ");
                                value = value.Replace(@"\!", string.Empty);

                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    for (int k = 0; k < vars.Count; k++)
                                    {
                                        if (vars[k].Name == value)
                                        {
                                            value = vars[k].value();
                                        }
                                    }
                                    if (VarList[j][0].Name == name)
                                    {
                                        Var var = new Var(name + ":" + (VarList[j].Count - 1).ToString());
                                        var.set(value);
                                        VarList[j].Add(var);
                                        vars.Add(var);
                                    }
                                }
                            }
                            else if (next == "equals")
                            {
                                string name = parts[i + 2];
                                string index = parts[i + 3];
                                string value = parts[i + 4];

                                value = getEquation(value, 3, parts);

                                value = value.Replace(@"\n", Environment.NewLine);
                                value = value.Replace(@"\_", " ");
                                value = value.Replace(@"\!", string.Empty);

                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == index)
                                    {
                                        index = vars[k].value();
                                    }
                                }
                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    string a = VarList[j][int.Parse(index) + 1].Name;
                                    string b = name + ":" + int.Parse(index);
                                    if (a == b)
                                    {
                                        Var var = VarList[j][int.Parse(index) + 1];
                                        var.set(value);
                                    }
                                }
                            }
                            else if (next == "clear")
                            {
                                string name = parts[i + 2];
                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[j][0].Name == name)
                                    {
                                        for (int k = 1; k < VarList[j].Count; k++)
                                        {
                                            Var var = VarList[j][k];
                                            vars.Remove(var);
                                        }
                                        Var varMain = VarList[j][0];
                                        VarList[j].Clear();
                                        VarList[j].Add(varMain);
                                    }
                                }
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error in line " + codeLine + ". Expected 'new', 'add', 'equals', or 'clear' after 'list' \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // list new name : 0,1,2,3 |or| list add name value |or| list equals name x(index) value |or| list clear name
                    else if (parts[i] == "listRaw")
                    {
                        try
                        {
                            string next = parts[i + 1];
                            if (next == "new")
                            {
                                string name = parts[i + 2];
                                string mid = parts[i + 3];
                                string valuesRaw = parts[i + 4];
                                if (mid == ":")
                                {
                                    List<string> values = valuesRaw.Split(",").ToList();
                                    List<Var> varList = new List<Var>();

                                    for (int j = -1; j < values.Count; j++)
                                    {
                                        if (j == -1)
                                        {
                                            Var var = new Var(name);
                                            var.text = name;
                                            varList.Add(var);

                                        }
                                        else
                                        {
                                            Var var = new Var(j.ToString());
                                            var.set(values[j]);
                                            varList.Add(var);

                                            Var v = var;
                                            v.isSet = true;
                                            v.Name = name + ":" + j;
                                            vars.Add(v);
                                        }
                                    }

                                    VarList.Add(varList);
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their was an error in line " + codeLine + ". Expected ':' to initiate values in the list \n", true);
                                }
                            }
                            else if (next == "add")
                            {
                                string name = parts[i + 2];
                                string value = parts[i + 3];
                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[i][0].Name == name)
                                    {
                                        Var var = new Var(name + ":" + (VarList[i].Count - 1).ToString());
                                        var.set(value);
                                        VarList[i].Add(var);
                                        vars.Add(var);
                                    }
                                }
                            }
                            else if (next == "equals")
                            {
                                string name = parts[i + 2];
                                string index = parts[i + 3];
                                string value = parts[i + 4];
                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == index)
                                    {
                                        index = vars[k].value();
                                    }
                                }
                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    string a = VarList[j][int.Parse(index) + 1].Name;
                                    string b = name + ":" + int.Parse(index);
                                    if (a == b)
                                    {
                                        Var var = VarList[j][int.Parse(index) + 1];
                                        var.set(value);
                                    }
                                }
                            }
                            else if (next == "clear")
                            {
                                string name = parts[i + 2];
                                for (int j = 0; j < VarList.Count; j++)
                                {
                                    if (VarList[j][0].Name == name)
                                    {
                                        for (int k = 1; k < VarList[j].Count; k++)
                                        {
                                            Var var = VarList[j][k];
                                            vars.Remove(var);
                                        }
                                        Var varMain = VarList[j][0];
                                        VarList[j].Clear();
                                        VarList[j].Add(varMain);
                                    }
                                }
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error in line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // listRaw new name : 0,1,2,3 |or| listRaw add name value
                    else if (parts[i] == "writeToFile")
                    {
                        try
                        {
                            string name = parts[1];
                            string file = "";
                            for (int j = 2; j < parts.Count; j++)
                            {
                                file += parts[j];
                                if (j < parts.Count - 1) file += " ";
                            }
                            if (file.Contains("~/"))
                            {
                                string[] dp = _File.Split(@"\");
                                string directory = "";
                                for (int j = 0; j < dp.Length; j++)
                                {
                                    if (j < dp.Length - 1)
                                    {
                                        directory += dp[j] + @"\\";
                                    }
                                }
                                directory += file.Remove(0, 2);
                                file = directory;
                            }
                            string val = "";
                            bool isSet = false;
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == file)
                                {
                                    file = vars[j].value();
                                }
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == name)
                                {
                                    val = vars[j].value();
                                    isSet = true;
                                }
                            }
                            if (!isSet)
                            {
                                val = name;
                                val = val.Replace(@"\n", Environment.NewLine);
                                val = val.Replace(@"\_", " ");
                                val = val.Replace(@"\!", string.Empty);
                            }
                            try
                            {
                                File.WriteAllText(file, val);
                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("There was an error writing the file: " + file + " In line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // writeToFile varName filePath
                    else if (parts[i] == "sound")
                    {
                        try
                        {
                            if (parts[1] == "play")
                            {
                                string file = part[2].Trim();
                                if (file.Contains("~/"))
                                {
                                    string[] dp = _File.Split(@"\");
                                    string directory = "";
                                    for (int j = 0; j < dp.Length; j++)
                                    {
                                        if (j < dp.Length - 1)
                                        {
                                            directory += dp[j] + @"\\";
                                        }
                                    }
                                    directory += file.Remove(0, 2);
                                    file = directory;
                                }
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == file)
                                    {
                                        file = vars[j].value();
                                    }
                                }
                                try
                                {
                                    outputPlayer = new WaveOutEvent();
                                    AudioFileReader player = new AudioFileReader(file);
                                    outputPlayer.Init(player);
                                    outputPlayer.Play();
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error playing the sound: " + file + " In line " + codeLine + " \n", true);
                                }
                            }
                            if (parts[1] == "stop")
                            {
                                outputPlayer.Stop();
                            }
                            if (parts[1] == "volume")
                            {
                                try
                                {
                                    string p = parts[2];
                                    string brackets = "";
                                    int ended = 0;
                                    List<string> texts = new List<string>();
                                    for (int l = 2; l < parts.Count; l++)
                                    {
                                        texts.Add(parts[l]);
                                    }
                                    if (p.StartsWith(@"("))
                                    {
                                        ended = 1;
                                        for (int l = 0; l < texts.Count; l++)
                                        {
                                            if (ended == 1)
                                            {
                                                brackets += texts[l];
                                                if (l < texts.Count - 1) brackets += " ";
                                            }
                                            if (texts[l].EndsWith(@")"))
                                            {
                                                ended = 2;
                                            }
                                        }
                                    }
                                    if (ended != 0)
                                    {
                                        string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                        string result = SolveEquation(equation);
                                        p = result;
                                    }
                                    WaveOutEvent outputPlayer = new WaveOutEvent();
                                    outputPlayer.Volume = float.Parse(p);
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with setting the volume in 'sound' in line " + codeLine + " \n", true);
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // sound name (play,pause,stop) PathToFile(maybe)
                    else if (parts[i] == "endBuild")
                    {
                        try
                        {
                            playing = false;
                            await Task.Delay(100);
                            console.AddText("Build Ended by Script" + Environment.NewLine + Environment.NewLine, true);
                            progressBar1.Value = 0;
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // endBuild
                    else if (parts[i] == "playFile")
                    {
                        try
                        {
                            string awaits = parts[i + 1];
                            string file = "";
                            for (int j = 2; j < parts.Count; j++)
                            {
                                file += parts[j];
                                if (j < parts.Count - 1) file += " ";
                            }
                            if (file.Contains("~/"))
                            {
                                string[] dp = _File.Split(@"\");
                                string directory = "";
                                for (int j = 0; j < dp.Length; j++)
                                {
                                    if (j < dp.Length - 1)
                                    {
                                        directory += dp[j] + @"\\";
                                    }
                                }
                                directory += file.Remove(0, 2);
                                file = directory;
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == file)
                                {
                                    file = vars[j].value();
                                }
                            }
                            string play = string.Empty;

                            try { play = File.ReadAllText(file); }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("could not find a file in the path " + file + " in line " + codeLine + Environment.NewLine, true);
                            }
                            if (awaits == "now")
                            {
                                try { PlayAsync(play); }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their was an error in reading the file " + file + " in line " + codeLine + Environment.NewLine, true);
                                }
                            }
                            else if (awaits == "await")
                            {
                                try { await PlayAsync(play); }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their was an error in reading the file " + file + " in line " + codeLine + Environment.NewLine, true);
                                }
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error with 'playFile' in line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // playFile await PathToFile |or| playFile now PathToFile
                    else if (parts[i] == "ifRaw")
                    {
                        try
                        {
                            string v1 = parts[1];
                            string mid = parts[2];
                            string v2 = parts[3];

                            if (mid == "=" && parts[4] == ":") // equal to
                            {
                                if (v1 == v2)
                                {
                                    string upcode = string.Empty;
                                    for (int j = 5; j < parts.Count; j++)
                                    {
                                        upcode += parts[j] + " ";
                                    }
                                    await PlayAsync(upcode);
                                }
                            }
                            else if (mid == "!" && parts[4] == ":") // not equal
                            {
                                if (v1 != v2)
                                {
                                    string upcode = string.Empty;
                                    for (int j = 5; j < parts.Count; j++)
                                    {
                                        upcode += parts[j] + " ";
                                    }
                                    await PlayAsync(upcode);
                                }
                            }
                            else if (mid == ">" && parts[4] == ":") // less than
                            {
                                int vA = 0, vB = 0;
                                bool vA_int = true, vB_int = true;
                                bool is_true = false;
                                try
                                {
                                    vA = int.Parse(v1);
                                }
                                catch
                                {
                                    vA_int = false;
                                }
                                try
                                {
                                    vB = int.Parse(v2);
                                }
                                catch
                                {
                                    vB_int = false;
                                }

                                if (vA_int && vB_int)
                                {
                                    if (vA > vB) is_true = true;
                                    else return;
                                }
                                else if (!vA_int && vB_int)
                                {
                                    if (v1.Length > vB) is_true = true;
                                    else return;
                                }
                                else if (vA_int && !vB_int)
                                {
                                    if (vA > v2.Length) is_true = true;
                                    else return;
                                }
                                else if (!vA_int && !vB_int)
                                {
                                    if (v1.Length > v2.Length) is_true = true;
                                    else return;
                                }
                                else return;

                                if (is_true)
                                {
                                    string upcode = string.Empty;
                                    for (int j = 5; j < parts.Count; j++)
                                    {
                                        upcode += parts[j] + " ";
                                    }
                                    await PlayAsync(upcode);
                                }
                            }
                            else if (mid == "<" && parts[4] == ":") // greater than
                            {
                                int vA = 0, vB = 0;
                                bool vA_int = true, vB_int = true;
                                bool is_true = false;
                                try
                                {
                                    vA = int.Parse(v1);
                                }
                                catch
                                {
                                    vA_int = false;
                                }
                                try
                                {
                                    vB = int.Parse(v2);
                                }
                                catch
                                {
                                    vB_int = false;
                                }

                                if (vA_int && vB_int)
                                {
                                    if (vA < vB) is_true = true;
                                    else return;
                                }
                                else if (!vA_int && vB_int)
                                {
                                    if (v1.Length < vB) is_true = true;
                                    else return;
                                }
                                else if (vA_int && !vB_int)
                                {
                                    if (vA < v2.Length) is_true = true;
                                    else return;
                                }
                                else if (!vA_int && !vB_int)
                                {
                                    if (v1.Length < v2.Length) is_true = true;
                                    else return;
                                }
                                else return;

                                if (is_true)
                                {
                                    string upcode = string.Empty;
                                    for (int j = 5; j < parts.Count; j++)
                                    {
                                        upcode += parts[j] + " ";
                                    }
                                    await PlayAsync(upcode);
                                }
                            }
                            else
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("The if statement was not formatted correctly in line " + codeLine + " \n", true);
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // if value1 mid value2 : stuff || mid is =,!,>,<
                    else if (parts[i] == "if")
                    {
                        try
                        {
                            multimids Mid = new multimids();
                            string brackets = "";
                            int ended = 0;
                            int endindex = 0;
                            List<string> textsA = new List<string>();
                            for (int l = 1; l < parts.Count; l++)
                            {
                                textsA.Add(parts[l]);
                            }
                            bool aV = false, bV = false;
                            string a = "";
                            string c = "";
                            try
                            {
                                if (parts[1].StartsWith(@"("))
                                {
                                    ended = 1;
                                    for (int l = 0; l < textsA.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            brackets += textsA[l];
                                            if (l < textsA.Count - 1) brackets += " ";
                                        }
                                        if (textsA[l].EndsWith(@")"))
                                        {
                                            if (endindex == 0) endindex = l;
                                            ended = 2;
                                        }
                                    }
                                    if (ended != 0)
                                    {
                                        string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                        string result = SolveEquation(equation);
                                        a = float.Parse(result).ToString();
                                    }
                                }
                                else
                                {
                                    string prt = parts[1];
                                    aV = true;
                                    try
                                    {
                                        a = Convert.ToInt16(prt).ToString();
                                    }
                                    catch
                                    {
                                        bool v = false;
                                        prt = prt.Replace(@"\n", Environment.NewLine);
                                        prt = prt.Replace(@"\_", " ");
                                        prt = prt.Replace(@"\!", string.Empty);
                                        for (int j = 0; j < vars.Count; j++)
                                        {
                                            if (vars[j].Name == prt)
                                            {
                                                a = vars[j].value();
                                                v = true;
                                            }
                                        }
                                        if (!v)
                                        {
                                            a = prt;
                                        }
                                    }
                                }
                                try
                                {
                                    string val = "";
                                    if (aV)
                                    {
                                        val = parts[2];
                                        bV = true;
                                    }
                                    else
                                    {
                                        val = textsA[endindex + 1];
                                    }
                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == val)
                                        {
                                            val = vars[j].value();
                                        }
                                    }
                                    if (val == "=") Mid = multimids.equals;
                                    if (val == "!") Mid = multimids.not;
                                    if (val == ">") Mid = multimids.less;
                                    if (val == "<") Mid = multimids.greater;
                                    endindex++;
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their was an error with 'if' expected '=', '!', '>', or '<' " + codeLine + "\n", true);
                                }
                                string va = "";
                                endindex++;
                                int ne = endindex;
                                if (bV)
                                {
                                    va = parts[3];
                                }
                                else
                                {
                                    va = textsA[ne];
                                }
                                brackets = "";
                                ended = 0;
                                if (va.StartsWith(@"("))
                                {
                                    ended = 1;
                                    for (int l = ne; l < textsA.Count; l++)
                                    {
                                        if (ended == 1)
                                        {
                                            brackets += textsA[l];
                                            if (l < textsA.Count - 1) brackets += " ";
                                        }
                                        if (textsA[l].EndsWith(@")"))
                                        {
                                            if (endindex == ne) endindex = l;
                                            ended = 2;
                                        }
                                    }
                                    if (ended != 0)
                                    {
                                        string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                        string result = SolveEquation(equation);
                                        c = float.Parse(result).ToString();
                                    }
                                }
                                else
                                {
                                    string prt = va;
                                    try
                                    {
                                        c = Convert.ToInt16(prt).ToString();
                                    }
                                    catch
                                    {
                                        bool v = false;
                                        prt = prt.Replace(@"\n", Environment.NewLine);
                                        prt = prt.Replace(@"\_", " ");
                                        prt = prt.Replace(@"\!", string.Empty);
                                        for (int j = 0; j < vars.Count; j++)
                                        {
                                            if (vars[j].Name == prt)
                                            {
                                                c = vars[j].value();
                                                v = true;
                                            }
                                        }
                                        if (!v)
                                        {
                                            c = prt;
                                        }
                                    }
                                }

                            }
                            catch
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Their was an error with 'if' the values were not formatted correctly in line " + codeLine + "\n", true);
                            }

                            bool is_true = ifcheck(Mid, a, c);

                            if (is_true && textsA[endindex + 1] == ":")
                            {
                                string upcode = string.Empty;
                                for (int j = endindex + 2; j < textsA.Count; j++)
                                {
                                    upcode += textsA[j] + " ";
                                }
                                if (upcode != string.Empty) await PlayAsync(upcode);
                                else
                                {
                                    bool aa = false;
                                    for (int z = lines.ToList().IndexOf(line) + 1; z < lines.Length; z++)
                                    {
                                        if (lines[z] == "endIf")
                                        {
                                            aa = true;
                                        }
                                    }
                                    if (!aa)
                                    {
                                        iftrue = false;
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Their is no 'endIf' for the if statement in line " + codeLine + " \n", true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                string upcode = string.Empty;
                                for (int j = endindex + 2; j < textsA.Count; j++)
                                {
                                    upcode += textsA[j] + " ";
                                }
                                if (upcode == string.Empty)
                                {
                                    iftrue = false;
                                }
                                bool aa = false;
                                for (int z = lines.ToList().IndexOf(line) + 1; z < lines.Length; z++)
                                {
                                    if (lines[z].Trim() == "endIf")
                                    {
                                        aa = true;
                                    }
                                }
                                if (!aa)
                                {
                                    iftrue = false;
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("Their is no 'endIf' for the if statement in line " + codeLine + " \n", true);
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    } // if value1 mid value2 : stuff || mid is =,!,>,<
                    else if (parts[i].Contains("//"))
                    { } // // comment text
                    else if (parts[i] == "#" || parts[i] == "#create")
                    {
                        try
                        {
                            if (parts[i] == "#")
                            {

                                if (parts[1] == "create" && parts[2] == "error")
                                {
                                    try
                                    {
                                        string text = "";
                                        for (int j = 3; j < parts.Count; j++)
                                        {
                                            text += parts[j];
                                            if (j < parts.Count - 1) text += " ";
                                        }
                                        if (text == "") int.Parse("jj");
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText(text + ". In line " + codeLine + " \n", true);
                                    }
                                    catch
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Error in line " + codeLine + " \n", true);
                                    }
                                }
                                else
                                {
                                    int.Parse("jj");
                                }
                            }
                            if (parts[i] == "#create")
                            {

                                if (parts[1] == "error")
                                {
                                    try
                                    {
                                        string text = "";
                                        for (int j = 2; j < parts.Count; j++)
                                        {
                                            text += parts[j];
                                            if (j < parts.Count - 1) text += " ";
                                        }
                                        if (text == "") int.Parse("jj");
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText(text + ". In line " + codeLine + " \n", true);
                                    }
                                    catch
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Error in line " + codeLine + " \n", true);
                                    }
                                }
                                else
                                {
                                    int.Parse("jj");
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with '# create error' in line " + codeLine + " \n", true);
                            return;
                        }
                    }// # create error errorText
                    else
                    {
                        if (parts[i] == "#" || parts[i] == "#create" || parts[i] == "#suppress" || parts[i] == "endIf")
                        {
                            return;
                        }
                        if (parts[i].Trim() == "") return;
                        //console.AddText("There is no function named '" + parts[i] + "' in line " + codeLine + Environment.NewLine);
                        try
                        {
                            string name = parts[0];
                            string mid = parts[1];

                            Var var = new Var(name);
                            var.isSet = false;

                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == var.Name)
                                {
                                    var = vars[j];
                                    var.isSet = true;
                                }
                            }
                            for (int j = 0; j < vars.Count; j++)
                            {
                                if (vars[j].Name == mid)
                                {
                                    mid = vars[j].value();
                                }
                            }

                            if (!var.isSet)
                            {
                                if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                console.AddText("Could not find a variable named '" + name + "' in line " + codeLine + "\n", true);
                                return;
                            }

                            if (mid == "Console" || mid == "Key" || mid == "KeyDown" || mid == "StickKey" || mid == "AwaitKey")
                            {
                                if (mid == "Console")
                                {
                                    // Wait for the user to press the "Send" button
                                    float qq = 0;
                                    while (!sent)
                                    {
                                        qq += .1f;
                                        //console.AddText("waiting for console input in line " + codeLine + ": " + qq + Environment.NewLine);
                                        await Task.Delay(100);
                                    }

                                    // Create the variable with the user's input as the value
                                    var.set(senttext);
                                    var.isSet = true;

                                    // Reset the "sent" flag
                                    sent = false;
                                    senttext = string.Empty;
                                }
                                else if (mid == "Key")
                                {
                                    // Create the variable with the user's input as the value
                                    var.set(keyPreview);
                                    var.isSet = true;
                                }
                                else if (mid == "KeyDown")
                                {
                                    // Create the variable with the user's input as the value
                                    var.set(keydown == false ? "0" : "1");
                                    var.isSet = true;
                                }
                                else if (mid == "StickyKey")
                                {
                                    // Create the variable with the user's input as the value
                                    var.set(awaitKeyPreview);
                                    var.isSet = true;
                                }
                                else if (mid == "AwaitKey")
                                {
                                    float qq = 0;
                                    while (!keydown)
                                    {
                                        qq += .1f;
                                        //console.AddText("waiting for key input in line " + codeLine + ": " + qq + Environment.NewLine);
                                        await Task.Delay(100);
                                    }

                                    // Create the variable with the user's input as the value
                                    var.set(keyPreview);
                                    var.isSet = true;
                                    keydown = false;
                                }
                                else
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error with 'varInput' Line " + codeLine + " \n", true);
                                }
                            }
                            else if (mid == "ReadFile" || mid == "FromTextBox" || mid == "IntersectsWith" || mid == "FromList" || mid == "Random")
                            {
                                if (mid == "ReadFile")
                                {
                                    string file = "";
                                    for (int j = 2; j < parts.Count; j++)
                                    {
                                        file += parts[j];
                                        if (j < parts.Count - 1) file += " ";
                                    }
                                    if (file.Contains("~/"))
                                    {
                                        string[] dp = _File.Split(@"\");
                                        string directory = "";
                                        for (int j = 0; j < dp.Length; j++)
                                        {
                                            if (j < dp.Length - 1)
                                            {
                                                directory += dp[j] + @"\\";
                                            }
                                        }
                                        directory += file.Remove(0, 2);
                                        file = directory;
                                    }
                                    string val = "";
                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (vars[j].Name == file)
                                        {
                                            file = vars[j].value();
                                        }
                                    }
                                    try
                                    {
                                        val = File.ReadAllText(file);
                                    }
                                    catch
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("There was an error reading the file: " + file + " In line " + codeLine + " \n", true);
                                    }

                                    var.set(val);
                                }
                                else if (mid == "FromTextBox")
                                {
                                    bool found = false;
                                    string val = "";
                                    for (int j = 0; j < textboxes.Count; j++)
                                    {
                                        if (textboxes[j].Name == parts[2])
                                        {
                                            found = true;
                                            val = textboxes[j].Text;
                                        }
                                    }
                                    if (!found)
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find a textbox named " + parts[3] + " In line " + codeLine + " \n", true);
                                        return;
                                    }

                                    var.set(val);
                                }
                                else if (mid == "IntersectsWith")
                                {
                                    string a = parts[2];
                                    string b = parts[3];
                                    string intersects;

                                    GObject A = new GObject(GObject.Type.Square);
                                    A.AccessibleName = "Error";
                                    GObject B = new GObject(GObject.Type.Square);
                                    B.AccessibleName = "Error";

                                    for (int j = 0; j < gameObjects.Count; j++)
                                    {
                                        if (a == gameObjects[j].Name)
                                        {
                                            A = gameObjects[j];
                                            A.AccessibleName = "";
                                        }
                                        if (b == gameObjects[j].Name)
                                        {
                                            B = gameObjects[j];
                                            B.AccessibleName = "";
                                        }
                                    }
                                    if (A.AccessibleName != "" && B.AccessibleName != "")
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find the objects: '" + a + "' and '" + b + "' in line " + codeLine + " \n", true);
                                        return;
                                    }
                                    else if (A.AccessibleName == "" && B.AccessibleName != "")
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find the object: '" + b + "' in line " + codeLine + " \n", true);
                                        return;
                                    }
                                    else if (A.AccessibleName != "" && B.AccessibleName == "")
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find the object: '" + a + "' in line " + codeLine + " \n", true);
                                        return;
                                    }

                                    if (A.Bounds.IntersectsWith(B.Bounds))
                                    {
                                        intersects = "1";
                                    }
                                    else
                                    {
                                        intersects = "0";
                                    }

                                    // Create the variable with the user's input as the value
                                    var.set(intersects);
                                }
                                else if (mid == "FromList")
                                {
                                    bool isSet = false;
                                    string listN = parts[2];
                                    List<Var> list = new List<Var>();

                                    for (int j = 0; j < VarList.Count; j++)
                                    {
                                        if (VarList[j][0].Name == listN)
                                        {
                                            list = VarList[j];
                                            isSet = true;
                                        }
                                    }
                                    if (isSet)
                                    {
                                        bool isVar = false;
                                        string number = parts[3];
                                        number = getEquation(number, 3, parts);
                                        int numberValue = 0;
                                        for (int k = 0; k < vars.Count; k++)
                                        {
                                            if (vars[k].Name == number.Trim() && vars[k].isNumber())
                                            {
                                                numberValue = (int)vars[k].number;
                                                isVar = true;
                                            }
                                        }
                                        if (!isVar) numberValue = Convert.ToInt16(number);

                                        // Create the variable with the user's input as the value
                                        var.set(list[numberValue].value());
                                    }
                                    else
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Could not find a list by the inputted name in " + codeLine + " \n", true);
                                    }
                                }
                                else if (mid == "Random")
                                {
                                    string mi = parts[2];
                                    string ma = parts[3];
                                    int min = 0;
                                    int max = 0;
                                    string brackets = "";
                                    int ended = 0;
                                    int endindex = 0;
                                    List<string> textsA = new List<string>();
                                    for (int l = 2; l < parts.Count; l++)
                                    {
                                        textsA.Add(parts[l]);
                                    }
                                    for (int j = 0; j < vars.Count; j++)
                                    {
                                        if (mi == vars[j].Name)
                                        {
                                            mi = vars[j].value();
                                        }
                                        if (ma == vars[j].Name)
                                        {
                                            ma = vars[j].value();
                                        }
                                    }
                                    try
                                    {
                                        try
                                        {
                                            min = int.Parse(mi);
                                        }
                                        catch
                                        {
                                            if (parts[2].StartsWith(@"("))
                                            {
                                                ended = 1;
                                                for (int l = 0; l < textsA.Count; l++)
                                                {
                                                    if (ended == 1)
                                                    {
                                                        brackets += textsA[l];
                                                        if (l < textsA.Count - 1) brackets += " ";
                                                    }
                                                    if (textsA[l].EndsWith(@")"))
                                                    {
                                                        if (endindex == 0) endindex = l;
                                                        ended = 2;
                                                    }
                                                }
                                            }
                                            if (ended != 0)
                                            {
                                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                                string result = SolveEquation(equation);
                                                min = (int)float.Parse(result);
                                            }
                                        }
                                        try
                                        {
                                            ma = textsA[endindex + 1];
                                            max = int.Parse(ma);
                                        }
                                        catch
                                        {
                                            string newended = textsA[endindex + 1];
                                            List<string> textsB = new List<string>();
                                            for (int l = endindex + 1; l < textsA.Count; l++)
                                            {
                                                textsB.Add(textsA[l]);
                                            }
                                            ended = 0;
                                            brackets = "";
                                            if (newended.StartsWith(@"("))
                                            {
                                                ended = 1;
                                                for (int l = 0; l < textsB.Count; l++)
                                                {
                                                    if (ended == 1)
                                                    {
                                                        brackets += textsB[l];
                                                        if (l < textsB.Count - 1) brackets += " ";
                                                    }
                                                    if (textsB[l].EndsWith(@")"))
                                                    {
                                                        ended = 2;
                                                    }
                                                }
                                            }
                                            if (ended != 0)
                                            {
                                                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                                string result = SolveEquation(equation);
                                                max = (int)float.Parse(result);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        console.AddText("There was an error with the minumim and maximum of Random in line " + codeLine + " \n", true);
                                    }

                                    Random rand = new Random();
                                    int rnd = rand.Next(min, max);
                                    // Create the variable with the user's input as the value
                                    var.set(rnd.ToString());
                                }

                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == var.Name)
                                    {
                                        vars[k] = var;
                                    }
                                }
                            }
                            else
                            {
                                string value = parts[2];
                                value = getEquation(value, 2, parts);

                                value = value.Replace(@"\n", Environment.NewLine);
                                value = value.Replace(@"\_", " ");
                                value = value.Replace(@"\!", string.Empty);
                                for (int j = 0; j < vars.Count; j++)
                                {
                                    if (vars[j].Name == value)
                                    {
                                        value = vars[j].value();
                                    }
                                }

                                if (!var.isNumber())
                                {
                                    string text = "";
                                    bool done = false;
                                    for (int j = 2; j < parts.Count; j++)
                                    {
                                        if (parts[j].Contains("//")) done = true;
                                        text += !done ? parts[j] : string.Empty;
                                        if (j < parts.Count - 1 && !done) text += " ";
                                    }

                                    bool isVar = false;
                                    string val = text;
                                    List<string> texts = text.Split(" ").ToList();
                                    int ended = 0;
                                    string brackets = "";
                                    int started = 0;
                                    int count = 0;

                                    for (int j = 0; j < texts.Count; j++)
                                    {
                                        for (int k = 0; k < vars.Count; k++)
                                        {
                                            if (vars[k].Name == texts[j])
                                            {
                                                isVar = true;
                                                texts[j] = vars[k].value();
                                            }
                                        }
                                        if (texts[j].StartsWith(@"\("))
                                        {
                                            ended = 1;
                                            count = 1;
                                            started = j;
                                            for (int l = j; l < texts.Count; l++)
                                            {
                                                if (ended == 1)
                                                {
                                                    count++;
                                                    brackets += texts[l];
                                                    if (l < texts.Count - 1) brackets += " ";
                                                }
                                                if (texts[l].EndsWith(@")\"))
                                                {
                                                    ended = 2;
                                                }
                                            }
                                        }
                                    }
                                    if (ended != 0)
                                    {
                                        string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                                        string result = SolveEquation(equation);
                                        texts[started] = result;

                                        int endIndex = started + count;
                                        if (endIndex < texts.Count)
                                        {
                                            texts.RemoveRange(started + 1, count - 2);
                                        }
                                        else
                                        {
                                            texts.RemoveRange(started + 1, texts.Count - (started + 1));
                                        }
                                    }
                                    text = "";
                                    for (int j = 0; j < texts.Count; j++)
                                    {
                                        text += texts[j];
                                        if (j < texts.Count - 1) text += " ";
                                    }
                                    val = text;

                                    if (!isVar)
                                    {
                                        val = val.Replace(@"\n", Environment.NewLine);
                                        val = val.Replace(@"\_", " ");
                                        val = val.Replace(@"\!", string.Empty);

                                        value = val;
                                        return;
                                    }
                                    else
                                    {
                                        value = text;
                                    }

                                    var.stringChange(value, mid);

                                    if (!var.isSet)
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Their was an error with '" + name + "' the called variable is not a number and cannot be divided or subtracted. Line " + codeLine + " \n", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    var.change(mid, value);

                                    if (!var.isSet)
                                    {
                                        if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                        console.AddText("Their was an error with 'varSet' in line " + codeLine + " \n", true);
                                        return;
                                    }
                                }

                                for (int k = 0; k < vars.Count; k++)
                                {
                                    if (vars[k].Name == var.Name)
                                    {
                                        vars[k] = var;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error in line " + codeLine + " \n", true);
                            return;
                        }
                    }
                }
            }
        }
        private bool ifcheck(multimids Mid, string v1, string v2)
        {

            if (Mid == multimids.equals) // equal to
            {
                if (v1 == v2)
                {
                    return true;
                }
            }
            else if (Mid == multimids.not) // not equal
            {
                if (v1 != v2)
                {
                    return true;
                }
            }
            else if (Mid == multimids.less) // less than
            {
                float vA = 0, vB = 0;
                bool vA_int = true, vB_int = true;
                try
                {
                    vA = float.Parse(v1);
                }
                catch
                {
                    vA_int = false;
                }
                try
                {
                    vB = float.Parse(v2);
                }
                catch
                {
                    vB_int = false;
                }

                if (vA_int && vB_int)
                {
                    if (vA > vB) return true;
                }
                else if (!vA_int && vB_int)
                {
                    if (v1.Length > vB) return true;
                }
                else if (vA_int && !vB_int)
                {
                    if (vA > v2.Length) return true;
                }
                else if (!vA_int && !vB_int)
                {
                    if (v1.Length > v2.Length) return true;
                }
            }
            else if (Mid == multimids.greater) // greater than
            {
                float vA = 0, vB = 0;
                bool vA_int = true, vB_int = true;
                try
                {
                    vA = float.Parse(v1);
                }
                catch
                {
                    vA_int = false;
                }
                try
                {
                    vB = float.Parse(v2);
                }
                catch
                {
                    vB_int = false;
                }

                if (vA_int && vB_int)
                {
                    if (vA < vB) return true;
                }
                else if (!vA_int && vB_int)
                {
                    if (v1.Length < vB) return true;
                }
                else if (vA_int && !vB_int)
                {
                    if (vA < v2.Length) return true;
                }
                else if (!vA_int && !vB_int)
                {
                    if (v1.Length < v2.Length) return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
        private string getEquation(string value, int a, List<string> parts)
        {
            string brackets = "";
            int ended = 0;
            List<string> texts = new List<string>();
            for (int l = a; l < parts.Count; l++)
            {
                texts.Add(parts[l]);
            }
            if (value.StartsWith(@"("))
            {
                ended = 1;
                for (int l = 0; l < texts.Count; l++)
                {
                    if (ended == 1)
                    {
                        brackets += texts[l];
                        if (l < texts.Count - 1) brackets += " ";
                    }
                    if (texts[l].EndsWith(@")"))
                    {
                        ended = 2;
                    }
                }
            }
            if (ended != 0)
            {
                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                string result = SolveEquation(equation);
                value = result;
            }
            else if (ended == 1)
            {
                console.AddText("Syntax error in line " + codeLine + ". Expected ')' to end equation \n", true);
            }

            return value;
        }
        private int[] getEquationWithNext(string value, int a, List<string> parts)
        {
            string brackets = "";
            int ended = 0;
            int next = 0;
            List<string> texts = new List<string>();
            for (int l = a; l < parts.Count; l++)
            {
                texts.Add(parts[l]);
            }
            if (value.StartsWith(@"("))
            {
                ended = 1;
                for (int l = 0; l < texts.Count; l++)
                {
                    if (ended == 1)
                    {
                        brackets += texts[l];
                        if (l < texts.Count - 1) brackets += " ";
                    }
                    if (texts[l].EndsWith(@")"))
                    {
                        ended = 2;
                        if (next == 0) next = l;
                    }
                }
            }
            if (ended != 0)
            {
                string equation = brackets.TrimStart('\\').TrimEnd('\\').Replace("\\", "");
                string result = SolveEquation(equation);
                value = result;
            }
            else if (ended == 1)
            {
                console.AddText("Syntax error in line " + codeLine + ". Expected ')' to end equation \n", true);
            }

            return new int[] { int.Parse(value), next };
        }
        private int[] find_value(List<string> parts, int next, int def)
        {
            int v = def;
            if (parts.Count - 1 >= next)
            {
                try
                {
                    v = int.Parse(parts[next]);
                    next++;
                }
                catch
                {
                    int[] result = getEquationWithNext(parts[next], next, parts);
                    v = result[0];
                    next += result[1] + 1;
                }
            }
            return new int[] { v, next };
        }
        private string SolveEquation(string equation)
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
        private void autoopenproject()
        {
            try
            {
                string filePath_ = "C:\\Users\\Public\\Temp\\ezcode\\startup_project.txt";

                if (File.Exists(filePath_))
                {
                    string filePath = File.ReadAllText(filePath_);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        listBox1.Items.Clear();
                        openfiles.Clear();

                        string[] lines = File.ReadAllLines(filePath);

                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] line = lines[i].Split("|");
                            string a = line[0];
                            string b = line[1];

                            openfiles.Add(new Open_File(a, b));
                            listBox1.Items.Add(a);
                        }
                    }

                    toolStripTextBox4.Text = filePath;
                }
            }
            catch
            {
                MessageBox.Show("There was an error opening project list. This is likely due to not being able to read 'startup_project.txt' in C:\\Users\\Public\\Temp\\ezcode\\", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Save(bool autosaved)
        {
            string a = "";
            if (_File == "NOTHING" || _File == string.Empty)
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "ezCode Files (*.ezcode)|*.ezcode";
                    saveFileDialog.ShowDialog();

                    a = saveFileDialog.FileName;
                    StreamWriter streamWriter = new StreamWriter(a);
                    streamWriter.Write(txt.Text);
                    streamWriter.Close();

                    saved = true;
                    if (!autosaved) console.AddText("File Saved \n", false);
                }
                catch
                {
                    MessageBox.Show("Did not save");
                }
            }
            else
            {
                try
                {
                    StreamWriter streamWriter = new StreamWriter(_File);
                    streamWriter.Write(txt.Text);
                    streamWriter.Close();

                    a = _File;

                    saved = true;
                    if (!autosaved) console.AddText("File Saved \n", false);
                }
                catch
                {
                    MessageBox.Show("Did not save");
                }
            }

            int equal = -1;
            for (int i = 0; i < openfiles.Count; i++)
            {
                if (a == openfiles[i].Directory)
                {
                    equal = i;
                }
            }
            if (equal == -1)
            {
                string[] b = a.Split("\\");
                openfiles.Add(new Open_File(b[b.Length - 1], a));
                listBox1.Items.Add(b[b.Length - 1]);
            }
        }
        private void Open(string file, string? safefile = "null")
        {
            try
            {
                if (saved)
                {
                    // The current document is saved, so open a new document
                    if (file == "" || file == "NOTHING")
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "ezCode Files (*.ezcode)|*.ezcode";
                        openFileDialog.ShowDialog();
                        file = openFileDialog.FileName;
                        safefile = openFileDialog.SafeFileName;
                    }

                    StreamReader streamReader = new StreamReader(file);
                    txt.Text = streamReader.ReadToEnd();
                    streamReader.Close();

                    _File = file;

                    openfiles.Add(new Open_File(safefile, file));
                    listBox1.Items.Add(safefile);

                    saved = true;
                }
                else
                {
                    // The current document is not saved, so prompt the user to save their work
                    DialogResult dr = MessageBox.Show("Your work has not been saved, do you want to save?", "Not Saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        // Save the user's work
                        Save(false);

                        // Open the new document
                        if (file == "" || file == "NOTHING")
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "ezCode Files (*.ezcode)|*.ezcode";
                            openFileDialog.ShowDialog();
                            file = openFileDialog.FileName;
                            safefile = openFileDialog.SafeFileName;
                        }

                        StreamReader streamReader = new StreamReader(file);
                        txt.Text = streamReader.ReadToEnd();
                        streamReader.Close();

                        openfiles.Add(new Open_File(file, safefile));
                        listBox1.Items.Add(safefile);

                        _File = file;
                        saved = true;
                    }
                    else if (dr == DialogResult.No)
                    {
                        // Discard the user's changes and open the new document
                        if (file == "" || file == "NOTHING")
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            openFileDialog.Filter = "ezCode Files (*.ezcode)|*.ezcode";
                            openFileDialog.ShowDialog();
                            file = openFileDialog.FileName;
                            safefile = openFileDialog.SafeFileName;
                        }

                        StreamReader streamReader = new StreamReader(file);
                        txt.Text = streamReader.ReadToEnd();
                        streamReader.Close();

                        openfiles.Add(new Open_File(file, safefile));
                        listBox1.Items.Add(safefile);

                        _File = file;
                        saved = true;
                    }
                    else //cancel
                    {
                        // Cancel the action, so the document is not opened
                    }
                }
            }
            catch
            {
                MessageBox.Show("Did not open a document");
            }
        }
        private void Undo()
        {
            if (undoStack.Count > 1)
            {
                // Pop the current text off the undo stack
                redoStack.Push(undoStack.Pop());

                // Get the previous text from the undo stack and display it
                txt.Text = undoStack.Peek();
            }
        }
        private void Redo()
        {
            if (redoStack.Count > 0)
            {
                // Pop the current text off the redo stack
                string nextText = redoStack.Pop();

                // Push the current text onto the undo stack
                undoStack.Push(txt.Text);

                // Display the next text in the txt control
                txt.Text = nextText;
            }
        }
        private async void InGameButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            string file = b.AccessibleDescription;

            if (file != string.Empty && File.Exists(file))
            {
                if (file.Contains("~/"))
                {
                    string[] dp = _File.Split(@"\");
                    string directory = "";
                    for (int j = 0; j < dp.Length; j++)
                    {
                        if (j < dp.Length - 1)
                        {
                            directory += dp[j] + @"\\";
                        }
                    }
                    directory += file.Remove(0, 2);
                    file = directory;
                }
                for (int j = 0; j < vars.Count; j++)
                {
                    if (vars[j].Name == file)
                    {
                        file = vars[j].value();
                    }
                }
                string play = string.Empty;

                try { play = File.ReadAllText(file); }
                catch { console.AddText("could not find a file in the path " + file + " in line " + codeLine + Environment.NewLine, true); }
                try { await PlayAsync(play); }
                catch { console.AddText("Their was an error in reading the file " + file + " in line " + codeLine + Environment.NewLine, true); }
            }
            else
            {
                console.AddText("Could not find the file: " + file + " \n", true);
            }
        }
        private void SetFont(Control label, string name, int size, FontStyle style)
        {
            Font replacementFont = new Font(name, size, style);
            label.Font = replacementFont;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) //Ctrl+S, Ctrl+Z, ...
        {
            if (msg.Msg == 256)
            {
                if (keyData == (Keys.Control | Keys.S))
                {
                    // Save the current text
                    saveToolStripMenuItem.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.Z))
                {
                    // Undo the last change
                    //Undo();
                }
                else if (keyData == (Keys.Control | Keys.Y))
                {
                    // Redo the last undone change
                    //Redo();
                }
                else if (keyData == (Keys.Control | Keys.D))
                {
                    toolStripMenuItem3.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.P))
                {
                    playToolStripMenuItem.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.O))
                {
                    openToolStripMenuItem.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.N))
                {
                    newToolStripMenuItem.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.Q))
                {
                    stopToolStripMenuItem.PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) //save
        {
            Save(false);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e) //open
        {
            Open("NOTHING");
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e) //exit
        {
            Application.Exit();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) //new
        {
            if (saved == true)
            {
                // The current document is already saved, so clear the text box
                _File = "NOTHING";
                txt.Text = string.Empty;
            }
            else
            {
                // The current document is not saved, so prompt the user to save their work
                DialogResult dr = MessageBox.Show("Your work has not been saved, do you want to save?", "Not Saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    // Save the user's work
                    Save(false);
                    _File = "NOTHING";
                    txt.Text = string.Empty;
                }
                else if (dr == DialogResult.No)
                {
                    // Discard the user's changes and create a new document
                    _File = "NOTHING";
                    txt.Text = string.Empty;
                }
                else //cancel
                {
                    // Cancel the action, so the document is not cleared
                }
            }

        }
        private async void toolStripButton1_Click(object sender, EventArgs e) //play
        {
            if (!playing && !debugging)
            {
                playing = true;
                progressBar1.Maximum = txt.Lines.Count;

                for (int i = 0; i < labels.Count; i++)
                {
                    Space.Controls.Remove(labels[i]);
                }
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    Space.Controls.Remove(gameObjects[i]);
                }
                for (int i = 0; i < textboxes.Count; i++)
                {
                    Space.Controls.Remove(textboxes[i]);
                }
                for (int i = 0; i < buttons.Count; i++)
                {
                    Space.Controls.Remove(buttons[i]);
                }
                labels.Clear();
                buttons.Clear();
                gameObjects.Clear();
                textboxes.Clear();
                vars.Clear();
                VarList.Clear();
                Group.Clear();
                console.AddText("Build Started" + Environment.NewLine, false);
                await PlayAsync(txt.Text);
            }
            playing = false;
            await Task.Delay(100);
            console.AddText("Build Ended" + Environment.NewLine + Environment.NewLine, false);
            progressBar1.Value = 0;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //form closing
        {
            if ((_File == "NOTHING" || _File == string.Empty) && txt.Text == string.Empty)
            {
                Application.Exit();
            }
            else if (saved == true)
            {
                Application.Exit();
            }
            else
            {
                DialogResult dr = MessageBox.Show("Your work has not been saved, do you want to save?", "Not Saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    // Save the user's work
                    // For example, you could call a Save() method here:
                    Save(false);
                    Application.Exit();
                }
                else if (dr == DialogResult.No)
                {
                    // Discard the user's changes and exit
                    Application.Exit();
                }
                else //cancel
                {
                    // Cancel the form closing event
                    e.Cancel = true;
                }
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e) //undo
        {
            Undo();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e) //redo
        {
            Redo();
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e) //stoped
        {
            playing = false;
            console.AddText("Build Stopped" + Environment.NewLine, false);
            progressBar1.Maximum = 1;
            progressBar1.Value = 0;
        }
        private void ConsoleSend_KeyDown(object sender, KeyEventArgs e) //Console Sender pressed enter
        {
            sent = false;
            if (e.KeyCode == Keys.Enter)
            {
                senttext = ConsoleSend.Text;
                sent = true;
                ConsoleSend.Clear();
            }
        }
        private void Send_Click(object sender, EventArgs e) //send
        {
            senttext = ConsoleSend.Text;
            sent = true;
            ConsoleSend.Clear();
        }
        private void clear_Click(object sender, EventArgs e) //clear
        {
            console.Clear();
        }
        private void txt_KeyPress(object sender, KeyPressEventArgs e) //typing
        {
            saved = false;

            try
            {
                if (txt.Text == File.ReadAllText(_File)) saved = true;
            }
            catch
            {
                saved = false;
            }

            if (_File != "NOTHING" && autosave)
            {
                Save(true);
                saved = true;
            }

            // Push the current text onto the undo stack
            undoStack.Push(txt.Text);

            // Clear the redo stack, since the user cannot redo after making a new change
            redoStack.Clear();
        }
        private void txt_KeyDown(object sender, KeyEventArgs e) //typing
        {
            saved = false;

            try
            {
                if (txt.Text == File.ReadAllText(_File)) saved = true;
            }
            catch
            {
                saved = false;
            }

            if (_File != "NOTHING" && autosave)
            {
                Save(true);
                saved = true;
            }

            // Push the current text onto the undo stack
            undoStack.Push(txt.Text);

            // Clear the redo stack, since the user cannot redo after making a new change
            redoStack.Clear();
        }
        private void console_TextChanged(object sender, EventArgs e) //scrolls to bottom of console
        {
            console.SelectionStart = console.TextLength;
            console.ScrollToCaret();
            Console2.SelectionStart = Console2.TextLength;
            Console2.ScrollToCaret();
        }
        private void timer1_Tick(object sender, EventArgs e) //playing and stopping buttons
        {
            if (playing && !debugging)
            {
                quitDebuggerToolStripMenuItem.Enabled = false;
                nextLineToolStripMenuItem.Enabled = false;
                debuggerEnabledToolStripMenuItem.Enabled = false;
                playToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = true;
                txt.Enabled = false;
                button2.Enabled = false;
                Console2.Enabled = false;
            }
            else if (!playing && !debugging)
            {
                button2.Enabled = false;
                quitDebuggerToolStripMenuItem.Enabled = false;
                nextLineToolStripMenuItem.Enabled = false;
                debuggerEnabledToolStripMenuItem.Enabled = true;
                playToolStripMenuItem.Enabled = true;
                stopToolStripMenuItem.Enabled = false;
                txt.Enabled = true;
                Console2.Enabled = false;
            }
            else if (playing && debugging)
            {
                button2.Enabled = true;
                quitDebuggerToolStripMenuItem.Enabled = true;
                nextLineToolStripMenuItem.Enabled = true;
                debuggerEnabledToolStripMenuItem.Enabled = false;
                playToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = false;
                Console2.Enabled = true;
            }
            if (_File != "NOTHING")
            {
                toolStripTextBox1.Text = _File;
            }
            Console2.Text = console.Text;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e) //keydown
        {
            keyPreview = e.KeyCode.ToString();
            awaitKeyPreview = e.KeyCode.ToString();
            keydown = true;
            if (e.KeyCode == Keys.F10)
            {
                debuggerEnabledToolStripMenuItem.PerformClick();
            }
            else if (e.KeyCode == Keys.F11)
            {
                nextLineToolStripMenuItem.PerformClick();
            }
            else if (e.KeyCode == Keys.F12)
            {
                quitDebuggerToolStripMenuItem.PerformClick();
            }
            if (e.KeyCode == Keys.Q && e.KeyCode == Keys.Control)
            {
                stopToolStripMenuItem.PerformClick();
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e) //keyup
        {
            keyPreview = "";
            keydown = false;
        }
        private void Space_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.None)
            {
                mc = 0;
            }
            else if (e.Button == MouseButtons.Left)
            {
                mc = 1;
            }
            else if (e.Button == MouseButtons.Right)
            {
                mc = 2;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                mc = 3;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All Files (*.*)|*.*|Script File (*.ezcode)|*.ezcode|Text File (*.txt)|*.txt";
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                List<Open_File> open_Files = new List<Open_File>();
                List<string> directories = new List<string>();
                List<string> names = new List<string>();
                foreach (string name in openFileDialog.FileNames)
                {
                    directories.Add(name);
                }
                foreach (string name in openFileDialog.SafeFileNames)
                {
                    names.Add(name);
                }
                for (int i = 0; i < names.Count; i++)
                {
                    open_Files.Add(new Open_File(names[i], directories[i]));
                }
                for (int i = 0; i < open_Files.Count; i++)
                {
                    openfiles.Add(open_Files[i]);
                    listBox1.Items.Add(open_Files[i].Name);
                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string file = openfiles[listBox1.SelectedIndex].Directory;
                if (saved)
                {

                    StreamReader streamReader = new StreamReader(file);
                    txt.Text = streamReader.ReadToEnd();
                    streamReader.Close();

                    _File = file;

                    saved = true;
                }
                else
                {
                    // The current document is not saved, so prompt the user to save their work
                    DialogResult dr = MessageBox.Show("Your work has not been saved, do you want to save?", "Not Saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.Yes)
                    {
                        // Save the user's work
                        Save(false);

                        StreamReader streamReader = new StreamReader(file);
                        txt.Text = streamReader.ReadToEnd();
                        streamReader.Close();

                        _File = file;
                        saved = true;
                    }
                    else if (dr == DialogResult.No)
                    {
                        StreamReader streamReader = new StreamReader(file);
                        txt.Text = streamReader.ReadToEnd();
                        streamReader.Close();

                        _File = file;
                        saved = true;
                    }
                    else //cancel
                    {
                        // Cancel the action, so the document is not opened
                    }
                }
            }
            catch
            {
                MessageBox.Show("Could not open a document");
            }
        }
        private void openProjectListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    listBox1.Items.Clear();
                    openfiles.Clear();

                    string[] lines = File.ReadAllLines(openFileDialog.FileName);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] line = lines[i].Split("|");
                        string a = line[0];
                        string b = line[1];

                        openfiles.Add(new Open_File(a, b));
                        listBox1.Items.Add(a);
                    }
                }
            }
            catch
            {
                MessageBox.Show("There was an error opening project list");
            }
        }
        private void saveProjectListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Text File (*.txt)|*.txt";
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    string file = string.Empty;

                    foreach (Open_File of in openfiles)
                    {
                        file += of.Name + "|" + of.Directory + Environment.NewLine;
                    }

                    StreamWriter stream = new StreamWriter(openFileDialog.FileName);
                    stream.Write(file);
                    stream.Close();
                }
            }
            catch
            {
                MessageBox.Show("There was an error saving project list");
            }
        }
        private void packageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Public\\Temp\\ezcode\\programs1.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            string[] lines = File.ReadAllLines(filePath);

            try
            {
                if (!string.IsNullOrEmpty(lines[0]))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = lines[0];
                    process.Start();
                    return;
                }
            }
            catch
            {
                // Handle exception if necessary
            }

            DialogResult dialogResult = MessageBox.Show("Do you have ezcode-Packager already installed?", "ezcode-Packager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable File (*.exe)|*.exe";
                openFileDialog.ShowDialog();

                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = openFileDialog.FileName;
                    process.Start();

                    try
                    {
                        File.WriteAllText(filePath, openFileDialog.FileName);
                    }
                    catch
                    {
                        // Handle exception if necessary
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("You will need to install ezcode-Packager to package your project", "Installation needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                // Do nothing
            }
        }
        private void playInViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Public\\Temp\\ezcode\\programs2.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            string[] lines = File.ReadAllLines(filePath);

            try
            {
                if (!string.IsNullOrEmpty(lines[0]))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = lines[0];
                    process.StartInfo.Arguments = _File;
                    process.Start();
                    return;
                }
            }
            catch
            {
                // Handle exception if necessary
            }

            DialogResult dialogResult = MessageBox.Show("Do you have ezcode_Viewer already installed?", "ezcode_Viewer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Executable File (*.exe)|*.exe";
                openFileDialog.ShowDialog();

                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = openFileDialog.FileName;
                    process.StartInfo.Arguments = _File;
                    process.Start();

                    try
                    {
                        File.WriteAllText(filePath, openFileDialog.FileName);
                    }
                    catch
                    {
                        // Handle exception if necessary
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("You will need to install ezcode_Viewer to View your Script", "Installation needed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                // Do nothing
            }
        }
        private void removeFileFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string number = Interaction.InputBox("Index of the file (starts with zero)", "Remove File From List");
            try
            {
                int n = int.Parse(number);
                listBox1.Items.RemoveAt(n);
                openfiles.RemoveAt(n);
            }
            catch
            {
                MessageBox.Show("Could find the file specified", "error", MessageBoxButtons.OK);
            }
        }
        private void viewDocsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "https://ez-code.web.app";
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;

                }
            }
        }
        private async void debuggerEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!playing && !debugging)
            {
                playing = true;
                debugging = true;
                debugWait = true;
                progressBar1.Maximum = txt.Lines.Count;

                for (int i = 0; i < labels.Count; i++)
                {
                    Space.Controls.Remove(labels[i]);
                }
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    Space.Controls.Remove(gameObjects[i]);
                }
                for (int i = 0; i < textboxes.Count; i++)
                {
                    Space.Controls.Remove(textboxes[i]);
                }
                for (int i = 0; i < buttons.Count; i++)
                {
                    Space.Controls.Remove(buttons[i]);
                }
                labels.Clear();
                buttons.Clear();
                gameObjects.Clear();
                textboxes.Clear();
                vars.Clear();
                VarList.Clear();
                console.AddText("Debug Started" + Environment.NewLine, false);
                await PlayAsync(txt.Text);
            }
            playing = false;
            debugging = false;
            await Task.Delay(100);
            console.AddText("Debug Ended" + Environment.NewLine + Environment.NewLine, false);
            progressBar1.Value = 0;
        }
        private void quitDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentLineTxt.Text = "";
            listBox2.Items.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            playing = false;
            debugging = false;
            console.AddText("Debug Stopped" + Environment.NewLine, false);
            progressBar1.Maximum = 1;
            progressBar1.Value = 0;
        }
        private void nextLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(comboBox1, e);
            debugWait = false;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (i == comboBox1.SelectedIndex)
                {
                    xpos1.Text = gameObjects[i].Left.ToString();
                    ypos1.Text = gameObjects[i].Top.ToString();
                    xscale1.Text = gameObjects[i].Width.ToString();
                    yscale1.Text = gameObjects[i].Height.ToString();
                }
            }
            for (int i = 0; i < labels.Count; i++)
            {
                if (i == comboBox2.SelectedIndex)
                {
                    xpos2.Text = labels[i].Left.ToString();
                    ypos2.Text = labels[i].Top.ToString();
                    xscale2.Text = labels[i].Text.ToString();
                    yscale2.Text = labels[i].Font.ToString();
                }
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Public\\Temp\\ezcode\\autosave.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            autosave = !autosave;
            if (autosave)
            {
                File.WriteAllText(filePath, "On");
                console.AddText("Autosave On" + Environment.NewLine, false);
                toolStripMenuItem3.Text = "Autosave: On";
            }
            else
            {
                File.WriteAllText(filePath, "Off");
                console.AddText("Autosave Off" + Environment.NewLine, false);
                toolStripMenuItem3.Text = "Autosave: Off";
            }
        }
        private void openProjectListToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.ShowDialog();
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    toolStripTextBox4.Text = openFileDialog.FileName;
                }
            }
            catch
            {
                MessageBox.Show("There was an error opening project list");
            }
        }
        private void clearProjectListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripTextBox4.Clear();
        }
        private void toolStripTextBox4_TextChanged(object sender, EventArgs e)
        {
            string filePath = "C:\\Users\\Public\\Temp\\ezcode\\startup_project.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            try
            {
                File.WriteAllText(filePath, toolStripTextBox4.Text);
            }
            catch
            {
                MessageBox.Show("An Unexpected error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void viewAllHotKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string HotKeys = @"Hotkeys:
Ctrl + S - Save
Ctrl + D - Toggle Autosave
Ctrl + O - Open
Ctrl + A - Select All
Ctrl + X - Cut
Ctrl + C - Copy
Ctrl + V - Paste
Ctrl + Z - Undo
Ctrl + Y - Redo
Ctrl + P - Play
Ctrl + Q - Quit
Ctrl + B - Bookmark
Ctrl + Shift + B - Remove Bookmark
F10 - Start Debugger
F11 - Nextline Debugger
F12 - Quit Debugger
Ctrl + Mouse Wheel - Zoom
Ctrl + 0 - Reset Zoom
Ctrl + Minus - Zoom Out
Ctrl + Plus - Zoom In
Ctrl + F - Find
F3 - Find Next
Tab - Increase Indent
Shift + Tab - Decrease Indent
Ctrl + G - Go to Dialog
Ctrl + H - Replace Dialog
Ctrl + N - Next Bookmark
Ctrl + Shift + N - Previous Bookmark
Ctrl + U - Uppercase
Ctrl + Shift + U - Lowercase
Ctrl + Shift + C - Comment Selected";

            MessageBox.Show(HotKeys, "HotKeys", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void viewDocsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string url = "https://ez-code.web.app/Tutorials.html";
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;

                }
            }
        }
        private void addFolderToListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            openFileDialog.ShowDialog();
            if (!string.IsNullOrEmpty(openFileDialog.SelectedPath))
            {
                List<string> files = Directory.GetFiles(openFileDialog.SelectedPath).ToList();
                List<Open_File> open_Files = new List<Open_File>();
                List<string> directories = new List<string>();
                List<string> names = new List<string>();
                foreach (string name in files)
                {
                    directories.Add(name);
                }
                foreach (string name in files)
                {
                    string[] _ = name.Split("\\");
                    names.Add(_[_.Length - 1]);
                }
                for (int i = 0; i < names.Count; i++)
                {
                    open_Files.Add(new Open_File(names[i], directories[i]));
                }
                for (int i = 0; i < open_Files.Count; i++)
                {
                    openfiles.Add(open_Files[i]);
                    listBox1.Items.Add(open_Files[i].Name);
                }
            }
        }
        private void clearListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            openfiles.Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //New File
            NewFile filedi = new NewFile();
            filedi.ShowDialog();
            Open(filedi.file, filedi.safefile);
        }
    }
    class Open_File
    {
        public string Name { get; set; }
        public string Directory { get; set; }
        public Open_File(string? name = "", string? directory = "")
        {
            Name = name;
            Directory = directory;
        }
    }
    class Group
    {
        public List<Button> Buttons = new List<Button>();
        public List<Label> Labels = new List<Label>();
        public List<GObject> Objects = new List<GObject>();
        public List<TextBox> Textboxes = new List<TextBox>();
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int scaleX { get; set; }
        public int scaleY { get; set; }
        public Group(string name)
        {
            this.Name = name;
        }
    }
}