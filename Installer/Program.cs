using EZCodeLanguage;
using Microsoft.Win32;
using System.IO.Compression;
using System.Net;

class Program
{
    private static readonly string owner = "EZCodeLanguage";
    private static readonly string repo = "EZCode";
    private static readonly string assetName = "EZCodeLanguage.zip";
    private static readonly string extractPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "EZCodeLanguage");
    private static readonly string directoryToAddToPath = extractPath + "\\";
    public class Option
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Checked { get; set; }

        public Option(string name, int id, bool check)
        {
            Id = id;
            Name = name;
            Checked = check;
        }
    }

    static async Task Main(string[] args)
    {
        try
        {
            List<Option> options = new List<Option>()
            {
                new Option($" - Install latest EZCode Version", 0, true),
                new Option(" - Uninstall EZCode", 1, false),
            };

            int index = 0;

            ConsoleKey key;

            string txt1 = "EZCode Installer";
            string txt2 = "Use the arrow keys to select an option, press enter to submit.";

            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(txt1);
                Console.ResetColor();
                Console.WriteLine(txt2);

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }

                    Console.WriteLine($"{options[i].Name}");

                    Console.ResetColor();
                }

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        index = index == 0 ? options.Count - 1 : index - 1;
                        for (int i = 0; i < options.Count; i++)
                        {
                            options[i].Checked = false;
                        }
                        options[index].Checked = true;
                        break;
                    case ConsoleKey.DownArrow:
                        index = index == options.Count - 1 ? 0 : index + 1;
                        for (int i = 0; i < options.Count; i++)
                        {
                            options[i].Checked = false;
                        }
                        options[index].Checked = true;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(" is invalid input. Please use the arrow keys or enter.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
                Console.Clear();

            } while (key != ConsoleKey.Enter);

            foreach (var option in options)
            {
                if (!option.Checked) continue;
                switch (option.Id)
                {
                    case 0: // Install
                        Console.WriteLine("Starting Installation");
                        await Install();
                        Console.WriteLine("Finished installation");
                        break;
                    case 1: // Uninstall
                        Console.WriteLine("Starting Uninstall");
                        Uninstall();
                        Console.WriteLine("Finished Uninstalling");
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ResetColor();
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    private static void Uninstall()
    {
        Directory.Delete(extractPath, true);
        Console.WriteLine($"Uninstalled EZCodeLanguage from: {extractPath}\\");
    
        Console.WriteLine($"Adding directory to user PATH: {directoryToAddToPath}");
        
        RemoveDirectoryFromPath(directoryToAddToPath);
    }
    private static async Task Install()
    {
        var latestRelease = await GetLatestReleaseAsync(owner, repo);
        var assetUrl = GetAssetDownloadUrl(latestRelease, assetName);

        if (assetUrl == null)
        {
            Console.WriteLine($"Asset {assetName} not found in the latest release.");
            return;
        }

        var downloadPath = Path.Combine(Path.GetTempPath(), assetName);
        await DownloadFileAsync(assetUrl, downloadPath);

        ExtractZipFile(downloadPath, extractPath);

        Console.WriteLine($"Extracted to: {extractPath}");
        Console.WriteLine("Installation completed successfully.");

        Directory.CreateDirectory(Package.PackagesDirectory);
        Console.WriteLine("Created Package Directory.");
        Console.WriteLine("Installing Packages.");

        string[] packages = ["main", "time", "windows-os"];

        foreach (var package in packages)
        {
            var downloadFilePath = Path.Combine(Package.PackagesDirectory, package);
            var downloadUrl = $"https://github.com/EZCodeLanguage/Packages/releases/download/{package}-package/{package}.zip";

            InstallAndExtract(downloadUrl, downloadFilePath);
            Console.WriteLine($"Installed package '{package}' to: {Package.PackagesDirectory}");
        }

        Console.WriteLine($"Adding directory to user PATH: {directoryToAddToPath}");
        
        AddDirectoryToPath(directoryToAddToPath);
    }

    static void AddDirectoryToPath(string directory)
    {
        UpdateProcessPath(directory, true);
        UpdateUserPath(directory, true);

        Console.WriteLine("successfully added directory to PATH.");
    }

    static void RemoveDirectoryFromPath(string directory)
    {
        UpdateProcessPath(directory, false);
        UpdateUserPath(directory, false);

        Console.WriteLine("Directory removed from PATH successfully.");
    }

    static void UpdateProcessPath(string directory, bool add)
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH") ?? "";
        string newPath = add ? AddDirectoryToPathString(currentPath, directory) : RemoveDirectoryFromPathString(currentPath, directory);
        Environment.SetEnvironmentVariable("PATH", newPath, EnvironmentVariableTarget.Process);
    }

    static void UpdateUserPath(string directory, bool add)
    {
        const string userPathSubkey = @"Environment";
        const string pathVariableName = "Path";

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(userPathSubkey, writable: true))
        {
            if (key == null)
            {
                throw new InvalidOperationException("Failed to open registry key.");
            }

            string currentPath = key.GetValue(pathVariableName, "", RegistryValueOptions.DoNotExpandEnvironmentNames).ToString();
            string newPath = add ? AddDirectoryToPathString(currentPath, directory) : RemoveDirectoryFromPathString(currentPath, directory);
            key.SetValue(pathVariableName, newPath, RegistryValueKind.ExpandString);
        }
    }

    static string AddDirectoryToPathString(string currentPath, string directory)
    {
        var pathParts = currentPath.Split(';');
        if (!pathParts.Contains(directory, StringComparer.OrdinalIgnoreCase))
        {
            currentPath += ";" + directory;
        }
        return currentPath;
    }

    static string RemoveDirectoryFromPathString(string currentPath, string directory)
    {
        var pathParts = currentPath.Split(';')
                                   .Where(part => !part.Equals(directory, StringComparison.OrdinalIgnoreCase))
                                   .ToArray();
        return string.Join(";", pathParts);
    }

    private static async Task<dynamic> GetLatestReleaseAsync(string owner, string repo)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
        var url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
        var response = await client.GetStringAsync(url);
        return Newtonsoft.Json.JsonConvert.DeserializeObject(response);
    }

    private static string GetAssetDownloadUrl(dynamic release, string assetName)
    {
        foreach (var asset in release.assets)
        {
            if (asset.name == assetName)
            {
                return asset.browser_download_url;
            }
        }
        return null;
    }

    private static async Task DownloadFileAsync(string url, string outputPath)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync();
        await using var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await stream.CopyToAsync(fileStream);
    } 

    private static void ExtractZipFile(string zipPath, string extractPath)
    {
        if (Directory.Exists(extractPath))
        {
            Directory.Delete(extractPath, true);
        }
        ZipFile.ExtractToDirectory(zipPath, extractPath);
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
}
