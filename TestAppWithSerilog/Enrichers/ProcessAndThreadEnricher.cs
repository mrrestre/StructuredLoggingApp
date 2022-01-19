using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace TestAppWithSerilog.Enrichers
{
    class ProcessAndThreadEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ProcessId", System.Environment.ProcessId));

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
