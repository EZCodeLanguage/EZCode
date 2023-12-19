using Microsoft.VisualBasic.Devices;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;

namespace EZ_IDE
{
    public partial class Project_Settings_Form : Form
    {
        public ProjectSettings projectSettings;
        public PropertyGridProjectSettings grid;
        public Project_Settings_Form(ProjectSettings ProjectSettings)
        {
            InitializeComponent();
            this.projectSettings = ProjectSettings;
            grid = new PropertyGridProjectSettings(projectSettings);
            propertyGrid1.SelectedObject = grid;

            textBox1.Text = projectSettings.ConverToCode();
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

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
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
            textBox1.Text = projectSettings.ConverToCode();
        }
    }
    public class PropertyGridProjectSettings
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
            if (projectSettings.StartUp != "") _StartUp = projectSettings.StartUp;
            Files_Code = projectSettings.Files_Code;
            Files_Code_Visual = ConvertFromCode(projectSettings.Files_Code, projectSettings.Directory, StartUpNotLocal);
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
        [DisplayName("Icon")]
        [Description("Icon for the Program")]
        public IconFile[] Icon_Info
        {
            get => _Icon_Info;
            set
            {
                _Icon_Info = value;
                Icon = value?[0].FilePath.Replace(projectSettings.Directory, "~");
            }
        }

        private IconFile[] _Icon_Info;
        [NonSerialized] public string Icon;

        [NonSerialized] public string StartUp = "";
        private string StartUpNotLocal = "";
        private string _StartUp
        {
            get
            {
                if (Files_Code_Visual.Select(x => x.IsStartup).Count() > 0)
                {
                    for (int i = 0; i < Files_Code_Visual.Select(x=>x.IsStartup).Count(); i++)
                    {
                        if (i != 0)
                        {
                            Files_Code_Visual.Select(x => x.IsStartup = false);
                        }
                    }
                }
                return StartUp;
            }
            set
            {
                string file = value.Replace("\"", "").Trim();
                if (file.Contains("~\\") || file.Contains("~/"))
                {
                    int indexOfTarget = file.IndexOf('~');

                    if (indexOfTarget != -1)
                    {
                        string resultString = file.Substring(indexOfTarget + 1);
                        resultString = resultString.Replace("~/", "").Replace("~\\", "").Replace("/", "\\");
                        file = projectSettings.Directory + resultString;
                    }
                }

                StartUp = file.Replace(projectSettings.Directory, "~");
                StartUpNotLocal = file;
                if (Files_Code_Visual.Select(x=>x.FilePath).Contains(value))
                {
                    Files_Code_Visual.First(x=>x.IsStartup == true).FilePath = value;
                }
                else
                {
                    Files_Code_Visual = Files_Code_Visual.Append(new AdditionalFiles(value, true)).ToArray();
                }
            }
        }

        [Category("Files")]
        [DisplayName("Additional Files")]
        [Description("Additional files required for the program.")]
        public AdditionalFiles[] Files_Code_Visual
        {
            get => _Files_Code_Visual;
            set
            {
                var distinctFiles = value.DistinctBy(x => x.FilePath).ToArray();

                _Files_Code_Visual = distinctFiles;

                foreach (var file in _Files_Code_Visual)
                {
                    if (file.IsStartup)
                    {
                        _StartUp = file.FilePath;
                    }
                }

                string[] temp_files = new string[0];

                foreach (var Ad_file in _Files_Code_Visual)
                {
                    string file = Ad_file.FilePath.Replace("\"", "").Trim();
                    if (file.Contains("~\\") || file.Contains("~/"))
                    {
                        int indexOfTarget = file.IndexOf('~');

                        if (indexOfTarget != -1)
                        {
                            string resultString = file.Substring(indexOfTarget + 1);
                            resultString = resultString.Replace("~\\", "").Replace("~/", "");
                            file = projectSettings.Directory + resultString;
                        }
                    }
                    else if (file.EndsWith("all") && file.Contains(AppDomain.CurrentDomain.BaseDirectory))
                    {
                        file = "all";
                    }
                    temp_files = temp_files.Append(file.ToString()).ToArray();
                }

                Files_Code = ConvertToCode(projectSettings.Directory, temp_files, StartUpNotLocal);
            }
        }
        private AdditionalFiles[] _Files_Code_Visual = new AdditionalFiles[0];
        [NonSerialized]
        public string[] Files_Code = new string[0];
        public static string[] ConvertToCode(string dir, string[] allfiles, string startupFullPath)
        {
            try
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
                    int startupInt = -1;
                    foreach (string file in dirFile)
                    {
                        if (!indir.Contains(file))
                        {
                            notfiles.Add(file);
                        }
                    }
                    if (startupFullPath != "")
                    {
                        notfiles.Add(startupFullPath);
                        startupInt = notfiles.IndexOf(startupFullPath);
                    }
                    for (int i = 0; i < indir.Count; i++)
                    {
                        indir[i] = indir[i].Replace(dir, "~");
                    }
                    for (int i = 0; i < notfiles.Count; i++)
                    {
                        notfiles[i] = notfiles[i].Replace(dir, "~");
                    }
                    if (indir.Count - (startupFullPath != "" ? 1 : 0) > notfiles.Count / 2)
                    {
                        file_code.Add("include:\"all\"");
                        //for (int i = 0; i < notfiles.Count; i++)
                        //{
                        //    file_code.Add($"exclude:\"{notfiles[i]}\"{(i == startupInt ? " // Excludes because already included in 'Startup' property" : "")}");
                        //}
                    }
                    else
                    {
                        foreach (string file in indir)
                        {
                            if(file != startupFullPath.Replace(dir, "~"))
                                file_code.Add($"include:\"{file}\"");
                        }
                    }
                }
                return file_code.ToArray();
            }
            catch
            {
                return new string[0];
            }
        }
        public static AdditionalFiles[] ConvertFromCode(string[] code, string directory, string startup)
        {
            List<AdditionalFiles> files = new List<AdditionalFiles>();
            try
            {
                List<string> files_code = new List<string>();
                foreach (var line in code)
                {
                    string before = line.Split(":")[0];
                    string after = string.Join(":", line.Split(":").Skip(1)).Replace("\"", "").Trim();
                    if (after.Contains("~\\") || after.Contains("~/"))
                    {
                        after = after.Replace("~\\", "").Replace("~/", "");
                        after = Path.Combine(directory, after);
                    }
                    if (before == "include" || before == "exclude")
                    {
                        if (after == "all")
                        {
                            if (before == "include")
                                foreach (string file in Directory.GetFiles(directory, "*.ezcode", SearchOption.AllDirectories))
                                    files_code.Add(file);
                            else if (before == "exclude")
                                foreach (string file in Directory.GetFiles(directory, "*.ezcode", SearchOption.AllDirectories))
                                    files_code.Remove(file);
                        }
                        else
                        {
                            if (before == "include")
                                files_code.Add(after);
                            else if (before == "exclude")
                                files_code.Remove(after);
                        }
                    }
                }

                foreach (var file in files_code)
                {
                    files.Add(new AdditionalFiles(file));
                }

                if (startup != "")
                {
                    files.Add(new AdditionalFiles(startup, true));
                }
            }
            catch
            {

            }
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
    [Category("File")] public class AdditionalFiles
    {
        private FileInfo fileInfo;
        [Description("If this file is the main entry point for the program. If none, one will be created")]
        public bool IsStartup { get; set; } = false;

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
        public AdditionalFiles(bool val)
        {
            IsStartup = val;
        }
        public AdditionalFiles(string filePath, bool val)
        {
            fileInfo = new FileInfo(filePath);
            IsStartup = val;
        }
        [Description("Path to the file")]
        public string FilePath
        {
            get { return fileInfo != null ? fileInfo.FullName : ""; }
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
            return FilePath;
        }
    }
    public class IconFile
    {
        private FileInfo fileInfo;

        public IconFile()
        {
            fileInfo = ChooseFile();
        }

        public IconFile(string filePath)
        {
            fileInfo = new FileInfo(filePath);
        }

        [Category("Project Properties")]
        [DisplayName("Icon Path")]
        [Description("Path to the icon of the Program (*.ico).")]
        public string FilePath
        {
            get { return fileInfo?.FullName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    fileInfo = null;
                }
                else
                {
                    fileInfo = new FileInfo(value);
                }
            }
        }

        public static FileInfo ChooseFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a File";
                openFileDialog.Filter = "Icon|*.ico";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return new FileInfo(openFileDialog.FileName);
                }
            }
            return null;
        }

        public override string ToString()
        {
            return fileInfo?.FullName;
        }
    }
}
