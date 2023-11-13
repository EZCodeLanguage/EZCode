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
        public enum Tab
        {
            settings, debug
        }
        IDE ide = new IDE();
        public Settings_Preferences(IDE ide, Tab tab)
        {
            InitializeComponent();

            tabControl1.SelectedIndex = tab == Tab.settings ? 0 : 1;

            Save_Folder.Checked = Settings.Save_Folder;
            Auto_Save.Checked = Settings.Auto_Save;
            current_project.Text = Settings.Current_Project_File;
            DefaultZoom.Value = Settings.Default_Zoom;
            this.ide = ide;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Save_Folder = Save_Folder.Checked;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Settings.Auto_Save = Auto_Save.Checked;
        }

        private void current_project_TextChanged(object sender, EventArgs e)
        {
            Settings.Current_Project_File = current_project.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default_Zoom = (int)DefaultZoom.Value;
            ide.fctb.Zoom = Settings.Default_Zoom;
        }
    }
}
