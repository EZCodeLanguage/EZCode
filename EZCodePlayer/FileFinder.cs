using Microsoft.Win32;

namespace EZCodePlayer
{
    public partial class FileFinder : Form
    {
        string keyName = @"JBrosDevelopment\EZCode\EZCodePlayer";
        public FileFinder(string? args)
        {
            bool er = false;
            InitializeComponent();

            if (args == null || args == "")
            {
                //Open File Picker
                using (var key = Registry.CurrentUser.OpenSubKey(keyName))
                {
                    if (key != null)
                    {
                        string retrievedData = key.GetValue("path") as string;
                        if (retrievedData != null)
                        {
                            path.Text = retrievedData;
                        }
                    }
                }
            }
            else
            {
                //Play File
                path.Text = args;
                play.PerformClick();
            }
            if (er)
            {
                MessageBox.Show("There was an error getting the file. Check if it's path is correct or it's extension is '.ezcode' or '.ezproj'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void search_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog(this);
            path.Text = openFileDialog.FileName;
        }

        private void play_Click(object sender, EventArgs e)
        {
            if (path.Text == "")
            {
                MessageBox.Show("No file inputted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!File.Exists(path.Text))
            {
                MessageBox.Show("The inputted file does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (var key = Registry.CurrentUser.CreateSubKey(keyName))
                {
                    key.SetValue("path", path.Text);
                }

                FileInfo fileInfo = new FileInfo(path.Text);
                if (fileInfo.Extension == ".ezproj")
                {
                    new Player(fileInfo, Player.ProjectType.Project);
                    Hide();
                }
                else if (fileInfo.Extension == ".ezcode")
                {
                    new Player(fileInfo, Player.ProjectType.Script);
                    Hide();
                }
                else
                {
                    MessageBox.Show("Expected '.ezcode' or '.ezproj' file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void FileFinder_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}