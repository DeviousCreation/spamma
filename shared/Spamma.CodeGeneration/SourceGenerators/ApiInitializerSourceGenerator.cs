using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class ApiInitializerSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ApiInitializerSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is ApiInitializerSyntaxReceiver receiver))
            {
                return;
            }

            if (receiver.ApiInputDefinitions.Count != 1)
            {
                return;
            }

            var apiInputDefinition = receiver.ApiInputDefinitions[0];

            var queries = new List<ApiInitializerOutputDefinitionProcessor.OutputDefinition.QueryDefinition>();
            foreach (var queryProcessorInputDefinition in receiver.QueryProcessorInputDefinitions.Select(x => x.QueryDeclaration))
            {
                queries.Add(new ApiInitializerOutputDefinitionProcessor.OutputDefinition.QueryDefinition(
                    queryProcessorInputDefinition.GetNamespace(context),
                    queryProcessorInputDefinition.ToString()));
            }

            var commands = new List<ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandDefinition>();
            foreach (var queryProcessorInputDefinition in receiver.CommandHandlerInputDefinitions.Select(x => x.CommandDeclaration))
            {
                commands.Add(new ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandDefinition(
                    queryProcessorInputDefinition.GetNamespace(context),
                    queryProcessorInputDefinition.ToString()));
            }

            var commandOfTs = new List<ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandOfTDefinition>();
            foreach (var queryProcessorInputDefinition in receiver.CommandOfTHandlerInputDefinitions)
            {
                commandOfTs.Add(new ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandOfTDefinition(
                    queryProcessorInputDefinition.CommandDeclaration.GetNamespace(context),
                    queryProcessorInputDefinition.CommandDeclaration.ToString(),
                    queryProcessorInputDefinition.CommandResultDeclaration.GetNamespace(context),
                    queryProcessorInputDefinition.CommandResultDeclaration.ToString()));
            }

            var outputDefinition = new ApiInitializerOutputDefinitionProcessor.OutputDefinition(
                apiInputDefinition.ApiInitializerDeclaration.GetNamespace(),
                queries,
                commands,
                commandOfTs);

            context.AddSource(
                $"ApiInitializer.g.cs",
                SourceText.From(new ApiInitializerOutputDefinitionProcessor().Process(outputDefinition), Encoding.UTF8));
        }

        private sealed class ApiInitializerSyntaxReceiver : ISyntaxReceiver
        {
            public List<ApiInitializerInputDefinitionProcessor.InputDefinition> ApiInputDefinitions { get; }
                = new List<ApiInitializerInputDefinitionProcessor.InputDefinition>();

            public List<QueryProcessorInputDefinitionProcessor.InputDefinition> QueryProcessorInputDefinitions { get; }
                = new List<QueryProcessorInputDefinitionProcessor.InputDefinition>();

            public List<CommandHandlerInputDefinitionProcessor.InputDefinition> CommandHandlerInputDefinitions { get; }
                = new List<CommandHandlerInputDefinitionProcessor.InputDefinition>();

            public List<CommandOfTHandlerInputDefinitionProcessor.InputDefinition> CommandOfTHandlerInputDefinitions { get; }
                = new List<CommandOfTHandlerInputDefinitionProcessor.InputDefinition>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                this.ApiInputDefinitions.AddRange(new ApiInitializerInputDefinitionProcessor().Process(syntaxNode));
                this.QueryProcessorInputDefinitions.AddRange(new QueryProcessorInputDefinitionProcessor().Process(syntaxNode));
                this.CommandHandlerInputDefinitions.AddRange(new CommandHandlerInputDefinitionProcessor().Process(syntaxNode));
                this.CommandOfTHandlerInputDefinitions.AddRange(new CommandOfTHandlerInputDefinitionProcessor().Process(syntaxNode));
            }
        }
    }
}