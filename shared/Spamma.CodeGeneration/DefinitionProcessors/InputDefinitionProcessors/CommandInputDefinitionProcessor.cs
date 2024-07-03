using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandInputDefinitionProcessor : IInputDefinitionProcessor<CommandInputDefinitionProcessor.InputDefinition>
    {
        public IEnumerable<InputDefinition> Process(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is RecordDeclarationSyntax classDeclarationSyntax))
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

                if (!(simpleBaseTypeSyntax.Type is IdentifierNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "ICommand")
                {
                    continue;
                }

                definitions.Add(new InputDefinition(classDeclarationSyntax));
                break;
            }

            return definitions;
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(RecordDeclarationSyntax commandDeclaration)
            {
                this.CommandDeclaration = commandDeclaration;
            }

            public RecordDeclarationSyntax CommandDeclaration { get; }
        }
    }
}