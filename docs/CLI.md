# CLI Docs

The CLI is the Command Line Interface with the `ez` program.

Once you have installed EZCode and restarted your computer, you can type `ez` into the command line a interact with the CLI

- [CLI Docs](#cli-docs)
  - [**help**](#help)
  - [**version**](#version)
  - [**run**](#run)
  - [**start**](#start)
  - [**install** or **i**](#install-or-i)
  - [**uninstall** or **u**](#uninstall-or-u)
  - [**new**](#new)
  - [**PATH**](#path)

## **help**

If you need a list of all of the possible commands, type in:

```
ez help
```

```
> ez help
All commands:

help                Writes all of the possible commands
version             Writes the current version of EZCode installed
run [CODE]          Runs a line of code. 'main' Package is already imported
start               Starts an EZCode environment
install [NAME]      Installs a package from github repo https://github.com/EZCodeLanguage/Packages.git
uninstall [NAME]    Uninstalls the package
new [TYPE] [NAME]   Creates an empty [Project], [Package], [Class] in the directory
[FILEPATH]          Runs file
```

## **version**

Outputs the current version of EZCode
```
> ez version
3.0.0
```

## **run**

Runs a singe line of EZCode
```
> ez run print Hello World
Hello World
```

## **start**

This starts an integrated environment in the command line
```
> ez start
EZCode Environment started. 'main' Package already included
Commands: END to end environment, RUN to run program, RESTART to restart environment, LIST to list all line in the program, BACK to remove line before it, and CLEAR to clear the screen,
```

To run some code in here, type the code and input `RUN`.
```
print Hello World
RUN
Hello World
RUN ENDED
```

## **install** or **i**

This command is used to install a package from the [Packages Repo](https://github.com/EZCodeLanguage/Packages.git). It will install the package to `C:\Users\%USERNAME%\EZCodeLanguage\Packages`

```
> ez install NAME
```

This will install the package so it can be included in programs. 

## **uninstall** or **u**

This command is used to uninstall a package from `C:\Users\%USERNAME%\EZCodeLanguage\Packages`

```
> ez uninstall NAME
```

This will uninstall the package so that it can't be included in programs. 

## **new**

This will create a `project`, `package`, or `class` in the directory. 

To create a new project sample:
```
> ez new project NAME
```

This will add a new directory:

```
.. 
|- NAME
     |- project.json
     |- entry.ezcode
```

To create a new Package:
```
> ez new package NAME
```

This will add a new directory sample,

```
.. 
|- NAME
     |- package.json
     |- NAME.ezcode
```

To create a new class sample:
```
> ez new class NAME
```

This will add a new file,

```
.. 
|- NAME.ezcode
```

## **PATH**

This runs EZCode from a file path

```
> ez path/to/file.ezcode
```

