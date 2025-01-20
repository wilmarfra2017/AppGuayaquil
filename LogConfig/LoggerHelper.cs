using Serilog;

namespace LogConfig;

public static class LoggerHelper
{
    public static void LogInfo(string message, object? context = null)
    {
        if (context != null)
        {
            Log.Information("{Message} - Context: {@Context}", message, context);
        }
        else
        {
            Log.Information(message);
        }
    }

    public static void LogError(string message, Exception ex, object? context = null)
    {
        if (context != null)
        {
            Log.Error(ex, "{Message} - Context: {@Context}", message, context);
        }
        else
        {
            Log.Error(ex, message);
        }
    }

    public static void LogDebug(string message, object? context = null)
    {
        if (context != null)
        {
            Log.Debug("{Message} - Context: {@Context}", message, context);
        }
        else
        {
            Log.Debug(message);
        }
    }
}
