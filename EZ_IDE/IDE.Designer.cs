namespace EZ_IDE
{
    partial class IDE
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDE));
            visualoutput = new Panel();
            Send = new Button();
            Clear = new Button();
            input = new TextBox();
            output = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            DebugList = new ListBox();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            newToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem = new ToolStripMenuItem();
            projectToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem1 = new ToolStripMenuItem();
            folderToolStripMenuItem1 = new ToolStripMenuItem();
            projectToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            editToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem1 = new ToolStripMenuItem();
            cutToolStripMenuItem = new ToolStripMenuItem();
            pastToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem2 = new ToolStripMenuItem();
            undoToolStripMenuItem1 = new ToolStripMenuItem();
            redoToolStripMenuItem1 = new ToolStripMenuItem();
            clearTreeViewToolStripMenuItem = new ToolStripMenuItem();
            refreshTreeViewToolStripMenuItem = new ToolStripMenuItem();
            projectSettingsToolStripMenuItem = new ToolStripMenuItem();
            settingsPreferencesToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton6 = new ToolStripDropDownButton();
            playProjectToolStripMenuItem = new ToolStripMenuItem();
            playFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton4 = new ToolStripDropDownButton();
            insertBreakpointToolStripMenuItem = new ToolStripMenuItem();
            startDebugSessionToolStripMenuItem = new ToolStripMenuItem();
            nextSegmentToolStripMenuItem = new ToolStripMenuItem();
            continueToolStripMenuItem = new ToolStripMenuItem();
            endDebugSessionToolStripMenuItem = new ToolStripMenuItem();
            debugSettingsToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton5 = new ToolStripDropDownButton();
            docsToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox2 = new ToolStripTextBox();
            FileURLTextBox = new ToolStripLabel();
            splitContainer1 = new SplitContainer();
            Tree = new TreeView();
            treeMenuSelect = new ContextMenuStrip(components);
            newToolStripMenuItem1 = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            renameToolStripMenuItem = new ToolStripMenuItem();
            fctb = new FastColoredTextBoxNS.FastColoredTextBox();
            splitContainer2 = new SplitContainer();
            imageList1 = new ImageList(components);
            Menu_Fctb = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            cutToolStripMenuItem1 = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem1 = new ToolStripMenuItem();
            undoToolStripMenuItem = new ToolStripMenuItem();
            redoToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            treeMenuSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fctb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            Menu_Fctb.SuspendLayout();
            SuspendLayout();
            // 
            // visualoutput
            // 
            visualoutput.BackColor = Color.Black;
            visualoutput.Dock = DockStyle.Fill;
            visualoutput.Location = new Point(3, 3);
            visualoutput.Name = "visualoutput";
            visualoutput.Size = new Size(1072, 153);
            visualoutput.TabIndex = 0;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.BackColor = Color.FromArgb(25, 25, 25);
            Send.FlatAppearance.BorderColor = Color.Gray;
            Send.FlatAppearance.BorderSize = 0;
            Send.FlatStyle = FlatStyle.Flat;
            Send.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Send.ForeColor = Color.FromArgb(235, 235, 235);
            Send.Location = new Point(915, 136);
            Send.Name = "Send";
            Send.Size = new Size(75, 23);
            Send.TabIndex = 3;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = false;
            Send.Click += Send_Click;
            // 
            // Clear
            // 
            Clear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Clear.BackColor = Color.FromArgb(25, 25, 25);
            Clear.FlatAppearance.BorderColor = Color.Gray;
            Clear.FlatAppearance.BorderSize = 0;
            Clear.FlatStyle = FlatStyle.Flat;
            Clear.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Clear.ForeColor = Color.FromArgb(235, 235, 235);
            Clear.Location = new Point(996, 136);
            Clear.Name = "Clear";
            Clear.Size = new Size(76, 23);
            Clear.TabIndex = 2;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = false;
            Clear.Click += Clear_Click;
            // 
            // input
            // 
            input.AcceptsReturn = true;
            input.AcceptsTab = true;
            input.AllowDrop = true;
            input.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            input.BackColor = Color.FromArgb(15, 15, 15);
            input.BorderStyle = BorderStyle.None;
            input.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point);
            input.ForeColor = Color.FromArgb(235, 235, 235);
            input.Location = new Point(3, 138);
            input.Name = "input";
            input.Size = new Size(907, 19);
            input.TabIndex = 1;
            input.KeyDown += input_KeyDown;
            // 
            // output
            // 
            output.AcceptsTab = true;
            output.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            output.AutoWordSelection = true;
            output.BackColor = Color.FromArgb(15, 15, 15);
            output.BorderStyle = BorderStyle.None;
            output.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point);
            output.ForeColor = Color.FromArgb(235, 235, 235);
            output.HideSelection = false;
            output.Location = new Point(3, 3);
            output.Name = "output";
            output.ReadOnly = true;
            output.Size = new Size(1076, 129);
            output.TabIndex = 0;
            output.Text = "";
            output.WordWrap = false;
            output.TextChanged += output_TextChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ItemSize = new Size(50, 20);
            tabControl1.Location = new Point(0, 0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1086, 187);
            tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Controls.Add(output);
            tabPage1.Controls.Add(input);
            tabPage1.Controls.Add(Send);
            tabPage1.Controls.Add(Clear);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1078, 159);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Console";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.Black;
            tabPage2.Controls.Add(visualoutput);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1078, 159);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Visual Output";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = Color.Black;
            tabPage3.Controls.Add(DebugList);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(1078, 159);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Debug";
            // 
            // DebugList
            // 
            DebugList.BackColor = SystemColors.WindowFrame;
            DebugList.Dock = DockStyle.Left;
            DebugList.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            DebugList.ForeColor = SystemColors.Info;
            DebugList.FormattingEnabled = true;
            DebugList.ItemHeight = 17;
            DebugList.Location = new Point(3, 3);
            DebugList.Name = "DebugList";
            DebugList.Size = new Size(539, 153);
            DebugList.TabIndex = 0;
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Margin = new Padding(5, 1, 10, 2);
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton1.Size = new Size(80, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileToolStripMenuItem, projectToolStripMenuItem });
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(177, 22);
            newToolStripMenuItem.Text = "New";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            fileToolStripMenuItem.Size = new Size(161, 22);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // projectToolStripMenuItem
            // 
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+K";
            projectToolStripMenuItem.Size = new Size(161, 22);
            projectToolStripMenuItem.Text = "Project";
            projectToolStripMenuItem.Click += projectToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileToolStripMenuItem1, folderToolStripMenuItem1, projectToolStripMenuItem1 });
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(177, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // fileToolStripMenuItem1
            // 
            fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            fileToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Shift+O";
            fileToolStripMenuItem1.Size = new Size(195, 22);
            fileToolStripMenuItem1.Text = "File";
            fileToolStripMenuItem1.Click += fileToolStripMenuItem1_Click;
            // 
            // folderToolStripMenuItem1
            // 
            folderToolStripMenuItem1.Name = "folderToolStripMenuItem1";
            folderToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+O";
            folderToolStripMenuItem1.Size = new Size(195, 22);
            folderToolStripMenuItem1.Text = "Folder";
            folderToolStripMenuItem1.Click += folderToolStripMenuItem1_Click;
            // 
            // projectToolStripMenuItem1
            // 
            projectToolStripMenuItem1.Name = "projectToolStripMenuItem1";
            projectToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Shift+K";
            projectToolStripMenuItem1.Size = new Size(195, 22);
            projectToolStripMenuItem1.Text = "Project";
            projectToolStripMenuItem1.Click += projectToolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            saveToolStripMenuItem.Size = new Size(177, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+Q";
            exitToolStripMenuItem.Size = new Size(177, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.BackColor = Color.Transparent;
            toolStrip1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton3, toolStripDropDownButton6, toolStripDropDownButton4, toolStripDropDownButton5, FileURLTextBox });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Margin = new Padding(10);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0);
            toolStrip1.RenderMode = ToolStripRenderMode.System;
            toolStrip1.ShowItemToolTips = false;
            toolStrip1.Size = new Size(1086, 25);
            toolStrip1.Stretch = true;
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { editToolStripMenuItem, clearTreeViewToolStripMenuItem, refreshTreeViewToolStripMenuItem, projectSettingsToolStripMenuItem, settingsPreferencesToolStripMenuItem });
            toolStripDropDownButton3.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton3.Image = (Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton3.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton3.Size = new Size(83, 22);
            toolStripDropDownButton3.Text = "Edit";
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyToolStripMenuItem1, cutToolStripMenuItem, pastToolStripMenuItem, deleteToolStripMenuItem2, undoToolStripMenuItem1, redoToolStripMenuItem1 });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(244, 22);
            editToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem1
            // 
            copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            copyToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+C";
            copyToolStripMenuItem1.Size = new Size(152, 22);
            copyToolStripMenuItem1.Text = "Copy";
            copyToolStripMenuItem1.Click += copyToolStripMenuItem_Click;
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
            cutToolStripMenuItem.Size = new Size(152, 22);
            cutToolStripMenuItem.Text = "Cut";
            cutToolStripMenuItem.Click += cutToolStripMenuItem_Click;
            // 
            // pastToolStripMenuItem
            // 
            pastToolStripMenuItem.Name = "pastToolStripMenuItem";
            pastToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            pastToolStripMenuItem.Size = new Size(152, 22);
            pastToolStripMenuItem.Text = "Paste";
            pastToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem2
            // 
            deleteToolStripMenuItem2.Name = "deleteToolStripMenuItem2";
            deleteToolStripMenuItem2.ShortcutKeyDisplayString = "Del";
            deleteToolStripMenuItem2.Size = new Size(152, 22);
            deleteToolStripMenuItem2.Text = "Delete";
            deleteToolStripMenuItem2.Click += deleteToolStripMenuItem1_Click;
            // 
            // undoToolStripMenuItem1
            // 
            undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
            undoToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Z";
            undoToolStripMenuItem1.Size = new Size(152, 22);
            undoToolStripMenuItem1.Text = "Undo";
            undoToolStripMenuItem1.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem1
            // 
            redoToolStripMenuItem1.Name = "redoToolStripMenuItem1";
            redoToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Y";
            redoToolStripMenuItem1.Size = new Size(152, 22);
            redoToolStripMenuItem1.Text = "Redo";
            redoToolStripMenuItem1.Click += redoToolStripMenuItem_Click;
            // 
            // clearTreeViewToolStripMenuItem
            // 
            clearTreeViewToolStripMenuItem.Name = "clearTreeViewToolStripMenuItem";
            clearTreeViewToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Q";
            clearTreeViewToolStripMenuItem.Size = new Size(244, 22);
            clearTreeViewToolStripMenuItem.Text = "Clear Tree View";
            clearTreeViewToolStripMenuItem.Click += clearTreeViewToolStripMenuItem_Click;
            // 
            // refreshTreeViewToolStripMenuItem
            // 
            refreshTreeViewToolStripMenuItem.Name = "refreshTreeViewToolStripMenuItem";
            refreshTreeViewToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+R";
            refreshTreeViewToolStripMenuItem.Size = new Size(244, 22);
            refreshTreeViewToolStripMenuItem.Text = "Refresh Tree View";
            refreshTreeViewToolStripMenuItem.Click += refreshTreeViewToolStripMenuItem_Click;
            // 
            // projectSettingsToolStripMenuItem
            // 
            projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            projectSettingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+P";
            projectSettingsToolStripMenuItem.Size = new Size(244, 22);
            projectSettingsToolStripMenuItem.Text = "Project Settings";
            projectSettingsToolStripMenuItem.Click += projectSettingsToolStripMenuItem_Click;
            // 
            // settingsPreferencesToolStripMenuItem
            // 
            settingsPreferencesToolStripMenuItem.Name = "settingsPreferencesToolStripMenuItem";
            settingsPreferencesToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+T";
            settingsPreferencesToolStripMenuItem.Size = new Size(244, 22);
            settingsPreferencesToolStripMenuItem.Text = "Settings/Preferences";
            settingsPreferencesToolStripMenuItem.Click += settingsPreferencesToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton6
            // 
            toolStripDropDownButton6.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton6.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton6.DropDownItems.AddRange(new ToolStripItem[] { playProjectToolStripMenuItem, playFileToolStripMenuItem });
            toolStripDropDownButton6.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton6.Image = (Image)resources.GetObject("toolStripDropDownButton6.Image");
            toolStripDropDownButton6.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton6.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton6.Name = "toolStripDropDownButton6";
            toolStripDropDownButton6.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton6.Size = new Size(84, 22);
            toolStripDropDownButton6.Text = "Play";
            // 
            // playProjectToolStripMenuItem
            // 
            playProjectToolStripMenuItem.Name = "playProjectToolStripMenuItem";
            playProjectToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+P or Ctrl+F5";
            playProjectToolStripMenuItem.Size = new Size(250, 22);
            playProjectToolStripMenuItem.Text = "Play Project";
            playProjectToolStripMenuItem.Click += playProjectToolStripMenuItem_Click;
            // 
            // playFileToolStripMenuItem
            // 
            playFileToolStripMenuItem.Name = "playFileToolStripMenuItem";
            playFileToolStripMenuItem.ShortcutKeyDisplayString = "Alt+P";
            playFileToolStripMenuItem.Size = new Size(250, 22);
            playFileToolStripMenuItem.Text = "Play File";
            playFileToolStripMenuItem.Click += playFileToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton4
            // 
            toolStripDropDownButton4.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton4.DropDownItems.AddRange(new ToolStripItem[] { insertBreakpointToolStripMenuItem, startDebugSessionToolStripMenuItem, nextSegmentToolStripMenuItem, continueToolStripMenuItem, endDebugSessionToolStripMenuItem, debugSettingsToolStripMenuItem });
            toolStripDropDownButton4.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton4.Image = (Image)resources.GetObject("toolStripDropDownButton4.Image");
            toolStripDropDownButton4.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton4.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            toolStripDropDownButton4.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton4.Size = new Size(100, 22);
            toolStripDropDownButton4.Text = "Debug";
            // 
            // insertBreakpointToolStripMenuItem
            // 
            insertBreakpointToolStripMenuItem.Name = "insertBreakpointToolStripMenuItem";
            insertBreakpointToolStripMenuItem.ShortcutKeyDisplayString = "F9";
            insertBreakpointToolStripMenuItem.Size = new Size(269, 22);
            insertBreakpointToolStripMenuItem.Text = "Insert Breakpoint";
            insertBreakpointToolStripMenuItem.Click += insertBreakpointToolStripMenuItem_Click;
            // 
            // startDebugSessionToolStripMenuItem
            // 
            startDebugSessionToolStripMenuItem.Name = "startDebugSessionToolStripMenuItem";
            startDebugSessionToolStripMenuItem.ShortcutKeyDisplayString = "F5 or Alt+D";
            startDebugSessionToolStripMenuItem.Size = new Size(269, 22);
            startDebugSessionToolStripMenuItem.Text = "Start Debug Session";
            startDebugSessionToolStripMenuItem.Click += startDebugSessionToolStripMenuItem_Click;
            // 
            // nextSegmentToolStripMenuItem
            // 
            nextSegmentToolStripMenuItem.Name = "nextSegmentToolStripMenuItem";
            nextSegmentToolStripMenuItem.ShortcutKeyDisplayString = "F11";
            nextSegmentToolStripMenuItem.Size = new Size(269, 22);
            nextSegmentToolStripMenuItem.Text = "Next Segment";
            nextSegmentToolStripMenuItem.Click += nextSegmentToolStripMenuItem_Click;
            // 
            // continueToolStripMenuItem
            // 
            continueToolStripMenuItem.Name = "continueToolStripMenuItem";
            continueToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            continueToolStripMenuItem.Size = new Size(269, 22);
            continueToolStripMenuItem.Text = "Next Breakpoint";
            continueToolStripMenuItem.Click += continueToolStripMenuItem_Click;
            // 
            // endDebugSessionToolStripMenuItem
            // 
            endDebugSessionToolStripMenuItem.Name = "endDebugSessionToolStripMenuItem";
            endDebugSessionToolStripMenuItem.ShortcutKeyDisplayString = "F12";
            endDebugSessionToolStripMenuItem.Size = new Size(269, 22);
            endDebugSessionToolStripMenuItem.Text = "End Debug Session";
            endDebugSessionToolStripMenuItem.Click += endDebugSessionToolStripMenuItem_Click;
            // 
            // debugSettingsToolStripMenuItem
            // 
            debugSettingsToolStripMenuItem.Name = "debugSettingsToolStripMenuItem";
            debugSettingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+D";
            debugSettingsToolStripMenuItem.Size = new Size(269, 22);
            debugSettingsToolStripMenuItem.Text = "Debug Settings";
            debugSettingsToolStripMenuItem.Click += debugSettingsToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton5
            // 
            toolStripDropDownButton5.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton5.DropDownItems.AddRange(new ToolStripItem[] { docsToolStripMenuItem, toolStripTextBox2 });
            toolStripDropDownButton5.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton5.Image = (Image)resources.GetObject("toolStripDropDownButton5.Image");
            toolStripDropDownButton5.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton5.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton5.Name = "toolStripDropDownButton5";
            toolStripDropDownButton5.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton5.Size = new Size(88, 22);
            toolStripDropDownButton5.Text = "Help";
            // 
            // docsToolStripMenuItem
            // 
            docsToolStripMenuItem.Name = "docsToolStripMenuItem";
            docsToolStripMenuItem.ShortcutKeyDisplayString = "F1";
            docsToolStripMenuItem.Size = new Size(160, 22);
            docsToolStripMenuItem.Text = "Docs";
            docsToolStripMenuItem.Click += docsToolStripMenuItem_Click;
            // 
            // toolStripTextBox2
            // 
            toolStripTextBox2.Name = "toolStripTextBox2";
            toolStripTextBox2.ReadOnly = true;
            toolStripTextBox2.Size = new Size(100, 23);
            toolStripTextBox2.Text = "IDE v1.0.0";
            // 
            // FileURLTextBox
            // 
            FileURLTextBox.ForeColor = SystemColors.ControlLight;
            FileURLTextBox.Name = "FileURLTextBox";
            FileURLTextBox.Size = new Size(0, 22);
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Tree);
            splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(fctb);
            splitContainer1.Panel2MinSize = 50;
            splitContainer1.Size = new Size(1086, 461);
            splitContainer1.SplitterDistance = 226;
            splitContainer1.TabIndex = 6;
            // 
            // Tree
            // 
            Tree.BackColor = SystemColors.WindowFrame;
            Tree.ContextMenuStrip = treeMenuSelect;
            Tree.Dock = DockStyle.Fill;
            Tree.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Tree.ForeColor = SystemColors.Info;
            Tree.ItemHeight = 24;
            Tree.Location = new Point(0, 0);
            Tree.Name = "Tree";
            Tree.Size = new Size(226, 461);
            Tree.TabIndex = 0;
            Tree.BeforeSelect += Tree_BeforeSelect;
            Tree.AfterSelect += Tree_AfterSelect;
            // 
            // treeMenuSelect
            // 
            treeMenuSelect.Items.AddRange(new ToolStripItem[] { newToolStripMenuItem1, deleteToolStripMenuItem, renameToolStripMenuItem });
            treeMenuSelect.Name = "contextMenuStrip1";
            treeMenuSelect.Size = new Size(223, 70);
            // 
            // newToolStripMenuItem1
            // 
            newToolStripMenuItem1.Name = "newToolStripMenuItem1";
            newToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Shift+(N or A)";
            newToolStripMenuItem1.Size = new Size(222, 22);
            newToolStripMenuItem1.Text = "New";
            newToolStripMenuItem1.Click += newToolStripMenuItem1_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+(Del or X)";
            deleteToolStripMenuItem.Size = new Size(222, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeyDisplayString = "F2";
            renameToolStripMenuItem.Size = new Size(222, 22);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // fctb
            // 
            fctb.AllowMacroRecording = false;
            fctb.AutoCompleteBrackets = true;
            fctb.AutoCompleteBracketsList = new char[] { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };
            fctb.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            fctb.AutoScrollMinSize = new Size(2, 31);
            fctb.BackBrush = null;
            fctb.BackColor = Color.FromArgb(30, 30, 30);
            fctb.CharHeight = 31;
            fctb.CharWidth = 16;
            fctb.ContextMenuStrip = Menu_Fctb;
            fctb.CurrentLineColor = Color.FromArgb(60, 60, 60, 40);
            fctb.DescriptionFile = "../EZCode_Syntax.xml";
            fctb.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            fctb.Dock = DockStyle.Fill;
            fctb.Font = new Font("Cascadia Code", 20.3860855F, FontStyle.Regular, GraphicsUnit.Point);
            fctb.ForeColor = Color.FromArgb(250, 250, 250);
            fctb.HighlightingRangeType = FastColoredTextBoxNS.HighlightingRangeType.VisibleRange;
            fctb.IsReplaceMode = false;
            fctb.LeftBracket = '(';
            fctb.LeftBracket2 = '{';
            fctb.Location = new Point(0, 0);
            fctb.Name = "fctb";
            fctb.Paddings = new Padding(0);
            fctb.RightBracket = ')';
            fctb.RightBracket2 = '}';
            fctb.SelectionColor = Color.FromArgb(90, 110, 110, 255);
            fctb.ServiceColors = null;
            fctb.ShowLineNumbers = false;
            fctb.Size = new Size(856, 461);
            fctb.TabIndex = 1;
            fctb.Zoom = 100;
            fctb.TextChanged += fctb_TextChanged;
            fctb.ZoomChanged += fctb_ZoomChanged;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 25);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(tabControl1);
            splitContainer2.Panel2MinSize = 0;
            splitContainer2.Size = new Size(1086, 652);
            splitContainer2.SplitterDistance = 461;
            splitContainer2.TabIndex = 0;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "script_16x16.png");
            imageList1.Images.SetKeyName(1, "app_16x16.png");
            imageList1.Images.SetKeyName(2, "1302166543_virtualbox.png");
            // 
            // Menu_Fctb
            // 
            Menu_Fctb.Items.AddRange(new ToolStripItem[] { copyToolStripMenuItem, cutToolStripMenuItem1, pasteToolStripMenuItem, deleteToolStripMenuItem1, undoToolStripMenuItem, redoToolStripMenuItem });
            Menu_Fctb.Name = "contextMenuStrip1";
            Menu_Fctb.Size = new Size(145, 136);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
            copyToolStripMenuItem.Size = new Size(144, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // cutToolStripMenuItem1
            // 
            cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            cutToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+X";
            cutToolStripMenuItem1.Size = new Size(144, 22);
            cutToolStripMenuItem1.Text = "Cut";
            cutToolStripMenuItem1.Click += cutToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
            pasteToolStripMenuItem.Size = new Size(144, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += pasteToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem1
            // 
            deleteToolStripMenuItem1.Name = "deleteToolStripMenuItem1";
            deleteToolStripMenuItem1.ShortcutKeyDisplayString = "Del";
            deleteToolStripMenuItem1.Size = new Size(144, 22);
            deleteToolStripMenuItem1.Text = "Delete";
            deleteToolStripMenuItem1.Click += deleteToolStripMenuItem1_Click;
            // 
            // undoToolStripMenuItem
            // 
            undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            undoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Z";
            undoToolStripMenuItem.Size = new Size(144, 22);
            undoToolStripMenuItem.Text = "Undo";
            undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
            // 
            // redoToolStripMenuItem
            // 
            redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            redoToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Y";
            redoToolStripMenuItem.Size = new Size(144, 22);
            redoToolStripMenuItem.Text = "Redo";
            redoToolStripMenuItem.Click += redoToolStripMenuItem_Click;
            // 
            // IDE
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(10, 10, 10);
            ClientSize = new Size(1086, 677);
            Controls.Add(splitContainer2);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(890, 499);
            Name = "IDE";
            Text = "EZCode IDE";
            WindowState = FormWindowState.Maximized;
            FormClosing += IDE_FormClosing;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            treeMenuSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)fctb).EndInit();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            Menu_Fctb.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private RichTextBox output;
        private Button Clear;
        private TextBox input;
        private Button Send;
        private Panel visualoutput;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private ListBox DebugList;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStrip toolStrip1;
        private SplitContainer splitContainer1;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripDropDownButton toolStripDropDownButton4;
        private ToolStripDropDownButton toolStripDropDownButton5;
        private SplitContainer splitContainer2;
        private ImageList imageList1;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem folderToolStripMenuItem1;
        private ToolStripMenuItem settingsPreferencesToolStripMenuItem;
        private ToolStripMenuItem insertBreakpointToolStripMenuItem;
        private ToolStripMenuItem startDebugSessionToolStripMenuItem;
        private ToolStripMenuItem nextSegmentToolStripMenuItem;
        private ToolStripMenuItem continueToolStripMenuItem;
        private ToolStripMenuItem endDebugSessionToolStripMenuItem;
        private ToolStripMenuItem debugSettingsToolStripMenuItem;
        private ToolStripMenuItem docsToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripDropDownButton toolStripDropDownButton6;
        private ToolStripMenuItem playProjectToolStripMenuItem;
        private ToolStripMenuItem playFileToolStripMenuItem;
        public TreeView Tree;
        public FastColoredTextBoxNS.FastColoredTextBox fctb;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem clearTreeViewToolStripMenuItem;
        private ToolStripMenuItem refreshTreeViewToolStripMenuItem;
        private ToolStripMenuItem projectSettingsToolStripMenuItem;
        private ToolStripMenuItem projectToolStripMenuItem1;
        public ToolStripLabel FileURLTextBox;
        private ContextMenuStrip treeMenuSelect;
        private ToolStripMenuItem newToolStripMenuItem1;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
        private ContextMenuStrip Menu_Fctb;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem1;
        private ToolStripMenuItem undoToolStripMenuItem;
        private ToolStripMenuItem redoToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem1;
        private ToolStripMenuItem pastToolStripMenuItem;
        private ToolStripMenuItem undoToolStripMenuItem1;
        private ToolStripMenuItem redoToolStripMenuItem1;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem2;
        private ToolStripMenuItem cutToolStripMenuItem1;
    }
}