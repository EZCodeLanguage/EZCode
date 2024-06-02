# EZCode

<details open>
<summary><strong>Directory</strong></summary>

- [Keywords](#keywords)
- [Special Keywords](#special-keywords)
- [Properties](#properties)
- [Arguments](#arguments)
- [Output Characters](#output-characters)
- [Comments](#comments)
- [Code Seperation](#code-seperation)
- [Special Characters](#special-characters)
- [System and References](#system-and-references)
- [Segments](#segments)
- [Errors](#errors)

</details>

## Keywords

<details open>
<summary><strong>Directory</strong></summary>

- [Await](#await)
- [BringTo](#bringto)
- [Button](#button)
- [Clear](#clear)
- [Destroy](#destroy)
- [Else](#else)
- [EndMethod](#endmethod)
- [Event](#event)
- [File](#file)
- [Global](#global)
- [Group](#group)
- [If](#if)
- [Input](#input)
- [Instance](#instance)
- [Intersects](#intersects)
- [label](#label)
- [list](#list)
- [Loop](#loop)
- [Math](#math)
- [MessageBox](#messagebox)
- [Method](#method)
- [Print](#print)
- [Shape](#shape)
- [Sound](#sound)
- [Stop](#stop)
- [Textbox](#textbox)
- [Variable](#variable)
- [Window](#window)

</details>

The complete EZCode Syntax. Ordered by alphabetical order. For quick Syntax, also see [README.md](https://github.com/JBrosDevelopment/EZCode/blob/master/README.md).

### Await

The `await` keyword is used to delay the program. It can be used in two ways. One, `await milliseconds` where `milliseconds is a number, will delay the program by the inputted time.
```
var tick 100
loop true {
    await tick
}
```
This is a common use of the await keyword. The next way the await keyword can be used is with an argument `await argument` where argument is a boolean value. See [arguments](#arguments) for details.

### BringTo

The `bringto` keyword brings a control to the front or back of the display. There is `bringto front controlName` and `bringto back controlName`. 
```
shape player
bringto front player
```
A common use of the `bringto` keyword is to make sure the control added is sent to the front. Or if there is a background that is declared after another control, to send the background to the back.

### Button

The `button` keyword is used to create a button. It will create it to the [visual output](Programs#visual-output) or create an instance that can be displayed to a window. The syntax is relatively simple, `button name` or `button name properties`. The default values for the properties are as follows. `backcolor:[255;255;255]`, `x:0`, `y:0`, `width:75`, and `height:25`.
```
button btn1                                 // create a button with default values
button btn2 x:50, y:50, text:Press Me!      // create a button with custom values
event btn2 click On_Click_Method            // set the trigger for when the button gets clicked
btn1 width:150                              // change the properties of a button
```
See [Properties](#properties) for more details on property values. 

### Clear

The `clear` keyword is used to clear the [console](Programs#console). It can be used by itself, or have an [argument](#arguments) after it `clear argument`.
```
print Input Your Name
var _input : input [console](Programs#console)
clear
print Welcome, `_input`
```
This code will ask for the name of the user, clears the [console](Programs#console), and then welcomes the user. This is an ordinary use of the keyword `clear`.

### Destroy

The keyword `destroy` is used to destroy a control. When an object is destroyed, it can never be accessed again. An error will occur if it is trying to be accessed. `destroy controlName` is the correct syntax, but there is also `destroy controlName argument`. This will only destroy the control if the [argument](#arguments) is `true`.

### Else

The `else` statement is only useful after an [`if`](#if) statement. If the `if` statement's argument is `false`, then the code inside the `else` statement will run. The syntax for this is very simple, `else : code...` or `else : { code_inside_brackets }`. If the `else` statement does not use brackets, then it will execute the next line of code, whether that be right after the colon, or on the next line. 
```
if false : // Nothing
else : {
    // Code
}
```
This code will skip over the `if` and go straight to the `else` because the if statement's argument is false.


### EndMethod

`endmethod` is what ends a [method](#method). If there is no `endmethod` to a `method`, then the program will not run and will give an error. 

### Event

The `event` keyword is for controls or windows. This acts as a trigger for the inputted action. Whenever the control or window is triggered, it can play a file, method, or single segment of code. The correct syntax is `event control_or_window eventType filePath_or_method_or_code`. 
```
shape player
event player click ~/player_clicked.ezcode`
```
In this example, a control named 'player' is created. It is then given the event `click` set to a file. If this control is clicked, then the file will be played. 

The list of events for controls: 
- `click` when the control is clicked by the mouse
- `hover` when the mouse hovers over the control
- `move` when the control's position changes
- `scale` when the control's width or height changes
- `backcolor` when the control's backcolor changes
- `forecolor` when the control's forecolor (text color) changes
- `image` when the control's image is changed
- `imagelayout` when the control's imagelayout (ex. tile, zoom, etc)
- `font` when the control's font is changed 
- `text` when the control's text is changed

The list of events for windows:

- `click` when the window is clicked by the mouse
- `hover` when the mouse hovers over the window 
- `move` when the window's position changes
- `scale` when the window's width or height changes
- `backcolor` when the window's backcolor changes
- `forecolor` when the window's forecolor (text color) changes
- `image` when the window's image is changed
- `imagelayout` when the window's imagelayout (ex. tile, zoom, etc)
- `font` when the window's font is changed
- `text` when the window's text is changed
- `focused` when the window is the active window
- `controladded` when a window is displayed to the window
- `controlremoved` when a window is removed from the window
- `defocused` when the window stops being the active window
- `close` when the window is closed
- `open` when the window is open
- `enabledchanged` when the window becomes enabled or uneneabled
- `keydown` when a key is pressed down and the window is focused
- `keyup` when a key lifted and the window is focused
- `keypress` when a key is pressed while window is focused
- `resize` when the window's width or height changes
- `resizestart` when the window starts changing size
- `resizeend` when the window finishes changing size

### File

The `file` keyword is used to manipulate files. There is `read`, `write`, `validpath`, `play`, `playproj`, `create`, `exists`, and `delete`. The following syntax is:
- `file read filepath`: This reads a file and outputs the contents.
  - `file read C:/file.txt => file_contents`
- `file write text_or_var filepath`: This writes contents to a file. 
  - `file write This\_is\_a\_test\_file C:/file.txt`: writes to a file
  - `file write This\_is\_a\_test\_file C:/file.txt => result`: Outputs a boolean if writing was successful
- `file validpath text_or_var`: This returns a boolean if the inputted text could be a valid file.
  - ```
    var inputted_file : input [console](Programs#console)
    file validpath inputted_file => valid
    if valid : print Valid Path!
    else : print Invalid Path ):
    ```
- `file play filepath`: This plays a file using EZCode.
  - `file play ~/code.ezcode`: Plays the file in the program
  - `file play ~/code.ezcde => results`: This outputs the [console](Programs#console) of the file into a variable`
- `file playproj filepath`: This plays a EZProj file
  - `file playproj ~/Project.ezproj`: Plays the project in the program
  - `file playproj ~/Project.ezproj => result` This outputs the [console](Programs#console) of the file into a variable
- `file create filepath`: This creates a file and outputs a boolean if successful
  - `file create file.txt`: Creates the file
  - `file create file.txt => result` Creates the file and outputs boolean if successful
- `file exists filepath`: This outputs a boolean if file exists
  - `file exists C:/Project/file.txt => result`
- `file delete filepath`
  - `file delete ~/file.txt`: Deletes the file
  - `file delete ~/file.txt => result` Delets the file and outputs a boolean if successful 

The `file` keyword uses output characters to output the results. See [Output Characters](#output-characters).

### Global

The `global` keyword is used for declaring a variable, list, group, or control. It's placed before the keyword to make it global. When a method runs, all variables, groups, lists, or controls from the method that ran it are inaccessible unless they are declared global.
```
method start
global var text \!
txtMaker : Hello World
endmethod

method txtMaker : something:\!
text = 'something'
endmethod
```
This is the main way of using the `global` keyword. The reason that `global` is necessary is so that variables, groups, lists, and controls can be used across different methods.

### Group

The `group` keyword is used to declare a group. A group is an array of controls and is used to change the properties of all of the controls in the group. There is a way to create a new group `new`, add a control `add`, replace `equals`, remove a control `remove`, clear all of the controls `clear`, destroy the group `destroy`, destroy the group and destroy all of the controls in it `destroyall`, and change the properties of all of the controls `change`.
- `group name new`: Creates a new group 
- `group name new : controls...`: Creates a new group and adds controls to it
- `group name add controlName`: Adds a control to the group
- `group name equals index controlName`: Replaces the controls with the corresponding index with the control inputted
- `group name remove index_or_name`: Removes a control from the group by name or index
- `group name clear`: Clears all of the controls from the group (does not affect the controls)
- `group name destroy`: Destroys the group making it inaccessible (does not affect the controls)
- `group name destroyall`: Destroys all of the controls in the group as well as the group
- `group name change : properties...`: Changes all controls in the group relative to the inputted properties (Ex: controls's 'x' value is '5.' the inputted properties are '10' so the new control's properties are '15')
- `group name argument change : properties...`: If the agrument is true, this changes all controls in the group to the properties, else it changes them relative to the controls properties

```
shape s x:0
label l x:5
button b x:10
textbox t x:15

group g new : s, l, b, t
group g change : x:50 // offsets all control's x positions by 50
```

### If

The `if` statement is used for executing code only if the argument given is true. An `argument` is a boolean value. The syntax for it is simple `if argument : `. Here is a common use of the `if` statement

```
var number 0
loop 100 {
    if number > 50 : // execute code
    number + 1
}
```
There can only be one `:` for the line containing the if statement. An error will occur because it won't find where the argument ends and the code begins. 

To extend the argument to have more than one condition is can be done with the `and` (also `&`) and `or` keywords. An example `if a = 1 & b = 2 : ` This statement will be true only if `a` is equal to one and `b` is equal to two. See [Conditional Syntax](#condition-syntax). If an if statement is true, then it will run the code after the `:`, whether it be right after it, the next line, in a couple lines, or if the code is in brackets (`{`, `}`). if wanted, the opening bracket does not need to be right after the colon, but can be on the proceeding lines.

### Input

The `input` keyword is used for getting inputs from the computer. This keyword outputs its value using output characters ( See [Output Characters](#output-characters)). The syntax is as follows.
- `input console`: waits for the user to input into the [console](Programs#console).
    - `input console => var_input`: `var_input` is equal to what the user inputted into the [console](Programs#console).
- `input key`: returns all of the Keys being pressed as text.
    - `input key => keys`: `keys` is equal to all of the keys being pressed separated by `, `. As an example, if 'A' and 'B' were being pressed at the same time, then the output would be `A, B`. 
- `input key keyName`: returns a boolean if the key is being pressed.
    -`input key Space => space`: `space` is equal to `1` or `0` depending on the space bar being pressed.
- `input mouse wheel`: returns `1`, `0`, or `-1` depending on the state of the mouse wheel.
- `input mouse wheel raw`: returns the raw output of the mouse wheel. Effected by speed of mouse wheel. See [Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.mouseeventargs.delta?view=windowsdesktop-8.0#:~:text=Int32-,A%20signed%20count%20of%20the%20number%20of%20detents%20the%20mouse%20wheel%20has%20rotated%2C%20multiplied%20by%20the%20WHEEL_DELTA%20constant.,-Examples).
- `input mouse button`: returns all of the mouse buttons being pressed as text.
    - `input mouse button => btns`: `btns` is equal to all of the mouse buttons being pressed separated by `, `. As an example, if the 'Left' and 'Right' mouse buttons were being pressed at the same time, then the output would be `Left, Right`.
- `input mouse button buttonName`: returns a boolean if the mouse button is being pressed.
    -`input mouse buttob Left => click`: `click` is equal to `1` or `0` depending on the left button being pressed.
- `input mouse position`: returns a value of the `x` and `y` positions of the mouse formatted as `x, y`.
- `input mouse position X_or_Y`: returns the value of the position of the mouse axis.
    - `input mouse position x => mouse_x`: `mouse_x` is equal to the x position of the mouse


### Instance

The `insetance` keyword is used for declaring a control. It's placed before the keyword to edit the syntax of the control declaration. The syntax, `instance control properties` removes the name declaration, but requires it as one of the [properties](#properties).
```
var i 0
loop 10 {
    instance shape name:box'i', bg:[255;0;0]
    i + 1
}
```
This is the most common use of `instance`. It allows 10 different shapes to be made each with uinque names. The names of the shapes shown above would be, `box0`, `box1`, etc. The shape can be accessed with a variable in the name as follows, (#properties).
```
group boxes new
var i 0
loop 10 {
    instance shape name:box'i', bg:[255;0;0]
    boxes add box'i'
    i + 1
}
```
This will add all of the shapes to a group. This can only be done with the `instance` keyword because the normal way of decalring a control will check if a control already exists by that name and set it to it if it does.

### Intersects

The `intersects` keyword was made for checking if two controls are intersecting each other. It uses [Output Characters](#output-characters) to return values. The syntax is simple: `intersects conntol_1 control_2`. An example of this being used is as follows.
```
shape obj1
textbox obj2
intersects obj1 obj2 => touching
```

### Label

The `label` keyword is used to create a label. It will create it to the [visual output](Programs#visual-output) or create an instance that can be displayed to a window. The syntax is relatively simple, `label name` or `label name properties`. The default values for the properties are as follows. `x:0`, `y:0`, `backcolor:[255;255;255]`, and `autosize:true`.
```
label lb1                                 // create a label with default values
label lb2 x:50, y:50, text:Hello World    // create a label with custom values
lb1 text:Hurray                           // change the properties of a label
```
See [Properties](#properties) for more details on property values. 

### List

The `list` keyword is used to declare a list. A list is an array of values and is used to store and use values in the list. There is a way to create a new list `new`, add a value `add`, replace `equals`, remove a value `remove`, clear all of the values `clear`, and destroy the list `destroy`.
- `list name new`: Creates a new list 
- `list name new : values...`: Creates a new list and adds values to it
- `list name add valueName`: Adds a value to the list
- `list name equals index valueName`: Replaces the values with the corresponding index with the value inputted
- `list name remove index`: Removes a value from the list by the index
- `list name clear`: Clears all of the values from the list
- `list name destroy`: Destroys the list making it inaccessible
An example of `list` is as follows.
- `list name split value splitter`: This will set the list to text split by the splitter value. An example, `list name split Yes;No;Maybe ;`. This will set the list to `Yes;No;Maybe` split by the semicolon `;`.
```
var name John Doe
list main new : Hello, 'name'
```
To access a value from a list, use `listName:index`. An example of this being used is in the [Loop Statement](#loop).

### Loop

The `loop` statement is used to loop through code. Its syntax is simple, `loop loopTimes { }` or `loop argument { }`. The statement will loop through all of the code within its brackets. It can loop by an [argument](#arguments) or by a number. Here are some common examples of it being used.
```
var notQuit true

loop notQuit {
    // code...
}
```
The code above creates a program loop which will loop until the variable `notQuit` becomes false.
```
list values new : a, b, c, d, e, f
var i 0
loop values:length {
    print values:i
    i + 1
}
```
The code above loops through a list and prints the list's values.

### Math

The `math` keyword is used to complete mathematic equations. It uses [Output Characters](#output-characters) to return the value. Here is an example,
```
var X 100
math X / 50 + 10 => Output
```
In this example, a variable named `X` is created and assigned the value of `100`. Then the variable `Output` is assigned to `X / 50 + 10` which will return `12`. 

There is Mathematical Functions that can be used inside of the segment. Here is a list of all of them,
- `abs(Value)`: This will return the absolute value of the value inputted
- `neg(Value)`: This will return the negative of the value inputted
- `sq(Value)`: This will return the inputted value squared
- `sqr(Value)`: This will return the square root of the inputted value
- `round(Value)`: This will return the value inputted to the nearest whole number
- `pow(Value,Exponent)`: This will retutn the value to the poswer of the Exponent inputted
- `clamp(Value,Min,Max)`: This will return the Value withen the minumum and maximum inputted
- `sum(Value1,Value2,etc)`: This will return the sum of all of the values inputted
- `avg(Value1,Value2,etc)`: This will return the average of all of the values inputted
- `min(Value1,Value2,etc)`: This will return the lowest number inputted
- `max(Value1,Value2,etc)`: This will return the largest number inputted
- `pi()`: This will return pi (π `3.1415927`)

For the Math Functions, there can not be any spaces inbetween the inputs. For example, the average function, `avg(100,30,80,90,20)`. In this, there can be no spaces in between the commas. Here is an example of the funtions being used,
```
var X 0
var BoundsEnd 100
loop isRunning {
    await 100
    // Movement
    var Left : input key Left
    var Right : input key Right
    if Left : X - 10 | if Right : X + 10
    // Bounds
    math clamp(X,0,BoundsEnd) : X
}
```
In this example, A simple movement system is created with `Left` and `Right` movement. The code will then clamp `X` in between `0` and `BoundsEnd` which equals `100`.

### MessageBox

The `messagebox` keyword is used to display a message to the user through a Window's popup message. The syntax is simple, `messagebox` title text`.

### Method

A method is code that can be called from anywhere in the program. Its syntax is as follows, `method name` or `method name : values`. It needs [`endmethod`](#endmethod) to end the method. If there is no [endmethod](#endmethod), then the program will give an error and will not run. Here is an example,
```
method Start

    //code...

endmethod
```
If there is one method in the program, then all code needs to be in methods. The startup method is the first method in the startup file. It is usually named `start`, but it could be anything. 

The way methods with values work is it takes values and there default value, `method name : val1:defaultVal, val2:defaultVal`. A method will also return the variables given into the method. Here is an example of a method with values,
```
method Start

    var Value 10
    
    Clamp : Value, 0, 5
    
    // value is now equal to 5. It got clamped between 0 and 5.
    
endmethod

method Clamp : value:0, min:0:, max:0

    if value < min : value = min
    if value > max : value = max

endmethod
```
In this example, the `Clamp` method takes 3 values, `value`, `min`, and `max`. When it is called, the variable `Value` is put into the method. The method can change any variable put into it. So `Value` will then be set to `5` becaue it was `10` and was clamped between `0` and `5`.

A method without values can be called like this,
```
method Start
    ExampleMethod
endmethod

method ExampleMethod
    // code...
endmethod
```

here are the various ways to call a method:
- `methodName` This will call a method that has not input values.
- `methodName : val_1, val_2` This will input the values into a method. The inputted value can be a variable or any value. If it is a variable, the variable can be manipulated by the method like in the `Clamp` example above.
- `methodName => var_1, var_2` This will create variables from the method. This is why the method has default values.

---

Here is a great example of methods being used:
```
method Start
    global var running true
    loop running {
        await 10
        INPUTS => left, right, up, down, space
        if space : Jump
    }
endmethod

method INPUTS : l, r, u, d, space
    l : input key Left
    r : input key right
    u : input key up
    d : input key down
    space : input key space
    var quit : input key escape
    if quit : running = false
endmethod

method Jump
    // Handle Jumping
endmethod
```

### Print

The `print` keyword has the simple function of printing values to the [console](Programs#console). The syntax is simple, `print text...`. Here is a simple example.
```
var name John Doe
print Hello, `name`
```

### Shape

The `shape` keyword is used to create a shape. It will create it to the [visual output](Programs#visual-output) or create an instance that can be displayed to a window. The syntax is relatively simple, `shape name` or `shape name properties`. The default values for the properties are as follows. `backcolor:[0;0;0]`, `x:0`, `y:0`, `width:50`, and `height:50`.
```
shape shp1                                 // create a shape with default values
shape shp2 x:50, y:50                      // create a shape with custom values
shp1 width:150                             // change the properties of a shape
```
See [Properties](#properties) for more details on property values. 

### Sound

The `sound` keyword is used to create an instance of a sound player. Here is the syntax.
- `sound stopall`: Stops all sounds playing
- `sound volume value`: Sets the volume of all sounds (0 - 1)
- `sound name new filepath`: Creates a new sound instance from the file
- `sound name play`: Plays the sound
- `sound name playloop`: Plays the sound in a loop
- `sound name stop`: Stops the sound
- `sound name destroy`: Destroys the sound completely

### Stop

The `stop` keyword is used to stop a file, method, or entire EZCode program. 

- `stop all`: This stops the entire EZCode program.
- `stop return`: This will stop the current method or file that was being played from another file or method.

Here is an example of `stop return`:
```
method Start
    Next
    print end
endmethod

method Next
    stop return
    print next
endmethod
```
This will stop the method before it prints `next` while still printing `end` in the first method.

### Textbox

The `textbox` keyword is used to create a textbox. It will create it to the [visual output](Programs#visual-output) or create an instance that can be displayed to a window. The syntax is relatively simple, `textbox name` or `textbox name properties`. The default values for the properties are as follows. `x:0`, `y:0`, `width:75`, `height:50`, and `backcolor:[255;255;255]`.
```
textbox tbox1                                 // create a textbox with default values
textbox tbox2 x:50, y:50, text:Hello World    // create a textbox with custom values
tbox1 text:John Doe                           // change the properties of a textbox
```
See [Properties](#properties) for more details on property values. 

### Variable

The `var` keyword is used to create a variable. A variable is an instance that holds a value. It has very simple syntax, `var name value` or `var name : code_output`. Here are some examples,
- `var name John Doe`: This creates a variable named `name` and sets its value to `John Doe`
- `var readFile : file read ~/text.txt`: This creates a variable named `readFile` and sets its value to whatever `text.txt` contains.
```
var pos_x 50
var pos_y 25
shape Player x:pos_x, y:pos_y
```
```
var KeyInput : input key
```
Var's value can be changed with the `=`, `+`, `-`, `/`, or `:`. Here are some examples.
```
var x 0
loop 50 {
   x + 1
}
```
```
var text
text = Hello World
```
```
var Up 0
Up : input key up
```
To be specific, `+` (adds to end of text), `-` (subtracts characters from end of text. For example: `txt - 5` this removes the last 5 characters from `txt`), and `=` (sets text) can be used for text and `-`, `+`, `=`, `*, and `/` can be used for numbers.

### Window

The `window` keyword is used to create a window. A window is a WinForms window. The syntax is as follows. Create a new window `new`, change the properties `change`, clear controls in window `clear`, open the window `open`, close the window `close`, display a control on the window `display`, destroy the window `destroy`.
- `window name new`: Creates a new window 
- `window name new : properties...`: Creates a new window and sets it's properties
- `window name change : properties...`: Changes the window's properties
- `window name clear`: Clears all of the controls from the window
- `window name open`: Opens the window
- `window name close`: Close the window
- `window name display controlName`: Display a control to the window
- `window name destroy`: Destroys the window making it inaccessible (does not affect the controls)
See [Properties](#properties) for more information on properties
Windows are accessible from any method. There is no `global window`.

## Special Keywords

Special Keywords modify the code. The are started with `#`. The `#` can be connected or seperated by a space to the keywords (`#suppress error` or `# suprress error`). Here is the list of the Spcial Keywords. 
- [Suppress Error](#suppress-error)
- [Create Error](#create-error)
- [Current File](#current-file)
- [Project Properties](#project-properties)
- [EZText Start and End](#eztext-start-and-end)

### Suppress Error

This is used to suppress an error from showing in the console. It is used at the end of the line of code that might create an error. It's syntax is simple, `# suppress error`. Here is an example.
```
var Number : input console
var NewNumber (Number * 2) # suppress error
```
This example gets input from the console. It then tries to multiply it by 2. If an error occurs, like if `Number` is a text value, it will not create an error in the console.

### Create Error

This is used to create an error to the console. It can be used in its own line, or after a line of code. Here is the syntax, `# create error` or `# create error Error Text`. If no error text is supplied, it will output, `An error occured in Segment <Code_Line>`. If Error text is supplied, it will output the text exactly.

### Current File

This will set the current file that is being played. It is really only used in the background by the [Player](programs#ezplayer) to tell the code what file is currently being played. The syntax is simple, `# current file URL_PATH`.

### Project Properties

This is used to set the project properties without using an EZProj file. This is mainly used by the [Player](programs#ezplayer) to set the Project properties from the EZProj file. The syntax is, `# project properties : properties...` The properties are syntaxed like control's properties with, `propertyName:Value`. Here is an example, 
```
# project properties : name:Project Properties Test, isviusal:true
```
All the properties are the [EZProj Properties](EZProject-Docs).

### EZText Start and End

This is used to start and end [EZText](eztext-docs). Here is an example,
```
print This is EZCode

# eztext start
Write This is EZText to the console.
# eztext end

print This is EZCode
```
If there is no `# eztext end` then it will just assume the rest of the document is EZText.

## Properties

Properties are the different values a control or window has. Properties are separated by commas. The value is set with colon. `shape name x:50, y:50`. 

### Control's Properties

- `id`: This is used for having another way of identifying a control. This can be used to check what control it is, or to call the control using the id.
  - ```
    shape name id:shape1
    shape1 x:50
    ```
- `focus`: This is used to set the control as the focus control. Takes [argument](#arguments).
- `font`: This is used to change the default font of the window. Syntax is as so: `font[Font_Name;Size;Style]`. The `Font_Name` is the name of the font (example: `Arial`. Uses fonts allowed by Microsoft). `Size` is the size of the font (example: `12`). Can not be negative size. `Style` is like `bold`, `regular`, `underline`, `italic`, or `strikeout`.
- `readonly`: This is used only for [textboxes](#textbox). It will make the user unable to change the text of the textbox. Takes [argument](#arguments).
- `enable`: This will enable or unenable the control. Takes [argument](#arguments).
- `font`: This is used to change the font of the control. Syntax is as so: `font[Font_Name;Size;Style]`. The `Font_Name` is the name of the font (example: `Arial`. Uses fonts allowed by Microsoft). `Size` is the size of the font (example: `12`). Can not be negative size. `Style` is like `bold`, `regular`, `underline`, or `strikeout`.
  - `label name text:Hello World, font:[Consolas;15;Bold]`
- `point` or `points`: This is only used for [shapes](#shape). It is for setting custom points of the shape. The syntax is as follows: `points:[(0*0);(50*0);(50*50)]`. The whole thing is inside `[ ]`. Each point is separated by `;`. Each position is separated by `*`. The example shown creates a right triangle with the points (0, 0), (50, 0), and (50, 50).
- `autosize` or `auto`: This is used with the text property. It sets the control's size to match the size of the text. Takes [argument](#arguments).
- `multiline` or `multi`: This is only used for [textboxes](#textbox). Sets if the textbox is allowed to be multi lined. Takes [argument](#arguments).
- `wordwrap` or `wrap`: This is only used for [textboxes](#textbox). Sets if the text allows scroll or wraps around to the next side. Needs `multiline` to be true. Takes [argument](#arguments).
- `verticalscrollbar` or `vertical`: This is only used for [textboxes](#textbox). Sets if the vertical scrollbar should show on the textbox. Needs `multiline` and `wordwrap` to be true. Takes [argument](#arguments).
- `horizantalscrollbar` or `horizontal`: This is only used for [textboxes](#textbox). Sets if the horizontal scrollbar should show on the textbox. Needs `multiline` and `wordwrap` to be true. Takes [argument](#arguments).
- `text` or `t`: The text the control shows. Only applies to [button](#button), [textbox](#textbox), or [label](#label). Takes any text.
- `x`: The x position of the control. Takes a number.
- `y`: The Y position of the control Takes a number.
- `height` or `h`: The height of the control. Takes a number.
- `width` or `w`:  The width of the control. Takes a number.
- `backcolor`, `bc`, or `bg`: This controls the backcolor of the control. It's syntax is as follows: `bg:[R;G;B]`. The rgb values need to be 0 - 255.
- `forecolor`, `fc`, or `fg`: This controls the forecolor of the control. It's syntax is as follows: `fg:[R;G;B]`. The rgb values need to be 0 - 255.
- `poly` or `p`: This is the polygon count for [shapes](#shape). Lowest is 3 (being a triangle) and there is no max. Takes a number.
- `image`: This is the background image of the control. It takes in a file URL. If the url is not a local path, then replace the `C:\` with `C\;/` and all of the other backslashes `\` to forward slashes `/`.
- `imagelayout`: This is the image layout of the control. It takes one of the following options, `tile`, `center`, `zoom`, `none`, or `stretch`.
- `align`: This will align the text only in [labels](#label) and [buttons](#button). It takes one of the following, `topleft`, `topcenter`, `topright`, `middleleft`, `middlecenter`, `middleright`, `bottomleft`, `bottomcenter`, or `bottomright` to align the text.
- `name`: The name of the control. Takes any text.
- `visible`: If the control is visible.
- `anchor`: Anchors the control to the [Window](#window). If the window resizes, then the control will resize with it. It Accepts `none`, `left`, `right`, `top`, and/or `down`. It always needs one vertical and one horizantal. If none are given, it will revert to `left` or `top`. 

### Window's Properties

- `focus`: This is used to set the window as the focus window. Takes [argument](#arguments).
- `enable`: This will enable or unenable the window. Takes [argument](#arguments).
- `font`: This is used to change the default font of the window. Syntax is as so: `font[Font_Name;Size;Style]`. The `Font_Name` is the name of the font (example: `Arial`. Uses fonts allowed by Microsoft). `Size` is the size of the font (example: `12`). Can not be negative size. `Style` is like `bold`, `regular`, `underline`, `italic`, or `strikeout`.
- `autosize` or `auto`: This sets the window's size to match the controls within it. Takes [argument](#arguments).
- `text` or `t`: The text the window shows. Takes any text.
- `x`: The x position of the window. Start position needs to be set to manual (`startposition:manual`). Takes a number.
- `y`: The Y position of the window. Start position needs to be set to manual (`startposition:manual`). Takes a number.
- `minheight`: The minumum height of the window. Takes a number.
- `minwidth`:  The minumum width of the window. Takes a number.
- `maxheight`: The maximum height of the window. Takes a number.
- `maxwidth`:  The maximum width of the window. Takes a number.
- `height` or `h`: The height of the window. Takes a number.
- `width` or `w`:  The width of the window. Takes a number.
- `backcolor`, `bc`, or `bg`: This controls the backcolor of the window. It's syntax is as follows: `bg:[R;G;B]`. The rgb values need to be 0 - 255.
- `forecolor`, `fc`, or `fg`: This controls the forecolor of the window. It's syntax is as follows: `fg:[R;G;B]`. The rgb values need to be 0 - 255.
- `image`: This is the background image of the window. It takes in a file URL. If the URL is not a local path, then replace the `C:\` with `C\;/` and all of the other backslashes `\` to forward slashes `/`.
- `imagelayout`: This is the image layout of the window. It takes one of the following options, `tile`, `center`, `zoom`, `none`, or `stretch`.
- `opacity`: This sets the opacity of the window and all of the controls in it.
- `minimizebox`: This sets if the window shows the minimize box as an option for the window. Takes [argument](#arguments).
- `maximizebox`: This sets if the window shows the maximize box as an option for the window. Takes [argument](#arguments).
- `showicon`: This sets if the window should show the icon at the top left corner of the window. Takes [argument](#arguments).
- `showintaskbar`: This sets if the window should show the icon in the taskbar. Takes [argument](#arguments).
- `icon`: This is the icon of the window. It takes in a file URL. If the URL is not a local path, then replace the `C:\` with `C\;/` and all of the other backslashes `\` to forward slashes `/`.
- `state`: This sets the state of the window, `maximized`, `minimized`, or `normal`.
- `type`: This sets the type of window, 
  - `none`: no top bar and can't move or resize
  - `fixed3d`: has a slight 3D look on border. Can't resize
  - `fixeddialog`: makes user unable to click any other window in the program until the window closes. Can't resize
  - `fixedsingle`: can't resize
  - `fixedtoolwindow`: makes window a tool window (no icon and only X button on top right) and can't resize
  - `sizable`: normal window
  - `sizabletoolwindow`: makes window a tool window (no icon and only X button on top right)
- `startposition`: The start position of the window whenever the window opens.
  - `manual`: manually set the position with `x`, and `y`
  - `center`: sets the start position to the center of the screen
  - `default`: sets the start position to the window's default (also default value of window)
  - `defaultbounds`: sets the start position to the window's default bounds position.

## Arguments

Arguments are a boolean value (true or false) that can be obtained from various options. First option is just inputting `true`, `false`, `yes`, `no`, `y`, `n`, `1`, or `0` as the value. These can be obtained from variables, 
```
var running true
if running : // code
```
Another option is using the [Question Mark Syntax](#question-mark-syntax).

If the argument is inside an [if statement](#if), then the argument uses a [conditional statement](#condition-syntax).

### Question Mark Syntax

The Question Mark Syntax uses `?(condition)?` to get an argument. Inside these parenthesis, there can be a [condition statement](#condition-syntax) like, `?(x = 5)?`. In this example, if `x` is equal to `5`, then it will return true, else it will return false.

### Condition Syntax

A condition is when multiple values are compared. The conditional characters can be used `=` (equal), `>` (greater than), `<` (less than), or `!` (not). A combination can be used, `>=` which is greater than or equal to. To add multiple statements together, `and` (also `&`), or `or` can be used. As an example, `if x > 10 or y > 10` will return true if `x` or `y` are greater than 10. Another example, `if x <= 0 and x ! -5` will return true if x is less than or equal to 0 and not -5. `!` can also be used with a boolean, `if ! running` will return true if `running` is false.

## Output Characters

Output characters is a special syntax to output value from a line of code that allows it. [Input](#input), [file](#file), [intersects](#intersects) all give an output. There are 3 ways to get the output.
- `code => var`: `=>` this will goes after the line of code and will create a variable with the output. `file read ~/file.txt => contents`, in this example a file is read and the contents is outputted to new variable named `contents`.
- `code : var`: `:` this will goes after the line of code and will set the corresponding variable to the output. `intersects ob_1 ob_2 : collision`, in this example, the code will check if `ob_1` and `ob_2` intersect. It will output the value to a preexisting variable named `collision`.
- `var : code`: `:` this will goes before the line of code and will set the variable to the output. `var keys : input key`, in this example, the code will get the keyboard input and set the value to the variable `keys`. This can also go to a preexisting variable, `Left : input key Left`.

## Comments

Comments are apart of the code that doesn't run. It is started with `//` and the rest of that line is excluded from bing executed. Here is an example, `print Hello World // This outputs to the [console](Programs#console) 'Hello World' and doesn't output anything after the '//'`.

## Code Seperation

Each line of code is seperated by a newline, or the pipe character `|`. Here is an example, 
```
var text Hello World | print 'txt'
```
Go to [Segments](#segments) to learn about what counts as a segment.

## Special Characters

A special character is a set of characters that output a different character. This can be used in any text input part of code. For example, `var text Hello,\nJohn Doe`. In this example, `\n` outputs as new line. Therefore, `text` outputs as:
```
Hello,
John Doe
```
Here is the list of special characters.
- `@s:`: If at beggining of text, gets litteral text
  - `print @s: No\_Space 'X'`: This will print `No\_Space 'X'`.
- `\!`: Value is nothing ''
  - `btn text:\!`: This sets a control named `btn` text to nothing.
- `\n`: Value of newline
  - `print Hello\nWorld!`: This outputs to the [console](Programs#console):
      - ```
        Hello
        World!
        ```
- `\_`: Value of space ' '
  - `file write Hello\_World C:\path.txt`: This writes `Hello World` to a file.
- `~\` or `~/`: Uses local path
  - `file play ~\file.ezcode`: This is used for local paths for the current file being played.
- `\;`: Value of colon ':'
  - `if \; = whatever : `: The `\;` is changed to a `:` and is allowed by the if statement (only 1 colon for the line of an if statement).
- `\q`: Value of equal sign '='
  - `var txt a\_\q\_b `: The `\q` is changed to `=`. Although `var txt a = b` could also output the same thing.
- `\c`: Value of comma ','
  - `label name text:Hi\c everybody!`: This creates a label and sets its text to `Hi, everybody!` and doesn't mess up the properties by using a comma.
- `\e`: Value of exclamation mark '!'
  - `var name \e`:  The `\e` is changed to `!`. Although `var name !` could also output the same thing.
- `\p`: Value of period '.'. No great use for this except in [EZText](EZtext-Docs)
- `\$`: Value of pipe '|'
  - `print Agency \$ Thing`: This outputs `Agency | Thing` without causing an error of there be a pipe `|` character separating the lines of code.
- `\&`: Value of semicolon ';'
  - `var semi \&`: The `\&` is changed to `;`. Although `var semi ;` could also output the same thing.
- `\-`: Returns a value with the '\-' taking out the character before it
  - `var val h3 | val = 'val'\- `: This will output `h` by subtracting the last character from `h3`.
- `\"`: Value of single quote '''
  - `var val hello | print \"'val'\"`: This will output `'hello'`.
- `\->`: Value of `->`. This is necessary because of [Arrow Continuing Code](#arrow-continuing-code).
  - `print value \-> 10`: This outputs `value -> 10`.

### Other Text Modifiers

- `\(` and `)\`: Solves equation inside of the backslashed parenthesis. This is used for Text values
  - `print Value = 10 + 10 = \10 + 10\ Hurray! // outputs 'Value = 10 + 10 = 20 Hurray!'`: This can be put inside of text to have an equation made withen the text.
- `(` and `)`: Solves equation inside of the parenthesis. This is used for Number values
  - `var X (10 - 5) // X will equal to the equation from the brackets. 
- `'variable'`: writes out the value of the variable
  - `var val Hello | print 'val' World!`: This gets a variable inside of text input.

### Arrow Continuing Code

The `->` modifier extends the line of code to the next line. It is placed at the end of the code that you want to seperate between multiple lines. Here is an Example, 
```
print Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ->
	ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ->
	ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur.
```
This will output in one line, instead of seperate lines. It is only used if the line of code is long and it looks better to put it on multiple lines. It comines the segments and only counts as one [segment](#segments).

## System and References

References are values that can be referenced by the code. The syntax is `owner:valName`. Examples of this being used:
```
print Hello, system:machine:username
```
This will output Hello, <USER_NAME>. If your Window's User Name was, `jbrosdev`, then it would output `Hello, jbrosdev`.
```
shape s x:50, y:50
print X = s:x
```
The creates a shape and prints its X value.

### Variable, List, and Group References

Variables, Lists, and Groups each have `length` reference which returns the length of the value. 
- group: 
  - `length` returns how many controls are in the group
- variable: 
  - `length` returns the length of characters of the value
  - `contains` returns `1` or `0` if the value is in the variable
- list: 
  - `length` returns how many values are in the list
  - `contains` returns `1` or `0` if the value is contained in the list

```
var val abcdef
var Leng val:length
// returns 6
```
```
var val abcdef
var Con val:contains:abc
// returns 1
```

### System References

Here is a tree graph made from all of the system values. Use the path as reference and join each with a `:`. For example, if trying to get to time's `utcnow`, get the path and use a colon inbetween each directory, `system:time:utcnow`.
- `system`: 

  - `time`: if just `system:time`, then it is routed to `system:time:now`
  - <details open>
      <summary><code>time</code></summary>

      - `today`: `MM/DD/YYYY HH:MM:SS AM/PM` Gets date and then 12:00:00 AM (Start of the day).
      - `now`: `MM/DD/YYYY HH:MM:SS AM/PM` Gets date and time.
      - `utcnow`: `MM/DD/YYYY HH:MM:SS AM/PM` Gets UTC (Coordinated Universal) Date and Time.
      - `unixepoch`: `MM/DD/YYYY HH:MM:SS AM/PM` Gets the Unixepoch (January 1st, 1970 `1/1/1970 12:00:00 AM`).
      - `hour24`: `HH` Gets the current hour (24 hour system).
      - `hour`: `H AM/PM` Gets the current hout (12 hour system).
      - `minute`: `00` Gets the current minute.
      - `second`: `00` Gets the current second.
      - `milisecond`: `000` Gets the current milisecond
      - `nownormal`: `MM/DD/YYYY HH:MM AM/PM` Gets date and time (not seconds).
      - `now24`: `MM/DD/YYYY HH:MM:SS` Gets date and time with 24 hours system.
      - `date`: `MM/DD/YYYY` Gets the current date.
      - `datedash`: `MM-DD-YYYY` Gets the current day.
      - `month`: `MMMM` Gets the current month (name).
      - `monthnumber`: `MM` Gets the current month (number).
      - `day`: `DD` Gets the current day (number).
      - `dayname`: `DD` Gets the current day (name).
  </details>

  - `random`: Gets a random number between the next two numbers given, `system:random:0:10`. This will give a number between 0 and 10.
  - <details open>
      <summary><code>random</code></summary>

      - `single`: Gets a random decimal between 0 and 1.
  </details>
  
  - <details open>
      <summary><code>machine</code></summary>

      - `machinename`: Gets the computer's name.
      - `osversion`: Gets the operating system's name and version
      - `is64bitoperatingsystem`: Gets if the machine is a 64 bit operating system.
      - `username`: Gets the User's Username.
      - `workingset`: Gets the machine's working set.
      - `hasshutdownstarted`: Gets if the machine has started shutting down.
  </details>

  - `isnumber`: Returns 1 or 0 if the input given is a number, `system:isnumber:inputvalue`.
    - ```
      print Input a number!
      var [console](Programs#console)Input : input [console](Programs#console)
      var isNumber system:isnumber:[console](Programs#console)Input
      ```

  - `currentfile`: Returns the current file path that is being played. If the file is unkown, returns empty.

  - `currentplaydirectory`: Returns the current path of the executable file that is playing the EZCode.

  - `space`: Returns a space character. Same as `\_` (see [Special Characters](#special-characters))

  - `newline`: Returns a new line. Same as `\n` (see [Special Characters](#special-characters))

  - `pipe`: Returns a pipe character. Same as `\$` (see [Special Characters](#special-characters))

  - `nothing`: Returns nothing. Same as `\!` (see [Special Characters](#special-characters))
</details>

### Control References

These are the list of all of the property references for controls `controlName:propertyReference`. 

- `id`: Returns the control's ID.
- `x`: Returns the control's X position.
- `y`: Returns the control's Y position.
- `w` or `width`: Returns the control's width.
- `h` or `height`: Returns the control's height.
- `backcolor`, `bc`, or `bg`: Returns the control's backcolor. Formatted as `R, G, B`.
- `backcolor-r` or `bcr`: Returns the control's red value backcolor.
- `backcolor-g` or `bcg`: Returns the control's green value backcolor.
- `backcolor-b` or `bcb`: Returns the control's blue value backcolor.
- `t` or `text`: Returns the control's text.
- `forecolor`, `fg`, or `fc`: Returns the control's forecolor. Formatted as `R, G, B`.
- `forecolor-r` or `fcr`: Returns the control's red value forecolor.
- `forecolor-g` or `fcg`: Returns the control's green value forecolor.
- `forecolor-b` or `fcb`: Returns the control's blue value forecolor.
- `click`: If the control is a button, it will return if it is currently being clicked.
- `font`: Returns the control's font. Formatted as `Font_Name, Font_Size, Font_Style`. Example, `Arial, 12, Bold`.
- `fontname`: Returns the control's font name.
- `fontsize`: Returns the control's font size.
- `fontstyle`: Returns the control's font style.
- `point` or `points`: If the control is a shape, it returns the custom points set for the control. Formatted as `{X=X_point1, Y=Y_point1}, {X=X_point2, Y=Y_point2}, etc...`.
- `auto` or `autosize`: Returns the control's auto size property.
- `multi` or `multiline`: If the control is a textbox, it will return the multiline property.
- `wrap` or `wordwrap`: If the control is a textbox, it will return the wordwrap property.
- `vertical` or `verticalscrollbar`: If the control is a textbox, it will return the horizantal scrollbar property.
- `horizantal` or `horizantalscrollbar`: If the control is a textbox, it will return the vertical scrollbar property.
- `p`: or `poly`: If the control is a shape, it returns the polygon count.
- `z` or `zindex`: Returns the control's Z index. The Z index is the order what controls are over others. When using the [`bringto`](#bringto) property, this can be effected, as well as when creating a control.
- `focused`: Returns if the control is focused.
- `enabled`: Returns if the control is enabled.
- `readonly`: If the control is a textbox, it will return the readonly property.
- `image`: Returns the control's Background image's file path.
- `imagelayout`: Returns the control's image layout.
- `name`: Returns the control's name

### Window References

These are the list of all of the property references for windows `windowName:propertyReference`. 

- `id`: Returns the window's ID.
- `x`: Returns the window's X position.
- `y`: Returns the window's Y position.
- `w` or `width`: Returns the window's width.
- `h` or `height`: Returns the window's height.
- `backcolor`, `bc`, or `bg`: Returns the window's backcolor. Formatted as `R, G, B`.
- `backcolor-r` or `bcr`: Returns the window's red value backcolor.
- `backcolor-g` or `bcg`: Returns the window's green value backcolor.
- `backcolor-b` or `bcb`: Returns the window's blue value backcolor.
- `t` or `text`: Returns the window's text.
- `forecolor`, `fg`, or `fc`: Returns the window's default forecolor. Formatted as `R, G, B`.
- `forecolor-r` or `fcr`: Returns the window's red value for the defaul forecolor.
- `forecolor-g` or `fcg`: Returns the window's for the defaul forecolor.
- `forecolor-b` or `fcb`: Returns the window's for the defaul forecolor.
- `font`: Returns the window's default font. Formatted as `Font_Name, Font_Size, Font_Style`. Example, `Arial, 12, Bold`.
- `fontname`: Returns the window's default font name.
- `fontsize`: Returns the window's default font size.
- `fontstyle`: Returns the window's default font style.
- `auto` or `autosize`: Returns the window's auto size property.
- `focused`: Returns if the window is focused.
- `enabled`: Returns if the window is enabled.
- `image`: Returns the window's Background image's file path.
- `imagelayout`: Returns the window's image layout.
- `minimizebox`: Returns if the window's minimize box is enabled.
- `maximizebox`: Returns if the window's maximize box is enabled.
- `showicon`: Returns if the window shows its icon in te top left corner.
- `showintaskbar`: Returns if the window shows its icon in the taskbar.
- `icon`: Returns the window's icon as a path.
- `state`: Returns the windows state.
- `startposition`: Returns the windows default start position.
- `type`: Returns the windows type.
- `maxwidth`: Returns the max width of the window.
- `maxheight`: Returns the max height of the window.
- `minwidth`: Returns the minumum width of the window.
- `minheight`: Returns the minumum height of the window.
- `opacity`: Returns the opacity value of the window.

## Segments

A Segment is each "line" of code. A segment includes, a line of code, each side of a `|` (go to [Code Seperation](#code-seperation)), and [comments](#comments). It does not include an empty line, or if there is nothing in between `|`. It also doesn't include [Arrows](#arrow-continuing-code) `->` that combine lines. Here is an example to explain it better.
```
print Segment 1
// This is Segment 2

print This is Segment 3 because the line above it is empty

||||||

var val Segment 4|// Segment 5
print A lot of text. ->
    This is the same line ->
    as the one above.

print Segment 6
loop 10 
{ // Segment 8
    // Segment 9
} // Segment 10
```

## Errors

Errors are the way EZCode communicates that something wrong is with the code. If it can, it will explain what is wrong, and maybe how to fix it. Depending on the Project Properties, it may include what file the error is in, [Method](#method) it is in, and also what [Segment](#segments) it is in. Here is an example of an error, `C:\Directorty\file.ezcode - Method : (ex00) Error Message in segment 1 : line of code`. The first part of the error is the file and method. The middle is the error id, the message, and the segment. The last bit is the segment of code that caused the error. If none is there, then EZCode does not know why the error occured. 

If there is an error that is just a message and keeps the program from running, it is because the [methods](#method) aren't set up properly. Here is an example, `Expected a method for a 'endmethod' in around segment {SEGMENT}`

### Error IDs

<details>
  <summary>Directory</summary>

- [ex00](#ex00)
- [ex01](#ex01)
- [ex02](#ex02)
- [ex03](#ex03)
- [ex04](#ex04)
- [ex05](#ex05)
- [ex06](#ex06)
- [ex07](#ex07)
- [ex08](#ex08)
- [ex09](#ex09)
- [ex10](#ex10)
- [ex11](#ex11)
- [ex12](#ex12)
- [ex13](#ex13)
- [ex14](#ex14)
- [ex15](#ex15)
- [ex16](#ex16)
- [ex17](#ex17)
- [ex18](#ex18)
- [ex19](#ex19)
- [ex20](#ex20)
- [ex21](#ex21)
- [ex22](#ex22)
- [ex23](#ex23)
- [ex24](#ex24)
- [ex25](#ex25)
- [ex26](#ex26)
- [ex27](#ex27)
- [ex28](#ex28)
- [ex29](#ex29)
- [ex30](#ex30)
- [ex31](#ex31)
- [ex32](#ex32)
- [ex33](#ex33)
- [ex34](#ex34)
- [ex35](#ex35)
- [ex36](#ex36)
- [ex37](#ex37)
- [ex38](#ex38)
- [ex39](#ex39)
- [ex40](#ex40)
- [ex41](#ex41)
- [ex42](#ex42)
- [ex43](#ex43)
- [ex44](#ex44)
- [ex45](#ex45)
- [ex46](#ex46)
- [ex47](#ex47)
</details>

#### **ex00**

An unknown error occured.
```csharp
$"An error occured"
```

#### **ex01**

An error occured with a keyword. Very broad and not specific.
```csharp
$"An error occured with '{keyword}'"
```

#### **ex02**

Naming Violation with control, variable, list, or group.
```csharp
$"Naming violation, '{name}' can not be used as a name"
```

#### **ex03**

Could not find control by that name. 
```csharp
$"Could not find a Control named '{name}'"
```
#### **ex04**

Could not find a Variable or Sound Player ny that name.
```csharp
$"Could not find a Variable or Sound Player named '{name}'"
```
#### **ex05**

Could not find a Group by that name.
```csharp
$"Could not find a Group named '{name}'"
```

#### **ex06**

Could not find a Window by that name.
```csharp
$"Could not find a Window named '{name}'"
```

#### **ex07**

Naming violation. There is already a variable, control, list, or group named that.
```csharp
$"Naming violation, there is already a '{keyword}' named '{name}'"
```

#### **ex08**

Unable to solve the equation.
```csharp
$"Unable to solve the equation"
```

#### **ex09**

Cannot name that variable, list, control, or group to that because there is already a method named that.
```csharp
$"Can not name '{keyowrd}' as '{name}' because there is a method already named '{name}'"
```

#### **ex10**

If `if` or `else` does not have a `:` after it (see [if](#if) or [else](#else)).
```csharp
$"Expected ':' for '{keyword}'"
```

#### **ex11**

If `if` can only have one `:` in the segment (see [if](#if)).
```csharp
$"Expected only one ':' for '{keyword}'"
```

#### **ex12**

Expected control after [`instance`](#instance)
```csharp
$"Expected 'textbox', 'label', 'button', or 'shape' after {keyword}"
```

#### **ex13**

Expected the correct declaration after [`global`](#global)
```csharp
$"Expected 'var', 'list', 'group', 'instance, 'textbox', 'label', 'button', or 'shape' after {keyword}"
```

#### **ex14**

Could not find the inputted file. 
```csharp
$"The file, '{file}' does not exist"
```

#### **ex15**

Expected the correct word for syntax.
```csharp
$"Expected {words}"
```
```csharp
$"Expected {words} for {keyword}"
```

#### **ex16**

Expected the correct word after the [special keyword](#special-keywords)
```csharp
$"Expected '{word}' keyword after '#{keyword}'"
```

#### **ex17**

Error from [EZText](eztext-docs). 

#### **ex18**

Error when making a control equal to another control, for example, `shp1 = shp2`. 
```csharp
$"An error occured when setting '{control.Name}' to '{SettingControl}'"
```

#### **ex19**

Error setting parameter when calling methods.
```csharp
$"Error setting the parameters for '{method.Name}'"
```

#### **ex20**

Expected the [output characters](#output-characters) after calling the method.
```csharp
$"Expected ':' or '=>' to set parameters for '{method.Name}'"
```

#### **ex21**

Could not find a keyword, variable, control, list, or group by that name. 
```csharp
$"Could not find a keyword, variable, control, list, or group named '{keyword}'"
```

#### **ex22**

Error solving [math](#math) value. For example, `math sqr(0)`.
```csharp
$"Error solving '{value}' with the value '{eq}' with '{keyword}'"
```
```csharp
$"Error solving '{value}' with the values '{eq}'. Try removing spaces between values, '{SYNTAX}'. This is with '{keyword}'"
```

#### **ex23**

Error solving [math](#math) value because there was not the right amount of values inputted.
```csharp
$"Expected {MANY} parts for {FUNCTION} function with '{keyword}'. Correct Syntax, '{SYNTAX}'"
```

#### **ex24**

Expected value to be in between 0 and 1.
```csharp
$"Expected {PROPERTY} value to be withen 0 and 1"
```

#### **ex25**

Error setting icon or image to control or window.
```csharp
$"An error occured setting the icon to '{name}'"
```

#### **ex26**

Errors with setting font to control or window. \
Not a valid font name,
```csharp
$"'{fontname}' is not a valid font. Try 'Arial' or go to https://learn.mcrosoft.com for more information about supported WinForms fonts. Exception for '{name}'"
```
Not a valid font size,
```csharp
$"Expected a number greater greater than zero for font size value"
```
Not a valid font style
```csharp
$"'{fontstyle}' is not a valid font style. Valid styles are: Regular, Bold, Italic, Underline, Strikeout.. Exception for '{name}'"
```

#### **ex27**

Requires a set or minumum amount of values withen `[]` for property.
```csharp
$"Requires 3 values for {PROPERTY}"
```
```csharp
$"A minumum of 3 points required for the {TYPE} '{name}'"
```

#### **ex28**

Requires value to be incased in `[]`. For example, `bg:[0;0;0]`
```csharp
$"Expected '[' and ']' for {PROPERTY} value"
```

#### **ex29**

the inputted property name is not a valid property.
```csharp
$"'{property}' is not a correct property for '{name}'"
```

#### **ex30**

Needs `:` to initialize value/s to set.
```csharp
$"Expected ':' to initialize values to the {TYPE}"
```

#### **ex31**

If there is no closing bracket `}` after openning bracket `{` for [loop](#loop), [if](#if), or [else](#else)
```csharp
$"Expected closing bracket for '{keyword}'"
```

##### **ex32**

Missing value/s to set type.
```csharp
$"Missing values to set to the {TYPE}"
```
```csharp
$"Expected values to set the {TYPE}"
```

##### **ex33**

Expected a type of [variable](#variable)
```csharp
$"Expected a list variable"
```
```csharp
$"Expected a number Variable"
```
```csharp
$"Expected a boolean Variable"
```

##### **ex34**

Missing a value or index to remove from [list](#list) or [group](#group)
```csharp
$"Expected an index or a value to remove from the list"
```
```csharp
$"Expected an index or a value to remove from the group"
```

##### **ex35**

Error splitting values to [list](#list).
```csharp
$"Error splitting values to list"
```

##### **ex36**

An error setting the variable to the correct value.
```csharp
$"There was an error setting the variable to the correct value"
```
```csharp
$"There was an Error with changing '{var.Name}'"
```

##### **ex37**

There was an error with [`?()?`](#question-mark-syntax)
```csharp
$"There was an error with ?( boolean )?"
```

##### **ex38**

Expected variable name after [output character](#output-characters)
```csharp
$"Expected new variable name after '=>' for '{keyword}'"
```
```csharp
$"Expected new variable name after ':' for '{keyword}'"
```

##### **ex39**

The minumum value can not be greater than the maximum value for [system:random](#system-and-references)
```csharp
$"Minumum can not be greater or equal to the max in 'system:random'"
```

#### **ex40**

Only certain control/s has the property
```csharp
$"Only {CONTROLS} have '{PropertyName}' property"
```

#### **ex41**

Expected a value to check if it contains, [`listName:contains:value`](#system-references)
```csharp
$"Only {CONTROLS} have '{PropertyName}' property"
```

#### **ex42**

Expected a the correct control type.
```csharp
$"Expected '{controlType}'"
```
```csharp
$"Expected '{controlType}' for '{control.Name}'"
```

#### **ex43**

A control aready has the ID inputted
```csharp
$"A control already has the id: '{id}'. For control '{control.Name}'"
```

#### **ex44**

The [instance](#instance) keyword needs to have `name:`[ property](#properties).
```csharp
$"Control instance created has no name. Please add the name property, `name:`, to the control"
```

#### **ex45**

Error with setting [points](#properties) for shape.\
Expected 2 values for point (x and y).
```csharp
$"Expected 2 values for a single point in points value"
```
Expected parenthesis to hold point in.
```csharp
$"Expected '(' and ')' for points value"
```

#### **ex46**

Expected `)` to end the equation.
```csharp
$"Syntax error. Expected ')' to end equation."
```

#### **ex47**

An error while finding the methods.
```csharp
$"An error occured while finding and setting methods. C# ERROR MESSAGE:'{ex.Message}'"
```