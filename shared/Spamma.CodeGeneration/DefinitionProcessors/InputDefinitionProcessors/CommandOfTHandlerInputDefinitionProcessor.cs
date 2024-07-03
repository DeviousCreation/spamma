using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandOfTHandlerInputDefinitionProcessor : IInputDefinitionProcessor<CommandOfTHandlerInputDefinitionProcessor.InputDefinition>
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
                if (!(baseType is PrimaryConstructorBaseTypeSyntax simpleBaseTypeSyntax))
                {
                    continue;
                }

                if (!(simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "CommandHandler" ||
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
            public InputDefinition(TypeSyntax commandDeclaration)
            {
                this.CommandDeclaration = commandDeclaration;
            }

            public TypeSyntax CommandDeclaration { get; }
        }
    }
}