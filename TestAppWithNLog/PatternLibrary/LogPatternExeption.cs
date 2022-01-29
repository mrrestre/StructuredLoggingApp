using CommandLine;
using NLog;
using System;
using System.Collections.Generic;

namespace TestAppWithNLog.PatternLibrary
{
    static class LogPatternExeption
    {
        public static void LogStartUpException(Exception exception, ILogger logger)
        {
            logger.Fatal(exception, "Application failed to start correctly. Exception trace: {Exception}");
        }

        public static void LogParsingException(IEnumerable<Error> errors, ILogger logger)
        {
            string errorString = "";

            foreach (var error in errors)
            {
                errorString += error.ToString();
            }

            logger.Fatal("Parser could not parse arguments correctly, Exception: {Exception}", errorString);
        }
        
    }
}
