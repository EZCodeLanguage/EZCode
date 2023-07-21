namespace ezCode
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            splitContainer1 = new SplitContainer();
            txt = new FastColoredTextBoxNS.FastColoredTextBox();
            Space = new Panel();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            toolStripMenuItem3 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripDropDownButton();
            toolStripMenuItem6 = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripMenuItem();
            addFolderToListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripMenuItem();
            clearListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripMenuItem();
            openProjectListToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox4 = new ToolStripTextBox();
            clearProjectListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripMenuItem();
            toolStripDropDownButton5 = new ToolStripDropDownButton();
            debuggerEnabledToolStripMenuItem = new ToolStripMenuItem();
            quitDebuggerToolStripMenuItem = new ToolStripMenuItem();
            nextLineToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            playToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            playInViewerToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton4 = new ToolStripDropDownButton();
            toolStripMenuItem10 = new ToolStripMenuItem();
            viewDocsToolStripMenuItem = new ToolStripMenuItem();
            viewAllHotKeysToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox3 = new ToolStripTextBox();
            toolStripTextBox2 = new ToolStripTextBox();
            toolStripTextBox1 = new ToolStripTextBox();
            splitContainer2 = new SplitContainer();
            progressBar1 = new ProgressBar();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            console = new RichTextBox();
            clear = new Button();
            ConsoleSend = new TextBox();
            Send = new Button();
            tabPage2 = new TabPage();
            splitContainer4 = new SplitContainer();
            label1 = new Label();
            listBox2 = new ListBox();
            Console2 = new RichTextBox();
            CurrentLineTxt = new TextBox();
            button2 = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            yscale2 = new TextBox();
            xscale2 = new TextBox();
            ypos2 = new TextBox();
            xpos2 = new TextBox();
            yscale1 = new TextBox();
            xscale1 = new TextBox();
            ypos1 = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            label3 = new Label();
            xpos1 = new TextBox();
            label2 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            splitContainer3 = new SplitContainer();
            button3 = new Button();
            listBox1 = new ListBox();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txt).BeginInit();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).BeginInit();
            splitContainer4.Panel1.SuspendLayout();
            splitContainer4.Panel2.SuspendLayout();
            splitContainer4.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = Color.Indigo;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = SystemColors.WindowFrame;
            splitContainer1.Panel1.Controls.Add(txt);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(Space);
            splitContainer1.Size = new Size(738, 342);
            splitContainer1.SplitterDistance = 325;
            splitContainer1.TabIndex = 0;
            // 
            // txt
            // 
            txt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txt.AutoCompleteBrackets = true;
            txt.AutoCompleteBracketsList = new char[] { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };
            txt.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            txt.AutoScrollMinSize = new Size(2, 18);
            txt.BackBrush = null;
            txt.BackColor = Color.FromArgb(40, 40, 40);
            txt.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            txt.CaretColor = Color.Silver;
            txt.CharHeight = 18;
            txt.CharWidth = 9;
            txt.CurrentLineColor = SystemColors.GrayText;
            txt.DescriptionFile = "C:\\Users\\jlham\\source\\repos\\ezCode\\ezcode.xml";
            txt.DisabledColor = Color.FromArgb(100, 180, 180, 180);
            txt.FoldingIndicatorColor = SystemColors.Highlight;
            txt.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point);
            txt.ForeColor = Color.LightGray;
            txt.HighlightFoldingIndicator = false;
            txt.Hotkeys = resources.GetString("txt.Hotkeys");
            txt.IndentBackColor = Color.Indigo;
            txt.IsReplaceMode = false;
            txt.LeftBracket = '(';
            txt.LeftBracket2 = '{';
            txt.LineNumberColor = SystemColors.ActiveCaption;
            txt.Location = new Point(-3, 0);
            txt.Name = "txt";
            txt.Paddings = new Padding(0);
            txt.RightBracket = ')';
            txt.RightBracket2 = '}';
            txt.SelectionColor = Color.FromArgb(210, 100, 120, 255);
            txt.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject("txt.ServiceColors");
            txt.Size = new Size(325, 342);
            txt.TabIndex = 0;
            txt.Zoom = 100;
            txt.KeyPressed += txt_KeyPress;
            txt.KeyDown += txt_KeyDown;
            // 
            // Space
            // 
            Space.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Space.BackColor = Color.DarkGray;
            Space.Location = new Point(-1, 3);
            Space.Name = "Space";
            Space.Size = new Size(403, 336);
            Space.TabIndex = 0;
            Space.MouseClick += Space_MouseClick;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.Gray;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(32, 32);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, toolStripMenuItem4, toolStripDropDownButton5, toolStripDropDownButton3, toolStripDropDownButton4, toolStripTextBox1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new Padding(0, 0, 2, 0);
            toolStrip1.Size = new Size(980, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem3, saveToolStripMenuItem, openToolStripMenuItem, newToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.ShortcutKeyDisplayString = "Ctrl + D";
            toolStripMenuItem3.Size = new Size(197, 22);
            toolStripMenuItem3.Text = "Autosave: Off ";
            toolStripMenuItem3.Click += toolStripMenuItem3_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + S";
            saveToolStripMenuItem.Size = new Size(197, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + O";
            openToolStripMenuItem.Size = new Size(197, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + N";
            newToolStripMenuItem.Size = new Size(197, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(197, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2 });
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(40, 22);
            toolStripDropDownButton2.Text = "Edit";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl + Z";
            toolStripMenuItem1.Size = new Size(150, 22);
            toolStripMenuItem1.Text = "Undo";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.ShortcutKeyDisplayString = "Ctrl + Y";
            toolStripMenuItem2.Size = new Size(150, 22);
            toolStripMenuItem2.Text = "Redo";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem6, toolStripMenuItem7, addFolderToListToolStripMenuItem, toolStripMenuItem9, clearListToolStripMenuItem, toolStripMenuItem5, toolStripMenuItem8 });
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(57, 22);
            toolStripMenuItem4.Text = "Project";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(190, 22);
            toolStripMenuItem6.Text = "Open Project List";
            toolStripMenuItem6.Click += openProjectListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(190, 22);
            toolStripMenuItem7.Text = "Save Project List";
            toolStripMenuItem7.Click += saveProjectListToolStripMenuItem_Click;
            // 
            // addFolderToListToolStripMenuItem
            // 
            addFolderToListToolStripMenuItem.Name = "addFolderToListToolStripMenuItem";
            addFolderToListToolStripMenuItem.Size = new Size(190, 22);
            addFolderToListToolStripMenuItem.Text = "Add Folder to List";
            addFolderToListToolStripMenuItem.Click += addFolderToListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(190, 22);
            toolStripMenuItem9.Text = "Remove File From List";
            toolStripMenuItem9.Click += removeFileFromListToolStripMenuItem_Click;
            // 
            // clearListToolStripMenuItem
            // 
            clearListToolStripMenuItem.Name = "clearListToolStripMenuItem";
            clearListToolStripMenuItem.Size = new Size(190, 22);
            clearListToolStripMenuItem.Text = "Clear List";
            clearListToolStripMenuItem.Click += clearListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[] { openProjectListToolStripMenuItem, toolStripTextBox4, clearProjectListToolStripMenuItem });
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(190, 22);
            toolStripMenuItem5.Text = "Auto Open Project";
            // 
            // openProjectListToolStripMenuItem
            // 
            openProjectListToolStripMenuItem.Name = "openProjectListToolStripMenuItem";
            openProjectListToolStripMenuItem.Size = new Size(164, 22);
            openProjectListToolStripMenuItem.Text = "Open Project List";
            openProjectListToolStripMenuItem.Click += openProjectListToolStripMenuItem_Click_1;
            // 
            // toolStripTextBox4
            // 
            toolStripTextBox4.Name = "toolStripTextBox4";
            toolStripTextBox4.Size = new Size(100, 23);
            toolStripTextBox4.TextChanged += toolStripTextBox4_TextChanged;
            // 
            // clearProjectListToolStripMenuItem
            // 
            clearProjectListToolStripMenuItem.Name = "clearProjectListToolStripMenuItem";
            clearProjectListToolStripMenuItem.Size = new Size(164, 22);
            clearProjectListToolStripMenuItem.Text = "Clear Project List";
            clearProjectListToolStripMenuItem.Click += clearProjectListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(190, 22);
            toolStripMenuItem8.Text = "Package";
            toolStripMenuItem8.Click += packageToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton5
            // 
            toolStripDropDownButton5.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton5.DropDownItems.AddRange(new ToolStripItem[] { debuggerEnabledToolStripMenuItem, quitDebuggerToolStripMenuItem, nextLineToolStripMenuItem });
            toolStripDropDownButton5.Image = (Image)resources.GetObject("toolStripDropDownButton5.Image");
            toolStripDropDownButton5.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton5.Name = "toolStripDropDownButton5";
            toolStripDropDownButton5.Size = new Size(55, 22);
            toolStripDropDownButton5.Text = "Debug";
            // 
            // debuggerEnabledToolStripMenuItem
            // 
            debuggerEnabledToolStripMenuItem.Name = "debuggerEnabledToolStripMenuItem";
            debuggerEnabledToolStripMenuItem.ShortcutKeyDisplayString = "F10";
            debuggerEnabledToolStripMenuItem.Size = new Size(177, 22);
            debuggerEnabledToolStripMenuItem.Text = "Debug";
            debuggerEnabledToolStripMenuItem.Click += debuggerEnabledToolStripMenuItem_Click;
            // 
            // quitDebuggerToolStripMenuItem
            // 
            quitDebuggerToolStripMenuItem.Name = "quitDebuggerToolStripMenuItem";
            quitDebuggerToolStripMenuItem.ShortcutKeyDisplayString = "F12";
            quitDebuggerToolStripMenuItem.Size = new Size(177, 22);
            quitDebuggerToolStripMenuItem.Text = "Quit Debugger";
            quitDebuggerToolStripMenuItem.Click += quitDebuggerToolStripMenuItem_Click;
            // 
            // nextLineToolStripMenuItem
            // 
            nextLineToolStripMenuItem.Name = "nextLineToolStripMenuItem";
            nextLineToolStripMenuItem.ShortcutKeyDisplayString = "F11";
            nextLineToolStripMenuItem.Size = new Size(177, 22);
            nextLineToolStripMenuItem.Text = "Next Line";
            nextLineToolStripMenuItem.Click += nextLineToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { playToolStripMenuItem, stopToolStripMenuItem, playInViewerToolStripMenuItem });
            toolStripDropDownButton3.Image = (Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Size = new Size(47, 22);
            toolStripDropDownButton3.Text = "Build";
            // 
            // playToolStripMenuItem
            // 
            playToolStripMenuItem.Name = "playToolStripMenuItem";
            playToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + P";
            playToolStripMenuItem.Size = new Size(147, 22);
            playToolStripMenuItem.Text = "Play";
            playToolStripMenuItem.Click += toolStripButton1_Click;
            // 
            // stopToolStripMenuItem
            // 
            stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            stopToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + Q";
            stopToolStripMenuItem.Size = new Size(147, 22);
            stopToolStripMenuItem.Text = "Stop";
            stopToolStripMenuItem.Click += stopToolStripMenuItem_Click;
            // 
            // playInViewerToolStripMenuItem
            // 
            playInViewerToolStripMenuItem.Name = "playInViewerToolStripMenuItem";
            playInViewerToolStripMenuItem.Size = new Size(147, 22);
            playInViewerToolStripMenuItem.Text = "Play in Viewer";
            playInViewerToolStripMenuItem.Click += playInViewerToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton4
            // 
            toolStripDropDownButton4.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton4.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem10, viewDocsToolStripMenuItem, viewAllHotKeysToolStripMenuItem, toolStripTextBox3, toolStripTextBox2 });
            toolStripDropDownButton4.Image = (Image)resources.GetObject("toolStripDropDownButton4.Image");
            toolStripDropDownButton4.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            toolStripDropDownButton4.Size = new Size(45, 22);
            toolStripDropDownButton4.Text = "Help";
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new Size(160, 22);
            toolStripMenuItem10.Text = "View Docs";
            toolStripMenuItem10.Click += viewDocsToolStripMenuItem_Click;
            // 
            // viewDocsToolStripMenuItem
            // 
            viewDocsToolStripMenuItem.Name = "viewDocsToolStripMenuItem";
            viewDocsToolStripMenuItem.Size = new Size(160, 22);
            viewDocsToolStripMenuItem.Text = "View Tutorials";
            viewDocsToolStripMenuItem.Click += viewDocsToolStripMenuItem_Click_1;
            // 
            // viewAllHotKeysToolStripMenuItem
            // 
            viewAllHotKeysToolStripMenuItem.Name = "viewAllHotKeysToolStripMenuItem";
            viewAllHotKeysToolStripMenuItem.Size = new Size(160, 22);
            viewAllHotKeysToolStripMenuItem.Text = "View HotKeys";
            viewAllHotKeysToolStripMenuItem.Click += viewAllHotKeysToolStripMenuItem_Click;
            // 
            // toolStripTextBox3
            // 
            toolStripTextBox3.Name = "toolStripTextBox3";
            toolStripTextBox3.ReadOnly = true;
            toolStripTextBox3.Size = new Size(100, 23);
            toolStripTextBox3.Text = "Editor 1.1.4";
            // 
            // toolStripTextBox2
            // 
            toolStripTextBox2.Name = "toolStripTextBox2";
            toolStripTextBox2.ReadOnly = true;
            toolStripTextBox2.Size = new Size(100, 23);
            toolStripTextBox2.Text = "EzCode 1.9.12";
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.BackColor = Color.Gray;
            toolStripTextBox1.BorderStyle = BorderStyle.None;
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.ReadOnly = true;
            toolStripTextBox1.Size = new Size(400, 25);
            // 
            // splitContainer2
            // 
            splitContainer2.BackColor = Color.Indigo;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(progressBar1);
            splitContainer2.Panel2.Controls.Add(tabControl1);
            splitContainer2.Size = new Size(738, 536);
            splitContainer2.SplitterDistance = 342;
            splitContainer2.TabIndex = 2;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(3, 175);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(735, 13);
            progressBar1.Step = 1;
            progressBar1.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(3, -2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(735, 175);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.DimGray;
            tabPage1.Controls.Add(console);
            tabPage1.Controls.Add(clear);
            tabPage1.Controls.Add(ConsoleSend);
            tabPage1.Controls.Add(Send);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(727, 147);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Console";
            // 
            // console
            // 
            console.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            console.BackColor = Color.Black;
            console.BorderStyle = BorderStyle.None;
            console.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            console.ForeColor = Color.DarkViolet;
            console.Location = new Point(3, 0);
            console.Name = "console";
            console.ReadOnly = true;
            console.Size = new Size(735, 125);
            console.TabIndex = 3;
            console.Text = "";
            console.TextChanged += console_TextChanged;
            // 
            // clear
            // 
            clear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            clear.Location = new Point(674, 129);
            clear.Name = "clear";
            clear.Size = new Size(60, 23);
            clear.TabIndex = 6;
            clear.Text = "Clear";
            clear.UseVisualStyleBackColor = true;
            clear.Click += clear_Click;
            // 
            // ConsoleSend
            // 
            ConsoleSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConsoleSend.BackColor = Color.Black;
            ConsoleSend.BorderStyle = BorderStyle.FixedSingle;
            ConsoleSend.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ConsoleSend.ForeColor = Color.MediumOrchid;
            ConsoleSend.Location = new Point(2, 126);
            ConsoleSend.Name = "ConsoleSend";
            ConsoleSend.Size = new Size(612, 23);
            ConsoleSend.TabIndex = 4;
            ConsoleSend.KeyDown += ConsoleSend_KeyDown;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.Location = new Point(615, 129);
            Send.Name = "Send";
            Send.Size = new Size(60, 23);
            Send.TabIndex = 5;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.LightGray;
            tabPage2.Controls.Add(splitContainer4);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(727, 147);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Debugger";
            // 
            // splitContainer4
            // 
            splitContainer4.Dock = DockStyle.Fill;
            splitContainer4.Location = new Point(3, 3);
            splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            splitContainer4.Panel1.Controls.Add(label1);
            splitContainer4.Panel1.Controls.Add(listBox2);
            // 
            // splitContainer4.Panel2
            // 
            splitContainer4.Panel2.Controls.Add(Console2);
            splitContainer4.Panel2.Controls.Add(CurrentLineTxt);
            splitContainer4.Panel2.Controls.Add(button2);
            splitContainer4.Panel2.Controls.Add(tableLayoutPanel1);
            splitContainer4.Panel2.Controls.Add(label2);
            splitContainer4.Size = new Size(721, 141);
            splitContainer4.SplitterDistance = 426;
            splitContainer4.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(3, 2);
            label1.Name = "label1";
            label1.Size = new Size(420, 23);
            label1.TabIndex = 1;
            label1.Text = "Variables";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // listBox2
            // 
            listBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox2.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            listBox2.FormattingEnabled = true;
            listBox2.HorizontalScrollbar = true;
            listBox2.ItemHeight = 17;
            listBox2.Location = new Point(0, 35);
            listBox2.MultiColumn = true;
            listBox2.Name = "listBox2";
            listBox2.Size = new Size(423, 106);
            listBox2.TabIndex = 0;
            // 
            // Console2
            // 
            Console2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Console2.BackColor = SystemColors.MenuText;
            Console2.BorderStyle = BorderStyle.None;
            Console2.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            Console2.ForeColor = Color.DarkViolet;
            Console2.Location = new Point(4, 57);
            Console2.Name = "Console2";
            Console2.ReadOnly = true;
            Console2.Size = new Size(284, 52);
            Console2.TabIndex = 7;
            Console2.Text = "";
            // 
            // CurrentLineTxt
            // 
            CurrentLineTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CurrentLineTxt.Location = new Point(4, 28);
            CurrentLineTxt.Multiline = true;
            CurrentLineTxt.Name = "CurrentLineTxt";
            CurrentLineTxt.ReadOnly = true;
            CurrentLineTxt.Size = new Size(284, 23);
            CurrentLineTxt.TabIndex = 0;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new Point(3, 115);
            button2.Name = "button2";
            button2.Size = new Size(285, 23);
            button2.TabIndex = 6;
            button2.Text = "Next Line (F11)";
            button2.UseVisualStyleBackColor = true;
            button2.Click += nextLineToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.BackgroundImageLayout = ImageLayout.None;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetPartial;
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.60674F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.32584F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.5618F));
            tableLayoutPanel1.Controls.Add(yscale2, 4, 2);
            tableLayoutPanel1.Controls.Add(xscale2, 3, 2);
            tableLayoutPanel1.Controls.Add(ypos2, 2, 2);
            tableLayoutPanel1.Controls.Add(xpos2, 1, 2);
            tableLayoutPanel1.Controls.Add(yscale1, 4, 1);
            tableLayoutPanel1.Controls.Add(xscale1, 3, 1);
            tableLayoutPanel1.Controls.Add(ypos1, 2, 1);
            tableLayoutPanel1.Controls.Add(label7, 4, 0);
            tableLayoutPanel1.Controls.Add(label6, 3, 0);
            tableLayoutPanel1.Controls.Add(label5, 2, 0);
            tableLayoutPanel1.Controls.Add(label4, 1, 0);
            tableLayoutPanel1.Controls.Add(comboBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(comboBox2, 0, 2);
            tableLayoutPanel1.Controls.Add(label3, 0, 0);
            tableLayoutPanel1.Controls.Add(xpos1, 1, 1);
            tableLayoutPanel1.Location = new Point(5000, 5000);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 27.36842F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 37.89474F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(272, 63);
            tableLayoutPanel1.TabIndex = 3;
            tableLayoutPanel1.Visible = false;
            // 
            // yscale2
            // 
            yscale2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            yscale2.Location = new Point(205, 45);
            yscale2.Name = "yscale2";
            yscale2.ReadOnly = true;
            yscale2.Size = new Size(61, 23);
            yscale2.TabIndex = 14;
            // 
            // xscale2
            // 
            xscale2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            xscale2.Location = new Point(139, 45);
            xscale2.Name = "xscale2";
            xscale2.ReadOnly = true;
            xscale2.Size = new Size(57, 23);
            xscale2.TabIndex = 13;
            // 
            // ypos2
            // 
            ypos2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ypos2.Location = new Point(100, 45);
            ypos2.Name = "ypos2";
            ypos2.ReadOnly = true;
            ypos2.Size = new Size(30, 23);
            ypos2.TabIndex = 12;
            // 
            // xpos2
            // 
            xpos2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            xpos2.Location = new Point(60, 45);
            xpos2.Name = "xpos2";
            xpos2.ReadOnly = true;
            xpos2.Size = new Size(31, 23);
            xpos2.TabIndex = 11;
            // 
            // yscale1
            // 
            yscale1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            yscale1.Location = new Point(205, 23);
            yscale1.Name = "yscale1";
            yscale1.ReadOnly = true;
            yscale1.Size = new Size(61, 23);
            yscale1.TabIndex = 10;
            // 
            // xscale1
            // 
            xscale1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            xscale1.Location = new Point(139, 23);
            xscale1.Name = "xscale1";
            xscale1.ReadOnly = true;
            xscale1.Size = new Size(57, 23);
            xscale1.TabIndex = 9;
            // 
            // ypos1
            // 
            ypos1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ypos1.Location = new Point(100, 23);
            ypos1.Name = "ypos1";
            ypos1.ReadOnly = true;
            ypos1.Size = new Size(30, 23);
            ypos1.TabIndex = 8;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label7.Location = new Point(205, 3);
            label7.Name = "label7";
            label7.Size = new Size(61, 14);
            label7.TabIndex = 6;
            label7.Text = "Y Scale / Font";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.Location = new Point(139, 3);
            label6.Name = "label6";
            label6.Size = new Size(57, 14);
            label6.TabIndex = 5;
            label6.Text = "X Scale / Text";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label5.Location = new Point(100, 3);
            label5.Name = "label5";
            label5.Size = new Size(30, 14);
            label5.TabIndex = 4;
            label5.Text = "Y Pos";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.Location = new Point(60, 3);
            label4.Name = "label4";
            label4.Size = new Size(31, 14);
            label4.TabIndex = 3;
            label4.Text = "X Pos";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(6, 23);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(45, 23);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(6, 45);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(45, 23);
            comboBox2.TabIndex = 1;
            comboBox2.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.Location = new Point(6, 3);
            label3.Name = "label3";
            label3.Size = new Size(45, 14);
            label3.TabIndex = 2;
            label3.Text = "Name";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // xpos1
            // 
            xpos1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            xpos1.Location = new Point(60, 23);
            xpos1.Name = "xpos1";
            xpos1.ReadOnly = true;
            xpos1.Size = new Size(31, 23);
            xpos1.TabIndex = 7;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label2.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(3, 2);
            label2.Name = "label2";
            label2.Size = new Size(275, 23);
            label2.TabIndex = 2;
            label2.Text = "Other Values";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // splitContainer3
            // 
            splitContainer3.BackColor = SystemColors.ControlDarkDark;
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 25);
            splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.BackColor = Color.Indigo;
            splitContainer3.Panel1.Controls.Add(button3);
            splitContainer3.Panel1.Controls.Add(listBox1);
            splitContainer3.Panel1.Controls.Add(button1);
            splitContainer3.Panel1MinSize = 20;
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(splitContainer2);
            splitContainer3.Panel2MinSize = 500;
            splitContainer3.Size = new Size(980, 536);
            splitContainer3.SplitterDistance = 238;
            splitContainer3.TabIndex = 1;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button3.BackColor = SystemColors.Info;
            button3.Location = new Point(4, 34);
            button3.Name = "button3";
            button3.Size = new Size(231, 23);
            button3.TabIndex = 2;
            button3.Text = "New File";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.BackColor = SystemColors.WindowFrame;
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            listBox1.ForeColor = Color.AliceBlue;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 21;
            listBox1.Location = new Point(3, 62);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(231, 462);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button1.BackColor = SystemColors.Info;
            button1.Location = new Point(4, 5);
            button1.Name = "button1";
            button1.Size = new Size(231, 23);
            button1.TabIndex = 0;
            button1.Text = "Add Files";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlDark;
            ClientSize = new Size(980, 561);
            Controls.Add(splitContainer3);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new Size(641, 449);
            Name = "Form1";
            Text = "ezCode";
            WindowState = FormWindowState.Maximized;
            FormClosing += Form1_FormClosing;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txt).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            splitContainer4.Panel1.ResumeLayout(false);
            splitContainer4.Panel2.ResumeLayout(false);
            splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer4).EndInit();
            splitContainer4.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel Space;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer splitContainer2;
        private RichTextBox console;
        private FastColoredTextBoxNS.FastColoredTextBox txt;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private Button Send;
        private TextBox ConsoleSend;
        private Button clear;
        private ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
        private ToolStripTextBox toolStripTextBox1;
        private SplitContainer splitContainer3;
        private ListBox listBox1;
        private Button button1;
        private ToolStripMenuItem playInViewerToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton4;
        private ToolStripMenuItem viewDocsToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton5;
        private ToolStripMenuItem debuggerEnabledToolStripMenuItem;
        private ToolStripMenuItem quitDebuggerToolStripMenuItem;
        private ToolStripMenuItem nextLineToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SplitContainer splitContainer4;
        private Label label1;
        private ListBox listBox2;
        private Label label2;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox CurrentLineTxt;
        private Button button2;
        private TextBox yscale2;
        private TextBox xscale2;
        private TextBox ypos2;
        private TextBox xpos2;
        private TextBox yscale1;
        private TextBox xscale1;
        private TextBox ypos1;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private Label label3;
        private TextBox xpos1;
        private ToolStripTextBox toolStripTextBox3;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem viewAllHotKeysToolStripMenuItem;
        private ToolStripDropDownButton toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem openProjectListToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox4;
        private ToolStripMenuItem clearProjectListToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem addFolderToListToolStripMenuItem;
        private ToolStripMenuItem clearListToolStripMenuItem;
        private Button button3;
        private RichTextBox Console2;
    }
}