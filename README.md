# EzCode

Welcome to the EzCode source code repository! This repository contains the code for the EzCode project, which includes the IDE, Viewer, Packager, and Player.

## Project Links

- [Project Repository](https://github.com/JBrosDevelopment/EZCode-Projects.git): This repository contains various projects created using EzCode. You can explore and contribute to different examples and applications built with EzCode.

## Features

- **Integrated Development Environment (IDE)**: The IDE allows you to write, edit, and debug EzCode scripts, providing a user-friendly coding experience.

- **Viewer**: The Viewer lets you visualize and interact with EzCode programs, making it easy to see the output and behavior of your code.

- **Packager**: The Packager allows you to bundle your EzCode projects into distributable packages that can be shared and executed independently.

- **Player**: The Player is a standalone runtime environment for EzCode, enabling you to run EzCode programs without the IDE.

## Getting Started

To get started with EzCode, you can follow these steps:

1. Clone the main repository: `git clone https://github.com/JBrosDevelopment/EZCode.git`

2. Explore the different components of EzCode, including the IDE, Viewer, Packager, and Player.

3. Contribute to the project by fixing bugs, adding new features, or creating example projects.

## Contribution Guidelines

We welcome contributions to the EzCode project! If you would like to contribute, please follow the guidelines in the [CONTRIBUTING.md](CONTRIBUTING.md) file.

## License

EzCode is released under the [MIT License](LICENSE).

## Contact

If you have any questions or feedback, feel free to reach out to us at [jbrosdevelopment@gmail.com](mailto:jbrosdevelopment@gmail.com).

## Documentation

For detailed documentation, tutorials, and examples, please visit the [EzCode Documentation](https://ez-code.web.app/Docs.html). It contains everything you need to get started, including installation instructions, language syntax, and code samples.

## Community and Support

Join the EzCode community to connect with other developers, ask questions, and share your projects:

- **Forums**: Visit our [community forums](https://ez-code.web.app/Forum.html) to engage in discussions, get help, and share your ideas.

- **News and Updates**: Stay up to date with the latest news, announcements, and updates about EzCode on our [News](https://ez-code.web.app) page.

## Contributing

We welcome contributions from the community to help improve and grow the EzCode programming language. If you have any suggestions, bug reports, or feature requests, please submit them to the [GitHub repository](https://github.com/JBrosDevelopment/EZCode/tree/master).

If you want to explore some EZCode projects, please go to the [Projects Repository](https://github.com/JBrosDevelopment/EZCode-Projects).

---

# Basic Syntax
| Keyword               | Syntax                                  | Example                                  |
|-----------------------|-----------------------------------------|------------------------------------------|
| Await                 | await miliseconds                       | await 1000                               |
| Button                | button name text                        | button btn Press\_Me                     |
| Button Click          | buttonClick btnName fileLocation        | button btn ~\btn_Click.ezcode            |
| Color                 | color controlName r g b                 | color object1 210 210 210                |
| Comments              | // comments                             | // this code will not run                |
| Clear Console         | ClearConsole                            | ClearConsole                             |
| Creating Errors       | # create error                          | # create error                           |
| Suppress Errors       | # suppress error                        | # suppress error                         |
| End Build             | endBuild                                | endBuild                                 |
| Font                  | font controlName weight size            | font label_1 Bold 14                     |
| Create Group          | group new name                          | group new groupName                      |
| Add to Group          | group name add controlType controlName  | group name add Button btn                |
| Modify Group          | group name change type modifier v1 v2 v3| group name change rel move 20 20         |
| If Statements         | if value1 mid value2 :                  | if 10 > 9 : print code                   |
| Image                 | image objectName PathToFile             | image obj ~/image.png                    |
| Label                 | label labelName                         | label label1                             |
| New List              | list new name : v1,v2,v3                | list new even_numbers : 2,4,6,8,10       |
| Add to List           | list add name val                       | list add even__numbers 12                |
| Modify List Value     | list equals name index val              | list equals even__numbers 3 9            |
| Clear List            | list clear name                         | list clear even__numbers                 |
| Loop                  | loop loopTimes (ON DIFFERENT LINE): end | loop 10  (NextLine): end                 |
| Move                  | move ControlName x y                    | move obj1 10 10                          |
| Multi-Line            | multiLine textboxName value             | multiLine tb1 true                       |
| Object                | object name sides                       | object obj1 4                            |
| playFile              | playFile modifier PathToFile            | playFile await ~/file.ezcode             |
| Print                 | print text                              | print hello world                        |
| Scale                 | scale controlName x y                   | scale button1 15 20                      |
| Play Sound            | sound sound play PathToFile             | sound play ~/sound.mp3                   |
| Stop Sound            | sound stop                              | sound stop                               |
| Sound Volume          | sound volume value                      | sound volume 0.8                         |
| Textbox               | textbox name                            | textbox tb1                              |
| Declare Varibles      | var name value                          | var variable1 10                         |
| Set Variable          | varName mid value                       | variable1 + 25                           |
| Variable Value        | varName modifier                        | variable1 KeyInput                       |
| Write                 | write controlName text                  | write label1 hello world!                |
| WriteToFile           | writeToFile value PathToFile            | writeToFile variable1 ~/file.txt         |
