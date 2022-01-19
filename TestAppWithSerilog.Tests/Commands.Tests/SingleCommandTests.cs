using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.SingleCommandTests
{
    [TestClass]
    public class SingleCommandTests
    {
        static readonly SingleCommand _testSingleCommand = new()
        {
            LogLevel = E_LogLevels.Information
        };

        [AssemblyInitialize]
        public static void ConfigureGlobalLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }

        [TestMethod]
        public void ByCallingExecuteFunction_ExactlyTwoLogs_AreGenerated()
        {
            using (TestCorrelator.CreateContext())
            {
                _testSingleCommand.Execute();

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(2);
            }
        }

        [TestMethod]
        public void IfTheLogLevelIsNotPossible_NoLogEventsAreGenerated()
        {
            using (TestCorrelator.CreateContext())
            {
                _testSingleCommand.LogLevel = (E_LogLevels)7;

                _testSingleCommand.Execute();

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().BeEmpty();
            }
        }

        [TestMethod]
        public void AStupidTest()
        {
            var value = 2;

            Assert.Equals(value, 2);
        }
    }
}
