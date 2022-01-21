using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using System;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.Commands.Tests
{
    [TestClass]
    public class AlgoCommandTests
    {
        private readonly AlgoCommand algoCommand = new();

        [TestMethod]
        public void PrecisionIsCalculatedRight()
        {
            double exponent = 5;

            double precision = AlgoCommand.CalculatePrecision(exponent);

            Assert.AreEqual(precision, 1E-05);
        }

        [TestMethod]
        public void ResultOfTheCalculationIsReliable()
        {
            algoCommand.Value = 25;
            algoCommand.Precision = AlgoCommand.CalculatePrecision(5);

            var result = algoCommand.RunAlgorithm(false);

            Assert.AreEqual(result, 5);
        }

        [TestMethod]
        public void ForTheValue16AndAPrecisionOf3_Exactly14IterationsAreNeeded_Therefor15MessagesAreGenerated()
        {
            algoCommand.Value = 16;
            algoCommand.Precision = AlgoCommand.CalculatePrecision(3);

            using (TestCorrelator.CreateContext())
            {
                algoCommand.RunAlgorithm(true);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(15);
            }
        }

        [TestMethod]
        public void ForAExtremelyBigNumberAndABigPrecision_TheAlgorithmBreaksAfter_GivenMaximumLoop_AndGeneratesMaximumLoopPlusOneMessages()
        {
            algoCommand.Value = 5296541235896522145;
            algoCommand.Precision = AlgoCommand.CalculatePrecision(15);

            using (TestCorrelator.CreateContext())
            {
                algoCommand.RunAlgorithm(true);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(AlgoCommand.maximumLoops + 1);
            }
        }
    }
}
