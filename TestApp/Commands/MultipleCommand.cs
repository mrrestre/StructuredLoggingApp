using CommandLine;
using Serilog;

namespace TestApp.Commands
{
    [Verb("multiple", HelpText = "Make some logs")]
    public class MultipleCommand
    {
        [Option('q', "quantity", Required = true, HelpText = "How many logs do you want to do")]
        public int _quantity { get; set; }

        public void Execute()
        {
            Log.Logger.Information("Application Starting with Command {command}", GetType().Name);
        }
    }
}
