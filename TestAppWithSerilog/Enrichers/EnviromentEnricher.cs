using Serilog.Core;
using Serilog.Events;

namespace TestAppWithSerilog.Enrichers
{
    class EnviromentEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "ApplicationName", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name));
        }
    }
}
