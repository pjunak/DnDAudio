using System;
using System.Windows.Forms;
using MusicPlayerApp.UI;


namespace MusicPlayerApp
{
    static class Program
    {
        /// <summary>
        /// Main entry point for the Music Player application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm()); // Initializes the main UI
        }
    }
}
