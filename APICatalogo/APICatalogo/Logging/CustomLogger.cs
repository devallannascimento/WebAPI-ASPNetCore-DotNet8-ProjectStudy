
namespace APICatalogo.Logging;

public class CustomLogger : ILogger
{
    readonly string loggerName;

    readonly CustomLoggerProviderConfiguration loggerConfig;

    public CustomLogger(string name, CustomLoggerProviderConfiguration config)
    {
        loggerName = name;
        loggerConfig = config;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == loggerConfig.LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }


    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string mensagem = $"{logLevel.ToString()}: {eventId.Id} = {formatter(state, exception)}";

        EscreverTextoNoArquivo(mensagem);
    }

    public void EscreverTextoNoArquivo(string mensagem) 
    {
        string caminhoArquivoLog = @"e:\Programação\Web API ASP .NET Core Essencial (.NET 8)\APICatalogo\api-catalago_log.txt";

        using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
        { 
            try
            {
                streamWriter.WriteLine(mensagem);
                streamWriter.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
