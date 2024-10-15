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

    IDisposable ILogger.BeginScope<TState>(TState state)
    {
        return null!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
        WriteTexttoFile(message);
    }

    private void WriteTexttoFile(string message)
    {
        string pathFileLog = @"Logging\CatalogWebAPI_log.txt";
        using (StreamWriter streamWriter = new StreamWriter(pathFileLog, true))
        {
            try
            {
                streamWriter.WriteLine(message);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
