using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class CapSubscribeInputDefinitionProcessor : InputDefinitionProcessor<CapSubscribeInputDefinitionProcessor.InputDefinition>
    {
        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            var classDeclarations = syntaxNode.DescendantNodes().OfType<ClassDeclarationSyntax>();
            return classDeclarations.Any(x => (x.BaseList?.Types
                .Any(t => t.ToString() == "ICapSubscribe") ?? false));
        }

        protected override InputDefinition ProcessInternal(SyntaxNode syntaxNode)
        {
            return new InputDefinition(syntaxNode.DescendantNodes().OfType<ClassDeclarationSyntax>().First());
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(ClassDeclarationSyntax classDeclaration)
            {
                this.ClassDeclaration = classDeclaration;
            }

            public ClassDeclarationSyntax ClassDeclaration { get; }
        }
    }
}