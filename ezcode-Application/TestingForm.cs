using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Data;
using Variables;
using GControls;
//using NAudio.Wave;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ezcode_Application
{
    partial class TestingForm : Form
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
        public static string title = "title";
        public string _File;
        public static bool showerrors = false;
        bool sent;
        int codeLine = 0;
        string keyPreview = "";
        string awaitKeyPreview = "";
        string senttext;
        bool keydown;
        string txt;
        RichTextBox console = new RichTextBox();
        //WaveOutEvent outputPlayer;
        List<Label> labels = new List<Label>();
        List<GObject> gameObjects = new List<GObject>();
        List<Var> vars = new List<Var>();
        List<List<Var>> VarList = new List<List<Var>>();
        List<TextBox> textboxes = new List<TextBox>();
        List<Button> buttons = new List<Button>();

        public TestingForm()
        {
            InitializeComponent();
            this.Load += Form_Load;
            this.FormClosing += TestingForm_FormClosing;
            this.PreviewKeyDown += TestingForm_PreviewKeyDown;
            this.KeyPress += TestingForm_KeyPress;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private async void Form_Load(object sender, EventArgs e)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + title + ".gp.ezproj1";
            string[] a = file.Split('\\');
            string f = "Apps\\" + title;
            string d = "C:\\Users\\Public\\Temp\\ezcode\\" + f + "\\";
            if (file != "")
            {
                try
                {
                    try
                    {
                        if (Directory.Exists(d)) Directory.Delete(d, true);
                    }
                    catch 
                    {
                        
                    } 
                    Directory.CreateDirectory(d);
                    ZipFile.ExtractToDirectory(file, d);

                    //string[] lis = File.ReadAllLines(d + "Lisence.txt");

                    StreamReader streamReader = new StreamReader(d + "root_.ezcode");
                    txt = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                catch
                {
                    MessageBox.Show("Could not play project");
                }
            }
            else
            {
                MessageBox.Show("Could not play project");
                return;
            }
            labels.Clear();
            buttons.Clear();
            textboxes.Clear();
            gameObjects.Clear();
            vars.Clear();
            VarList.Clear();
            console.Clear();
            _File = d + "root_.ezcode";
            await PlayAsync(txt);
        }
        private async void TestingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string file = AppDomain.CurrentDomain.BaseDirectory + title + ".gp.ezproj1";
                string[] a = file.Split('\\');
                string f = "Apps\\" + title;
                string d = "C:\\Users\\Public\\Temp\\ezcode\\" + f + "\\";
                string[] q = Directory.GetFiles(d);
                for (int i = 0; i < q.Length; i++)
                {
                    try
                    {
                        File.Delete(q[i]);
                    }
                    catch
                    {

                    }
                }
                await Task.Delay(250);
            }
            catch
            {

            }
        }

        private async Task PlayAsync(string code)
        {
            string[] lines = code.Split('\n');

            List<string> loopCode = new List<string>(); // Create a list to store the loop code

            bool breaked = false;
            bool hasEnded = false;
            int endl = 0;
            int stackNow = 0;

            for (int w = 0; w < lines.Length; w++)
            {
                codeLine = w + 1;
                string[] part = lines[w].Trim().Split(' ').ToArray();
                if (part[0] == "loop")
                {
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
                            if (!breaked) await ExecuteLine(loopLine);
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
                    if (keyPreview == null) keyPreview = "";
                    if (awaitKeyPreview == null) awaitKeyPreview = "";
                    List<string> parts = line.Trim().Split(' ').ToList();
                    int i = 0;
                    if (parts[i] == "print")
                    {
                        try
                        {
                            string text = "";
                            for (int j = 1; j < parts.Count; j++)
                            {
                                text += parts[j];
                                if (j < parts.Count - 1) text += " ";
                            }

                            bool isVar = false;
                            string val = text;
                            List<string> texts = text.Split(' ').ToList();
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

                            List<string> texts = labelText.Split(' ').ToList();
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
                            string text = parts[i + 2];

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

                            Button b = new Button();
                            b.Left = 0;
                            b.Top = 0;
                            b.Name = name;
                            b.Text = text;
                            b.FlatStyle = FlatStyle.Flat;
                            b.FlatAppearance.BorderSize = 1;

                            this.Controls.Add(b);
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
                                if (vars[j].Name == code)
                                {
                                    code = vars[j].value();
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
                                string[] dp = _File.Split('\\');
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
                            string text = parts[i + 2];

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

                            TextBox tb = new TextBox();
                            tb.Left = 0;
                            tb.Top = 0;
                            tb.Name = name;
                            tb.Text = text;

                            this.Controls.Add(tb);
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

                            go.Left = 0;
                            go.Top = 0;
                            go.Width = 50;
                            go.Height = 50;
                            go.Name = name;
                            go.BackColor = Color.Black;

                            this.Controls.Add(go);
                            gameObjects.Add(go);
                        }
                        catch
                        {
                            if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                            console.AddText("Their was an error with 'object' in line " + codeLine + " \n", true);
                            return;
                        }
                    } // object name x
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
                                string[] dp = _File.Split('\\');
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

                            this.Controls.Add(label);
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
                                Var var = new Var(name);
                                var.set(senttext);
                                var.isSet = true;
                                var.stack = stackNow;

                                vars.Add(var);

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
                                Var var = new Var(name);
                                var.set(intersects);
                                var.isSet = true;
                                var.stack = stackNow;

                                vars.Add(var);
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
                                Var var = new Var(name);
                                var.set(rnd.ToString());
                                var.isSet = true;
                                var.stack = stackNow;

                                vars.Add(var);
                            }
                            else if (value == "KeyInput")
                            {
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(keyPreview);
                                var.isSet = true;
                                var.stack = stackNow;

                                vars.Add(var);
                            }
                            else if (value == "KeyDown")
                            {
                                // Create the variable with the user's input as the value
                                Var var = new Var(name);
                                var.set(keydown == false ? "0" : "1");
                                var.isSet = true;
                                var.stack = stackNow;

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
                                var.stack = stackNow;

                                vars.Add(var);
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
                                    string[] dp = _File.Split('\\');
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
                                var.stack = stackNow;

                                vars.Add(var);
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
                                Var var = new Var(name);
                                var.set(val);
                                var.isSet = true;
                                var.stack = stackNow;

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
                                    Var var = new Var(name);
                                    var.set(list[numberValue].value());
                                    var.isSet = true;
                                    var.stack = stackNow;

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
                                Var var = new Var(name);
                                var.set(value);
                                var.isSet = true;
                                var.stack = stackNow;

                                vars.Add(var);
                            }
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
                                    string[] dp = _File.Split('\\');
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
                                    string[] dp = _File.Split('\\');
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
                                    List<string> values = valuesRaw.Split(',').ToList();
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
                                    List<string> values = valuesRaw.Split(',').ToList();
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
                                string[] dp = _File.Split('\\');
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
                                    string[] dp = _File.Split('\\');
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
                                    //outputPlayer = new WaveOutEvent();
                                    //AudioFileReader player = new AudioFileReader(file);
                                    //outputPlayer.Init(player);
                                    //outputPlayer.Play();
                                }
                                catch
                                {
                                    if (line.Contains("# suppress error") || line.Contains("#suppress error")) return;
                                    console.AddText("There was an error playing the sound: " + file + " In line " + codeLine + " \n", true);
                                }
                            }
                            if (parts[1] == "stop")
                            {
                                //outputPlayer.Stop();
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
                                    //WaveOutEvent outputPlayer = new WaveOutEvent();
                                    //outputPlayer.Volume = float.Parse(p);
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
                            await Task.Delay(300);
                            Application.Exit();
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
                                string[] dp = _File.Split('\\');
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
                                await PlayAsync(upcode);
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
                        if (parts[i] == "#" || parts[i] == "#create" || parts[i] == "#suppress")
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
                                        string[] dp = _File.Split('\\');
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
        private async void InGameButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            string file = b.AccessibleDescription;

            if (file != string.Empty && File.Exists(file))
            {
                if (file.Contains("~/"))
                {
                    string[] dp = _File.Split('\\');
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
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
            keyPreview = e.KeyCode.ToString();
            awaitKeyPreview = e.KeyCode.ToString();
            keydown = true;
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            MessageBox.Show("keyup");
            keyPreview = "";
            keydown = false;
        }
        private void TestingForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show(e.KeyChar.ToString());
            keyPreview = e.KeyChar.ToString();
            awaitKeyPreview = e.KeyChar.ToString();
            keydown = true;
        }
        private void TestingForm_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            MessageBox.Show(e.KeyCode.ToString());
            keyPreview = e.KeyCode.ToString();
            awaitKeyPreview = e.KeyCode.ToString();
            keydown = true;
        }
        private void SetFont(Control label, string name, int size, FontStyle style)
        {
            Font replacementFont = new Font(name, size, style);
            label.Font = replacementFont;
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
                        if (l < texts.Count - 1) brackets += "";
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
    }
    public static class ControlExtensions
    {
        public static void AddText(this Control control, string text, bool error)
        {
            if (control is RichTextBox && error)
            {
                MessageBox.Show(text, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

namespace GControls
{
    using System.Drawing.Drawing2D;
    using Point = System.Drawing.Point;
    interface IObject
    {
        PointF[] Points { get; set; }
        int Poly { get; set; }
    }
    public partial class GObject : IObject
    {
        public enum Type
        {
            Square,
            Circle,
            Triangle,
            Polygon,
            Custom,
        }
        public PointF[] Points { get; set; }
        public int Poly { get; set; }
        public Type Square { get; }
        int types;

        public GObject(Type type, int? poly = null, PointF[] points = null)
        {
            //sets all values
            if (poly != null) Poly = (int)poly;
            if (points != null) Points = points;

            switch (type)
            {
                case Type.Square:
                    types = 1;
                    break;
                case Type.Circle:
                    types = 2;
                    break;
                case Type.Triangle:
                    types = 3;
                    break;
                case Type.Polygon:
                    types = 4;
                    break;
                case Type.Custom:
                    types = 5;
                    break;
            }
        }
    }
    public partial class GObject : PictureBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath obj = new GraphicsPath())
            {
                if (types == 1)
                {
                    Rectangle rectangle = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    obj.AddRectangle(rectangle);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 2)
                {
                    obj.AddEllipse(0, 0, this.Width - 1, this.Height - 1);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 3)
                {
                    obj.AddPolygon(new Point[] {
                        new Point(this.Width / 2, 0),
                        new Point(0, Height),
                        new Point(Width, Height) }
                    );
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 4 && Poly > 2)
                {
                    PointF center = new PointF(this.Width / 2, this.Height / 2);
                    PointF[] points = new PointF[Poly];
                    for (int i = 0; i < Poly; i++)
                    {
                        float angle = (2 * (float)Math.PI / Poly) * i;
                        float x = center.X + (this.Width / 2) * (float)Math.Cos(angle);
                        float y = center.Y + (this.Height / 2) * (float)Math.Sin(angle);
                        points[i] = new PointF(x, y);
                    }
                    obj.AddPolygon(points);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
                else if (types == 5)
                {
                    PointF center = new PointF(this.Width / 2, this.Height / 2);
                    obj.AddPolygon(Points);
                    Region = new Region(obj);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawEllipse(new Pen(new SolidBrush(this.BackColor), 1), 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }
    }
}
namespace Variables
{
    using System.Globalization;
    internal interface Ivar
    {
        string Name { get; set; }
        float number { get; set; }
        string text { get; set; }
        bool isSet { get; set; }
        int stack { get; set; }
        void set(string value);
        void change(string middle, string multiplier);
        void stringChange(string adds, string mid);
        bool isNumber();
        string value();
    }
    public class Var : Ivar
    {
        public string Name { get; set; }
        public float number { get; set; }
        public string text { get; set; }
        public int stack { get; set; }
        public bool isSet { get; set; }

        string StringStandered = "IF YOU GET THIS. IT IS AN ERROR MESSAGE - (23dsffdsf86dg45b64ytu7578566434654fg4g4fhjd) = just some random text";
        float FloatStatendered = 1111.111100015684465464864f;

        public Var(string name)
        {
            Name = name;
            number = FloatStatendered;
            text = StringStandered;
        }
        public void set(string value)
        {
            try
            {
                number = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                text = value;
            }
        }
        public void change(string middle, string multiplier)
        {
            try
            {
                float value = float.Parse(multiplier, CultureInfo.InvariantCulture.NumberFormat);
                float final;
                if (middle == "+")
                {
                    final = number + value;
                }
                else if (middle == "*")
                {
                    final = number * value;
                }
                else if (middle == "-")
                {
                    final = number - value;
                }
                else if (middle == "/")
                {
                    final = number / value;
                }
                else if (middle == "=")
                {
                    final = value;
                }
                else
                {
                    final = number;
                }
                number = final;
            }
            catch
            {
                isSet = false;
            }
        }
        public void stringChange(string value, string mid)
        {
            try
            {
                bool setted = false;
                if (!isNumber())
                {
                    switch (mid)
                    {
                        case "+":
                            setted = true;
                            text += value;
                            break;
                        case "=":
                            setted = true;
                            text = value;
                            break;
                        case "-":
                            int v = 0;
                            try
                            {
                                v = int.Parse(value);
                            }
                            catch
                            {
                                isSet = false;
                                return;
                            }
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (i >= text.Length - v)
                                {
                                    text = text.Remove(i);
                                }
                            }
                            setted = true;
                            break;
                    }
                    if (!setted)
                    {
                        isSet = false;
                    }
                }
                else
                {
                    isSet = false;
                }
            }
            catch
            {
                isSet = false;
            }
        }
        public bool isNumber()
        {
            if (number != FloatStatendered)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string value()
        {
            if (isNumber())
            {
                return number.ToString();
            }
            else
            {
                return text.ToString();
            }
        }

    }
}
