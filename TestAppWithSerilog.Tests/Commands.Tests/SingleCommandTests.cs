using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.Commands.Tests
{
    [TestClass]
    public class SingleCommandTests
    {
        private readonly SingleCommand _testSingleCommand = new()
        {
            LogLevel = E_LogLevels.Information
        };

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
        public void IfTheLogLevelIsNotPossible_OneLogIsGenereated_WithErrorLevelAndAnException()
        {
            using (TestCorrelator.CreateContext())
            {
                _testSingleCommand.LogLevel = (E_LogLevels)7;

                _testSingleCommand.Execute();

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Level.Should().Be(Serilog.Events.LogEventLevel.Error);
            }
        }
    }
}
