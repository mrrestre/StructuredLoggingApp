using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
using System.Timers;
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
                multipleCommand.TestKind = (E_TestKinds)4;

                multipleCommand.Execute();

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
                multipleCommand.Number = 30;
                multipleCommand.Time = 1;

                var howLong = multipleCommand.Time * 1000;

                multipleCommand.CallSendLogs_TimerVariant(howLong);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(multipleCommand.Number);
            }
        }

        [TestMethod]
        public void ForARunWithMaxMessageKing_NumberOfIterationsReflectsOnNumberOfLogsCreatedMinusOne()
        {
            using (TestCorrelator.CreateContext())
            {
                int timeInMilliseconds = 100;

                var numberOfMessages = MultipleCommand.SendLogs_MaxMessagesInTime(timeInMilliseconds);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(numberOfMessages + 1);
            }
        }

        [TestMethod]
        public void ForARunWithTimeForMessages_TheRightAmountOfMessagesIsSent()
        {
            using (TestCorrelator.CreateContext())
            {
                int howManyMessages = 100;

                MultipleCommand.SendLogs_TimeForMessages(howManyMessages);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(howManyMessages + 1);
            }
        }

    }
}
