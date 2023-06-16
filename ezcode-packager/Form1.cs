namespace ezcode_packager
{
    using PackageSystem;
    using System.Text;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBox1.Text = folderBrowserDialog.SelectedPath + @"\";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EzCode Files (*.ezcode)|*.ezcode";
            openFileDialog.ShowDialog();
            textBox2.Text = openFileDialog.SafeFileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string input = textBox1.Text;
                string startUp = textBox2.Text;
                string outputFile = textBox3.Text;
                string outputFolder = textBox4.Text;
                string output = outputFolder + outputFile + ".ezproj";
                string author = textBox5.Text;
                string company = textBox6.Text;

                if (author == "Author") author = "Unknown";
                if (company == "Company") company = "Unknown";

                File.WriteAllText(input + "Lisence.txt", "Author: " + Environment.NewLine + author + Environment.NewLine + "Company: " + Environment.NewLine + company);
                File.WriteAllText(input + "root_.ezcode", "playFile await ~/" + startUp);

                if (!Directory.Exists(outputFolder)) Directory.CreateDirectory(outputFolder);

                PackageHandler.CreatePackage(input, output);

                File.Delete(input + "root_.ezcode");
                File.Delete(input + "Lisence.txt");

                MessageBox.Show("Succesfully Packaged " + outputFile, "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception er)
            {
                Console.WriteLine("MESSAGE: " + er.Message);
                Console.WriteLine("STACKTRACE: " + er.StackTrace);
                MessageBox.Show("There was an Error while packaging", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            textBox4.Text = folderBrowserDialog.SelectedPath + @"\";
        }
    }
}

namespace PackageSystem
{
    using System.IO;
    using System.IO.Compression;
    public static class PackageHandler
    {
        public static void CreatePackage(string inputFolderPath, string outputFilePath)
        {
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            ZipFile.CreateFromDirectory(inputFolderPath, outputFilePath);
        }

        public static void extractPackage(string output, string location)
        {
            if (!Directory.Exists(location)) Directory.CreateDirectory(location);
            if (File.Exists(output))
            {
                ZipFile.ExtractToDirectory(output, location);
            }
        }

        public static void deleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public static void deleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}