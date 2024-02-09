using EZCode.Converter;

namespace TestEnvironment
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
            //Application.Run(new ConverterExample());

            Converter converter = new Converter();
            string all = converter.Convert(
                """
                window Wind new : x:50, y:60, w:100, h:100, t:text, minwidth:50, maxwidth:50, tert:dfs, icon:C\;/Users/jlham/OneDrive/Desktop/EZCode_Logos/Normal/EZCode_Logo.ico
                shape shp x:50, y:50
                label lb t:TEXT
                textbox tb x:50, y:25, t:TEXT
                button btn t:TEXT, y:25
                Wind display shp
                Wind display lb
                Wind display tb
                Wind display btn

                Wind open
                Wind change : w:600
                """, Converter.ProgrammingLanguage.Python, out Converter.ProgramFile[] files);

            for (int i = 0; i < files.Length; i++)
            {
                File.WriteAllText(Path.Combine("D:\\Python\\ezcode\\Conv", files[i].Name), files[i].Content);
            }
            File.WriteAllText(Path.Combine("D:\\Python\\ezcode\\Conv", "All.py"), all);

            Application.Exit();
        }
    }
}