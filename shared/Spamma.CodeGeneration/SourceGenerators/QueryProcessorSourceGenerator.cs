using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors;
using Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors;
using Spamma.CodeGeneration.Extensions;

namespace Spamma.CodeGeneration.SourceGenerators
{
    [Generator]
    public class QueryProcessorSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new QueryInterfaceSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!(context.SyntaxReceiver is QueryInterfaceSyntaxReceiver receiver))
            {
                return;
            }

            var outputProcessor = new QueryProcessorOutputDefinitionProcessor();
            foreach (var queryInputDefinition in receiver.QueryInputDefinitions)
            {
                var queryName = queryInputDefinition.QueryDeclaration.Identifier.Text;
                var queryNamespace = queryInputDefinition.QueryDeclaration.GetNamespace();

                var src = outputProcessor.Process(new QueryProcessorOutputDefinitionProcessor.OutputDefinition(
                    queryNamespace,
                    queryInputDefinition.QueryResultDeclaration.GetNamespace(context),
                    queryNamespace.Replace("Queries", "QueryProcessors"),
                    queryName,
                    queryInputDefinition.QueryResultDeclaration.ToString()));

                context.AddSource(
                    $"{queryName}QueryProcessor.g.cs",
                    SourceText.From(src, Encoding.UTF8));
            }
        }

        private sealed class QueryInterfaceSyntaxReceiver : ISyntaxReceiver
        {
            public List<QueryInputDefinitionProcessor.InputDefinition> QueryInputDefinitions { get; }
                = new List<QueryInputDefinitionProcessor.InputDefinition>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                var queryInputDefinitionProcessor = new QueryInputDefinitionProcessor();
                this.QueryInputDefinitions.AddRange(queryInputDefinitionProcessor.Process(syntaxNode));
            }
        }
    }
}