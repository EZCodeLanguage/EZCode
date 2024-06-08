  # EZCode Docs

- [EZCode Docs](#ezcode-docs)
- [Keywords](#keywords)
  - [`if`](#if)
  - [`loop`](#loop)
  - [`else`](#else)
  - [`elif`](#elif)
  - [`try`](#try)
  - [`fail`](#fail)
  - [`dispose`](#dispose)
  - [`explicit`](#explicit)
  - [`watch`](#watch)
  - [`params`](#params)
  - [`alias`](#alias)
  - [`typeof`](#typeof)
  - [`class`](#class)
  - [`nocol`](#nocol)
  - [`method`](#method)
  - [`get`](#get)
  - [`make`](#make)
  - [`runexec`](#runexec)
  - [`undefined`](#undefined)
  - [`global`](#global)
  - [`new`](#new)
  - [`throw`](#throw)
  - [`break`](#break)
  - [`yield`](#yield)
  - [`return`](#return)
  - [`true`](#true)
  - [`false`](#false)
  - [`and`](#and)
  - [`not`](#not)
  - [`or`](#or)
- [Statements](#statements)
- [Classes](#classes)
- [Methods](#methods)
- [Variables](#variables)

  # Keywords

  All Keywords and what they do:
  ## `if`

  - An if statement. Go to [statements](#statements) for more
  - ```js
    // Syntax:
    if argument : // code...
    if argument {
        // code...
    }
    // argument is some term or set of terms that return a boolean value that defines if the statement should execute
    ```

  ## `elif`
  - An elif statement. Go to [statements](#statements) for more
  - ```js
    // Syntax:
    elif argument : // code...
    elif argument {
        // code...
    }
    // argument is some term or set of terms that return a boolean value that defines if the statement should execute

    // only used after an 'if' statement
    if argument1 : // code...
    elif argument2 : // code...
    ```
    
  ## `else`

  - An else statement. Go to [statements](#statements) for more
  - ```js
    // Syntax:
    else : // code...
    else {
        // code...
    }

    // only used after an 'if' statement
    if argument1 : // code...
    else : // code...
    ```

  ## `loop`

  - A loop statement. Go to [statements](#statements) for more
  - ```js
    // Syntax:
    loop number : // code...
    loop number {
        // code...
    }
    loop argument : // code...
    loop argument {
        // code...
    }
    // number is an integer that defines how many times the loop loops.
    // argument is some term or set of terms that return a boolean value that defines how the loop will while the argument is true.
    ```

  ## `try`

  - A try statement. Go to [statements](#statements) for more.
  - This will execute code and break from the statement if an error occurs.
  - A [fail](#fail) statement is not required to be after it.
  - ```js
    // syntax:
    try : // code...
    try {
        // code...
    }
    ```

  ## `fail`

  - A fail statement Go to [statements](#statements) for more
  - This will execute if there is a [try](#try) statement before it where an error occured
  - ```js
    // syntax:
    fail : // code...
    fail {
        // code...
    }
    ```
  - The variable 'error' is automatically added with the fail statement. The value is the error message from the last try statement.
  - ```js
    try {
        some-code-with-error
    }
    fail {
        print 'error'
    }
    // this code will print the error message from the code that has an error in the try statement
    ```

  ## Include

  - This is used to include a package into the EZCode Project
  - ```js
    include PACKAGE-NAME
    include first, second
    ```

  ## Exclude

  - This is used to exclude a package from the EZCode Project
  - ```js
    exclude PACKAGE-NAME
    exclude first, second
    ```

  ## `dispose`

  - To dispose variable from scope. 
  - ```js
    // removes variables x and y from scope
    dispose x, y 

    ```

  ## `explicit`

  - Used to define an explicit property of the class 
  - Used before `watch`, `params`, `alias`, or `typeof`

  ## `watch`

  - Used to define regex match for creating an instance of a class
  - ```js
    // Syntax (inside a class):
    explicit watch REGEX_MATCH => methodName
    // Example: regex -- (this is will be INSIDE_PARANTHESIS)
    explicit watch \((.*? {INSIDE_PARANTHESIS}))\ => set : INSIDE_PARANTHESIS
    ```
  - ```js
    // This will create an instance of the class inside the line
    int i new : (5 + 5) // () converts to class that has 'get' method for 'int'
    ```

  ## `params`

  - This sets class parameter to value instead of properties
  - ```js
    // Inside A Class
    explicit params => methodName : PARAMS // PARAMS is the variable for the set method parameter
    ```
  - ```js
    // Example:
    class int {
        explicit params => set : PARAMS
        undefined Value
        method set : val {
            Value => val
        }
    }
    // With Explicit Params
    int i new : 50
    // Without
    int i new : Value:50
    ```

  ## `alias`

  - Adds a "second" name for the class
  - ```js
    class str {
        explicit alias string
    }
    // You can use 'str' or 'string'
    str new val 
    string new val
    ```

  ## `typeof`

  - Describes the datatype of the class to a C# datatype
  - ```js
    class bool {
        explicit typeof => EZCodeLanguage.EZCode.DataType("bool")
    }
    // this allows "true" and "false" to be parsed to boolean value without creating a method for this
    ```

  - For normal classes, this is not neccisary. There are the following data types:
    - `object`: This is the defualt data type for a class
    - `string`: The 'str' class in the 'main' package
    - `int`: The 'int' class in the 'main' package
    - `float`: The 'float' class in the 'main' package
    - `bool`: The 'bool' class in the 'main' package
    - `char`: The 'char' class in the 'main' package
    - `double`: There is no class associated with 'double'
    - `decimal`: There is no class associated with 'decimal'
    - `long`: There is no class associated with 'long'
    - `uint`: There is no class associated with 'uint'
    - `ulong`: There is no class associated with 'ulong'

  ## `class`

  - A class is a way to hold data as a single object that holds properties and methods.
  - To define a property in the class, create the variable outside of the method and optionally set it's value
  - Defining a method in a class, put it inside the class
  - To add class properties ([alias](#alias), [params](#params), [watch](#watch), [typeof](#typeof)), use the [explicit](#explicit) keyword
  - ```js
    class vector {
        float X new : 0
        float Y new : 0
      
        method add-vectors : @vector:left, @vector:right => @vector {
            float x new => left X
            x + right X
            float y new => left Y
            y + right Y
            vector v new : X:x, Y:y
            return v
        }
    }
    ```
  - If there is a variable named `Value`, It will automatically be assigned in many cases. For example:
  - ```js
    class int {
        undefined Value
    }
    int a new => 123 // this sets the 'Value' property to '150'
    ```
  - The [get](#get) method is used to return the class a seperate class instance
  - ```js
    class vector {
        float X new : 0
        float Y new : 0
        
        get => @str {
            str s new : 'X'\c 'Y'
            return s Value
        }
    }  
    
    vector v new : X:15, Y:6
    print v // prints: 15, 6
    ```

  ## `nocol`

  - `nocol` is used to modify a [method](#method) to not need a `:` after the method name
  - ```js
    // Without 'nocol' 
    method normal-method : @str:Value {
        
    }
    // With 'nocol'
    nocol method nocol-method : @str:Value {
        
    }
    ```
  - ```js
    // method without nocol
    normal-method : Hello World
    // method with nocol
    nocol-method Hello World
    ```

  ## `method`

  - This keyword is used to define a method
  - It can be called, before or after the method is defined because it is found in the parsing and available anywhere in the scope
  - ```js
    // SYNTAX:
    
    method METHOD_NAME {
    }
    method METHOD_NAME : PARAMETERS {
    }
    method METHOD_NAME => @CLASS_TYPE {
    }
    method METHOD_NAME : PARAMETERS => @CLASS_TYPE { 
    }

    // 'PARAMETERS' can be seperated by ',' To specify the PARAMETER type, use '@CLASS_TYPE:PARAM_NAME' 
    // For Example:
    method METHOD_NAME : @type:param1, @type:param2 {
    }
    ```
  - ```js
    // EXAMPLE: 

    method add : @int:left, @int:right {
        return (left + right)
    }
    ```
  - Use [nocol](#nocol) to change the way the method is called. 
  - ```js
    // Run Method
    add : 5, 6
    // Assign variable to method result
    undefined value => add : 5, 6
    ``` 
  - To modify the type of parameter, use `?` for optional parameter and `!` for params parameter. To add default value to parameter, put the default value after the name
  - ```js
    // optional parameter
    method create-file : @str:path, ? @str:content {
        // create file and maybe add content
    }
    create-file : PATH
    creat-file : PATH, CONTENT
    ```
  - ```js
    // params parameter
    nocol method print : ! @str:text {
        // print the text to the console
    }
    print Hello World
    print Hello, World
    ```
  - ```js
    // default value for parameter
    method print : ? @str:text Hello World {
        // print the text to the console
    }
    print // prints Hello World
    print : Hi // prints Hi
    ```

  ## `get`

  - The `get` keyword defines a method that is used in a class that returns a specific type
  - ```js
    // Example of 'get' methods inside the 'var' class inside the 'main' package
    class var {
        get => @str {
            return Value
        }
        get => @char {
            undefined returns 
            int length new => str string-length : Value
            if length != 1 {
                char c new : Value
                returns => c
            }
            else {
                throw Can not return type char from type var
            }
            return returns
        }
        get => @bool {
            return runexec EZCodeLanguage.EZHelp.ObjectParse ~> {Value}, bool
        }
        get => @int {
            return runexec EZCodeLanguage.EZHelp.ObjectParse ~> {Value}, int
        }
        get => @float {
            return runexec EZCodeLanguage.EZHelp.ObjectParse ~> {Value}, float
        }
    }

    // Create variables with values
    var a new : 120
    var b new : false

    // Assign a variable with specific type to 'var' class instance that uses a get method to return the specific class
    float fl new : a
    bool bol new : b
    ```
  - This is only used withen a [class](#class).

  ## `make`

  - Used to change grammer in the EZCode syntax
  - ```js
    // Correct Syntax:
    make REGEX_EXPRESSION => TURNS_INTO_THIS
    ```
  - It converts the syntax while parsing the file. Any text will not take variable values. For Example:
    ```js
    var hw new : 10
    make hw => print Hello World
    print hw
    // this will output 'print Hello World' because it will convert 'hw' into 'print Hello World'
    ```
  - ```js
    // Example:
    make int {NAME} = => int {NAME} new :
    // This converts:
    int i = 0 
    // Into this:
    int i new : 0
    ```
  - This can be used over multiple lines
  - ```js
    make print {TEXT} => {
        str t new : {TEXT}
        if t == Hello {
            print HI
        }
        else {
            print t
        }
    }
    // this will check if 'print Hello' happens, and if it does, print 'HI' instead
    print Hello
    ```
  - It can also be used to make loops
  - ```js
    make for {TYPE} {I1} {VAL}, {I2} {COMP} {NUM}, {I3} {METHOD} {ADD} \{ => {
        {TYPE} {I1} new : ({VAL} - 1)
        loop {I2} {COMP} {NUM} \{
        {I3} {METHOD} {ADD}
    }

    // this means,
    for var i 0, i < 10, i + 1 {
        // code...
    }

    // turns into,
    var i new : (0 - 1)
    loop i < 10 {
        i + 1
        // code...
    }
    ```

  ## `runexec`

  - This is used for executing a C# method from inside EZCode
  - The syntax is weird compared to other EZCode syntax
  - ```js
    // This is the basic syntax
    runexec Namespace.Class.Function ~> PARAM1, PARAM2
    // To pass a value from a variable from EZCode, use brackets { }
    int x new : 20
    runexec Namespace.Class.Function ~> {x}, 40
    // These can return a value. Use 'undefined' if returning value to a variable. 
    // If you return the value to a class instace, it will overide the class instance and just set the variable's value to what it returns, assuming the class does not have a variable property named 'Value' 
    undefined input => System.Console.ReadLine
    ```
  - `runexec` can also call a value from a dll that was loaded in through the [package.json](Packages.md)
  - ```js
    // make sure 'CustomDll' was in the path of the "LibraryDirectory" in the package.json
    runexec CustomDll.dll.Path.To.Function 
    ```
  - `runexec` is how anything is done inside EZCode. For example, there are no operators in EZCode, just methods that call the C# function
  - ```js
    class int {
        undefined Value
        // The '+' is the name of the method, it could be anything
        nocol method + : @int:val {
            Value => runexec EZCodeLanguage.EZHelp.Add ~> {Value}, {val}
        }
        // rest of class...
    }
    int i new : 0
    i + 50
    ```

  ## `undefined`

  - This is the keyword to make a variable that is detached from any class. It can hold any value in it.
  - This is how values are held in base classes like `int` and `str` (and more). 
  - ```js
    undefined X // Set a variable without a value
    undefined Y => VALUE // Set a variable with a value
    ```

  ## `global`

  - The `global` keyword is used to defined a variable or method that does not leave the scope. 
  - If the script has a `start` method, all global variables will be loaded before the method starts getting executed.
  - ```js
    // Defining a global variable
    global str NAME new : John Doe
    // This ensures the variable 'NAME' will not leave the scope and can be used from anywhere
    ```
  - ```js
    // Defining a global method
    global nocol method print : ! @str:__text {
        runexec EZCodeLanguage.EZHelp.Print ~> {__text}
    }
    // This ensures that the method 'print' will never leave the scope and can be called from anywhere
    ```

  ## `new`

  - This is used to define a class instance
  - ```js
    // Sytnax:
    CLASS_NAME INSTANCE_NAME new
    // Example:
    int x new
    ```
  - To create an instance of a class while setting the properties of the class, use `:`. This only works for classes that don't have the [params](#params) class property.
  - ```js
    class color {
        int R new
        int G new
        int B new
    }
    color bg new : R:59, G:59, B:59
    ```
  - ```js
    // If the class has the params class property, Everything after the ':' will be the value for the parameter in the method being called
    class color {
        int R new
        int G new
        int B new
        explicit params => set : PARAMS
        method set : val {
            // set R, G, B values
        }
    }
    color bg new : R:59, G:59, B:59
    // This, 'R:59, G:59, B:59' will be the parameter for the 'set' method and will can cause an error if the set method expects a specific format
    ```
  - To create an instance of a class while setting the instance to a certain value, use `=>`.
  - ```js
    method get-color : r, g, b => color {
        color c new : R:r, G:g, B:b
        return c
    }
    color bg new => get-color : 10, 20, 30
    ```

  ## `throw`

  - This will throw an error with a custom error message when executed
  - ```js
    throw Exception Text
    ```
  - ```js
    method get-err => @str {
        return Hello World
    }
    throw get-err
    ```

  ## `break`

  - This keyword is used to break from a statement
  - It doesn't really work as expected without `yield` because just placing a `break` will break out of the current statement even if it doesn't have `{ }`
  - ```js
    if argument {
        break
    }
    ```
  - ```js
    loop argument {
        // This will break from the 'if' statement and not the 'loop'
        if argument : break
    }
    ```
  - ```js
    loop argument {
        // This will break outside of the 'if' statement and break the 'loop'
        if argument : yield break
    }
    ```

  ## `yield`

  - This keyword is used with the `break` keyword
  - ```js
    loop argument {
        // breaks from loop
        break
    }

    loop argument {
        // This will break from the 'if' statement and not the 'loop'
        if argument : break
    }

    loop argument {
        // This will break outside of the 'if' statement and break the 'loop'
        if argument : yield break
    }
    ```
  - The `yield` keyword can not be stacked like `yield yield break`. 

  ## `return`

  - The `return` keyword is used to return a value at the end of a method
  - It is only useable inside a [method](#method).
  - It can return, `true`, `false`, a variable, any number, and any text
  - ```js
    // inside a method
    return VALUE
    ```
  - ```js
    // Example:
    method add : @int:left, @int:right => @int {
        return left + right
    }
    ```

  ## `true`

  - This is a representation of the boolean value `true`

  ## `false`

  - This is a representation of the boolean value `false`

  ## `and`

  - This keyword is used to combine arguments in a statement
  - ```js
    if arg1 and arg2 : // Only executes if arg1 and arg2 are true
    ```

  ## `not`

  - This keyword is the same thing as `!`
  - It is used before a term in an statement's argument
  - ```js
    if arg1 and not arg2 : // only executes if arg1 is true and arg2 is false
    if not arg1 : // only executes if arg1 is false
    if ! arg1 : // only executes if arg1 is false
    ```

  ## `or`

  - This keyword is used to combine arguments in a statement
  - ```js
    if arg1 or arg2 : // Only executes if arg1 or arg2 are true
    ```

# Statements

Statements are a very important part of EZCode
  - [if](#if)
  - [elif](#elif)
  - [else](#else)
  - [loop](#loop)
  - [try](#try)
  - [fail](#fail)

```js
// Syntax:
// where STATEMENT is the name of the statement ('if', 'else', etc.)

STATEMENT {
    // code...
}
STATEMENT : // code...
```
Statements that have arguments require it to return boolean. An argument is not like normal arguments in languages. `intName == 5` is valid because `==` is a method in the int class that returns a boolean. `5 == intName` is not valid because `5` is a constant and not a variable therefore it doesn't have any methods. Also, [and](#and) and [or](#or) can be used to combine arguments. 

Loops check first if the argument is actoually a number. If it is, it will loop through the loop as many times as the number states. If it is an argument, it will loop through the loop as long as the argument is true.

# Classes

Classes are a fundemental part of EZCode. The language would look extremely unstructored without them. Go to **[class](#class)** to learn how to define and create an instance of a class. Classes are extremely tricky to get right. They can hold variables, methods, and have unique properties to them like [alias](#alias), [params](#params), [watch](#watch), and [typeof](#typeof). A class can have many purposes. Here are some of the main purposes of a class (any one or multiple of these), 

1. To define a type like $int$, $str$, $bool$, etc.
2. To store data in a structured way (like if you created a $vector$ class that stored `X`, and `Y`)
3. To act as a namespace or container that can hold a bunch of methods

```js
// This class is used to define 'color' as an object
class color {
    // This explicitely states that whenever c[0;0;0] (where 0 can be any value) shows up in the code, that is refering to the color class and creates an instance of the class
    explicit watch c\[{R};{G};{B}\] => set : R, G, B
    // Define properties to the class
    int R new : 0
    int G new : 0
    int B new : 0
    // A method that is used to set the color object when 'c[0;0;0]' is found
    method set : @int:r, @int:g, @int:b {
        R = r
        G = g
        B = b
    }
}
```

# Methods

A method is basically (if not the same) as a *function* or *sub process*. To define and call a method, go to [methods](#method). Methods are used to to run code and return values

```js
method add : @int:left, @int:right => @int {
    return (left + right)
}

undefined val => add : 5, 6
print 5 + 6 = 'val'
```

## Entry

The entry to any project the `start` method

```js
method start {
  print Hello World
}
```

# Variables

Variables are used to store data. [Undefined](#undefined) is the base variable. To have a [variable as a class instane](#new), use `CLASS_NAME name new`. 

```js
undefined variable
undefined variable => // returns a value
variable => // returns a value

int variable new 
int variable new : Value:150
variable => // returns a value
variable methodName
```