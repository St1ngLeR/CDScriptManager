using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace CDScriptManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            label5.Text = $"{Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0,5)} ({GetLinkerTime(Assembly.GetExecutingAssembly())})";
        }

        public static string GetLinkerTime(Assembly assembly)
        {
            const string BuildVersionMetadataPrefix = "+build";

            var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            if (attribute?.InformationalVersion != null)
            {
                var value = attribute.InformationalVersion;
                var index = value.IndexOf(BuildVersionMetadataPrefix);
                if (index > 0)
                {
                    value = value[(index + BuildVersionMetadataPrefix.Length)..];
                    return value;
                }
            }
            return default;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/St1ngLeR",
                UseShellExecute = true
            });
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();
        }

    }
}
