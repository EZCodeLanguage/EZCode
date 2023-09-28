namespace EzCode_API
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new EZCode_Form());

            //EZCode.EZProj ez = new EZCode.EZProj(@"C:\Users\jlham\OneDrive\Documents\ezcodeTesting\input-test.ezproj");
            //Application.Run(new EZCode.EZPlayer.Player(ez));
        }
    }
}