using EZCode.Variables;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using static EZCode.EzCode;

namespace EZCode
{
    public class EZProj
    {
        /// <summary>
        /// The path of the EZProj file
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// The contents of the EZProj file
        /// </summary>
        public string FileContents { get; private set; }
        /// <summary>
        /// The new ezcode file
        /// </summary>
        public string Program { get; private set; }
        /// <summary>
        /// any errors that occured during the <see cref="ReadConvert(string?)"/> Process
        /// </summary>
        public string[] Errors { get; private set; }
        /// <summary>
        /// is the opposite of <see cref="EzCode.InPanel"/>
        /// </summary>
        public bool Window { get; set; }
        /// <summary>
        /// is <see cref="EzCode.showFileInError"/>
        /// </summary>
        public bool FileInErrors { get; set; }
        /// <summary>
        /// is <see cref="EzCode.showStartAndEnd"/>
        /// </summary>
        public bool ShowBuild { get; set; }
        /// <summary>
        /// is visual or just console
        /// </summary>
        public bool IsVisual { get; set; }
        /// <summary>
        /// Clear console before each build
        /// </summary>
        public bool ClearConsole { get; set; }
        /// <summary>
        /// Name of Project
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Icon for project
        /// </summary>
        public string IconPath { get; set; }
        /// <summary>
        /// Icon for project
        /// </summary>
        public Icon Icon { get; set; }
        /// <summary>
        /// Debugs project
        /// </summary>
        public bool Debug { get; set; }
        /// <summary>
        /// close the application on end
        /// </summary>
        public bool CloseOnEnd { get; set; }

        /// <summary>
        /// Empty Instance of <see cref="EZProj"/>.
        /// </summary>
        public EZProj() { }
        public EZProj(EzCode code)
        {
            string[] lines = code.Code.Split("\n");
            string[] parts = new string[0];
            int index = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("# project properties :") || lines[i].Trim().StartsWith("#project properties :"))
                {
                    if (lines[i].Trim().StartsWith("# project properties :")) index = 4;
                    else if (lines[i].Trim().StartsWith("#project properties :")) index = 3;
                    parts = lines[i].Split(" ");
                }
            }
            string file = "";
            if (parts.Length != 0)
            {
                string[] commas = string.Join(" ", parts.Skip(index)).Split(",");
                foreach (string comma in commas)
                {
                    string before = comma.Split(":")[0];
                    string after = string.Join(":", comma.Split(":").Skip(1));
                    file += $"{before}:\"{after}\"".Trim() + Environment.NewLine;
                }
            }
            Program = ReadConvert(file);
            Program += "\n" + code.Code;
        }
        /// <summary>
        /// New instanse of <see cref="EZProj"/>. 
        /// </summary>
        /// <param name="file">The ezproj file contents being read as a <seealso cref="string"/></param>
        public EZProj(string file)
        {
            FilePath = file;
            FileContents = File.ReadAllText(file);
            Program = ReadConvert();
        }
        /// <summary>
        /// Initiats and fills out all of the variables: <see cref="FileContents"/>, <see cref="Program"/>, and <see cref="Errors"/>
        /// </summary>
        public EZProj(string contents, string filepath)
        {
            FilePath = filepath;
            FileContents = contents;
            Program = ReadConvert();
        }
        /// <summary>
        /// Initiats and fills out all of the variables: <see cref="FileContents"/>, <see cref="Program"/>, and <see cref="Errors"/>
        /// </summary>
        /// <param name="file">The ezproj file contents being read as a <seealso cref="FileInfo"/></param>
        public EZProj(FileInfo file, string filepath = "")
        {
            FilePath = filepath;
            FileContents = File.ReadAllText(file.FullName);
            Program = ReadConvert();
        }
        /// <summary>
        /// Initiats and fills out all of the variables: <see cref="FileContents"/>, <see cref="Program"/>, and <see cref="Errors"/>
        /// </summary>
        /// <param name="file">The ezproj file contents being read as a <seealso cref="Stream"/></param>
        public EZProj(Stream file, string filepath = "")
        {
            FilePath = filepath;
            StreamReader sr = new StreamReader(file);
            FileContents = sr.ReadToEnd();
            sr.Close();
            Program = ReadConvert();
        }

        /// <summary>
        /// Converts the ezproj file to one big ezcode file
        /// </summary>
        /// <param name="_filecontent">The ezproj file contents being read as a <seealso cref="string"/></param>. Not needed if <see cref="FileContents"/> is filled out.
        /// <![CDATA[
        /// "name":"C:/complete/file.path"    // reads file and sets a local proj var to value
        /// "name":value                      // sets var to specific var
        /// 
        /// startup:"C:/complete/file.path"   // sets the start up file to the specified path
        /// startup:"~/local/file.path"       // sets the start up file to the local specified path
        /// startup:"local var name"          // reads variable and includes the variable's value (assumed valid file)
        /// 
        /// include:"C:/complete/file.path"   // includes file from the specified path
        /// include:"~/local/file.path"       // includes file from the local specified path
        /// include:"all"                     // includes all files in proj's directory
        /// include:"~/local/directory/"      // includes all files in specified local directory
        /// include:"C:/complete/directory/"  // includes all files in specified directory
        /// include:"local var name"          // reads variable and includes the variable's value (assumed valid file)
        /// 
        /// exclude:"C:/complete/file.path"   // excludes file from the specified path
        /// exclude:"~/local/file.path"       // excludes file from the local specified path
        /// exclude:"all"                     // excludes all files in proj's directory
        /// exclude:"~/local/directory/"      // excludes all files in specified local directory
        /// exclude:"C:/complete/directory/"  // excludes all files in specified directory
        /// exclude:"local var name"          // reads variable and includes the variable's value (assumed valid file)
        /// 
        /// window:"boolean"                  // sets the 'In Panel' value to the opposite of specified value
        /// window:"default"                  // sets the 'In Panel' value to the opposite of defualt (true)
        /// window:"local var name"           // reads variable and includes the variable's value (assumed boolean)
        /// 
        /// showbuild:"boolean"               // sets the 'Show Start and End' value to specified value
        /// showbuild:"default"               // sets the 'Show Start and End' value to defualt (true)
        /// showbuild:"local var name"        // reads variable and includes the variable's value (assumed boolean)
        /// 
        /// fileinerror:"boolean"             // sets the 'File In Error' value to specified value
        /// fileinerror:"default"             // sets the 'File In Error' value to defualt (true)
        /// fileinerror:"local var name"      // reads variable and includes the variable's value (assumed boolean)
        /// 
        /// // Commented Text                 // Text that will be ignored by the Converter
        /// ]]>
        /// <returns>string that contains ezcode. <seealso cref="Program"/></returns>
        public string ReadConvert(string? _filecontent = null)
        {
            ClearConsole = true;
            IsVisual = false;
            ShowBuild = false;
            FileInErrors = true;
            Window = false;
            Debug = false;
            CloseOnEnd = true;
            Name ??= $"EZCode_v{EzCode.Version}";
            IconPath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EZCode", "EZCode", "EZCode_Logo.ico");

            string filecontent = FileContents;
            if (_filecontent != null) filecontent = _filecontent;

            string ezcode = "";
            errors = new List<string>();

            string[] lines = filecontent.Split(EzCode.seperatorChars).Select(x => x.Trim()).Where(a => a != "").ToArray();

            lineindex = 0;
            HashSet<string> files = new HashSet<string>();
            string startup = string.Empty;
            foreach (string line in lines)
            {
                try
                {
                    lineindex++;
                    string[] parts = line.Split(":");
                    string keyword = parts[0];
                    string value = string.Join(":", parts.Skip(1)).Trim();
                    value = string.Join(" ", value.Split(" ").TakeWhile(x => !x.Contains("//")));
                    switch (keyword)
                    {
                        case "startup":
                        case "include":
                        case "exclude":
                            if (isvar(value))
                            {
                                value = getvar(value);
                            }
                            string v = getfile(value, true, false);
                            value = v;
                            if ((Directory.Exists(value) || value == "all" || value == "folder") && keyword != "startup")
                            {
                                if (keyword == "include")
                                    foreach (string file in getdirectory(value))
                                    {
                                        files.Add(file);
                                    }
                                else if (keyword == "exclude")
                                {
                                    foreach (string file in getdirectory(value))
                                    {
                                        files.Remove(file);
                                    }
                                }
                            }
                            else
                            {
                                if (validfile(value, true))
                                {
                                    if (keyword == "include") files.Add(value);
                                    else if (keyword == "exclude") files.Remove(value);
                                    else if (keyword == "startup")
                                    {
                                        //files.Add(value);
                                        startup = value;
                                    }
                                }
                                else
                                {
                                    errors.Add($"'{value}' does not exist or is not a valid file path in {SegmentSeperator} {lineindex}");
                                }
                            }
                            break;
                        case "name":
                        case "icon":
                            if (isvar(value))
                            {
                                value = getvar(value);
                            }
                            v = insidemarks(value, false);
                            if (keyword == "name") Name = v;
                            else if (keyword == "icon" && validfile(v, true))
                            {
                                IconPath = v;
                                Icon = new Icon(v);
                            }
                            else
                            {
                                errors.Add($"'{value}' does not exist or is not a valid file path in {SegmentSeperator} {lineindex}");
                            }
                            break;
                        case "fileinerror":
                        case "showbuild":
                        case "isvisual":
                        case "closeonend":
                        case "debug":
                        case "window":
                        case "clearconsole":
                            if (isvar(value))
                            {
                                value = getvar(value);
                            }
                            bool? check = Var.staticReturnBool(insidemarks(value, false));
                            if (value == @"""default""") check = keyword == "fileinerror" || keyword == "clearconsole" || keyword == "closeonend" ? true : false;
                            if (check == null) errors.Add($"'{value}' is not a boolean in {SegmentSeperator} {lineindex}");
                            switch (keyword)
                            {
                                case "fileinerror": FileInErrors = check == true; break;
                                case "showbuild": ShowBuild = check == true; break;
                                case "window": Window = check == true; break;
                                case "isvisual": IsVisual = check == true; break;
                                case "clearconsole": ClearConsole = check == true; break;
                                case "debug": Debug = check == true; break;
                                case "closeonend": CloseOnEnd = check == true; break;
                            }
                            break;
                        default:
                            if (keyword.StartsWith("//")) break;
                            v = insidemarks(keyword, false);
                            if (keyword == v)
                                errors.Add($"'{keyword}' is an unexpected keyword in {SegmentSeperator} {lineindex}");
                            else
                            {
                                string _o = getfile(insidemarks(value, false), false);
                                string a = !value.StartsWith(@"""") ? value : File.ReadAllText(_o);
                                Var var = new Var(v, a);
                                vars.Add(var);
                            }
                            break;
                    }
                }
                catch
                {
                    errors.Add($"An error occured in {SegmentSeperator} {lineindex}");
                }
            }
            Errors = errors.ToArray();


            if (startup != "") ezcode += $"#current file {startup}\n\n";
            ezcode += $"//Project Properties\n#project properties : name:{Name}, icon:{IconPath}, fileinerror:{FileInErrors}, showbuild:{ShowBuild}, window:{Window}, isvisual:{IsVisual}, clearconsole:{ClearConsole}, debug:{Debug}, closeonend:{CloseOnEnd}\n";
            if (startup != "") ezcode += $"\n//{new FileInfo(startup).Name}\n{File.ReadAllText(startup)}\n";

            files.Remove(startup);
            foreach (string file in files)
            {
                ezcode += $"\n//{new FileInfo(file).Name}\n#current file {file}\n{File.ReadAllText(file)}\n";
            }

            return ezcode;
        }

        /// <summary>
        /// Checks if inputted string is a valid path and exists
        /// </summary>
        public static bool validfile(string path, bool exists = false)
        {
            path = path.Replace("/", "\\");
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (string.IsNullOrWhiteSpace(path) || path.Length < 3)
            {
                return false;
            }

            if (!driveCheck.IsMatch(path.Substring(0, 3)))
            {
                return false;
            }
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
            {
                return false;
            }
            return exists ? File.Exists(path) : true;
        }


        //Private Variabes
        private List<string> errors = new List<string>();
        private int lineindex = 0;
        private List<Var> vars = new List<Var>();

        //Private Methods
        private bool isvar(string name)
        {
            string? nomarks = insidemarks(name);
            if (nomarks != name)
            {
                return vars.Any(x => x.Name == nomarks);
            }
            else if (nomarks == null)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
        private string getfile(string path, bool inside = true, bool err = true)
        {
            if (inside) path = insidemarks(path, err);
            if (path.StartsWith("~/") || path.StartsWith("~\\"))
            {
                FileInfo fileinfo = new FileInfo(FilePath);
                DirectoryInfo directoryinfo = fileinfo.Directory;
                string filename = path.Replace("~\\", "").Replace("~/", "");
                string _path = Path.Combine(directoryinfo.FullName, filename);
                path = _path;
            }
            return path;
        }
        private string[] getdirectory(string path)
        {
            if (path == "all" || path == "folder")
            {
                string _p = Path.GetDirectoryName(FilePath);
                return Directory.GetFiles(_p, "*.ezcode", path == "all" ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            path = getfile(path, false);
            return Directory.GetFiles(path, "*.ezcode", SearchOption.AllDirectories);
        }
        private string? getvar(string name, bool innermarks = true)
        {
            if (innermarks) name = insidemarks(name);
            return vars.FirstOrDefault(x => x.Name == name, null).Value;
        }
        private string? insidemarks(string text, bool err = true)
        {
            if (text.StartsWith("\"") && text.EndsWith("\""))
            {
                int count = text.Count(c => c == '"');

                if (count == 2) return text.Replace("\"", "");
            }

            if (err) errors.Add($"'{text}' is not a valid keyword in {EzCode.SegmentSeperator} {lineindex}");
            return text;
        }
    }
}