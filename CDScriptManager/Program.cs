using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CDScriptManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern int FlashWindow(IntPtr Hwnd, bool Revert);

        [STAThread]
        static void Main()
        {
            using (var mutex = new Mutex(false, "CDScriptManager"))
            {
                // TimeSpan.Zero to test the mutex's signal state and
                // return immediately without blocking
                bool isAnotherInstanceOpen = !mutex.WaitOne(TimeSpan.Zero);
                if (isAnotherInstanceOpen)
                {
                    MessageBox.Show("The application is already running!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // main application entry point
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
                mutex.ReleaseMutex();
            }
        }
    }
}