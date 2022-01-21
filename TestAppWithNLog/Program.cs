using CommandLine;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using TestAppWithNLog.Commands;

namespace TestAppWithNLog
{
    class Program
    {
        private static ILogger logger;

        static void Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");

            logger = LogManager.GetLogger(typeof(Program).FullName);

            try
            {
                logger.Debug("Application starting up");

                Parser.Default.ParseArguments<SingleCommand, MultipleCommand, AlgoCommand>(args)
                    .WithParsed<SingleCommand>(t => t.Execute())
                    .WithParsed<MultipleCommand>(t => t.Execute())
                    .WithParsed<AlgoCommand>(t => t.Execute())
                    .WithNotParsed(HandleParseError);

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Application failed to start correctly. Exception trace: {Exception}");
            }
            finally
            {
                logger.Debug("Application shutting down");
                logger.Factory.Flush();
            }            
        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {
            if (errs.IsVersion())
            {
                var appName = AppDomain.CurrentDomain.FriendlyName;
                var appVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

                logger.Trace("Version request from: {AppName} actual application Version: {Version}", appName, appVersion);
                return;
            }

            if (errs.IsHelp())
            {
                logger.Trace("Help Request");
                return;
            }

            logger.Error("Parser could not parse arguments correctly, Exception: {Exception}", errs.GetEnumerator().ToString());
        }
    }
}
