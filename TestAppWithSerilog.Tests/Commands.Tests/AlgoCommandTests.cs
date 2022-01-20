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
        private AlgoCommand algoCommand = new();

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
        public void WithAQuadraticNumber_ExactlyOneIterationIsNeeded_AndOneMessageLogGenerated()
        {
            algoCommand.Value = 49;
            algoCommand.Precision = AlgoCommand.CalculatePrecision(5);

            using (TestCorrelator.CreateContext())
            {
                algoCommand.RunAlgorithm(true);

                TestCorrelator.GetLogEventsFromCurrentContext()
                    .Should().HaveCount(1);
            }
        }

        
    }
}
