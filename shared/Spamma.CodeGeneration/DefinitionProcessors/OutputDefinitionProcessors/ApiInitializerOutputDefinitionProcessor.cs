using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class ApiInitializerOutputDefinitionProcessor : IOutputDefinitionProcessor<ApiInitializerOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            var s = new StringBuilder();
            s.AppendLine("using System.Text.Json;");
            s.AppendLine("using Spamma.App.Client.Infrastructure.Contracts.Querying;");
            s.AppendLine("using Spamma.App.Infrastructure.Contracts.Domain;");
            s.AppendLine("using Spamma.App.Client.Infrastructure.Contracts.Domain;");
            var namespaces = new List<string>();
            foreach (var dataNamespace in data.QueryDefinitions)
            {
                namespaces.Add($"using {dataNamespace.Namespace};");
            }

            foreach (var dataNamespace in data.CommandDefinitions)
            {
                namespaces.Add($"using {dataNamespace.Namespace};");
            }

            foreach (var dataNamespace in data.CommandOfTDefinitions)
            {
                namespaces.Add($"using {dataNamespace.CommandNamespace};");
                namespaces.Add($"using {dataNamespace.CommandResultNamespace};");
            }

            foreach (var ns in namespaces.Distinct())
            {
                s.AppendLine(ns);
            }

            s.AppendLine();
            s.AppendLine($"namespace {data.Namespace};");
            s.AppendLine();
            s.AppendLine("public static partial class ApiInitializer");
            s.AppendLine("{");
            s.AppendLine("    public static partial void ConfigureApi(this WebApplication app)");
            s.AppendLine("    {");
            foreach (var definition in data.QueryDefinitions)
            {
                s.AppendLine("      app.MapPost(");
                s.AppendLine($"          \"/api/query/{definition.Namespace}.{definition.Name}\",");
                s.AppendLine($"          ({definition.Name} query, IQuerier sender) => sender.Send(query));");
            }

            foreach (var definition in data.CommandDefinitions)
            {
                s.AppendLine("      app.MapPost(");
                s.AppendLine($"          \"/api/command/{definition.Namespace}.{definition.Name}\",");
                s.AppendLine($"          ({definition.Name} command, ICommander sender) => sender.Send(command));");
            }

            foreach (var definition in data.CommandOfTDefinitions)
            {
                s.AppendLine("      app.MapPost(");
                s.AppendLine($"          \"/api/command/{definition.CommandNamespace}.{definition.CommandName}\",");
                s.AppendLine($"          async ({definition.CommandName} command, ICommander sender) =>");
                s.AppendLine("          {");
                s.AppendLine($"              var result = await sender.Send<{definition.CommandName}, {definition.CommandResultName}>(command);");
                s.AppendLine("              var options = new JsonSerializerOptions();");
                s.AppendLine("              options.PropertyNameCaseInsensitive = true;");
                s.AppendLine($"             options.Converters.Add(new CommandResultConverter<{definition.CommandName}>());");
                s.AppendLine("              return Results.Json(command, options);");
                s.AppendLine("          });");
            }

            s.AppendLine("    }");
            s.AppendLine("}");

            return s.ToString();
        }

        internal class OutputDefinition : IOutputDefinition
        {
            public OutputDefinition(string ns, List<QueryDefinition> queryDefinitions,
                List<CommandDefinition> commandDefinitions, List<CommandOfTDefinition> commandOfTDefinitions)
            {
                this.Namespace = ns;
                this.QueryDefinitions = queryDefinitions;
                this.CommandDefinitions = commandDefinitions;
                this.CommandOfTDefinitions = commandOfTDefinitions;
            }

            public string Namespace { get; }

            public List<QueryDefinition> QueryDefinitions { get; }

            public List<CommandDefinition> CommandDefinitions { get; }

            public List<CommandOfTDefinition> CommandOfTDefinitions { get; }

            internal class QueryDefinition
            {
                public QueryDefinition(string ns, string name)
                {
                    this.Namespace = ns;
                    this.Name = name;
                }

                public string Namespace { get; }

                public string Name { get; }
            }

            internal class CommandDefinition
            {
                public CommandDefinition(string ns, string name)
                {
                    this.Namespace = ns;
                    this.Name = name;
                }

                public string Namespace { get; }

                public string Name { get; }
            }

            internal class CommandOfTDefinition
            {
                public CommandOfTDefinition(string commandNamespace, string commandName, string commandResultNamespace, string commandResultName)
                {
                    this.CommandNamespace = commandNamespace;
                    this.CommandName = commandName;
                    this.CommandResultNamespace = commandResultNamespace;
                    this.CommandResultName = commandResultName;
                }

                public string CommandNamespace { get; }

                public string CommandName { get; }

                public string CommandResultNamespace { get; }

                public string CommandResultName { get; }
            }
        }
    }
}