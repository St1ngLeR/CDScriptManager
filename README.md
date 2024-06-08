# CDScriptManager
![CDScript](https://github.com/St1ngLeR/CDScriptManager/blob/master/CDScript_Logo1.png?raw=true)
## Introduction
> [!WARNING]
> To run the application, you need to install [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

**CDScriptManager (*later CDSM*)** is a tool for fast and handy managing CDScript files for Crashday. CDSM gives you the opportunity to configure and apply special script files to a game, which open up the possibilities of low-level modding.

## Program overview
### Main
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/d80fa094-8417-4874-88ad-1a4675a70413)

The CDSM is a small window that consists of various main elements:
1. "Game" tab. Contains options related to the game itself (check for 8-10 items)
2. "Presets" tab. Contains options related to the script presets (check for 11-13 items)
3. "About" tab. Contains miscellaneous ~~not really importent~~ stuff (check for 8-10 items)
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

# Files overview
## Contents
Out of the box, the program consists of one file - the executive file itself. But after the first launch, the program creates some files, which are described below:
### `cdscript_log.txt`
The logging file that captures each action of the program. This file can be useful for script writing. After each launch, the file is written over again.
### `cdsmanager_settings.ini`
The file that contains basic settings for proper work of program.
### `scripts` folder
A folder for CDScript files that CDSM uses to managing them. Initially, the folder does not contain any scripts.
### `presets` folder
A folder inside the `scripts` folder for script preset files. Initially, the folder contains a default preset.
## Structure
> [!WARNING]
> The following files are updated by the program itself, so manual modification is not recommended unless as a last resort.

The files created and used by CDSM have their own structure, which is described below:
### `cdsmanager_settings.ini`
- `skipmanager` - allow to skip manager on it's launch for quick run game. (default value is `0`)
- `exec` - game executable. (default value is `Crashday.exe`)
- `currentpreset` - current selected script preset. (default value is `default`)
### `Preset file (scripts/presets/*.ini)`
- `[(script_name).cdscript]` section contains all values related to it's script.
- `~state` is the main key that is rensposible for disabling/enabling script to load. (`false` - disable, `true` - enable)
The script section can also use other keys if the script has settings that the user can configure in the manager.
# Writing a CDScript file
> [!CAUTION]
> This section is dedicated to creating script files and is intended exclusively for experienced users. Any manipulation of scripts can lead to malfunction of the game. Do this at your own risk.

As described earlier, the main task of CDSM is to download CDScript files for the Crashday game. CDScript files are unencrypted human-readable files with the extension `.cdscript`. Script files can be opened and edited in any text editor (for example, [Notepad++](https://github.com/notepad-plus-plus/notepad-plus-plus)).
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/2efcb020-cacd-49eb-a862-560694a6192f)
*Sample code "Hello World!" in the form of a CDScript. The script outputs the line "Hello World!" to the `cdscript_log.txt` logging file.*

Above you can see an example of a simple script. Based on it, the explanations for writing the script are given below.
The CDScript language is very limited (for now yet) and similar to the C# language, although it has some differences. 
## Comments
All comments are marked with a "#" character (for example, `# this is my useful comment!`). However, keep in mind that only single-line comments are supported.
## Main variables
In order for information about scripts to be conveniently displayed in managers, the CDScript language introduces the main variables that are read by the manager when selecting a script from the list of scripts. There are only 6 main changes and the beginning of their names are marked with a "~" character: `~cdscript_name`, `~cdscript_version`, `~cdscript_description`, `~cdscript_author`, `~cdscript_website`, `~cdscript_email`.

These variables do not need to be represented, because their purpose is indicated directly in their name. In any case, despite the fact that these variables are the main ones, they are not necessary for the script to work. They are needed exclusively for display in CDSM.
## Script body
To create a script body, like creating a function in C#, you need to set a header `script (your_script_body_name)` and limit the body with curly brackets. Tabulation is not important here, but it is recommended to use it for a beautiful look.
## 3rd-party plugins
The application is using [Costura.Fody](https://github.com/Fody/Costura/) to compile all resources into a single executable file.
