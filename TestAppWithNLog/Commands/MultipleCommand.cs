using CommandLine;
using NLog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace TestAppWithNLog.Commands
{
    [Verb("multiple", HelpText = "Generate multiple logs and send the to the defined Sinks")]
    public class MultipleCommand
    {
        private static ILogger logger = LogManager.GetLogger(typeof(MultipleCommand).FullName);

        [Option('n', "number", 
            Required = false, 
            Default = 10,
            HelpText = "Defines the number of Log-Events to be generated")]
        public int number { get; set; }

        [Option('t', "time", 
            Required = false,
            Default = 1,
            HelpText = "Defines the time for the Log-Events to be generated")]
        public int time { get; set; }

        [Option('k', "kind",
            Default = (E_TestKinds)1,
            Required = false,
            HelpText = "Defines how the messages should be sent\n" +
                        "1 --> A Test where messages are sent with a calculated interval for a given time\n" +
                        "2 --> A Test where the maximal quantity of messages in a given time are produced\n" +
                        "3 --> A Test where an amount of messages is sent and the time it took is registered\n")]
        public E_TestKinds testKind { get; set; }


        public void Execute()
        {
            if (Enum.IsDefined(typeof(E_TestKinds), testKind))
            {
                logger.Debug("Choosen configurations: {@Configurations}", this);

                var howLong = time * 1000;

                switch (testKind)
                {
                    case E_TestKinds.TimerVariant:
                        CallSendLogs_TimerVariant(howLong);
                        break;

                    case E_TestKinds.MaxMessagesInTime:
                        SendLogs_MaxMessagesInTime(howLong);
                        break;

                    case E_TestKinds.TimeForMessages:
                        SendLogs_TimeForMessages(number);
                        break;

                }
            }
            else
            {
                logger.Error(new Exception("Kind of Test not defined"), "Kind of test not Valid. Exception: {Exception}");
            }
        }

        public void CallSendLogs_TimerVariant(int howLong)
        {
            var howLongBetweenLogs = (time * 1000) / number;

            var intervallTimer = new Timer(howLongBetweenLogs);
            var taskDelay = SendLogs_TimerVariant(howLong, intervallTimer);

            Task.WaitAll(taskDelay);

            intervallTimer.Stop();
            intervallTimer.Dispose();
        }

        private async Task SendLogs_TimerVariant(int howLong, Timer intervallTimer)
        {
            int counter = 1;

            intervallTimer.AutoReset = true;
            intervallTimer.Enabled = true;

            intervallTimer.Elapsed += (s, e) =>
            {
                logger.Info("Testing the multiple command. Currently writting message: {MessageNumber} from {HowMany}", counter, number);
                counter++;
            };

            intervallTimer.Start();

            await Task.Delay(howLong);
        }

        public static int SendLogs_MaxMessagesInTime(int howLong)
        {
            int counter = 0;
            Stopwatch sw = new();
            sw.Start();

            while (sw.Elapsed.TotalMilliseconds < howLong)
            {
                counter++;
                logger.Trace("Testing the maximum posible Logs in {HowLong} second(s). Currently writting message: {MessageNumber}", ( howLong/1000 ), counter);
            }

            logger.Info("Managed to send {NumberOfMessages} messages in {Time} milliseconds", counter, howLong);

            sw.Stop();

            return counter;
        }

        public static void SendLogs_TimeForMessages(int howMany)
        {
            Stopwatch sw = new();
            sw.Start();

            for (int i = 0; i < howMany; i++)
            {
                logger.Trace("Current message: {CurrentMessage}, current taken time: {CurrentTime}", i, sw.ElapsedMilliseconds);
            }

            logger.Info("It took {Time} miliseconds to send {NumberOfMessages} messages", sw.ElapsedMilliseconds, howMany);

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
