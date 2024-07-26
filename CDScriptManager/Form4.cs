using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CDScriptManager
{
    public partial class Form4 : Form
    {
        IniFile SettingsFile = new IniFile(Directory.GetCurrentDirectory() + "\\cdsmanager_settings.ini");
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                SettingsFile.DeleteKey("execargs", "CDScriptManager");
            }
            else
            {
                SettingsFile.Write("execargs", textBox1.Text, "CDScriptManager");
            }
            MessageBox.Show("The arguments were saved successfully.","Save",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if(SettingsFile.KeyExists("execargs", "CDScriptManager"))
            {
                textBox1.Text = SettingsFile.Read("execargs", "CDScriptManager");
            }
            if (SettingsFile.KeyExists("exec", "CDScriptManager"))
            {
                this.Text = $"Command-Line Arguments - {SettingsFile.Read("exec", "CDScriptManager")}";
            }
        }
    }
}
