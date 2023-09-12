namespace ezCode
{
    partial class NewFile
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
            Create = new Button();
            FileName = new TextBox();
            Directory = new TextBox();
            label1 = new Label();
            Open = new Button();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // Create
            // 
            Create.Location = new Point(604, 135);
            Create.Name = "Create";
            Create.Size = new Size(120, 46);
            Create.TabIndex = 0;
            Create.Text = "Create";
            Create.UseVisualStyleBackColor = true;
            Create.Click += Create_Click;
            // 
            // FileName
            // 
            FileName.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point);
            FileName.Location = new Point(160, 15);
            FileName.Name = "FileName";
            FileName.Size = new Size(438, 43);
            FileName.TabIndex = 1;
            // 
            // Directory
            // 
            Directory.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point);
            Directory.Location = new Point(160, 72);
            Directory.Name = "Directory";
            Directory.Size = new Size(381, 43);
            Directory.TabIndex = 2;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(8, 15);
            label1.Name = "label1";
            label1.Size = new Size(142, 43);
            label1.TabIndex = 3;
            label1.Text = "File Name";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Open
            // 
            Open.Location = new Point(553, 72);
            Open.Name = "Open";
            Open.Size = new Size(169, 46);
            Open.TabIndex = 4;
            Open.Text = "Open";
            Open.UseVisualStyleBackColor = true;
            Open.Click += Open_Click;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(604, 15);
            label2.Name = "label2";
            label2.Size = new Size(118, 43);
            label2.TabIndex = 5;
            label2.Text = ".ezcode";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(12, 72);
            label3.Name = "label3";
            label3.Size = new Size(142, 43);
            label3.TabIndex = 6;
            label3.Text = "Directory";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // NewFile
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(736, 190);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(Open);
            Controls.Add(label1);
            Controls.Add(Directory);
            Controls.Add(FileName);
            Controls.Add(Create);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximumSize = new Size(762, 261);
            MinimumSize = new Size(762, 261);
            Name = "NewFile";
            Text = "NewFile";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Create;
        private TextBox FileName;
        private TextBox Directory;
        private Label label1;
        private Button Open;
        private Label label2;
        private Label label3;
    }
}