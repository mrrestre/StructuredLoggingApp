using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Sinks.TestCorrelator;
using TestAppWithSerilog.Commands;

namespace TestAppWithSerilog.Tests.AlgoCommandTests
{
    [TestClass]
    class AlgoCommandTests
    {
        static readonly AlgoCommand algoCommand = new();

        [AssemblyInitialize]
        public static void ConfigureGlobalLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator()
                .CreateLogger();
        }

        [TestMethod]
        public void PrecisionIsCalculatedRight()
        {
            double exponent = 5;

            double precision = AlgoCommand.CalculatePrecision(exponent);

            Assert.Equals(precision, 1E-05);
        }
    }
}
