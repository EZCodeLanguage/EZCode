using Newtonsoft.Json;

namespace EZCodeLanguage
{
    public static class Package
    {
        public static string PackagesDirectory = "D:\\EZCodeLanguage\\Packages\\";
        public static string LibraryDirName = "Libraries";
        public static void AddPackageToExecutionDirectory(Project project, string executionDirectory, out string[] files)
        {
            files = [];
            string projectPath = GetPackageDirectory(project.Name);
            string libraryDirectory = Path.Combine(projectPath, project.LibraryDirectory ?? throw new Exception("Project.LibraryDirectory is null"));
            string executionSubDirectoryName = LibraryDirName;
            string executionSubDirectory = Path.Combine(executionDirectory, executionSubDirectoryName);

            Directory.CreateDirectory(executionSubDirectory);

            string[] libraryFiles = Directory.GetFiles(libraryDirectory, "*");
            foreach (string libraryFile in libraryFiles)
            {
                string libraryFileName = Path.GetFileName(libraryFile);
                string executionSubDirectoryFile = Path.Combine(executionSubDirectory, libraryFileName);
                File.Copy(libraryFile, executionSubDirectoryFile);
                files = [.. files, executionSubDirectoryFile];
            }
        }
        public static void RemovePackageFromExecutionDirectory(string[] projectFileNames)
        {
            foreach(string file in projectFileNames)
            {
                File.Delete(file);
            }
        }
        public static void RemoveAllPackagesFromExecutionDirectory(string executionDirectory)
        {
            try
            {
                string executionSubDirectory = Path.Combine(executionDirectory, LibraryDirName);
                string[] files = Directory.GetFiles(executionSubDirectory);
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
        public static string GetPackageDirectory(string package_name)
        {
            return Path.Combine(PackagesDirectory, package_name);
        }
        public static string GetPackageFile(string package_name)
        {
            return Path.Combine(PackagesDirectory, package_name, "package.json");
        }
        public static Project GetPackageAsProject(string package_name)
        {
            return JsonConvert.DeserializeObject<Project>(File.ReadAllText(GetPackageFile(package_name)));
        }
        public static Parser GetPackageAsParser(string package_name)
        {
            string file = GetPackageFile(package_name);
            string pack_dir = GetPackageDirectory(package_name);
            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(file));
            string[] global_packages = project.Configuration != null ? project.Configuration.GlobalPackages ?? [] : [];
            Parser[] parsers = [];
            foreach (var pack in global_packages)
            {
                string path = GetPackageFile(pack);
                string contents = File.ReadAllText(path);
                Parser p = new Parser(contents, path);
                p.Parse();
                parsers = [.. parsers, p];
            }
            foreach (string f in project.Files)
            {
                string path = Path.Combine(pack_dir, f);
                string contents = File.ReadAllText(path);
                Parser p = new Parser(contents, path);
                p.Parse();
                parsers = [.. parsers, p];
            }
            Parser parser = CombineParsers(parsers);

            if (parser.Classes.Count == 0 && parser.Methods.Count == 0 && parser.LinesWithTokens.Length == 0)
                parser.Parse();

            return parser;
        }
        public static Parser ReturnParserWithPackages(Parser parser, string[] package_names)
        {
            for (int i = 0; i < package_names.Length; i++)
            {
                Parser p = GetPackageAsParser(package_names[i]);
                parser = CombineParsers(parser, p);
            }
            return parser;
        }
        public static Parser RemovePackageFromParser(Parser main_parser, Parser remove)
        {
            main_parser.Classes = main_parser.Classes.Where(x => !remove.Classes.Any(y => y.Name == x.Name)).ToList();
            main_parser.Methods = main_parser.Methods.Where(x => !remove.Methods.Any(y => y.Name == x.Name)).ToList();
            return main_parser;
        }
        public static Parser CombineParsers(Parser[] parsers)
        {
            Parser parser = parsers.First();
            foreach(var p in parsers.Skip(1))
            {
                parser = CombineParsers(parser, p);
            }
            return parser;
        }
        public static Parser CombineParsers(Parser parser, Parser p2)
        {
            parser.Code += "\n\n//NextFile\n\n" + p2.Code + "\n\n";
            parser.LinesWithTokens = [.. parser.LinesWithTokens, .. p2.LinesWithTokens];
            parser.Classes = [.. parser.Classes, .. p2.Classes];
            parser.Methods = [.. parser.Methods, .. p2.Methods];

            return parser;
        }
    }
}
