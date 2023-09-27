using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace EZCodePlayer
{
    internal static class Program
    {

        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[]? args)
        {
            if (!IsAssociated())
            {
                //do nothing
            }
            else
            {
                Associate();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new FileFinder(""));
            }
            else
            {
                bool a = false;
                //while (!a){}
                FileInfo fileInfo = new FileInfo(args[0]);
                if (fileInfo.Extension == ".ezproj")
                {
                    Application.Run(new Player(fileInfo, Player.ProjectType.Project));

                }
                else if (fileInfo.Extension == ".ezcode")
                {
                    Application.Run(new Player(fileInfo, Player.ProjectType.Script));
                }
            }
        }

        public static bool IsAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezproj", false) == null);
        }
        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.ezproj");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\EZCodePlayer.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezproj");

            FileReg.CreateSubKey("DefaultIcon").SetValue("", "C:\\REPOS\\EZCode\\App\\EZCode_Logo.ico"); // Remember To Change
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Text");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + Application.ExecutablePath + "\"" + "%1");
            AppReg.CreateSubKey("shell\\edit\\command").SetValue("", "\"" + Application.ExecutablePath + "\"" + "%1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", "C:\\REPOS\\EZCode\\App\\EZCode_Logo.ico"); // Remember To Change

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\EZCodePlayer.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
