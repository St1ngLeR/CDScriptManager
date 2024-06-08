# CDScriptManager
![CDScript](https://github.com/St1ngLeR/CDScriptManager/blob/master/CDScript_Logo1.png?raw=true)
## Introduction
> [!WARNING]
> To run the application, you need to install [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).
> 
**CDScriptManager (*later CDSM*)** is a tool for fast and handy managing CDScript files for Crashday. CDSM gives you the opportunity to configure and apply special script files to a game, which open up the possibilities of low-level modding.

## Program Overview
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
10. "Create shortcut" option. Create shortcut with preinstalled game executable and script preset.
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
