namespace Spamma.CodeGeneration.Contracts
{
    internal interface IOutputDefinitionProcessor<in TDefinition>
        where TDefinition : IOutputDefinition
    {
        string Process(TDefinition data);
    }
}