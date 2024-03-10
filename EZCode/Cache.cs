using Newtonsoft.Json;

namespace EZCodeLanguage
{
    public static class Cache
    {
        internal static string Extension = ".cache.json";
        internal static string Folder = "cache";
        public static string SaveCache(string file, Parser parser)
        {
            if (parser.Tokens == null || parser.Tokens.Length == 0)
                parser.Tokenize();

            FileInfo fileInfo = new FileInfo(file);
            if (fileInfo.Name == "package.json")
            {
                Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(fileInfo.FullName));
                Parser parse = OpenCache(project.Files.Select(x => Path.Combine(fileInfo.DirectoryName, x)).ToArray());

                if (parse.Classes.Count == 0 && parse.Methods.Count == 0 && parse.Containers.Count == 0 && parse.Tokens.Length == 0)
                    parse.Tokenize(string.Join("\n\nEnd of File\n\n", project.Files.Select(x => File.ReadAllText(Path.Combine(fileInfo.DirectoryName, x)))));

                parser = parse;
            }

            string cache = JsonConvert.SerializeObject(parser, Formatting.Indented);
            string path = Path.Combine(fileInfo.DirectoryName, Folder, Path.GetFileNameWithoutExtension(file)) + Extension; 
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

                    if (cache.Classes.Count == 0 && cache.Methods.Count == 0 && cache.Containers.Count == 0 && cache.Tokens.Length == 0) 
                        cache.Tokenize(string.Join("\n\nEnd of File\n\n", project.Files.Select(x => File.ReadAllText(Path.Combine(fileInfo.DirectoryName, x)))));

                    goto SetParser;
                }

                FileInfo Read = new FileInfo(Path.Combine(fileInfo.DirectoryName, Folder, Path.GetFileNameWithoutExtension(fileInfo.Name) + Extension));
                if (!Read.Exists) continue;

                cache = JsonConvert.DeserializeObject<Parser>(File.ReadAllText(Read.FullName));

                SetParser:
                    parser.Code += cache.Code + "\n\n//End of File";
                    parser.Tokens = [.. parser.Tokens, .. cache.Tokens];
                    parser.Classes = [.. parser.Classes, .. cache.Classes];
                    parser.Methods = [.. parser.Methods, .. cache.Methods];
                    parser.Containers = [.. parser.Containers, .. cache.Containers];
            }
            return parser;
        }
    }
}
