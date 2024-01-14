using EZCode;
using EZCode.Debug;
using EZCode.EZPlayer;
using EZCode.Variables;
using FastColoredTextBoxNS;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Debugger = EZCode.Debug.Debugger;

namespace EZ_IDE
{
    public partial class IDE : Form
    {
        public static readonly string Version = "1.0.10";

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
            foreach (var item in sources)
                items.Add(new MethodAutocompleteItem2(item));

            items.Add(new InsertSpaceSnippet());
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

        private string[] sources
        {
            get
            {
                return fctb.Text.Split(new[] { '|', '\n', ' ' }).Select(x => x.Trim()).Where(y => !keywords.Contains(y)).Distinct().ToArray();
            }
        }
        /// <summary>
        /// This autocomplete item appears after dot
        /// </summary>
        public class MethodAutocompleteItem2 : MethodAutocompleteItem
        {
            readonly string firstPart;
            readonly string lastPart;

            public MethodAutocompleteItem2(string text)
                : base(text)
            {
                var i = text.LastIndexOf('.');
                if (i < 0)
                    firstPart = text;
                else
                {
                    firstPart = text[..i];
                    lastPart = text[(i + 1)..];
                }
            }

            public override CompareResult Compare(string fragmentText)
            {
                int i = fragmentText.LastIndexOf('.');

                if (i < 0)
                {
                    if (firstPart.StartsWith(fragmentText) && string.IsNullOrEmpty(lastPart))
                        return CompareResult.VisibleAndSelected;
                    //if (firstPart.ToLower().Contains(fragmentText.ToLower()))
                    //  return CompareResult.Visible;
                }
                else
                {
                    var fragmentFirstPart = fragmentText[..i];
                    var fragmentLastPart = fragmentText[(i + 1)..];


                    if (firstPart != fragmentFirstPart)
                        return CompareResult.Hidden;

                    if (lastPart != null && lastPart.StartsWith(fragmentLastPart))
                        return CompareResult.VisibleAndSelected;

                    if (lastPart != null && lastPart.ToLower().Contains(fragmentLastPart.ToLower()))
                        return CompareResult.Visible;

                }

                return CompareResult.Hidden;
            }

            public override string GetTextForReplace()
            {
                if (lastPart == null)
                    return firstPart;

                return firstPart + "." + lastPart;
            }

            public override string ToString()
            {
                if (lastPart == null)
                    return firstPart;

                return lastPart;
            }
        }

        #endregion

        #region EZCode

        EzCode ezcode = new EzCode();
        FileInfo file;
        EZProj ezproj;
        ProjectType projectType;
        Player dedicated_player;
        public enum ProjectType
        {
            Project,
            Script,
            None
        }
        public void Start(FileInfo _file, ProjectType _projectType = ProjectType.None, bool debug = false)
        {
            switch (_projectType)
            {
                case ProjectType.Script:
                    project.Directory = FileURLTextBox.Text;
                    ezproj = new EZProj(new EzCode() { Code = File.ReadAllText(_file.FullName) }, _file.FullName);
                    break;
                case ProjectType.Project:
                    project.Directory = Settings.Current_Project_File;
                    ezproj = new EZProj(_file);
                    break;
            }
            project.Initialize(ref ezcode, visualoutput, output);

            if (!debug && Settings.Play_In_Dedicated_Window)
            {
                dedicated_player = new Player(ezproj, false);
                dedicated_player.Show();
            }
            else
            {
                int d = 0;
                bool window = false;
                projectType = _projectType;
                file = _file;

                if (ezproj.Window)
                {
                    window = true;
                }
                else if (ezproj.IsVisual)
                {
                    d = 1;
                }
                else if (!ezproj.IsVisual)
                {
                    d = 0;
                }
                if (debug || ezproj.Debug)
                {
                    d = 2;
                }

                tabControl1.SelectedIndex = d;

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

                if (!debug)
                    Play();
            }
        }

        private async void Play()
        {
            if (ProjectType.Script == projectType) await ezcode.Play(File.ReadAllText(file.FullName));
            else if (ProjectType.Project == projectType) await ezcode.PlayFromProj(ezproj);
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            output.Clear();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            ezcode.ConsoleInput(input.Text);
            InputDebug.Clear();
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
            MiniConsole.Text = output.Text;
        }
        #endregion

        #region Property Grids
        class PropertyGridControls
        {
            public PropertyGridControls(EzCode ez) => ezcode = ez;
            public EzCode ezcode = new EzCode();
            [Category("Controls")]
            [Description("The controls the program contains")]
            public Control[] Controls
            {
                get
                {
                    Control[] controls = new Control[0];
                    try
                    {
                        controls = ezcode.AllControls.ToArray();
                    }
                    catch
                    {

                    }
                    return controls;
                }
                set
                {
                    try
                    {
                        ezcode.AllControls = value.ToList();
                    }
                    catch
                    {

                    }
                }
            }
        }

        class PropertyGridVars
        {
            public PropertyGridVars(EzCode ez) => ezcode = ez;
            public EzCode ezcode = new EzCode();
            [Category("Variables")]
            [Description("The vars the program contains")]
            public Var[] Variables
            {
                get
                {
                    Var[] vars = new Var[0];
                    try
                    {
                        vars = ezcode.vars.ToArray();
                    }
                    catch
                    {

                    }
                    return vars;
                }
                set
                {
                    try
                    {
                        ezcode.vars = value.ToList();
                    }
                    catch
                    {

                    }
                }
            }
        }

        class PropertyGridGroup
        {
            public PropertyGridGroup(EzCode ez) => ezcode = ez;
            public EzCode ezcode = new EzCode();
            [Category("Groups")]
            [Description("The groups the program contains")]
            public EZCode.Groups.Group[] Groups
            {
                get
                {
                    EZCode.Groups.Group[] groups = new EZCode.Groups.Group[0];
                    try
                    {
                        groups = ezcode.groups.ToArray();
                    }
                    catch
                    {

                    }
                    return groups;
                }
                set
                {
                    try
                    {
                        ezcode.groups = value.ToList();
                    }
                    catch
                    {

                    }
                }
            }
        }

        class PropertyGridBreakPoints
        {
            public PropertyGridBreakPoints(IDE ide) => this.ide = ide;
            public IDE ide = new IDE();

            [Category("Breakpoints")]
            [Description("Accessible with Ctrl+F9")]
            public Breakpoint[] Breakpoints
            {
                get
                {
                    return ide.debugSettings.Breakpoints;
                }
                set
                {
                    ide.debugSettings.Breakpoints = value;
                    ide.debugSettings.Save();
                }
            }
        }

        #endregion

        #region IDE_Main

        public TreeManager Manager;
        public ProjectSettings project;
        public Debugger Debug;
        public DebugSettings debugSettings;
        bool loaded = false;

        TextBox _FCTB_Highlight;
        public TextBox FCTB_Highlight
        {
            get
            {
                _FCTB_Highlight = new TextBox() { Text = fctb.Text };
                return _FCTB_Highlight;
            }
            set
            {
                _FCTB_Highlight = value;
                fctb.SelectionStart = _FCTB_Highlight.SelectionStart;
                fctb.SelectionLength = _FCTB_Highlight.SelectionLength;
            }
        }
        System.Windows.Forms.Timer higlight_timer = new System.Windows.Forms.Timer();

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
            project = new ProjectSettings();

            if (path == "") Manager.SetTreeNodes();
            else Manager.OpenPath(path);

            Settings.StartUp();
            fctb.Zoom = Settings.Default_Zoom;

            debugSettings = new DebugSettings();
            DebugSave.StartUp(this);

            higlight_timer.Interval = 100;
            higlight_timer.Enabled = true;
            higlight_timer.Tick += Higlight_timer_Tick;
        }

        private void IDE_Load(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = Settings.Left_Splitter_Distance;
            splitContainer2.SplitterDistance = Settings.Bottom_Splitter_Distance;

            ezcode ??= new EzCode();
            project.Initialize(ref ezcode, visualoutput, output);

            ControlsPropertyGrid.SelectedObject = new PropertyGridControls(ezcode);
            BreakpointsPropertyGridDebug.SelectedObject = new PropertyGridBreakPoints(this);
            VarPropertyGridDebug.SelectedObject = new PropertyGridVars(ezcode);
            GroupsPropertyGridDebug.SelectedObject = new PropertyGridGroup(ezcode);
            loaded = true;

            VersionTB.Text = $"IDE v{Version}";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256)
            {
                switch (keyData)
                {
                    case Keys.Control | Keys.O:
                        folderToolStripMenuItem1.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.O:
                        fileToolStripMenuItem1.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.K:
                        projectToolStripMenuItem1.PerformClick(); break;
                    case Keys.Control | Keys.S:
                        saveToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.N:
                        fileToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.K:
                        projectToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.Q:
                        exitToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Q:
                        clearTreeViewToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.T:
                        settingsPreferencesToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.P:
                        projectSettingsToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.F5:
                        playProjectToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.F6:
                        playFileToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.F4:
                        quitToolStripMenuItem.PerformClick(); break;
                    case Keys.F1:
                        docsToolStripMenuItem.PerformClick(); break;
                    case Keys.F9:
                        insertBreakpointToolStripMenuItem.PerformClick(); break;
                    case Keys.F5:
                        startDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.Alt | Keys.D:
                        startDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.F6:
                        debugFileToolStripMenuItem.PerformClick(); break;
                    case Keys.Alt | Keys.Shift | Keys.D:
                        debugFileToolStripMenuItem.PerformClick(); break;
                    case Keys.F11:
                        nextSegmentToolStripMenuItem.PerformClick(); break;
                    case Keys.F10:
                        nextbreakpointToolStripMenuItem.PerformClick(); break;
                    case Keys.F12:
                        endDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.F9:
                        allBreakpointsToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.R:
                        refreshTreeViewToolStripMenuItem.PerformClick(); break;
                    case Keys.F2:
                        renameToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.Delete:
                        deleteToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.X:
                        deleteToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.N:
                        newToolStripMenuItem1.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.A:
                        newToolStripMenuItem1.PerformClick(); break;
                    case Keys.Alt | Keys.T:
                        textToCodeToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.W:
                        playInDedicatedWindowToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.D:
                        FCTB_DuplicateLine(); break;
                }
            }
            else if (msg.Msg == 260)
            {
                switch (keyData)
                {
                    case Keys.F10:
                        fctb.Focus();
                        nextbreakpointToolStripMenuItem.PerformClick();
                        break;
                    case Keys.Alt | Keys.T:
                        textToCodeToolStripMenuItem.PerformClick(); break;
                    case Keys.Alt | Keys.P:
                        playFileToolStripMenuItem.PerformClick(); break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region events
        private void FCTB_DuplicateLine()
        {
            int currentLine = fctb.Selection.Start.iLine;
            string currentLineText = fctb.Lines[currentLine].ToString();

            int charIndex = 0;
            for (int i = 0; i < fctb.GetLine(currentLine).Start.iLine; i++)
            {
                charIndex += fctb.Lines[i].Length + 2;
            }
            fctb.Text = fctb.Text.Insert(charIndex, currentLineText + Environment.NewLine);

            fctb.Selection = new FastColoredTextBoxNS.Range(fctb, currentLineText.Length, currentLine + 1, currentLineText.Length, currentLine + 1);
        }

        public int changeTime = 0;

        string[] varListItems = new string[0];
        private void Higlight_timer_Tick(object? sender, EventArgs e)
        {
            if (Debug != null ? Debug.IsPlaying : false)
            {
                FCTB_Highlight = Debug.HighlightTextbox;
                CurrentLine.Text = Debug.CurrentLineTextbox.Text;
                currentSegment.Text = Debug.ezcode.codeLine.ToString();
                fctb.ReadOnly = true;
                string[] vars = ezcode.vars.Select(x => $"'{x.Name}' = '{x.Value}'").ToArray();
                if (!varListItems.SequenceEqual(vars))
                {
                    VarListView.Items.Clear();
                    VarListView.Items.AddRange(vars);
                    varListItems = VarListView.Items.OfType<string>().ToArray();
                }
            }
            else
            {
                fctb.ReadOnly = false;
            }
            if (dedicated_player != null && dedicated_player.isClosing)
            {
                dedicated_player.Dispose();
            }
            playInDedicatedWindowToolStripMenuItem.Checked = Settings.Play_In_Dedicated_Window;
        }
        private void IDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            // closing
            Settings.Exit(this);
        }

        private void fctb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                changeTime++;
                if (changeTime > Settings.IntelliSense_Refresh)
                {
                    // IntelliSense
                    BuildAutocompleteMenu();

                    // auto save
                    if (Settings.Auto_Save && FileURLTextBox.Text != "")
                        Manager.SaveFile();
                    changeTime = 0;
                }
            }
            catch
            {
                // nothing
            }
        }

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // exit
            Settings.Exit(this);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // save file
            try
            {
                Manager.SaveFile();
            }
            catch
            {
                MessageBox.Show("Could not save file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            // settings
            Settings_Preferences settings_Preferences = new Settings_Preferences(this, Settings_Preferences.Tab.settings);
            settings_Preferences.ShowDialog();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new file
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "EZCode|*.ezcode|EZProj|*.ezproj|Text Document|*.txt|All Files|*";
                saveFileDialog.ShowDialog();

                string filePath = saveFileDialog.FileName;

                File.Create(filePath).Close();

                FileInfo file = new FileInfo(filePath);

                try
                {
                    if (file.Directory.FullName.StartsWith(Settings.Open_Folder_Path, StringComparison.OrdinalIgnoreCase))
                    {
                        refreshTreeViewToolStripMenuItem.PerformClick();
                        Tree.SelectedNode = Tree.Nodes.Find(file.FullName, true).First();
                    }
                    else
                    {
                        Manager.OpenFile(file.FullName);
                    }
                }
                catch
                {

                }
            }
            catch
            {

            }
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // new project
            New_Project newProject = new New_Project();
            newProject.ShowDialog();

            if (newProject.DONE)
                Manager.OpenFolder(Settings.New_Project_Default_Directory);
        }

        private void projectSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // project settings
            try
            {
                if (Settings.Current_Project_File != "" && !File.Exists(Settings.Current_Project_File))
                    throw new Exception();

                if (Settings.Current_Project_File != "")
                {
                    project.ConvertFromCode(FileURLTextBox.Text == Settings.Current_Project_File ? fctb.Text : File.ReadAllText(Settings.Current_Project_File));
                    project.Directory = new FileInfo(Settings.Current_Project_File).DirectoryName;
                    Project_Settings_Form project_Settings_Form = new Project_Settings_Form(project);
                    project_Settings_Form.ShowDialog();
                    DialogResult result = MessageBox.Show("Introduce changes to project file", "Change File", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        project = project_Settings_Form.projectSettings;
                        File.WriteAllText(Settings.Current_Project_File, project.ConverToCode());
                        if (FileURLTextBox.Text == Settings.Current_Project_File)
                            fctb.Text = project.ConverToCode();
                    }
                }
                else
                {
                    MessageBox.Show("Please open a project", "No Project", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("The Project no longer exists", "No Project", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void projectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // open project
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "EZProj|*.ezproj";
                dialog.ShowDialog();
                Settings.Current_Project_File = dialog.FileName;
                Manager.OpenFolder(new FileInfo(dialog.FileName).Directory.ToString());
            }
            catch
            {
                MessageBox.Show("Could not open project", "EZ IDE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void playProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play project
            try
            {
                if (Settings.Save_On_Play && FileURLTextBox.Text != "")
                {
                    Manager.SaveFile();
                }
                Start(new FileInfo(Settings.Current_Project_File), ProjectType.Project);
            }
            catch
            {
                MessageBox.Show("Could not play project", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void playFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play file
            try
            {
                if (Settings.Save_On_Play)
                {
                    Manager.SaveFile();
                }
                Start(new FileInfo(FileURLTextBox.Text), ProjectType.Script);
            }
            catch
            {
                MessageBox.Show("Could not play script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ezcode ??= new EzCode();

            ezcode.Stop();
        }

        private void docsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // help/docs
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/JBrosDevelopment/EZCode/wiki/IDE-Docs",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void insertBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // insert breakpoint
            new Insert_Breakpoint(this, fctb.Text, fctb.SelectionStart, FileURLTextBox.Text);
        }

        private void debugProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // debug project
            try
            {
                if (Debug != null ? Debug.IsPlaying : false)
                {
                    MessageBox.Show("A debug session is already open", "Already Debugging", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (Settings.Always_Show_Highlight_Warning)
                {
                    DialogResult d = MessageBox.Show("The Highlighted text that shows when debugging a line may not always be correct. Please refer to the 'Debug' tab. Do you want to see this warning again", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.No)
                    {
                        Settings.Always_Show_Highlight_Warning = false;
                    }
                }

                if (Settings.Save_On_Play && FileURLTextBox.Text != "")
                {
                    Manager.SaveFile();
                }
                Start(new FileInfo(Settings.Current_Project_File), ProjectType.Project, true);

                Debug = new Debugger(ezcode, DebugSave.Breakpoints, FCTB_Highlight, CurrentLine);
                higlight_timer.Start();
                Debug.StartDebugSession(ezproj.Program);
            }
            catch
            {
                MessageBox.Show("Could not start debugging project", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void debugFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // debug file
            try
            {
                if (Debug != null ? Debug.IsPlaying : false)
                {
                    MessageBox.Show("A debug session is already open", "Already Debugging", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (Settings.Always_Show_Highlight_Warning)
                {
                    DialogResult d = MessageBox.Show("The Highlighted text that shows when debugging a line may not always be correct. Please refer to the 'Debug' tab. Do you want to see this warning again", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.No)
                    {
                        Settings.Always_Show_Highlight_Warning = false;
                    }
                }

                if (Settings.Save_On_Play)
                {
                    Manager.SaveFile();
                }
                Start(new FileInfo(FileURLTextBox.Text), ProjectType.Script, true);

                Debug = new Debugger(ezcode, DebugSave.Breakpoints, FCTB_Highlight);
                higlight_timer.Start();
                Debug.StartDebugSession(File.ReadAllText(FileURLTextBox.Text));
                project.Directory = Settings.Current_Project_File;
            }
            catch
            {
                MessageBox.Show("Could not start debugging file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nextSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // next segment
            try
            {
                Debug.NextSegment(Settings.Debug_Pause);
            }
            catch
            {

            }
        }

        private void nextbreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // next breakpoint
            try
            {
                Debug.NextBreakpoint();
            }
            catch
            {

            }
        }

        private void endDebugSessionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // end debug session
            try
            {
                Debug.StopDebugSession();
            }
            catch
            {

            }
        }

        private void allBreakpointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // all breakpoints
            Breakpoints_Form form = new Breakpoints_Form();
            form.ShowDialog();
            debugSettings = form.debugSettings;
        }

        private void clearTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // clear tree view
            Tree.Nodes.Clear();
            Settings.Open_Folder_Path = "";
        }

        private void refreshTreeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // refresh
            Manager.OpenFolder(Settings.Open_Folder_Path);
        }

        private void fctb_ZoomChanged(object sender, EventArgs e)
        {
            // zoom change
            Settings.Default_Zoom = fctb.Zoom;
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // tree context menu click - new

            try
            {
                string name = Interaction.InputBox("Enter the Name of the file:", "New File", ".ezcode");
                if (new FileInfo(name).Extension == "") name += ".ezcode";
                string dir = new FileInfo(Tree.SelectedNode.Name).DirectoryName;
                string path = Path.Combine(dir, name);
                int go = 1;
                while (File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    string extension = fileInfo.Extension;
                    string newName = name.Replace(extension, $" ({go}){extension}");
                    path = Path.Combine(dir, newName);
                    go++;
                    if (!File.Exists(path))
                        name = newName;
                }
                File.Create(path).Close();
                TreeNode node = new TreeNode(path) { Text = name };
                try
                {
                    refreshTreeViewToolStripMenuItem.PerformClick();
                    Tree.SelectedNode = Tree.Nodes.Find(path, true).FirstOrDefault(node);
                }
                catch
                {
                    Manager.OpenFile(path);
                }
            }
            catch
            {

            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // tree context menu click - delete

            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this file?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string path = FileURLTextBox.Text;
                        File.Delete(path);
                        Tree.Nodes.Remove(Tree.SelectedNode);
                    }
                    catch
                    {
                        MessageBox.Show("Could not delete file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {

            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // tree context menu click - rename

            try
            {
                string name = Interaction.InputBox("Enter the new Name of the file:", "Rename File", Tree.SelectedNode.Text);
                string dir = new FileInfo(Tree.SelectedNode.Name).DirectoryName;
                string path = Path.Combine(dir, name);
                int go = 1;
                while (File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    string extension = fileInfo.Extension;
                    string newName = name.Replace(extension, $" ({go}){extension}");
                    path = Path.Combine(dir, newName);
                    go++;
                    if (!File.Exists(path))
                        name = newName;
                }
                if (FileURLTextBox.Text == Tree.SelectedNode.Name && File.ReadAllText(Tree.SelectedNode.Name) != fctb.Text)
                {
                    Manager.SaveFile(true);
                }
                File.WriteAllText(path, File.ReadAllText(Tree.SelectedNode.Name));
                File.Delete(Tree.SelectedNode.Name);
                TreeNode node = new TreeNode(path) { Text = name };
                try
                {
                    refreshTreeViewToolStripMenuItem.PerformClick();
                    Tree.SelectedNode = Tree.Nodes.Find(path, true).FirstOrDefault(node);
                }
                catch
                {
                    Manager.OpenFile(path);
                }
            }
            catch
            {

            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // copy
            fctb.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // paste
            fctb.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // cut
            fctb.Cut();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // delete
            fctb.SelectedText = "";
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // undo
            fctb.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // redo
            fctb.Redo();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // bottom splitter moved
            if (loaded) Settings.Bottom_Splitter_Distance = splitContainer2.SplitterDistance;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // left splitter moved
            if (loaded) Settings.Left_Splitter_Distance = splitContainer1.SplitterDistance;
        }

        private void textToCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // text to code
            Format_Code text_to_code = new Format_Code();
            text_to_code.ShowDialog();
        }

        private void playInDedicatedWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Play_In_Dedicated_Window = playInDedicatedWindowToolStripMenuItem.Checked;
        }

        #endregion
    }
}
