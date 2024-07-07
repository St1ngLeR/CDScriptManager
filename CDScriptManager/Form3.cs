using System.Text;

namespace CDScriptManager
{
    public partial class Form3 : Form
    {
        int linescount;
        string currentscript;
        string code_string_create = "Setting.Create";
        string scriptsetting = "setting ";
        string scriptsettingname;
        string settingvar;
        string settingtext;
        string settingtype;
        string settingdefaultvalue;
        string logfilepath = Directory.GetCurrentDirectory() + "\\cdscript_log.txt";
        DirectoryInfo scriptsfolder = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\scripts");
        StringBuilder contentBuilder = new StringBuilder();

        public Form3(string selectedItem)
        {
            InitializeComponent();
            currentscript = selectedItem;
            Main(currentscript);
        }

        public void Main(string currentscript)
        {
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            var PresetFile = new IniFile(Directory.GetCurrentDirectory() + "\\scripts\\presets\\" + SettingsFile.Read("currentpreset", "CDScriptManager") + ".ini");

            SettingsChecking(currentscript);

            bool readContent = false;
            string previousLine = null;
            linescount = 0;
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + currentscript))
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
            }
            string[] lines = contentBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            int j = 1;
            for (int i = 1; i < linescount+1; i++)
            {
                string line = lines[i];
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
                            logfile.Write($"Found {i} setting \"{settingvar}\"\n");
                        }
                        Label label = new Label();
                        label.Text = settingtext;
                        label.AutoSize = true;
                        label.Location = new Point(3, (j * 32) - 24);
                        panel1.Controls.Add(label);
                        if (settingtype == "numericUpDown")
                        {
                            NumericUpDown nud = new NumericUpDown();
                            nud.Minimum = 0;
                            nud.Maximum = 2147483647;
                            if (PresetFile.KeyExists(settingvar, currentscript))
                            {
                                nud.Value = Int32.Parse(PresetFile.Read(settingvar, currentscript));
                            }
                            else
                            {
                                PresetFile.Write(settingvar, settingdefaultvalue, currentscript);
                                nud.Value = Int32.Parse(settingdefaultvalue);
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"Default setting value is \"{settingdefaultvalue}\"\n");
                                }
                            }
                            nud.Width = 256;
                            nud.Height = 24;
                            nud.Location = new Point(174, (j * 32) - 26);
                            panel1.Controls.Add(nud);
                            nud.ValueChanged += NumericUpDown_ValueChanged;
                            nud.Tag = settingvar;
                        }
                        else if (settingtype == "textBox")
                        {
                            TextBox tb = new TextBox();
                            if (PresetFile.KeyExists(settingvar, currentscript))
                            {
                                tb.Text = PresetFile.Read(settingvar, currentscript);
                            }
                            else
                            {
                                PresetFile.Write(settingvar, settingdefaultvalue, currentscript);
                                tb.Text = settingdefaultvalue;
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"Default setting value is \"{settingdefaultvalue}\"\n");
                                }
                            }
                            tb.Width = 256;
                            tb.Height = 24;
                            tb.Location = new Point(174, (j * 32) - 26);
                            panel1.Controls.Add(tb);
                            tb.TextChanged += TextBox_TextChanged;
                            tb.Tag = settingvar;
                        }
                        else if (settingtype == "checkBox")
                        {
                            CheckBox cb = new CheckBox();
                            if (PresetFile.KeyExists(settingvar, currentscript))
                            {
                                if (PresetFile.Read(settingvar, currentscript) == "true")
                                {
                                    cb.Checked = true;
                                }
                                else
                                {
                                    cb.Checked = false;
                                }
                            }
                            else
                            {
                                PresetFile.Write(settingvar, settingdefaultvalue, currentscript);
                                cb.Checked = bool.Parse(settingdefaultvalue);
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"Default setting value is \"{settingdefaultvalue}\"\n");
                                }
                            }
                            cb.Width = 256;
                            cb.Height = 24;
                            cb.CheckAlign = ContentAlignment.MiddleRight;
                            cb.Location = new Point(174, (j * 32) - 26);
                            panel1.Controls.Add(cb);
                            cb.CheckedChanged += CheckBox_CheckedChanged;
                            cb.Tag = settingvar;
                        }
                        j++;
                    }
                }
            }
            j = 0;
        }

        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            var PresetFile = new IniFile(Directory.GetCurrentDirectory() + "\\scripts\\presets\\" + SettingsFile.Read("currentpreset", "CDScriptManager") + ".ini");

            NumericUpDown nud = sender as NumericUpDown;
            if (nud != null)
            {
                PresetFile.Write(nud.Tag.ToString(), nud.Value.ToString(), currentscript);
            }
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"New setting value for \"{nud.Tag}\" is \"{nud.Value}\"\n");
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            var PresetFile = new IniFile(Directory.GetCurrentDirectory() + "\\scripts\\presets\\" + SettingsFile.Read("currentpreset", "CDScriptManager") + ".ini");

            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                PresetFile.Write(tb.Tag.ToString(), tb.Text.Replace(',', '.'), currentscript);
            }
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"New setting value for \"{tb.Tag}\" is \"{tb.Text.Replace(',', '.')}\"\n");
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
            var PresetFile = new IniFile(Directory.GetCurrentDirectory() + "\\scripts\\presets\\" + SettingsFile.Read("currentpreset", "CDScriptManager") + ".ini");

            CheckBox cb = sender as CheckBox;
            PresetFile.Write(cb.Tag.ToString(), cb.Checked.ToString().ToLower(), currentscript);

            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"New setting value for \"{cb.Tag}\" is \"{cb.Checked.ToString().ToLower()}\"\n");
            }
        }

        public void SettingsChecking(string filename)
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
                try
                {
                    if (scriptsettingname.Length > 0)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write("Checking the script setting\n");
                        }
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset all the defaults?", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("The default values have been reset for all script values\n");
                }
                var SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
                var PresetFile = new IniFile(Directory.GetCurrentDirectory() + "\\scripts\\presets\\" + SettingsFile.Read("currentpreset", "CDScriptManager") + ".ini");
                string presetState = PresetFile.Read("~state", currentscript);
                PresetFile.DeleteSection(currentscript);
                PresetFile.Write("~state", presetState, currentscript);
                this.Controls.Clear();
                this.InitializeComponent();
                Main(currentscript);
            }
        }
    }
}
