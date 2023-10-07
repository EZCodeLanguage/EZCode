using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace Installer
{
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
    public static class Download
    {
        static bool _done;
        static bool done
        {
            get => _done;
            set
            {
                _done = value;
                if (_done)
                {
                    MainText += " Done!";
                }
            }
        }
        static int percentage;
        static string MainText;
        public static void DownloadMain()
        {
            Option[] options = Options();
            try
            {
                string githubRepoUrl = "https://github.com/JBrosDevelopment/EZCode";
                string realTag = "2.0.0";
                string releaseTag = $"v{realTag}";
                string releaseTitle = $"{releaseTag}_Beta";

                Working($"Installing From {githubRepoUrl}.git... This may take a second.");

                string filepath = "C:\\Program Files\\EZCode\\";

                foreach (var option in options)
                {
                    if (!option.Checked) continue;
                    switch (option.Id)
                    {
                        case 0: // Install main
                            installcore();
                            ezcodeplayer();
                            break;
                        case 1: // Install SLN
                            slnbuilder();
                            break;
                    }
                }

                void installcore() // Install Source Start
                {
                    string tempDirectory = Path.Combine(Path.GetTempPath(), "EZCode");

                    Directory.CreateDirectory(tempDirectory);

                    string installFile = releaseTitle + ".zip";

                    string downloadUrl = $"{githubRepoUrl}/archive/refs/tags/{releaseTag}.zip";

                    string decompressDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EZCode");

                    WebInstaller(installFile, downloadUrl, tempDirectory, true); //$"{githubRepoUrl}/releases/download/{releaseTag}/EzCode.All-Programs.-V1.11.5.msi"

                    string[] d = Directory.GetDirectories(Path.Combine(tempDirectory, $"EZCode-{realTag}"));
                    for (int i = 0; i < d.Length; i++)
                    {
                        DirectoryInfo info = new DirectoryInfo(d[i]);
                        string t_decompress = Path.Combine(decompressDirectory, info.Name); 
                        if (info.Name.ToLower() == "ezcode" || info.Name.ToLower() == "ezcode-winforms" || info.Name.ToLower() == "packages")
                        {
                            if (Directory.Exists(t_decompress))
                                Directory.Delete(t_decompress);
                            Directory.Move(d[i], t_decompress);
                        }
                    }
                    Directory.Delete(tempDirectory, true);
                }

                void ezcodeplayer() // Install EZCode Main
                {
                    string installFile = $"EZCode_Player_{releaseTitle}.zip";

                    string downloadUrl = $"{githubRepoUrl}/releases/download/{releaseTag}/{installFile}";

                    string directorypath = Path.Combine(filepath, installFile);
                    if (!Directory.Exists(directorypath)) Directory.CreateDirectory(directorypath);

                    WebInstaller($"{installFile}.zip", downloadUrl, directorypath, true);

                    Program.CreateShortcut("EZCode", Path.Combine(directorypath, $"EZCode_Player_{releaseTag}", "EZCodePlayer.exe"));
                }

                void slnbuilder() // Install Sln Builder
                {
                    string installFile = $"Sln_Builder_{releaseTitle}.zip";

                    string downloadUrl = $"{githubRepoUrl}/releases/download/{releaseTag}/{installFile}";

                    string directorypath = Path.Combine(filepath, installFile);
                    if (!Directory.Exists(directorypath)) Directory.CreateDirectory(directorypath);

                    WebInstaller($"{installFile}.zip", downloadUrl, directorypath, true);

                    Program.CreateShortcut("SLN Builder", Path.Combine(directorypath, $"Sln_Builder", "EZ_SLN_Builder.exe"));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during installation: {ex.Message}");
            }
        }
        static Option[] Options()
        {
            Console.ResetColor();
            string txt = "Choose what to Install";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(txt);
            Console.ResetColor();

            List<Option> options = new List<Option>()
            {
                new Option("EZCode Core and Player", 0, true),
                new Option("EZCode SLN (Microsoft Visual Studio Project) Builder", 1, false)
            };

            int index = 0;

            ConsoleKey key;

            do
            {
                Console.Clear();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(txt);
                Console.ResetColor();

                for (int i = 0; i < options.Count; i++)
                {
                    if (i == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }

                    Console.WriteLine($"{(options[i].Checked ? "[X]" : "[ ]")} {options[i].Name}");

                    Console.ResetColor();
                }

                key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        index = index == 0 ? options.Count - 1 : index - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        index = index == options.Count - 1 ? 0 : index + 1;
                        break;
                    case ConsoleKey.Spacebar:
                        bool o = options[index].Checked;
                        for (int i = 0; i < options.Count; i++)
                        {
                            options[i].Checked = options[i].Id == 0 ? true : options[i].Checked;
                        }
                        options[index].Checked = options[index].Id == 0 ? true : !o;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(" is invalid input. Please use the arrow keys, spacebar, or enter.");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }

            } while (key != ConsoleKey.Enter);

            return options.ToArray();
        }

        static void WebInstaller(string installFile, string downloadUrl, string tempDirectory, bool Decompress = false, string decompressDirectory = "")
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string downloadFilePath = Path.Combine(tempDirectory, installFile);

                    webClient.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) => percentage = e.ProgressPercentage;
                    webClient.DownloadFile(downloadUrl, downloadFilePath);
                    done = true;

                    if (Decompress)
                    {
                        Working($"\n\nDecompress... ");

                        ZipFile.ExtractToDirectory(downloadFilePath, decompressDirectory == "" ? tempDirectory : decompressDirectory, true);
                        done = true;

                        File.Delete(downloadFilePath);
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nInstallation completed successfully.");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                done = true;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n\n" + e.Message);
            }
        }

        static async Task Working(string text)
        {
            MainText += text;
            done = false;
            int tick = 0;
            while (!done)
            {
                Console.Clear();
                Console.Write(MainText);
                Console.ForegroundColor = ConsoleColor.Yellow;
                switch (tick)
                {
                    case 0:
                        Console.Write("/");
                        tick++;
                        break;
                    case 1:
                        Console.Write("-");
                        tick++;
                        break;
                    case 2:
                        Console.Write(@"\");
                        tick = 0;
                        break;
                }
                //Console.Write($" {percentage}% ");
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.Write("|");
                //Console.ForegroundColor = ConsoleColor.Blue;
                //string per = "";
                //int dashes = percentage / 5;
                //for (int i = 0; i < 20; i++)
                //{
                //    per += i < dashes ? "-" : " ";
                //}
                //Console.Write(per);
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.Write("|");
                await Task.Delay(250);
                Console.ResetColor();
            }
        }
    }
}
