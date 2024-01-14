namespace EZ_IDE
{
    partial class EZText_To_EZCode
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
            Copy = new Button();
            OutputCode = new TextBox();
            button1 = new Button();
            linkLabel1 = new LinkLabel();
            SuspendLayout();
            // 
            // InputText
            // 
            InputText.Location = new Point(3, 26);
            InputText.Multiline = true;
            InputText.Name = "InputText";
            InputText.ScrollBars = ScrollBars.Both;
            InputText.Size = new Size(687, 192);
            InputText.TabIndex = 0;
            InputText.WordWrap = false;
            // 
            // Copy
            // 
            Copy.AutoSize = true;
            Copy.Location = new Point(3, 386);
            Copy.Name = "Copy";
            Copy.Size = new Size(687, 25);
            Copy.TabIndex = 3;
            Copy.Text = "Copy EZCode";
            Copy.UseVisualStyleBackColor = true;
            Copy.Click += Copy_Click;
            // 
            // OutputCode
            // 
            OutputCode.Location = new Point(3, 249);
            OutputCode.Multiline = true;
            OutputCode.Name = "OutputCode";
            OutputCode.ReadOnly = true;
            OutputCode.ScrollBars = ScrollBars.Both;
            OutputCode.Size = new Size(687, 133);
            OutputCode.TabIndex = 2;
            OutputCode.WordWrap = false;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.Location = new Point(3, 221);
            button1.Name = "button1";
            button1.Size = new Size(687, 25);
            button1.TabIndex = 1;
            button1.Text = "Translate To EZCode";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Translate;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(5, 5);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(98, 15);
            linkLabel1.TabIndex = 4;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "View EZText Docs";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // EZText_To_EZCode
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 413);
            Controls.Add(linkLabel1);
            Controls.Add(button1);
            Controls.Add(InputText);
            Controls.Add(Copy);
            Controls.Add(OutputCode);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "EZText_To_EZCode";
            Text = "EZText To EZCode";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputText;
        private Button Copy;
        private TextBox OutputCode;
        private Button button1;
        private LinkLabel linkLabel1;
    }
}