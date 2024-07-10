# Writing a CDScript files
> [!CAUTION]
> This section is dedicated to creating script files and is intended exclusively for experienced users. Any manipulation of scripts can lead to malfunction of the game. Do this at your own risk.

As described earlier, the main task of CDSM is to integrate CDScript files to the Crashday game. CDScript files are unencrypted human-readable files with the extension `.cdscript`. Script files can be created, edited and opened in any text editor (for example, [Notepad++](https://github.com/notepad-plus-plus/notepad-plus-plus)).

![image](https://github.com/St1ngLeR/CDScriptManager/assets/63962772/2efcb020-cacd-49eb-a862-560694a6192f)
*Sample code "Hello World!" in the form of a CDScript. The script outputs the line "Hello World!" to the `cdscript_log.txt` logging file.*

Above you can see an example of a simple script. Based on it, the explanations for writing the script are given below.
The CDScript language is very limited (for now yet) and similar to the C# language, although it has some differences. 
## Main features
- All comments are marked with a "#" character (for example, `# this is my useful comment!`). However, keep in mind that only single-line comments are supported.
- CDScript does not use any specific character to end a line. Use a new line for other actions.
- To go to the next line, "string" values ​​use `\n`.
- For "integer" values CDScript supports simple mathematical operations (addition, subtraction, multiplication, division).
## Main variables
In order for information about scripts to be conveniently displayed in managers, the CDScript language introduces the main variables that are read by the manager when selecting a script from the list of scripts. There are only 6 main variables and the beginning of their names are marked with a "~" character: `~cdscript_name`, `~cdscript_version`, `~cdscript_description`, `~cdscript_author`, `~cdscript_website`, `~cdscript_email`. (see [Readme >> Main](README.md#main))

These variables don't need to be represented, because their purpose is indicated directly in their name. In any case, despite the fact that these variables are the main ones, they are not necessary for the script to work. They are needed exclusively for display in CDSM. If any of these main variables are not written in the script, the manager show it as "(unknown)".
## Script body
To create a script body, like creating a function in C#, you need to set a header `script (your_script_body_name)` and limit the body with curly brackets. Tabulation is not important here, but it is recommended to use it for a beautiful look. Example:
```
script MyScript
{
  do something...
}
```
## Methods
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
- `Setting.Create` (textBox, numericUpDown, checkBox) - creates configurable script setting. This method should be called with following template - `variable_name, text_in_manager, output_type, default_value`. The separator is "comma" (","). Example:
### textBox
Subtypes - `float` & `string`
```
Setting.Create = MySetting, My awesome setting:, textBox, AWESOME, string  # This line creates setting with variable "MySetting", with type "textBox" with default value "AWESOME" with subtype "string", which is named in "Configuration" window as "My awesome setting:"
```
### numericUpDown
Minimum value - `0`

Maximum value - `2147483647`
> [!TIP]
> This setting type is recommended for applying Int(8,16,32,64) values.
```
Setting.Create = MySetting, My awesome setting:, numericUpDown, 1337  # This line creates setting with variable "MySetting", with type "numericUpDown" with default value "1337", which is named in "Configuration" window as "My awesome setting:"
```
### checkBox
Accepted values - `true` or `false`
> [!IMPORTANT]
> Letter case is important. Make sure variable names are in lowercase.
```
Setting.Create = MySetting, My awesome setting:, checkBox, true  # This line creates setting with variable "MySetting", with type "textBox" with default value "true" (i.e. checkbox is checked), which is named in "Configuration" window as "My awesome setting:"
```
## Script settings
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
    Setting.Create = MySetting, My awesome setting:, textBox, AWESOME, string  # Creates setting
    Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point
    Exec.Replace.String = MySetting  # Replace content on string from "MySetting" setting
  }
}
```
## Tips
For a more convenient and practical script writing, various tips that may be useful are described below.
### Replacement in a row
To replace several values in a row after the starting point, you just need to call the method `Exec.Replace.(output_type)` as many times in a row as you need. Example:
```
Exec.StartPoint = 00 01 02 0A 0B 0C  # Sets the starting point
Exec.Replace.Float = 0.25  # Replace after starting point
Exec.Replace.Bytes = 2B 3A 0D E5  # Replace after replaced value specified above
```
### Starting points for different game versions
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
