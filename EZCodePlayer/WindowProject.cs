using EZCode;

namespace EZCodePlayer
{
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
    }
}
