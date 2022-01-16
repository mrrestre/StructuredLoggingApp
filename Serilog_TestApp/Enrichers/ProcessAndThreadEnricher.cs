using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace TestApp.Enrichers
{
    class ProcessAndThreadEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ProcessId", System.Diagnostics.Process.GetCurrentProcess().Id));

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ThreadId", Thread.CurrentThread.ManagedThreadId));
        }
    }
}
