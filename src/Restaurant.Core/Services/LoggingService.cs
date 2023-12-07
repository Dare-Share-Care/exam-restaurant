using Microsoft.Extensions.Logging;
using Restaurant.Core.Interfaces;
using Path = System.IO.Path;

namespace Restaurant.Core.Services;

public class LoggingService : ILoggingService
{
   
    public void LogToFile(LogLevel logLevel, string logMessage, Exception? exception = null)
    {
        try
        {
            string logFileName;

            // Determine the log file name based on the log level
            switch (logLevel)
            {
                case LogLevel.Information:
                    logFileName = "info.log";
                    break;
                case LogLevel.Warning:
                    logFileName = "warning.log";
                    break;
                case LogLevel.Error:
                    logFileName = "error.log";
                    break;
                case LogLevel.Critical:
                    logFileName = "critical.log";
                    break;
                default:
                    logFileName = "unknown.log";
                    break;
            }

            string logFilePath = Path.Combine("logs", logFileName);

            // Ensure the directory exists before logging
            string? logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Create a log entry with various information
            string logEntry = $"{DateTime.Now} [{logLevel}] - {logMessage}";

            // Include exception information if available
            if (exception != null)
            {
                logEntry += $"\nException: {exception.GetType().Name} - {exception.Message}\nStackTrace: {exception.StackTrace}";
            }

            // Append the log message to the file
            File.AppendAllText(logFilePath, logEntry + "\n");
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }


    
   
    
}