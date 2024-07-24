using Serilog;
using Serilog.Context;
using System.Reflection;

namespace Academy.Infrastructure.Logging.Serilog
{
    public static class SerilogService
    {
        public static void Information(string messageTemplate)
        {
            Log.Information(messageTemplate);
        }
        public static void Information(string messageTemplate, params object?[]? properties)
        {
            Log.Information(messageTemplate, properties);
        }

        public static void Information<T>(string messageTemplate, T properties)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
                LogContext.PushProperty(property.Name, property.GetValue(properties));

            Log.Information(messageTemplate);
        }

        public static void Error(Exception exception, string messageTemplate)
        {
            Log.Error(exception, messageTemplate);
        }

        public static void Error(string messageTemplate, params object?[]? properties)
        {
            Log.Error(messageTemplate, properties);
        }

        public static void Error<T>(Exception exception, string messageTemplate, T properties)
        {
            foreach (PropertyInfo property in typeof(T).GetProperties())
                LogContext.PushProperty(property.Name, property.GetValue(properties));

            Log.Error(exception, messageTemplate);
        }
    }
}
