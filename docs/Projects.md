# Projects Docs

- [Projects Docs](#projects-docs)
  - [Project.json](#projectjson)
    - [Name](#name)
    - [Files](#files)
  - [Creating a Project](#creating-a-project)
  - [Running a Project](#running-a-project)
 

## Project.json

The project.json is the config file for the EZCode Project. It uses the custom [schema](https://raw.githubusercontent.com/EZCodeLanguage/Projects/main/ezcode.project.schema.json) from the [projects repo](https://github.com/EZCodeLanguage/Projects)

Here is a sample of the project.json

```json
{
    "$schema":"https://raw.githubusercontent.com/EZCodeLanguage/Projects/main/ezcode.project.schema.json",
    "Name":"NAME",
    "Files": [
        "NAME.ezcode"
    ]
}
```

### Name

*REQUIRED PROPERTY* \
This is the name of the project

### Files

*REQUIRED PROPERTY* \
This is all of the EZCode files included in the project. It doesn't matter the order as long as there is the [entry](EZCode.md#entry), `method start`.

## Creating a Project

To create a project, use the [ez CLI](CLI.md#new):

> `ez new project NAME`

This will create a [`project.json`](#projectjson) and `entry.ezcode`. You can add more files that can hold methods or classes that can be called.

## Running a Project

To run the project from the command line, use:

> `ez "path/to/json"`

To run it from C#, use the [API](API.md#ezcodelanguageezcode) function `EZCodeLanguage.EZCode.RunProject`.