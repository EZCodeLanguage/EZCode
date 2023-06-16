using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace ezCode
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
            try
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
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", "Exception Message: \n" + ex.Message + "\n\n\nStack Trace: \n" + ex.StackTrace);
            }
        }

        public static bool IsAssociated()
        {
            return (Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezcode", false) == null);
        }
        public static void Associate()
        {
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\.ezcode");
            RegistryKey AppReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\Applications\\ezCode.exe");
            RegistryKey AppAssoc = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.ezcode");

            FileReg.CreateSubKey("DefaultIcon").SetValue("", AppDomain.CurrentDomain.BaseDirectory + "ezCode.ico");
            FileReg.CreateSubKey("PerceivedType").SetValue("", "Text");

            AppReg.CreateSubKey("shell\\open\\command").SetValue("", "\"" + Application.ExecutablePath + "\"" + "%1");
            AppReg.CreateSubKey("shell\\edit\\command").SetValue("", "\"" + Application.ExecutablePath + "\"" + "%1");
            AppReg.CreateSubKey("DefaultIcon").SetValue("", "C:\\Users\\jlham\\source\\repos\\ezCode\\ezCode.ico");

            AppAssoc.CreateSubKey("UserChoice").SetValue("Progid", "Applications\\ezCode.exe");

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
    }
    public static class ControlExtensions
    {
        public static void AddText(this Control control, string text, bool error)
        {
            if (!error) control.Text += text;
            if (control is RichTextBox && error)
            {
                RichTextBox richTextBox = control as RichTextBox;
                richTextBox.SelectionStart = richTextBox.TextLength;
                richTextBox.SelectionLength = 0;
                richTextBox.SelectionColor = Color.Red;
                richTextBox.AppendText(text);
                richTextBox.SelectionColor = richTextBox.ForeColor;
            }
        }
    }

}