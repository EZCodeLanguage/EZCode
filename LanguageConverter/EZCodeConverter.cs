namespace LanguageConverter
{
    using EZCode.Converter;
    using Microsoft.Win32;
    using RegestrySettings;
    public partial class EZCodeConverter : Form
    {
        Converter converter = new Converter();
        public EZCodeConverter()
        {
            InitializeComponent();

            try
            {

                Settings.keyName = @"JBrosDevelopment\EZCode\LanguageConverter";
                Language.DataSource = Enum.GetValues(typeof(Converter.ProgrammingLanguage));
                comboBox1.DataSource = Enum.GetValues(typeof(Converter.ProgrammingLanguage));
                textBox1.Text = Settings.GetKey("ConverterText", "") != null ? Settings.GetKey("ConverterText", "")!.ToString() : "";
                filepath.Text = Settings.GetKey("FileUrl", "") != null ? Settings.GetKey("FileUrl", "")!.ToString() : "";
                directorypath.Text = Settings.GetKey("DirectoryUrl", "") != null ? Settings.GetKey("DirectoryUrl", "")!.ToString() : "";
                string Lang = Settings.GetKey("ConverterLanguage", "") != null ? Settings.GetKey("ConverterLanguage", "")!.ToString() : "";
                tabControl1.SelectedIndex = int.Parse(Settings.GetKey("Tab", "") != null ? Settings.GetKey("Tab", "")!.ToString() : "0");

                Converter.ProgrammingLanguage language;
                Enum.TryParse(Lang, out language);
                Language.SelectedIndex = Language.Items.Cast<Converter.ProgrammingLanguage>().ToList().IndexOf(language);
                comboBox1.SelectedIndex = Language.Items.Cast<Converter.ProgrammingLanguage>().ToList().IndexOf(language);
            }
            catch
            {

            }
        }

        private void Language_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = Language.SelectedIndex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Converter.ProgrammingLanguage language;
            Enum.TryParse(Language.SelectedValue.ToString(), out language);

            textBox2.Text = converter.Convert(textBox1.Text, language);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool con = textBox1.Text.Contains("\t");
            int sel = textBox1.SelectionStart;
            textBox1.Text = textBox1.Text.Replace("\t", "    ");
            if (con) textBox1.SelectionStart = sel + 3;
        }

        private void EZCodeConverter_FormClosed(object sender, EventArgs e)
        {
            Settings.SetKey("ConverterText", textBox1.Text);
            Settings.SetKey("ConverterLanguage", Language.SelectedValue.ToString());
            Settings.SetKey("FileUrl", filepath.Text.ToString());
            Settings.SetKey("DirectoryUrl", directorypath.Text.ToString());
            Settings.SetKey("Tab", tabControl1.SelectedIndex.ToString());
        }

        private void filebutton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "EZCode|*.ezcode";
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
            List<string> error = new List<string>();

            try
            {
                if (!file.Contains(":")) new Exception();
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                {
                    error.Add("The (.ezcode) file was not found.");
                }
                if (fileInfo.Extension != ".ezcode")
                {
                    error.Add("The file specified does not have th correct extension (.ezproj).");
                }
            }
            catch
            {
                error.Add("The specified file is invalid.");
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

                    }
                }
            }
            catch
            {
                error.Add("The specified directory is invalid.");
            }
            if (error.Count != 0)
            {
                error.Add("An error occured while converting.");
                for (int i = 0; i < error.Count; i++)
                {
                    MessageBox.Show(error[i], "EZCode Language Converter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Converter.ProgrammingLanguage language;
                Enum.TryParse(Language.SelectedValue.ToString(), out language);

                converter.Convert(File.ReadAllText(file), language, out Converter.ProgramFile[] files);
                
                for (int i = 0; i < files.Length; i++)
                {
                    File.WriteAllText(Path.Combine(directory, files[i].Name), files[i].Content);
                }

                MessageBox.Show($"Successfully Wrote converted files to '{directory}'", "EZCode Lanfuage Converter", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
