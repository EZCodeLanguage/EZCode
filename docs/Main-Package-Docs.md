# Main Package Docs

This is the package with the main functionality of EZCode

- [Main Package Docs](#main-package-docs)
  - [Overview](#overview)
  - [Classes and Their Methods](#classes-and-their-methods)
    - [`class bool`](#class-bool)
    - [`class float`](#class-float)
    - [`class int`](#class-int)
    - [`class str`](#class-str)
    - [`class char`](#class-char)
    - [`class var`](#class-var)
    - [`class expressions`](#class-expressions)
    - [`class file`](#class-file)
    - [`class array`](#class-array)
  - [Global Methods](#global-methods)

## Overview

This EZCode file defines several classes (`bool`, `float`, `int`, `str`, `char`, `var`, `expressions`, `file`) and global methods. Each class is designed to handle specific data types and their associated operations, while global methods provide utility functions for the EZCode language.

## Classes and Their Methods

### `class bool`

Handles boolean values and operations.

- **Properties:**
  - `Value`: Holds the boolean value.

- **Methods:**
  - `set : val`: Assigns `val` to `Value`.
  - `= @bool:val`: Parses and assigns a boolean value.
  - `!`: Negates the current boolean value.
  - `oposite : @bool:__bol`: Returns the opposite of the boolean value.
  - `is-type : val`: Checks if `val` can be parsed as a boolean.
  - `parse : val`: Parses `val` as a boolean.

### `class float`

Handles floating-point values and operations.

- **Properties:**
  - `Value`: Holds the float value.

- **Methods:**
  - `set : @float:num`: Assigns `num` to `Value`.
  - `= @float:val`: Parses and assigns a float value.
  - `+ @float:val`: Adds `val` to `Value`.
  - `- @float:val`: Subtracts `val` from `Value`.
  - `* @float:val`: Multiplies `Value` by `val`.
  - `/ @float:val`: Divides `Value` by `val`.
  - `^ @float:val`: Raises `Value` to the power of `val`.
  - `is-type : val`: Checks if `val` can be parsed as a float.
  - `parse : val`: Parses `val` as a float.
  - Comparison methods (`<`, `>`, `<=`, `>=`, `!=`, `==`): Compare `Value` with `val` and return a boolean.

### `class int`

Handles integer values and operations.

- **Properties:**
  - `Value`: Holds the integer value.

- **Methods:**
  - `set : @int:num`: Assigns `num` to `Value`.
  - `= @int:val`: Parses and assigns an integer value.
  - `+ @int:val`: Adds `val` to `Value`.
  - `- @int:val`: Subtracts `val` from `Value`.
  - `* @int:val`: Multiplies `Value` by `val`.
  - `/ @int:val`: Divides `Value` by `val`.
  - `^ @float:val`: Raises `Value` to the power of `val`.
  - `is-type : val`: Checks if `val` can be parsed as an integer.
  - `parse : val`: Parses `val` as an integer.
  - Comparison methods (`<`, `>`, `<=`, `>=`, `!=`, `==`): Compare `Value` with `val` and return a boolean.

### `class str`

Handles string values and operations.

- **Properties:**
  - `Value`: Holds the string value.

- **Methods:**
  - `set : text`: Assigns `text` to `Value`.
  - `= @str:text`: Parses and assigns a string value.
  - `+ @str:text`: Concatenates `text` to `Value`.
  - `- @str:text`: Removes `text` from `Value`.
  - `* @str:text`: Repeats `Value` by `text`.
  - `/ text`: Splits `Value` by `text`.
  - `format : @str:text`: Formats `text`.
  - `parse : val`: Parses `val` as a string.
  - `length`: Returns the length of `Value`.
  - `string-length : val`: Returns the length of `val`.
  - Comparison methods (`<`, `>`, `<=`, `>=`, `!=`, `==`): Compare `Value` with `val` and return a boolean.
  - `lower : @str:text`: Converts `text` to lowercase.
  - `upper : @str:text`: Converts `text` to uppercase.
  - `replace : @str:text, @str:older, @str:newwer`: Replaces `older` with `newwer` in `text`.
  - `trim : @str:text`: Trims whitespace from `text`.
  - `substring : @str:text, @int:index, @int:length`: Extracts a substring from `text`.
  - `starts-with : @str:text, @str:val`: Checks if `text` starts with `val`.
  - `ends-with : @str:text, @str:val`: Checks if `text` ends with `val`.

### `class char`

Handles character values and operations.

- **Properties:**
  - `Value`: Holds the character value.

- **Methods:**
  - `set : ch`: Assigns `ch` to `Value`.
  - `= @char:ch`: Parses and assigns a character value.
  - `parse : val`: Parses `val` as a character.
  - `!= val`: Compares `Value` with `val` and returns a boolean.
  - `== val`: Compares `Value` with `val` and returns a boolean.
  - `lower : @char:val`: Converts `val` to lowercase.
  - `upper : @char:val`: Converts `val` to uppercase.

### `class var`

Handles generic variable values and operations.

- **Properties:**
  - `Value`: Holds the variable value.

- **Methods:**
  - `set : val`: Assigns `val` to `Value`.
  - `= val`: Parses and assigns a value.
  - `+ val`: Adds `val` to `Value`.
  - `- val`: Subtracts `val` from `Value`.
  - `* val`: Multiplies `Value` by `val`.
  - `/ val`: Divides `Value` by `val`.
  - `get => @str`: Returns `Value` as a string.
  - `get => @char`: Returns `Value` as a char if its length is 1, otherwise throws an error.
  - `get => @bool`: Returns `Value` as a boolean.
  - `get => @int`: Returns `Value` as an integer.
  - `get => @float`: Returns `Value` as a float.
  - `get => @var`: Returns `Value` as a variable.
  - Comparison methods (`<`, `>`, `<=`, `>=`, `!=`, `==`): Compare `Value` with `val` and return a boolean.

### `class expressions`

Handles expressions.

- **Properties:**
  - `Value`: Holds the expression value.

- **Methods:**
  - `expression : vals`: Parses and assigns an expression.
  - `get => @bool`: Evaluates the expression and returns a boolean.
  - `get => @float`: Evaluates the expression and returns a float.
  - `get => @int`: Evaluates the expression and returns an integer.
  - `get => @str`: Evaluates the expression and returns a string.
  - `get => @var`: Evaluates the expression and returns a variable.

### `class file`

Handles file operations.

- **Methods:**
  - `read : @str:path`: Reads the file at `path` and returns its content as a string.
  - `write : @str:path, @str:text`: Writes `text` to the file at `path`.
  - `create : @str:path`: Creates a file at `path`.
  - `delete : @str:path`: Deletes the file at `path`.

### `class array`

This class represents an array data structure and provides methods for manipulating arrays.

- **Properties:**
  - `Value`: Holds the array value.

- **Methods:**
  - `get-value : @int:index`: Retrieves the value at the specified index.
  - `set-value : @int:index, value`: Sets the value at the specified index.
  - `append : value`: Appends a value to the end of the array.
  - `prepend : value`: Prepends a value to the beginning of the array.
  - `length => @int`: Returns the length of the array.
  - `hashcode => @int`: Calculates the hash code of the array.
  - `first`: Returns the first element of the array.
  - `last`: Returns the last element of the array.
  - `to-list : => @list`: Converts the array to a list.
  - `get-array : _array => @array`: Gets the array value.
  - `generate-array : ?@int:length => @array`: Generates an array of the specified length.
  - `split : @str:_text, @str:_splitter => @array`: Splits a string into an array using a specified splitter.

## Global Methods

Basic methods
- `print ! @str:__text`: Prints `__text`.
- `run-code : @str:__code`: Runs the EZCode specified by `__code`.
- `clear`: Clears the console or environment.
- `input => @str`: Returns console input.
- `is-type : __val, __type => @bool`: Checks if `__val` is of type `__type`.
- `regex-match : @str:__val, @str:__match => @bool`: Checks if `__val` matches the regex `__match`.
- `return : __val`: Returns `__val` to the caller.

Math Methods
- `add obj1, obj2 => @float`: Adds `obj1` and `obj2` together.
- `subtract obj1, obj2 => @float`: Subtracts `obj2` from `obj1`.
- `multiply obj1, obj2 => @float`: Multiplies `obj1` and `obj2`.
- `divide obj1, obj2 => @float`: Divides `obj1` by `obj2`.
- `avg ! @float:nums => @float`: Calculates the average of the numbers in `nums`.
- `clamp @float:val, @float:min, @float:max => @float`: Clamps `val` between `min` and `max`.
- `random @float:min, @float:max => @int`: Generates a random integer between `min` and `max`.
- `round-to-int @float:num => @int`: Rounds `num` to the nearest integer.
- `pi => @float`: Returns the value of Pi.
- `logbase-e => @float`: Returns the value of Euler's number (e).
- `operate @str:op, ! @float:obj => @float`: Performs a mathematical operation specified by `op` on `obj`.