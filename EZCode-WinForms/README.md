# EZCode-WinForms
 
 This is an example on how to use EZCode to quickly make a WinForms Program

 ---
 **Program.cs**
 ```
namespace EZPlayer_WinForms {
    internal static class Program {
        [STAThread] static void Main() {
            ApplicationConfiguration.Initialize();
            EZCode.EZProj ez = new EZCode.EZProj(@"C:\Path\To\EZProj\File.ezproj");
            Application.Run(new EZCode.EZPlayer.Player(ez));
        }
    }
}
```

  ---
  Replace `@"C:\Path\To\EZProj\File.ezproj"` with path to project file `.ezproj`.
