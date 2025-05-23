

using System.Text;
using TaskManager.Application.Interfaces;

namespace TaskManager.Infrastructure.Logging;

public class FileLogger : IFileLogger
{
   
        private static readonly string _logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string _logFilePath = Path.Combine(_logDirectory, $"{DateTime.Now:dd_MM_yyyy}.txt");

        public FileLogger()
        {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }

        public void LogException(Exception ex, string context = "General")
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            logMessage.AppendLine(new string('.', 72));
            logMessage.AppendLine($"From: [{context}]");
            logMessage.AppendLine(new string('.', 72));
            logMessage.AppendLine($"Message: {ex.Message}");
            logMessage.AppendLine(new string('.', 72));
            logMessage.AppendLine("StackTrace:");
            logMessage.AppendLine(ex.StackTrace ?? "No stack trace available.");
            logMessage.AppendLine(new string('-', 72));
            logMessage.AppendLine();

            using var writer = new StreamWriter(_logFilePath, append: true);
            writer.WriteLine(logMessage.ToString());

            Console.WriteLine(logMessage.ToString());
        }


        public void LogInfo(string message, string context = "Info")
        {
            var logMessage = $@"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] [{context}]
            {message}
            -------------------------------------------------------";

            using var writer = new StreamWriter(_logFilePath, append: true);
            writer.WriteLine(logMessage);
        }
    }

