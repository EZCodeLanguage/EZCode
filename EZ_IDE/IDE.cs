using EZCode;
using FastColoredTextBoxNS;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EZ_IDE
{
    public partial class IDE : Form
    {
        #region AutoComplete
        readonly AutocompleteMenu popupMenu;
        readonly string[] keywords = { "list", "group", "sound", "var", "print", "await", "bringto", "clear", "destroy", "event", "file", "input", "intersects", "messagebox", "stop", "shape", "textbox", "label", "button", "window", "global", "if", "loop", "else", "DEVPORTAL", "method", "endmethod" };
        readonly string[] methods = { /*"new", "change", "add", "equals", "remove", "destroy", "destroyall", "clear", "close", "open", "play", "playall", "volume", "stop", "playloop"*/ };
        readonly string[] snippets = { "if ^ : ", "if ^ : \n{\n\n}", "loop ^ {\n\n\n\n}", "var ^ : input console" };
        readonly string[] declarationSnippets = { "method ^\n\n\n\nendmethod", "method ^ : \n\n\n\nendmethod" };



        private void BuildAutocompleteMenu()
        {
            List<AutocompleteItem> items = new List<AutocompleteItem>();

            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem(item) { ImageIndex = 1 });
            foreach (var item in declarationSnippets)
                items.Add(new DeclarationSnippet(item) { ImageIndex = 0 });
            foreach (var item in methods)
                items.Add(new MethodAutocompleteItem(item) { ImageIndex = 2 });
            foreach (var item in keywords)
                items.Add(new AutocompleteItem(item));

            items.Add(new InsertSpaceSnippet());
            items.Add(new InsertSpaceSnippet(@"^(\w+)([=<>!:]+)(\w+)$"));
            items.Add(new InsertEnterSnippet());

            //set as autocomplete source
            popupMenu.Items.SetAutocompleteItems(items);
        }

        /// <summary>
        /// This item appears when any part of snippet text is typed
        /// </summary>
        class DeclarationSnippet : SnippetAutocompleteItem
        {
            public DeclarationSnippet(string snippet)
                : base(snippet)
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var pattern = Regex.Escape(fragmentText);
                if (Regex.IsMatch(Text, "\\b" + pattern, RegexOptions.IgnoreCase))
                    return CompareResult.Visible;
                return CompareResult.Hidden;
            }
        }

        /// <summary>
        /// Divides numbers and words: "123AND456" -> "123 AND 456"
        /// Or "i=2" -> "i = 2"
        /// </summary>
        class InsertSpaceSnippet : AutocompleteItem
        {
            string pattern;

            public InsertSpaceSnippet(string pattern) : base("")
            {
                this.pattern = pattern;
            }

            public InsertSpaceSnippet()
                : this(@"^(\d+)([a-zA-Z_]+)(\d*)$")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                if (Regex.IsMatch(fragmentText, pattern))
                {
                    Text = InsertSpaces(fragmentText);
                    if (Text != fragmentText)
                        return CompareResult.Visible;
                }
                return CompareResult.Hidden;
            }

            public string InsertSpaces(string fragment)
            {
                var m = Regex.Match(fragment, pattern);
                if (m == null)
                    return fragment;
                if (m.Groups[1].Value == "" && m.Groups[3].Value == "")
                    return fragment;
                return (m.Groups[1].Value + " " + m.Groups[2].Value + " " + m.Groups[3].Value).Trim();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return Text;
                }
            }
        }

        /// <summary>
        /// Inerts line break after '}'
        /// </summary>
        class InsertEnterSnippet : AutocompleteItem
        {
            Place enterPlace = Place.Empty;

            public InsertEnterSnippet()
                : base("[Line break]")
            {
            }

            public override CompareResult Compare(string fragmentText)
            {
                var r = Parent.Fragment.Clone();
                while (r.Start.iChar > 0)
                {
                    if (r.CharBeforeStart == '}')
                    {
                        enterPlace = r.Start;
                        return CompareResult.Visible;
                    }

                    r.GoLeftThroughFolded();
                }

                return CompareResult.Hidden;
            }

            public override string GetTextForReplace()
            {
                //extend range
                FastColoredTextBoxNS.Range r = Parent.Fragment;
                Place end = r.End;
                r.Start = enterPlace;
                r.End = r.End;
                //insert line break
                return Environment.NewLine + r.Text;
            }

            public override void OnSelected(AutocompleteMenu popupMenu, SelectedEventArgs e)
            {
                base.OnSelected(popupMenu, e);
                if (Parent.Fragment.tb.AutoIndent)
                    Parent.Fragment.tb.DoAutoIndent();
            }

            public override string ToolTipTitle
            {
                get
                {
                    return "Insert line break after '}'";
                }
            }
        }

        #endregion

        #region EZCode

        EzCode ezcode = new EzCode();
        FileInfo file;
        EZProj proj;
        public bool useConsole = true;
        ProjectType projectType;
        public enum ProjectType
        {
            Project,
            Script,
            None
        }
        public void Start(FileInfo _file, ProjectType _projectType = ProjectType.None)
        {
            ProjectSettings.Initialize(ref ezcode);
            int d = 0;
            bool window = false;
            proj = new EZProj(_file, _file.FullName);
            projectType = _projectType;
            file = _file;
            if (_projectType == ProjectType.Project)
            {
                if (proj.Window)
                {
                    window = true;
                }
                else if (proj.IsVisual)
                {
                    d = 1;
                }
                else if (!proj.IsVisual)
                {
                    d = 0;
                }
                if (proj.Debug)
                {
                    d = 2;
                }
            }
            if (!window)
            {
                tabControl1.SelectedIndex = d;
            }
            else
            {
                useConsole = false;
            }
            if (proj.Name != null) Text = proj.Name;
            else Text = _file.FullName;
            ezcode.errorColor = Color.FromArgb(255, 20, 20);
            ezcode.normalColor = !window ? output.ForeColor : Color.Black;

            AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;
            KeyDown += ezcode.KeyInput_Down;
            KeyUp += ezcode.KeyInput_Up;
            MouseWheel += ezcode.MouseInput_Wheel;
            MouseMove += ezcode.MouseInput_Move;
            MouseDown += ezcode.MouseInput_Down;
            MouseUp += ezcode.MouseInput_Up;
            output.MouseWheel += ezcode.MouseInput_Wheel;
            output.MouseMove += ezcode.MouseInput_Move;
            output.MouseDown += ezcode.MouseInput_Down;
            output.MouseUp += ezcode.MouseInput_Up;
            visualoutput.MouseWheel += ezcode.MouseInput_Wheel;
            visualoutput.MouseMove += ezcode.MouseInput_Move;
            visualoutput.MouseDown += ezcode.MouseInput_Down;
            visualoutput.MouseUp += ezcode.MouseInput_Up;

            Play();
        }

        private async void Play()
        {
            if (ProjectType.Script == projectType) await ezcode.Play(File.ReadAllText(file.FullName));
            else if (ProjectType.Project == projectType) await ezcode.PlayFromProj(proj);
        }

        private void Player_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            output.Clear();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            ezcode.ConsoleInput(input.Text);
            input.Clear();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Send.PerformClick();
            }
        }

        private void output_TextChanged(object sender, EventArgs e)
        {
            ezcode.ScrollToEnd(true, output.ForeColor, Color.FromArgb(255, 20, 20));
        }

        private void fastColoredTextBox1_KeyPressing(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == char.Parse("\b") && ModifierKeys.HasFlag(Keys.Control) && fctb.Text == "")
            {
                e.Handled = true;
            }
        }
        #endregion

        TreeManager Manager;

        public IDE(string path = "")
        {
            InitializeComponent();

            KeyPreview = true;
            fctb.Focus();

            //create autocomplete popup menu
            popupMenu = new AutocompleteMenu(fctb);
            popupMenu.Items.ImageList = imageList1;
            popupMenu.SearchPattern = @"[\w\.:=!<>]";
            popupMenu.AllowTabKey = true;

            popupMenu.BackColor = Color.Gray;
            popupMenu.SelectedColor = Color.CornflowerBlue;

            BuildAutocompleteMenu();

            Manager = new TreeManager(this);

            if (path == "") Manager.SetTreeNodes();
            else Manager.OpenPath(path);

            Settings.StartUp();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256)
            {
                if (keyData == (Keys.Control | Keys.O))
                {
                    // open folder
                    folderToolStripMenuItem1.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.S))
                {
                    // save file
                    saveToolStripMenuItem.PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region events
        private void folderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // open folder
            Manager.OpenFolder();
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // open file
            Manager.OpenFile();
        }

        private void playProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play project
        }

        private void playFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play file
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // exit
            Settings.Exit(this);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save file
            Manager.SaveFile();
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // after selected file
            Manager.SelectedNode(e);
            changeTime = 0;
        }
        private void Tree_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            // before selected file
            try
            {
                if (fctb.Text != File.ReadAllText(FileURLTextBox.Text))
                {
                    bool @continue = Manager.SaveFile(!Settings.Auto_Save);
                    if (!@continue)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch
            {
                Manager.SelectedCatchCheck(FileURLTextBox.Text);
            }
        }
        private void settingsPreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings_Preferences settings_Preferences = new Settings_Preferences();
            settings_Preferences.ShowDialog();
        }


        public int changeTime = 0;
        private void fctb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (Settings.Auto_Save)
                {

                    changeTime++;
                    if (changeTime > 10)
                    {
                        Manager.SaveFile();
                        changeTime = 0;
                    }
                }
            }
            catch
            {
                // nothing
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new file
        }

        private void folderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new folder
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new project
        }

        private void fileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // create file
        }

        private void folderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // create folder
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // delete
        }

        private void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // project settings
        }

        private void includeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // include
        }

        private void excludeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // exclude
        }

        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // help/docs
        }

        private void insertBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // inset breakpoint
        }

        private void startDebugSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // start debug session
        }

        private void nextSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // next segment
        }

        private void continueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // continue
        }

        private void endDebugSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // end debug session
        }

        private void debugSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // debug settings
        }
        private void clearTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear tree view
            Tree.Nodes.Clear();
            Settings.Open_Folder_Path = "";
        }
        #endregion

    }
}
