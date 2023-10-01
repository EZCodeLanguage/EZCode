using Microsoft.Win32;
using System.IO;

namespace EZBuilder
{
    public partial class BuildingPrompt : Form
    {
        string keyName = @"JBrosDevelopment\EZCode\EZBuilder";
        public BuildingPrompt()
        {
            InitializeComponent();

            using (var key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    string retrievedData1 = key.GetValue("file") as string;
                    if (retrievedData1 != null)
                    {
                        filepath.Text = retrievedData1;
                    }
                    string retrievedData2 = key.GetValue("directory") as string;
                    if (retrievedData2 != null)
                    {
                        directorypath.Text = retrievedData2;
                    }
                }
            }
        }

        private void filebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "EZProj|*.ezproj";
            ofd.ShowDialog();
            filepath.Text = ofd.FileName;
        }

        private void finddirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            directorypath.Text = dialog.SelectedPath;
        }

        private void buildbutton_Click(object sender, EventArgs e)
        {
            string file = filepath.Text;
            string directory = directorypath.Text;
            string? error = null;

            using (var key = Registry.CurrentUser.CreateSubKey(keyName))
            {
                key.SetValue("file", file);
                key.SetValue("directory", directory);
            }

            try
            {
                if (!file.Contains(":")) new Exception();
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                {
                    error ??= "The (.ezproj) file was not found.";
                }
                if (fileInfo.Extension != ".ezproj")
                {
                    error ??= "The file specified does not have th correct extension (.ezproj).";
                }
            }
            catch
            {
                error ??= "The specified file is invalid.";
            }
            try
            {
                if (!directory.Contains(":")) new Exception();
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                if (!directoryInfo.Exists)
                {
                    DialogResult result = MessageBox.Show("The specified directory does not exist. Do you want to create it?", "Does Not Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Directory.CreateDirectory(directoryInfo.FullName);
                    }
                    else
                    {
                        error = "null";
                    }
                }
            }
            catch
            {
                error ??= "The specified directory is invalid.";
            }
            if (error != null && error != "null")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (error == null)
            {
                Builder builder = new Builder(file, directory);
                bool success = builder.Build();
                MessageBox.Show($"{builder.Output}", success ? "Successfull" : "Faild", MessageBoxButtons.OK, success ? MessageBoxIcon.Exclamation : MessageBoxIcon.Error);
            }
        }
    }
}