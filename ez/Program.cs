using EZCodeLanguage;
using System.IO.Compression;
using System.Net;

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
                    
                    help              Writes all of the possible commands
                    version           Writes the current version of EZCode installed
                    run [CODE]        Runs a line of code. 'main' Package is already imported
                    start             Starts an EZCode environment
                    install [NAME]    Installs a package from github repo https://github.com/EZCodeLanguage/Packages.git
                    uninstall [NAME]  Uninstalls the package
                    new [TYPE]        Creates an empty [Project], [Package], [Class] in the directory
                    [FILEPATH]        Runs file
                    """);
                else
                {
                    error_code = 1;
                    Console.WriteLine("Error, expected nothing after 'help' command");
                }
                break;

            case "new":
                string type = args[1].ToLower();
                if (args.Length > 2)
                {
                    error_code = 1;
                    Console.WriteLine($"Error, unexpected '{string.Join(" ", args.Skip(1))}'");
                }
                string defaultMainFileName, defaultMainFile, defaultMainFileNameExtension, projectPath, currentDirectory = Directory.GetCurrentDirectory();
                int fileIndex = 0;
                switch (type)
                {
                    case "package":
                        defaultMainFileName = "project";
                        defaultMainFileNameExtension = ".ezcode";
                        defaultMainFile = """
                            method printHelloWorld {
                                print Hello World
                            }
                            """;
                        string defaultProjectFileName = "package";
                        string defaultProjectFileNameExtension = ".json";
                        string defaultProjectFile = """
                            {
                                "Name":"project",
                                "Files": [
                                    "project.ezcode"
                                ],
                                "Configuration":{
                                    "GlobalPackages":[
                                        "main"
                                    ]
                                }
                            }
                            """;
                        projectPath = Path.Combine(currentDirectory, defaultProjectFileName, defaultProjectFileName + defaultProjectFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            fileIndex++;
                            projectPath = Path.Combine(currentDirectory, defaultProjectFileName + fileIndex, defaultProjectFileName + defaultProjectFileNameExtension);
                        }
                        defaultMainFileName = Path.Combine(defaultProjectFileName + (fileIndex != 0 ? fileIndex : ""), defaultMainFileName);
                        string parentDirectory = Directory.GetParent(projectPath).FullName;
                        Directory.CreateDirectory(parentDirectory);
                        File.WriteAllText(projectPath, defaultProjectFile);
                        break;

                    case "project":
                        defaultMainFileName = "project";
                        defaultMainFileNameExtension = ".ezcode";
                        defaultMainFile = """
                            method start {
                                print Hello World!
                            }
                            """;
                        break;
                    case "class":
                        defaultMainFileName = "example-class";
                        defaultMainFileNameExtension = ".ezcode";
                        defaultMainFile = """
                            class example-type {
                                explicit params => set : PARAMS
                                undefined Value 
                                method set : val {
                                    Value => format : val
                                }
                                get => @str {
                                    return Value
                                }
                            }
                            /* Example Use:
                             * example-type name new : Hello World!
                             */

                            class example-object {
                                undefined x => 0
                                undefined y => 50
                                undefined z => 123
                            }
                            /* Example Use:
                             * example-object name new : x:50, z:90
                             */
                            """;
                        break;
                    default:
                        Console.WriteLine($"Error, unexpected '{type}' Valid types: 'project', 'package', or 'class'");
                        error_code = 1;
                        Environment.Exit(error_code);
                        return;
                }

                fileIndex = 0;
                projectPath = Path.Combine(currentDirectory, defaultMainFileName + defaultMainFileNameExtension);
                while (File.Exists(projectPath))
                {
                    projectPath = Path.Combine(currentDirectory, defaultMainFileName + fileIndex + defaultMainFileNameExtension);
                    fileIndex++;
                }

                File.WriteAllText(projectPath, defaultMainFile);
                break;

            case "i":
            case "install":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error, Expected package name");
                    error_code = 1;
                    break;
                }
                string package = args[1];

                if (args.Length > 2)
                {
                    Console.WriteLine($"Error, unexpected '{string.Join(" ", args.Skip(1))}'");
                    error_code = 1;
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Starting Install of '{package}' package");
                Console.ResetColor();

                string downloadFilePath = Path.Combine(Package.PackagesDirectory, package),
                    downloadUrl = $"https://github.com/EZCodeLanguage/Packages/releases/download/{package}-package/{package}.zip";

                try
                {
                    InstallAndExtract(downloadUrl, downloadFilePath);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Install Complete");
                }
                catch (Exception ex)
                {
                    error_code = 1;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(ex.Message);
                }
                Console.ResetColor();
                break;

            case "u":
            case "uninstall":
                if (args.Length < 2)
                {
                    Console.WriteLine("Error, Expected package name");
                    error_code = 1;
                    break;
                }
                package = args[1];

                if (args.Length > 2)
                {
                    Console.WriteLine($"Error, unexpected '{string.Join(" ", args.Skip(1))}'");
                    error_code = 1;
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Starting uninstalling of '{package}' package");
                Console.ResetColor();

                try
                {
                    downloadFilePath = Path.Combine(Package.PackagesDirectory, package);
                    if (!Directory.Exists(downloadFilePath))
                    {
                        throw new Exception($"Error, Package '{package}' can't be found");
                    }
                    else
                    {
                        Directory.Delete(downloadFilePath, true);
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Uninstall Complete");
                }
                catch (Exception ex)
                {
                    error_code = 1;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(ex.Message);
                }
                Console.ResetColor();
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
    static void InstallAndExtract(string downloadUrl, string downloadFilePath)
    {
        using (WebClient webClient = new WebClient())
        {
            string tempDownloadFilePath = downloadFilePath + "_.zip";

            webClient.DownloadFile(downloadUrl, tempDownloadFilePath);

            ZipFile.ExtractToDirectory(tempDownloadFilePath, downloadFilePath, true);

            File.Delete(tempDownloadFilePath);
        }
    }

    static void EZEnvironment(string[] contents = null)
    {
        string help = "Commands: " +
                "END to end environment, " +
                "RUN to run program, " +
                "RESTART to restart environment, " +
                "LIST to list all line in the program, " +
                "BACK to remove line before it, " +
                "and CLEAR to clear the screen, ";
        if (contents == null)
        {
            Console.WriteLine("EZCode Environment started. 'main' Package already included");
            Console.WriteLine(help);
        }
        bool again = false, restart = false;
        contents ??= [];
        for (int i = 0; i < 100000; i++)
        {
            if (i <= 100000) throw new Exception("Error, out of bounds. 99999 line limit");
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
            else if (line == "HELP")
            {
                Console.WriteLine(help);
                continue;
            }
            contents = [.. contents, line];
        }
        if (again) EZEnvironment(contents);
        if (restart) EZEnvironment();
    }
}