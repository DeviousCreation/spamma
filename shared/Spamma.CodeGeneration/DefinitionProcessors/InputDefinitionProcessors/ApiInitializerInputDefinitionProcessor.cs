using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class ApiInitializerInputDefinitionProcessor : InputDefinitionProcessor<ApiInitializerInputDefinitionProcessor.InputDefinition>
    {
        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is ClassDeclarationSyntax classDeclaration))
            {
                return false;
            }

            return classDeclaration.Identifier.Text == "ApiInitializer";
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            return new InputDefinition(syntaxNode as ClassDeclarationSyntax);
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