using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class QueryProcessorInputDefinitionProcessor : IInputDefinitionProcessor<QueryProcessorInputDefinitionProcessor.InputDefinition>
    {
        public IEnumerable<InputDefinition> Process(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classDeclarationSyntax))
            {
                return Array.Empty<InputDefinition>();
            }

            var definitions = new List<InputDefinition>();
            foreach (var baseType in classDeclarationSyntax.BaseList?.Types ?? Enumerable.Empty<BaseTypeSyntax>())
            {
                if (!(baseType is SimpleBaseTypeSyntax simpleBaseTypeSyntax))
                {
                    continue;
                }

                if (!(simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "IQueryProcessor" ||
                    genericNameSyntax.TypeArgumentList.Arguments.Count != 2)
                {
                    continue;
                }

                definitions.Add(new InputDefinition(
                    genericNameSyntax.TypeArgumentList.Arguments[0]));
                break;
            }

            return definitions;
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(TypeSyntax queryDeclaration)
            {
                this.QueryDeclaration = queryDeclaration;
            }

            public TypeSyntax QueryDeclaration { get; }
        }
    }
}