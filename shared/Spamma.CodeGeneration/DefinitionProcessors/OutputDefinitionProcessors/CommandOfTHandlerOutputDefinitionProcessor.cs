using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class CommandOfTHandlerOutputDefinitionProcessor
        : IOutputDefinitionProcessor<CommandOfTHandlerOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            return $@"using Spamma.App.Client.Infrastructure.Contracts.Domain;
using {data.CommandNamespace};
using {data.CommandResultNamespace};

namespace {data.Namespace};

internal class {data.CommandName}Handler(IHttpClientFactory httpClientFactory, ILogger<GenericCommandHandler<{data.CommandName}, {data.CommandResultName}>> logger)
        : GenericCommandHandler<{data.CommandName}, {data.CommandResultName}>(httpClientFactory, logger);";
        }

        internal class OutputDefinition : IOutputDefinition
        {
            public OutputDefinition(string commandNamespace, string commandResultNamespace, string ns, string commandName, string commandResultName)
            {
                this.CommandNamespace = commandNamespace;
                this.CommandResultNamespace = commandResultNamespace;
                this.Namespace = ns;
                this.CommandName = commandName;
                this.CommandResultName = commandResultName;
            }

            public string CommandNamespace { get; }

            public string CommandResultNamespace { get; }

            public string Namespace { get; }

            public string CommandName { get; }

            public string CommandResultName { get; }
        }
    }
}