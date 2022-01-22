using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.Commands.Tests
{
    [TestClass]
    public class MultipleCommandTests
    {
        private readonly MultipleCommand multipleCommand = new();

        [TestMethod]
        public void IfTheKindOfTestIsNotValid_OneLogIsGenereated_WithErrorLevelAndAnException()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                multipleCommand.testKind = (E_TestKinds)4;

                // Calculate
                multipleCommand.Execute();

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().ContainSingle()
                    .Which.Level.Should().Be(Serilog.Events.LogEventLevel.Error);
            }
        }

        [TestMethod]
        public void ForARunWithTimerVariant_GivenNumberOfLogMessages_AreCreated()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                multipleCommand.number = 10;
                multipleCommand.time = 1;
                var howLong = multipleCommand.time * 1000;

                // Calculate
                multipleCommand.CallSendLogs_TimerVariant(howLong);

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(multipleCommand.number);
            }
        }

        [TestMethod]
        public void ForARunWithMaxMessageKind_NumberOfIterationsReflectsOnNumberOfLogsCreated()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                int timeInMilliseconds = 100;

                // Calculate
                var numberOfMessages = MultipleCommand.SendLogs_MaxMessagesInTime(timeInMilliseconds);

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(numberOfMessages);
            }
        }

        [TestMethod]
        public void ForARunWithTimeForMessages_TheRightAmountOfMessagesIsSent()
        {
            using (TestCorrelator.CreateContext())
            {
                // Given
                int howManyMessages = 100;

                // Calculate
                MultipleCommand.SendLogs_TimeForMessages(howManyMessages);

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(howManyMessages);
            }
        }
    }
}