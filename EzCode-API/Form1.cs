using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EzCode_API
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            API.Initialize(richTextBox1, pictureBox1, textBox1.Text, AppDomain.CurrentDomain.BaseDirectory);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            API.ConsoleInput(textBox2.Text);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            API.KeyInput_Down(e);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            API.KeyInput_Up(e);
        }
    }
}
