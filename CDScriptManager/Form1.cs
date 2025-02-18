using IWshRuntimeLibrary;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using File = System.IO.File;

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
        string exec;
        string execargs;
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

        IniFile SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");

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
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/St1ngLeR",
                UseShellExecute = true
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";
            string[] args = Environment.GetCommandLineArgs();
            int arg_preset_found = 0;
            foreach (string arg in args)
            {
                if (arg.Contains("/preset="))
                {
                    currentpresetname = arg.Replace("/preset=", "").Replace("\"", "");
                    arg_preset_found = 1;
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Command line argument \"preset\" is found. The value is \"{currentpresetname}\"\n");
                    }
                }
                else if (arg_preset_found == 0)
                {
                    currentpresetname = SettingsFile.Read("currentpreset", "CDScriptManager");
                }
            }

            if (currentpresetname != "default")
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
                logfile.Write($"Running application \"{Path.GetFileName(Application.ExecutablePath)}\"\n");
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
            int arg_exec_found = 0;
            int arg_execargs_found = 0;
            foreach (string arg in args)
            {
                if (arg.Contains("/preset="))
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Command line argument \"preset\" is found. The value is \"{currentpresetname}\"\n");
                    }
                }
                if (arg.Contains("/exec="))
                {
                    exec = arg.Replace("/exec=", "").Replace("\"", "");
                    arg_exec_found = 1;

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Command line argument \"exec\" is found. The value is \"{exec}\"\n");
                    }
                }
                else
                {
                    exec = SettingsFile.Read("exec", "CDScriptManager");
                }
                if (arg.Contains("/execargs="))
                {
                    execargs = arg.Replace("/execargs=", "").Replace("\"", "");
                    arg_execargs_found = 1;

                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Command line argument \"execargs\" is found. The value is \"{execargs}\"\n");
                    }
                }
                else
                {
                    if (SettingsFile.KeyExists("execargs", "CDScriptManager"))
                    {
                        execargs = SettingsFile.Read("execargs", "CDScriptManager");
                    }
                    else
                    {
                        execargs = null;
                    }
                }
            }
            if ((SettingsFile.Read("skipmanager", "CDScriptManager") == "1") && (arg_exec_found == 0))
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write("Skipping manager is enabled\n");
                }
            }
            if (arg_preset_found == 1 || arg_exec_found == 1 || arg_execargs_found == 1)
            {
                RunGame(exec, execargs);
                this.Close();
            }
        }

        public void RunGame(string filePath, string args)
        {
            try
            {
                bool checksum;
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("----------------------------------------\n");
                }
                SHA256 sha256 = SHA256.Create();
                if (BitConverter.ToString(sha256.ComputeHash(File.ReadAllBytes(exec))).Replace("-", "") == "BF26B285AC25316191411006D787B2A50D00B63ACCC8CA0A2ABC99EF4314704E")
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write("A valid checksum of the executable file was found. Game version - 1.0\n");
                    }
                    checksum = true;
                }
                else if (BitConverter.ToString(sha256.ComputeHash(File.ReadAllBytes(exec))).Replace("-", "") == "DF3672DE82B4D971E5785F34C1A20DD607716D9654C2AD3F4D77451C878AF39B")
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write("A valid checksum of the executable file was found. Game version - 1.1\n");
                    }
                    checksum = true;
                }
                else if (BitConverter.ToString(sha256.ComputeHash(File.ReadAllBytes(exec))).Replace("-", "") == "551837A0BE60EAD7E4797AFE27864165FEC81605078C964EFE6A03D386E49395")
                {
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write("A valid checksum of the executable file was found. Game version - 1.2\n");
                    }
                    checksum = true;
                }
                else
                {
                    MessageBox.Show("The file checksum does not match. Please make sure you are using valid game executables. See the GitHub page for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [ERROR] ");
                        logfile.Write("The file checksum does not match. The file cannot be manipulated.\n");
                    }
                    checksum = false;
                }
                if (checksum == true)
                {
                    File.SetAttributes(exec, FileAttributes.Normal);
                    foreach (Control c in this.Controls)
                    {
                        c.Enabled = false;
                    }
                    tempfile = Path.GetTempFileName();
                    File.Copy(Directory.GetCurrentDirectory() + "\\" + filePath, tempfile, true);
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Creating temporary file \"{tempfile}\"\n");
                    }
                    File.SetAttributes(tempfile, FileAttributes.Normal);
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
                        {
                            using (var logfile = new StreamWriter(logfilepath, true))
                            {
                                logfile.Write("----------------------------------------\n");
                            }
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar1, mainvar1value, true);
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar2, mainvar2value, true);
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar3, mainvar3value, true);
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar4, mainvar4value, true);
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar5, mainvar5value, true);
                            CheckMainVar(checkedListBox1.Items[i].ToString(), mainvar6, mainvar6value, true);
                            ScriptChecking(checkedListBox1.Items[i].ToString());
                        }
                    }
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("----------------------------------------\n");
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Running executable file \"{exec}\"\n");
                    }
                    if (args != null)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Command-line arguments found: \"{args}\"\n");
                        }
                    }
                    else
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"No command-line arguments found\n");
                        }
                    }
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = Directory.GetCurrentDirectory() + "\\" + filePath,
                        Arguments = args
                    };
                    Process.Start(startInfo).WaitForExit();
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Closing executable file \"{exec}\"\n");
                    }
                    File.Copy(tempfile, Directory.GetCurrentDirectory() + "\\" + filePath, overwrite: true);
                    File.Delete(tempfile);
                    using (var logfile = new StreamWriter(logfilepath, true))
                    {
                        logfile.Write("[" + DateTime.Now.ToString() + "]");
                        logfile.Write(" [INFO] ");
                        logfile.Write($"Deleting temporary file \"{tempfile}\"\n");
                    }
                    foreach (Control c in this.Controls)
                    {
                        c.Enabled = true;
                    }
                }
            }
            catch
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [ERROR] ");
                    logfile.Write("The executable file is not found\n");
                }
                MessageBox.Show("The executable file could not be found in the current directory. Make sure that the file is in the current folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CheckScriptFiles()
        {
            try
            {
                FileInfo[] Files = scriptsfolder.GetFiles("*.cdscript");

                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Scanning the \"scripts\" folder\n");
                }

                if (Files.Length == 0)
                {
                    checkedListBox1.Visible = false;
                    label2.Visible = false;
                    groupBox1.Visible = false;
                    label3.Visible = true;
                }
                else
                {
                    checkedListBox1.Visible = true;
                    label2.Visible = true;
                    groupBox1.Visible = true;
                    label3.Visible = false;

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
            catch
            {
                Directory.CreateDirectory(presetspath);
                checkedListBox1.Visible = false;
                label2.Visible = false;
                groupBox1.Visible = false;
                label3.Visible = true;
            }
        }
        private void CheckPresetFile()
        {
            IniFile PresetFile = new IniFile(presetspath + currentpresetname + ".ini");

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("----------------------------------------\n");
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
            }
        }

        private void CheckFormTitle()
        {
            if ((currentpresetname == "default") || (currentpresetname == ""))
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
                logfile.Write("----------------------------------------\n");
            }
            mainvarcount = 0;

            // �������� "����" ������� CDScript
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // ������� ������, ��� ���������� ������ ���������
                    int startIndex = line.IndexOf(scriptbody);
                    if (startIndex != -1 && line.StartsWith(scriptbody))
                    {
                        // ���� ������ �������� ������, �� ������� ���������� ����� ����
                        scriptbodyname = line.Substring(startIndex + scriptbody.Length);
                        break; // ��������� ���� ����� ���������� ������� ����������
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
                catch
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
                    MessageBox.Show($"Failed to read script file \"{filename}\". Script body is not found. Please check the correctness of the script code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        readContent = true; // �������� ������ ����� ����������� ������
                        continue;
                    }
                    else if (line.Trim() == "}")
                    {
                        readContent = false; // ����������� ������ ����� ����������� �������
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
                            contentBuilder.Append("\r\n"); // ��������� ������� ������
                            linescount++;
                        }
                        previousLine = line.TrimStart('\t'); // ������� ��������� ��������� � ��������� ������

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

            string[] lines = contentBuilder.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < linescount; i++)
            {
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"Checking line {i + 1}\n");
                }
                string line = lines[i];
                int patternIndex;
                string setting_type = null;
                if (line.Contains(code_print))
                {
                    patternIndex = line.IndexOf(code_print);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
                        string contentAfterPattern = line.Substring(patternIndex + code_print.Length);
                        string result = contentAfterPattern.Split('\n')[0]
                            .Replace("(", "")
                            .Replace(")", "")
                            .Replace("\"", "")
                            .Replace("\\n", "\n")
                            .Replace("\t", "");
                        //MessageBox.Show(result); // ����� ����������� �� �������� ������ "\n"
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

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                        byteArray_startpoint = ConvertHexStringToByteArray(result);
                        byte[] fileBytes = File.ReadAllBytes(exec);
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
                        setting_type = settingParts[2];
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
                            logfile.Write($"Found variable \"{key}\". The value is \"{settings[key]}\"\n");
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_b))
                {
                    patternIndex = line.IndexOf(code_exec_rep_b);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                        try
                        {
                            byteArray_insert = ConvertHexStringToByteArray(result);
                            ReplaceBytesInFile(exec, byteindex, byteArray_insert);
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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_b);
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i8.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();
                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    if (expression == "true")
                                    {
                                        ReplaceBytesInFile(exec, byteindex, [01]);
                                    }
                                    else if (expression == "false")
                                    {
                                        ReplaceBytesInFile(exec, byteindex, [00]);
                                    }
                                    byteindex += 1;
                                }
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [INFO] ");
                                    logfile.Write($"1 byte replaced in index {byteindex}\n");
                                }
                            }
                            catch
                            {
                                using (var logfile = new StreamWriter(logfilepath, true))
                                {
                                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                                    logfile.Write(" [WARNING] ");
                                    logfile.Write("Byte operations failed\n");
                                }
                            }
                        }
                    }
                }
                else if (line.Contains(code_exec_rep_f))
                {
                    patternIndex = line.IndexOf(code_exec_rep_f);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(exec, byteindex, byteArray_insert);

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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_f);
                            float computeResult = 0f;
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_f.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = Convert.ToSingle(dataTable.Compute(expression, ""));
                                    ReplaceBytesInFile(exec, byteindex, BitConverter.GetBytes(computeResult));
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

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                        int result = SByte.Parse(result_text);
                        byteArray_insert = BitConverter.GetBytes(result);
                        Array.Resize(ref byteArray_insert, byteArray_insert.Length - 3);
                        try
                        {
                            ReplaceBytesInFile(exec, byteindex, byteArray_insert);
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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_i8);
                            int computeResult = 0;
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i8.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = SByte.Parse(expression);
                                    ReplaceBytesInFile(exec, byteindex, BitConverter.GetBytes(computeResult));
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
                else if (line.Contains(code_exec_rep_i16))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i16);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                            int result = Int16.Parse(result_text);
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(exec, byteindex, byteArray_insert);

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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_i16);
                            int computeResult = 0;
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i16.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = Int16.Parse(expression);
                                    ReplaceBytesInFile(exec, byteindex, BitConverter.GetBytes(computeResult));
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
                else if (line.Contains(code_exec_rep_i32))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i32);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                            int result = Int32.Parse(result_text);
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(exec, byteindex, byteArray_insert);

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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_i32);
                            int computeResult = 0;
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i32.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = Int32.Parse(expression);
                                    ReplaceBytesInFile(exec, byteindex, BitConverter.GetBytes(computeResult));
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
                else if (line.Contains(code_exec_rep_i64))
                {
                    patternIndex = line.IndexOf(code_exec_rep_i64);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                            long result = Int64.Parse(result_text);
                            byteArray_insert = BitConverter.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(exec, byteindex, byteArray_insert);

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
                        catch
                        {
                            patternIndex = line.IndexOf(code_exec_rep_i64);
                            long computeResult = 0;
                            // ���������, �������� �� ������ ������� �������
                            try
                            {
                                if (patternIndex != -1)
                                {
                                    // ���������� ���������� ���������� ������ ����� ��������
                                    contentAfterPattern = line.Substring(patternIndex + code_exec_rep_i64.Length);
                                    string expression = contentAfterPattern.Split('=')[1].Trim();

                                    foreach (var setting in settings)
                                    {
                                        expression = Regex.Replace(expression, setting.Key, setting.Value.ToString(CultureInfo.InvariantCulture));
                                    }
                                    DataTable dataTable = new DataTable();
                                    computeResult = Int64.Parse(expression);
                                    ReplaceBytesInFile(exec, byteindex, BitConverter.GetBytes(computeResult));
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
                else if (line.Contains(code_exec_rep_s))
                {
                    patternIndex = line.IndexOf(code_exec_rep_s);

                    // ���������, �������� �� ������ ������� �������
                    if (patternIndex != -1)
                    {
                        // ���������� ���������� ���������� ������ ����� ��������
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
                            byteArray_insert = Encoding.ASCII.GetBytes(result);
                            try
                            {
                                ReplaceBytesInFile(exec, byteindex, byteArray_insert);

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
            //MessageBox.Show(contentBuilder.ToString()); // ��������� ��������
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
            // ��������� ���� �� ������ � ������
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                // ���������� ��������� � ����� �� �������� ������
                fs.Seek(index, SeekOrigin.Begin);

                // ���������� ����� ����� � ����, ������� � ���������� �������
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
                    // ������� ������, ��� ���������� ������ ���������
                    int startIndex = line.IndexOf(scriptsetting);
                    if (startIndex != -1 && line.Trim('\t').StartsWith(scriptsetting))
                    {
                        // ���� ������ �������� ������, �� ������� ���������� ����� ����
                        scriptsettingname = line.Trim('\t').Substring(scriptsetting.Length);
                        break; // ��������� ���� ����� ���������� ������� ����������
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
                            readContent = true; // �������� ������ ����� ����������� ������
                            continue;
                        }
                        else if (line.Trim() == "}")
                        {
                            readContent = false; // ����������� ������ ����� ����������� �������
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
                                contentBuilder.Append("\r\n"); // ��������� ������� ������
                                linescount2++;
                            }
                            previousLine = line.TrimStart('\t'); // ������� ��������� ��������� � ��������� ������
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

                        // ���������, �������� �� ������ ������� �������
                        if (patternIndex != -1)
                        {
                            // ���������� ���������� ���������� ������ ����� ��������
                            string contentAfterPattern = line.Substring(patternIndex + code_string_create.Length);
                            string result = contentAfterPattern.Split('\n')[0]
                                .Replace("\t", "")
                                .Replace(" = ", "")
                                .Replace(" =", "")
                                .Replace("= ", "")
                                .Replace("=", "");
                            //MessageBox.Show(result); // ����� ����������� �� �������� ������ "\n"
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
            return -1;  // ���������� -1, ���� ������������������ �� �������
        }

        private byte[] ConvertHexStringToByteArray(string hexString)
        {
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                string[] hexPairs = hexString.Split(' ');
                byte[] bytes = new byte[hexPairs.Length];
                for (int i = 0; i < hexPairs.Length; i++)
                {
                    bytes[i] = Convert.ToByte(hexPairs[i], 16);
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
                    // ������� ������, ��� ���������� ������ ���������
                    int startIndex = line.IndexOf(mainvar);
                    if (startIndex != -1 && line.StartsWith(mainvar))
                    {
                        // ���� ������ �������� ������, �� ������� ���������� ����� ����
                        mainvarvalue = line.Substring(startIndex + mainvar.Length);
                        break; // ��������� ���� ����� ���������� ������� ����������
                    }
                }
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
                    if (mainvarvalue != null)
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [INFO] ");
                            logfile.Write($"Found main CDScript variable \"{mainvar}\", value is \"{mainvarvalue}\"\n");
                            mainvarcount++;
                        }
                    }
                    else
                    {
                        using (var logfile = new StreamWriter(logfilepath, true))
                        {
                            logfile.Write("[" + DateTime.Now.ToString() + "]");
                            logfile.Write(" [WARNING] ");
                            logfile.Write($"Could not found value for main CDScript variable \"{mainvar}\"\n");
                        }
                    }
                }
                return mainvarvalue;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // �������� "�������" ���������� CDScript
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
                    }
                }
            }
            catch
            {
                MessageBox.Show("Failed to load script file information. Refresh the list to get up-to-date list of script files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ToggleConfigButton(string filename)
        {
            using (StreamReader reader = new StreamReader(scriptsfolder + "\\" + filename))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    // ������� ������, ��� ���������� ������ ���������
                    int startIndex = line.IndexOf(scriptsetting);
                    if (startIndex != -1 && line.Trim('\t').StartsWith(scriptsetting))
                    {
                        // ���� ������ �������� ������, �� ������� ���������� ����� ����
                        scriptsettingname = line.Trim('\t').Substring(scriptsetting.Length);
                        break; // ��������� ���� ����� ���������� ������� ����������
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
                    catch
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
            // ���������� ��������� ������� ��� null, ���� ������ �� �������
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
                previousIndex = -1;
                checkedListBox1.ClearSelected();
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
                exec = Path.GetFileName(ofd.FileName);
                SettingsFile.Write("exec", exec, "CDScriptManager");
                using (var logfile = new StreamWriter(logfilepath, true))
                {
                    logfile.Write("[" + DateTime.Now.ToString() + "]");
                    logfile.Write(" [INFO] ");
                    logfile.Write($"New executable file is \"{SettingsFile.Read("exec", "CDScriptManager")}\"\n");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(GetSelectedListItem());
            try
            {
                form3.ShowDialog();
            }
            catch { }
        }

        private void runGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SettingsFile.KeyExists("execargs", "CDScriptManager"))
            {
                execargs = SettingsFile.Read("execargs", "CDScriptManager");
            }
            else
            {
                execargs = null;
            }
            RunGame(exec, execargs);
        }

        private void createAShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.FileName = $"Crashday (CDScript - {currentpresetname})";
            sfd.DefaultExt = ".lnk";
            sfd.Filter = "Shortcut (*.lnk)|*.lnk";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var wsh = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)wsh.CreateShortcut(sfd.FileName);
                shortcut.Description = "Crashday with integrated CDScript";
                shortcut.IconLocation = Application.ExecutablePath + ",1";
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                if (SettingsFile.KeyExists("execargs", "CDScriptManager"))
                {
                    shortcut.Arguments = $"/preset=\"{currentpresetname}\" /exec=\"{exec}\" /execargs=\"{execargs}\"";
                }
                else
                {
                    shortcut.Arguments = $"/preset=\"{currentpresetname}\" /exec=\"{exec}\"";
                }
                shortcut.Save(); // ��������� �����
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var logfile = new StreamWriter(logfilepath, true))
            {
                logfile.Write("[" + DateTime.Now.ToString() + "]");
                logfile.Write(" [INFO] ");
                logfile.Write($"Closing application \"{Path.GetFileName(Application.ExecutablePath)}\"\n");
            }
        }

        private void setExecutableArgumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 aboutWindow = new Form4();
            aboutWindow.ShowDialog();
        }
    }
}