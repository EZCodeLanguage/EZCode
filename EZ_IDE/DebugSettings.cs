using EZCode.Debug;
using System.ComponentModel;
using System.IO;
using Debugger = EZCode.Debug.Debugger;
using static EZ_IDE.Settings;
using Microsoft.Win32;


namespace EZ_IDE
{
    public static class DebugSave
    {
        public static string[] FilePaths
        {
            get
            {
                string[]? val = GetArrayKey(nameof(FilePaths));
                return val != null ? val : new string[0];
            }
            set => SetArrayKey(nameof(FilePaths), value);
        }
        public static string[] Methods
        {
            get
            {
                string[]? val = GetArrayKey(nameof(Methods), false);
                return val != null ? val : new string[0];
            }
            set => SetArrayKey(nameof(Methods), value);
        }
        public static bool[] RemoveFileOnHit
        {
            get
            {
                string[]? val = GetArrayKey(nameof(RemoveFileOnHit));
                val ??= new string[0];
                bool[] b = new bool[0];
                for (int i = 0; i < val.Length; i++)
                {
                    try
                    {
                        b = b.Append(BoolParse(val[i]) == true).ToArray();
                    }
                    catch
                    {
                        b = b.Append(false).ToArray();
                    }
                }
                return b;
            }
            set => SetArrayKey(nameof(RemoveFileOnHit), value.Select(x => x.ToString()).ToArray());
        }
        public static bool[] Enables
        {
            get
            {
                string[]? val = GetArrayKey(nameof(Enables));
                val ??= new string[0];
                bool[] b = new bool[0];
                for (int i = 0; i < val.Length; i++)
                {
                    try
                    {
                        b = b.Append(BoolParse(val[i]) == true).ToArray();
                    }
                    catch
                    {
                        b = b.Append(false).ToArray();
                    }
                }
                return b;
            }
            set => SetArrayKey(nameof(Enables), value.Select(x => x.ToString()).ToArray());
        }
        public static int[] Segments
        {
            get
            {
                string[]? val = GetArrayKey(nameof(Segments));
                val ??= new string[0];
                int[] b = new int[0];
                for (int i = 0; i < val.Length; i++)
                {
                    try
                    {
                        b = b.Append(int.Parse(val[i])).ToArray();
                    }
                    catch
                    {
                        b = b.Append(0).ToArray();
                    }
                }
                return b;
            }
            set => SetArrayKey(nameof(Segments), value.Select(x => x.ToString()).ToArray());
        }
        public static Breakpoint[] Breakpoints
        {
            get
            {
                Breakpoint[] breaks = new Breakpoint[0];
                int length = 0;
                if (FilePaths.Length == RemoveFileOnHit.Length && RemoveFileOnHit.Length == Segments.Length && Segments.Length == Methods.Length && Methods.Length == Enables.Length)
                    length = FilePaths.Length;
                else Breakpoints = new Breakpoint[0];

                for (int i = 0; i < length; i++)
                {
                    breaks = breaks.Append(new Breakpoint(FilePaths[i], Segments[i], RemoveFileOnHit[i], Methods[i], Enables[i])).ToArray();
                }
                return breaks;
            }
            set
            {
                FilePaths = value.Select(x => x.FilePath).ToArray();
                RemoveFileOnHit = value.Select(x => x.RemoveOnFirstHit).ToArray();
                Segments = value.Select(x => x.Segment).ToArray();
                Methods = value.Select(x => x.Method).ToArray();
                Enables = value.Select(x => x.Enabled).ToArray();
            }
        }
        public static void StartUp(IDE ide)
        {
            ide.debugSettings.Breakpoints = Breakpoints;
        }
    }
    public class DebugSettings
    {
        public DebugSettings()
        {
            Set();
        }
        public DebugSettings(Breakpoint[] breakpoints)
        {
            _Breakpoints = breakpoints;
        }
        public void Save()
        {
            DebugSave.Breakpoints = Breakpoints;
        }
        public void Save(Breakpoint[] breakpoints)
        {
            DebugSave.Breakpoints = breakpoints;
        }
        public void Set()
        {
            Settings_Breakpoint[] breaks = new Settings_Breakpoint[0];
            foreach (var b in DebugSave.Breakpoints)
            {
                breaks = breaks.Append(new Settings_Breakpoint(b.FilePath, b.Segment, b.RemoveOnFirstHit, b.Method, b.Enabled)).ToArray();
            }
            Settings_Breakpoints = breaks;
        }
        [NonSerialized] public Breakpoint[] Breakpoints;
        private Breakpoint[] _Breakpoints
        {
            get
            {
                return Breakpoints;
            }
            set
            {
                Breakpoints = value;
            }
        }
        [Category("Breakpoints")]
        [Description("Breakpoints for Debug file")]
        public Settings_Breakpoint[] Settings_Breakpoints
        {
            get
            {
                return _settings_Breakpoints;
            }
            set
            {
                Breakpoint[] breaks = new Breakpoint[0];
                foreach (var b in value)
                {
                    breaks = breaks.Append(new Breakpoint(b.FilePath, b.Segment, b.RemoveOnFirstHit, b.Method, b.Enabled)).ToArray();
                }
                _Breakpoints = breaks;
                _settings_Breakpoints = value;

            }
        }
        private Settings_Breakpoint[] _settings_Breakpoints = new Settings_Breakpoint[0];
    }
    public class Settings_Breakpoint
    {
        [Description("If the Breakpoint is enabled")]
        public bool Enabled { get; set; } = true;
        [Description("Removes the breakpoint when it gets hit")]
        public bool RemoveOnFirstHit { get; set; } = false;
        [Description("Method in the breakpoint")]
        public string Method { get; set; }
        [Description("File assosiated with the breakpoint")]
        public FileInfo Fileinfo { get; set; }
        public string FilePath 
        {
            get 
            {
                return Fileinfo != null ? Fileinfo.FullName : "";
            }
            set
            {
                Fileinfo = new FileInfo(value);
            } 
        }
        [Description("Segment for the breakpoint to be hit")]
        public int Segment { get; set; }
        public Settings_Breakpoint()
        {
            Fileinfo = ChooseFile();
        }
        
        public Settings_Breakpoint(string file, int segment, bool remove, string method, bool enabled)
        {
            Segment = segment;
            FilePath = file;
            RemoveOnFirstHit = remove;
            Method = method;
            Enabled = enabled;
        }
        public Settings_Breakpoint(string file, int segment, bool remove, string method)
        {
            Segment = segment;
            FilePath = file;
            RemoveOnFirstHit = remove;
            Method = method;
        }
        public Settings_Breakpoint(string file, int segment, bool remove)
        {
            Segment = segment;
            FilePath = file;
            RemoveOnFirstHit = remove;
        }
        public Settings_Breakpoint(string file, int segment)
        {
            Segment = segment;
            FilePath = file;
        }
        public Settings_Breakpoint(string file, bool remove)
        {
            RemoveOnFirstHit = remove;
            FilePath = file;
        }
        public Settings_Breakpoint(string file)
        {
            FilePath = file;
        }
        public Settings_Breakpoint(bool removeOnFirstHit)
        {
            RemoveOnFirstHit = removeOnFirstHit;
        }
        public Settings_Breakpoint(int segment)
        {
            Segment = segment;
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
            return Fileinfo;
        }
        public override string ToString()
        {
            return FilePath;
        }
    }
}
