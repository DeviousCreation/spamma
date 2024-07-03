using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.OutputDefinitionProcessors
{
    internal class CommandHandlerOutputDefinitionProcessor : IOutputDefinitionProcessor<CommandHandlerOutputDefinitionProcessor.OutputDefinition>
    {
        public string Process(OutputDefinition data)
        {
            return $@"using Spamma.App.Client.Infrastructure.Contracts.Domain;
using {data.CommandNamespace};

namespace {data.Namespace};

internal class {data.CommandName}Handler(IHttpClientFactory httpClientFactory, ILogger<GenericCommandHandler<{data.CommandName}>> logger)
        : GenericCommandHandler<{data.CommandName}>(httpClientFactory, logger);";
        }

        public class OutputDefinition : IOutputDefinition
        {
            public OutputDefinition(string commandNamespace, string ns, string commandName)
            {
                this.CommandNamespace = commandNamespace;
                this.Namespace = ns;
                this.CommandName = commandName;
            }

            public string CommandNamespace { get; }

            public string Namespace { get; }

            public string CommandName { get; }
        }
    }
}