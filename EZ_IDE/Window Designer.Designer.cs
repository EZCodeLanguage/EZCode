namespace EZ_IDE
{
    partial class Window_Designer
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
            panel2 = new Panel();
            button8 = new Button();
            button7 = new Button();
            propertyGrid = new PropertyGrid();
            Dropdown = new ComboBox();
            MAIN = new Panel();
            panel4 = new Panel();
            button9 = new Button();
            button6 = new Button();
            OutputCode = new TextBox();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(button8);
            panel2.Controls.Add(button7);
            panel2.Controls.Add(propertyGrid);
            panel2.Controls.Add(Dropdown);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(588, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(264, 486);
            panel2.TabIndex = 1;
            // 
            // button8
            // 
            button8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button8.Location = new Point(134, 458);
            button8.Name = "button8";
            button8.Size = new Size(125, 23);
            button8.TabIndex = 8;
            button8.Text = "To Back";
            button8.UseVisualStyleBackColor = true;
            button8.Click += To_Back;
            // 
            // button7
            // 
            button7.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button7.Location = new Point(4, 458);
            button7.Name = "button7";
            button7.Size = new Size(125, 23);
            button7.TabIndex = 7;
            button7.Text = "To Front";
            button7.UseVisualStyleBackColor = true;
            button7.Click += To_Front;
            // 
            // propertyGrid
            // 
            propertyGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            propertyGrid.Location = new Point(4, 30);
            propertyGrid.Margin = new Padding(3, 2, 3, 2);
            propertyGrid.Name = "propertyGrid";
            propertyGrid.Size = new Size(255, 422);
            propertyGrid.TabIndex = 2;
            propertyGrid.PropertyValueChanged += Property_Value_Changed;
            // 
            // Dropdown
            // 
            Dropdown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            Dropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            Dropdown.FormattingEnabled = true;
            Dropdown.Location = new Point(2, 3);
            Dropdown.Name = "Dropdown";
            Dropdown.Size = new Size(258, 23);
            Dropdown.TabIndex = 1;
            Dropdown.SelectedIndexChanged += Object_Selection_Changed;
            // 
            // MAIN
            // 
            MAIN.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MAIN.BackColor = SystemColors.Control;
            MAIN.BorderStyle = BorderStyle.FixedSingle;
            MAIN.Location = new Point(6, 6);
            MAIN.Name = "MAIN";
            MAIN.Size = new Size(577, 341);
            MAIN.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.BackColor = Color.White;
            panel4.Controls.Add(button9);
            panel4.Controls.Add(button6);
            panel4.Controls.Add(OutputCode);
            panel4.Controls.Add(button5);
            panel4.Controls.Add(button4);
            panel4.Controls.Add(button3);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button1);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 353);
            panel4.Name = "panel4";
            panel4.Size = new Size(588, 133);
            panel4.TabIndex = 3;
            // 
            // button9
            // 
            button9.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button9.Location = new Point(150, 4);
            button9.Name = "button9";
            button9.Size = new Size(432, 23);
            button9.TabIndex = 7;
            button9.Text = "Generate Code";
            button9.UseVisualStyleBackColor = true;
            button9.Click += Generate_Code;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button6.Location = new Point(150, 104);
            button6.Name = "button6";
            button6.Size = new Size(432, 23);
            button6.TabIndex = 6;
            button6.Text = "Copy Code";
            button6.UseVisualStyleBackColor = true;
            button6.Click += Copy_Code;
            // 
            // OutputCode
            // 
            OutputCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            OutputCode.Location = new Point(150, 34);
            OutputCode.Multiline = true;
            OutputCode.Name = "OutputCode";
            OutputCode.ReadOnly = true;
            OutputCode.ScrollBars = ScrollBars.Vertical;
            OutputCode.Size = new Size(433, 66);
            OutputCode.TabIndex = 5;
            OutputCode.WordWrap = false;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            button5.Location = new Point(6, 104);
            button5.Name = "button5";
            button5.Size = new Size(138, 23);
            button5.TabIndex = 4;
            button5.Text = "Delete Selected";
            button5.UseVisualStyleBackColor = true;
            button5.Click += Delete_Selected;
            // 
            // button4
            // 
            button4.Location = new Point(3, 75);
            button4.Name = "button4";
            button4.Size = new Size(141, 23);
            button4.TabIndex = 3;
            button4.Text = "Add Textbox";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Add_Textbox;
            // 
            // button3
            // 
            button3.Location = new Point(3, 51);
            button3.Name = "button3";
            button3.Size = new Size(141, 23);
            button3.TabIndex = 2;
            button3.Text = "Add Label";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Add_Label;
            // 
            // button2
            // 
            button2.Location = new Point(3, 27);
            button2.Name = "button2";
            button2.Size = new Size(141, 23);
            button2.TabIndex = 1;
            button2.Text = "Add Button";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Add_Button;
            // 
            // button1
            // 
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(141, 23);
            button1.TabIndex = 0;
            button1.Text = "Add Shape";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Add_Shape;
            // 
            // Window_Designer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(852, 486);
            Controls.Add(panel4);
            Controls.Add(MAIN);
            Controls.Add(panel2);
            MinimumSize = new Size(800, 498);
            Name = "Window_Designer";
            ShowIcon = false;
            Text = "Window Designer";
            FormClosing += Window_Designer_FormClosing;
            Load += Window_Designer_Load;
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel2;
        private Panel MAIN;
        private Panel panel4;
        private Button button3;
        private Button button2;
        private Button button1;
        private Button button5;
        private Button button4;
        private Button button6;
        private TextBox OutputCode;
        private ComboBox Dropdown;
        private PropertyGrid propertyGrid;
        private Button button8;
        private Button button7;
        private Button button9;
    }
}