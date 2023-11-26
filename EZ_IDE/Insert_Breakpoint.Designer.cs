namespace EZ_IDE
{
    partial class Insert_Breakpoint
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
            button1 = new Button();
            FilePathTextBox = new TextBox();
            MethodTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            SegmentNumericUpDown = new NumericUpDown();
            label3 = new Label();
            RemoveOnHitCheckbox = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)SegmentNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(329, 90);
            button1.Name = "button1";
            button1.Size = new Size(103, 23);
            button1.TabIndex = 0;
            button1.Text = "Insert";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Insert_Click;
            // 
            // FilePathTextBox
            // 
            FilePathTextBox.Location = new Point(110, 61);
            FilePathTextBox.Name = "FilePathTextBox";
            FilePathTextBox.Size = new Size(322, 23);
            FilePathTextBox.TabIndex = 1;
            // 
            // MethodTextBox
            // 
            MethodTextBox.Location = new Point(110, 32);
            MethodTextBox.Name = "MethodTextBox";
            MethodTextBox.Size = new Size(322, 23);
            MethodTextBox.TabIndex = 2;
            // 
            // label1
            // 
            label1.Location = new Point(5, 35);
            label1.Name = "label1";
            label1.Size = new Size(90, 23);
            label1.TabIndex = 3;
            label1.Text = "Method Name";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.Location = new Point(5, 61);
            label2.Name = "label2";
            label2.Size = new Size(90, 23);
            label2.TabIndex = 4;
            label2.Text = "File Path";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SegmentNumericUpDown
            // 
            SegmentNumericUpDown.Location = new Point(110, 5);
            SegmentNumericUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            SegmentNumericUpDown.Name = "SegmentNumericUpDown";
            SegmentNumericUpDown.Size = new Size(190, 23);
            SegmentNumericUpDown.TabIndex = 5;
            // 
            // label3
            // 
            label3.Location = new Point(5, 3);
            label3.Name = "label3";
            label3.Size = new Size(106, 23);
            label3.TabIndex = 6;
            label3.Text = "Segment Number";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // RemoveOnHitCheckbox
            // 
            RemoveOnHitCheckbox.AutoSize = true;
            RemoveOnHitCheckbox.Location = new Point(306, 7);
            RemoveOnHitCheckbox.Name = "RemoveOnHitCheckbox";
            RemoveOnHitCheckbox.Size = new Size(126, 19);
            RemoveOnHitCheckbox.TabIndex = 7;
            RemoveOnHitCheckbox.Text = "Remove on first hit";
            RemoveOnHitCheckbox.UseVisualStyleBackColor = true;
            // 
            // Insert_Breakpoint
            // 
            AcceptButton = button1;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(435, 118);
            Controls.Add(RemoveOnHitCheckbox);
            Controls.Add(SegmentNumericUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(MethodTextBox);
            Controls.Add(FilePathTextBox);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Insert_Breakpoint";
            Text = "Insert Breakpoint";
            ((System.ComponentModel.ISupportInitialize)SegmentNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox FilePathTextBox;
        private TextBox MethodTextBox;
        private Label label1;
        private Label label2;
        private NumericUpDown SegmentNumericUpDown;
        private Label label3;
        private CheckBox RemoveOnHitCheckbox;
    }
}