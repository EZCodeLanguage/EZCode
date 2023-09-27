namespace EZCodePlayer
{
    partial class WindowProject
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            button1 = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            errors = new RichTextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(273, 23);
            label1.TabIndex = 0;
            label1.Text = "Running Program (Debug)";
            // 
            // button1
            // 
            button1.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(12, 170);
            button1.Name = "button1";
            button1.Size = new Size(337, 30);
            button1.TabIndex = 1;
            button1.Text = "Quit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 250;
            timer1.Tick += timer1_Tick;
            // 
            // errors
            // 
            errors.AccessibleDescription = "windowProject";
            errors.BorderStyle = BorderStyle.None;
            errors.Font = new Font("Segoe UI Semibold", 8.75F, FontStyle.Bold, GraphicsUnit.Point);
            errors.Location = new Point(12, 33);
            errors.Name = "errors";
            errors.ReadOnly = true;
            errors.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            errors.Size = new Size(337, 129);
            errors.TabIndex = 2;
            errors.Text = "";
            errors.WordWrap = false;
            // 
            // WindowProject
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonShadow;
            ClientSize = new Size(361, 207);
            Controls.Add(errors);
            Controls.Add(button1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "WindowProject";
            Text = "WindowProject";
            FormClosed += WindowProject_FormClosed;
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Button button1;
        private System.Windows.Forms.Timer timer1;
        public RichTextBox errors;
    }
}