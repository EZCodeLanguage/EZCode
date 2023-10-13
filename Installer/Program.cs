using IWshRuntimeLibrary;

namespace Installer
{
    public class Program
    { 
        static void Main(string[] args)
        {
            List<Option> options = new List<Option>()
            {
                new Option(" - Install latest version of EZCode", 0, true),
                new Option(" - Uninstall EZCode", 1, false),
                //new Option(" - Download extension", 2, false),
            };

            int index = 0;

            ConsoleKey key;

            do
            {
                Console.Clear();

                string txt1 = "Welcome to EZCode Installer! What would you like to do today?";
                string txt2 = "\nUse the arrow keys to select an option, press enter to submit.";
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

        public static void CreateShortcut(string name, string path)
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\{name}.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = path;
            shortcut.Save();
        }
    }
}
