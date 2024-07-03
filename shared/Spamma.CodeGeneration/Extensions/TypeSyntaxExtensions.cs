using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Spamma.CodeGeneration.Extensions
{
    internal static class TypeSyntaxExtensions
    {
        internal static string GetNamespace(this TypeSyntax typeArgument, GeneratorExecutionContext context)
        {
            var model = context.Compilation.GetSemanticModel(typeArgument.SyntaxTree);
            var typeSymbol = model.GetSymbolInfo(typeArgument).Symbol as INamedTypeSymbol;

            var namespaceName = string.Empty;
            if (typeSymbol != null)
            {
                namespaceName = typeSymbol.ContainingNamespace.ToDisplayString();
            }

            return namespaceName;
        }
    }
}