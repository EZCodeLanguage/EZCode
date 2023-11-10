namespace EZCodePlayer
{
    partial class Player
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            splitContainer = new SplitContainer();
            visualoutput = new Panel();
            Send = new Button();
            Clear = new Button();
            input = new TextBox();
            output = new RichTextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 0);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(visualoutput);
            splitContainer.Panel1MinSize = 0;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(Send);
            splitContainer.Panel2.Controls.Add(Clear);
            splitContainer.Panel2.Controls.Add(input);
            splitContainer.Panel2.Controls.Add(output);
            splitContainer.Panel2MinSize = 0;
            splitContainer.Size = new Size(783, 415);
            splitContainer.SplitterDistance = 148;
            splitContainer.SplitterIncrement = 7;
            splitContainer.SplitterWidth = 3;
            splitContainer.TabIndex = 0;
            // 
            // visualoutput
            // 
            visualoutput.BackColor = Color.Black;
            visualoutput.Dock = DockStyle.Fill;
            visualoutput.Location = new Point(0, 0);
            visualoutput.Name = "visualoutput";
            visualoutput.Size = new Size(783, 148);
            visualoutput.TabIndex = 0;
            // 
            // Send
            // 
            Send.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Send.BackColor = Color.FromArgb(25, 25, 25);
            Send.FlatAppearance.BorderColor = Color.Gray;
            Send.FlatAppearance.BorderSize = 0;
            Send.FlatStyle = FlatStyle.Flat;
            Send.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Send.ForeColor = Color.FromArgb(235, 235, 235);
            Send.Location = new Point(648, 239);
            Send.Name = "Send";
            Send.Size = new Size(65, 23);
            Send.TabIndex = 3;
            Send.Text = "Send";
            Send.UseVisualStyleBackColor = false;
            Send.Click += Send_Click;
            // 
            // Clear
            // 
            Clear.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            Clear.BackColor = Color.FromArgb(25, 25, 25);
            Clear.FlatAppearance.BorderColor = Color.Gray;
            Clear.FlatAppearance.BorderSize = 0;
            Clear.FlatStyle = FlatStyle.Flat;
            Clear.Font = new Font("Cascadia Code", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            Clear.ForeColor = Color.FromArgb(235, 235, 235);
            Clear.Location = new Point(715, 239);
            Clear.Name = "Clear";
            Clear.Size = new Size(65, 23);
            Clear.TabIndex = 2;
            Clear.Text = "Clear";
            Clear.UseVisualStyleBackColor = false;
            Clear.Click += Clear_Click;
            // 
            // input
            // 
            input.AcceptsReturn = true;
            input.AcceptsTab = true;
            input.AllowDrop = true;
            input.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            input.BackColor = Color.FromArgb(15, 15, 15);
            input.BorderStyle = BorderStyle.None;
            input.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point);
            input.ForeColor = Color.FromArgb(235, 235, 235);
            input.Location = new Point(4, 241);
            input.Name = "input";
            input.Size = new Size(641, 19);
            input.TabIndex = 1;
            input.KeyDown += input_KeyDown;
            // 
            // output
            // 
            output.AcceptsTab = true;
            output.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            output.AutoWordSelection = true;
            output.BackColor = Color.FromArgb(15, 15, 15);
            output.BorderStyle = BorderStyle.None;
            output.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point);
            output.ForeColor = Color.FromArgb(235, 235, 235);
            output.HideSelection = false;
            output.Location = new Point(0, -2);
            output.Name = "output";
            output.ReadOnly = true;
            output.Size = new Size(780, 237);
            output.TabIndex = 0;
            output.Text = "";
            output.WordWrap = false;
            output.TextChanged += output_TextChanged;
            // 
            // Player
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(10, 10, 10);
            ClientSize = new Size(783, 415);
            Controls.Add(splitContainer);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(200, 200);
            Name = "Player";
            Text = "Player";
            FormClosed += Player_FormClosed;
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private SplitContainer splitContainer;
        private RichTextBox output;
        private Button Clear;
        private TextBox input;
        private Button Send;
        private Panel visualoutput;
    }
}