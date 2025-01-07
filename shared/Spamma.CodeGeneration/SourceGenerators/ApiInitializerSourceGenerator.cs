using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;
using ApiInitializerInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.ApiInitializerInputDefinitionProcessor.InputDefinition;
using CommandHandlerInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandHandlerInputDefinitionProcessor.InputDefinition;
using CommandOfTHandlerInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandOfTHandlerInputDefinitionProcessor.InputDefinition;
using QueryProcessorInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.QueryProcessorInputDefinitionProcessor.InputDefinition;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class ApiInitializerSourceGenerator : IIncrementalGenerator
    {
        private enum InputType
        {
            None,
            ApiInitializer,
            QueryProcessor,
            CommandHandler,
            CommandOfTHandler,
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var apiInitializerInputDefinitionProcessor = new ApiInitializerInputDefinitionProcessor();
            var queryProcessorInputDefinitionProcessor = new QueryProcessorInputDefinitionProcessor();
            var commandHandlerInputDefinitionProcessor = new CommandHandlerInputDefinitionProcessor();
            var commandOfTHandlerInputDefinitionProcessor = new CommandOfTHandlerInputDefinitionProcessor();

            var syntaxProvider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (syntaxNode, _) =>
                        apiInitializerInputDefinitionProcessor.CanProcess(syntaxNode) ||
                        queryProcessorInputDefinitionProcessor.CanProcess(syntaxNode) ||
                        commandHandlerInputDefinitionProcessor.CanProcess(syntaxNode) ||
                        commandOfTHandlerInputDefinitionProcessor.CanProcess(syntaxNode),
                    transform: (ctx, _) => new Container(
                        ctx.SemanticModel,
                        apiInitializerInputDefinitionProcessor.Process(ctx.Node),
                        queryProcessorInputDefinitionProcessor.Process(ctx.Node),
                        commandHandlerInputDefinitionProcessor.Process(ctx.Node),
                        commandOfTHandlerInputDefinitionProcessor.Process(ctx.Node)))
                .Where(result => result.InputType != InputType.None);

            var combined = syntaxProvider.Collect();

            context.RegisterSourceOutput(combined, (spc, source) =>
            {
                var apiInputDefinition = source.SingleOrDefault(x => x.InputType == InputType.ApiInitializer);
                if (apiInputDefinition == null)
                {
                    return;
                }

                var queries = source.Where(x => x.InputType == InputType.QueryProcessor)
                        .Select(item => new ApiInitializerOutputDefinitionProcessor.OutputDefinition.QueryDefinition(
                        item.QueryProcessor.QueryDeclaration.GetNamespace(item.SemanticModel), item.QueryProcessor.QueryDeclaration.ToString())).ToList();

                var commands = source.Where(x => x.InputType == InputType.CommandHandler)
                       .Select(item => new ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandDefinition(
                           item.CommandHandler.CommandDeclaration.GetNamespace(item.SemanticModel),
                           item.CommandHandler.CommandDeclaration.ToString())).ToList();

                var commandOfTs = source.Where(x => x.InputType == InputType.CommandOfTHandler)
                         .Select(item => new ApiInitializerOutputDefinitionProcessor.OutputDefinition.CommandOfTDefinition(
                             item.CommandOfTHandler.CommandDeclaration.GetNamespace(item.SemanticModel),
                             item.CommandOfTHandler.CommandDeclaration.ToString(),
                             item.CommandOfTHandler.CommandResultDeclaration.GetNamespace(item.SemanticModel),
                             item.CommandOfTHandler.CommandResultDeclaration.ToString())).ToList();

                var outputDefinition = new ApiInitializerOutputDefinitionProcessor.OutputDefinition(
                    apiInputDefinition.Api.ApiInitializerDeclaration.GetNamespace(),
                    queries,
                    commands,
                    commandOfTs);

                spc.AddSource(
                $"ApiInitializer.g.cs",
                SourceText.From(
                    new ApiInitializerOutputDefinitionProcessor().Process(outputDefinition),
                    Encoding.UTF8));
            });
        }

        private sealed class Container
        {
            private readonly ApiInitializerInputDefinition? _api;
            private readonly QueryProcessorInputDefinition? _queryProcessor;
            private readonly CommandHandlerInputDefinition? _commandHandler;
            private readonly CommandOfTHandlerInputDefinition? _commandOfTHandler;

            public Container(
                SemanticModel semanticModel,
                ApiInitializerInputDefinition? api,
                QueryProcessorInputDefinition? queryProcessor,
                CommandHandlerInputDefinition? commandHandler,
                CommandOfTHandlerInputDefinition? commandOfTHandler)
            {
                this.SemanticModel = semanticModel;
                if (api != default(ApiInitializerInputDefinition))
                {
                    this.InputType = InputType.ApiInitializer;
                    this._api = api;
                }
                else if (queryProcessor != default(QueryProcessorInputDefinition))
                {
                    this.InputType = InputType.QueryProcessor;
                    this._queryProcessor = queryProcessor;
                }
                else if (commandHandler != default(CommandHandlerInputDefinition))
                {
                    this.InputType = InputType.CommandHandler;
                    this._commandHandler = commandHandler;
                }
                else if (commandOfTHandler != default(CommandOfTHandlerInputDefinition))
                {
                    this.InputType = InputType.CommandOfTHandler;
                    this._commandOfTHandler = commandOfTHandler;
                }
                else
                {
                    this.InputType = InputType.None;
                }
            }

            public InputType InputType { get; }

            public SemanticModel SemanticModel { get; }

            public ApiInitializerInputDefinition Api
            {
                get
                {
                    if (this.InputType == InputType.ApiInitializer)
                    {
                        return this._api!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }

            public QueryProcessorInputDefinition QueryProcessor
            {
                get
                {
                    if (this.InputType == InputType.QueryProcessor)
                    {
                        return this._queryProcessor!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }

            public CommandHandlerInputDefinition CommandHandler
            {
                get
                {
                    if (this.InputType == InputType.CommandHandler)
                    {
                        return this._commandHandler!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }

            public CommandOfTHandlerInputDefinition CommandOfTHandler
            {
                get
                {
                    if (this.InputType == InputType.CommandOfTHandler)
                    {
                        return this._commandOfTHandler!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }
        }
    }
}