using System.Collections.Generic;

namespace TestAppWithNLog.Helpers
{
    internal static class LogLevelDefinition
    {
        public static Dictionary<string, string> log_levels = new Dictionary<string, string>()
        {
            { "Trace",  "Very detailed log messages, potentially of a high frequency and volume" },
            { "Debug",  "Less detailed and/or less frequent debugging messages" },
            { "Info",   "Informational messages" },
            { "Warning","Warnings which don't appear to the user of the application" },
            { "Error",  "When functionality is unavailable or expectations broken, an Error event is used" },
            { "Fatal",  "Fatal error messages. After a fatal error, the application usually terminates" }
        };
    }
}