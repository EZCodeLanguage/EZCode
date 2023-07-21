namespace ezcode_Project_Player
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
            Space = new Panel();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            openToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            playToolStripMenuItem = new ToolStripMenuItem();
            stopToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            showErrorsToolStripMenuItem = new ToolStripMenuItem();
            toolStripTextBox4 = new ToolStripTextBox();
            toolStripTextBox3 = new ToolStripTextBox();
            toolStripComboBox1 = new ToolStripComboBox();
            toolStripTextBox2 = new ToolStripTextBox();
            toolStripTextBox1 = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            splitContainer1 = new SplitContainer();
            Send = new Button();
            ConsoleSend = new TextBox();
            console = new RichTextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // Space
            // 
            Space.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Space.BackColor = Color.DarkGray;
            Space.Location = new Point(1, 2);
            Space.Name = "Space";
            Space.Size = new Size(1158, 439);
            Space.TabIndex = 0;
            Space.MouseClick += Space_MouseClick;
            // 
            // toolStrip1
            // 
            toolStrip1.BackColor = Color.Gray;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton3, toolStripDropDownButton2, toolStripComboBox1, toolStripTextBox2, toolStripTextBox1, toolStripLabel1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1161, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.Image = (Image)resources.GetObject("toolStripDropDownButton1.Image");
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(38, 22);
            toolStripDropDownButton1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl + O";
            openToolStripMenuItem.Size = new Size(152, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(152, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { playToolStripMenuItem, stopToolStripMenuItem });
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
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { showErrorsToolStripMenuItem, toolStripTextBox4, toolStripTextBox3 });
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(45, 22);
            toolStripDropDownButton2.Text = "Help";
            // 
            // showErrorsToolStripMenuItem
            // 
            showErrorsToolStripMenuItem.Name = "showErrorsToolStripMenuItem";
            showErrorsToolStripMenuItem.Size = new Size(180, 22);
            showErrorsToolStripMenuItem.Text = "Show Errors Toggle";
            showErrorsToolStripMenuItem.Click += showErrorsToolStripMenuItem_Click;
            // 
            // toolStripTextBox4
            // 
            toolStripTextBox4.Name = "toolStripTextBox4";
            toolStripTextBox4.ReadOnly = true;
            toolStripTextBox4.Size = new Size(100, 23);
            toolStripTextBox4.Text = "Project Player 1.1.0";
            // 
            // toolStripTextBox3
            // 
            toolStripTextBox3.Name = "toolStripTextBox3";
            toolStripTextBox3.ReadOnly = true;
            toolStripTextBox3.Size = new Size(100, 23);
            toolStripTextBox3.Text = "EzCode 1.9.12";
            // 
            // toolStripComboBox1
            // 
            toolStripComboBox1.Items.AddRange(new object[] { "Space", "Console", "Space + Console" });
            toolStripComboBox1.Name = "toolStripComboBox1";
            toolStripComboBox1.Size = new Size(121, 25);
            toolStripComboBox1.ToolTipText = "Layout";
            toolStripComboBox1.SelectedIndexChanged += toolStripComboBox1_Click;
            toolStripComboBox1.Click += toolStripComboBox1_Click;
            // 
            // toolStripTextBox2
            // 
            toolStripTextBox2.Name = "toolStripTextBox2";
            toolStripTextBox2.ReadOnly = true;
            toolStripTextBox2.Size = new Size(125, 25);
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.ReadOnly = true;
            toolStripTextBox1.Size = new Size(125, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(0, 22);
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.BackColor = Color.Maroon;
            splitContainer1.Location = new Point(0, 23);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(Space);
            splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(Send);
            splitContainer1.Panel2.Controls.Add(ConsoleSend);
            splitContainer1.Panel2.Controls.Add(console);
            splitContainer1.Panel2MinSize = 1;
            splitContainer1.Size = new Size(1161, 693);
            splitContainer1.SplitterDistance = 442;
            splitContainer1.TabIndex = 2;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.Location = new Point(1101, 221);
            Send.Name = "Send";
            Send.Size = new Size(60, 23);
            Send.TabIndex = 5;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // ConsoleSend
            // 
            ConsoleSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConsoleSend.BackColor = Color.Black;
            ConsoleSend.BorderStyle = BorderStyle.FixedSingle;
            ConsoleSend.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            ConsoleSend.ForeColor = Color.IndianRed;
            ConsoleSend.Location = new Point(-1, 222);
            ConsoleSend.Name = "ConsoleSend";
            ConsoleSend.Size = new Size(1102, 23);
            ConsoleSend.TabIndex = 4;
            ConsoleSend.KeyDown += ConsoleSend_KeyDown;
            // 
            // console
            // 
            console.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            console.BackColor = Color.Black;
            console.BorderStyle = BorderStyle.None;
            console.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            console.ForeColor = Color.Maroon;
            console.Location = new Point(0, 1);
            console.Name = "console";
            console.ReadOnly = true;
            console.Size = new Size(1161, 223);
            console.TabIndex = 3;
            console.Text = "";
            console.TextChanged += console_TextChanged;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1161, 716);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MinimumSize = new Size(399, 202);
            Name = "Form1";
            Text = "ezCode";
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel Space;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem openToolStripMenuItem;
        private SplitContainer splitContainer1;
        private RichTextBox console;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripMenuItem playToolStripMenuItem;
        private ToolStripMenuItem stopToolStripMenuItem;
        private Button Send;
        private TextBox ConsoleSend;
        private System.Windows.Forms.Timer timer1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripTextBox toolStripTextBox2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem showErrorsToolStripMenuItem;
        private ToolStripTextBox toolStripTextBox4;
        private ToolStripTextBox toolStripTextBox3;
    }
}