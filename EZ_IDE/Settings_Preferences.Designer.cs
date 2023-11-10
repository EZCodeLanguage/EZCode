namespace EZ_IDE
{
    partial class Settings_Preferences
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            Auto_Save = new CheckBox();
            label1 = new Label();
            Save_Folder = new CheckBox();
            tabPage2 = new TabPage();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(526, 419);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(Auto_Save);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(Save_Folder);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(518, 391);
            tabPage1.TabIndex = 2;
            tabPage1.Text = "Settings";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // Auto_Save
            // 
            Auto_Save.AutoSize = true;
            Auto_Save.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            Auto_Save.Location = new Point(11, 71);
            Auto_Save.Name = "Auto_Save";
            Auto_Save.Size = new Size(125, 24);
            Auto_Save.TabIndex = 3;
            Auto_Save.Text = "Auto Save File";
            Auto_Save.UseVisualStyleBackColor = true;
            Auto_Save.CheckedChanged += checkBox1_CheckedChanged_1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(11, 8);
            label1.Name = "label1";
            label1.Size = new Size(93, 30);
            label1.TabIndex = 2;
            label1.Text = "Settings";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Save_Folder
            // 
            Save_Folder.AutoSize = true;
            Save_Folder.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            Save_Folder.Location = new Point(11, 41);
            Save_Folder.Name = "Save_Folder";
            Save_Folder.Size = new Size(286, 24);
            Save_Folder.TabIndex = 1;
            Save_Folder.Text = "Save Tree View Folder Across Startups";
            Save_Folder.UseVisualStyleBackColor = true;
            Save_Folder.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(518, 391);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Preferences";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Settings_Preferences
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(526, 419);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MinimumSize = new Size(542, 456);
            Name = "Settings_Preferences";
            Text = "Settings_Preferences";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label1;
        private CheckBox Save_Folder;
        private CheckBox Auto_Save;
    }
}