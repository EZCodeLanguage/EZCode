![Main Image](https://raw.githubusercontent.com/JBrosDevelopment/EZCode/master/docs/Images/EZCode_Wide_Logo.png)

# Welcome!

**Welcome to EZCode!** EZCode is a comprehensive programming language built with C#. EZCode is designed to be easy to use straightforward, *although sometimes it can be tuff*. Refer to [Community](#community) to learn more on how to contribute and where to join our Discord Server!

Install EZCode using the installer from the [latest release](https://github.com/EZCodeLanguage/EZCode/releases/latest) and click **Installer.zip** to download it.
## *EZCode v3.0.0 Example*
```c
// include main package
include main

make ^int {NAME} {VALUE} => int {NAME} new : {VALUE}
// turns: int name 0
// into: int name new : 0
// which is the valid way to create class instance

// color class that stores R, G, B values
// looks for c[R;G;B] and turns that into a new color instance
class color {
    explicit watch c\[{R};{G};{B}\] => set : R, G, B
    int R 0
    int G 0
    int B 0
    method set : @int:r, @int:g, @int:b {
        R = r
        G = g
        B = b
    }
}

// prints color with the new instance of color class c[50;60;90]
printColor : c[50;60;90]

method printColor : @color:c {
    print The color: 'c:R' 'c:G' 'c:B'
}
// Outputs:
// The color: 50 60 90
```

# Command Line

To run EZCode, use the following command (assuming you have installed it [HERE](https://github.com/EZCodeLanguage/EZCode/releases/latest))
> `ez FILE_PATH`

To start a integrated environment, use:
> `ez start`

To create a project, use:
> `ez new project NAME`

View all commands with: 
> `ez help`


# Packages

All the official Packages are in [Packages Repository](https://github.com/EZCodeLanguage/Packages.git). To add your own, create a pull request and it will be looked over. 


# Community

Contribute to the community in many ways including the [EZCode Project Repository](https://github.com/EZCodeLanguage/Projects.git). To contribute, create a pull request and it will be accepted as soon as possible. 

Please join our [Discord Server](https://discord.gg/DpBrp6Zy) to get closer to the community!

# License

EZCode is released under the [MIT License](LICENSE).
