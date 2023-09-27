using System;
using EZCode;

namespace EZCodePlayer
{
    public partial class Player : Form
    {
        EzCode ezcode = new EzCode();
        FileInfo file;
        EZProj proj;
        public bool useConsole = true;
        ProjectType projectType;
        public enum ProjectType
        {
            Project,
            Script,
            None
        }
        public Player(FileInfo _file, ProjectType _projectType = ProjectType.None)
        {
            InitializeComponent();
            int d = 0;
            bool window = false;
            proj = new EZProj(_file, _file.FullName);
            projectType = _projectType;
            file = _file;
            WindowProject windowProject = new WindowProject(proj.Name != null ? proj.Name : file.FullName, ezcode, proj, this);
            if (_projectType == ProjectType.Project)
            {
                if (proj.Window)
                {
                    window = true;
                    if (proj.Debug) windowProject.Show();
                }
                else if (proj.IsVisual)
                {
                    d = Height - 25;
                }
                else if (!proj.IsVisual)
                {
                    d = 5;
                }
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
            else Text = _file.FullName;
            ezcode.Initialize(!window, _file.FullName, visualoutput != null ? visualoutput : new Panel(), proj.Debug && proj.Window ? windowProject.errors : output != null ? output : new RichTextBox(), proj.FileInErrors, proj.ShowBuild, proj.ClearConsole);
            ezcode.errorColor = Color.FromArgb(255, 20, 20);
            ezcode.normalColor = !window ? output.ForeColor : Color.Black;

            AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;
            KeyDown += ezcode.KeyInput_Down;
            KeyUp += ezcode.KeyInput_Up;
            MouseWheel += ezcode.MouseInput_Wheel;
            MouseMove += ezcode.MouseInput_Move;
            MouseDown += ezcode.MouseInput_Down;
            MouseUp += ezcode.MouseInput_Up;

            Play();
        }
        private async void Play()
        {
            if (ProjectType.Script == projectType) await ezcode.Play(File.ReadAllText(file.FullName));
            else if (ProjectType.Project == projectType) await ezcode.PlayFromConfig(proj);
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
    }
}
