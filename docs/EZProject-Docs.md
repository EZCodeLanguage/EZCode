# EZProj

<details open>
<summary>Directory</summary>

- [Syntax](#syntax)
- [Properties](#properties)
- [Variables](#vars)
- [Comments](#comments)
- [Code Seperation](#code-seperation)
- [Examples](#examples)
</details>

## Syntax

The Syntax is simple, 
```
name:"Hello World Test"
icon:"~/icon.ico"
startup:"~/Main.ezcode"
```
It's the [property](#propery), a colon `:`, and then the value in quotations `""`.

## Properties

<details open>
<summary>Properties</summary>

- [Clearconsole](#clear-console)
- [Closeonend](#close-on-end)
- [Debug](#debug)
- [Exclude](#exclude)
- [Fileinerror](#file-in-error)
- [Icon](#icon)
- [Include](#include)
- [Isvisual](#is-visual)
- [Name](#name)
- [Showbuild](#show-build)
- [Startup](#start-up)
- [Window](#window)
</details>

### Clear Console

The `clearconsole` property will clear the console before playing the program.

Default = "True"

### Close On End

the `closeonend` property will Stop the program when the program ends.

Default = "True"

### Debug

The `debug` property will Open a Debug window if the project property [Window](#window) is true. This Debug window contains a console and a quit button.

Default = "False"

### Exclude

The `exclude` property excludes files in the program. It takes a full or local file path (`~/` or `~\`) that has the EZCode extension `.ezcode`. It can also take `"all"` or `"folder"`. 
- `"all"`: will get all of the files in the current directory. It will get all `.ezcode` files including sub directories.
- `"folder"`: will get all of the files in the current folder. It will not get any sub directories.

This is especially used when [Include](#include) proprety has `"all"` but a file or two needs to be excluded.

### File In Error

The `fileinerror` will show the file path the error occured in when an error occurers.

Default = "True"

### Icon

The `icon` property is the Icon of the program. It takes a file path with a `.ico` extension. The path can be local using `~/` or `~\`.

Default = "C:\ProgramData\EZCode\EZCode\EZCode_Logo.ico"

### Include

The `include` property includes files into the program. It takes a full or local file path (`~/` or `~\`) that has the EZCode extension `.ezcode`. It can also take `"all"` or `"folder"`. 
- `"all"`: will get all of the files in the current directory. It will get all `.ezcode` files including sub directories.
- `"folder"`: will get all of the files in the current folder. It will not get any sub directories.

### Is Visual

The `isvisual` proprety will tell the program that the code uses the [visual-output](Programs#visual-output).

Default = "False"

### Name

The `name` property sets the name of the program.

Default = "EZCode_v{EzCode.Version}"

### Show Build

The `showbuild` property will show `Build Started` and `Build Ended` whenever the program starts and ends.

Default = "False"

### Start Up

The `startup` property sets the start up file for the program. Takes a full or local path (`~/` or `~\`). If there is only one file in the program, then using both `include` and `startup` is not needed, just choose one of them. See the example in [Syntax](#syntax).

The Default is the first file that was included. If includes `"all"`, the first on alphabetically.

### Window

The `window` property tells the program that this program uses one or more windows.

## Variable

This is a more complex use of EZProj syntax. To declare a variable, just put its name in quotation marks and the value on the other side of the colon also in quotation marks,
```
"varName":"value"
```
Then to use the variable,
```
isvisual:"varName"
```

Another feature of variables is to read a file and get its value from that. It will read the file if the value side does not contain any quotations. Here is an example,
```
"varName":~/file.txt
```

## Comments

Comments are apart of the code that doesn't run. It is started with // and the rest of that line is excluded from being executed. Here is an example, `name:"Hello World Test" // This sets the project name to 'Hello World Test' and doesn't cause any errors becaues of the presence of '//'`.

## Code Seperation

Each line of code is seperated by a newline, or the pipe character `|`. Here is an example,

```
isvisual:"true" | clearconsole:"false"
```

## Examples

Here is an example of a program that contains [windows](ezcode-docs#window).
```
name:"Hello World Test"
window:"true"
icon:"~/icon.ico"
startup:"~/Main.ezcode"
include:"all"
```

Here is an example of a program that uses the [visual-output](Programs#visual-output).
```
name:"Visual Test"
isvisual:"true"
startup:"~\Main.ezcode"
```

Here is an example of a program that just uses the [console](Programs#console).
```
name:"Console Test"
startup:"~\Main.ezcode"
```