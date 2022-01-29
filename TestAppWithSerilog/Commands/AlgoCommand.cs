using CommandLine;
using Serilog;
using System;
using System.Diagnostics;
using TestAppWithSerilog.PatternLibrary;

namespace TestAppWithSerilog.Commands
{
    [Verb("algo", HelpText = "Run an iterative algorithm (Square Root Calculation) with and without logging to compare the needed time")]
    public class AlgoCommand
    {
        // Constant to prevent infinite loops in case that the algorithm needs to many operations
        public const int maximumLoops = 100000;

        [Option('v', "value",
            Required = false,
            Default = (double)4133,
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
            // Since precision is given as a whole number, the quantity of needed zeros is calculated with this simple function
            precision = CalculatePrecision(precision);

            // It is a good practice to log an Object as a whole before procedure that may go wrong. The logger generates a json from all the parameters
            LogPatternConfiguration.LogConfiguration(this);

            // The algorithm is run twice. Once with internal logging and once without
            S_AlgorithmResults algoWithLogs     = RunAlgorithm(true, value, precision);
            S_AlgorithmResults algoWithoutLogs  = RunAlgorithm(false, value, precision);

            Log.Logger.Information("Algorithm with Logging calculated in {Seconds}, Answer: {Answer}",      algoWithLogs.neededTime,    algoWithLogs.result);
            Log.Logger.Information("Algorithm without Logging calculated in {Seconds}, Answer: {Answer}",   algoWithoutLogs.neededTime, algoWithoutLogs.result);
        }

        public static double CalculatePrecision(double exponent)
        {
            return Math.Pow(10, -(exponent));
        }

        public static S_AlgorithmResults RunAlgorithm(bool loggingEnabled, double baseValue, double wantedPrecision)
        {
            // Instantiate a struct to save the results of this algorithm run
            var algoResult = new S_AlgorithmResults();
            
            Stopwatch sw = new();
            sw.Start();

            int counter = 0;

            double low = 0;
            double high = baseValue;
            double mid = 0;

            while ((high - low) > wantedPrecision)
            {
                mid = (double)((low + high) / 2);
                if ((mid - wantedPrecision) >= mid * mid && mid * mid <= (wantedPrecision + mid))
                {
                    break;
                }
                else if (mid * mid < baseValue)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }

                // If logging should be enabled, create one log on each iteration
                if (loggingEnabled)
                {
                    Log.Logger.Verbose("Algorithm running, currently on iteration: {Iteration}", counter);
                }

                counter++;

                //Avoid infinite loops
                if (counter >= maximumLoops)
                {
                    break;
                }
            }

            algoResult.neededTime = sw.Elapsed;
            algoResult.iterationsNeeded = counter;
            algoResult.result = Math.Round(mid, 3);

            sw.Stop();

            return algoResult;
        }
    }

    public struct S_AlgorithmResults
    {
        public TimeSpan neededTime;
        public int iterationsNeeded;
        public double result;
    }
}