using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ezcode_Project_Player
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
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
                Application.Run(new Form1("NOTHING"));
            }
            else
            {
                Application.Run(new Form1(args[0]));
            }
        }

        public static bool IsAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezproj", false) == null);
        }
        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.ezproj");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\ezcode-Project Player.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezproj");

            FileReg.CreateSubKey("DefaultIcon").SetValue("", AppDomain.CurrentDomain.BaseDirectory + "ezCode.ico");
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Text");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + Application.ExecutablePath + "\"" + "%1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", AppDomain.CurrentDomain.BaseDirectory + "ezCode.ico");

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\ezcode-Project Player.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
}