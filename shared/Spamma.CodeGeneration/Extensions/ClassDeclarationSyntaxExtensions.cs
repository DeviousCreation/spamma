using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Spamma.CodeGeneration.Extensions
{
    internal static class ClassDeclarationSyntaxExtensions
    {
        internal static string GetNamespace(this ClassDeclarationSyntax syntaxNode)
        {
            var namespaceDeclaration = syntaxNode.AncestorsAndSelf().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
            if (namespaceDeclaration != null)
            {
                return namespaceDeclaration.Name.ToString();
            }

            var namespaceDeclaration2 = syntaxNode.AncestorsAndSelf().OfType<FileScopedNamespaceDeclarationSyntax>().FirstOrDefault();
            return namespaceDeclaration2?.Name.ToString();
        }
    }
}