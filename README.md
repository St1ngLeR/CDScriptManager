# CDScriptManager
![CDScript](https://github.com/St1ngLeR/CDScriptManager/blob/master/CDScript_Logo1.png?raw=true)
## Introduction
> [!IMPORTANT]
> To run the application, you need to install [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

**CDScriptManager (*later CDSM*)** is a tool for fast and handy managing CDScript files for classic Crashday (1.0-1.2). CDSM gives you the opportunity to configure and apply special script files to a game, which open up the possibilities of low-level modding.

## Program overview
### Main
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/d80fa094-8417-4874-88ad-1a4675a70413)

CDSM is a small window that consists of various main elements:
1. "Game" tab. Contains options related to the game itself (see ["Game" tab](tree/master?tab=readme-ov-file#game-tab))
2. "Presets" tab. Contains options related to the script presets (check for 11-13 items)
3. "About" tab. Contains miscellaneous ~~not really importent~~ stuff (check for 14-15 items)
4. Script list. The list shows all scripts, located in `scripts` folder in the game directory.
5. Script information. The area for displaying the description of the selected script.
6. "Refresh" button. The button to refrest script list.
7. "Configure" button. The button to configure script settings in separated window. The button is enabled only if script has settings to configure.
### "Game" tab
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/c0556bfb-8b54-45b2-a435-1dfbd5dd9aa9)

8. "Run game" option. Run the game executable (`.exe` or `.dll`).
9. "Set game executable" option. Change the game executable (`.exe` or `.dll`).
10. "Create shortcut" option. Create shortcut with preinstalled game executable and script preset for quick game run.
### "Presets" tab
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/6d751ff5-ace5-4dba-9b5f-ac5dac1eba41)

11. "New preset" option. Create a new script preset. When creating a preset, it is automatically selected as the current one.
12. "Open preset" option. Loads preset file.
13. "Save preset as" option. Works similarly to the "New preset" option, but without preset selection.
### "About" tab
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/698ffcef-9cff-4df7-b586-4ef2061c9b3a)

14. "Information" option. Opens a separated window, which contains CDSM version.
15. "GitHub page" option. Opens St1ngLeR's GitHub page.
### "Configuration" window
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/b88389fa-0742-45bf-8cfa-250bced96a5d)

16. Settings list. The list of script settings that can be configurated by user. The count of settings is depends on the script.
17. "Reset button". Reset all script settings to the default values.

## Command-line arguments
CDSM supports command-line arguments to speed up and simplify the procedure of running the game:
- `/preset="(preset_name)"` - loads specified preset from `scripts/presets` folder.
- `/exec="(executable_file_name)"` - runs specified executable file (`.exe` or `.dll`).
> [!IMPORTANT]
> The order of the arguments is important. If you want to start the game with a preset, make sure that the preset is set **BEFORE** assigning the file to run.

> [!IMPORTANT]
> For the correct assignment of the preset and the executable file, it's recommended to enclose their names in quotation marks to avoid malfunctions. This is especially true if there is a space character in their names.

## Files overview
### Contents
Out of the box, the program consists of one file - the executive file itself. But after the first launch, the program creates some files, which are described below:
#### `cdscript_log.txt`
The logging file that captures each action of the program. This file can be useful for script writing. After each launch, the file is written over again.
#### `cdsmanager_settings.ini`
The file that contains basic settings for proper work of program.
#### `scripts` folder
A folder for CDScript files that CDSM uses to managing them. Initially, the folder does not contain any scripts.
#### `presets` folder
A folder inside the `scripts` folder for script preset files. Initially, the folder contains a default preset.
### Structure
> [!WARNING]
> The following files are updated by the program itself, so manual modification is not recommended unless as a last resort.

The files created and used by CDSM have their own structure, which is described below:
#### `cdsmanager_settings.ini`
- `skipmanager` - allow to skip manager on it's launch for quick run game. (default value is `0`)
- `exec` - game executable. (default value is `Crashday.exe`)
- `currentpreset` - current selected script preset. (default value is `default`)
#### `Preset file (scripts/presets/*.ini)`
- `[(script_name).cdscript]` section contains all values related to it's script.
- `~state` is the main key that is rensposible for disabling/enabling script to load. (`false` - disable, `true` - enable)

The script section can also use other keys if the script has settings that the user can configure in the manager.
## Writing a CDScript file
> [!CAUTION]
> This section is dedicated to creating script files and is intended exclusively for experienced users. Any manipulation of scripts can lead to malfunction of the game. Do this at your own risk.

As described earlier, the main task of CDSM is to integrate CDScript files to the Crashday game. CDScript files are unencrypted human-readable files with the extension `.cdscript`. Script files can be created, edited and opened in any text editor (for example, [Notepad++](https://github.com/notepad-plus-plus/notepad-plus-plus)).
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/2efcb020-cacd-49eb-a862-560694a6192f)
*Sample code "Hello World!" in the form of a CDScript. The script outputs the line "Hello World!" to the `cdscript_log.txt` logging file.*

Above you can see an example of a simple script. Based on it, the explanations for writing the script are given below.
The CDScript language is very limited (for now yet) and similar to the C# language, although it has some differences. 
### Main features
- All comments are marked with a "#" character (for example, `# this is my useful comment!`). However, keep in mind that only single-line comments are supported.
- CDScript does not use any specific character to end a line. Use a new line of other actions.
- To go to the next line, "string" values ​​use `\n`.
- For "integer" values CDScript is supports simple mathematical operations (addition, subtraction, multiplication, division).
### Main variables
In order for information about scripts to be conveniently displayed in managers, the CDScript language introduces the main variables that are read by the manager when selecting a script from the list of scripts. There are only 6 main variables and the beginning of their names are marked with a "~" character: `~cdscript_name`, `~cdscript_version`, `~cdscript_description`, `~cdscript_author`, `~cdscript_website`, `~cdscript_email`.

These variables do not need to be represented, because their purpose is indicated directly in their name. In any case, despite the fact that these variables are the main ones, they are not necessary for the script to work. They are needed exclusively for display in CDSM. If any of these main variables are not written in the script, the manager show it as "(unknown)".
### Script body
To create a script body, like creating a function in C#, you need to set a header `script (your_script_body_name)` and limit the body with curly brackets. Tabulation is not important here, but it is recommended to use it for a beautiful look. Example:
```
script MyScript
{
  do something...
}
```
### Methods
As in C#, CDScript uses various methods that you can call and work with them inside the code. Here are descriptions of all existing CDScript methods:
- `Console.Print` (string) - outputs lines `cdscript_log.txt` to the logging file. Example:
```
Console.Print("My awesome line!\nMy MORE awesome line!")
```
- `Exec.StartPoint` (bytes) - assigns a starting point, which is represented as a byte sequence for working with the executable file of the game. Example:
```
Exec.StartPoint = 00 01 02 0A 0B 0C
```
- `Exec.Replace.(output_type)` (bytes, int8, int16, int32, int64, float, string) - replaces the contents of the executable file with the new specified values after the starting point taken from `Exec.StartPoint` before it. Example:
```
Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point
Exec.Replace.Float = 0.25  # Converts float value to bytes and replace them after the staring point specified above
```
- `Setting.Create` (textBox, numericUpDown) - creates configurable script setting. This method should be called with following template - `variable_name, text_in_manager, output_type, default_value`. The separator is "comma" (","). Example:
```
Setting.Create = MySetting, My awesome setting:, textBox, AWESOME  # This line creates setting with variable "MySetting", with type "textBox" with default value "AWESOME", which is named in "Configuration" window as "My awesome setting:"
```
### Script settings
Scripts can also have settings that are changed in the manager through the "Configuration" window. To set the settings, you need to specify the script setting body inside the script body with `setting (your_setting_body_name)` and limit the body with curly brackets. Example:
```
script MyScript
{
  setting MySetting
  {
    do something...
  }
}
```
To make setting configurable, you need to call the method `Setting.Create`. The rules of the call are described above. To apply the value from the setting, you need to call the method by specifying its variable. Example:
```
script MyScript
{
  setting MySetting
  {
    Setting.Create = MySetting, My awesome setting:, textBox, AWESOME  # Creates setting
    Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point
    Exec.Replace.String = MySetting  # Replace content on string from "MySetting" setting
  }
}
```
### Tips
For a more convenient and practical script writing, various tips that may be useful are described below.
#### Replacement in a row
To replace several values in a row after the starting point, you just need to call the method `Exec.Replace.(output_type)` as many times in a row as you need. Example:
```
Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point
Exec.Replace.Float = 0.25  # Replace after starting point
Exec.Replace.Bytes = 2B 3A 0D E5  # Replace after replaced value specified above
```
#### Starting points for different game versions
Crashday has three released versions (1.0, 1.1, 1.2) and each of these versions has executable files whose structure differs from each other and, accordingly, the byte sequences are different. To avoid the fact that, for example, the script works for version 1.2, but not for 1.1 and 1.0, then you need to set the starting points for each version, where they differ. Example:
```
Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point for 1.0
Exec.Replace.Float = 0.25  # Replace after starting point
Exec.StartPoint = 2A 7B 3F 4A A0 03  # Sets the starting point for 1.1
Exec.Replace.Float = 0.25  # Replace after starting point
Exec.StartPoint = 02 0F 1B 07 04 0E  # Sets the starting point for 1.2
Exec.Replace.Float = 0.25  # Replace after starting point
```
In the example above, three starting points are set, at which the same value is replaced. CDScript works in such a way that the manager ignores the replacement of values if the specified sequence of bytes is not found. Thus, this example will be suitable for all three versions of the game.
## 3rd-party plugins
The application is using [Costura.Fody](https://github.com/Fody/Costura/) to compile all resources into a single executable file.
