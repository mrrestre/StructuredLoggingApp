using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;

namespace TestAppWithSerilog.Tests
{
    [TestClass]
    public class GlobalTestInitializer
    {
        [AssemblyInitialize()]
        public static void ConfigureGlobalLogger(TestContext testContext)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.TestCorrelator()
                .WriteTo.Console()
                .MinimumLevel.Verbose()
                .CreateLogger();
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            // The test framework will call this method once -AFTER- each test run.
        }
    }
}
