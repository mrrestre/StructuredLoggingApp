using CommandLine;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace TestApp.Commands
{
    [Verb("multiple", HelpText = "Generate periodic logs and send the to the defined Sinks")]
    public class MultipleCommand
    {
        [Option('n', "number", 
            Required = false, 
            Default = 10,
            HelpText = "Defines the number of Log-Events to be generated")]
        public int _number { get; set; }

        [Option('t', "time", 
            Required = false,
            Default = 1,
            HelpText = "Defines the time for the Log-Events to be generated")]
        public int _time { get; set; }

        [Option('r', "random",
            Required = false,
            Default = false,
            HelpText = "If this option is activated, the level of the Log-Event is randomly generated")]
        public bool _random { get; set; }

        public void Execute()
        {
            Log.Logger.Debug("Choosen configurations: {@Configurations}", this);

            var howLongBetweenLogs = (_time * 1000) / _number;
            var howLong = _time * 1000 + howLongBetweenLogs;

            /*
            Task sendLogs = new Task(() => SendLogs(howLongBetweenLogs, howLong));
            sendLogs.RunSynchronously();
            sendLogs.Wait();
            */
            
            SendLogs(howLongBetweenLogs, howLong);
        }

        private static void SendLogs(int howLongBetweenLogs, int howLong)
        {
            var totalTimeToRun = new Timer(howLong);
            var intervallTime = new Timer(howLongBetweenLogs);

            totalTimeToRun.Enabled = true;
            totalTimeToRun.Elapsed += (s, e) =>
            {
                intervallTime.Stop();
                totalTimeToRun.Stop();

                intervallTime.Dispose();
                totalTimeToRun.Dispose();
            };

            intervallTime.AutoReset = true;
            intervallTime.Enabled = true;
            intervallTime.Elapsed += (s, e) =>
            {
                Log.Logger.Information("Writting something {Time}", e.SignalTime);
            };

            totalTimeToRun.Start();
            intervallTime.Start();

            Console.ReadLine();
        }
    }
}
