using Microsoft.CodeAnalysis;

using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;
using CommandInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandInputDefinitionProcessor.InputDefinition;
using CommandOfTInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandOfTInputDefinitionProcessor.InputDefinition;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class EndpointAuthorizationSourceGenerator :  IIncrementalGenerator
    {
        internal enum InputType
        {
            None,
            CommandInput,
            CommandOfTInput,
        }
        
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var commandInputDefinitionProcessor = new CommandInputDefinitionProcessor();
            var commandOfTInputDefinitionProcessor = new CommandOfTInputDefinitionProcessor();
            
            var syntaxProvider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (syntaxNode, _) =>
                        commandInputDefinitionProcessor.CanProcess(syntaxNode) ||
                        commandOfTInputDefinitionProcessor.CanProcess(syntaxNode),
                    transform: (syntaxContext, _) => new Container(
                        syntaxContext.SemanticModel,
                        commandInputDefinitionProcessor.Process(syntaxContext.Node),
                        commandOfTInputDefinitionProcessor.Process(syntaxContext.Node)))
                .Where(result => result.InputType != InputType.None);
        }
        
        private sealed class Container
        {
            private readonly CommandInputDefinition? _commandInputDefinition;
            private readonly CommandOfTInputDefinition? _commandOfTInputDefinition;

            public Container(SemanticModel semanticModel, CommandInputDefinition? commandInputDefinition, CommandOfTInputDefinition? commandOfTInputDefinition)
            {
                this.SemanticModel = semanticModel;
                if (commandInputDefinition != default(CommandInputDefinition))
                {
                    this.InputType = InputType.CommandInput;
                    this._commandInputDefinition = commandInputDefinition;
                }
                else if (commandOfTInputDefinition != default(CommandOfTInputDefinition))
                {
                    this.InputType = InputType.CommandOfTInput;
                    this._commandOfTInputDefinition = commandOfTInputDefinition;
                }
                else
                {
                    this.InputType = InputType.None;
                }
            }

            public InputType InputType { get; }

            public SemanticModel SemanticModel { get; }

            public CommandInputDefinition CommandInputDefinition
            {
                get
                {
                    if (this.InputType == InputType.CommandInput)
                    {
                        return this._commandInputDefinition!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }

            public CommandOfTInputDefinition CommandOfTInputDefinition
            {
                get
                {
                    if (this.InputType == InputType.CommandOfTInput)
                    {
                        return this._commandOfTInputDefinition!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }
        }
    }
}