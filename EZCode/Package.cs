using Newtonsoft.Json;

namespace EZCodeLanguage
{
    public static class Package
    {
        internal static string Extension = ".cache.json";
        internal static string Folder = "cache";
        public static string PackagesDirectory = "D:\\EZCodeLanguage\\Packages\\";
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
            Parser parser = new Parser();
            if (project.Configuration.Cache) parser = OpenCache(project.Files.Select(x => Path.Combine(pack_dir, x)).ToArray());

            // CACHE SYSTEM NOT WORKING PROPERLY

            if (parser.Classes.Count == 0 && parser.Methods.Count == 0 && parser.LinesWithTokens.Length == 0)
                parser.Parse(string.Join("\n\n// End of File\n\n", project.Files.Select(x => File.ReadAllText(Path.Combine(pack_dir, x)))));

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
        public static string SaveCache(string file, Parser parser)
        {
            if (parser.LinesWithTokens == null || parser.LinesWithTokens.Length == 0)
                parser.Parse();

            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Name == "package.json")
            {
                Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(fileInfo.FullName));
                Parser parse = OpenCache(project.Files.Select(x => Path.Combine(fileInfo.DirectoryName, x)).ToArray());

                if (parse.Classes.Count == 0 && parse.Methods.Count == 0 && parse.LinesWithTokens.Length == 0)
                    parse.Parse(string.Join("\n\n// End of File\n\n", project.Files.Select(x => File.ReadAllText(Path.Combine(fileInfo.DirectoryName, x)))));

                parser = parse;
            }

            string cache = JsonConvert.SerializeObject(parser, Formatting.Indented);
            string path = Path.Combine(fileInfo.DirectoryName, Folder, fileInfo.Directory.Name) + Extension; 
            if (!Directory.Exists(Path.Combine(fileInfo.DirectoryName, Folder))) Directory.CreateDirectory(Path.Combine(fileInfo.DirectoryName, Folder));
            File.WriteAllText(path, cache);
            return cache;
        }
        public static Parser OpenCache(string file, params string[] files) => 
            OpenCache(files.Prepend(file).ToArray());
        public static Parser OpenCache(string[] files)
        {
            Parser parser = new Parser();

            for (int i = 0; i < files.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(files[i]);
                if (!fileInfo.Exists) continue;

                Parser cache;

                if (fileInfo.Name == "package.json")
                {
                    Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(fileInfo.FullName));
                    cache = OpenCache(project.Files.Select(x=> Path.Combine(fileInfo.DirectoryName, x)).ToArray());

                    if (cache.Classes.Count == 0 && cache.Methods.Count == 0 && cache.LinesWithTokens.Length == 0) 
                        cache.Parse(string.Join("\n\n// End of File\n\n", project.Files.Select(x => File.ReadAllText(Path.Combine(fileInfo.DirectoryName, x)))));

                    CombineParsers(parser, cache);
                }

                FileInfo Read = new FileInfo(Path.Combine(fileInfo.DirectoryName, Folder, Path.GetFileNameWithoutExtension(fileInfo.Name) + Extension));
                if (!Read.Exists) continue;

                cache = JsonConvert.DeserializeObject<Parser>(File.ReadAllText(Read.FullName));

                parser = CombineParsers(parser, cache);
            }
            return parser;
        }
        public static Parser CombineParsers(Parser parser, Parser p2)
        {
            parser.Code += p2.Code + "\n\n// End of File";
            parser.LinesWithTokens = [.. parser.LinesWithTokens, .. p2.LinesWithTokens];
            parser.Classes = [.. parser.Classes, .. p2.Classes];
            parser.Methods = [.. parser.Methods, .. p2.Methods];

            return parser;
        }
    }
}
