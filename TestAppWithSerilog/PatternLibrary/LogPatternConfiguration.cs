using CommandLine;
using Serilog;
using System;
using System.Collections.Generic;

namespace TestAppWithSerilog.PatternLibrary
{
    static class LogPatternConfiguration
    {
        public static void LogConfiguration(object _object)
        {
            Log.Logger.Debug("Chosen configurations: {@Configurations}", _object);
        }
    }
}
