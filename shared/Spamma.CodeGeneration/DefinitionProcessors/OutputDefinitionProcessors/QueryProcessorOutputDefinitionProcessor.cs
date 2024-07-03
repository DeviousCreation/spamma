using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class QueryProcessorOutputDefinitionProcessor : IOutputDefinitionProcessor<QueryProcessorOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            return $@"using Spamma.App.Client.Infrastructure.Contracts.Querying;
using {data.QueryNamespace};
using {data.QueryResultNamespace};

namespace {data.Namespace};

internal class {data.QueryName}Processor(IHttpClientFactory factory)
        : GenericQueryProcessor<{data.QueryName}, {data.QueryResult}>(factory);";
        }

        internal class OutputDefinition : IOutputDefinition
        {
            internal OutputDefinition(string queryNamespace, string queryResultNamespace, string ns, string queryName, string queryResult)
            {
                this.QueryNamespace = queryNamespace;
                this.QueryResultNamespace = queryResultNamespace;
                this.Namespace = ns;
                this.QueryName = queryName;
                this.QueryResult = queryResult;
            }

            public string QueryNamespace { get; }

            public string QueryResultNamespace { get; }

            public string Namespace { get; }

            public string QueryName { get; }

            public string QueryResult { get; }
        }
    }
}