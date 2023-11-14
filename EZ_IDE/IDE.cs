using EZCode;
using FastColoredTextBoxNS;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

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
        EZProj ezproj;
        ProjectType projectType;
        public enum ProjectType
        {
            Project,
            Script,
            None
        }
        public void Start(FileInfo _file, ProjectType _projectType = ProjectType.None)
        {
            project.Initialize(ref ezcode, visualoutput, output);
            int d = 0;
            bool window = false;
            ezproj = new EZProj(_file, _file.FullName);
            projectType = _projectType;
            file = _file;
            if (_projectType == ProjectType.Project)
            {
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
                if (ezproj.Debug)
                {
                    d = 2;
                }
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

            Play();
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
        #endregion

        TreeManager Manager;
        ProjectSettings project;

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
                    case Keys.Control | Keys.P:
                        playProjectToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.F5:
                        playProjectToolStripMenuItem.PerformClick(); break;
                    case Keys.Alt | Keys.P:
                        playFileToolStripMenuItem.PerformClick(); break;
                    case Keys.F1:
                        docsToolStripMenuItem.PerformClick(); break;
                    case Keys.F9:
                        insertBreakpointToolStripMenuItem.PerformClick(); break;
                    case Keys.F5:
                        startDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.Alt | Keys.D:
                        startDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.F11:
                        nextSegmentToolStripMenuItem.PerformClick(); break;
                    case Keys.F10:
                        continueToolStripMenuItem.PerformClick(); break;
                    case Keys.F12:
                        endDebugSessionToolStripMenuItem.PerformClick(); break;
                    case Keys.Control | Keys.Shift | Keys.D:
                        debugSettingsToolStripMenuItem.PerformClick(); break;
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
                }
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

        private void IDE_FormClosing(object sender, FormClosingEventArgs e)
        {
            // closing
            Settings.Exit(this);
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
            // settings
            Settings_Preferences settings_Preferences = new Settings_Preferences(this, Settings_Preferences.Tab.settings);
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
            NewProject newProject = new NewProject();
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
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EZProj|*.ezproj";
            dialog.ShowDialog();
            Settings.Current_Project_File = dialog.FileName;
            Manager.OpenFolder(new FileInfo(dialog.FileName).Directory.ToString());
        }

        private void playProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // play project
            try
            {
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
                Start(new FileInfo(FileURLTextBox.Text), ProjectType.Script);
            }
            catch
            {
                MessageBox.Show("Could not play script", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            Settings_Preferences settings_Preferences = new Settings_Preferences(this, Settings_Preferences.Tab.debug);
            settings_Preferences.ShowDialog();
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

        #endregion
    }
}
