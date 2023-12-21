# IDE Docs

The EZ IDE is the easiest way to develop with EZCode. It has a [Debugger](#debugger-view), [Console](console), and [Visual Output](visual-output) built into the IDE. It has a [project explorer](project-explorer), and many [settings](#settings) to customize your experience. There is alsol the [Keyword Highlighting Textbox](#keyword-highlighting-textbox) to type in the code. 

<details open>
<summary><strong>Directory</strong></summary>

- [Tool Strip](#tool-strip)
- [Project Explorer](#project-explorer)
- [Keyword Highlighting Textbox](#keyword-highlighting-textbox)
- [Bottom Tab Control](#bottom-tab-control)
- [Debugging](#debugging)
- [Settings](#settings)
- [Project Settings](#project-settings)
- [Text to Code](#text-to-code)
</details>

## Tool Strip

![Tool Strip](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.png)

The Tool Strip is the way to access anything in the IDE.
- [File](#file)
- [Edit](#edit)
- [Play](#play)
- [Debug](#debug)
- [Help](#help)
- [Current File](#current-file)

### File

 File > New | File > Open
----|----
![File 1](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.File.1.png) | ![File 2](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.File.2.png)

- New > File: This will open a file dialog to create a new file. If the file is not in the current directory, it will ask to open the file's directory. If not, it will add the file to the [Project Explorer](#project-explorer) in a seperate item.
- New > Project: This will open the [New Project](#new-project) dialog.
- Open > File: This will open a file dialog to open a file and open a prompt for the user. If the file is not in the current directory, it will ask to open the file's directory. If not, it will add the file to the [Project Explorer](#project-explorer) in a seperate item.
- Open > Folder: This will open a folder dialog and open the folder to the [Project Explorer](#project-explorer).
- Open > Project: This will open a file dialog to find a `ezproj` file. It will then open the [Project Explorer](#project-explorer) to that directory.
- Save: This will save the current file.
- Exit: This will exit EZ IDE.

#### New Project

![New Project](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/New_Project.png)

This is used to create a new project. Input the Project's directory and name. Then optionally go into the [project properties](#project-settings) to edit the [EZProj](EZProject-Docs) file. It can be edited later.

### Edit

![Edit](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.Edit.png)

- Edit > Copy: Copies the selected text.
- Edit > Cut: Cuts the selected text.
- Edit > Paste: Pastes to the selected text.
- Edit > Delete: Deletes the selected text.
- Edit > Undo: Undos the last Action in the [Keyword Highlighting Textbox](#keyword-highlighting-textbox).
- Edit > Redo: Redos the last Action in the [Keyword Highlighting Textbox](#keyword-highlighting-textbox).
- Clear Tree View: Clears the [Project Explorer](#project-explorer).
- Refresh Tree View: Refreshes the [Project Explorer](#project-explorer) to any changes in the current directory.
- Text To Code: Opens [Text to Code](#text-to-code) dialog.
- Project Settings: Opens the [Project Settings](#project-settings) dialog.
- Settings/Preferences: Opens the [Settings](#settings) dialog.

### Play

![Play](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.Play.png)

- Play Project: Plays the [current project](#current-project).
- Play File: Plays the currently open file.
- Quit: Quits any playing project or file.
- Play in Dedicated Window (toggle): Changes the [Dedicated Window](#play-in-dedicated-window) setting. 

### Debug

![Debug](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.Debug.png)

- Start > Debug Project: This [starts debugging](#debugging) the [current project](#current-project).
- Start > Debug File: This [starts debugging](#debugging) the cuurent file.
- Next Segment: This goes to the next [segment](EZCode-Docs#segments) in a [debug session](#debugging).
- Next Breakpoint: This goes to the next [breakpoint] in the [debug session](#debugging).
- End Debug Session: This will end a [debug session](#debugging).
- Insert Breakpoint: This opens the [Insert Breakpoint](#insert-breakpoints) dialog.
- All Breakpoints: This opens the [all breapoints](#breakpoints) dialog.

### Help

![Help](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.Help.png)

- Docs: This opens the [Docs](Ide-docs).
- Version Textbox: This contains the current IDE version of the program.

### Current File

![Current File](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tool_Strip.Current_File.png)

This shows the current open file. 

## Project Explorer

![Project Explorer](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Project_Explorer.png)

This is a Tree View that allows an easy way to access files. Its very simple and if right clicking in it, a menu appears to create a new file, rename a file, or delete a file.

## Keyword Highlighting Textbox

![Keyword Highlighting Textbox](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Keyword_Highlighting_Textbox.png)

This is where the user codes. It has semantic highlighting using Regex text matching. It uses the [Fast Color TextBox](https://github.com/PavelTorgashov/FastColoredTextBox) as the textbox. To view and change the syntax highlighing, go to the `EZCode_Syntax.xml` or `EZProj_Syntax.xml` in the Executable's directory (usually `C:/Program Files/EZCode/EZ_IDE <RELEASE_NAME>/EZCode_Syntax.xml`).\
Here is an example for comments in the `EZCode_Syntax.xml`:
```
<style name="Comment" color="Green" fontStyle="Regular"/>
<rule style="Comment">//.*</rule>
```

### Intellisense

![Intellisense](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Keyword_Highlighting_Textbox.intellisense.png)

This uses the built in [Fast Color TextBox](https://github.com/PavelTorgashov/FastColoredTextBox) Intellisense. It will open with Ctrl+Space or if there are any matches. It uses the [EZCode Keywords](EZCode-Docs#keywords) along with a couple snippets for matches. It also uses the current text in the [Keyword Highlighting Textbox](#keyword-highlighting-textbox) to look for matches. It waits for so many presses before it refreshes the Intellisense matches. The amount is the [Key Presses Before Intellisense Refresh setting](#key-presses-before-intellisense-refresh).

## Bottom Tab Control

### Console

![Tab Control Console](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tab_Control.Console.png)

The console tab is the tab that has the [console](programs#ez-ide) in it. It has the Console Output, Console Input, Send Button, and Clear Button.

### Visual Output

![Tab Control Visual Output](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tab_Control.Visual_Output.png)

This has the [Visual Output](programs#visual-output) in this tab.

### Debugger View

![Tab Control Debugger](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Tab_Control.Debug.png)

The Debug tab contains the following,
- Breakpoint View: Click the `...` in the Breakpoint[] View to open the property view of all of the current breakpoints.
- Controls View: Click the `...` in the Controls[] View to open the property view of all of the current controls in the last or current EZCode play or debug.
- Var Listbox: Contains all of the current variables in the play or debug with there values.
- Current Segment Textbox: Shows the current Segment being played.
- Mini Console: A mini [console](#console).
- Next Segment: This goes to the next [segment](EZCode-Docs#segments) in a [debug session](#debugging).
- Next Breakpoint: This goes to the next [breakpoint] in the [debug session](#debugging).
- Quit: This will end a [debug session](#debugging).

## Debugging

![Debug Example](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Debug_Example.png)

Debugging is a way of playing a program where it shows certain values in the program as well as being able to go line by line through the code. This is great to use when trying to solve a problem that occurs in the code. To start a debug session, go to [Debug > Start > Debug File](#debug) or [Debug > Start > Debug Project](#debug). The hotkeys for that is F5 or F6. Use [Breakpoints](#breakpoints) to make it easier to debug a problem. The breakpoint will stop a debug program and wait for [Debug > Next Breakpoint](#debug) or [Debug > Next Segment](#debug) to be clicked to move on to the next segment or breakpoint. 

## Breakpoints

![Breakpoints](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Breakpoints.png)

Breakpoints are used to pause a debug session so the programmer can view values or go line by line from there. To set a breakpoint in the code, use [Debug > Insert Breakpoint](#debug) or just F9. It will open up the [insert breakpoint](#insert-breakpoint) dialog to make it easier to add a breakpoint. To remove it, go to [Debug > All Breakpoints](#debug) and select the breakpoint to remove. A breakpoint stores the file its in, the method its in (if no method, leave it blank), and the segment to stop at.

### Insert Breakpoint

![Insert Breakpoint](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Breakpoints.Insert.png)

This will automatically fill out some of the values of the breakpoint to make it easier to add it. It isn't perfect with the segments a may need to be checked. It will fill out the segment, method, and file of the breakpoint.

## Settings

Settings > Settings | Settings > Debug
----|----
![Settings IDE](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Settings.IDE.png) | ![Settings Debug](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Settings.Debug.png)

The IDE Settings are to help personalize the experience for the user, as well as important insights to the IDE.
- [Save Tree View Folder Accross Startups](#save-file-when-start-playing)
- [Current Project](#current-project)
- [Default Zoom](#default-zoom)
- [Save File When Start Playing](#save-file-when-start-playing)
- [Play In Dedicated Window](#play-in-dedicated-window)
- [Key Presses Before Intellisense Refresh](#key-presses-before-intellisense-refresh)
- [Allow Pausing Debug Session With Next Segment](#allow-pausing-debug-session-with-next-segment)
- [Highlight Current Line](#highlight-current-line)

### Save Tree View Folder Accross Startups

This will save the [Project Explorer](#project-explorer) over startups to allow the user to get right back into coding. Default is True.

### Current Project

This is the current open [EZProject](EZProject-Docs). The current project will determine the project for [project settings](#project-settings), [playing project](#play), and [debugging project](#debug).

### Default Zoom

This is the current zoom of the [Keyword Highlighting Textbox](#keyword-highlighting-textbox). Default is 100.

### Save File When Start Playing

This will save the open file when starting to play to allow unsaved changes to be in the program and save the file. default is True.

### Play In Dedicated Window

This will play the program in its own window. It doesn't effect debugging. Default is False.

### Key Presses Before Intellisense Refresh

Intellisense uses the current text in the [Keyword Highlighting Textbox](#keyword-highlighting-textbox) to look for matches. It waits for so many presses before it refreshes the Intellisense for matches. Default is 5 presses.

### Allow Pausing Debug Session With Next Segment

This allows the user to press [Next Segment](#debug) in the middle of debugging to pause the program and act like a breakpont was just hit. Default is True.

### Highlight Current Line

This will try to highlight the current segment that the code is on. It works best when debugging a file instead of a project. Default is True.

## Project Settings

![Project Settings](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Project_Settings.png)

The Project Settings Dialog is to make it easier for the user to edit the project settings. It takes the [current project](#current-project) and reads the [EZProj](EZProject-Docs) file. It can't read the file if there are any [EZProj Vars](EZProject-Docs#variable). It will try its best to read the file and display it in the dialog. It may not work perfectly specifically with the files to include. If the user updates the project settings with the dialog, a message box will pop up asking if the EZProj file should be updated. If this is not wanted, the Project Settings will revert back to the file. If it is wanted, then the [current project's](#current-project) EZProj file will be updated to show the new project settings.

## Text to Code

![Text to Code](https://github.com/JBrosDevelopment/EZCode/blob/master/docs/Images/Text_To_Code.png)

This converts normal text to formatted text to be put in wherever. It will convert this, `Hello World, My name is John Doe!` to `Hello\_World\c\_My\_name\_is\_John\_Doe\e`.