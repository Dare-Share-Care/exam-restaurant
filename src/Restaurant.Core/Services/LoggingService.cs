using Restaurant.Core.Interfaces;
using Path = System.IO.Path;

namespace Restaurant.Core.Services;

public class LoggingService : ILoggingService
{
    public void LogToFile(string logMessage)
    {
        try
        {
            string logFilePath = "logs/logfile.txt";
            
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