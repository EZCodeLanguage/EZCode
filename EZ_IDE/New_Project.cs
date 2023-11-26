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
    public partial class New_Project : Form
    {
        public bool DONE = false;
        public New_Project()
        {
            InitializeComponent();

            textBox1.Text = Settings.New_Project_Default_Directory;
            dir = Settings.New_Project_Default_Directory;
        }

        public ProjectSettings project = new ProjectSettings();
        string dir = "";

        private void button1_Click(object sender, EventArgs e)
        {
            // project directory
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string selectedPath = folderBrowserDialog.SelectedPath ?? "";
            textBox1.Text = selectedPath;
            project.Directory = selectedPath;
            dir = selectedPath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // project settings
            try
            {
                if (dir != "" && !Directory.Exists(dir))
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Please Input the Project Directory", "Project Directory is Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Settings.New_Project_Default_Directory = dir;
            project.Directory = dir;
            project.Name = textBox2.Text;
            Project_Settings_Form project_Settings_Form = new Project_Settings_Form(project);
            project_Settings_Form.ShowDialog();
            project = project_Settings_Form.projectSettings;
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
                return;
            }

            DONE = true;

            string proj_path = Path.Combine(textBox1.Text, textBox2.Text + ".ezproj");
            string main_path = Path.Combine(textBox1.Text, textBox2.Text + "_entry.ezcode");

            File.WriteAllText(proj_path, project.ConverToCode());
            if (project.StartUp == "")
                File.WriteAllText(main_path, "method Start\n\n// Entry Point for the program\n\nendmethod");

            Settings.New_Project_Default_Directory = dir;
            Settings.Current_Project_File = proj_path;

            Close();
        }
    }
}
