using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandOfTInputDefinitionProcessor : IInputDefinitionProcessor<CommandOfTInputDefinitionProcessor.InputDefinition>
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

                if (!(simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "ICommand" ||
                    genericNameSyntax.TypeArgumentList.Arguments.Count != 1)
                {
                    continue;
                }

                definitions.Add(new InputDefinition(classDeclarationSyntax, genericNameSyntax.TypeArgumentList.Arguments[0]));
                break;
            }

            return definitions;
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(RecordDeclarationSyntax commandDeclaration, TypeSyntax commandResultDeclaration)
            {
                this.CommandDeclaration = commandDeclaration;
                this.CommandResultDeclaration = commandResultDeclaration;
            }

            public RecordDeclarationSyntax CommandDeclaration { get; }

            public TypeSyntax CommandResultDeclaration { get; }
        }
    }
}