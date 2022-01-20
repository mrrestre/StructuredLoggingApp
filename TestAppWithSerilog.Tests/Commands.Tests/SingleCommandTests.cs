using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.Commands.Tests
{
    [TestClass]
    public class SingleCommandTests
    {
        private SingleCommand _testSingleCommand = new()
        {
            LogLevel = E_LogLevels.Information
        };

        [TestMethod]
        public void ByCallingExecuteFunction_ExactlyOneLog_IsGenerated()
        {
            using (TestCorrelator.CreateContext())
            {
                _testSingleCommand.Execute();

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle();
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
    }
}
