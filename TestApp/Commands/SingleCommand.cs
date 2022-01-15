using CommandLine;
using Serilog;

namespace TestApp.Commands
{
    [Verb("single", HelpText = "Create a single log and send it to defined Sinks")]
    public class SingleCommand
    {
        [Option('l', "level", Default = 3, Required = false, 
            HelpText =  "Creates a log event with a given Log-Level\n" +
                        "1 --> Verbose\n" +
                        "2 --> Debug\n" +
                        "3 --> Information\n" +
                        "4 --> Warning\n" +
                        "5 --> Error\n" +
                        "6 --> Fatal\n")]
        public int _logLevel { get; set; }
        
        //TODO: how can I make that the posible answers are the ones that I want?


        public void Execute()
        {
            Log.Logger.Information("Application Starting with Command {command}", GetType().Name);

            Log.Logger.Information("Choosen level {logLevel}", _logLevel);

            Log.Logger.Information("Executing verb Single");
        }
    }

    enum E_LogLevels
    {
        verbose     = 1,
        debug       = 2,
        information = 3,
        warning     = 4,
        error       = 5, 
        fatal       = 6
    };
}
