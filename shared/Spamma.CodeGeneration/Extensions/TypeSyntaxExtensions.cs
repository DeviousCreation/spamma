﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Spamma.CodeGeneration.Extensions
{
    internal static class TypeSyntaxExtensions
    {
        internal static string GetNamespace(this TypeSyntax typeSyntax, SemanticModel semanticModel)
        {
            var typeSymbol = semanticModel.GetSymbolInfo(typeSyntax).Symbol as INamedTypeSymbol;

            var namespaceName = string.Empty;
            if (typeSymbol != null)
            {
                namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
            }

            return namespaceName;
        }
    }
}