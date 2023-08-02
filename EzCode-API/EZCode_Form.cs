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

namespace EzCode_API
{
    public partial class EZCode_Form : Form
    {
        EZCode.EZCode ezcode = new EZCode.EZCode();

        public EZCode_Form()
        {
            InitializeComponent();
            ezcode.Initialize(_space: OutputPanel, _console: Console);
            if(File.Exists("cache"))InputText.Text = File.ReadAllText("cache");
            else File.Create("cache").Close();
        }

        private async void Start_Click(object sender, EventArgs e) // START
        {
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
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void EZCode_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText("cache", InputText.Text);
        }
    }
}
