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
        public string Name { get; set; }
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
                RawIcon = new Icon(value);
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
                DirectoryInfo = new DirectoryInfo(value);
            }
        }
        public DirectoryInfo DirectoryInfo;
        public string StartUp { get; set; }
        public string[] Files_Code { get; set; }
        public ProjectSettings() { }

        public void Initialize(ref EzCode code, Panel? visualoutput = null, RichTextBox? output = null)
        {
            code.Initialize(!Window, Directory, visualoutput != null ? visualoutput : new Panel(), output != null ? output : new RichTextBox(), FileInError, ShowBuild, ClearConsole);
        }

        public string ConverToCode()
        {
            string code = $"name:\"{Name}\"\nstartup:\"{StartUp}\"\nfileinerror:\"{FileInError}\"\nshowbuild:\"{ShowBuild}\"\nisvisual:\"{IsVisual}\"\ncloseonend:\"{CloseOnEnd}\"\ndebug:\"{Debug}\"\nclearconsole:\"{ClearConsole}\"\nwindow:\"{Window}\"{(Icon != "" || Icon != null ? $"\nicon:\"{Icon}\"" : "")}";
            foreach (var file in Files_Code)
            {
                code += file + "\n";
            }

            return code;
        }
        public void ConvertFromCode(string code)
        {
            EZProj e = new EZProj(new EzCode() { Code = code });
            FileInError = e.FileInErrors;
            ShowBuild = e.ShowBuild;
            IsVisual = e.IsVisual;
            CloseOnEnd = e.CloseOnEnd;
            Debug = e.Debug;
            ClearConsole = e.ClearConsole;
            Window = e.Window;
            Icon = e.IconPath;

            string[] lines = code.Split(new[] { '|', '\n' });
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
        }
    }
}
