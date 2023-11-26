namespace EZ_IDE
{
    partial class Text_To_Code
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
            OutputCode = new TextBox();
            Copy = new Button();
            label1 = new Label();
            label2 = new Label();
            InputText = new TextBox();
            AllowSpaces = new CheckBox();
            UseWrap = new CheckBox();
            CharsBeforeWrap = new NumericUpDown();
            CBF_label = new Label();
            ((System.ComponentModel.ISupportInitialize)CharsBeforeWrap).BeginInit();
            SuspendLayout();
            // 
            // OutputCode
            // 
            OutputCode.Location = new Point(1, 180);
            OutputCode.Multiline = true;
            OutputCode.Name = "OutputCode";
            OutputCode.ReadOnly = true;
            OutputCode.ScrollBars = ScrollBars.Both;
            OutputCode.Size = new Size(618, 110);
            OutputCode.TabIndex = 1;
            OutputCode.WordWrap = false;
            // 
            // Copy
            // 
            Copy.AutoSize = true;
            Copy.Location = new Point(480, 296);
            Copy.Name = "Copy";
            Copy.Size = new Size(137, 25);
            Copy.TabIndex = 2;
            Copy.Text = "Copy Converted Text";
            Copy.UseVisualStyleBackColor = true;
            Copy.Click += Copy_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(2, 162);
            label1.Name = "label1";
            label1.Size = new Size(35, 15);
            label1.TabIndex = 7;
            label1.Text = "Code";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(2, 26);
            label2.Name = "label2";
            label2.Size = new Size(28, 15);
            label2.TabIndex = 6;
            label2.Text = "Text";
            // 
            // InputText
            // 
            InputText.Location = new Point(1, 44);
            InputText.Multiline = true;
            InputText.Name = "InputText";
            InputText.ScrollBars = ScrollBars.Both;
            InputText.Size = new Size(618, 112);
            InputText.TabIndex = 0;
            InputText.WordWrap = false;
            InputText.TextChanged += InputText_TextChanged;
            // 
            // AllowSpaces
            // 
            AllowSpaces.AutoSize = true;
            AllowSpaces.Checked = true;
            AllowSpaces.CheckState = CheckState.Checked;
            AllowSpaces.Location = new Point(2, 4);
            AllowSpaces.Name = "AllowSpaces";
            AllowSpaces.Size = new Size(95, 19);
            AllowSpaces.TabIndex = 3;
            AllowSpaces.Text = "Allow Spaces";
            AllowSpaces.UseVisualStyleBackColor = true;
            AllowSpaces.CheckedChanged += AllowSpaces_CheckedChanged;
            // 
            // UseWrap
            // 
            UseWrap.AutoSize = true;
            UseWrap.Location = new Point(103, 4);
            UseWrap.Name = "UseWrap";
            UseWrap.Size = new Size(76, 19);
            UseWrap.TabIndex = 4;
            UseWrap.Text = "Use Wrap";
            UseWrap.UseVisualStyleBackColor = true;
            UseWrap.CheckedChanged += UseWrap_CheckedChanged;
            // 
            // CharsBeforeWrap
            // 
            CharsBeforeWrap.Enabled = false;
            CharsBeforeWrap.Location = new Point(296, 3);
            CharsBeforeWrap.Name = "CharsBeforeWrap";
            CharsBeforeWrap.Size = new Size(49, 23);
            CharsBeforeWrap.TabIndex = 5;
            CharsBeforeWrap.Value = new decimal(new int[] { 35, 0, 0, 0 });
            CharsBeforeWrap.ValueChanged += CharsBeforeWrap_ValueChanged;
            // 
            // CBF_label
            // 
            CBF_label.AutoSize = true;
            CBF_label.Enabled = false;
            CBF_label.Location = new Point(185, 5);
            CBF_label.Name = "CBF_label";
            CBF_label.Size = new Size(105, 15);
            CBF_label.TabIndex = 8;
            CBF_label.Text = "Chars Before Wrap";
            // 
            // Text_To_Code
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(621, 323);
            Controls.Add(CBF_label);
            Controls.Add(CharsBeforeWrap);
            Controls.Add(UseWrap);
            Controls.Add(AllowSpaces);
            Controls.Add(InputText);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Copy);
            Controls.Add(OutputCode);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Text_To_Code";
            Text = "Text To Code";
            ((System.ComponentModel.ISupportInitialize)CharsBeforeWrap).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox OutputCode;
        private Button Copy;
        private Label label1;
        private Label label2;
        private TextBox InputText;
        private CheckBox AllowSpaces;
        private CheckBox UseWrap;
        private NumericUpDown CharsBeforeWrap;
        private Label CBF_label;
    }
}