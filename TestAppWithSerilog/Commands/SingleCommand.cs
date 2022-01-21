using CommandLine;
using Serilog;
using System;
using TestAppWithSerilog.Helpers;

namespace TestAppWithSerilog.Commands
{
    [Verb("single", HelpText = "Create a single log and send it to defined Sinks")]
    public class SingleCommand
    {
        [Option('l', "level", 
            Default = (E_LogLevels)3, 
            Required = false, 
            HelpText =  "Creates a log event with a given Log-Level\n" +
                        "1 --> Verbose\n" +
                        "2 --> Debug\n" +
                        "3 --> Information\n" +
                        "4 --> Warning\n" +
                        "5 --> Error\n" +
                        "6 --> Fatal\n")]
        public E_LogLevels LogLevel { get; set; }


        public void Execute()
        {
            if(Enum.IsDefined(typeof(E_LogLevels), LogLevel))
            {
                Log.Logger.Debug("Choosen configurations: {@Configurations}", this);

                switch (LogLevel)
                {
                    case E_LogLevels.Verbose:
                        Log.Logger.Verbose(LogLevelDefinition.log_levels["Verbose"]);
                        break;

                    case E_LogLevels.Debug:
                        Log.Logger.Debug(LogLevelDefinition.log_levels["Debug"]);
                        break;

                    case E_LogLevels.Information:
                        Log.Logger.Information(LogLevelDefinition.log_levels["Information"]);
                        break;

                    case E_LogLevels.Warning:
                        Log.Logger.Warning(LogLevelDefinition.log_levels["Warning"]);
                        break;

                    case E_LogLevels.Error:
                        Log.Logger.Error(LogLevelDefinition.log_levels["Error"]);
                        break;

                    case E_LogLevels.Fatal:
                        Log.Logger.Fatal(LogLevelDefinition.log_levels["Fatal"]);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Log.Logger.Error("Log-Level not in Range. Exception: {Exception}", new Exception("Log Level not defined"));
            }
        }
    }

    public enum E_LogLevels
    {
        Verbose     = 1,
        Debug       = 2,
        Information = 3,
        Warning     = 4,
        Error       = 5, 
        Fatal       = 6
    };
}
