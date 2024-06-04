using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CDScriptManager
{
    public partial class Form1 : Form
    {

        string mainvar1 = "~cdscript_name";
        string mainvar2 = "~cdscript_version";
        string mainvar3 = "~cdscript_description";
        string mainvar4 = "~cdscript_author";
        string mainvar5 = "~cdscript_website";
        string mainvar6 = "~cdscript_email";
        string mainvar1value;   // name
        string mainvar2value;   // version
        string mainvar3value;   // description
        string mainvar4value;   // author
        string mainvar5value;   // website
        string mainvar6value;   // email
        string scriptbody = "script ";
        string scriptsetting = "setting ";
        string scriptbodyname;
        string scriptsettingname;
        string currentpresetname;
        string settingvar;
        string settingtext;
        string settingtype;
        string settingdefaultvalue;
        string code_print = "Console.Print";
        string code_string_create = "Setting.Create";
        string code_exec_rep_b = "Exec.Replace.Bytes";
        string code_exec_rep_i8 = "Exec.Replace.Int8";
        string code_exec_rep_i16 = "Exec.Replace.Int16";
        string code_exec_rep_i32 = "Exec.Replace.Int32";
        string code_exec_rep_i64 = "Exec.Replace.Int64";
        string code_exec_rep_f = "Exec.Replace.Float";
        string code_exec_rep_s = "Exec.Replace.String";
        string code_exec_startpoint = "Exec.StartPoint";
        string tempfile;
        StringBuilder contentBuilder = new StringBuilder();

        int mainvarcount;
        int linescount;
        int linescount2;
        int previousIndex = -1;
        int byteindex;

        object result_compute;
        object setting_key;

        byte[] byteArray_startpoint;
        byte[] byteArray_insert;

        string setting_var;

        string logfilepath = Directory.GetCurrentDirectory() + "\\cdscript_log.txt";
        string presetspath = Directory.GetCurrentDirectory() + "\\scripts\\presets\\";
        DirectoryInfo scriptsfolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\scripts");
        Dictionary<string, string> settings = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void informationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 aboutWindow = new Form2();
            aboutWindow.ShowDialog();
        }

        private void gitHubPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/St1ngLeR",
                UseShellExecute = true
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");

            currentpresetname = SettingsFile.Read("currentpreset", "CDScriptManager");
            if (currentpresetname.Length != 0)
            {
                CheckFormTitle();
            }

            if (!SettingsFile.KeyExists("skipmanager", "CDScriptManager"))
            {
                SettingsFile.Write("skipmanager", "0", "CDScriptManager");
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Skipping manager is disabled\n");
                }
            }

            using (var logfile = new StreamWriter(logfilepath, false))
            {
                logfile.Write(string.Empty);
            }
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("CDScript - Log file\n");
                logfile.Write("----------------------------------------\n");
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"Launching the \"{Path.GetFileName(Application.ExecutablePath)}\" application\n");
            }

            if (!SettingsFile.KeyExists("exec", "CDScriptManager"))
            {
                SettingsFile.Write("exec", "Crashday.exe", "CDScriptManager");
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Default executable file is \"Crashday.exe\"\n");
                }
            }
            else
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"The executable file is \"{SettingsFile.Read("exec", "CDScriptManager")}\"\n");
                }
            }
            if (!SettingsFile.KeyExists("currentpreset", "CDScriptManager"))
            {
                SettingsFile.Write("currentpreset", "default", "CDScriptManager");
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Created current preset indication file\n");

                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Current preset is \"defalut\"\n");
                }
                currentpresetname = "default";
            }
            else
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Current preset indication file is found\n");

                    currentpresetname = SettingsFile.Read("currentpreset");

                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Current preset is \"{currentpresetname}\"\n");
                }
            }

            using (var logfile = new StreamWriter(logfilepath, true))
            {
                if (!Directory.Exists(presetspath))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Presets folder created\n");
                    Directory.CreateDirectory(presetspath);
                }
                else
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Presets folder found\n");
                }
                if (!File.Exists(presetspath + "default.ini"))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Default preset file (default.ini) created\n");
                    IniFile PresetFile = new IniFile(presetspath + "default.ini");
                }
                else
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Default preset file (default.ini) found\n");

                }
                if (Directory.GetFiles(presetspath, "*.ini").Length == 0)
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("No preset files found\n");
                }
                else if (Directory.GetFiles(presetspath, "*.ini").Length == 1)
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("1 preset file found\n");
                }
                else
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"{Directory.GetFiles(presetspath, "*.ini").Length} presets file found\n");
                }
            }

            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("----------------------------------------\n");
            }
            CheckScriptFiles();

            if (SettingsFile.Read("skipmanager", "CDScriptManager") == "1")
            {
                RunGame(SettingsFile.Read("exec", "CDScriptManager"));
                this.Close();
            }
        }

        public void RunGame(string filePath)
        {
            tempfile = Path.GetTempFileName();
            File.Copy(Directory.GetCurrentDirectory() + "\\" + filePath, tempfile, true);
            File.SetAttributes(tempfile, FileAttributes.Normal);
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                {
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar1, mainvar1value, true);
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar2, mainvar2value, true);
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar3, mainvar3value, true);
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar4, mainvar4value, true);
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar5, mainvar5value, true);
                    CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar6, mainvar6value, true);
                    ScriptChecking(checkedListBox1.Items[i].ToString());
                }
            }
            Process.Start(Directory.GetCurrentDirectory() + "\\" + filePath).WaitForExit();
            File.Copy(tempfile, Directory.GetCurrentDirectory() + "\\" + filePath, overwrite: true);
            File.Delete(tempfile);
        }

        private void CheckScriptFiles()
        {
            FileInfo[] Files = scriptsfolder.GetFiles("*.cdscript");

            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"Scanning the \"scripts\" folder\n");
            }

            foreach (FileInfo file in Files)
            {
                IniFile PresetFile = new IniFile(presetspath + currentpresetname + ".ini");
                if (!PresetFile.KeyExists("~state", file.Name))
                {
                    PresetFile.Write("~state", "false", file.Name);
                }
                checkedListBox1.Items.Add(file.Name);
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"The script file \"{file.Name}\" is found\n");
                }
            }
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                if (checkedListBox1.Items.Count == 0)
                {
                    logfile.Write("No scripts found\n");
                }
                else if (checkedListBox1.Items.Count == 1)
                {
                    logfile.Write("Total 1 script found\n");
                }
                else
                {
                    logfile.Write($"Total {checkedListBox1.Items.Count} scripts found\n");
                }
            }
            CheckPresetFile();
        }
        private void CheckPresetFile()
        {
            IniFile PresetFile = new IniFile(presetspath + currentpresetname + ".ini");

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Reading script file \"{checkedListBox1.Items[i]}\" values\n");
                }

                if (PresetFile.Read("~state", checkedListBox1.Items[i].ToString()) == "true")
                {
                    checkedListBox1.SetItemChecked(i, true);

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Script file \"{checkedListBox1.Items[i]}\" state is \"true\"\n");
                    }
                }
                else
                {
                    checkedListBox1.SetItemChecked(i, false);

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Script file \"{checkedListBox1.Items[i]}\" state is \"false\"\n");
                    }
                }

                //SettingsChecking(checkedListBox1.Items[i].ToString());

            }
        }

        private void CheckFormTitle()
        {
            if (currentpresetname == "default")
            {
                this.Text = "CDScriptManager";
            }
            else
            {
                this.Text = $"CDScriptManager - {currentpresetname}.ini";
            }
        }

        private void ScriptChecking(string filename)
        {
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                if (mainvarcount == 0)
                {
                    logfile.Write("No main variables found\n");
                }
                else if (mainvarcount == 1)
                {
                    logfile.Write("Total 1 main variable found\n");
                }
                else
                {
                    logfile.Write($"Total {mainvarcount} main variables found\n");
                }
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write("Main variables checking finished\n");
            }
            mainvarcount = 0;

            // Проверка "тела" скрипта CDScript
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Находим индекс, где начинается нужная подстрока
                    int startIndex = line.IndexOf(scriptbody);
                    if (startIndex != -1 && line.StartsWith(scriptbody))
                    {
                        // Если строка содержит шаблон, то выводим содержимое после него
                        scriptbodyname = line.Substring(startIndex + scriptbody.Length);
                        break; // Прерываем цикл после нахождения первого совпадения
                    }
                }
                try
                {
                    if (scriptbodyname.Length > 0)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Body \"{scriptbodyname}\" script is found\n");

                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write("Checking the body script\n");
                        }
                    }

                    else
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [ERROR] ");
                            logfile.Write($"Body script name can't be empty\n");

                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write("Script checking stopped");
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [ERROR] ");
                        logfile.Write("Script body is not found\n");

                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write("Script checking stopped");
                    }
                    scriptbodyname = null;
                }
            }

            bool readContent = false;
            string previousLine = null;

            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Trim() == "{")
                    {
                        readContent = true; // Начинаем чтение после открывающей скобки
                        continue;
                    }
                    else if (line.Trim() == "}")
                    {
                        readContent = false; // Заканчиваем чтение перед закрывающей скобкой
                        break;
                    }
                    if (readContent)
                    {
                        if (previousLine != null)
                        {
                            if (previousLine.Contains('#'))
                            {
                                contentBuilder.Append(previousLine.Substring(0, previousLine.IndexOf('#')));
                            }
                            else
                            {
                                contentBuilder.Append(previousLine);
                            }
                            contentBuilder.Append("\r\n"); // Добавляем перенос строки
                            linescount++;
                        }
                        previousLine = line.TrimStart('\t'); // Удаляем начальную табуляцию и сохраняем строку

                    }
                }
                if (previousLine != null)
                {
                    if (previousLine.Contains('#'))
                    {
                        contentBuilder.Append(previousLine.Substring(0, previousLine.IndexOf('#')));
                    }
                    else
                    {
                        contentBuilder.Append(previousLine);
                    }
                }
                linescount++;
            }
            // TODO: Код ниже нужно сделать функцией
            // "Обсасывание" каждой строчки
            string[] lines = contentBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < linescount; i++)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Checking line {i + 1}\n");
                }
                //MessageBox.Show(lines[i]); // Проверка на правильный вывод строк
                string line = lines[i];
                int patternIndex;
                if (line.Contains(code_print))
                {
                    patternIndex = line.IndexOf(code_print);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_print.Length);
                        string result = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "");
                        //MessageBox.Show(result); // Показ содержимого до переноса строки "\n"
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [CONSOLE] ");
                            logfile.Write($"Line {i + 1}: \"{result}\"\n");
                        }
                    }
                }
                else if (line.Contains(code_exec_startpoint))
                {
                    patternIndex = line.IndexOf(code_exec_startpoint);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_startpoint.Length);
                        string result = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        //MessageBox.Show(result); // Показ содержимого до переноса строки "\n"
                        var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                        byteArray_startpoint = ConvertHexStringToByteArray(result);
                        byte[] fileBytes = File.ReadAllBytes(SettingsFile.Read("exec", "CDScriptManager"));
                        byteindex = FindSequenceIndex(fileBytes, byteArray_startpoint);

                        if (byteindex != -1)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write($"Found mentioned byte sequence at index {byteindex}\n");
                            }
                            byteindex += byteArray_startpoint.Length;
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write($"The starting point is shifted forward {byteArray_startpoint.Length} bytes\n");
                            }
                        }
                        else
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [WARNING] ");
                                logfile.Write("Mentioned byte sequence is not found\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_string_create))
                {
                    var PresetFile = new IniFile(presetspath + currentpresetname + ".ini");

                    if (line.StartsWith(code_string_create))
                    {
                        var parts = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                        var settingParts = parts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        string key = settingParts[0].Trim();
                        if (PresetFile.KeyExists(key, Path.GetFileName(filename)))
                        {
                            settings[key] = PresetFile.Read(key, Path.GetFileName(filename));
                        }
                        else
                        {
                            PresetFile.Write(key, settingParts[3].Trim(), Path.GetFileName(filename));
                            settings[key] = settingParts[3].Trim();
                        }
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Found variable \"{key}\"\n");
                        }
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"The value is \"{settings[key]}\"\n");
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_b))
                {
                    patternIndex = line.IndexOf(code_exec_rep_b);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_b.Length);
                        string result = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        //MessageBox.Show(result); // Показ содержимого до переноса строки "\n"
                        var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                        byteArray_insert = ConvertHexStringToByteArray(result);
                        try
                        {
                            ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                if (byteArray_insert.Length == 0)
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write($"No bytes to replace found\n");
                                }
                                else if (byteArray_insert.Length == 1)
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"1 byte replaced in index {byteindex}\n");
                                }
                                else
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                    byteindex += byteArray_insert.Length;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_f))
                {
                    patternIndex = line.IndexOf(code_exec_rep_f);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_f.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        try
                        {
                            float result = float.Parse(result_text, CultureInfo.InvariantCulture);
                            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);

                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (byteArray_insert.Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (byteArray_insert.Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                        byteindex += byteArray_insert.Length;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            patternIndex = line.IndexOf(code_exec_rep_f);
                            float computeResult = 0f;
                            // Проверяем, содержит ли строка искомый паттерн
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // Отображаем оставшееся содержимое строки после паттерна
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_f.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = Convert.ToSingle(dataTable.Compute(expression, ""));
                                    var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                                    //MessageBox.Show(byteindex.ToString());
                                    ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, BitConverter.GetBytes(computeResult));
                                    byteindex += BitConverter.GetBytes(computeResult).Length;
                                }
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (BitConverter.GetBytes(computeResult).Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (BitConverter.GetBytes(computeResult).Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{BitConverter.GetBytes(computeResult).Length} bytes replaced in index {byteindex}\n");
                                    }
                                }
                            }
                            catch
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_i8))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i8);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i8.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        //MessageBox.Show(result); // Показ содержимого до переноса строки "\n"
                        var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                        int result = SByte.Parse(result_text);
                        byteArray_insert = BitConverter.GetBytes(result);
                        Array.Resize(ref byteArray_insert, byteArray_insert.Length - 3);
                        try
                        {
                            ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                if (byteArray_insert.Length == 0)
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write($"No bytes to replace found\n");
                                }
                                else if (byteArray_insert.Length == 1)
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"1 byte replaced in index {byteindex}\n");
                                }
                                else
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                    byteindex += byteArray_insert.Length;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_i16))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i16);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i16.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        try
                        {
                            int result = Int16.Parse(result_text, CultureInfo.InvariantCulture);
                            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);

                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (byteArray_insert.Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (byteArray_insert.Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                        byteindex += byteArray_insert.Length;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_i32))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i32);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i32.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        try
                        {
                            int result = Int32.Parse(result_text, CultureInfo.InvariantCulture);
                            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);

                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (byteArray_insert.Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (byteArray_insert.Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                        byteindex += byteArray_insert.Length;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_i64))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i64);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i64.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        try
                        {
                            long result = Int64.Parse(result_text, CultureInfo.InvariantCulture);
                            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);

                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (byteArray_insert.Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (byteArray_insert.Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                        byteindex += byteArray_insert.Length;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_s))
                {
                    patternIndex = line.IndexOf(code_exec_rep_s);

                    // Проверяем, содержит ли строка искомый паттерн
                    if (patternIndex != -1)
                    {
                        // Отображаем оставшееся содержимое строки после паттерна
                        string contentAfterPattern = line.Substring(patternIndex + code_exec_rep_s.Length);
                        string result_text = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "")
                            .Replace(" = ", "")
                            .Replace(" =", "")
                            .Replace("= ", "")
                            .Replace("=", "");
                        try
                        {
                            string result = result_text;
                            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                            byteArray_insert = Encoding.ASCII.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(SettingsFile.Read("exec", "CDScriptManager"), byteindex, byteArray_insert);

                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    if (byteArray_insert.Length == 0)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [ERROR] ");
                                        logfile.Write($"No bytes to replace found\n");
                                    }
                                    else if (byteArray_insert.Length == 1)
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"1 byte replaced in index {byteindex}\n");
                                    }
                                    else
                                    {
                                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                                        logfile.Write(" [INFO] ");
                                        logfile.Write($"{byteArray_insert.Length} bytes replaced in index {byteindex}\n");
                                        byteindex += byteArray_insert.Length;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [ERROR] ");
                                    logfile.Write("Could not find the starting point to replace bytes\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write("Could not find the starting point to replace bytes\n");
                            }
                        }
                    }
                }
            }
            //MessageBox.Show(contentBuilder.ToString()); // Очередная проверка
            contentBuilder = new StringBuilder();
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"All lines are checked\n");
            }
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                if (linescount == 0)
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [ERROR] ");
                    logfile.Write($"No lines found\n");

                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Script checking stopped");
                }
                else if (linescount == 1)
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Total 1 line found\n");
                }
                else
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Total {linescount} lines found\n");
                }
            }
            linescount = 0;
        }

        private void ReplaceBytesInFile(string filePath, long index, byte[] newBytes)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            // Открываем файл на чтение и запись
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                // Перемещаем указатель в файле на заданный индекс
                fs.Seek(index, SeekOrigin.Begin);

                // Записываем новые байты в файл, начиная с указанного индекса
                fs.Write(newBytes, 0, newBytes.Length);
            }

        }

        public void SettingsChecking(string filename)
        {
            IniFile PresetFile = new IniFile(presetspath + currentpresetname + ".ini");
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    // Находим индекс, где начинается нужная подстрока
                    int startIndex = line.IndexOf(scriptsetting);
                    if (startIndex != -1 && line.Trim('\t').StartsWith(scriptsetting))
                    {
                        // Если строка содержит шаблон, то выводим содержимое после него
                        scriptsettingname = line.Trim('\t').Substring(scriptsetting.Length);
                        break; // Прерываем цикл после нахождения первого совпадения
                    }
                }

            }
            bool readContent = false;
            string previousLine = null;

            if (scriptsettingname != null)
            {
                using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim() == "{")
                        {
                            readContent = true; // Начинаем чтение после открывающей скобки
                            continue;
                        }
                        else if (line.Trim() == "}")
                        {
                            readContent = false; // Заканчиваем чтение перед закрывающей скобкой
                            break;
                        }
                        if (readContent)
                        {
                            if (previousLine != null)
                            {
                                if (previousLine.Contains('#'))
                                {
                                    contentBuilder.Append(previousLine.Substring(0, previousLine.IndexOf('#')));
                                }
                                else
                                {
                                    contentBuilder.Append(previousLine);
                                }
                                contentBuilder.Append("\r\n"); // Добавляем перенос строки
                                linescount2++;
                            }
                            previousLine = line.TrimStart('\t'); // Удаляем начальную табуляцию и сохраняем строку
                        }
                    }
                    if (previousLine != null)
                    {
                        if (previousLine.Contains('#'))
                        {
                            contentBuilder.Append(previousLine.Substring(0, previousLine.IndexOf('#')));
                        }
                        else
                        {
                            contentBuilder.Append(previousLine);
                        }
                    }
                }
                string[] lines = contentBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                for (int j = 1; j < linescount2; j++)
                {
                    string line = lines[j];
                    int patternIndex;
                    if (line.Contains(code_string_create))
                    {
                        patternIndex = line.IndexOf(code_string_create);

                        // Проверяем, содержит ли строка искомый паттерн
                        if (patternIndex != -1)
                        {
                            // Отображаем оставшееся содержимое строки после паттерна
                            string contentAfterPattern = line.Substring(patternIndex + code_string_create.Length);
                            string result = contentAfterPattern.Split('\n')[0]
                                .Replace("\t", "")
                                .Replace(" = ", "")
                                .Replace(" =", "")
                                .Replace("= ", "")
                                .Replace("=", "");
                            //MessageBox.Show(result); // Показ содержимого до переноса строки "\n"
                            string[] settingparams = result.Split(", ");
                            settingvar = settingparams[0];
                            settingtext = settingparams[1];
                            settingtype = settingparams[2];
                            settingdefaultvalue = settingparams[3];

                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write($"Value \"{settingvar}\" for script file \"{filename}\" is \"{PresetFile.Read(settingvar, filename)}\"\n");
                            }
                        }
                    }
                }
                linescount2 = 0;
            }
        }

        private static int FindSequenceIndex(byte[] fileBytes, byte[] sequenceToFind)
        {
            for (int i = 0; i < fileBytes.Length - sequenceToFind.Length + 1; i++)
            {
                if (fileBytes.Skip(i).Take(sequenceToFind.Length).SequenceEqual(sequenceToFind))
                {
                    return i;
                }
            }
            return -1;  // Возвращает -1, если последовательность не найдена
        }

        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                string[] hexPairs = hexString.Split(' ');
                byte[] bytes = new byte[hexPairs.Length];
                try
                {
                    for (int i = 0; i < hexPairs.Length; i++)
                    {
                        bytes[i] = Convert.ToByte(hexPairs[i], 16);
                    }
                }
                catch (Exception ex)
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Calculating bytes\n");
                }
                return bytes;
            }
        }

        private string CheckMainVar(string filename, string mainvar, string mainvarvalue, bool islogging)
        {
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Находим индекс, где начинается нужная подстрока
                    int startIndex = line.IndexOf(mainvar);
                    if (startIndex != -1 && line.StartsWith(mainvar))
                    {
                        // Если строка содержит шаблон, то выводим содержимое после него
                        mainvarvalue = line.Substring(startIndex + mainvar.Length);
                        break; // Прерываем цикл после нахождения первого совпадения
                    }
                }
                try
                {
                    if (mainvarvalue != null && mainvarvalue.Contains(" = "))
                    {
                        mainvarvalue = mainvarvalue.Replace(" = ", "");
                    }
                    else if (mainvarvalue != null && mainvarvalue.Contains(" ="))
                    {
                        mainvarvalue = mainvarvalue.Replace(" =", "");
                    }
                    else if (mainvarvalue != null && mainvarvalue.Contains("= "))
                    {
                        mainvarvalue = mainvarvalue.Replace("= ", "");
                    }
                    else if (mainvarvalue != null && mainvarvalue.Contains("="))
                    {
                        mainvarvalue = mainvarvalue.Replace("=", "");
                    }

                    if (mainvarvalue != null && mainvarvalue.Contains("#"))
                    {
                        mainvarvalue = mainvarvalue.Substring(0, mainvarvalue.LastIndexOf("#"));
                    }

                    if (mainvarvalue != null && mainvarvalue.Contains("\t"))
                    {
                        mainvarvalue = mainvarvalue.Replace("\t", "");
                    }

                    if (islogging == true)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Found main CDScript variable \"{mainvar}\", value is \"{mainvarvalue}\"\n");
                            mainvarcount++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (islogging == true)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [WARNING] ");
                            logfile.Write($"Main CDScript variable \"{mainvar}\" is not found, value is unknown\n");
                        }
                    }
                }
                return mainvarvalue;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Проверка "главных" переменных CDScript
            if (checkedListBox1.SelectedIndex != previousIndex)
            {
                label2.Text = "";
                previousIndex = checkedListBox1.SelectedIndex;

                if (checkedListBox1.SelectedItem != null)
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("----------------------------------------\n");
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Checking the \"{checkedListBox1.SelectedItem}\" script\n");
                    }

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write("Checking the main variables\n");
                    }

                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar1, mainvar1value, false) != null)
                    {
                        label2.Text += $"Name: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar1, mainvar1value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "Name: (unknown)\n\n";
                    }
                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar2, mainvar2value, false) != null)
                    {
                        label2.Text += $"Version: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar2, mainvar2value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "Version: (unknown)\n\n";
                    }
                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar3, mainvar3value, false) != null)
                    {
                        label2.Text += $"Description: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar3, mainvar3value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "Description: (unknown)\n\n";
                    }
                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar4, mainvar4value, false) != null)
                    {
                        label2.Text += $"Author: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar4, mainvar4value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "Author: (unknown)\n\n";
                    }
                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar5, mainvar5value, false) != null)
                    {
                        label2.Text += $"Website: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar5, mainvar5value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "Website: (unknown)\n\n";
                    }
                    if (CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar6, mainvar6value, false) != null)
                    {
                        label2.Text += $"E-Mail: {CheckMainVar(checkedListBox1.SelectedItem.ToString(), mainvar6, mainvar6value, true)}\n\n";
                    }
                    else
                    {
                        label2.Text += "E-Mail: (unknown)\n\n";
                    }
                    ToggleConfigButton(checkedListBox1.SelectedItem.ToString());
                    //SettingsChecking(checkedListBox1.SelectedItem.ToString());
                    //ScriptChecking(checkedListBox1.SelectedItem.ToString());
                    /*using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Checking script file \"{checkedListBox1.SelectedItem}\" finished\n");
                    }*/
                }
            }
        }

        public void ToggleConfigButton(string filename)
        {
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    // Находим индекс, где начинается нужная подстрока
                    int startIndex = line.IndexOf(scriptsetting);
                    if (startIndex != -1 && line.Trim('\t').StartsWith(scriptsetting))
                    {
                        // Если строка содержит шаблон, то выводим содержимое после него
                        scriptsettingname = line.Trim('\t').Substring(scriptsetting.Length);
                        break; // Прерываем цикл после нахождения первого совпадения
                    }
                }
                if ((line != null) && (line.Contains(scriptsetting)))
                {
                    try
                    {
                        if (scriptsettingname.Length > 0)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write($"Script setting \"{scriptsettingname}\" is found\n");

                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write("Checking the script setting\n");
                            }
                            button1.Enabled = true;
                        }
                        else
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [ERROR] ");
                                logfile.Write($"Script setting name can't be empty\n");

                                logfile.Write("[" + DateTime.Now.ToString() + "]");
                                logfile.Write(" [INFO] ");
                                logfile.Write("Script checking stopped");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write("Script setting is not found\n");
                        }
                    }
                }
                else
                {
                    button1.Enabled = false;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("----------------------------------------\n");
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write("Refreshing script list\n");
            }
            CheckScriptFiles();
            previousIndex = -1;
            label2.Text = "";
        }

        public string GetSelectedListItem()
        {
            // Возвращает выбранный элемент или null, если ничего не выбрано
            return checkedListBox1.SelectedItem as string;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            IniFile PresetFile = new IniFile(presetspath + currentpresetname + ".ini");

            if (checkedListBox1.SelectedIndex != -1)
            {
                if (checkedListBox1.GetItemCheckState(checkedListBox1.SelectedIndex) == CheckState.Checked)
                {
                    PresetFile.Write("~state", "false", checkedListBox1.SelectedItem.ToString());

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Script file \"{checkedListBox1.SelectedItem}\" state is \"false\"\n");
                    }
                }
                else
                {
                    PresetFile.Write("~state", "true", checkedListBox1.SelectedItem.ToString());

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Script file \"{checkedListBox1.SelectedItem}\" state is \"true\"\n");
                    }
                }
            }
        }

        private void newPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = presetspath;
            sfd.FileName = "";
            sfd.DefaultExt = ".ini";
            sfd.Filter = "Scripts preset (*.ini)|*.ini";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = File.CreateText(sfd.FileName);

                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Preset file \"{Path.GetFileName(sfd.FileName)}\" created\n");

                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Preset file \"{Path.GetFileName(sfd.FileName)}\" is current preset\n");
                }
                currentpresetname = Path.GetFileNameWithoutExtension(sfd.FileName);
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
                var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                SettingsFile.Write("currentpreset", Path.GetFileNameWithoutExtension(sfd.FileName), "CDScriptManager");
                CheckFormTitle();
            }
        }

        private void openPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = presetspath;
            ofd.FileName = "";
            ofd.DefaultExt = ".ini";
            ofd.Filter = "Scripts preset (*.ini)|*.ini";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Preset file \"{Path.GetFileName(ofd.FileName)}\" loaded\n");

                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Preset file \"{Path.GetFileName(ofd.FileName)}\" is current preset\n");
                }
                currentpresetname = Path.GetFileNameWithoutExtension(ofd.FileName);

                var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                SettingsFile.Write("currentpreset", Path.GetFileNameWithoutExtension(ofd.FileName), "CDScriptManager");
                CheckFormTitle();
                CheckPresetFile();
                label2.Text = "";
                checkedListBox1.SelectedIndex = -1;
            }
        }

        private void savePresetAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = presetspath;
            sfd.FileName = "";
            sfd.DefaultExt = ".ini";
            sfd.Filter = "Scripts preset (*.ini)|*.ini";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Preset file \"{Path.GetFileName(sfd.FileName)}\" created\n");
                }
                var PresetFile = new IniFile(sfd.FileName);

                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                    {
                        PresetFile.Write("~state", "true", checkedListBox1.Items[i].ToString());

                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Script file \"{checkedListBox1.Items[i]}\" state is \"true\"\n");
                        }
                    }
                    else
                    {
                        PresetFile.Write("~state", "false", checkedListBox1.Items[i].ToString());

                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Script file \"{checkedListBox1.Items[i]}\" state is \"false\"\n");
                        }
                    }
                }
            }
        }

        private void setGameExecutableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = Directory.GetCurrentDirectory();
            ofd.FileName = "";
            ofd.DefaultExt = ".exe";
            ofd.Filter = "Executable file (*.exe)|*.exe|Dynamic-link library (*.dll)|*.dll";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                SettingsFile.Write("exec", Path.GetFileName(ofd.FileName), "CDScriptManager");
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"New executable file is \"{Path.GetFileName(ofd.FileName)}\"\n");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(GetSelectedListItem());
            form3.ShowDialog();
        }

        private void runGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            RunGame(SettingsFile.Read("exec", "CDScriptManager"));
        }
    }
}