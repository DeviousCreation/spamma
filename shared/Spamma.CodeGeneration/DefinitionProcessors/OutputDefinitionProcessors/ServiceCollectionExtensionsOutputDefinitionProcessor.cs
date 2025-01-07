using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class ServiceCollectionExtensionsOutputDefinitionProcessor : IOutputDefinitionProcessor<ServiceCollectionExtensionsOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            var s = new StringBuilder();
            var namespaces = new List<string>();
            s.AppendLine("using DotNetCore.CAP;");

            foreach (var dataNamespace in data.EventSubscriptionDefinitions)
            {
                namespaces.Add($"using {dataNamespace.Namespace};");
            }

            foreach (var ns in namespaces.Distinct())
            {
                s.AppendLine(ns);
            }

            s.AppendLine();
            s.AppendLine($"namespace {data.Namespace};");
            s.AppendLine();
            s.AppendLine("internal static partial class ServiceCollectionExtensions");
            s.AppendLine("{");
            s.AppendLine("    internal static partial IServiceCollection RegisterEventSubscribers(this IServiceCollection services)");
            s.AppendLine("    {");
            foreach (var eventSubscription in data.EventSubscriptionDefinitions.Select(x => x.Name).Distinct())
            {
                s.AppendLine($"      services.AddScoped<ICapSubscribe, {eventSubscription}>();");
            }

            s.AppendLine("      return services;");
            s.AppendLine("    }");
            s.AppendLine("}");

            return s.ToString();
        }

        internal class OutputDefinition : IOutputDefinition
        {
            public OutputDefinition(string ns, List<EventSubscriptionDefinition> eventSubscriptionDefinitions)
            {
                this.Namespace = ns;
                this.EventSubscriptionDefinitions = eventSubscriptionDefinitions;
            }

            public string Namespace { get; }

            public List<EventSubscriptionDefinition> EventSubscriptionDefinitions { get; }

            public class EventSubscriptionDefinition
            {
                public EventSubscriptionDefinition(string name, string ns)
                {
                    this.Name = name;
                    this.Namespace = ns;
                }

                public string Name { get; }

                public string Namespace { get; }
            }
        }
    }
}