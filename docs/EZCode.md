# EZCode Docs

- [EZCode Docs](#ezcode-docs)
- [Keywords](#keywords)
- [Statements](#statements)
- [Classes](#classes)
- [Methods](#methods)
- [Variables](#variables)

# Keywords

All Keywords and what they do:
- `if`: An if statement. 
  - Go to [statements](#statements) for more
- `loop`: A loop statement. 
  - Go to [statements](#statements) for more
- `else`: An else statement. 
  - Go to [statements](#statements) for more
- `elif`: An elif statement.
  -  Go to [statements](#statements) for more
- `try`: A try statement.
  -  Go to [statements](#statements) for more
- `fail`: A fail statement
  -  Go to [statements](#statements) for more
- `dispose`: To dispose variable from scope. 
  - ```js
    // removes variables x and y from scope
    dispose x, y 
    ```
- `explicit`: Used to define an explicit property of the class 
  - Used before `watch`, `params`, `alias`, or `typeof`
- `watch`: Used to define regex match for creating an instance of a class
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
- `params`: This sets class parameter to value instead of properties
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
- `alias`: Adds a "second" name for the class
  - ```js
    class str {
        explicit alias string
    }
    // You can use 'str' or 'string'
    str new val 
    string new val
    ```
- `typeof`: Describes the datatype of the class to a C# datatype
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
- `make`
- `runexec`
- `undefined`
- `global`
- `new`
- `throw`
- `break`
- `return`
- `yield`
- `class`
- `method`
- `nocol`
- `get`
- `null`
- `true`
- `false`
- `and`
- `not`
- `or`

# Statements

# Classes

# Methods

# Variables