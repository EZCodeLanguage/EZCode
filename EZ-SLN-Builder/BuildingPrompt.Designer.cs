namespace EZBuilder
{
    partial class BuildingPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildingPrompt));
            filelabel = new Label();
            filepath = new TextBox();
            filebutton = new Button();
            filepanel = new Panel();
            directorypanel = new Panel();
            directorylabel = new Label();
            directorypath = new TextBox();
            finddirectory = new Button();
            buildbutton = new Button();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            filepanel.SuspendLayout();
            directorypanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // filelabel
            // 
            filelabel.AutoSize = true;
            filelabel.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            filelabel.Location = new Point(3, 3);
            filelabel.Name = "filelabel";
            filelabel.Size = new Size(108, 20);
            filelabel.TabIndex = 0;
            filelabel.Text = "EZProj file";
            // 
            // filepath
            // 
            filepath.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            filepath.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            filepath.Location = new Point(3, 37);
            filepath.Name = "filepath";
            filepath.Size = new Size(317, 25);
            filepath.TabIndex = 2;
            // 
            // filebutton
            // 
            filebutton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            filebutton.AutoSize = true;
            filebutton.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            filebutton.Location = new Point(206, 3);
            filebutton.Name = "filebutton";
            filebutton.Size = new Size(114, 30);
            filebutton.TabIndex = 1;
            filebutton.Text = "Find File";
            filebutton.UseVisualStyleBackColor = true;
            filebutton.Click += filebutton_Click;
            // 
            // filepanel
            // 
            filepanel.BorderStyle = BorderStyle.FixedSingle;
            filepanel.Controls.Add(filelabel);
            filepanel.Controls.Add(filepath);
            filepanel.Controls.Add(filebutton);
            filepanel.Location = new Point(8, 12);
            filepanel.Name = "filepanel";
            filepanel.Size = new Size(325, 71);
            filepanel.TabIndex = 3;
            // 
            // directorypanel
            // 
            directorypanel.BorderStyle = BorderStyle.FixedSingle;
            directorypanel.Controls.Add(directorylabel);
            directorypanel.Controls.Add(directorypath);
            directorypanel.Controls.Add(finddirectory);
            directorypanel.Location = new Point(8, 99);
            directorypanel.Name = "directorypanel";
            directorypanel.Size = new Size(325, 71);
            directorypanel.TabIndex = 4;
            // 
            // directorylabel
            // 
            directorylabel.AutoSize = true;
            directorylabel.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            directorylabel.Location = new Point(3, 3);
            directorylabel.Name = "directorylabel";
            directorylabel.Size = new Size(153, 20);
            directorylabel.TabIndex = 0;
            directorylabel.Text = "Output Directory";
            // 
            // directorypath
            // 
            directorypath.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            directorypath.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            directorypath.Location = new Point(4, 41);
            directorypath.Name = "directorypath";
            directorypath.Size = new Size(316, 25);
            directorypath.TabIndex = 2;
            // 
            // finddirectory
            // 
            finddirectory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            finddirectory.AutoSize = true;
            finddirectory.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            finddirectory.Location = new Point(175, 7);
            finddirectory.Name = "finddirectory";
            finddirectory.Size = new Size(145, 30);
            finddirectory.TabIndex = 1;
            finddirectory.Text = "Find Directory";
            finddirectory.UseVisualStyleBackColor = true;
            finddirectory.Click += finddirectory_Click;
            // 
            // buildbutton
            // 
            buildbutton.Font = new Font("Cascadia Code", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            buildbutton.Location = new Point(2, 5);
            buildbutton.Name = "buildbutton";
            buildbutton.Size = new Size(319, 32);
            buildbutton.TabIndex = 0;
            buildbutton.Text = "Build";
            buildbutton.UseVisualStyleBackColor = true;
            buildbutton.Click += buildbutton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(3, 303);
            label1.Name = "label1";
            label1.Size = new Size(287, 16);
            label1.TabIndex = 7;
            label1.Text = "All Rights Reserved By JBros Development";
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(1, 44);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(322, 63);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(buildbutton);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(8, 188);
            panel1.Name = "panel1";
            panel1.Size = new Size(325, 112);
            panel1.TabIndex = 8;
            // 
            // BuildingPrompt
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(340, 322);
            Controls.Add(panel1);
            Controls.Add(label1);
            Controls.Add(directorypanel);
            Controls.Add(filepanel);
            Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "BuildingPrompt";
            Text = "EZ SLN Builder";
            filepanel.ResumeLayout(false);
            filepanel.PerformLayout();
            directorypanel.ResumeLayout(false);
            directorypanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label filelabel;
        private TextBox filepath;
        private Button filebutton;
        private Panel filepanel;
        private Panel directorypanel;
        private Label directorylabel;
        private TextBox directorypath;
        private Button finddirectory;
        private Button buildbutton;
        private Label label1;
        private PictureBox pictureBox1;
        private Panel panel1;
    }
}