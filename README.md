# EZCode

![Main Image](docs/Images/EZCode_Wide_Logo.png)

---

<details open>
<summary><h2>Welcome!</h2></summary>

**Welcome to EZCode!** EZCode is a comprehensive programming language built off of Microsoft WinForms. EZCode strives to make it as easy as possible to build a Windows program ranging from a Visual Application to simple console programs. Refer to [Community](#community) to learn more on how to contribute and where to join our Discord Server!
</details>

<details open>
<summary><h2>Docs</h2></summary>

The [Official Docs](https://github.com/JBrosDevelopment/EZCode/wiki/EZCode-Docs) are on the our [GitHub Wiki Page](https://github.com/JBrosDevelopment/EZCode/wiki). Please refer to this for any detailed instrictions.
</details>

<details open>
<summary><h2>Community</h2></summary>

Contribute to the community in many ways including the [EZCode Project Repository](https://github.com/JBrosDevelopment/EZCode-Projects.git) for the community. Create a pull request and I will accept as soon as I can. There is also the [Discussion Board](https://github.com/JBrosDevelopment/EZCode/discussions) for anybody who has questions or wants to share. If you need any help, you can look over the [Wiki](https://github.com/JBrosDevelopment/EZCode/wiki) which includes instructions and documentation.

Please join our [Discord Server](https://discord.gg/DpBrp6Zy) to get closer to the community!
</details>

<details open>
<summary><h2>Example</h2></summary>

This is a simple example of a program where the `X_Scale` and `Y_Scale` print out a square of `char` characters. It outputs a 12 x 10 square of `%` with spaces between the. Look over this code and see if you can follow along with thte comments.

```ezcode
// Set dimensions of square
var x_Scale 12
var Y_Scale 10

// Character for square
var char %

// Create and set the 'txt' var to the correct dimensions
var txt
var interval 0
loop Y_Scale 
{
    interval + 1
    loop x_Scale 
    {
        txt + 'char'\_
    }

    // Check if this is the last row before adding a newline
    if interval <= Y_Scale : txt + \n
}

// Print the output
print 'txt'

// Outputs:
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
// % % % % % % % % % % % % 
```
You can change the `X_Scale` and `Y_Scale` variables as well as the character being used for the square shape.
</details>

<details open>
<summary><h2>License</h2></summary>

EzCode is released under the [MIT License](LICENSE).
</details>

<details open>
<summary><h2>Overview</h2></summary>

EZCode is a multipurpose programming language built off of C# and WinForms. It has simple syntax and is designed to be user friendly. Use it no matter your programming experince. From just getting started to a seasoned developer, EZCode can help you develop your projects with a minimal amount of code. Download the **[Official Installer](https://github.com/JBrosDevelopment/EZCode/releases/latest)** and choose from the options on what to install.

#### EZCode Player

This is a **neccessary option** for EZCode and will play any EZCode file (.ezcode or .ezproj). It will play the file no matter what type of project it is **(Windows, Console, Console + Visual, or Console + Windows)**.

#### SLN Builder

This is an optional application to install and it creates a Microsoft Visual Studio Project from an EZProject file **(.ezproj)**. Input the project and the desired output directory.

</details>

<details open>
<summary><h2>Basic Syntax</h2></summary>

<details open>
<summary><h3>Keywords</h3></summary>

| Keyword | Syntax | Function |
|---------|--------|----------|
| Await | await miliseconds | Delays the program by miliseconds |
|       | await argument | Delays program until argument is true |
| BringTo | bringto front control | Brings a control to the front of the screen |
|         | bringto back control | Sends cotrolto the back of the screen |
| Button | button name | Creates a button control |
|        | button name properties | Creates a button control and sets its properties |
| Clear | clear | Clears the console |
|       | clear argument | Clears the console if argument is true |
| Destroy | destroy control | Destroys the control completely from the program |
|         | destroy control argument | Destroys the control only if the argument is true |
| Else | else : | Goes after an 'If' statement and is only executed if the 'If' is false. Only executes the line of code after the colon, whether it is on the same line or not |
|      | else : { } | Goes after an 'If' statement and is only executed if the 'If' is false. Executes only the code in the brackets |
| Event | event control_or_window eventType method | Executes the method if event is triggered.
|       | event control_or_window eventType filepath | Executes the file (assuming ezcode) if event is triggered.
| File | file read filepath | Reads the file and outputs the text as a var through using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file write text_or_var filepath | Writes to the file and outputs successful *1* or unsuccessful **0* using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file validpath filepath | Checks if the inputted file is a correct file path and outputs *1* as yes and *0* as no using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file play filepath | Plays the file (assuming ezcode) and outputs the output of the program through ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file playproj filepath | Plays the file (assuming ezproj) and outputs the output of the program through ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file create filepath | Creates the file and outputs successful *1* or unsuccessful *0* using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file exists filepath | Checks if the inputted file exists and outputs *1* as yes and *0* as no using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|      | file delete filepath | Deletes the file and outputs successful *1* or unsuccessful *0* using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
| Global | global var ... | Creates a variable and makes it global, meaning it can be accessed from any method. See 'var' for details |
|        | global controlType ... | Creates a control and makes it global, meaning it can be accessed from any method See any of the controls (button, label, shape, or textbox) fot details |
|        | global list ... | Creates a list and makes it global, meaning it can be accessed from any method. See 'list' for details |
|        | global group ... | Creates a group and makes it global, meaning it can be accessed from any method. See 'group' for details |
| Group | group name new : controls, ...  | Creates a group that can hold as many controls as needed |
|       | group name add control | Adds a control to the group |
|       | group name equals index control | Finds a control by its index and sets it to the inputted control |
|       | group name remove index_or_name | Removes a control from the group by its index or name |
|       | group name clear | Clears all controls from the group |
|       | group name destroy | Destroys the group completely |
|       | group name destroyall | Destroys the group and all controls in it |
|       | group name change : properties | Changes all controls in the group relative to the inputted properties (Ex: controls's 'x' value is '5.' the inputted properties are '10' so the new control's properties are '15') |
|       | group name argument change : properties | If the agrument is true, this changes all controls in the group to the properties, else it changes them relative to the controls properties |
| If | if argument : | Executes the line if the argument is true. Only executes the line of code after the colon, whether it is on the same line or not |
|    | if argument : { } | Executes the line if the argument is true. Executes only the code in the brackets |
| Input | input console |  Waits for the user to input into the console and gives that as output through ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input key | Outputs the keys pressed as (Key, Key, Key (EX: A, Up)) using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input key keyName | Checks if the specified key is beng pressed and outputs *1* as yes and *0* as no using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse wheel | Outputs the state of the mouse wheel as (-1, 0, or 1) using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse wheel raw | Outputs the state of the mouse wheel as the raw value the computer is getting through ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse button | Outputs the state of the mouse button as (Button, Button (EX: Left, Middle)) using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse button buttonName | Checks if the specified button is beng pressed and outputs *1* as yes and *0* as no using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse position | Outputs the position of the mouse as (X:pos, Y:pos (EX: X:840, Y:92)) using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
|       | input mouse position X_or_Y | Outputs the position of the mouse axis using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
| Intersects | intersects conntol_1 control_2 | Checks if the specified controls are overlapping and outputs *1* as yes and *0* as no using ':' or '=>' [see here](#modifier-syntax "Last two rows of the table") |
| Label | label name | Creates a label control |
|       | label name properties | Creates a label control and sets its properties |
| list | list name new : value, value  | Creates a list that can hold different values |
|      | list name add value | Adds a value to the list |
|      | list name equals index value | Finds a value by its index and sets it to the inputted value |
|      | list name remove index_or_name | Removes a value from the list by its index or name |
|      | list name clear | Clears all values from the list |
|      | list name destroy | Destroys the list completely |
| Loop | loop loopTimes { } | Loops through the code in the brackets the inputted amount of times |
|      | loop argument { } | Loops through the code in the brackets the while the argument is true |
| MessageBox | messagebox title text | Pops up a Windows messagebox with the inputted title and text |
| Print | print text... | prints all of the text after the keyword |
| Shape | shape name | Creates a shape control |
|       | shape name properties | Creates a shape control and sets its properties |
| Stop | stop all | Quits the entire program |
|      | stop file | Stops the current file running |
|      | stop method | Stops the current method running |
| Sound | sound stopall | Stops all sounds playing |
|       | sound volume value | Sets the volume of all sounds (0 - 1) |
|       | sound name new filepath | Creates a new sound instance from the file |
|       | sound name play | Plays the sound |
|       | sound name playloop | Plays the sound in a loop |
|       | sound name stop | Stops the sound |
|       | sound name destroy | Destroys the sound completely |
| Textbox | textbox name | Creates a textbox control |
|         | textbox name properties | Creates a textbox control and sets its properties |
| Variable | var name | Creates a variable |
|          | var name value | Creates a variable and gives it the specified value |
|          | var name : code_output | Creates a variable and sets its output to the output the inpputted code (EZ: var x : input mouse position x)
| Window | window name new | Creates a window |
|        | window name new : properties, ...  | Creates a window and sets its properties |
|        | window name change : properties, ... | Changes the window's properties |
|        | window name clear | Clears all controls from the window |
|        | window name open | Opens the window |
|        | window name close | Closes the window |
|        | window name display control | Adds a control to the window |
|        | window name display group | Adds all of the controls in the group to the window |
|        | window name destroy | Destroys the window completely |

</details>

<details open>
<summary><h3>Modifier</h3></summary>

 Modifier | Use | Example 
----------|-----|---------
// | Does not execute any of the code after the double slash | `var result (x * 5) // The result is the 'X' variable multiplied by 5!`
\| | Starts the next line of code | `var x 10 \| var y 5`
\\! | Value is nothing ('') | `var empty \!`
\\n | Value of newline | `print Hello\nWorld!`
\\_ | Value of space (' ') | `file write Hello\_Y'all C:/path.txt`
~\\ or ~/ | Uses local path | `file play ~\file.ezcode`
\\; | Value of colon (':') | `if \; = whatever : `
\\q | Value of equal sign ('=') | `if \q = whatever : `
\\c | Value of comma (',') | `label name text:Hi\c everybody!`
\\e | Value of exclamation mark ('!') | `if \e ! whatever : `
\\$ | Value of pipe ('\|') | `print Agency \& Thing`
\\& | Value of semicolon (';') | `shape name bg:[name\&red;50;50]`
\\( and )\\ | Solves equation inside of the backslashed parenthesis | `print Value = \(10 + 10)\ // outputs 'Value = 20'`
\\- | Returns a value with the '\\-' taking out the character before it | `var val h3 \| val = 'val'\-`
\\" | Value of single quote (''') | `var val hello \| print \"'hello'\"`
'variable' | writes out the value of the variable | `var val Hello \| print 'val' World!`
(equation) | If this is in a place where a number needs to be, it will solve the equation and set the value to it | `shape name x:(10 + 5)`
?(argument)? | If this is in a place where a boolean needs to be, it will solve the equation and set the value to it | `loop ?(0 > 5)? : { }`
=> | Works only after some keywords, but creates a variable from the output of the line | `input console => consoleOutput `
: | Similar to '=>', but instead of creating a variable, it sets a variable to the output of the line | `var consoleOutput \| input console : consoleOutput`

</details>

<details open>
<summary><h3>System and References</h3></summary>

Reference | Use | Example
:-|:-|:-
system:time | gets the current time. Has many formats so see the [Wiki](https://github.com/JBrosDevelopment/EZCode/wiki "Wiki contains the official docs") for that. | `print Time: system:time`
system:random | gets a random number from the inputted minumin and maximum | `print Random Number between 1 and 10: system:random:1:10`
system:isnumber | returns _0_ or _1_ depending on if the value is a number or not | `input console => number \| var number? system:isnumber:number`
system:machine | gets infornation about the machine. Has many subreferences so see the [Wiki](https://github.com/JBrosDevelopment/EZCode/wiki "Wiki contains the official docs") for that. | `print User Name: system:machine:username`
system:currentfile | returns the current file being played... if known. | `print Current file: system:currentfile // only really for debugging`
system:currentplaydirectory | gets where the executable application that is running the EZCode file is. | `print Executable Directory: system:currentplaydirectory`
system:space | returns a space. use \\_ instead.
system:newline | returns a newline. use \\n instead.
system:nothing | returns a space. use \\! instead.
system:pipe | returns a space. use \\$ instead.
controlName:propertyName | returns the property value of the control | `shape player \| print X = player:x`
windowName:propertyName | returns the property value of the window | `window main new \| print Title = main:text`
var_or_list_or_group:length | if var, returns the length of the value (ie. 'hello' has length of 5). If list or group, returns how many values are in the list or group. | `var name John \| print Number of Name's letters: name:length \| // or with list \| list names new : John, Sally \| print Number of names in list: names:length \|`
var_or_list_or_group:index | If var, returns the letter that the index corrosponds to (remember index starts from 0). If list, returns the value of the list that corrosponds with the index. If group, it will return the control's name. | `var a abc \| print a:1 // displays 1 \| list b new : ab, cd \| print b:0 // prints ab \| group c new : label1, shape2, shape3 \| print c:2 // returns shape3`
var_or_list:contains | returns _0_ or _1_ if the var's value contains the the inputted value or if a list's value is equal to the inputted value | `var v abcdef \| var contains? v:contains:g // returns 0 \| //Or for list \| list v new : abc, def, ghi \| var contains? v:contains:ghi // returns 1`
value:index_or_length | If there is a value seperated by commas (a,b,c,d), a colon can be put after that and either an index (number), or 'length' and it will return an output | `print 10,20,30:length // outputs 3 \| print 10,20,30:2 // outputs 30 (0 would be 10, 1 would be 20 in this example)`

</details>
</details>
