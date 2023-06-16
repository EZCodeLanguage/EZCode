namespace EzCode_API
{
    partial class Form1
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
            textBox1 = new TextBox();
            pictureBox1 = new PictureBox();
            richTextBox1 = new RichTextBox();
            button1 = new Button();
            textBox2 = new TextBox();
            button2 = new Button();
            button3 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(22, 26);
            textBox1.Margin = new Padding(6);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(643, 469);
            textBox1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(680, 26);
            pictureBox1.Margin = new Padding(6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(758, 471);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.PreviewKeyDown += Form1_PreviewKeyDown;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(22, 567);
            richTextBox1.Margin = new Padding(6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1414, 307);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            richTextBox1.KeyDown += Form1_KeyDown;
            richTextBox1.KeyUp += Form1_KeyUp;
            richTextBox1.PreviewKeyDown += Form1_PreviewKeyDown;
            // 
            // button1
            // 
            button1.Location = new Point(22, 512);
            button1.Margin = new Padding(6);
            button1.Name = "button1";
            button1.Size = new Size(1226, 49);
            button1.TabIndex = 3;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(22, 892);
            textBox2.Margin = new Padding(6);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(1226, 39);
            textBox2.TabIndex = 4;
            // 
            // button2
            // 
            button2.Location = new Point(1263, 892);
            button2.Margin = new Padding(6);
            button2.Name = "button2";
            button2.Size = new Size(176, 49);
            button2.TabIndex = 5;
            button2.Text = "Send";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(1260, 512);
            button3.Margin = new Padding(6);
            button3.Name = "button3";
            button3.Size = new Size(176, 49);
            button3.TabIndex = 6;
            button3.Text = "Quit";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1460, 960);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(textBox2);
            Controls.Add(button1);
            Controls.Add(richTextBox1);
            Controls.Add(pictureBox1);
            Controls.Add(textBox1);
            Margin = new Padding(6);
            Name = "Form1";
            Text = "Form1";
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            PreviewKeyDown += Form1_PreviewKeyDown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private PictureBox pictureBox1;
        private RichTextBox richTextBox1;
        private Button button1;
        private TextBox textBox2;
        private Button button2;
        private Button button3;
    }
}