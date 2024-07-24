namespace Academy.Infrastructure.Logging
{
    public class LoggerSettings
    {
        public string AppName { get; set; } = "AI Core";
        public string ElasticSearchUrl { get; set; } = string.Empty;
        public bool WriteToConsole { get; set; } = false;
        public bool WriteToFile { get; set; } = false;
        public bool StructuredConsoleLogging { get; set; } = false;
        public string MinimumLogLevel { get; set; } = "";
        public bool IsRequestTracingEnabled { get; set; } = false;
        public bool IsDevelopmentEnv { get; set; } = false;
    }
}
