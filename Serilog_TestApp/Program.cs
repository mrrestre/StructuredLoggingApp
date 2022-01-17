using CommandLine;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using TestApp.Commands;
using TestApp.Enrichers;

namespace TestApp
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
                .Enrich.FromLogContext()
                .Enrich.With(new ProcessAndThreadEnricher())
                .CreateLogger();

            try
            {
                Log.Debug("Application starting up");

                /*
                Task runParser = new Task(() =>
                {
                    Parser.Default.ParseArguments<SingleCommand, MultipleCommand>(args)
                    .WithParsed<SingleCommand>(t => t.Execute())
                    .WithParsed<MultipleCommand>(t => t.Execute())
                    .WithNotParsed(HandleParseError);
                });
                runParser.RunSynchronously();
                runParser.Wait();
                */

                
                Parser.Default.ParseArguments<SingleCommand, MultipleCommand>(args)
                    .WithParsed<SingleCommand>(t => t.Execute())
                    .WithParsed<MultipleCommand>(t => t.Execute())
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
