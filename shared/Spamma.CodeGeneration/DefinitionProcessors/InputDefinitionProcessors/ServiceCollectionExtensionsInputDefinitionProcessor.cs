using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class ServiceCollectionExtensionsInputDefinitionProcessor : InputDefinitionProcessor<ServiceCollectionExtensionsInputDefinitionProcessor.InputDefinition>
    {
        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            return syntaxNode is ClassDeclarationSyntax classDeclaration &&
                   classDeclaration.Identifier.Text == "ServiceCollectionExtensions";
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            return new InputDefinition(syntaxNode as ClassDeclarationSyntax);
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(ClassDeclarationSyntax serviceCollectionExtensionsDeclaration)
            {
                this.ServiceCollectionExtensionsDeclaration = serviceCollectionExtensionsDeclaration;
            }

            public ClassDeclarationSyntax ServiceCollectionExtensionsDeclaration { get; }
        }
    }
}