using CommandLine;
using NLog;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using TestAppWithNLog.PatternLibrary;

namespace TestAppWithNLog.Commands
{
    [Verb("multiple", HelpText = "Generate multiple logs and send the to the defined Sinks")]
    public class MultipleCommand
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

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
            // Ensure that the value given in the parameter is valid for a test
            if (Enum.IsDefined(typeof(E_TestKinds), testKind))
            {
                // It is a good practice to log an Object as a whole before procedure that may go wrong. The logger generates a json from all the parameters
                LogPatternConfiguration.LogConfiguration(this, logger);

                // Transform second into milliseconds
                var howLong = time * 1000;

                switch (testKind)
                {
                    case E_TestKinds.TimerVariant:
                        CallSendLogs_TimerVariant(howLong);
                        break;

                    case E_TestKinds.MaxMessagesInTime:
                        logger.Info("Testing the maximum possible Logs generation in {HowLong} second(s)", time);
                        var messagesSent = SendLogs_MaxMessagesInTime(howLong);
                        logger.Info("Managed to send {NumberOfMessages} messages in {Time} milliseconds", messagesSent, howLong);
                        break;

                    case E_TestKinds.TimeForMessages:
                        logger.Info("Testing how long does it take to send {NumberOfMessages} logs", number);
                        var neededTime = SendLogs_TimeForMessages(number);
                        logger.Info("It took {Time} milliseconds to send {NumberOfMessages} messages", neededTime, number);
                        break;
                }
            }
            else
            {
                logger.Error(new Exception("Kind of Test not defined"), "Kind of test not Valid. Exception: {Exception}");
            }
        }

        // This function is needed to control the calling from the timer variant
        public void CallSendLogs_TimerVariant(int howLong)
        {
            // Calculate how many millisecond should elapse between two logs
            double howLongBetweenLogs = (time * 1000) / number;

            // Create timer that lasts for the just calculated interval
            var intervallTimer = new Timer(howLongBetweenLogs);

            // Calls a function that sends a message each time the timer has elapsed for a given time
            var taskDelay = SendLogs_TimerVariant(howLong, intervallTimer);

            // Needed to ensure all messages are sent
            Task.WaitAll(taskDelay);

            // Cleanup the timer after user
            intervallTimer.Stop();
            intervallTimer.Dispose();
        }

        private async Task SendLogs_TimerVariant(int howLong, Timer intervallTimer)
        {
            int counter = 1;

            // Definition the after each time, that the timer is finished, it starts again
            intervallTimer.AutoReset = true;
            intervallTimer.Enabled = true;

            // Definition of what happens after each time the timer is finished (Send a message with current message number)
            intervallTimer.Elapsed += (s, e) =>
            {
                logger.Info("Testing the multiple command. Currently writing message: {MessageNumber} from {HowMany}", counter, number);
                counter++;
            };

            intervallTimer.Start();

            // Do this function until the given time (howLong) has elapsed
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
                logger.Trace("Max Message Test. Currently writing message: {MessageNumber}", counter);
            }

            sw.Stop();

            return counter;
        }

        public static long SendLogs_TimeForMessages(int howMany)
        {
            Stopwatch sw = new();
            sw.Start();

            for (int i = 0; i < howMany; i++)
            {
                logger.Trace("Current message: {CurrentMessage}, current taken time: {CurrentTime}", i, sw.ElapsedMilliseconds);
            }

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }
    }

    public enum E_TestKinds
    {
        TimerVariant = 1,
        MaxMessagesInTime = 2,
        TimeForMessages = 3
    };
}