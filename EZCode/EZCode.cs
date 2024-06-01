namespace EZCodeLanguage
{
    public class EZCode
    {
        public static string Version = "3.0.0-beta";
        public static void RunFileWithMain(string file)
        {
            Parser parser = new Parser(new FileInfo(Path.GetFullPath(file)));
            parser = Package.ReturnParserWithPackages(parser, ["main"]);
            Interpreter interpreter = new Interpreter(parser);
            interpreter.Interperate();
        }
        public static void RunFile(string file)
        {
            Parser parser = new Parser(new FileInfo(Path.GetFullPath(file)));
            parser.Parse();
            Interpreter interpreter = new Interpreter(parser);
            interpreter.Interperate();
        }
        public static void DebugFile(Debug.Breakpoint[] breakpoints, string file)
        {
            Parser parser = new Parser(new FileInfo(Path.GetFullPath(file)));
            parser.Parse();
            Interpreter interpreter = new Interpreter(parser, breakpoints);
            interpreter.Interperate();
        }
        public static void RunCode(string code, string path = "Running from inside program")
        {
            Parser parser = new Parser(code, path);
            parser.Parse();
            Interpreter interpreter = new Interpreter(parser);
            interpreter.Interperate();
        }
        public static void RunCodeWithMain(string code, string path = "Running from inside program")
        {
            Parser parser = new Parser(code, path);
            parser = Package.ReturnParserWithPackages(parser, ["main"]);
            Interpreter interpreter = new Interpreter(parser);
            interpreter.Interperate();
        }
        public static void DebugCodeWithMain(Debug.Breakpoint[] breakpoints, string code, string path = "Running from inside program")
        {
            Parser parser = new Parser(code, path);
            parser = Package.ReturnParserWithPackages(parser, ["main"]);
            Interpreter interpreter = new Interpreter(parser, breakpoints);
            interpreter.Interperate();
        }
        public static void RunProject(string path)
        {
            ProjectClass project = Project.GetProjectFromPath(path);
            Project.Run(project);
        }
        public static void DebugProject(Debug.Breakpoint[] breakpoints, string path)
        {
            ProjectClass project = Project.GetProjectFromPath(path);
            Parser par = Project.GetParserFromProject(project);
            Interpreter interpreter = new Interpreter(par, breakpoints);
            interpreter.Interperate();
        }
    }
}
