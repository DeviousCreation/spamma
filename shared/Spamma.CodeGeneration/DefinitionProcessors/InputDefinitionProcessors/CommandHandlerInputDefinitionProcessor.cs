using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandHandlerInputDefinitionProcessor : InputDefinitionProcessor<CommandHandlerInputDefinitionProcessor.InputDefinition>
    {
        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classDeclarationSyntax))
            {
                return false;
            }

            foreach (var baseType in classDeclarationSyntax.BaseList?.Types ?? Enumerable.Empty<BaseTypeSyntax>())
            {
                if (!(baseType is PrimaryConstructorBaseTypeSyntax simpleBaseTypeSyntax))
                {
                    continue;
                }

                if (!(simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax) || genericNameSyntax.Identifier.Text != "CommandHandler" ||
                    genericNameSyntax.TypeArgumentList.Arguments.Count != 1)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            return new InputDefinition(((ClassDeclarationSyntax)syntaxNode).BaseList
                .Types.Select(type =>
                    ((GenericNameSyntax)((PrimaryConstructorBaseTypeSyntax)type).Type).TypeArgumentList.Arguments[0])
                .First());
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