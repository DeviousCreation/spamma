using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class ApiInitializerInputDefinitionProcessor : IInputDefinitionProcessor<ApiInitializerInputDefinitionProcessor.InputDefinition>
    {
        public IEnumerable<InputDefinition> Process(SyntaxNode syntaxNode)
        {
            var definitions = new List<InputDefinition>();
            if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.Identifier.Text == "ApiInitializer")
            {
                definitions.Add(new InputDefinition(classDeclarationSyntax));
            }

            return definitions;
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(ClassDeclarationSyntax apiInitializerDeclaration)
            {
                this.ApiInitializerDeclaration = apiInitializerDeclaration;
            }

            public ClassDeclarationSyntax ApiInitializerDeclaration { get; }
        }
    }
}