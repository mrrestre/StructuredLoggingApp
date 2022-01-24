using CommandLine;
using NLog;
using System;
using TestAppWithNLog.Helpers;

namespace TestAppWithNLog.Commands
{
    [Verb("single", HelpText = "Create a single log and send it to defined Sinks")]
    public class SingleCommand
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        [Option('l', "level",
            Default = (E_LogLevels)3,
            Required = false,
            HelpText = "Creates a log event with a given Log-Level\n" +
                        "1 --> Verbose\n" +
                        "2 --> Debug\n" +
                        "3 --> Information\n" +
                        "4 --> Warning\n" +
                        "5 --> Error\n" +
                        "6 --> Fatal\n")]
        public E_LogLevels logLevel { get; set; }

        public void Execute()
        {
            if (Enum.IsDefined(typeof(E_LogLevels), logLevel))
            {
                logger.Debug("Chosen configurations: {@Configurations}", this);

                switch (logLevel)
                {
                    case E_LogLevels.Trace:
                        logger.Trace(LogLevelDefinition.log_levels["Trace"]);
                        break;

                    case E_LogLevels.Debug:
                        logger.Debug(LogLevelDefinition.log_levels["Debug"]);
                        break;

                    case E_LogLevels.Info:
                        logger.Info(LogLevelDefinition.log_levels["Info"]);
                        break;

                    case E_LogLevels.Warn:
                        logger.Warn(LogLevelDefinition.log_levels["Warning"]);
                        break;

                    case E_LogLevels.Error:
                        logger.Error(LogLevelDefinition.log_levels["Error"]);
                        break;

                    case E_LogLevels.Fatal:
                        logger.Fatal(LogLevelDefinition.log_levels["Fatal"]);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                logger.Error(new Exception("Log Level not defined"), "Log-Level not in Range. Exception: {Exception}");
            }
        }
    }

    public enum E_LogLevels
    {
        Trace = 1,
        Debug = 2,
        Info = 3,
        Warn = 4,
        Error = 5,
        Fatal = 6
    };
}