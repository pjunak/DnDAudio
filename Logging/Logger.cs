using System;
using System.Diagnostics;
using System.IO;

namespace MusicPlayerApp.Logging
{
    public static class Logger
    {
        private static readonly string LogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        /// <summary>
        /// Initializes the logger and creates a log file.
        /// </summary>
        public static void Initialize()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        public static void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        /// <summary>
        /// Writes log message to a file with timestamp and type.
        /// </summary>
        private static void WriteLog(string type, string message)
        {
            string logFile = Path.Combine(LogDirectory, $"Log_{DateTime.Now:yyyyMMdd}.txt");
            using (StreamWriter writer = new StreamWriter(logFile, true))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{type}] {message}");
            }
        }
    }
}
