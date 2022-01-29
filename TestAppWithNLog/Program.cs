using CommandLine;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using TestAppWithNLog.Commands;
using TestAppWithNLog.PatternLibrary;

namespace TestAppWithNLog
{
    internal class Program
    {
        private static ILogger logger;

        private static void Main(string[] args)
        {
            LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");

            logger = LogManager.GetCurrentClassLogger();

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
                LogPatternExeption.LogStartUpException(ex, logger);
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

            LogPatternExeption.LogParsingException(errs, logger);
        }
    }
}