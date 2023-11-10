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
            folderToolStripMenuItem = new ToolStripMenuItem();
            projectToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem1 = new ToolStripMenuItem();
            folderToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            createToolStripMenuItem = new ToolStripMenuItem();
            fileToolStripMenuItem2 = new ToolStripMenuItem();
            folderToolStripMenuItem2 = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            settingsPreferencesToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            projectSettingsToolStripMenuItem = new ToolStripMenuItem();
            includeToolStripMenuItem = new ToolStripMenuItem();
            excludeToolStripMenuItem = new ToolStripMenuItem();
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
            FileURLTextBox = new ToolStripTextBox();
            splitContainer1 = new SplitContainer();
            Tree = new TreeView();
            fctb = new FastColoredTextBoxNS.FastColoredTextBox();
            splitContainer2 = new SplitContainer();
            imageList1 = new ImageList(components);
            clearTreeViewToolStripMenuItem = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)fctb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
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
            newToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileToolStripMenuItem, folderToolStripMenuItem, projectToolStripMenuItem });
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(147, 22);
            newToolStripMenuItem.Text = "New";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
            fileToolStripMenuItem.Size = new Size(194, 22);
            fileToolStripMenuItem.Text = "File";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // folderToolStripMenuItem
            // 
            folderToolStripMenuItem.Name = "folderToolStripMenuItem";
            folderToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+N";
            folderToolStripMenuItem.Size = new Size(194, 22);
            folderToolStripMenuItem.Text = "Folder";
            folderToolStripMenuItem.Click += folderToolStripMenuItem_Click;
            // 
            // projectToolStripMenuItem
            // 
            projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            projectToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+K";
            projectToolStripMenuItem.Size = new Size(194, 22);
            projectToolStripMenuItem.Text = "Project";
            projectToolStripMenuItem.Click += projectToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileToolStripMenuItem1, folderToolStripMenuItem1 });
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(147, 22);
            openToolStripMenuItem.Text = "Open";
            // 
            // fileToolStripMenuItem1
            // 
            fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            fileToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+Shift+O";
            fileToolStripMenuItem1.Size = new Size(180, 22);
            fileToolStripMenuItem1.Text = "File";
            fileToolStripMenuItem1.Click += fileToolStripMenuItem1_Click;
            // 
            // folderToolStripMenuItem1
            // 
            folderToolStripMenuItem1.Name = "folderToolStripMenuItem1";
            folderToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+O";
            folderToolStripMenuItem1.Size = new Size(180, 22);
            folderToolStripMenuItem1.Text = "Folder";
            folderToolStripMenuItem1.Click += folderToolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
            saveToolStripMenuItem.Size = new Size(147, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.ShortcutKeyDisplayString = "";
            exitToolStripMenuItem.Size = new Size(147, 22);
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
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton3, toolStripDropDownButton2, toolStripDropDownButton6, toolStripDropDownButton4, toolStripDropDownButton5, FileURLTextBox });
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
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem, deleteToolStripMenuItem, clearTreeViewToolStripMenuItem, settingsPreferencesToolStripMenuItem });
            toolStripDropDownButton3.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton3.Image = (Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton3.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton3.Size = new Size(83, 22);
            toolStripDropDownButton3.Text = "Edit";
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileToolStripMenuItem2, folderToolStripMenuItem2 });
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.Size = new Size(239, 22);
            createToolStripMenuItem.Text = "Create";
            // 
            // fileToolStripMenuItem2
            // 
            fileToolStripMenuItem2.Name = "fileToolStripMenuItem2";
            fileToolStripMenuItem2.ShortcutKeyDisplayString = "Alt+Ctrl+N";
            fileToolStripMenuItem2.Size = new Size(189, 22);
            fileToolStripMenuItem2.Text = "File";
            fileToolStripMenuItem2.Click += fileToolStripMenuItem2_Click;
            // 
            // folderToolStripMenuItem2
            // 
            folderToolStripMenuItem2.Name = "folderToolStripMenuItem2";
            folderToolStripMenuItem2.ShortcutKeyDisplayString = "Alt+Shift+N";
            folderToolStripMenuItem2.Size = new Size(189, 22);
            folderToolStripMenuItem2.Text = "Folder";
            folderToolStripMenuItem2.Click += folderToolStripMenuItem2_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Ctrl+X";
            deleteToolStripMenuItem.Size = new Size(239, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // settingsPreferencesToolStripMenuItem
            // 
            settingsPreferencesToolStripMenuItem.Name = "settingsPreferencesToolStripMenuItem";
            settingsPreferencesToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+T";
            settingsPreferencesToolStripMenuItem.Size = new Size(239, 22);
            settingsPreferencesToolStripMenuItem.Text = "Settings/Preferences";
            settingsPreferencesToolStripMenuItem.Click += settingsPreferencesToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.BackColor = Color.FromArgb(64, 64, 64);
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { projectSettingsToolStripMenuItem, includeToolStripMenuItem, excludeToolStripMenuItem });
            toolStripDropDownButton2.ForeColor = SystemColors.ButtonFace;
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Margin = new Padding(0, 1, 10, 2);
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Padding = new Padding(20, 10, 20, 10);
            toolStripDropDownButton2.Size = new Size(101, 22);
            toolStripDropDownButton2.Text = "Project";
            // 
            // projectSettingsToolStripMenuItem
            // 
            projectSettingsToolStripMenuItem.Name = "projectSettingsToolStripMenuItem";
            projectSettingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+P";
            projectSettingsToolStripMenuItem.Size = new Size(244, 22);
            projectSettingsToolStripMenuItem.Text = "Project Settings";
            projectSettingsToolStripMenuItem.Click += projectSettingsToolStripMenuItem_Click;
            // 
            // includeToolStripMenuItem
            // 
            includeToolStripMenuItem.Name = "includeToolStripMenuItem";
            includeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+I";
            includeToolStripMenuItem.Size = new Size(244, 22);
            includeToolStripMenuItem.Text = "Include";
            includeToolStripMenuItem.Click += includeToolStripMenuItem_Click;
            // 
            // excludeToolStripMenuItem
            // 
            excludeToolStripMenuItem.Name = "excludeToolStripMenuItem";
            excludeToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+E";
            excludeToolStripMenuItem.Size = new Size(244, 22);
            excludeToolStripMenuItem.Text = "Exclude";
            excludeToolStripMenuItem.Click += excludeToolStripMenuItem_Click;
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
            playProjectToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+P";
            playProjectToolStripMenuItem.Size = new Size(187, 22);
            playProjectToolStripMenuItem.Text = "Play Project";
            playProjectToolStripMenuItem.Click += playProjectToolStripMenuItem_Click;
            // 
            // playFileToolStripMenuItem
            // 
            playFileToolStripMenuItem.Name = "playFileToolStripMenuItem";
            playFileToolStripMenuItem.ShortcutKeyDisplayString = "Alt+P";
            playFileToolStripMenuItem.Size = new Size(187, 22);
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
            insertBreakpointToolStripMenuItem.Size = new Size(245, 22);
            insertBreakpointToolStripMenuItem.Text = "Insert Breakpoint";
            insertBreakpointToolStripMenuItem.Click += insertBreakpointToolStripMenuItem_Click;
            // 
            // startDebugSessionToolStripMenuItem
            // 
            startDebugSessionToolStripMenuItem.Name = "startDebugSessionToolStripMenuItem";
            startDebugSessionToolStripMenuItem.ShortcutKeyDisplayString = "Alt+D";
            startDebugSessionToolStripMenuItem.Size = new Size(245, 22);
            startDebugSessionToolStripMenuItem.Text = "Start Debug Session";
            startDebugSessionToolStripMenuItem.Click += startDebugSessionToolStripMenuItem_Click;
            // 
            // nextSegmentToolStripMenuItem
            // 
            nextSegmentToolStripMenuItem.Name = "nextSegmentToolStripMenuItem";
            nextSegmentToolStripMenuItem.ShortcutKeyDisplayString = "F11";
            nextSegmentToolStripMenuItem.Size = new Size(245, 22);
            nextSegmentToolStripMenuItem.Text = "Next Segment";
            nextSegmentToolStripMenuItem.Click += nextSegmentToolStripMenuItem_Click;
            // 
            // continueToolStripMenuItem
            // 
            continueToolStripMenuItem.Name = "continueToolStripMenuItem";
            continueToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            continueToolStripMenuItem.Size = new Size(245, 22);
            continueToolStripMenuItem.Text = "Next Breakpoint";
            continueToolStripMenuItem.Click += continueToolStripMenuItem_Click;
            // 
            // endDebugSessionToolStripMenuItem
            // 
            endDebugSessionToolStripMenuItem.Name = "endDebugSessionToolStripMenuItem";
            endDebugSessionToolStripMenuItem.ShortcutKeyDisplayString = "F12";
            endDebugSessionToolStripMenuItem.Size = new Size(245, 22);
            endDebugSessionToolStripMenuItem.Text = "End Debug Session";
            endDebugSessionToolStripMenuItem.Click += endDebugSessionToolStripMenuItem_Click;
            // 
            // debugSettingsToolStripMenuItem
            // 
            debugSettingsToolStripMenuItem.Name = "debugSettingsToolStripMenuItem";
            debugSettingsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+D";
            debugSettingsToolStripMenuItem.Size = new Size(245, 22);
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
            FileURLTextBox.AutoSize = false;
            FileURLTextBox.BackColor = Color.FromArgb(80, 80, 80);
            FileURLTextBox.BorderStyle = BorderStyle.FixedSingle;
            FileURLTextBox.ForeColor = Color.White;
            FileURLTextBox.Name = "FileURLTextBox";
            FileURLTextBox.ReadOnly = true;
            FileURLTextBox.Size = new Size(400, 23);
            FileURLTextBox.ToolTipText = "File URL of open file";
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
            splitContainer1.SplitterDistance = 282;
            splitContainer1.TabIndex = 6;
            // 
            // Tree
            // 
            Tree.BackColor = SystemColors.WindowFrame;
            Tree.Dock = DockStyle.Fill;
            Tree.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Tree.ForeColor = SystemColors.Info;
            Tree.ItemHeight = 24;
            Tree.Location = new Point(0, 0);
            Tree.Name = "Tree";
            Tree.Size = new Size(282, 461);
            Tree.TabIndex = 0;
            Tree.BeforeSelect += Tree_BeforeSelect;
            Tree.AfterSelect += Tree_AfterSelect;
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
            fctb.Size = new Size(800, 461);
            fctb.TabIndex = 1;
            fctb.Zoom = 100;
            fctb.TextChanged += fctb_TextChanged;
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
            // clearTreeViewToolStripMenuItem
            // 
            clearTreeViewToolStripMenuItem.Name = "clearTreeViewToolStripMenuItem";
            clearTreeViewToolStripMenuItem.Size = new Size(239, 22);
            clearTreeViewToolStripMenuItem.Text = "Clear Tree View";
            clearTreeViewToolStripMenuItem.Click += clearTreeViewToolStripMenuItem_Click;
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
            Text = "Player";
            WindowState = FormWindowState.Maximized;
            FormClosed += Player_FormClosed;
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
            ((System.ComponentModel.ISupportInitialize)fctb).EndInit();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
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
        private ToolStripDropDownButton toolStripDropDownButton2;
        private SplitContainer splitContainer1;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripDropDownButton toolStripDropDownButton4;
        private ToolStripDropDownButton toolStripDropDownButton5;
        private SplitContainer splitContainer2;
        private ImageList imageList1;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem projectToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem folderToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem folderToolStripMenuItem1;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripMenuItem fileToolStripMenuItem2;
        private ToolStripMenuItem folderToolStripMenuItem2;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem settingsPreferencesToolStripMenuItem;
        private ToolStripMenuItem projectSettingsToolStripMenuItem;
        private ToolStripMenuItem includeToolStripMenuItem;
        private ToolStripMenuItem excludeToolStripMenuItem;
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
        public ToolStripTextBox FileURLTextBox;
        private ToolStripMenuItem clearTreeViewToolStripMenuItem;
    }
}