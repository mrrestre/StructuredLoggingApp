using CommandLine;
using Serilog;
using System;
using System.Collections.Generic;

namespace TestAppWithSerilog.PatternLibrary
{
    static class LogPatternExeption
    {
        public static void LogStartUpException(Exception exception)
        {
            Log.Logger.Fatal("Application failed to start correctly. Exception trace: {Exception}", exception);
        }

        public static void LogParsingException(IEnumerable<Error> errors)
        {
            string errorString = "";

            foreach (var error in errors)
            {
                errorString += error.ToString();
            }

            Log.Fatal("Parser could not parse arguments correctly, Exception: {Exception}", errorString);
        }
        
    }
}
