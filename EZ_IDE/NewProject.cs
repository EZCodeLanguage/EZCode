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
    public partial class NewProject : Form
    {
        public NewProject()
        {
            InitializeComponent();
        }

        public ProjectSettings project = new ProjectSettings();

        private void button1_Click(object sender, EventArgs e)
        {
            // project directory
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBox1.Text = folderBrowserDialog.SelectedPath;
            project.Directory = folderBrowserDialog.SelectedPath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // project settings
            Project_Settings_Form project_Settings_Form = new Project_Settings_Form(project);
            project_Settings_Form.ShowDialog();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            // create project
            try
            {
                if (!Directory.Exists(textBox1.Text))
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("The Project Directory is invalid or doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string proj_path = Path.Combine(textBox1.Text, textBox2.Text + ".ezproj");
            string main_path = Path.Combine(textBox1.Text, textBox2.Text + "_entry.ezproj");

            File.WriteAllText(proj_path, project.ConverToCode());
            File.WriteAllText(main_path, "method Start\n\n// Entry Point for the program\n\nendmethod");

            project.Directory = textBox1.Text;
            project.StartUp = main_path;
            Settings.Current_Project_File = proj_path;

            Close();
        }
    }
}
