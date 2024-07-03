using System.Collections.Generic;
using System.Text;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class ApiInitializerOutputDefinitionProcessor : IOutputDefinitionProcessor<ApiInitializerOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            var s = new StringBuilder();
            s.AppendLine("using MediatR;");
            s.AppendLine("using Spamma.App.Infrastructure.Contracts.Domain;");
            foreach (var dataNamespace in data.QueryDefinitions)
            {
                s.AppendLine($"using {dataNamespace.Namespace};");
            }

            foreach (var dataNamespace in data.CommandDefinitions)
            {
                s.AppendLine($"using {dataNamespace.Namespace};");
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
                s.AppendLine($"          ({definition.Name} query, ISender sender) => sender.Send(query));");
            }

            foreach (var definition in data.CommandDefinitions)
            {
                s.AppendLine("      app.MapPost(");
                s.AppendLine($"          \"/api/command/{definition.Namespace}.{definition.Name}\",");
                s.AppendLine($"          ({definition.Name} command, ISender sender) => sender.Send(command));");
            }

            foreach (var definition in data.CommandOfTDefinitions)
            {
                s.AppendLine("      app.MapPost(");
                s.AppendLine($"          \"/api/command/{definition.Namespace}.{definition.Name}\",");
                s.AppendLine($"          async ({definition.Name} command, ISender sender) =>");
                s.AppendLine("          {");
                s.AppendLine("              var result = await sender.Send(query);");
                s.AppendLine("              var options = new JsonSerializerOptions();");
                s.AppendLine("              options.PropertyNameCaseInsensitive = true;");
                s.AppendLine($"              options.Converters.Add(new CommandResultConverter<{definition.Name}>());");
                s.AppendLine("              return Results.Json(query, options)");
                s.AppendLine("          }");
            }

            s.AppendLine("    }");
            s.AppendLine("}");

            return s.ToString();
        }

        internal class OutputDefinition : IOutputDefinition
        {
            public OutputDefinition(string ns, List<ApiDefinition> queryDefinitions, List<ApiDefinition> commandDefinitions, List<ApiDefinition> commandOfTDefinitions)
            {
                this.Namespace = ns;
                this.QueryDefinitions = queryDefinitions;
                this.CommandDefinitions = commandDefinitions;
                this.CommandOfTDefinitions = commandOfTDefinitions;
            }

            public string Namespace { get; }

            public List<ApiDefinition> QueryDefinitions { get; }

            public List<ApiDefinition> CommandDefinitions { get; }

            public List<ApiDefinition> CommandOfTDefinitions { get; }

            internal class ApiDefinition
            {
                public ApiDefinition(string ns, string name)
                {
                    this.Namespace = ns;
                    this.Name = name;
                }

                public string Namespace { get; }

                public string Name { get; }
            }
        }
    }
}