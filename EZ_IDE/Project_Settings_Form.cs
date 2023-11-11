using System.ComponentModel;
using System.Data;
using System.IO;

namespace EZ_IDE
{
    public partial class Project_Settings_Form : Form
    {
        ProjectSettings projectSettings = new ProjectSettings();
        PropertyGridProjectSettings grid;
        public Project_Settings_Form(ProjectSettings ProjectSettings)
        {
            InitializeComponent();
            grid = new PropertyGridProjectSettings(projectSettings);
            this.projectSettings = ProjectSettings;
            propertyGrid1.SelectedObject = grid;
        }

        private void Project_Settings_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            projectSettings = new ProjectSettings()
            {
                FileInError = grid.FileInError,
                ShowBuild = grid.ShowBuild,
                IsVisual = grid.IsVisual,
                CloseOnEnd = grid.CloseOnEnd,
                Debug = grid.Debug,
                ClearConsole = grid.ClearConsole,
                Window = grid.Window,
                Name = grid.Name,
                Icon = grid.Icon,
                StartUp = grid.StartUp,
                Files_Code = grid.Files_Code,
            };
        }
    }
    class PropertyGridProjectSettings
    {
        private ProjectSettings projectSettings;
        public PropertyGridProjectSettings(ProjectSettings projectSettings)
        {
            this.projectSettings = projectSettings;

            FileInError = projectSettings.FileInError;
            ShowBuild = projectSettings.ShowBuild;
            IsVisual = projectSettings.IsVisual;
            CloseOnEnd = projectSettings.CloseOnEnd;
            Debug = projectSettings.Debug;
            ClearConsole = projectSettings.ClearConsole;
            Window = projectSettings.Window;
            Name = projectSettings.Name;
            Icon = projectSettings.Icon;
            StartUp = projectSettings.StartUp;


        }
        [Category("Project Settings")]
        [DisplayName("Display File In Errors")]
        [Description("Shows the file that the error occured in when displaying the error. Default true")]
        public bool FileInError { get; set; } = true;

        [Category("Project Settings")]
        [DisplayName("Show Start and End")]
        [Description("Shows 'Build Started' and 'Build Ended' when the build starts and ends. Default false")]
        public bool ShowBuild { get; set; } = false;

        [Category("Project Settings")]
        [DisplayName("Is Visual")]
        [Description("If the program has a visual output that is not a window. Default false")]
        public bool IsVisual { get; set; } = false;

        [Category("Project Settings")]
        [DisplayName("Close On End")]
        [Description("Close the window when the program ends. Only effects the program if a window is open. Default true")]
        public bool CloseOnEnd { get; set; } = true;

        [Category("Project Settings")]
        [DisplayName("Debug Window")]
        [Description("Displays Debug window if 'Window' is true. Default false")]
        public bool Debug { get; set; } = false;

        [Category("Project Settings")]
        [DisplayName("Clear Console")]
        [Description("Clear the console before each build. Default true")]
        public bool ClearConsole { get; set; } = true;

        [Category("Project Settings")]
        [DisplayName("Window Program")]
        [Description("Tells the program that windows will be displayed during the progam. Default false")]
        public bool Window { get; set; } = false;

        [Category("Project Properties")]
        [DisplayName("Name")]
        [Description("Name of the Program")]
        public string Name { get; set; }

        [Category("Project Properties")]
        [DisplayName("Icon Path")]
        [Description("Path to the icon of the Program (*.ico).")]
        public string Icon { get; set; }

        [Category("Files")]
        [DisplayName("Start Up File Path")]
        [Description("Entry point for the program. If left blank, one will be created.")]
        public string StartUp { get; set; }

        [Category("Files")]
        [DisplayName("Additional Files")]
        [Description("Additional files required for the program.")]
        public AdditionalFiles[] Files_Code_Visual
        {
            get => _Files_Code_Visual;
            set
            {
                _Files_Code_Visual = value;

                Files_Code = ConvertToCode(projectSettings.Directory, _Files_Code_Visual.Select(x => x.ToString()).ToArray());
            }
        }
        private AdditionalFiles[] _Files_Code_Visual = new AdditionalFiles[0];
        [NonSerialized]
        public string[] Files_Code = new string[0];
        public static string[] ConvertToCode(string dir, string[] allfiles)
        {

            List<string> indir = new List<string>();
            List<string> notindir = new List<string>();
            List<string> file_code = new List<string>();
            string[] file_code_visual = allfiles;
            foreach (string file in file_code_visual)
            {
                if (FileInDirectory(file, dir))
                {
                    indir.Add(file);
                }
                else
                {
                    notindir.Add(file);
                }
            }
            for (int i = 0; i < indir.Count; i++)
            {
                string localPath = Path.Combine(dir, indir[i]);
                indir[i] = $"~\\{localPath}";
            }
            foreach (string file in notindir)
            {
                file_code.Add($"include:\"{file}\"");
            }
            string[] dirFile = Directory.GetFiles(dir, "*.ezcode", SearchOption.AllDirectories);
            if (dirFile == indir.ToArray())
            {
                file_code.Add("include:\"all\"");
            }
            else
            {
                List<string> notfiles = new List<string>();
                foreach (string file in dirFile)
                {
                    if (!indir.Contains(file))
                    {
                        notfiles.Add(file);
                    }
                }
                if (indir.Count > notfiles.Count / 2)
                {
                    notfiles.Add("include:\"all\"");
                    foreach (string file in notfiles)
                    {
                        notfiles.Add($"exclude:\"{file}\"");
                    }
                }
                else
                {
                    foreach (string file in indir)
                    {
                        notfiles.Add($"include:\"{file}\"");
                    }
                }
            }
            return file_code.ToArray();
        }
        public static AdditionalFiles[] ConvertFromCode(string code)
        {
            List<AdditionalFiles> files = new List<AdditionalFiles>();



            return files.ToArray();
        }
        public static bool FileInDirectory(string file, string directory)
        {
            if (!Directory.Exists(directory))
            {
                MessageBox.Show("Directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string fullPath = Path.GetFullPath(file);

            string fullDirectoryPath = Path.GetFullPath(directory);

            return fullPath.StartsWith(fullDirectoryPath, StringComparison.OrdinalIgnoreCase);
        }
    }
    public class AdditionalFiles
    {
        private FileInfo fileInfo;

        public AdditionalFiles()
        {
            try
            {
                fileInfo = ChooseFile();
            }
            catch
            {

            }
        }

        public AdditionalFiles(string filePath)
        {
            fileInfo = new FileInfo(filePath);
        }
        [Category("File")]
        [Description("Path to the file")]
        public string FilePath
        {
            get { return fileInfo.FullName; }
            set { fileInfo = new FileInfo(value); }
        }

        public FileInfo ChooseFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a File";
                openFileDialog.Filter = "EZCode|*.ezcode";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return new FileInfo(openFileDialog.FileName);
                }
            }
            return fileInfo;
        }
        public override string ToString()
        {
            return fileInfo.FullName;
        }
    }
}
