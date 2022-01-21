using CommandLine;
using NLog;
using System;
using System.Diagnostics;

namespace TestAppWithNLog.Commands
{
    [Verb("algo", HelpText = "Run an iterative algorithm (Square Root Calculation) with and without logging to compare the needed time")]
    public class AlgoCommand
    {
        private static ILogger logger = LogManager.GetLogger(typeof(AlgoCommand).FullName);

        public const int maximumLoops = 100000;

        [Option('v', "value",
            Required = false,
            Default = (double)4133,             //Prime number to make the calculation a little harder
            HelpText = "The value from which the square root should be calculated")]
        public double value { get; set; }

        [Option('p', "precision",
            Required = false,
            Default = 5,
            HelpText = "Defines how many precise the answer is going to be. If the precision is high, the calculation may take long\n" +
                        "Example: 2 --> Precision = 0.01")]
        public double precision { get; set; }

        public void Execute()
        {
            precision = CalculatePrecision(precision);
            logger.Debug("Choosen configurations: {@Configurations}", this);

            RunAlgorithm(true);
            RunAlgorithm(false);
        }

        public static double CalculatePrecision(double exponent)
        {
            return Math.Pow(10, -(exponent));
        }

        public double RunAlgorithm(bool loggingEnabled)
        {
            Stopwatch sw = new();
            sw.Start();

            int counter = 0;

            double low = 0;
            double high = value;
            double mid = 0;

            while ((high - low) > precision)
            {
                mid = (double)((low + high) / 2);
                if ((mid - precision) >= mid * mid && mid * mid <= (precision + mid))
                {
                    break;
                }
                else if (mid * mid < value)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }


                if (loggingEnabled)
                {
                    logger.Trace("Algorith running, currently on iteration: {Iteration}", counter);
                }


                counter++;

                //Avoid infinite loops
                if (counter >= maximumLoops)
                {
                    break;
                }
            }

            if (loggingEnabled)
            {
                logger.Info("Algorithm with Logging calculated in {Seconds}, Answer: {Answer}", sw.Elapsed, mid);
            }
            else
            {
                logger.Info("Algorithm without Logging calculated in {Seconds}, Answer: {Answer}", sw.Elapsed, mid);
            }

            sw.Stop();

            return Math.Round(mid, 3);
        }
    }
}
