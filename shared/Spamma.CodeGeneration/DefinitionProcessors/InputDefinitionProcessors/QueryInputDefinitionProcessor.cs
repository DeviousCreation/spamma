using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Spamma.CodeGeneration.Contracts;

namespace Spamma.CodeGeneration.DefinitionProcessors.InputDefinitionProcessors
{
    internal class QueryInputDefinitionProcessor : InputDefinitionProcessor<QueryInputDefinitionProcessor.InputDefinition>
    {
        protected override InputDefinition? ProcessInternal(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not RecordDeclarationSyntax classDeclarationSyntax)
            {
                return null;
            }

            foreach (var baseType in classDeclarationSyntax.BaseList?.Types ?? Enumerable.Empty<BaseTypeSyntax>())
            {
                if (baseType is SimpleBaseTypeSyntax simpleBaseTypeSyntax &&
                    simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax1 &&
                    !classDeclarationSyntax.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.AbstractKeyword)) &&
                    genericNameSyntax1.Identifier.Text == "IQuery" &&
                    genericNameSyntax1.TypeArgumentList.Arguments.Count == 1)
                {
                    return new InputDefinition(classDeclarationSyntax, genericNameSyntax1.TypeArgumentList.Arguments[0]);
                    break;
                }

                if (baseType is PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax &&
                    primaryConstructorBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax &&
                    genericNameSyntax.Identifier.Text == "GridParams" &&
                    genericNameSyntax.TypeArgumentList.Arguments.Count == 1)
                {
                    return new InputDefinition(classDeclarationSyntax, genericNameSyntax.TypeArgumentList.Arguments[0]);
                }
            }

            return null;
        }

        public override bool CanProcess(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is RecordDeclarationSyntax classDeclarationSyntax))
            {
                return false;
            }

            foreach (var baseType in classDeclarationSyntax.BaseList?.Types ?? Enumerable.Empty<BaseTypeSyntax>())
            {
                if (baseType is SimpleBaseTypeSyntax simpleBaseTypeSyntax &&
                    simpleBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax1 &&
                    !classDeclarationSyntax.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.AbstractKeyword)) &&
                    genericNameSyntax1.Identifier.Text == "IQuery" &&
                    genericNameSyntax1.TypeArgumentList.Arguments.Count == 1)
                {
                    return true;
                }

                if (baseType is PrimaryConstructorBaseTypeSyntax primaryConstructorBaseTypeSyntax &&
                    primaryConstructorBaseTypeSyntax.Type is GenericNameSyntax genericNameSyntax &&
                    genericNameSyntax.Identifier.Text == "GridParams" &&
                    genericNameSyntax.TypeArgumentList.Arguments.Count == 1)
                {
                    return true;
                }
            }

            return false;
        }

        internal class InputDefinition : IInputDefinition
        {
            public InputDefinition(RecordDeclarationSyntax queryDeclaration, TypeSyntax queryResultDeclaration)
            {
                this.QueryDeclaration = queryDeclaration;
                this.QueryResultDeclaration = queryResultDeclaration;
            }

            public RecordDeclarationSyntax QueryDeclaration { get; }

            public TypeSyntax QueryResultDeclaration { get; }
        }
    }
}