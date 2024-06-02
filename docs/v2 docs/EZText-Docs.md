# EZText

EZText is a part of [EZCode](ezcode-docs) that translates Formatted English to EZCode. The English has to be in a specific format for it to work, but it allows EZCode to be written in paragraphs. It is initiated in EZCode with the [Special Keyword](ezcode-docs#special-keywords) `# eztext start` and `# eztext end`. Here is an example,

```
# eztext start
Create a variable named X with the value of 10. If the console input is equal to AddFifty, then add 50 to X. If not, then subtract 50 from X.
# eztext end
```
This EZText translates to the following EZCode,
```
var X 10
var EZTEXT_GENERATED_VARIABLE : input console
if 'EZTEXT_GENERATED_VARIABLE' = AddFifty : X + 50
else : X - 50
```

## Replacements

Some series of text is replaced when creating the different lines to execute. Here is the C# code used for this
```csharp
string[] lines = EZText.Replace(".", "|").Split(new[] { '\n', '|' }).Select(x => x.Trim()
    .Replace("is not equal to", "!=")
    .Replace("is not", "!")
    .Replace("is equal to", "equals")
    .Replace(" equals ", " = ")
    .Replace("is greater than or equal to", ">=")
    .Replace("is less than or equal to", "<=")
    .Replace("is greater than", ">")
    .Replace("is less than", "<")
    .Replace(", then ", " then ")
    .Replace(" then ", " : ")
    .Replace("[p]", ".")
    .Replace("[SPACE]", " ")
    ).Where(y => !y.Equals("")).ToArray();
```

## Syntax
__Directory:__
- [Add to list](#add-to-list)
- [Add to variable](#add-to-variable)
- [Clear list](#clear-list)
- [Clear the console](#clear-the-console)
- [Create list](#create-list)
- [Create variable](#create-variable)
- [Delete File](#delete-file)
- [Destroy List](#destroy-list)
- [Display messagebox](#display-messagebox)
- [Divide Variable](#divide-variable)
- [If](#if)
- [If not](#if-not)
- [Math](#math)
- [Multiply variable](#multiply-variable)
- [Play](#play)
- [Remove from list](#remove-from-list)
- [Replace in list](#replace-in-list)
- [Split list](#split-list)
- [Stop](#stop)
- [Subtract from variable](#subtract-from-variable)
- [Wait](#wait)
- [Write to console](#write-to-console)
- [Write to file](#write-to-file)
- [Variable equals](#variable-equals)

---

### Add To List

The syntax for this, `Add value to list NAME`. The `NAME` is the name of the list. This will translate to `NAME add value`.

### Add To Variable

The syntax is, `Add value to Var`. `Var` is the name of the variable. This translates to `Var + value`.

### Clear List

The syntax for this, `Clear the list NAME`. The `NAME` is the name of the list. This will translate to `NAME clear`.

### Clear the Console

The syntax is simple for this, `Clear the console`. This will translate to `clear`.

### Create List

The syntax is, `Create a list named NAME` or `Crate a list named NAME with the value(s) VALUE`. The `NAME` is the name of the list and `VALUE` is the value(s) of the list. If just `value`, it will search for the next word. If `values`, it will search for everything after it. It needs to be seperated with commas `,` like in EZCode. For Example, `create a list named List1 with the values of A, B, C`.

### Create Variable

The syntax is, `Create a vcariable named NAME` or `Crate a variable named NAME with the value VALUE`. The `NAME` is the name of the variable and `VALUE` is the value of the variable. To assign the variable to other values, use `of` after `value`. Here are the following values that it can be set to,
- `of if A intersects B`: This will translate to, `var NAME : intersects A B`.
- `of if the file FILEPATH exists`: This will translate to, `var NAME : file exists FILEPATH`.
- `of if the file path FILEPATH is valid`: This will translate to, `var NAME : file validpath FILEPATH`.
- `of the console input`: This translates to, `var NAME : input console`.
- `of the current keys being pressed`: This translates to, `var NAME : input key`.
- `of if KEY is being pressed`: This translates to, `var NAME : input key KEY`.
- `of the current mouse buttons being pressed`: This translates to, `var NAME : input mouse button`.
- `of if the BUTTON mouse button is pressed`: This translates to, `var NAME : input mouse button BUTTON`.
- `of the current mouse position`: This translates to, `var NAME : input mouse position`.
- `of the current X/Y mouse position`: This translates to, `var NAME : input mouse position X/Y`.
- `of the current mouse wheel state`: This translates to, `var NAME : input mouse wheel`.

Here is an example,
```
Create a variable named X with the value of the current X mouse position
```

### Delete File

The syntax is, `Delete the file FILEPATH`. `FILEPATH` is the file path that is going to be deleted. This will translate to `file delete FILEPATH`.

### Destroy List

The syntax for this, `Destroy NAME`. The `NAME` is the name of the list. This will translate to `destroy NAME`.

### Display Messagebox

The syntax is, `Display TEXT with the title, TITLE via messagebox`. `TEXT` is the text of the messagebox and `TITLE` is the title of the messagebox. This translates to `messagebox TITLE TEXT`

### Divide Variable

The syntax is, `Divide Var by value`. `Var` is the name of the variable. This translates to `Var / value`.

### If 

The syntax to this is similar to EZCode, `if argument : code...`. Although, `:` the same as putting `then` or `, then` with the [replacements](#replacements). As an example, `if X is greater than or equal to 20, then X is equal to 50`. This will translate to `if x >= 20 : X = 50`. It can also create variables inside of the if with the followinf syntax,
- `if A intersects B then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : intersects A B
    if 'EZTEXT_GENERATED_VARIABLE' : CODE
    ```
- `if the console input is equal to TEXT then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : input console
    if 'EZTEXT_GENERATED_VARIABLE' = TEXT : CODE
    ```
- `if the contents of the file FILEPATH is equal to TEXT then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : file read FILEPATH
    if 'EZTEXT_GENERATED_VARIABLE' = TEXT : CODE
    ```
- `if KEY is being pressed then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : input key KEY
    if 'EZTEXT_GENERATED_VARIABLE' : CODE
    ```
- `if the BUTTON mouse button is being pressed then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : input mouse button BUTTON
    if 'EZTEXT_GENERATED_VARIABLE' : CODE
    ```
- `if the X/Y mouse position is equal to VALUE then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : input mouse position X/Y
    if 'EZTEXT_GENERATED_VARIABLE' = VALUE : CODE
    ```
- `if the mouse wheel state is equal to VALUE then CODE`: This will translate to, 
  - ```
    var EZTEXT_GENERATED_VARIABLE : input mouse wheel
    if 'EZTEXT_GENERATED_VARIABLE' = VALUE : CODE
    ```

### If Not

The syntax is, `if not : CODE`. `:` is replaced by `then` or `, then` with the [replacements](#replacements). It is translated to `else : CODE`. It is only used after an [if](#if) and will be executed if it is false.

### Math

The syntax is, `VAR = MATH`. `MATH` is any mathematical stuff including Functions. The `=` is the same as `is equal to` or `equals` with [replacement](#replacements). It translates to, `var VAR : math MATH`.

### Multiply Variable

The syntax is, `Multiply Var by value`. `Var` is the name of the variable. This translates to `Var * value`.

### Play

The syntax is, `play the file/project FILEPATH`. Either use `file` or `project`. It is translated as `file play file/project FILEPATH`.

### Remove From List

The syntax is, `Remove INDEX/VALUE in list NAME`. `NAME` is the name of the list and `INDEX/VALUE` is either the index or value that is going to be removed. It is translated to `NAME remove INDEX/VALUE`.

### Replace In List

The syntax is, `Replace INDEX with VALUE in list NAME`. `NAME` is the name of the list. `INDEX` is the index of the value that is going ot be replaced with `VALUE`. It is translated to `NAME equals INDEX VALUE`.

### Split List

The syntax is `NAME = split by VALUE SPLITTER`. The `=` is the same as `is equal to` or `equals` with [replacement](#replacements). The `NAME` is the name of the list, `VALUE` is the value going to be split, and the `SPLITTER` is the value that is going to split the `VALUE`. It translates to, `NAME split VALUE SPLITTER`.

### Stop

The syntax, `Stop the program/file`. Expected either `program` or `file`. Translates to `stop program` or `stop file`.

### Subtract From Variable 

The syntax is, `Add value from Var`. `Var` is the name of the variable. This translates to `Var - value`.

### Wait

The syntax is, `Wait X miliseconds` or `Wait until X is equal to true`. `X` in the first example is a number. In the second example, it is an argument. It translates to, `await X`.

### Write To Console

The syntax is, `Write TEXT to the console`. `TEXT` is the text that ias going to be written to the console. It can be more than one word. It translates to, `print TEXT`.

### Write To File

The syntax is, `Write TEXT to the file FILEPATH`. `FILEPATH` is the file path and `TEXT` is the text that is going to be written. This will translate to `file write TEXT FILEPATH`.

### Variable Equals

The syntax is, `VAR = VALUE`. `VALUE` is the value that the variable is going to be equal to. The `=` is the same as `is equal to` or `equals` with [replacement](#replacements). It translates to, `VAR = VALUE`.