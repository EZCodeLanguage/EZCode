# C# API For EZCode

- [C# API For EZCode](#c-api-for-ezcode)
  - [Example Project](#example-project)
- [EZCodeLanguage.EZCode](#ezcodelanguageezcode)
- [EZCodeLanguage.Debug](#ezcodelanguagedebug)
  - [Debug.Breakpoint](#debugbreakpoint)
- [EZCodeLanguage.Interpreter](#ezcodelanguageinterpreter)
  - [Setting it up](#setting-it-up)
  - [Properties](#properties)
  - [Methods and Functions](#methods-and-functions)
  - [Events](#events)
  - [Interpreter.CustomAssemblyLoadContext](#interpretercustomassemblyloadcontext)
- [EZCodeLanguage.Parser](#ezcodelanguageparser)
  - [Setting it up](#setting-it-up-1)
  - [Properties](#properties-1)
  - [Enum](#enum)
- [EZCodeLanguage.PackageClass](#ezcodelanguagepackageclass)
- [EZCodeLanguage.Package](#ezcodelanguagepackage)
- [EZCodeLanguage.PackageClass](#ezcodelanguagepackageclass-1)
- [EZCodeLanguage.Projects](#ezcodelanguageprojects)
- [EZCodeLanguage.EZHelp](#ezcodelanguageezhelp)

The API for EZCode is supposed to be as straightforward as possible and easy to incorporate into your project. You'll need to add the nuget package to your project, or clone the repository and add it as a dependency to your project. 

> `dotnet add package EZCode` 

To include it in the file, use, `using EZCodeLanguage`. 


## Example Project
```cs
using EZCodeLanguage;

public class Program
{
    public static void Main(string[] args)
    {
        var code = """
            include main

            int x new : 50
            int y new : 100

            print '(x * y)'
            """;
        EZCode.RunCode(code);
        // outputs 5000
    }
}
```



# EZCodeLanguage.EZCode

This class is the easiest way to add EZCode to your project. 
- ```cs
  // Run and execute EZCode file  
  EZCode.RunFile("path/to/file.ezcode")
  ```
- ```cs
  // Run EZCode file with the 'main' package automatically included 
  EZCode.RunFileWithMain("path/to/file.ezcode")
  ```
- ```cs
  // Debug an EZCode file by adding breakpoints with: Debug.Breakpoint[] 
  // new Parser.Line("line_content", NUMBER, "file");
  // var line = parser.TokenWithLines[1]; // assuming 'parser' is instance of Parser
  EZCode.DebugFile([new Breakpoint(line)], "path/to/file.ezcode")
  ```
- ```cs
  // Run code without a file
  // 'path' is optional parameter
  EZCode.RunCode("code")
  EZCode.RunCode("code", path:"Running from inside program")
  ```
- ```cs
  // Run code with the 'main' package automatically included 
  // 'path' is optional parameter
  EZCode.RunCodeWithMain("code")
  EZCode.RunCodeWithMain("code", path:"Running from inside program")
  ```
- ```cs
  // Debug an EZCode code by adding breakpoints with: Debug.Breakpoint[] 
  // new Parser.Line("line_content", NUMBER, "file");
  // var line = parser.TokenWithLines[1]; // assuming 'parser' is instance of Parser
  // 'path' is optional parameter
  EZCode.DebugCode([new Breakpoint(line)], "path/to/file.ezcode", path:"Running from inside program")
  ```
- ```cs
  // Run and execute a Project
  // input path and directory into function
  // You can use: EZCode.RunProject(path, Path.GetDirectoryName(path));
  EZCode.RunProject("directory/project.json", "directory/")
  ```
- ```cs
  // Debug an EZCode Project by adding breakpoints with: Debug.Breakpoint[] 
  // input breakpoints, path, and directory into function
  // You can use: EZCode.DebugProject(breakpoints, path, Path.GetDirectoryName(path));
  EZCode.DebugProject(breakpoints, "path/to/file.ezcode", path:"Running from inside program")
  ```

# EZCodeLanguage.Debug

Static class used for debugging in the interpreter

```cs
var interpreter = /*Interpreter Class*/;
var line = interpreter.CurrentLine;
var breakpoints = [
    new Breakpoint(line)
];

// Check if Breakpoint is hit.
// Returns boolean depending on if hit or not
IsHit(line, breakpoints, interpreter)
```

## Debug.Breakpoint
```cs
public class Breakpoint
{
    // Required:
    public Line Line { get; set; }
    // Optional:
    public bool Enabled { get; set; }
    public bool DisableWhenHit { get; set; }
    public string? EZCodeConditionToHit { get; set; }
    public string? EZCodeActionWhenHit { get; set; }
    public Breakpoint? EnabledWhenBreakpointIsHit { get; set; }
    public bool Hit { get; set; }
    // Constructor:
    public Breakpoint(Line line, bool enabled = true, bool disableWhenHit = false,
        string? ezcodeConditionToHit = null, string? ezcodeActionWhenHit = null, 
        Breakpoint? enabledWhenBreakpointIsHit = null)
    {
        Line = line;
        Enabled = enabled;
        DisableWhenHit = disableWhenHit;
        EZCodeConditionToHit = ezcodeConditionToHit;
        EZCodeActionWhenHit = ezcodeActionWhenHit;
        EnabledWhenBreakpointIsHit = enabledWhenBreakpointIsHit;
    }
}
```

# EZCodeLanguage.Interpreter

This class is used to interpret parsed EZCode.

```cs
new Interpreter(parser, /*optional for debugging:*/ breakpoints)
```

## Setting it up

```cs
var parser = /*set up parser*/;
var interpreter = new Interpreter(parser);
/* Set up Input Type (Optional):
 * interpreter.InputType = EZInputType.Console; // default
 * //                    = EZInputType.InputMethod; // needs SetInput()
 * // SetInput("input") // Input for 'input' method 
 */
interpreter.Interperate();
```

## Properties

- ```cs
  // Instance of EZCode used with EZHelp
  public static Interpreter Instance { get; private set; }
  ```
- ```cs
  public string[] Output; // Console output from execution
  ```
- ```cs
  public enum EZInputType { Console, InputMethod } // Type of input
  public EZInputType InputType { get; set; } = EZInputType.Console; // Input type, sets default to Console
  ```
- ```cs
  // For debugging. Used to continue when a break point is hit. Set it to true for that
  public bool ContinuedForBreakpoint = false;
  ```
- ```cs
  // The Parser used in the interpreting
  public Parser parser { get; private set; }
  ```
- ```cs
  public EZHelp EZHelp { get; private set; } // The EZHelp instance
  ```
- ```cs
  // Don't mess with.
  // Used for including EZCode Packages that have custom dll
  public CustomAssemblyLoadContext[] LoadedAssemblies = []; 
  ```
- ```cs
  public Stack<string> StackTrace { get; private set; } // Stacktrace
  ```
- ```cs
  public Exception[] Errors { get; private set; } = []; // Errors that have occured in the script
  ```
- ```cs
  public Var[] Vars { get; set; } = []; // Array of variables that are in the scope of the current line in the interpreter
  ```
- ```cs
  public Method[] Methods { get; set; } = []; // Array of methods that are in the scope of the current line in the interpreter
  ```
- ```cs
  public Class[] Classes { get; set; } = []; // Array of classes that are in the scope of the current line in the interpreter
  ```
- ```cs
  public Debug.Breakpoint[]? Breakpoints { get; set; } // Debug Breakpoints for the interpreter
  ```
- ```cs
  public Line CurrentLine { get; private set; } // current line being executed in the interpreter
  ```

## Methods and Functions

- ```cs
  // This would be used if the environment that EZCode is running on does not have a console.
  // This could be used in a method that gets called when a user types the input somewhere and clicks send
  // It would call this method with the input like this:
  /* public Send_Clicked() 
   * {
   *     SetInput(InputTextBox.Text);
   * }
   */
  // Used for the EZCode method 'input' in the main package
  public void SetInput(string input, EZInputType inputType = EZInputType.InputMethod)
  ```
- ```cs
  // Start Interpreting  
  public void Interperate() 
  ```
- ```cs
  // Start Interpreting with custom LineWithToken array and not the parser.LinesWithTokens
  public void Interperate(LineWithTokens[] LineTokens) 
  ```
- ```cs
  // InvokeMethod is a function that invokes a method from the 'methodPath' and 'parameters'
  // var methodPath = "System.Console.WriteLine";
  // var parameters = ["Hello World"];
  // Interpreter.InvokeMethod(methodPath, parameters, new EZHelp(), out _)
  public static object? InvokeMethod(string methodPath, object[] parameters, EZHelp e, out CustomAssemblyLoadContext? assemblyContext)
  ```

## Events

- ```cs
  // This event gets called when the output is wrote to with the Output property
  public event EventHandler OutputWrote;
  ```
- ```cs
  // This event gets called when the output is cleared
  public event EventHandler OutputCleared;
  ```

## Interpreter.CustomAssemblyLoadContext

Empty class used for EZCode Packages that include custom dll's
```cs
public class CustomAssemblyLoadContext: AssemblyLoadContext
{
    public CustomAssemblyLoadContext() : base(isCollectible: true) { }
    protected override Assembly Load(AssemblyName assemblyName) => null;
}
```

# EZCodeLanguage.Parser

This class is used to Parse from string EZCode to LineWithToken[]

```cs
new Parser(code, filePath)
```

## Setting it up

With a FileInfo Instance
```cs
// create FileInfo
var file = "C:/file.ezcode";
var fileInfo = new FileInfo(file);

// set up parser
var parser = new Parser(fileInfo);
```

With code and filepath
```cs
// set up path and code
var path = "C:/file.ezcode";
var code = "print Hello World";

// set up parser
var parser = new Parser(code, path);
```

## Properties

- ```cs
  // seperating chars that seperate tokens
  public static char[] Delimeters = [' ', '{', '}', '@', ':', ',', '?', '!'];
  ```
- ```cs
  // the code property that gets parsed
  public string Code { get; private set; }
  ```
- ```cs
  // the filepath property that goes with the Lines
  public string FilePath { get; set; }
  ```
- ```cs
  // the list of classes found in the code property
  public List<Class> Classes = [];
  ```
- ```cs
  // the list of methods found in the code property
  public List<Method> Methods = [];
  ```
- ```cs
  // the list of line with tokens found in the code property
  public List<LineWithTokens> LinesWithTokens = [];
  ```

## Enum

The Enum for Token Types that are all the available types
- ```cs
  public enum TokenType
  {
      None, Null, Comment, Comma, QuestionMark, Colon, Arrow, DataType,
      OpenCurlyBracket, CloseCurlyBracket, New, If, Else, Elif, Loop, Try, Fail,
      Argument, Identifier, Undefined, Class, Explicit, Alias, Watch, Params,
      TypeOf, NoCol, Method, Match, Break, Yield, Return, Get, And, Not, Or, Make,
      RunExec, EZCodeDataType, Include, Exclude, Global, True, False, Throw, Dispose, 
  }
  ```

## Classes

**Token**

The class for tokens
```cs
public class Token
{
    public TokenType Type { get; set; }
    public object Value { get; set; }
    public string StringValue { get; set; }
    public Token(TokenType type, object value, string stringValue)
    {
        Type = type;
        Value = value;
        StringValue = stringValue;
    }
}  
```

**Line**

The class for lines
```cs
public class Line
{
    public string Value { get; set; }
    public int CodeLine { get; set; }
    public string FilePath { get; set; }
    public Line(string value, int codeLine, string file)
    {
        FilePath = file;
        Value = value;
        CodeLine = codeLine;
    }
}
```

**There are many others, they can be found in `EZCode/Parser.cs`**

Here is a list, 
- LineWithTokens
- Argument
- Statement
- DataType
- ExplicitWatch
- ExplicitParams
- RunMethod
- GetValueMethod
- Class
- Var
- CSharpMethod
- CSharpDataType

# EZCodeLanguage.PackageClass

This is the equivelent for the [JSON package](Packages.md#package.json)
```cs
public class PackageClass
{
    public string Name { get; set; }
    public string[] Files { get; set; }
    public Config? Configuration { get; set; }
    public class Config
    {
        public string? LibraryDirectory { get; set; }
        public string[]? GlobalPackages { get; set; }
    }
}
```

# EZCodeLanguage.Package

This is a static class for Packages

- ```cs
  // Where the Package Directory is located:
  // C:\Users\%USER%\EZCodeLanguage\Packages\
  public static string PackagesDirectory;
  ```
- ```cs
  // This adds an EZCode Package into the Execution directory (used for importing dll into program). 
  public static void AddPackageToExecutionDirectory(PackageClass project, string executionDirectory)
  ```
- ```cs
  // This removes an EZCode Package from the Execution directory (used for removing imported dll from program). 
  public static void RemovePackageFromExecutionDirectory(PackageClass project, string executionDirectory)
  ```
- ```cs
  // This removes all EZCode Package from the Execution directory (used for removing imported dll from program). 
  public static void RemoveAllPackagesFromExecutionDirectory(string executionDirectory)
  ```
- ```cs
  // Gets package directory from a package name
  public static string GetPackageDirectory(string package_name)
  ```
- ```cs
  // Gets package file from a package name
  public static string GetPackageFile(string package_name)
  ```
- ```cs
  // Gets package from package name
  public static PackageClass GetPackageAsProject(string package_name)
  ```
- ```cs
  // Gets parser from package name
  public static Parser GetPackageAsParser(string package_name)
  ```
- ```cs
  // Returns a parser with packages:
  // parser = /*Parser Instance*/;
  // parser = Parser.ReturnParserWithPackages(parser, ["main", "time"])
  public static Parser ReturnParserWithPackages(Parser parser, string[] package_names)
  ```
- ```cs
  // Removes a parser from a parser
  public static Parser RemovePackageFromParser(Parser main_parser, Parser remove)
  ```
- ```cs
  // Combines parsers into a single parser
  public static Parser CombineParsers(Parser[] parsers) 
  ```
  ```cs
  // Combines parsers into a single parser
  public static Parser CombineParsers(Parser parser, Parser p2)
  ```

# EZCodeLanguage.PackageClass

This is the equivelent for the [JSON Project](Projects.md#project.json)
```cs
public class ProjectClass
{
    public string Name { get; set; }
    public string[] Files { get; set; }
    public string[]? GlobalPackages { get; set; }
}
```

# EZCodeLanguage.Projects

This is a static class for Projects

- ```cs
  // Returns a project from a file
  public static ProjectClass GetProjectFromPath(string path)
  ```
- ```cs
  // Returns a parser from a project
  public static Parser GetParserFromProject(ProjectClass project, string directory)
  ```
- ```cs
  // Returns a parser that includes a the global packages in the project
  public static Parser IncludeGlobalPackages(Parser parser, ProjectClass project)
  ```
- ```cs
  // Runs and executes the EZCode Project
  public static void Run(ProjectClass project, string directory)
  ```

# EZCodeLanguage.EZHelp

**The Api for making a package is in the [package docs](Packages.md#creating-a-package)**