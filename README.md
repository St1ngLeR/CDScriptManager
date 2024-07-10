# CDScriptManager
![CDScript](https://github.com/St1ngLeR/CDScriptManager/blob/master/CDScript_Logo1.png?raw=true)
## Introduction
> [!IMPORTANT]
> To run the application, you need to install [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

**CDScriptManager (*later CDSM*)** is a tool for fast and handy managing CDScript files for classic Crashday (1.0-1.2). CDSM gives you the opportunity to configure and apply special script files to a game, which open up the possibilities of low-level modding.

Current version - *0.1.1*

**Click [here](CHANGELOGS.md) to see all changelogs**

## Program overview
### Main
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/d80fa094-8417-4874-88ad-1a4675a70413)

CDSM is a small window that consists of various main elements:
1. "Game" tab. Contains options related to the game itself. (see ["Game" tab](#game-tab))
2. "Presets" tab. Contains options related to the script presets. (see ["Presets" tab](#presets-tab))
3. "About" tab. Contains miscellaneous ~~not really importent~~ stuff. (see ["About" tab](#about-tab))
4. Script list. The list shows all scripts, located in `scripts` folder in the game directory.
5. Script information. The area for displaying the description of the selected script.
6. "Refresh" button. The button to refrest script list.
7. "Configure" button. The button to configure script settings in separated window. The button is enabled only if script has settings to configure. (see ["Configuration" window](#configuration-window))
### "Game" tab
![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/c0556bfb-8b54-45b2-a435-1dfbd5dd9aa9)

8. "Run game" option. Run the game executable (`.exe` or `.dll`).
9. "Set game executable" option. Change the game executable (`.exe` or `.dll`).
10. "Create shortcut" option. Create shortcut with preinstalled game executable and script preset via command-line arguments for quick game run. (see [Command-line arguments](#command-line-arguments))
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
- `/preset=(preset_name)` - loads specified preset from `scripts/presets` folder.
- `/exec=(executable_file_name)` - runs specified executable file (`.exe` or `.dll`).
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

## 3rd-party plugins
The application is using [Costura.Fody](https://github.com/Fody/Costura/) to compile all resources into a single executable file.
