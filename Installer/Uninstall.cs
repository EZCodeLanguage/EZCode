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
                Directory.Delete(Install.appdataDir, true);
                Directory.Delete(Install.filepath, true);
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress1 = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\EZCode.lnk";
                string shortcutAddress2 = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\SLN Builder.lnk";
                if (File.Exists(shortcutAddress1)) File.Delete(shortcutAddress1);
                if (File.Exists(shortcutAddress2)) File.Delete(shortcutAddress2);
                Console.Write(" Done!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\nError during installation: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
