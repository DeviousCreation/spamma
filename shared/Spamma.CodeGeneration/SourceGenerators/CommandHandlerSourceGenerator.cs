using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;
using CommandInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandInputDefinitionProcessor.InputDefinition;
using CommandOfTInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CommandOfTInputDefinitionProcessor.InputDefinition;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class CommandHandlerSourceGenerator : IIncrementalGenerator
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

            var combined = syntaxProvider.Collect();

            context.RegisterSourceOutput(combined, (spc, source) =>
            {
                var commandHandlerOutputDefinitionProcessor = new CommandHandlerOutputDefinitionProcessor();
                foreach (var commandDeclaration in source.Where(x => x.InputType == InputType.CommandInput)
                             .Select(item => item.CommandInputDefinition.CommandDeclaration))
                {
                    var commandName = commandDeclaration.Identifier.Text;
                    var commandNamespace = commandDeclaration.GetNamespace();

                    var src = commandHandlerOutputDefinitionProcessor.Process(
                        new CommandHandlerOutputDefinitionProcessor.OutputDefinition(
                            commandNamespace,
                            commandNamespace.Replace("Commands", "CommandHandlers"),
                            commandName));

                    spc.AddSource(
                        $"{commandName}Handler.g.cs",
                        SourceText.From(src, Encoding.UTF8));
                }

                var commandOfTHandlerOutputDefinitionProcessor = new CommandOfTHandlerOutputDefinitionProcessor();
                foreach (var definition in source.Where(x => x.InputType == InputType.CommandOfTInput))
                {
                    var commandName = definition.CommandOfTInputDefinition.CommandDeclaration.Identifier.Text;
                    var commandNamespace = definition.CommandOfTInputDefinition.CommandDeclaration.GetNamespace();

                    var src = commandOfTHandlerOutputDefinitionProcessor.Process(
                        new CommandOfTHandlerOutputDefinitionProcessor.OutputDefinition(
                            commandNamespace,
                            definition.CommandOfTInputDefinition.CommandResultDeclaration.GetNamespace(definition
                                .SemanticModel),
                            commandNamespace.Replace("Commands", "CommandHandlers"),
                            commandName,
                            definition.CommandOfTInputDefinition.CommandResultDeclaration.ToString()));

                    spc.AddSource(
                        $"{commandName}Handler.g.cs",
                        SourceText.From(src, Encoding.UTF8));
                }


            });
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