namespace EZPlayer_WinForms {
    internal static class Program {
        [STAThread] static void Main() {
            ApplicationConfiguration.Initialize();
            EZCode.EZProj ez = new EZCode.EZProj(@"C:\Path\To\EZProj\File.ezproj");
            Application.Run(new EZCode.EZPlayer.Player(ez));
        }
    }
}
