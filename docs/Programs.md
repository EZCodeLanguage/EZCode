# Installer

The EZCode Installer is the best way to install EZCode and prevent future errors. 

The Installer gives the options to `Uninstall` or `Install` EZCode. \
If 'Uninstall' is selected, it will verify that you want to do that, and then it will uninstall EZCode. \
If 'Install' is selected, A selection will appear on what to install (the programs in the page). \
After that, it will clone a copy of EZCode and trim it up to only the EZCode Library and EZPlayer-WinForms directories from the repository. It clones it to `C:/Users/<USERNAME>/AppData/Roaming/EZCode`. \
After that, It installs the optional and mandatory programs to `C:/Program Files/EZCode/`.

# EZPlayer

The EZCode Player will take a _ezcode_ or _ezproj_ file and try to execute it using EZCode.

When the program is opened, the user is prompted to locate the file to play. If the program is opened straight from the file (clicking the file and opening from the File Explorer), then the prompt step will be skipped.

Now the Player will see the extension and if it is an _ezproj_ file, then it will go through a process of converting it into EZCode. After that, the Player will figure out the type of project this is from either the _ezproj's_ properties or _ezcode's_ `# project properties`. If this project is a windows project (`window:"true"` this means the program needs at least one window) then the console window will not open, unless the file opened is _ezcode_. 

The Debug option (`debug:"true"`) will add another window only if the project is also a window project. This extra window will record any errors and will have an easy way to quit the program.

There are more options that will affect the way the player works, but find that in the [EZProj Syntax](EZProject-Docs).

When the Player opens up, it will have two parts that may or may not be seen depending on the type of the program. One is the visual output and the other is the console.

### **Visual Output**

The visual Output is where any controls (shapes, labels, buttons, or textboxes) will go. The background is completely black.

### **Console**

This is where any errors show or where the [`print`](EZCode-Docs#print) outputs to. It also contains an input below it withe a clear and send button. The clear button clears the console. The send button will send the text to the program and can be picked up by [`input console`](ezcode-docs#input).

# SLN Builder

The SLN Builder will take your _ezproj_ file and will build a Visual Studio Project for it. First it will start with converting the _ezproj_ file into EZCode. This bit of code is what is used for making the **sln** file and main **Program.cs** file:
```csharp
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
```
```csharp
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
```
It then imports the EZCode Library as one of the child directories. If a version of Visual Studio is installed that has WinForms installed, then you can build the program into an Executable. **If you click build in Visual Studio and it pops up an error**, go to `Project > Configure Startup Project > Single starup projects` and select EZPlayer-WinForms. If this error occurs, it is because it is trying to build the EZCode Library not the desired program.

# EZ IDE

The EZ IDE is the easiest way to develop with EZCode. It has a [Debugger](ide-docs#debugger), [Console](#console), and [Visual Output](#visual-output) built into the IDE. It has a [project explorer](ide-docs#project-explorer), and many [settings](ide-docs#settings) to customize your experience. Go to the [IDE Docs](ide-docs) to learn more about the EZ IDE.