using System.ComponentModel;

namespace CatalogWebAPI.Logging;

public class CustomerLogger : ILogger
{
    readonly string loggerName;
    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomerLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public IDisposable BeginScope<TState>(TState? state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensage = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        WriteTexttoFile(mensage);
    }

    private void WriteTexttoFile(string mensage)
    {
        string pathFileLog = @"Logging\CatalogWebAPI_log.txt";
        using (StreamWriter streamWriter = new StreamWriter(pathFileLog, true))
        {
            try
            {
                streamWriter.WriteLine(mensage);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
