using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog.Sinks.TestCorrelator;
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
            // Given
            double exponent = 5;

            // Calculate
            double precision = AlgoCommand.CalculatePrecision(exponent);

            // Ensure
            Assert.AreEqual(expected: 1E-05, actual: precision);
        }

        [TestMethod]
        public void ResultOfTheCalculationIsReliable()
        {
            // Given
            algoCommand.value = 25;
            algoCommand.precision = AlgoCommand.CalculatePrecision(5);

            // Calculate
            var resultStruct = AlgoCommand.RunAlgorithm(false, algoCommand.value, algoCommand.precision);

            // Ensure
            Assert.AreEqual(expected: resultStruct.result, actual: 5);
        }

        [TestMethod]
        public void ForTheValue16AndAPrecisionOf3_Exactly14IterationsAreNeeded_Therefor14MessagesAreGenerated()
        {
            // Given
            algoCommand.value = 16;
            algoCommand.precision = AlgoCommand.CalculatePrecision(3);

            using (TestCorrelator.CreateContext())
            {
                // Calculate
                AlgoCommand.RunAlgorithm(true, algoCommand.value, algoCommand.precision);

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(14);
            }
        }

        [TestMethod]
        public void ForAExtremelyBigNumberAndABigPrecision_TheAlgorithmBreaksAfter_GivenMaximumLoop_AndGeneratesMaximumLoopMessages()
        {
            // Given
            algoCommand.value = 5296541235896522145;
            algoCommand.precision = AlgoCommand.CalculatePrecision(15);

            using (TestCorrelator.CreateContext())
            {
                // Calculate
                AlgoCommand.RunAlgorithm(true, algoCommand.value, algoCommand.precision);

                // Ensure
                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(AlgoCommand.maximumLoops);
            }
        }
    }
}