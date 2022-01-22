using CommandLine;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TestAppWithSerilog.Commands;
using TestAppWithSerilog.Enrichers;

namespace TestAppWithSerilog
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                // Needed configuration to read from appsettings.json (Not standard in console apps)
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                /*
                 * Logger configuration
                 * Notice that the sinks could be defined as well at this point instead from redden from json file
                 * At this point any number of enricher (Either default or personalized) can be added.
                 */
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .Enrich.With(new ProcessAndThreadEnricher())
                    .Enrich.With(new UserEnricher())
                    .CreateLogger();

                Log.Debug("Application starting up");

                // Definition of the command line parser. It takes care of handling the arguments and options and calling the right functions
                Parser.Default.ParseArguments<SingleCommand, MultipleCommand, AlgoCommand>(args)
                    .WithParsed<SingleCommand>(t => t.Execute())
                    .WithParsed<MultipleCommand>(t => t.Execute())
                    .WithParsed<AlgoCommand>(t => t.Execute())
                    .WithNotParsed(HandleParseError);
            }
            catch (Exception ex)
            {
                Log.Fatal("Application failed to start correctly. Exception trace: {Exception}", ex);
            }
            finally
            {
                Log.Debug("Application shutting down");
                Log.CloseAndFlush();
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion())
            {
                var appName = AppDomain.CurrentDomain.FriendlyName;
                var appVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

                Log.Verbose("Version request from: {AppName} actual application Version: {Version}", appName, appVersion);
                return;
            }

            if (errs.IsHelp())
            {
                Log.Verbose("Help Request");
                return;
            }

            Log.Error("Parser could not parse arguments correctly, Exception: {Exception}", errs.GetEnumerator().ToString());
        }
    }
}