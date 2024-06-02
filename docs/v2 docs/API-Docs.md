# EZCode API

The EZCode API allows EZCode to be incorperated into **C# WinForms** programs. First, either clone EZCode, `git clone https://github.com/JBrosDevelopment/EZCode.git` or install the [Nuget Package](https://www.nuget.org/packages/EZCode). EZCode was made for [.NET 7.0](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

.Net CLI 
> ``` .Net CLI
>  dotnet add package EZCode
>  ```

Package Manager 
> ``` .Net CLI
>  NuGet\Install-Package EZCode
>  ```
To view more ways to install EZCode Package, go to https://www.nuget.org/packages/EZCode.

# Overview
- [EZCode Samples](#ezcode-sample)
- [Set Up EZCode in Program](#set-up-ezcode-in-program)
- [Set Up EZProject in Program](#set-up-ezproject-in-program)
- [Set Up EZPlayer in Program](#set-up-ezplayer-for-program)
- [All Public Methods and Variables](#all-public-methods-and-variables)
- [Debugger In Project](#debugger-in-project)

# EZCode Sample

Example with returning a `string`.
```csharp
using EZCode;

EzCode ezcode = new EzCode();
ezcode.Initialize(_directory:"C:/");
string result = ezcode.Play("EZCode...");
```
Example of using EZCode with WinForms
```csharp
using EZCode;
using System.Windows.Forms;

Panel VisualOutput = new Panel();
RichTextBox Console = new RichTextBox();

EzCode ezcode = new EzCode();
ezcode.Initialize(true, "C:/", VisualOutput, Console);
ezcode.Play("EZCode...");
```
Example of using Running EZCode and EZPlayer with WinForms from Program Entry.
```csharp
using EZCode;
using EZCode.EZPlayer;
using System.Windows.Forms;

internal static class Program
{
    [STAThread] static void Main()
    {
        ApplicationConfiguration.Initialize();

        EzCode ezcode = new EzCode() {
            Code = "EZCode...",
        };
        EZProj ezproj = new EZProj(ezcode);
        EZPlayer player = new Player(ezproj)

        Application.Run(player);
    }
}
```

# Set Up [EZCode](ezcode-docs) in Program

First decide which type of program you are using EZCode for,
- A text based program with only text input/output.
- A [Console](Programs#console) and [Visual Output](Programs#visual-output) program.
- A Window based program.

Create an Instance of EZCode and optionally add the code to run,
```csharp
using EZCode;

EzCode ezcode = new EzCode();
ezcode.Code = "print Hello World!";
```

Next, Initialize the EZCode Instance,
```csharp
ezcode.Initialize(bool inpanel = false, string _directory = "", Control _space = null, RichTextBox _console = null, bool _showFileWithErrors = true, bool _showStartAndEnd = true, bool _clearConsole = true);
```
- `inpanel` if the program is a window or visual output program.
- `directory` the directory the program is running in.
- `_space` the visual output of the program. Any control but usually a panel
- `_console` the console output of the program. Always a RichTextBox.
- `_showFileWithErrors`, `_showStartAndEnd`, and `_clearConsole` are some of the project properties that can be set without using an [EZProject](ezproject-docs).

So for a **text based program**, all of the default values can be used unless you want to input the directory, `Initialize(_directory:"C:\\");`.\
For a **console based program**, input either or both `_space` and `_console`. Also can input the `_directory`, `Initialize(true, "C:/", VisualOutput, Console);`\
For a **window based program**, set the `_inpanel` value to `false` and fill out the `_console` if the program needs a text output, `Initialize(false, "C:/", _console:Console);`

So now an instance of EZCode should be created and initialized to supply the needs of the program. The next step is to add the nessicary event listeners.
```csharp
this.AppDomain.CurrentDomain.UnhandledException += ezcode.CurrentDomain_UnhandledException;

this.KeyDown += ezcode.KeyInput_Down;
this.KeyUp += ezcode.KeyInput_Up;
this.KeyPreview = true;

this.MouseWheel += ezcode.MouseInput_Wheel;
Console.MouseWheel += ezcode.MouseInput_Wheel;
Visualoutput.MouseWheel += ezcode.MouseInput_Wheel;

this.MouseMove += ezcode.MouseInput_Move;
Console.MouseMove += ezcode.MouseInput_Move;
Visualoutput.MouseMove += ezcode.MouseInput_Move;

this.MouseDown += ezcode.MouseInput_Down;
Console.MouseDown += ezcode.MouseInput_Down;
Visualoutput.MouseDown += ezcode.MouseInput_Down;

this.MouseUp += ezcode.MouseInput_Up;
Console.MouseUp += ezcode.MouseInput_Up;
Visualoutput.MouseUp += ezcode.MouseInput_Up;
```
This is nessicary if any [input](EZCode-docs#input) is used in the program, then these event listeners are nessicary. The Unhandled Exception error is optional incase your program takes care of that. Each listener has two methods. For example with Key Down Input,
- `KeyInput_Down(object sender, KeyEventArgs e)`
- `KeyInput_Down(KeyEventArgs e)`

This is like this so that it is easier to call it from a method that already has the event listener. 
```csharp
private void Form1_MouseDown(object sender, EventArgs e)
{
    ezcode.MouseInput_Down(e);
}
``` 

Now [Console Input](#consoleinput) needs to be added into the program,
```csharp
private void SentButton_Click(object sender, EventArgs e)
{
    ezcode.ConsoleInput(ConsoleInput.Text);
}
```
This is just an example assuming your program has a button named, `SentButton` and textbox named, `ConsoleInput`. The Console Input is nessicary if the program uses `input console` anywhere in the EZCode.

Once that is done, the last step is to play the EZCode, 
```csharp
ezcode.Play("print Hello World!");
```

# Set Up [EZProject](EZProject-docs) in Program

Before going over this section, read over the [section](#set-up-ezcode-in-program) before this. After EZCode has been initialized and all of the nessicary event listeners have been added, use the `PlayFromProj` method to play.
```csharp
EzCode ezcode = new EzCode() {
    Code = @"
    name:""Example""
    startup:""~/file.ezcode"""
};
new EZProj proj = new EZProj(ezcode);
ezcode.PlayFromProj(proj);
```
Here is all of the EZProj method inputs,
```csharp
public EZProj() // empty instance
public EZProj(EzCode code, string _file = "") // EZCode for Contents (converts EZCode # project properties) and optionally FilePath
public EZProj(string file) // FilePath and Contents from string
public EZProj(string contents, string filepath) // FilePath and Contents seperately
public EZProj(FileInfo file) // FilePath and Contents from FileInfo
public EZProj(Stream file, string filepath = "") // Contents and optionally FilePath from Stream.
```
In most cases, you want to use `EZProj(EzCode code, string _file = "")` incase `# project properties` are used in the EZCode file with the (see [Special Keywords](EZCode-Docs#project-properties)).

# Set Up EZPlayer For Program

EZPlayer is designed to simplify displaying **Window based Program**, **Console based Programs**, and **Visual Output programs**. It takes care of displaying the console and visual output with a standered console look-alike window.
```csharp
using EZCode.EZPlayer;

public partial class Player : Form
{
    public Player(EZProj eZProj, bool closeAppOnQuit = true)
}
```
All that is needed is the [EZProject](ezproject-docs) to run and it will take care of everything. It is a Form and acts like that so it can be easily used. Here is an example of it being used,
```csharp
using EZCode;
using EZCode.EZPlayer;

EzCode ez = new EzCode();
string code = "Code...";
ez.Code = code;
EZProj proj = new EZProj(ez);
Player player = new Player(proj);
player.Open();
```
This makes it extremely easy to run an EZCode file. This is what the standered console looks like for Console Programs. It also contains a Visual Output that will display depending on the program type.

![Console Picture](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Console_Picture.png)

# All Public Methods and Variables

## Methods

### `AddText()`

This Appends text the console and [`ConsoleText`](#consoletext).
```csharp
public void AddText(string text, bool error = false, RichTextBox? control = null, bool? newLine = true)
```
- `text` is the text to append to the console.
- `error` is if the text is an error and should be added to [`errors`](#variables) and displayed with [`errorColor`](#variables).
- `control` overides the [`RichConsole`](#variables).
- `newLine` if the text should be added on a new line.

### `ConsoleInput()`

This is how EZCode recieves [console input](EZCode-Docs#input). If `help` is inputted and it is not playing, this will be sent to the console, `@"Need help? Please go to the official EZCode website: https://ez-code.web.app"`. 
```csharp
public void ConsoleInput(string text)
```

### `CurrentDomain_UnhandledException()`

This is the event listener form any unhandled exception in the current domain. 
```csharp
public void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    var ex = e.ExceptionObject as Exception;
    ErrorText(new string[0], ErrorTypes.unkown);
}
```

### `ErrorText()`

This is used to make it easier to add an error to the console.

```csharp
public string ErrorText(string[] parts, ErrorTypes error, string keyword = "keyword", string name = "name", string custom = "An Error Occured", bool returnoutput = true, bool dontshowsegment = false, bool dontshowcode = false)
```
- `parts` The array of the current line split by spaces `line.Split(" ")` to check if there is a [`# suppress error`](EZCode-Docs#suppress-error) in the line.
- `error` The type of error from [`ErrorTypes`](#ErrorTypes) enum.
- `keyword` The keyword (first word in line `parts[0]`) of the line.
- `name` The name of control/variable.
- `custom` The custom error text.
- `returnoutput` If the error should be added to the output.
- `dontshowsegment` If the error should not show the segment the error occured in with [`codeLine`](#variables).
- `dontshowcode` If the error should not show the line the error occured in `string.Join(" ", parts)`

Here is the resulting error depending on the [error type](#errortypes).
```csharp
string text =
    error == ErrorTypes.unkown ? $"An error occured in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.normal ? $"An error occured with '{keyword}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.violation ? $"Naming violation in {SegmentSeperator} {codeLine}. '{name}' can not be used as a name" :
    error == ErrorTypes.missingControl ? $"Could not find a Control named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.missingVar ? $"Could not find a Variable named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.missingSound ? $"Could not find a Sound Player named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.missingGroup ? $"Could not find a Group named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.missingWindow ? $"Could not find a Window named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.alreadyMember ? $"Naming violation in {SegmentSeperator} {codeLine}. There is already a '{keyword}' named '{name}'" :
    error == ErrorTypes.errorEquation ? $"Unable to solve the equation in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.methodnamingvoilation ? $"Can not name '{keyword}' as '{name}' because there is a method already named '{name}' in {SegmentSeperator} {codeLine}" :
    error == ErrorTypes.custom ? $"{custom}{(!dontshowsegment ? $" in {SegmentSeperator} {codeLine}" : "")}" :
    "An Error Occured, We don't know why. If it helps, it was on line " + codeLine;
```

### `Intitialize()`

This is nessicary before playing the project.
```csharp
ezcode.Initialize(bool inpanel = false, string _directory = "", Control _space = null, RichTextBox _console = null, bool _showFileWithErrors = true, bool _showStartAndEnd = true, bool _clearConsole = true);
```
- `inpanel` if the program is a window or visual output program.
- `directory` the directory the program is running in.
- `_space` the visual output of the program. Any control but usually a panel
- `_console` the console output of the program. Always a RichTextBox.
- `_showFileWithErrors`, `_showStartAndEnd`, and `_clearConsole` are some of the project properties that can be set without using an [EZProject](ezproject-docs).

### `KeyInput_Down`

The Key Down listener.
```csharp
public void KeyInput_Down(object sender, KeyEventArgs e) => KeyInput_Down(e);
public void KeyInput_Down(KeyEventArgs e)
{
    Keys.Add(e.KeyCode);
}
```

### `KeyInput_Up`

The Key Up listener.
```csharp
public void KeyInput_Up(object sender, KeyEventArgs e) => KeyInput_Up(e);
public void KeyInput_Up(KeyEventArgs e)
{
    Keys.Remove(e.KeyCode);
}
```

### `MouseInput_Down`

The Mouse button down listener.
```csharp
public void MouseInput_Down(object sender, MouseEventArgs e) => MouseInput_Down(e);
public void MouseInput_Down(MouseEventArgs e)
{
    mouseButtons.Add(e.Button);
}
```

### `MouseInput_Move`

The Mouse move event listener.
```csharp
public void MouseInput_Move(object sender, MouseEventArgs e) => MouseInput_Move(e);
public void MouseInput_Move(MouseEventArgs e)
{
    MousePosition = Cursor.Position;
}
```

### `MouseInput_Up`

The Mouse button up listener.
```csharp
public void MouseInput_Up(object sender, MouseEventArgs e) => MouseInput_Up(e);
public void MouseInput_Up(MouseEventArgs e)
{
    mouseButtons.Remove(e.Button);
}
```

### `MouseInput_Wheel`

The Mouse wheel event listener.
```csharp
public void MouseInput_Wheel(object sender, MouseEventArgs e) => MouseInput_Wheel(e);
public void MouseInput_Wheel(MouseEventArgs e)
{
    mouseWheel = e.Delta;
}
```

#### `Play()`

This plays the inputted EZCode. This instance has to be [initialized](#intitialize) before playing.
```csharp
public async Task<string> Play(string code, bool clearsconsole = true, Debugger? debugger = null)
```
- `code` this is the code that will play
- `clearconsole` this will overide to not clear the console, `if (`[`ClearConsole`](#variables)` && clearsconsole) RichConsole.Clear();`
- `debugger` the instance of [debugger](#debbuger) that can be in the program.

### `PlayFromProject()`

This will Play an [EZProject](EZProject-Docs) using EZCode.

```csharp
public async Task<string> PlayFromProj(EZProj proj)
```
This is the code that runs,
```csharp
foreach (string error in proj.Errors)
{
    ErrorText(new string[0], ErrorTypes.custom, custom: error);
}
return await Play(proj.Program, false);
```

### `ScrollToEnd()`

This is used to scroll the end of the console whenever it's text changed.
```csharp
public RichTextBox ScrollToEnd(bool scrollToEnd, Color? output = null, Color? error = null, RichTextBox? Console = null, bool? showFileError = null)
```
- `scrollToEnd` If the RichTextBox should scroll to its end
- `output` The normal output color for the console
- `error` The error color for the output
- `console` Sets the [Console](#variables) if it is not already set, `RichConsole = Console != null ? Console : RichConsole;`

Here is the common use of it,
```csharp
private void Console_TextChanged(object sender, EventArgs e)
{
    ezcode.ScrollToEnd(true, Color.Black, Color.Red);
}
```

### `SetStringDirectory()`

This chacks the inputted script directory and sets it to it if it is valid.
```csharp
public bool SetScriptDirectory(string scriptDirectory)
{
    if (EZProj.validfile(scriptDirectory))
    {
        ScriptDirectory = scriptDirectory;
        return true;
    }
    else
    {
        return false;
    }
}
```

### `Stop()`

This stops the current EZCode run.
```csharp
public void Stop()
```
This stops the EZCode run as well as any [sounds](EZCode-Docs#sound) going on. 

## Enums

### `ErrorTypes`

This is used for [`ErrorText`](#errortext) method.
```csharp
public enum ErrorTypes
{
    none,
    violation,
    normal,
    missingControl,
    alreadyMember,
    missingVar,
    missingSound,
    missingGroup,
    missingWindow,
    errorEquation,
    methodnamingvoilation,
    unkown,
    custom
}
```

## Variables

- `public static Icon EZCodeIcon`: The Official EZCode Icon
- `public string ScriptDirectory`: Directory of the script playing
- `public HashSet<MouseButtons> mouseButtons`: List of mouse buttons being pressed
- `public Point MousePosition`: Position of the mouse
- `public int mouseWheel`: Delta of the Mouse Wheel
- `public Color errorColor`: The output color of an error
- `public Color normalColor`: The normal output color
- `public List<Player> sounds`: List for Audio output players
- `public List<GLabel> labels`: List for Labels
- `public List<GTextBox> textboxes`: List for textboxes
- `public List<GButton> buttons`: List for buttons
- `public List<GShape> shapes`: List for shapes
- `public List<Window> windows`: List for windows
- `public List<Var> vars`: List for variables
- `public List<Group> groups`: List of groups
- `public List<Control> AllControls`: List of all controls
- `public bool showFileInError`: Shows the file directory whenever an error occurs. Recommended for debugging.
- `public bool showStartAndEnd`: Shows the 'Build Started' and 'Build Ended' console values.
- `public bool ClearConsole`: Clear the console after each build
- `public bool ClearAfterBuild`: Clear the Visual output after the build
- `public int codeLine`: Current line that is being ran
- `public RichTextBox RichConsole`: RichTextbox for the console
- `public string ConsoleText`: The output console
- `Control Space { get; set; }`: The visual output. Usually a Panel
- `public string Code { get; set; }`: Code to be ran
- `public bool playing`: Bool to decide if the script is playing
- `public static string[] UnusableNames`: string array for naming violations
- `public static char[] UnusableContains`: char array for unusable names that can't even be used once in the name
- `public static char[] seperatorChars`: The character that separates each line of code. Automatically { '\n', '|' } but this can be added to if needed
- `public HashSet<Keys> Keys`: List of keys being pressed. Needs to have Key_Down and Key_Up event connected to KeyInput_Down and KeyInput_Up
- `private bool isEZText`: If the code is currently running EZText
- `public static readonly string SegmentSeperator = "segment"`: The character that separates each segment of code
- `public bool RefreshOnControl`: Refreshes screen when a control is changed/created
- `public bool InPanel { get; set; }`: Decides if the program is in a panel or something, or if it is used to open a new window.
- `private List<Method> methods`: The list of methods in the program
- `public List<string> Errors`: List of errors

# Debugger in Project

To quickly go over setting up a debugger for an EZCode project, first create an instance of the class.
```csharp
public Debugger(EzCode ezcode, Breakpoint[] breakpoints, TextBox? highlightTextBox = null, TextBox? currentLineTextbox = null)
```
- `ezcode` The EZCode class
- `breakpoints` The Breakpoints to stop for
- `highlightTextBox` The Textbox to highlight the current line. This is optional and the highlighting is not always accurate.
- `currentLineTextbox` The textbox to display the current line. This is optional

This is a one way to initialize and start session. Make sure the EZCode class is already [initialized](#intitialize). This the code used in the [IDE](Programs#ez-ide),
```csharp
Debugger Debug = new Debugger(ezcode, DebugSave.Breakpoints, FCTB_Highlight, CurrentLine);
Debug.StartDebugSession(ezproj.Program);
```