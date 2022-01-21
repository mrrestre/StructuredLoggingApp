﻿using System.Collections.Generic;

namespace TestAppWithNLog.Helpers
{
    static class LogLevelDefinition
    {
        //TODO: Add the right naming and descriptions

        static public Dictionary<string, string> log_levels = new Dictionary<string, string>()
        {
            { "Verbose",    "Verbose is the noisiest level, rarely (if ever) enabled for a production app." },
            { "Debug",      "Debug is used for internal system events that are not necessarily observable from the outside, but useful when determining how something happened." },
            { "Information","Information events describe things happening in the system that correspond to its responsibilities and functions. Generally these are the observable actions the system can perform." },
            { "Warning",    "When service is degraded, endangered, or may be behaving outside of its expected parameters, Warning level events are used." },
            { "Error",      "When functionality is unavailable or expectations broken, an Error event is used." },
            { "Fatal",      "The most critical level, Fatal events demand immediate attention." }
        };
    }
}