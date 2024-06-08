# Packages Docs

- [Packages Docs](#packages-docs)
  - [Using A Package](#using-a-package)
  - [Package.json](#packagejson)
    - [Name](#name)
    - [Files](#files)
    - [Configuration](#configuration)
      - [Library Directory](#library-directory)
      - [GlobalPackages](#globalpackages)
  - [Creating a Package](#creating-a-package)
    - [Creating the Library](#creating-the-library)

## Using A Package

To include a package and have access to it in EZCode, use the [include](EZCode.md#include) keyword. If the package needs to be installed, use `ez i NAME` where `NAME` is the name of the package ([CLI Documentation](CLI.md#install-or-i)). Once included in the environment, all methods and classes can be accessed accordingly. You can also include it as a global package to a [package.json](#packagejson) or a [project.json](Projects.md#projectjson). 

## Package.json

The package.json is the config file for the EZCode Package. It uses the custom [schema](https://raw.githubusercontent.com/EZCodeLanguage/Packages/main/ezcode.package.schema.json) from the [package repo](https://github.com/EZCodeLanguage/Packages)

Here is a sample of the package.json

```json
{
    "$schema":"https://raw.githubusercontent.com/EZCodeLanguage/Packages/main/ezcode.package.schema.json",
    "Name":"NAME",
    "Files": [
        "NAME.ezcode"
    ],
    "Configuration":{
        "LibraryDirectory":"path/to/lib/",
        "GlobalPackages":[
            "main"
        ]
    }
}
```

### Name

*REQUIRED PROPERTY* \
This is the name of the package. It **needs** to match with the directory name.

### Files

*REQUIRED PROPERTY* \
This is all of the EZCode files included in the package. It doesn't matter the order of the files or if all of the classes/methods are in 1 file or multiple files.

### Configuration

*OPTIONAL PROPERTY* \
This is not mandatory to include the json package. 

#### Library Directory

*OPTIONAL PROPERTY* \
This is used for the packages that require C# libraries. 
```json
"LibraryDirectory":"subdir/bin/release/dotnet/"
```
It is not nessicary for the *dll files* to be in there own sub directory, but it is strongly encoureged. The It should also be stated that to create a pull request to the [package repo](https://github.com/EZCodeLanguage/Packages) with yout package in it, the entire C# solution needs to be in a sub directory of the package and the **LibraryDirectory** in the package json needs to point to the release folder.

#### GlobalPackages

*OPTIONAL PROPERTY* \
This is an array of all the packages that should be included in this package. It is the same as using [include](EZCode.md#include) to include the package/s in all of the files. Most packages usually include `main` for ease of use, but it isn't required. 

## Creating a Package

Creating a package is extremely simple. 

1. Type in where `NAME` is the name of your package ([CLI Docs](CLI.md#new)):
   ```
   ez new package NAME
   ```
2. This will create a directory with the outline to a package. Add any classes or methods you want to the package.
3. Update the [package.json](#packagejson) if neccissary.
4. If you need to create a C# liibrary with your Package, go to [Creating the Library](#creating-the-library).
    - Add the [library directory](#library-directory) to the [package.json](#packagejson). 
    - Make sure the source code for the library is in a subdirectory in the package folder. This is to allow it to be open source and easy to see what the package/library actoually does.
5. Once the package is finished, you can do the following:
   1. Add it to the Package repo so it can be installed by anyone with just `ez i NAME`. Go to https://github.com/EZCodeLanguage/Packages/pulls to create the pull request from a fork.
   2. Place your package in the package directory `C:\Users\%USERNAME%\EZCodeLanguage\Packages` where it can be accessed from any EZCode program with [include](EZCode.md#include) or as a [global package](#globalpackages) to another package.


### Creating the Library

This is to create a C# library for your EZCode package. You will want to create a new directory and have dotnet installed. 

Type in the command to create a new dotnet project (or create a new one with Visual Studio or something):

> `dotnet new classlib`

This will create a `Class1.cs` and a `csproj` file. Next, you will want to install the [EZCode Nuget Package](https://www.nuget.org/packages/EZCode), this can be done through the command line or through Visual Stuidio's package manager:

> `dotnet add package EZCode`

You might want to rename `Class1.cs` to something more specific. You also will want to rename the class inside of it and use the nuget package:

```cs
using EZCodeLanguage;

namespace Package
{
    class PackageName 
    {

    }
}
```

The next step is to create functions inside of this class. They can be static or nonstatic, it doesn't matter. You will need the following methods from [EZCodeLanguage.EZHelp](API.md#ezcodelanguageezhelp):
- `public static T GetParameter<T>(object obj)`
  - This is nessicary to get the parameter inputted into the function. 
- `public static T GetParameter<T>(object obj, string type)`
  - This is nessicary to get the parameter inputted into the function. 
- `public static object GetParameter(object obj, string type)`
  - This is nessicary to get the parameter inputted into the function. 
  - Returns an object from the `obj` (the value to get) and `type` (the type to get from (`"str"`, `"custom-class"`)) 
- `public static Exception ThrowError(string message)`
  - Wrap all functions in a try catch statement and put this in the catch part. 
  - This is to make sure the error message will display in the console correctly
- `public static Exception ThrowError(Exception exception)`
  - Wrap all functions in a try catch statement and put this in the catch part. 
  - This is to make sure the error message will display in the console correctly
  
Here is an example of a function that adds two numbers together:

```cs
using EZCodeLanguage;

namespace CustomMath;
class Math
{
    // function needs to be public and parameters need to be an object
    public static float Add(object _left, object _right) 
    {
        try 
        {
            float left = EZHelp.GetParameter<float>(_left);
            float right = EZHelp.GetParameter<float>(_right);
            return left + right;
        }
        catch (Exception ex) 
        {
            // This is for error messages to correctly be shown in output
            throw EZHelp.ThrowError(ex);
        }
    }
}
```

You can create a custom class and use that as a type. This requires an EZCode class with the same name and the property `Value` to be present. Here is an example:

```js
// EZCode class
class custom {
    undefined Value
    explicit params => set : PARAMS
    method set : val {
        Value => runexec FileName.dll.Namespace.Class.SetFunction ~> {val}  
    }
    get => @str {
        return runexec FileName.dll.Namespace.Class.GetFunction ~> {Value}  
    }
}
```

```cs
// C# Class

// This puts static function in scope from EZHelp class
using static EZCodeLanguage.EZHelp; 

namespace Namespace
{
    // make sure this has the same name as the EZCode class 
    class custom 
    {
        public int X { get; set; }
        public int Y { get; set; }
        // return class instance from string value
        public static custom Parse(string val) 
        {
            // parse X;Y 
            string[] parts = val.Split(";");
            if (parts.Length != 2) throw new Exception("Expects syntax, 'X;Y'");
            var c = new custom() 
            {
                X = int.Parse(parts[0].Trim()),
                Y = int.Parse(parts[1].Trim()),
            }
            return c;
        } 
    }
    // This is the class that holds the functions to call
    class Class
    {
        // Returns 'custom' from a string
        public static custom SetFunction(object _val) 
        {
            try 
            {
                string val = GetParameter<string>(_val)
                return custom.Parse(val)
            }
            catch (Exception ex) 
            {
                throw ThrowError(ex)
            }
        } 
        // Returns string from 'custom'
        public static string GetFunction(object _value) 
        {
            try 
            {
                custom instance = GetParameter<custom>(_value)
                return $"{instance.X}, {instance.Y}"; // returns X, Y
            }
            catch (Exception ex) 
            {
                throw ThrowError(ex)
            }
        } 
    }
}
```

```js
// Example EZCode

custom c new : 50;90 // create an instance of 'custom' 
print c // return 'c' as a string value
```