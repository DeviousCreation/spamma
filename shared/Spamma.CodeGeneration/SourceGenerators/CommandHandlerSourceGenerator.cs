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
    public class CommandHandlerSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new CommandInterfaceSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is CommandInterfaceSyntaxReceiver receiver))
            {
                return;
            }

            var commandHandlerOutputDefinitionProcessor = new CommandHandlerOutputDefinitionProcessor();
            foreach (var commandDeclaration in receiver.CommandDefinitions.Select(x => x.CommandDeclaration))
            {
                var commandName = commandDeclaration.Identifier.Text;
                var commandNamespace = commandDeclaration.GetNamespace();

                var src = commandHandlerOutputDefinitionProcessor.Process(new CommandHandlerOutputDefinitionProcessor.OutputDefinition(
                    commandNamespace,
                    commandNamespace.Replace("Commands", "CommandHandlers"),
                    commandName));

                context.AddSource(
                    $"{commandName}Handler.g.cs",
                    SourceText.From(src, Encoding.UTF8));
            }

            var commandOfTHandlerOutputDefinitionProcessor = new CommandOfTHandlerOutputDefinitionProcessor();
            foreach (var definition in receiver.CommandOfTDefinitions)
            {
                var commandName = definition.CommandDeclaration.Identifier.Text;
                var commandNamespace = definition.CommandDeclaration.GetNamespace();

                var src = commandOfTHandlerOutputDefinitionProcessor.Process(new CommandOfTHandlerOutputDefinitionProcessor.OutputDefinition(
                    commandNamespace,
                    definition.CommandResultDeclaration.GetNamespace(context),
                    commandNamespace.Replace("Commands", "CommandHandlers"),
                    commandName,
                    definition.CommandResultDeclaration.ToString()));

                context.AddSource(
                    $"{commandName}Handler.g.cs",
                    SourceText.From(src, Encoding.UTF8));
            }
        }

        private sealed class CommandInterfaceSyntaxReceiver : ISyntaxReceiver
        {
            public List<CommandInputDefinitionProcessor.InputDefinition> CommandDefinitions { get; }
                = new List<CommandInputDefinitionProcessor.InputDefinition>();

            public List<CommandOfTInputDefinitionProcessor.InputDefinition> CommandOfTDefinitions { get; }
                = new List<CommandOfTInputDefinitionProcessor.InputDefinition>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                this.CommandDefinitions.AddRange(new CommandInputDefinitionProcessor().Process(syntaxNode));
                this.CommandOfTDefinitions.AddRange(new CommandOfTInputDefinitionProcessor().Process(syntaxNode));
            }
        }
    }
}