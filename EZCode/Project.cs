using Newtonsoft.Json;

namespace EZCodeLanguage
{
    public class ProjectClass
    {
        public string Name { get; set; }
        public string[] Files { get; set; }
        public string[]? GlobalPackages { get; set; }
    }
    public static class Project
    {
        public static ProjectClass GetProjectFromPath(string path)
        {
            string content = File.ReadAllText(path);
            ProjectClass project = JsonConvert.DeserializeObject<ProjectClass>(content);
            return project;
        }
        public static Parser GetParserFromProject(ProjectClass project, string directory)
        {
            Parser[] parsers = new Parser[project.Files.Length];
            for (int i = 0; i < parsers.Length; i++)
            {
                parsers[i] = new Parser(new FileInfo(Path.Combine(directory, project.Files[i])));
                parsers[i].Parse();
            }
            Parser p = Package.CombineParsers(parsers);
            return IncludeGlobalPackages(p, project);
        }
        public static Parser IncludeGlobalPackages(Parser parser, ProjectClass project)
        {
            Parser[] parsers = new Parser[project.GlobalPackages.Length];
            for (int i = 0; i < parsers.Length; i++)
            {
                parsers[i] = Package.GetPackageAsParser(project.GlobalPackages[i]);
                parsers[i].Parse();
            }
            parsers = [parser, .. parsers];
            return Package.CombineParsers(parsers);
        }
        public static void Run(ProjectClass project, string directory)
        {
            Parser par = GetParserFromProject(project, directory);
            Interpreter interpreter = new Interpreter(par);
            interpreter.Interperate();
        }
    }
}
