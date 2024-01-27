namespace TestEnvironment
{
    partial class ConverterExample
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
            splitContainer1 = new SplitContainer();
            main = new TextBox();
            converted = new TextBox();
            panel1 = new Panel();
            generate = new Button();
            Language = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(main);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(converted);
            splitContainer1.Size = new Size(800, 426);
            splitContainer1.SplitterDistance = 342;
            splitContainer1.TabIndex = 0;
            // 
            // main
            // 
            main.Dock = DockStyle.Fill;
            main.Location = new Point(0, 0);
            main.Multiline = true;
            main.Name = "main";
            main.ScrollBars = ScrollBars.Both;
            main.Size = new Size(342, 426);
            main.TabIndex = 0;
            main.WordWrap = false;
            // 
            // converted
            // 
            converted.Dock = DockStyle.Fill;
            converted.Location = new Point(0, 0);
            converted.Multiline = true;
            converted.Name = "converted";
            converted.ReadOnly = true;
            converted.ScrollBars = ScrollBars.Both;
            converted.Size = new Size(454, 426);
            converted.TabIndex = 1;
            converted.WordWrap = false;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(splitContainer1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 426);
            panel1.TabIndex = 1;
            // 
            // generate
            // 
            generate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            generate.Location = new Point(0, 427);
            generate.Name = "generate";
            generate.Size = new Size(679, 23);
            generate.TabIndex = 2;
            generate.Text = "Generate";
            generate.UseVisualStyleBackColor = true;
            generate.Click += generate_Click;
            // 
            // Language
            // 
            Language.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Language.DropDownStyle = ComboBoxStyle.DropDownList;
            Language.FormattingEnabled = true;
            Language.Location = new Point(679, 427);
            Language.Name = "Language";
            Language.Size = new Size(121, 23);
            Language.TabIndex = 3;
            // 
            // ConverterExample
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Language);
            Controls.Add(generate);
            Controls.Add(panel1);
            Name = "ConverterExample";
            Text = "ConverterExample";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TextBox main;
        private TextBox converted;
        private Panel panel1;
        private Button generate;
        private ComboBox Language;
    }
}