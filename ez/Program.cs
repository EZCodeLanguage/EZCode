using EZCodeLanguage;
using System.IO.Compression;
using System.Net;

// Release command:
// dotnet clean ; dotnet publish -o "out/ez" ez/ez.csproj -r win-x64 -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained ; dotnet publish -o "out/installer" Installer/Installer.csproj -r win-x64 -c Release -p:PublishReadyToRun=true -p:PublishSingleFile=true --self-contained
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
                    
                    help                Writes all of the possible commands
                    version             Writes the current version of EZCode installed
                    run [CODE]          Runs a line of code. 'main' Package is already imported
                    start               Starts an EZCode environment
                    install [NAME]      Installs a package from github repo https://github.com/EZCodeLanguage/Packages.git
                    uninstall [NAME]    Uninstalls the package
                    new [TYPE] [NAME]   Creates an empty [Project], [Package], [Class] in the directory
                    [FILEPATH]          Runs file
                    """);
                else
                {
                    error_code = 1;
                    Console.WriteLine("Error, expected nothing after 'help' command");
                }
                break;

            case "new":
                if (args.Length > 3)
                {
                    error_code = 1;
                    Console.WriteLine($"Error, unexpected '{string.Join(" ", args.Skip(3))}' Please use '-' or '_' instead of spaces");
                    break;
                }
                if (args.Length < 3)
                {
                    error_code = 1;
                    Console.WriteLine($"Error, expected syntax 'ez [TYPE] [NAME]");
                    break;
                }
                string type = args[1].ToLower();
                string name = args[2];
                 
                string mainFileName, mainFileContents, mainFileNameExtension, projectPath, currentDirectory = Directory.GetCurrentDirectory();
                int fileIndex = 0;
                switch (type)
                {
                    case "package":
                        mainFileName = name;
                        mainFileNameExtension = ".ezcode";
                        mainFileContents = """
                            method printHelloWorld { 
                                print Hello World 
                            }
                            """;
                        string projectDirectoryName = name;
                        string projectFileName = "package";
                        string projectFileNameExtension = ".json";
                        string ProjectFileContents = @"{
    ""$schema"":""https://raw.githubusercontent.com/EZCodeLanguage/Packages/main/ezcode.package.schema.json"",
    ""Name"":""" + name + @""",
    ""Files"": [
        """ + name + @".ezcode""
    ],
    ""Configuration"":{
        ""GlobalPackages"":[
            ""main""
        ]
    }
}";
                        projectPath = Path.Combine(currentDirectory, projectDirectoryName, projectFileName + projectFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            fileIndex++;
                            projectPath = Path.Combine(currentDirectory, projectDirectoryName + fileIndex, projectFileName + projectFileNameExtension);
                        }
                        mainFileName = Path.Combine(projectDirectoryName + (fileIndex != 0 ? fileIndex : ""), mainFileName);
                        string parentDirectory = Directory.GetParent(projectPath).FullName;
                        Directory.CreateDirectory(parentDirectory);
                        File.WriteAllText(projectPath, ProjectFileContents);

                        fileIndex = 0;
                        projectPath = Path.Combine(currentDirectory, mainFileName + mainFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            projectPath = Path.Combine(currentDirectory, mainFileName + fileIndex + mainFileNameExtension);
                            fileIndex++;
                        }

                        File.WriteAllText(projectPath, mainFileContents);
                        break;
                    case "project":
                        mainFileName = "entry";
                        mainFileNameExtension = ".ezcode";
                        mainFileContents = """
                            method start { 
                                print Hello World 
                            }
                            """;
                        projectDirectoryName = name;
                        projectFileName = "project";
                        projectFileNameExtension = ".json";
                        ProjectFileContents = @"{
    ""$schema"":""https://raw.githubusercontent.com/EZCodeLanguage/Projects/main/ezcode.project.schema.json"",
    ""Name"":""" + name + @""",
    ""Files"": [
        ""entry.ezcode""
    ],
    ""GlobalPackages"":[
        ""main""
    ]
}";
                        projectPath = Path.Combine(currentDirectory, projectDirectoryName, projectFileName + projectFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            fileIndex++;
                            projectPath = Path.Combine(currentDirectory, projectDirectoryName + fileIndex, projectFileName + projectFileNameExtension);
                        }
                        mainFileName = Path.Combine(projectDirectoryName + (fileIndex != 0 ? fileIndex : ""), mainFileName);
                        parentDirectory = Directory.GetParent(projectPath).FullName;
                        Directory.CreateDirectory(parentDirectory);
                        File.WriteAllText(projectPath, ProjectFileContents); 

                        fileIndex = 0;
                        projectPath = Path.Combine(currentDirectory, mainFileName + mainFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            projectPath = Path.Combine(currentDirectory, mainFileName + fileIndex + mainFileNameExtension);
                            fileIndex++;
                        }

                        File.WriteAllText(projectPath, mainFileContents);
                        break;
                    case "class":
                        mainFileName = "name";
                        mainFileNameExtension = ".ezcode";
                        mainFileContents = @"
class " + name + @"-example-type {
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
    * " + name + @"-example-type name new : Hello World!
    */

class " + name + @"-example-object {
    undefined x => 0
    undefined y => 50
    undefined z => 123
}
/* Example Use:
    * " + name + @"-example-object name new : x:50, z:90
    */
";

                        fileIndex = 0;
                        projectPath = Path.Combine(currentDirectory, mainFileName + mainFileNameExtension);
                        while (File.Exists(projectPath))
                        {
                            projectPath = Path.Combine(currentDirectory, mainFileName + fileIndex + mainFileNameExtension);
                            fileIndex++;
                        }

                        File.WriteAllText(projectPath, mainFileContents);
                        break;
                    default:
                        Console.WriteLine($"Error, unexpected '{type}' Valid types: 'project', 'package', or 'class'");
                        error_code = 1;
                        Environment.Exit(error_code);
                        return;
                }
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
                    Console.WriteLine(EZCode.Version);
                else
                {
                    error_code = 1;
                    Console.WriteLine("Error, expected nothing after 'version' command");
                }
                break;
            case "run":
                EZCode.RunCodeWithMain(string.Join(" ", args.Skip(1).ToArray()), AppDomain.CurrentDomain.BaseDirectory);
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
                if (Path.GetExtension(path) == ".json")
                {
                    EZCode.RunProject(path, Path.GetDirectoryName(path));
                }
                EZCode.RunCodeWithMain(contents, path);
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
        for (int i = 0; i < 10000; i++)
        {
            if (i >= 10000) throw new Exception("Error, out of bounds. 99999 line limit");
            string line = Console.ReadLine();
            if (line == "RUN")
            {
                EZCode.RunCodeWithMain(string.Join(Environment.NewLine, contents), AppDomain.CurrentDomain.BaseDirectory + "ez.exe");
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