using Microsoft.Extensions.Logging;

namespace Restaurant.Core.Interfaces;

public interface ILoggingService
{
    void LogToFile(LogLevel logLevel, string logMessage, Exception? exception = null);
    
   
}