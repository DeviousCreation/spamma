using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Spamma.CodeGeneration.Contracts
{
    internal interface IInputDefinitionProcessor<out TDefinition>
        where TDefinition : IInputDefinition
    {
        IEnumerable<TDefinition> Process(SyntaxNode syntaxNode);
    }
}