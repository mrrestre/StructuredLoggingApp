using Serilog.Core;
using Serilog.Events;
using System;

namespace TestAppWithSerilog.Enrichers
{
    internal class UserEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "UserName", value: Environment.UserName));

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
                "UserDomainName", value: Environment.UserDomainName));
        }
    }
}