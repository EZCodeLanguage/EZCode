using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EZCode;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EzCode_API
{
    public partial class EZCode_Form : Form
    {
        EzCode ezcode = new EzCode();
        private float zoomFactor = 1.1f;

        public EZCode_Form()
        {
            InitializeComponent();
            ezcode.Initialize(checkBox4.Checked, "NOTHING", OutputPanel, Console, checkBox2.Checked, checkBox1.Checked, checkBox3.Checked);
            if (File.Exists("cache")) InputText.Text = File.ReadAllText("cache");
            else File.Create("cache").Close();
            if (File.Exists("dircache")) directory.Text = File.ReadAllText("dircache");
            else File.Create("dircache").Close();
            AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;
            OutputPanel.MouseWheel += ezcode.MouseInput_Wheel;
            OutputPanel.MouseMove += ezcode.MouseInput_Move;
            OutputPanel.MouseDown += ezcode.MouseInput_Down;
            OutputPanel.MouseUp += ezcode.MouseInput_Up;
            version.Text = $"EZCode {ezcode.Version}";

            InputText.MouseWheel += InputText_MouseWheel;
            InputText.KeyDown += InputText_KeyDown;
        }

        private async void Start_Click(object sender, EventArgs e) // START
        {
            bool str = directory.Text == "" ? true : ezcode.SetScriptDirectory(directory.Text);
            ezcode.showStartAndEnd = checkBox1.Checked;
            ezcode.showFileInError = checkBox2.Checked;
            ezcode.ClearConsole = checkBox3.Checked;
            ezcode.InPanel = checkBox4.Checked;
            if (!str) MessageBox.Show("Invalid path", "Directory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            await ezcode.Play(InputText.Text);
        }

        private void Quit_Click(object sender, EventArgs e) // QUIT
        {
            ezcode.Stop();
        }

        private void Console_TextChanged(object sender, EventArgs e) // CONSOLE TEXT CHANGED
        {
            ezcode.ScrollToEnd(true, Color.Black, Color.Red);
        }

        private void ConsoleInput_KeyDown(object sender, KeyEventArgs e) // CONSOLE INPUT KEY DOWN
        {
            if (e.KeyCode == Keys.Enter)
            {
                ezcode.ConsoleInput(ConsoleInput.Text);
                ConsoleInput.Clear();
            }
        }

        private void Send_Click(object sender, EventArgs e) // SEND INPUT
        {
            ezcode.ConsoleInput(ConsoleInput.Text);
            ConsoleInput.Clear();
        }

        private void Clear_Click(object sender, EventArgs e) // CLEAR CONSOLE
        {
            Console.Clear();
        }

        private void EZCode_Form_KeyDown(object sender, KeyEventArgs e) // KEY DOWN
        {
            ezcode.KeyInput_Down(e);
        }

        private void EZCode_Form_KeyUp(object sender, KeyEventArgs e) // KEY UP
        {
            ezcode.KeyInput_Up(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) // START AND STOP SHORTCUTS
        {
            if (msg.Msg == 256)
            {
                if (keyData == (Keys.Control | Keys.P))
                {
                    Start.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.Q))
                {
                    Quit.PerformClick();
                }
                else if (keyData == (Keys.Control | Keys.S))
                {
                    File.WriteAllText("cache", InputText.Text);
                    File.WriteAllText("dircache", directory.Text);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EZCode_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("cache", InputText.Text);
            File.WriteAllText("dircache", directory.Text);
        }

        private void InputText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\t')
            {
                e.Handled = true;
                InputText.SelectedText = new string(' ', 4);
            }
        }
        private void InputText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Add)
            {
                IncreaseFontSize();
            }
            else if (e.Control && e.KeyCode == Keys.Subtract)
            {
                DecreaseFontSize();
            }
        }
        private void InputText_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                if (e.Delta > 0)
                {
                    IncreaseFontSize();
                }
                else if (e.Delta < 0)
                {
                    DecreaseFontSize();
                }
            }
        }
        private void IncreaseFontSize()
        {
            InputText.Font = new Font(InputText.Font.FontFamily, InputText.Font.Size * zoomFactor);
        }

        private void DecreaseFontSize()
        {
            InputText.Font = new Font(InputText.Font.FontFamily, InputText.Font.Size / zoomFactor);
        }
    }
}
