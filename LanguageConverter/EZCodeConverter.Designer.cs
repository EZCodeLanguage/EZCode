namespace LanguageConverter
{
    partial class EZCodeConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EZCodeConverter));
            panel1 = new Panel();
            comboBox1 = new ComboBox();
            buildbutton = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            directorypanel = new Panel();
            directorylabel = new Label();
            directorypath = new TextBox();
            finddirectory = new Button();
            filepanel = new Panel();
            filelabel = new Label();
            filepath = new TextBox();
            filebutton = new Button();
            tabPage2 = new TabPage();
            button2 = new Button();
            panel2 = new Panel();
            label2 = new Label();
            textBox1 = new TextBox();
            panel4 = new Panel();
            button1 = new Button();
            Language = new ComboBox();
            label3 = new Label();
            textBox2 = new TextBox();
            label4 = new Label();
            tabPage1 = new TabPage();
            tabControl1 = new TabControl();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            directorypanel.SuspendLayout();
            filepanel.SuspendLayout();
            tabPage2.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            tabPage1.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(buildbutton);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(9, 189);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(417, 202);
            panel1.TabIndex = 12;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Cascadia Code", 12.25F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(407, 29);
            comboBox1.TabIndex = 7;
            // 
            // buildbutton
            // 
            buildbutton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            buildbutton.Font = new Font("Cascadia Code", 11.25F);
            buildbutton.Location = new Point(0, 39);
            buildbutton.Margin = new Padding(4);
            buildbutton.Name = "buildbutton";
            buildbutton.Size = new Size(410, 43);
            buildbutton.TabIndex = 0;
            buildbutton.Text = "Convert";
            buildbutton.UseVisualStyleBackColor = true;
            buildbutton.Click += buildbutton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(3, 90);
            pictureBox1.Margin = new Padding(4);
            pictureBox1.MaximumSize = new Size(10000, 250);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(407, 103);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 9F);
            label1.Location = new Point(2, 396);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(287, 16);
            label1.TabIndex = 11;
            label1.Text = "All Rights Reserved By JBros Development";
            // 
            // directorypanel
            // 
            directorypanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            directorypanel.BorderStyle = BorderStyle.FixedSingle;
            directorypanel.Controls.Add(directorylabel);
            directorypanel.Controls.Add(directorypath);
            directorypanel.Controls.Add(finddirectory);
            directorypanel.Location = new Point(10, 96);
            directorypanel.Margin = new Padding(4);
            directorypanel.Name = "directorypanel";
            directorypanel.Size = new Size(417, 85);
            directorypanel.TabIndex = 10;
            // 
            // directorylabel
            // 
            directorylabel.AutoSize = true;
            directorylabel.Font = new Font("Cascadia Code", 11.25F);
            directorylabel.Location = new Point(4, 7);
            directorylabel.Margin = new Padding(4, 0, 4, 0);
            directorylabel.Name = "directorylabel";
            directorylabel.Size = new Size(153, 20);
            directorylabel.TabIndex = 0;
            directorylabel.Text = "Output Directory";
            // 
            // directorypath
            // 
            directorypath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            directorypath.Font = new Font("Cascadia Code", 11.25F);
            directorypath.Location = new Point(4, 45);
            directorypath.Margin = new Padding(4);
            directorypath.Name = "directorypath";
            directorypath.Size = new Size(405, 25);
            directorypath.TabIndex = 2;
            // 
            // finddirectory
            // 
            finddirectory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            finddirectory.AutoSize = true;
            finddirectory.Font = new Font("Cascadia Code", 11.25F);
            finddirectory.Location = new Point(224, 7);
            finddirectory.Margin = new Padding(4);
            finddirectory.Name = "finddirectory";
            finddirectory.Size = new Size(186, 30);
            finddirectory.TabIndex = 1;
            finddirectory.Text = "Locate Directory";
            finddirectory.UseVisualStyleBackColor = true;
            finddirectory.Click += finddirectory_Click;
            // 
            // filepanel
            // 
            filepanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            filepanel.BorderStyle = BorderStyle.FixedSingle;
            filepanel.Controls.Add(filelabel);
            filepanel.Controls.Add(filepath);
            filepanel.Controls.Add(filebutton);
            filepanel.Location = new Point(9, 8);
            filepanel.Margin = new Padding(4);
            filepanel.Name = "filepanel";
            filepanel.Size = new Size(417, 80);
            filepanel.TabIndex = 9;
            // 
            // filelabel
            // 
            filelabel.AutoSize = true;
            filelabel.Font = new Font("Cascadia Code", 11.25F);
            filelabel.Location = new Point(5, 9);
            filelabel.Margin = new Padding(4, 0, 4, 0);
            filelabel.Name = "filelabel";
            filelabel.Size = new Size(198, 20);
            filelabel.TabIndex = 0;
            filelabel.Text = "EZProj Or EZCode File";
            // 
            // filepath
            // 
            filepath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            filepath.Font = new Font("Cascadia Code", 11.25F);
            filepath.Location = new Point(6, 42);
            filepath.Margin = new Padding(4);
            filepath.Name = "filepath";
            filepath.Size = new Size(407, 25);
            filepath.TabIndex = 2;
            // 
            // filebutton
            // 
            filebutton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            filebutton.AutoSize = true;
            filebutton.Font = new Font("Cascadia Code", 11.25F);
            filebutton.Location = new Point(263, 4);
            filebutton.Margin = new Padding(4);
            filebutton.Name = "filebutton";
            filebutton.Size = new Size(147, 30);
            filebutton.TabIndex = 1;
            filebutton.Text = "Find File";
            filebutton.UseVisualStyleBackColor = true;
            filebutton.Click += filebutton_Click;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(panel2);
            tabPage2.Controls.Add(panel4);
            tabPage2.Controls.Add(label4);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4);
            tabPage2.Size = new Size(439, 416);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Convert Code";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Font = new Font("Cascadia Code", 11.25F);
            button2.Location = new Point(8, 185);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(416, 33);
            button2.TabIndex = 0;
            button2.Text = "Convert";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label2);
            panel2.Controls.Add(textBox1);
            panel2.Location = new Point(9, 8);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(417, 169);
            panel2.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Cascadia Code", 11.25F);
            label2.Location = new Point(4, 4);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 20);
            label2.TabIndex = 0;
            label2.Text = "EZCode";
            // 
            // textBox1
            // 
            textBox1.AcceptsReturn = true;
            textBox1.AcceptsTab = true;
            textBox1.AllowDrop = true;
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Font = new Font("Cascadia Code", 9F);
            textBox1.Location = new Point(0, 28);
            textBox1.Margin = new Padding(4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(415, 139);
            textBox1.TabIndex = 2;
            textBox1.WordWrap = false;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panel4
            // 
            panel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(button1);
            panel4.Controls.Add(Language);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(textBox2);
            panel4.Location = new Point(9, 225);
            panel4.Margin = new Padding(4);
            panel4.Name = "panel4";
            panel4.Size = new Size(416, 167);
            panel4.TabIndex = 14;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.AutoSize = true;
            button1.Font = new Font("Cascadia Code", 10F);
            button1.Location = new Point(326, 4);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(84, 28);
            button1.TabIndex = 16;
            button1.Text = "Copy";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Language
            // 
            Language.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Language.DropDownStyle = ComboBoxStyle.DropDownList;
            Language.Font = new Font("Cascadia Code", 10F);
            Language.FormattingEnabled = true;
            Language.Location = new Point(198, 6);
            Language.Name = "Language";
            Language.Size = new Size(121, 25);
            Language.TabIndex = 4;
            Language.SelectedIndexChanged += Language_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Cascadia Code", 11.25F);
            label3.Location = new Point(5, 7);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(135, 20);
            label3.TabIndex = 0;
            label3.Text = "Converted Code";
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Bottom;
            textBox2.Font = new Font("Cascadia Code", 9F);
            textBox2.Location = new Point(0, 34);
            textBox2.Margin = new Padding(4);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = ScrollBars.Vertical;
            textBox2.Size = new Size(414, 131);
            textBox2.TabIndex = 2;
            textBox2.WordWrap = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Font = new Font("Cascadia Code", 9F);
            label4.Location = new Point(2, 396);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(287, 16);
            label4.TabIndex = 15;
            label4.Text = "All Rights Reserved By JBros Development";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(filepanel);
            tabPage1.Controls.Add(panel1);
            tabPage1.Controls.Add(directorypanel);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4);
            tabPage1.Size = new Size(439, 416);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Convert File";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(447, 449);
            tabControl1.TabIndex = 13;
            // 
            // EZCodeConverter
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(447, 449);
            Controls.Add(tabControl1);
            Font = new Font("Cascadia Code", 11.25F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MinimumSize = new Size(463, 488);
            Name = "EZCodeConverter";
            RightToLeft = RightToLeft.No;
            Text = "EZCode Language Converter";
            FormClosed += EZCodeConverter_FormClosed;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            directorypanel.ResumeLayout(false);
            directorypanel.PerformLayout();
            filepanel.ResumeLayout(false);
            filepanel.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button buildbutton;
        private PictureBox pictureBox1;
        private Label label1;
        private Panel directorypanel;
        private Label directorylabel;
        private TextBox directorypath;
        private Button finddirectory;
        private Panel filepanel;
        private Label filelabel;
        private TextBox filepath;
        private Button filebutton;
        private TabPage tabPage2;
        private TabPage tabPage1;
        private TabControl tabControl1;
        private Button button2;
        private Panel panel2;
        private Label label2;
        private TextBox textBox1;
        private Panel panel4;
        private Label label3;
        private TextBox textBox2;
        private Label label4;
        private ComboBox Language;
        private Button button1;
        private ComboBox comboBox1;
    }
}
