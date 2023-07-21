using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ezCode
{
    public partial class NewFile : Form
    {
        public string file;
        public string safefile;
        public NewFile()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            Directory.Text = folderBrowserDialog.SelectedPath;
            file = Directory.Text + "\\" + FileName.Text + ".ezcode";
            safefile = FileName.Text + ".ezcode";
        }

        private void Create_Click(object sender, EventArgs e)
        {
            try
            {
                File.Create(file).Close();
                Close();
            }
            catch
            {
                MessageBox.Show("Unable to create File");
            }
        }
    }
}
