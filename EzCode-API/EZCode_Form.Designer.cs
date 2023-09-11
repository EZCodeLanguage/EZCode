namespace EzCode_API
{
    partial class EZCode_Form
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
            InputText = new TextBox();
            Console = new RichTextBox();
            Start = new Button();
            ConsoleInput = new TextBox();
            Send = new Button();
            Quit = new Button();
            Clear = new Button();
            OutputPanel = new Panel();
            directory = new TextBox();
            checkBox1 = new CheckBox();
            checkBox2 = new CheckBox();
            checkBox3 = new CheckBox();
            version = new TextBox();
            checkBox4 = new CheckBox();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // InputText
            // 
            InputText.AcceptsReturn = true;
            InputText.AcceptsTab = true;
            InputText.AllowDrop = true;
            InputText.BackColor = Color.WhiteSmoke;
            InputText.Dock = DockStyle.Fill;
            InputText.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            InputText.ForeColor = SystemColors.ControlDarkDark;
            InputText.Location = new Point(0, 0);
            InputText.Multiline = true;
            InputText.Name = "InputText";
            InputText.ScrollBars = ScrollBars.Both;
            InputText.Size = new Size(505, 302);
            InputText.TabIndex = 0;
            InputText.WordWrap = false;
            InputText.KeyPress += InputText_KeyPress;
            // 
            // Console
            // 
            Console.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Console.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Console.Location = new Point(5, 31);
            Console.Name = "Console";
            Console.ReadOnly = true;
            Console.Size = new Size(916, 112);
            Console.TabIndex = 2;
            Console.Text = "";
            Console.TextChanged += Console_TextChanged;
            // 
            // Start
            // 
            Start.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Start.Location = new Point(524, 2);
            Start.Name = "Start";
            Start.Size = new Size(291, 23);
            Start.TabIndex = 3;
            Start.Text = "Start";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // ConsoleInput
            // 
            ConsoleInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConsoleInput.Location = new Point(3, 149);
            ConsoleInput.Name = "ConsoleInput";
            ConsoleInput.Size = new Size(716, 23);
            ConsoleInput.TabIndex = 4;
            ConsoleInput.KeyDown += ConsoleInput_KeyDown;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.Location = new Point(725, 148);
            Send.Name = "Send";
            Send.Size = new Size(100, 23);
            Send.TabIndex = 5;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // Quit
            // 
            Quit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Quit.Location = new Point(821, 2);
            Quit.Name = "Quit";
            Quit.Size = new Size(98, 23);
            Quit.TabIndex = 6;
            Quit.Text = "Quit";
            Quit.UseVisualStyleBackColor = true;
            Quit.Click += Quit_Click;
            // 
            // Clear
            // 
            Clear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Clear.Location = new Point(831, 148);
            Clear.Name = "Clear";
            Clear.Size = new Size(90, 23);
            Clear.TabIndex = 7;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // OutputPanel
            // 
            OutputPanel.BackColor = SystemColors.ButtonFace;
            OutputPanel.Dock = DockStyle.Fill;
            OutputPanel.Location = new Point(0, 0);
            OutputPanel.Name = "OutputPanel";
            OutputPanel.Size = new Size(415, 302);
            OutputPanel.TabIndex = 8;
            // 
            // directory
            // 
            directory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            directory.Location = new Point(11, 9);
            directory.Name = "directory";
            directory.Size = new Size(924, 23);
            directory.TabIndex = 9;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(3, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(128, 19);
            checkBox1.TabIndex = 10;
            checkBox1.Text = "Show Start and End";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Checked = true;
            checkBox2.CheckState = CheckState.Checked;
            checkBox2.Location = new Point(137, 4);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(122, 19);
            checkBox2.TabIndex = 11;
            checkBox2.Text = "Show File in Errors";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Checked = true;
            checkBox3.CheckState = CheckState.Checked;
            checkBox3.Location = new Point(265, 3);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(99, 19);
            checkBox3.TabIndex = 12;
            checkBox3.Text = "Clear Console";
            checkBox3.UseVisualStyleBackColor = true;
            // 
            // version
            // 
            version.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            version.Location = new Point(439, 2);
            version.Name = "version";
            version.ReadOnly = true;
            version.Size = new Size(79, 23);
            version.TabIndex = 13;
            version.Text = "EZCode 0.0.0";
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Checked = true;
            checkBox4.CheckState = CheckState.Checked;
            checkBox4.Location = new Point(370, 3);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(68, 19);
            checkBox4.TabIndex = 14;
            checkBox4.Text = "In Panel";
            checkBox4.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(InputText);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(OutputPanel);
            splitContainer1.Size = new Size(924, 302);
            splitContainer1.SplitterDistance = 505;
            splitContainer1.TabIndex = 15;
            // 
            // splitContainer2
            // 
            splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer2.Location = new Point(11, 38);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(splitContainer1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(Start);
            splitContainer2.Panel2.Controls.Add(checkBox4);
            splitContainer2.Panel2.Controls.Add(Clear);
            splitContainer2.Panel2.Controls.Add(Quit);
            splitContainer2.Panel2.Controls.Add(Send);
            splitContainer2.Panel2.Controls.Add(version);
            splitContainer2.Panel2.Controls.Add(ConsoleInput);
            splitContainer2.Panel2.Controls.Add(checkBox1);
            splitContainer2.Panel2.Controls.Add(Console);
            splitContainer2.Panel2.Controls.Add(checkBox3);
            splitContainer2.Panel2.Controls.Add(checkBox2);
            splitContainer2.Size = new Size(924, 481);
            splitContainer2.SplitterDistance = 302;
            splitContainer2.TabIndex = 0;
            // 
            // EZCode_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(942, 524);
            Controls.Add(splitContainer2);
            Controls.Add(directory);
            KeyPreview = true;
            MinimumSize = new Size(873, 450);
            Name = "EZCode_Form";
            Text = "EZCode-Testing-Form ";
            FormClosing += EZCode_Form_FormClosing;
            KeyDown += EZCode_Form_KeyDown;
            KeyUp += EZCode_Form_KeyUp;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputText;
        private RichTextBox Console;
        private Button Start;
        private TextBox ConsoleInput;
        private Button Send;
        private Button Quit;
        private Button Clear;
        private Panel OutputPanel;
        private TextBox directory;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private TextBox version;
        private CheckBox checkBox4;
        private SplitContainer splitContainer1;
        private SplitContainer splitContainer2;
    }
}