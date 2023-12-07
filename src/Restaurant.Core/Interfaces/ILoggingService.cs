namespace Restaurant.Core.Interfaces;

public interface ILoggingService
{
    void LogToFile(string fileName, string logMessage);
    
    void LogError(string logMessage);
    
    void LogUnauthorizedAttempt(string logMessage);
}