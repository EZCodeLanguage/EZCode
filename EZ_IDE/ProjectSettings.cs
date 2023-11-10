using EZCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_IDE
{
    public static class ProjectSettings
    {
        static bool _FileInError = true;
        public static bool FileInError
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
        static bool _ShowBuild = false;
        public static bool ShowBuild
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
        static bool _IsVisual = false;
        public static bool IsVisual
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
        static bool _CloseOnEnd = true;
        public static bool CloseOnEnd
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
        static bool _Debug = false;
        public static bool Debug
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
        static bool _ClearConsole = true;
        public static bool ClearConsole
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
        static bool _Window = false;
        public static bool Window
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
        static bool _EZProj = false;
        public static bool EZProj
        {
            get
            {
                return _EZProj;
            }
            set
            {
                _EZProj = value;
            }
        }
        public static string Name = "";
        static string _Icon = "";
        public static string Icon 
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
        public static Icon RawIcon;

        public static string _Directory = "";
        public static string Directory
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
        public static DirectoryInfo DirectoryInfo;

        public static void Initialize(ref EzCode code, Panel? visualoutput = null, RichTextBox? output = null)
        {
            code.Initialize(!Window, Directory, visualoutput != null ? visualoutput : new Panel(), output != null ? output : new RichTextBox(), FileInError, ShowBuild, ClearConsole);
        }
    }
}
