using CommandLine;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TestApp.Commands;

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
                .CreateLogger();

            try
            {
                Log.Information("Application starting up");

                Parser.Default.ParseArguments<SingleCommand, MultipleCommand>(args)
                    .WithParsed<SingleCommand>(t => t.Execute())
                    .WithParsed<MultipleCommand>(t => t.Execute())
                    .WithNotParsed(HandleParseError);
            }
            catch (Exception ex)
            {
                Log.Information("Application failed to start correctly. Exception trace: {exception}", ex);
            }
            finally
            {
                Log.Information("Application shutting down");
            }
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion())
            {
                var appName = AppDomain.CurrentDomain.FriendlyName;
                var appVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

                Log.Information("Version request from: {AppName} actual application Version: {Version}", appName, appVersion);
                return;
            }

            if (errs.IsHelp())
            {
                Log.Information("Help Request");
                return;
            }

            Log.Error("Parser could not parse arguments correctly, Exception: {exception}", errs.GetEnumerator().ToString());
        }
    }
}
