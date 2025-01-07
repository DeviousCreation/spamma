using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandInputDefinitionProcessor : InputDefinitionProcessor<CommandInputDefinitionProcessor.InputDefinition>
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

                if (!(simpleBaseTypeSyntax.Type is IdentifierNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "ICommand")
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            return new InputDefinition(syntaxNode as RecordDeclarationSyntax);
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