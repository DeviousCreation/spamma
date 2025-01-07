using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;
using CapSubscribeInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.CapSubscribeInputDefinitionProcessor.InputDefinition;
using ServiceCollectionExtensionsInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.ServiceCollectionExtensionsInputDefinitionProcessor.InputDefinition;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class ServiceCollectionExtensionsSourceGenerator : IIncrementalGenerator
    {
        internal enum InputType
        {
            None,
            ServiceCollectionExtensions,
            CapSubscribe,
        }

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            //Debugger.Launch();
            var serviceCollectionExtensionsInputDefinitionProcessor =
                new ServiceCollectionExtensionsInputDefinitionProcessor();
            var capSubscribeInputDefinitionProcessor = new CapSubscribeInputDefinitionProcessor();
            var syntaxProvider = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: (syntaxNode, _) =>
                        serviceCollectionExtensionsInputDefinitionProcessor.CanProcess(syntaxNode) ||
                        capSubscribeInputDefinitionProcessor.CanProcess(syntaxNode),
                    transform: (syntaxContext, _) => new Container(
                        syntaxContext.SemanticModel,
                        capSubscribeInputDefinitionProcessor.Process(syntaxContext.Node),
                        serviceCollectionExtensionsInputDefinitionProcessor.Process(syntaxContext.Node)))
                .Where(result => result.InputType != InputType.None);

            var combined = syntaxProvider.Collect();

            context.RegisterSourceOutput(combined, (spc, source) =>
            {
                var serviceCollectionExtensions = source.SingleOrDefault(x => x.InputType == InputType.ServiceCollectionExtensions);
                if (serviceCollectionExtensions == null)
                {
                    return;
                }

                var capSubscribeInputDefinitions = source
                    .Where(x => x.InputType == InputType.CapSubscribe)
                    .Select(item => new ServiceCollectionExtensionsOutputDefinitionProcessor.OutputDefinition.EventSubscriptionDefinition(
                        item.CapSubscribe.ClassDeclaration.Identifier.Text,
                        item.CapSubscribe.ClassDeclaration.GetNamespace())).ToList();

                var outputDefinition = new ServiceCollectionExtensionsOutputDefinitionProcessor.OutputDefinition(
                    serviceCollectionExtensions.ServiceCollectionExtensions.ServiceCollectionExtensionsDeclaration.GetNamespace(),
                    capSubscribeInputDefinitions);

                spc.AddSource(
                    $"ServiceCollectionExtensions.g.cs",
                    SourceText.From(
                        new ServiceCollectionExtensionsOutputDefinitionProcessor().Process(outputDefinition),
                        Encoding.UTF8));
            });
        }

        private sealed class Container
        {
            private readonly CapSubscribeInputDefinition? _capSubscribeInputDefinition;
            private readonly ServiceCollectionExtensionsInputDefinition? _serviceCollectionExtensionsInputDefinition;

            public Container(SemanticModel semanticModel, CapSubscribeInputDefinition? capSubscribeInputDefinition, ServiceCollectionExtensionsInputDefinition? serviceCollectionExtensionsInputDefinition)
            {
                this.SemanticModel = semanticModel;
                if (capSubscribeInputDefinition != default(CapSubscribeInputDefinition))
                {
                    this.InputType = InputType.CapSubscribe;
                    this._capSubscribeInputDefinition = capSubscribeInputDefinition;
                }
                else if (serviceCollectionExtensionsInputDefinition != default(ServiceCollectionExtensionsInputDefinition))
                {
                    this.InputType = InputType.ServiceCollectionExtensions;
                    this._serviceCollectionExtensionsInputDefinition = serviceCollectionExtensionsInputDefinition;
                }
                else
                {
                    this.InputType = InputType.None;
                }
            }

            public InputType InputType { get; }

            public SemanticModel SemanticModel { get; }

            public CapSubscribeInputDefinition CapSubscribe
            {
                get
                {
                    if (this.InputType == InputType.CapSubscribe)
                    {
                        return this._capSubscribeInputDefinition!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }

            public ServiceCollectionExtensionsInputDefinition ServiceCollectionExtensions
            {
                get
                {
                    if (this.InputType == InputType.ServiceCollectionExtensions)
                    {
                        return this._serviceCollectionExtensionsInputDefinition!;
                    }

                    throw new System.InvalidOperationException("Invalid input type");
                }
            }
        }
    }
}