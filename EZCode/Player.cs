namespace EZCode.EZPlayer
{
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// An official way to play an ezcode file with WinForms.
    /// </summary>
    public partial class Player : Form
    {
        EzCode ezcode = new EzCode();
        EZProj proj;
        public bool useConsole = true;
        /// <summary>
        /// Initiates form. Use this: 'Application.Run(new Player(new EZProj("code or project file")));' in the Program class.
        /// </summary>
        public Player(EZProj eZProj)
        {
            try
            {
                proj = eZProj;
                if (eZProj.IconPath != null) Icon = new Icon(eZProj.IconPath);
                InitializeComponent();
                int d = 0;
                bool window = false;

                WindowProject windowProject = new WindowProject(proj.Name != null ? proj.Name : proj.FilePath, ezcode, proj, this);
                if (eZProj.IconPath != null) windowProject.Icon = new Icon(eZProj.IconPath);
                if (proj.Window)
                {
                    window = true;
                    if (proj.Debug) windowProject.Show();
                }
                else if (proj.IsVisual)
                {
                    d = 300;
                }
                else if (!proj.IsVisual)
                {
                    d = 5;
                }
                if (!window)
                {
                    splitContainer.SplitterDistance = d;
                    Show();
                }
                else
                {
                    Hide();
                    useConsole = false;
                }
                if (proj.Name != null) Text = proj.Name;
                else Text = proj.FilePath;
                ezcode.Initialize(!window, proj.FilePath, visualoutput != null ? visualoutput : new Panel(), proj.Debug && proj.Window ? windowProject.errors : output != null ? output : new RichTextBox(), proj.FileInErrors, proj.ShowBuild, proj.ClearConsole);
                ezcode.errorColor = Color.FromArgb(255, 20, 20);
                ezcode.normalColor = !window ? output.ForeColor : Color.Black;

                KeyPreview = true;
                AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;
                KeyDown += ezcode.KeyInput_Down;
                KeyUp += ezcode.KeyInput_Up;
                MouseWheel += ezcode.MouseInput_Wheel;
                MouseMove += ezcode.MouseInput_Move;
                MouseDown += ezcode.MouseInput_Down;
                MouseUp += ezcode.MouseInput_Up;
                output.MouseWheel += ezcode.MouseInput_Wheel;
                output.MouseMove += ezcode.MouseInput_Move;
                output.MouseDown += ezcode.MouseInput_Down;
                output.MouseUp += ezcode.MouseInput_Up;
                visualoutput.MouseWheel += ezcode.MouseInput_Wheel;
                visualoutput.MouseMove += ezcode.MouseInput_Move;
                visualoutput.MouseDown += ezcode.MouseInput_Down;
                visualoutput.MouseUp += ezcode.MouseInput_Up;

                PlayFromProj();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured.\nHere is the C# Exception message:'{ex.Message}'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private async void PlayFromProj()
        {
            await ezcode.PlayFromProj(proj);
        }

        private void Player_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            output.Clear();
        }

        private void Send_Click(object sender, EventArgs e)
        {
            ezcode.ConsoleInput(input.Text);
            input.Clear();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Send.PerformClick();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256)
            {
                if (keyData == (Keys.Control | Keys.Alt | Keys.H))
                {
                    //BackColor = Color.FromArgb(255 - BackColor.R, 255 - BackColor.G, 255 - BackColor.B);
                    Control[] c = new Control[] { input, output, splitContainer, Clear, Send };
                    foreach (Control control in c)
                    {
                        int am = 255;
                        control.BackColor = Color.FromArgb(am - control.BackColor.R, am - control.BackColor.G, am - control.BackColor.B);
                        control.ForeColor = Color.FromArgb(am - control.ForeColor.R, am - control.ForeColor.G, am - control.ForeColor.B);
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void output_TextChanged(object sender, EventArgs e)
        {
            ezcode.ScrollToEnd(true, output.ForeColor, Color.FromArgb(255, 20, 20));
        }
    } // MAIN
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
            Send.Location = new Point(648, 236);
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
            Clear.Location = new Point(715, 236);
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
            input.Location = new Point(4, 238);
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
            output.Size = new Size(780, 234);
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
    } // DESIGNER

    public partial class WindowProject : Form
    {
        EzCode EzCode;
        Player player;
        EZProj EZProj;
        bool started = false;
        public WindowProject(string name, EzCode ez, EZProj ezProj, Player p)
        {
            InitializeComponent();
            Name = name;
            EzCode = ez;
            EZProj = ezProj;
            player = p;
        }

        private void WindowProject_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        bool t = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!player.useConsole && !t) { player.Hide(); t = true; }
            if (started && !EzCode.playing && EZProj.CloseOnEnd)
            {
                Application.Exit();
            }
            else if (!started && EzCode.playing)
            {
                started = true;
            }
        }
    } // MAIN
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
    } // DESIGNER
}