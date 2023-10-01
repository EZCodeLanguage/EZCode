namespace EZBuilder
{
    public class Builder
    {
        public string AppRoamingDirectory { get; private set; }
        public string WinFormDirectory { get; private set; }
        public string EZCodeDirectory { get; private set; }
        public string File { get; set; }
        public string OutputPath { get; set; }
        public string Output { get; private set; }
        public Builder(string file, string path)
        {
            File = file;
            OutputPath = path;

            AppRoamingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EZCode");
            WinFormDirectory = Path.Combine(AppRoamingDirectory, "EZCode-WinForms");
            EZCodeDirectory = Path.Combine(AppRoamingDirectory, "EZCode");
        }
        public bool Build(string file, string path)
        {
            File = file;
            OutputPath = path;
            return Build();
        }
        public bool Build()
        {
            try
            {
                //EZProj File
                EZCode.EZProj proj = new EZCode.EZProj(File);
                if (proj.Name == null) proj.Name = "EZCode Project";
                OutputPath = Path.Combine(OutputPath, proj.Name);
                string SlnPath = Path.Combine(OutputPath, proj.Name + ".sln");
                string ProgramCSPath = Path.Combine(OutputPath, "EZCode-WinForms", "Program.cs");

                //Build
                DuplicateDirectories(AppRoamingDirectory, OutputPath);
                CopyFilesToDirectories(AppRoamingDirectory, OutputPath, true);

                //Create sln
                string slnContents = @"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.4.33213.308
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""EZPlayer-WinForms"", ""EZCode-WinForms\EZPlayer-WinForms.csproj"", ""{8BE15D64-F561-4087-AB87-D0230BA54ACB}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""EZCode"", ""EZCode\EZCode.csproj"", ""{C8FB09DE-19EC-4F2A-89F9-C159B0956355}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{C8FB09DE-19EC-4F2A-89F9-C159B0956355}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{C8FB09DE-19EC-4F2A-89F9-C159B0956355}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{C8FB09DE-19EC-4F2A-89F9-C159B0956355}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{C8FB09DE-19EC-4F2A-89F9-C159B0956355}.Release|Any CPU.Build.0 = Release|Any CPU
		{8BE15D64-F561-4087-AB87-D0230BA54ACB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{8BE15D64-F561-4087-AB87-D0230BA54ACB}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{8BE15D64-F561-4087-AB87-D0230BA54ACB}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{8BE15D64-F561-4087-AB87-D0230BA54ACB}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {B9D3EA0D-6162-49A3-9CB5-3688200298A2}
	EndGlobalSection
EndGlobal
";
                System.IO.File.WriteAllText(SlnPath, slnContents);

                //Update Program.cs
                string programContents = @"using EZCode;
namespace EZPlayer_WinForms {
    internal static class Program {
        [STAThread] static void Main() {
            ApplicationConfiguration.Initialize();
            EzCode ez = new EzCode();
            string code = @""" + proj.Program + @""";
            ez.Code = code;
            EZProj proj = new EZProj(ez);
            Application.Run(new EZCode.EZPlayer.Player(proj));
        }
    }
}";
                System.IO.File.WriteAllText(ProgramCSPath, programContents);

                //Success
                Output ??= $"Build Succeeded '{SlnPath}'";
                return true;
            }
            catch (Exception e)
            {
                Output ??= e.Message;
                return false;
            }
        }

        public static void DuplicateDirectories(string sourceDirectory, string targetDirectory, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            foreach (string dir in Directory.GetDirectories(sourceDirectory, searchPattern, searchOption))
            {
                var relativePath = Path.GetRelativePath(sourceDirectory, dir);
                var targetPath = Path.Combine(targetDirectory, relativePath);
                Directory.CreateDirectory(targetPath);
            }
        }

        public static void CopyFilesToDirectories(string sourceDirectory, string targetDirectory, bool replaceIfExists, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            foreach (string filePath in Directory.GetFiles(sourceDirectory, searchPattern, searchOption))
            {
                var relativePath = Path.GetRelativePath(sourceDirectory, filePath);
                var targetPath = Path.Combine(targetDirectory, relativePath);
                System.IO.File.Copy(filePath, targetPath, replaceIfExists);
            }
        }
    }
}
