namespace EZCodePlayer
{
    partial class FileFinder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileFinder));
            play = new Button();
            path = new TextBox();
            search = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // play
            // 
            play.AutoSize = true;
            play.Font = new Font("Cascadia Code", 12.75F, FontStyle.Regular, GraphicsUnit.Point);
            play.Location = new Point(94, 87);
            play.Name = "play";
            play.Size = new Size(339, 34);
            play.TabIndex = 0;
            play.Text = "Play";
            play.UseVisualStyleBackColor = true;
            play.Click += play_Click;
            // 
            // path
            // 
            path.Font = new Font("Cascadia Code", 12.75F, FontStyle.Regular, GraphicsUnit.Point);
            path.Location = new Point(135, 55);
            path.Name = "path";
            path.Size = new Size(373, 27);
            path.TabIndex = 1;
            // 
            // search
            // 
            search.AutoSize = true;
            search.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            search.Location = new Point(12, 54);
            search.Name = "search";
            search.Size = new Size(119, 28);
            search.TabIndex = 2;
            search.Text = "Search";
            search.UseVisualStyleBackColor = true;
            search.Click += search_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.EZCode_Logo;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(12, 127);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(496, 68);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Cascadia Code", 9F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(4, 196);
            label1.Name = "label1";
            label1.Size = new Size(287, 16);
            label1.TabIndex = 4;
            label1.Text = "All Rights Reserved By JBros Development";
            // 
            // label2
            // 
            label2.Font = new Font("Cascadia Code", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, -1);
            label2.Name = "label2";
            label2.Size = new Size(496, 52);
            label2.TabIndex = 5;
            label2.Text = "EZCode Player";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FileFinder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(520, 215);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(search);
            Controls.Add(path);
            Controls.Add(play);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "FileFinder";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EZCode Player";
            FormClosed += FileFinder_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button play;
        private TextBox path;
        private Button search;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
    }
}