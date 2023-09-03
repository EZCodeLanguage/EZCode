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
            SuspendLayout();
            // 
            // InputText
            // 
            InputText.AcceptsReturn = true;
            InputText.AcceptsTab = true;
            InputText.AllowDrop = true;
            InputText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            InputText.BackColor = Color.WhiteSmoke;
            InputText.ForeColor = SystemColors.ControlDarkDark;
            InputText.Location = new Point(11, 38);
            InputText.Multiline = true;
            InputText.Name = "InputText";
            InputText.ScrollBars = ScrollBars.Both;
            InputText.Size = new Size(422, 264);
            InputText.TabIndex = 0;
            InputText.WordWrap = false;
            InputText.KeyPress += InputText_KeyPress;
            // 
            // Console
            // 
            Console.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Console.Location = new Point(11, 334);
            Console.Name = "Console";
            Console.ReadOnly = true;
            Console.Size = new Size(866, 146);
            Console.TabIndex = 2;
            Console.Text = "";
            Console.TextChanged += Console_TextChanged;
            // 
            // Start
            // 
            Start.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Start.Location = new Point(11, 308);
            Start.Name = "Start";
            Start.Size = new Size(763, 23);
            Start.TabIndex = 3;
            Start.Text = "Start";
            Start.UseVisualStyleBackColor = true;
            Start.Click += Start_Click;
            // 
            // ConsoleInput
            // 
            ConsoleInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ConsoleInput.Location = new Point(11, 486);
            ConsoleInput.Name = "ConsoleInput";
            ConsoleInput.Size = new Size(664, 23);
            ConsoleInput.TabIndex = 4;
            ConsoleInput.KeyDown += ConsoleInput_KeyDown;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.Location = new Point(681, 485);
            Send.Name = "Send";
            Send.Size = new Size(95, 23);
            Send.TabIndex = 5;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = true;
            Send.Click += Send_Click;
            // 
            // Quit
            // 
            Quit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Quit.Location = new Point(780, 308);
            Quit.Name = "Quit";
            Quit.Size = new Size(95, 23);
            Quit.TabIndex = 6;
            Quit.Text = "Quit";
            Quit.UseVisualStyleBackColor = true;
            Quit.Click += Quit_Click;
            // 
            // Clear
            // 
            Clear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Clear.Location = new Point(782, 485);
            Clear.Name = "Clear";
            Clear.Size = new Size(95, 23);
            Clear.TabIndex = 7;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = true;
            Clear.Click += Clear_Click;
            // 
            // OutputPanel
            // 
            OutputPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            OutputPanel.BackColor = SystemColors.ButtonFace;
            OutputPanel.Location = new Point(439, 38);
            OutputPanel.Name = "OutputPanel";
            OutputPanel.Size = new Size(436, 264);
            OutputPanel.TabIndex = 8;
            // 
            // directory
            // 
            directory.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            directory.Location = new Point(11, 9);
            directory.Name = "directory";
            directory.Size = new Size(866, 23);
            directory.TabIndex = 9;
            // 
            // EZCode_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(884, 517);
            Controls.Add(directory);
            Controls.Add(OutputPanel);
            Controls.Add(Clear);
            Controls.Add(Quit);
            Controls.Add(Send);
            Controls.Add(ConsoleInput);
            Controls.Add(Start);
            Controls.Add(Console);
            Controls.Add(InputText);
            KeyPreview = true;
            MinimumSize = new Size(617, 405);
            Name = "EZCode_Form";
            Text = "EZCode-Form";
            FormClosing += EZCode_Form_FormClosing;
            KeyDown += EZCode_Form_KeyDown;
            KeyUp += EZCode_Form_KeyUp;
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
    }
}