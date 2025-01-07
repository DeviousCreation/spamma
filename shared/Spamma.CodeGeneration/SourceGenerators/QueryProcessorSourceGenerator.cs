using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;
using QueryInputDefinition = Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors.QueryInputDefinitionProcessor.InputDefinition;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class QueryProcessorSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var queryInputDefinitionProcessor = new QueryInputDefinitionProcessor();
            var syntaxProvider = context.SyntaxProvider.CreateSyntaxProvider(
                    predicate: (syntaxNode, _) => queryInputDefinitionProcessor.CanProcess(syntaxNode),
                    transform: (ctx, _) => new Container(ctx.SemanticModel, queryInputDefinitionProcessor.Process(ctx.Node)))
                .Where(x => x.QueryInputDefinition != null);

            var combined = syntaxProvider.Collect();

            context.RegisterSourceOutput(combined, (spc, source) =>
            {
                var outputProcessor = new QueryProcessorOutputDefinitionProcessor();
                foreach (var container in source)
                {
                    var queryName = container.QueryInputDefinition.QueryDeclaration.Identifier.Text;
                    var queryNamespace = container.QueryInputDefinition.QueryDeclaration.GetNamespace();

                    var src = outputProcessor.Process(new QueryProcessorOutputDefinitionProcessor.OutputDefinition(
                        queryNamespace,
                        container.QueryInputDefinition.QueryResultDeclaration.GetNamespace(container.SemanticModel),
                        queryNamespace.Replace("Queries", "QueryProcessors"),
                        queryName,
                        container.QueryInputDefinition.QueryResultDeclaration.ToString()));

                    spc.AddSource(
                        $"{queryName}QueryProcessor.g.cs",
                        SourceText.From(src, Encoding.UTF8));
                }
            });
        }

        private sealed class Container
        {
            public Container(SemanticModel semanticModel, QueryInputDefinition? queryInputDefinition)
            {
                this.SemanticModel = semanticModel;
                this.QueryInputDefinition = queryInputDefinition;
            }

            public SemanticModel SemanticModel { get; }

            public QueryInputDefinition? QueryInputDefinition { get; }
        }
    }
}