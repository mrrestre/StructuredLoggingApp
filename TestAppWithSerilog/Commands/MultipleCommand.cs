﻿using CommandLine;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace TestAppWithSerilog.Commands
{
    [Verb("multiple", HelpText = "Generate multiple logs and send the to the defined Sinks")]
    public class MultipleCommand
    {
        [Option('n', "number", 
            Required = false, 
            Default = 10,
            HelpText = "Defines the number of Log-Events to be generated")]
        public int Number { get; set; }

        [Option('t', "time", 
            Required = false,
            Default = 1,
            HelpText = "Defines the time for the Log-Events to be generated")]
        public int Time { get; set; }

        [Option('k', "kind",
            Default = (E_TestKinds)1,
            Required = false,
            HelpText = "Defines how the messages should be sent\n" +
                        "1 --> A Test where messages are sent with a calculated interval for a given time\n" +
                        "2 --> A Test where the maximal quantity of messages in a given time are produced\n" +
                        "3 --> A Test where an amount of messages is sent and the time it took is registered\n")]
        public E_TestKinds TestKind { get; set; }


        public void Execute()
        {
            Log.Logger.Debug("Choosen configurations: {@Configurations}", this);
            
            var howLong = Time * 1000;

            switch (TestKind)
            {
                case E_TestKinds.TimerVariant:
                    var howLongBetweenLogs = (Time * 1000) / Number;
                    
                    var intervallTimer = new Timer(howLongBetweenLogs);
                    var taskDelay = SendLogs_TimerVariant(howLong, intervallTimer);

                    Task.WaitAll(taskDelay);

                    intervallTimer.Stop();
                    intervallTimer.Dispose();
                    break;

                case E_TestKinds.MaxMessagesInTime:
                    SendLogs_MaxMessagesInTime(howLong);
                    break;

                case E_TestKinds.TimeForMessages:
                    SendLogs_TimeForMessages(Number);
                    break;

            }
        }

        private async Task SendLogs_TimerVariant(int howLong, Timer intervallTimer)
        {
            int counter = 1;

            intervallTimer.AutoReset = true;
            intervallTimer.Enabled = true;

            intervallTimer.Elapsed += (s, e) =>
            {
                Log.Logger.Information("Testing the multiple command. Currently writting message: {MessageNumber} from {HowMany}", counter, Number);
                counter++;
            };

            intervallTimer.Start();

            await Task.Delay(howLong);
        }

        private static void SendLogs_MaxMessagesInTime(int howLong)
        {
            int counter = 0;
            Stopwatch sw = new();
            sw.Start();

            while (sw.Elapsed.TotalMilliseconds < howLong)
            {
                counter++;
                Log.Logger.Verbose("Testing the maximum posible Logs in {HowLong} second(s). Currently writting message: {MessageNumber}", ( howLong/1000 ), counter);
            }

            Log.Logger.Information("Managed to send {NumberOfMessages} messages in {Time} milliseconds", counter, howLong);

            sw.Stop();
        }

        private static void SendLogs_TimeForMessages(int howMany)
        {
            Stopwatch sw = new();
            sw.Start();

            for (int i = 0; i < howMany; i++)
            {
                Log.Logger.Verbose("Current message: {CurrentMessage}, current taken time: {CurrentTime}", i, sw.ElapsedMilliseconds);
            }

            Log.Logger.Information("It took {Time} miliseconds to send {NumberOfMessages} messages", sw.ElapsedMilliseconds, howMany);

            sw.Stop();
        }
    }

    public enum E_TestKinds
    {
        TimerVariant        = 1,
        MaxMessagesInTime   = 2,
        TimeForMessages     = 3
    };
}