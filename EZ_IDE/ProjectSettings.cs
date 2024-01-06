using EZCode;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_IDE
{
    public partial class ProjectSettings
    {
        bool _FileInError = true;
        public bool FileInError
        {
            get
            {
                return _FileInError;
            }
            set
            {
                _FileInError = value;
            }
        }
        bool _ShowBuild = false;
        public bool ShowBuild
        {
            get
            {
                return _ShowBuild;
            }
            set
            {
                _ShowBuild = value;
            }
        }
        bool _IsVisual = false;
        public bool IsVisual
        {
            get
            {
                return _IsVisual;
            }
            set
            {
                _IsVisual = value;
            }
        }
        bool _CloseOnEnd = true;
        public bool CloseOnEnd
        {
            get
            {
                return _CloseOnEnd;
            }
            set
            {
                _CloseOnEnd = value;
            }
        }
        bool _Debug = false;
        public bool Debug
        {
            get
            {
                return _Debug;
            }
            set
            {
                _Debug = value;
            }
        }
        bool _ClearConsole = true;
        public bool ClearConsole
        {
            get
            {
                return _ClearConsole;
            }
            set
            {
                _ClearConsole = value;
            }
        }
        bool _Window = false;
        public bool Window
        {
            get
            {
                return _Window;
            }
            set
            {
                _Window = value;
            }
        }
        public string Name { get; set; } = "";
        string _Icon = "";
        public string Icon 
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
                try
                {
                    RawIcon = new Icon(value);
                }
                catch
                {

                }
            }
        }
        public Icon RawIcon;

        string _Directory = "";
        public string Directory
        {
            get
            {
                return _Directory;
            }
            set
            {
                _Directory = value;
                try
                {
                    DirectoryInfo = new DirectoryInfo(value);
                }
                catch
                {

                }
            }
        }
        public DirectoryInfo DirectoryInfo;
        public string StartUp { get; set; } = "";
        public string[] Files_Code { get; set; } = new string[0];
        public ProjectSettings() { }

        public void Initialize(ref EzCode code, Panel? visualoutput = null, RichTextBox? output = null)
        {
            code.Initialize(!Window, Directory, visualoutput != null ? visualoutput : new Panel(), output != null ? output : new RichTextBox(), FileInError, ShowBuild, ClearConsole);
        }

        public string ConverToCode()
        {
            string startup = StartUp.Replace("\"", "").Trim();
            string code = $"{(Name != "" ? $"name:\"{Name}\"{Environment.NewLine}" : "")}{(!FileInError ? $"fileinerror:\"{FileInError}\"{Environment.NewLine}" : "")}{(ShowBuild ? $"showbuild:\"{ShowBuild}\"{Environment.NewLine}" : "")}{(IsVisual ? $"isvisual:\"{IsVisual}\"{Environment.NewLine}" : "")}{(!CloseOnEnd ? $"closeonend:\"{CloseOnEnd}\"{Environment.NewLine}" : "")}{(Debug ? $"debug:\"{Debug}\"{Environment.NewLine}" : "")}{(!ClearConsole ? $"clearconsole:\"{ClearConsole}\"{Environment.NewLine}" : "")}{(Window ? $"window:\"{Window}\"{Environment.NewLine}" : "")}{(!(Icon == EZProj.DefaultIconPath || Icon == "") ? $"icon:\"{Icon}\"{Environment.NewLine}" : "")}{(startup != "" ? $"startup:\"{startup}\"" : "")}";
            foreach (var file in Files_Code)
            {
                code += Environment.NewLine + file;
            }

            return code;
        }
        public ProjectSettings ConvertFromCode(string projcode)
        {
            string code = "# project properties : ";
            string[] code_lines = projcode.Split(new[] { '|', '\n' }).Select(x => x.Trim()).Where(y=>y != "").ToArray();
            for (int i = 0; i < code_lines.Length; i++)
            {
                if (code_lines[i].StartsWith("include:") || code_lines[i].StartsWith("exclude:") || code_lines[i].StartsWith("startup:"))
                    continue;

                string property = code_lines[i].Replace("\"", "");
                code += property + (i != code_lines.Length - 1 ? ", " : "");
            }
            EZProj e = new EZProj(new EzCode() { Code = code });
            FileInError = e.FileInErrors;
            ShowBuild = e.ShowBuild;
            IsVisual = e.IsVisual;
            CloseOnEnd = e.CloseOnEnd;
            Debug = e.Debug;
            ClearConsole = e.ClearConsole;
            Window = e.Window;
            Icon = e.IconPath;
            Name = e.Name;

            string[] lines = projcode.Split(new[] { '|', '\n' });
            List<string> files_code = new List<string>();
            foreach (var line in lines)
            {
                string before = line.Split(":")[0];
                string after = string.Join(":", line.Split(":").Skip(1));
                if (before == "startup") StartUp = after;
                if (before == "include" || before == "exclude")
                    files_code.Add(line);
            }
            Files_Code = files_code.ToArray();
            return this;
        }
    }
}
