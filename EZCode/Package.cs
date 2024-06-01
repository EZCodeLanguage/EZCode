using Newtonsoft.Json;

namespace EZCodeLanguage
{
    public class PackageClass
    {
        public string Name { get; set; }
        public string[] Files { get; set; }
        public Config? Configuration { get; set; }
        public class Config
        {
            public string? LibraryDirectory { get; set; }
            public string[]? GlobalPackages { get; set; }
        }
    }

    public static class Package
    {
        public static string PackagesDirectory = "D:\\EZCodeLanguage\\Packages\\";
        public static void AddPackageToExecutionDirectory(PackageClass project, string executionDirectory)
        {
            string projectPath = GetPackageDirectory(project.Name);
            string libraryDirectory = Path.Combine(projectPath, project.Configuration?.LibraryDirectory ?? throw new Exception("Project.LibraryDirectory is null"));
            string executionSubDirectoryName = project.Configuration.LibraryDirectory.Split(['/', '\\']).First();
            string executionSubDirectory = Path.Combine(executionDirectory, executionSubDirectoryName);

            Directory.CreateDirectory(executionSubDirectory);

            string[] libraryFiles = Directory.GetFiles(libraryDirectory, "*");
            foreach (string libraryFile in libraryFiles)
            {
                string libraryFileName = Path.GetFileName(libraryFile);
                string executionSubDirectoryFile = Path.Combine(executionSubDirectory, libraryFileName);
                File.Copy(libraryFile, executionSubDirectoryFile);
            }
        }
        public static void RemovePackageFromExecutionDirectory(PackageClass project, string executionDirectory)
        {
            string libraryDirectory = project.Configuration?.LibraryDirectory ?? throw new Exception("Project.LibraryDirectory is null");
            string executionSubDirectory = Path.Combine(executionDirectory, libraryDirectory);

            Directory.Delete(executionSubDirectory, true);
        }
        public static void RemoveAllPackagesFromExecutionDirectory(string executionDirectory)
        {
            string[] directories = Directory.GetDirectories(executionDirectory);
            foreach (string directory in directories)
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch
                {

                }
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
        public static PackageClass GetPackageAsProject(string package_name)
        {
            return JsonConvert.DeserializeObject<PackageClass>(File.ReadAllText(GetPackageFile(package_name)));
        }
        public static Parser GetPackageAsParser(string package_name)
        {
            string file = GetPackageFile(package_name);
            string pack_dir = GetPackageDirectory(package_name);
            PackageClass project = JsonConvert.DeserializeObject<PackageClass>(File.ReadAllText(file));
            string[] global_packages = project.Configuration != null ? project.Configuration.GlobalPackages ?? [] : [];
            Parser[] parsers = [];
            foreach (var pack in global_packages)
            {
                Parser p = GetPackageAsParser(pack);
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
            parser.Classes = parser.Classes.DistinctBy(x => x.Name).ToList();
            parser.Methods = parser.Methods.DistinctBy(x => x.Name).ToList();
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
