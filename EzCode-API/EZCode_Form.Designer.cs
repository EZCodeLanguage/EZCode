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
            this.InputText = new System.Windows.Forms.TextBox();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.Start = new System.Windows.Forms.Button();
            this.ConsoleInput = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.Quit = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.OutputPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // InputText
            // 
            this.InputText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.InputText.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.InputText.Location = new System.Drawing.Point(12, 12);
            this.InputText.Multiline = true;
            this.InputText.Name = "InputText";
            this.InputText.Size = new System.Drawing.Size(350, 250);
            this.InputText.TabIndex = 0;
            // 
            // Console
            // 
            this.Console.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Console.Location = new System.Drawing.Point(12, 294);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.Size = new System.Drawing.Size(765, 146);
            this.Console.TabIndex = 2;
            this.Console.Text = "";
            this.Console.TextChanged += new System.EventHandler(this.Console_TextChanged);
            // 
            // Start
            // 
            this.Start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Start.Location = new System.Drawing.Point(12, 268);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(662, 23);
            this.Start.TabIndex = 3;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // ConsoleInput
            // 
            this.ConsoleInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleInput.Location = new System.Drawing.Point(12, 446);
            this.ConsoleInput.Name = "ConsoleInput";
            this.ConsoleInput.Size = new System.Drawing.Size(563, 23);
            this.ConsoleInput.TabIndex = 4;
            this.ConsoleInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleInput_KeyDown);
            // 
            // Send
            // 
            this.Send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Send.Location = new System.Drawing.Point(581, 445);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(95, 23);
            this.Send.TabIndex = 5;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(680, 268);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(95, 23);
            this.Quit.TabIndex = 6;
            this.Quit.Text = "Quit";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // Clear
            // 
            this.Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Clear.Location = new System.Drawing.Point(682, 445);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(95, 23);
            this.Clear.TabIndex = 7;
            this.Clear.Text = "Clear";
            this.Clear.UseVisualStyleBackColor = true;
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // OutputPanel
            // 
            this.OutputPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.OutputPanel.Location = new System.Drawing.Point(375, 12);
            this.OutputPanel.Name = "OutputPanel";
            this.OutputPanel.Size = new System.Drawing.Size(400, 250);
            this.OutputPanel.TabIndex = 8;
            // 
            // EZCode_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(783, 477);
            this.Controls.Add(this.OutputPanel);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.ConsoleInput);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.Console);
            this.Controls.Add(this.InputText);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(617, 405);
            this.Name = "EZCode_Form";
            this.Text = "EZCode-Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EZCode_Form_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EZCode_Form_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EZCode_Form_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}