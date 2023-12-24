using IWshRuntimeLibrary;
using System.Xml.Linq;
using File = System.IO.File;

namespace Installer
{
    public class Uninstall
    {
        public static void RemovePrompt()
        {
            Console.ResetColor();
            string text = "Are you sure you want to uninstall EZCode? (y/n) ";
            string addedText = "";
            bool? uninstall = null;
            bool done = false;
            ConsoleKey key;
            do
            {
                Console.Clear();
                Console.Write(text + addedText);
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Y:
                        uninstall = true;
                        addedText = "Yes\nPress Enter to submit";
                        break;
                    case ConsoleKey.N:
                        uninstall = false;
                        addedText = "No\nPress Enter to submit";
                        break;
                    case ConsoleKey.Enter:
                        if (uninstall == null)
                        {
                            addedText = "\nPlease enter 'y' or 'n'";
                        }
                        else if (uninstall == true)
                        {
                            Remove();
                            done = true;
                        }
                        else if (uninstall == false)
                        {
                            addedText = "\nUninstall aborted.";
                            done = true;
                        }
                        break;
                    default:
                        addedText = $"\n'{key}' is not a valid key ('y' or 'n').";
                        uninstall = null;
                        break;
                }
            }
            while (!done);
        }
        static void Remove()
        {
            try
            {
                Install.CreateDirs();
                Console.Clear();
                Console.Write("Uninstalling...");

                // Remove program files and directories
                Directory.Delete(Install.appdataDir, true);
                Directory.Delete(Install.filepath, true);

                // Remove desktop shortcuts
                RemoveDesktopShortcut("EZCode Player.lnk");
                RemoveDesktopShortcut("SLN Builder.lnk");
                RemoveDesktopShortcut("EZ IDE.lnk");

                // Remove Start Menu shortcuts
                RemoveStartMenuShortcut();

                Console.Write(" Done!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\nError during uninstallation: {ex.Message}");
                Console.ResetColor();
            }
        }

        static void RemoveDesktopShortcut(string shortcutName)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = Path.Combine((string)shell.SpecialFolders.Item(ref shDesktop), shortcutName);

            if (File.Exists(shortcutAddress))
            {
                File.Delete(shortcutAddress);
            }
        }

        static void RemoveStartMenuShortcut()
        {
            string startMenuFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "EZCode");

            if (Directory.Exists(startMenuFolder))
            {
                Directory.Delete(startMenuFolder, true);
            }
        }

    }
}
