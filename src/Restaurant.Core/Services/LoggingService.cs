using Restaurant.Core.Interfaces;
using Path = System.IO.Path;

namespace Restaurant.Core.Services;

public class LoggingService : ILoggingService
{
   

    public void LogError(string logMessage)
    {
        LogToFile("errorlog.txt", logMessage);
    }

    public void LogUnauthorizedAttempt(string logMessage)
    {
        LogToFile("unauthorizedlog.txt", logMessage);
    }

    public void LogToFile(string fileName, string logMessage)
    {
        try
        {
            string logFilePath = Path.Combine("logs", fileName);

            // Ensure the directory exists before logging
            string? logDirectory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            File.AppendAllText(logFilePath, $"{DateTime.Now}: {logMessage}\n");
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
    }

    
   
    
}