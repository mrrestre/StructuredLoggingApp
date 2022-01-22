using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.Commands.Tests
{
    [TestClass]
    public class SingleCommandTests
    {
        private readonly SingleCommand _testSingleCommand = new();

        [TestMethod]
        public void ByCallingExecuteFunctionWithWarningAsLevel_ExactlyOneLogWithWarningLevelAndOneWithDebugLevel_AreGenerated()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                _testSingleCommand.logLevel = E_LogLevels.Warning;
                int warningMessageCounter = 0;
                int debugMessageCounter = 0;
                int otherMessages = 0;

                // Calculate
                _testSingleCommand.Execute();
                var enumerator = TestCorrelator.GetLogEventsFromCurrentContext().GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if(enumerator.Current.Level == Serilog.Events.LogEventLevel.Warning)
                    {
                        warningMessageCounter++;
                    }
                    else if (enumerator.Current.Level == Serilog.Events.LogEventLevel.Debug)
                    {
                        debugMessageCounter++;
                    }
                    else
                    {
                        otherMessages++;
                    }
                }

                // Ensure
                Assert.AreEqual(expected: 1, actual: warningMessageCounter);
                Assert.AreEqual(expected: 1, actual: debugMessageCounter);
                Assert.AreEqual(expected: 0, actual: otherMessages);
            }
        }

        [TestMethod]
        public void IfTheLogLevelIsNotPossible_OneLogIsGenereated_WithErrorLevelAndAnException()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                _testSingleCommand.logLevel = (E_LogLevels)7;

                // Calculate
                _testSingleCommand.Execute();

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Level.Should().Be(Serilog.Events.LogEventLevel.Error);
            }
        }
    }
}