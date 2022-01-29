using NLog;

namespace TestAppWithNLog.PatternLibrary
{
    static class LogPatternConfiguration
    {
        public static void LogConfiguration(object _object, ILogger logger)
        {
            logger.Debug("Chosen configurations: {@Configurations}", _object);
        }
    }
}
