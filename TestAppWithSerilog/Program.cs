using CommandLine;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Graylog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TestAppWithSerilog.Commands;
using TestAppWithSerilog.Enrichers;

namespace TestAppWithSerilog
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.With(new ProcessAndThreadEnricher())
                .Enrich.With(new UserEnricher())
                
                .CreateLogger();

            try
            {
                Log.Debug("Application starting up");
                
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
