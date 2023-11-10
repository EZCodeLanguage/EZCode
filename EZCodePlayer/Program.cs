using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace EZCodePlayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[]? args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                Application.Run(new FileFinder(""));
            }
            else
            {
                FileInfo fileInfo = new FileInfo(args[0]);
                if (fileInfo.Extension == ".ezcode" || fileInfo.Extension == ".ezproj")
                {
                    using (var key = Registry.CurrentUser.CreateSubKey(FileFinder.keyName))
                    {
                        key.SetValue("path", fileInfo.FullName);
                    }

                    if (fileInfo.Extension == ".ezproj")
                    {
                        EZProj proj = new EZProj(args[0]);
                        Application.Run(new EZCode.EZPlayer.Player(proj));
                    }
                    else if (fileInfo.Extension == ".ezcode")
                    {
                        EzCode ez = new EzCode();
                        ez.Code = File.ReadAllText(args[0]);
                        EZProj proj = new EZProj(ez, args[0]);
                        Application.Run(new EZCode.EZPlayer.Player(proj));
                    }
                }
                else
                {
                    Application.Run(new FormError(args[0]));

                    Application.Exit();
                }
            }
        }
    }
    class FormError : Form
    {
        public FormError(string file)
        {
            MessageBox.Show($"The file ({file}) does not have the correct file extension (.ezcode or .ezproj). Please change the file extension before you continue playing this file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Close();
        }
    }
}
