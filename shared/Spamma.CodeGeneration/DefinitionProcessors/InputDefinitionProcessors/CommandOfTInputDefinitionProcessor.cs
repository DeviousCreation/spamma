using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandOfTInputDefinitionProcessor : InputDefinitionProcessor<CommandOfTInputDefinitionProcessor.InputDefinition>
    {
        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is RecordDeclarationSyntax classDeclarationSyntax))
            {
                return false;
            }

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

                return true;
            }

            return false;
        }

        protected override InputDefinition? ProcessInternal(SyntaxNode syntaxNode)
        {
            return ((RecordDeclarationSyntax)syntaxNode).BaseList?.Types.Select(type =>
                ((GenericNameSyntax)((SimpleBaseTypeSyntax)type).Type)).First() is { } t
                ? new InputDefinition(
                    (syntaxNode as RecordDeclarationSyntax)!,
                    t.TypeArgumentList.Arguments[0])
                : null;
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