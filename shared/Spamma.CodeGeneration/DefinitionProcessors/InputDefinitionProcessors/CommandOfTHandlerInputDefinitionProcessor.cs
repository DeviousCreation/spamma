using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CommandOfTHandlerInputDefinitionProcessor : InputDefinitionProcessor<CommandOfTHandlerInputDefinitionProcessor.InputDefinition>
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
                    genericNameSyntax.TypeArgumentList.Arguments.Count != 2)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)syntaxNode;
            var t = classDeclarationSyntax.BaseList.Types.Select(type =>
                ((GenericNameSyntax)((PrimaryConstructorBaseTypeSyntax)type).Type)).First();

            return new InputDefinition(
                    t.TypeArgumentList.Arguments[0],
                    t.TypeArgumentList.Arguments[1]);
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(TypeSyntax commandDeclaration, TypeSyntax commandResultDeclaration)
            {
                this.CommandDeclaration = commandDeclaration;
                this.CommandResultDeclaration = commandResultDeclaration;
            }

            public TypeSyntax CommandDeclaration { get; }

            public TypeSyntax CommandResultDeclaration { get; }
        }
    }
}