using EZCodeLanguage;

static class EZ
{
    public static int error_code = 0;
    public static void Main(string[] args)
    {

        if (args.Length == 0)
        {
            args = ["help"];
        }

        switch (args[0])
        {
            case "help":
                if (args.Length == 1)
                    Console.WriteLine("""
                    All commands:
                    
                    help          Writes all of the possible commands
                    version       Writes the current version of EZCode installed
                    run [CODE]    Runs a line of code. 'main' Package is already imported
                    start         Starts an EZCode environment
                    [FILEPATH]    Runs file
                    """);
                else
                {
                    error_code = 1;
                    Console.WriteLine("Error, expected nothing after 'help' command");
                }
                break;
            case "version":
                if (args.Length == 1)
                    Console.WriteLine(Interpreter.Version);
                else
                {
                    error_code = 1;
                    Console.WriteLine("Error, expected nothing after 'version' command");
                }
                break;
            case "run":
                Parser parser = new Parser(string.Join(" ", args.Skip(1).ToArray()), AppDomain.CurrentDomain.BaseDirectory);
                parser = Package.ReturnParserWithPackages(parser, ["main"]);
                parser.Parse();
                Interpreter interpreter = new Interpreter(parser);
                interpreter.Interperate();
                break;
            case "start":
                try
                {
                    EZEnvironment();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    error_code = 1;
                }
                break;
            default:
                string path = string.Join(" ", args);
                string contents;
                try
                {
                    contents = File.ReadAllText(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    error_code = 1;
                    break;
                }
                parser = new Parser(contents, path);
                parser.Parse();
                interpreter = new Interpreter(parser);
                interpreter.Interperate();
                break;
        }

        Environment.Exit(error_code);
    }
    static void EZEnvironment(string[] contents = null)
    {
        if (contents == null)
        {
            Console.WriteLine("EZCode Environment started. 'main' Package already included");
            Console.WriteLine("Commands: " +
                "END to end environment, " +
                "RUN to run program, " +
                "RESTART to restart environment, " +
                "LIST to list all line in the program, " +
                "BACK to remove line before it, " +
                "and CLEAR to clear the screen, ");
        }
        bool again = false, restart = false;
        contents ??= [];
        for (int i = 0; i < 100000; i++)
        {
            if (i == 100000) throw new Exception("Error, out of bounds. 99999 line limit");
            string line = Console.ReadLine();
            if (line == "RUN")
            {
                Parser parser = new Parser(string.Join(Environment.NewLine, contents), AppDomain.CurrentDomain.BaseDirectory + "ez.exe");
                parser = Package.ReturnParserWithPackages(parser, ["main"]);
                parser.Parse();
                Interpreter interpreter = new Interpreter(parser);
                interpreter.Interperate();
                again = true;
                Console.WriteLine("RUN ENDED");
                Console.WriteLine();
                break;
            }
            else if (line == "END")
            {
                return;
            }
            else if (line == "RESTART")
            {
                restart = true;
                break;
            }
            else if (line == "LIST")
            {
                Console.WriteLine(string.Join(Environment.NewLine, contents));
                Console.WriteLine("LIST ENDED");
                continue;
            }
            else if (line == "CLEAR")
            {
                Console.Clear();
                continue;
            }
            else if (line == "BACK")
            {
                contents = contents.Where((x, y) => y != i - 1).ToArray();
                continue;
            }
            contents = [.. contents, line];
        }
        if (again) EZEnvironment(contents);
        if (restart) EZEnvironment();
    }
}