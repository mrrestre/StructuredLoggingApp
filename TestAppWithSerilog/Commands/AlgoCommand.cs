﻿using CommandLine;
using Serilog;
using System;
using System.Diagnostics;

namespace TestAppWithSerilog.Commands
{
    [Verb("algo", HelpText = "Run an iterative algorithm (Square Root Calculation) with and without logging to compare the needed time")]
    public class AlgoCommand
    {
        private const int maximumLoops = 100000;

        [Option('v', "value",
            Required = false,
            Default = (double)4133,             //Prime number to make the calculation a little harder
            HelpText = "The value from which the square root should be calculated")]
        public double Value { get; set; }

        [Option('p', "precision",
            Required = false,
            Default = 5,
            HelpText = "Defines how many precise the answer is going to be. If the precision is high, the calculation may take long\n" +
                        "Example: 2 --> Precision = 0.01")]
        public double Precision { get; set; }

        public void Execute()
        {
            Precision = CalculatePrecision(Precision);
            Log.Logger.Debug("Choosen configurations: {@Configurations}", this);

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

            int counter = 1;

            double low = 0;
            double high = Value;
            double mid = 0;

            while ((high - low) > Precision)
            {
                mid = (double)((low + high) / 2);
                if ((mid - Precision) >= mid * mid && mid * mid <= (Precision + mid))
                {
                    break;
                }
                else if (mid * mid < Value)
                {
                    low = mid;
                }
                else
                {
                    high = mid;
                }

                counter++;

                if (loggingEnabled)
                {
                    Log.Logger.Verbose("Algorith running, currently on iteration: {Iteration}", counter);
                }

                //Avoid infinite loops
                if(counter >= maximumLoops)
                {
                    break;
                }
            }

            if (loggingEnabled)
            {
                Log.Logger.Information("Algorithm with Logging calculated in {Seconds}, Answer: {Answer}", sw.Elapsed, mid);
            }
            else
            {
                Log.Logger.Information("Algorithm without Logging calculated in {Seconds}, Answer: {Answer}", sw.Elapsed, mid);
            }

            sw.Stop();

            return Math.Round(mid, 3);
        }
    }
}
