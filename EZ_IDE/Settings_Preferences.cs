using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZ_IDE
{
    public partial class Settings_Preferences : Form
    {
        public Settings_Preferences()
        {
            InitializeComponent();

            Save_Folder.Checked = Settings.Save_Folder;
            Auto_Save.Checked = Settings.Auto_Save;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Save_Folder = Save_Folder.Checked;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Settings.Auto_Save = Auto_Save.Checked;
        }
    }
}
