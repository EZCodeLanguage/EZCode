using EZCode;
using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace Installer
{
    public class Program
    { 
        static void Main(string[] args)
        {
            List<Option> options = new List<Option>()
            {
                new Option($" - Install EZCode v{EZCode.EzCode.Version}", 0, true),
                new Option(" - Uninstall EZCode", 1, false),
                //new Option(" - Download extension", 2, false),
            };

            int index = 0;

            ConsoleKey key;

            string txt1 = "Welcome to EZCode Installer! What would you like to do today?";
            string txt2 = "\nUse the arrow keys to select an option, press enter to submit.";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(txt1);
            Console.ResetColor();
            Console.WriteLine(txt2);

            do
            {
                Console.Clear();

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

            } while (key != ConsoleKey.Enter);

            foreach (var option in options)
            {
                if (!option.Checked) continue;
                switch (option.Id)
                {
                    case 0: // Install
                        Install.DownloadMain();
                        RegisterInUninstallRegistry("EZCode", Path.Combine(Install.filepath, $"Installer v{EzCode.Version}", "EZCode_Installer.exe"), Path.Combine(Install.appdataDir, "EZCode", "EZCode_Logo.ico"));
                        SetFullAccessPermissions(Install.filepath);
                        break;
                    case 1: // Uninstall
                        Uninstall.RemovePrompt();
                        break;
                    case 2: // Extension
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("This feature is unavailable at this time.");
                        Console.ResetColor();
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ResetColor();
            Console.ReadKey();
        }
        public static void RegisterInUninstallRegistry(string displayName, string uninstallString, string displayIcon)
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
                {
                    if (key != null)
                    {
                        using (RegistryKey appKey = key.CreateSubKey(displayName))
                        {
                            appKey.SetValue("DisplayName", displayName);
                            appKey.SetValue("UninstallString", uninstallString);
                            appKey.SetValue("DisplayIcon", displayIcon);
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Error: Unable to open registry key.");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }
        static void SetFullAccessPermissions(string directoryPath)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

                // Get the existing access control settings for the directory
                DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

                // Add a rule that allows full control to all users
                directorySecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));

                // Apply the new access control settings to the directory
                directoryInfo.SetAccessControl(directorySecurity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting permissions: {ex.Message}");
            }
        }

        public static void CreateShortcut(string name, string path)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\{name}.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = path;
            shortcut.Save();
        }
        public static void CreateStartMenuShortcut(string folderName, string name, string path)
        {
            WshShell shell = new WshShell();
            string startMenuFolder = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
            string shortcutAddress = Path.Combine(startMenuFolder, "Programs", folderName, $"{name}.lnk");
            Directory.CreateDirectory(Path.GetDirectoryName(shortcutAddress));
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = path;
            shortcut.Save();
        }

        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        [STAThread]
        public static void SetUpFile(string executablePath, string iconPath)
        {
            string fileEnding = ".ezcode";
            for (int i = 0; i < 2; i++)
            {
                if (!IsAssociated(fileEnding, executablePath)) 
                { 
                    Associate(executablePath, iconPath, fileEnding);
                    Associate(executablePath, iconPath, fileEnding);
                }
                fileEnding = ".ezproj";
            }
        }
        public static bool IsAssociated(string fileEnding, string programPath)
        {
            string associatedProgramPath = GetAssociatedProgram(fileEnding);
            return string.Equals(associatedProgramPath, programPath, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetAssociatedProgram(string fileExtension)
        {
            string programPath = null;

            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(fileExtension))
            {
                if (key != null)
                {
                    object value = key.GetValue(null);
                    if (value != null)
                    {
                        using (RegistryKey progKey = Registry.ClassesRoot.OpenSubKey(value.ToString() + "\\shell\\open\\command"))
                        {
                            if (progKey != null)
                            {
                                object path = progKey.GetValue(null);
                                if (path != null)
                                {
                                    programPath = path.ToString();
                                }
                            }
                        }
                    }
                }
            }

            return programPath;
        }

        public static void Associate(string executablePath, string iconPath, string fileEnding)
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + fileEnding);
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\EZCodePlayer.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + fileEnding);

            FileReg.CreateSubKey("DefaultIcon").SetValue("", iconPath);
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Text");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + executablePath + "\"" + "%1");
            AppReg.CreateSubKey("shell\\edit\\command").SetValue("", "\"" + executablePath + "\"" + "%1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", iconPath);

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\EZCodePlayer.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
