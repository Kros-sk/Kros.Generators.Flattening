using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kros.Generators.Flattening
{
    internal static class RoslynExtensions
    {
        public static CompilationUnitSyntax GetCompilationUnit(this SyntaxNode syntaxNode)
            => syntaxNode.Ancestors().OfType<CompilationUnitSyntax>().FirstOrDefault();

        public static bool HaveAttribute(this ClassDeclarationSyntax classDeclaration, string attributeName)
            => classDeclaration?.AttributeLists.Count > 0
                && classDeclaration
                    .AttributeLists
                       .SelectMany(SelectWithAttributes(attributeName))
                       .Any();

        public static IEnumerable<AttributeSyntax> GetAttributes(
            this ClassDeclarationSyntax classDeclaration,
            string attributeName)
            => classDeclaration
                .AttributeLists
                    .SelectMany(SelectWithAttributes(attributeName));

        public static AttributeSyntax GetAttribute(this ClassDeclarationSyntax classDeclaration, string attributeName)
            => classDeclaration.GetAttributes(attributeName).Single();

        private static Func<AttributeListSyntax, IEnumerable<AttributeSyntax>> SelectWithAttributes(string attributeName)
            => l => l?.Attributes.Where(a => (a.Name as IdentifierNameSyntax)?.Identifier.Text == attributeName);

        public static string GetClassName(this ClassDeclarationSyntax classDeclaration)
            => classDeclaration.Identifier.Text;

        public static string GetClassModifier(this ClassDeclarationSyntax classDeclaration)
            => classDeclaration.Modifiers.ToFullString().Trim();

        public static string GetNamespace(this CompilationUnitSyntax root)
            => root.ChildNodes()
                .OfType<NamespaceDeclarationSyntax>()
                .First().Name.ToString();

        public static bool ContainsArguments(this AttributeSyntax attribute, string argumentName)
            => attribute
               .ArgumentList
               .Arguments
               .Any(p => p.NameEquals.Name.Identifier.ValueText == argumentName);

        public static INamedTypeSymbol GetTypeArgument(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel)
        {
            TypeSyntax typeOfExpression = attribute.GetArgument<TypeOfExpressionSyntax>(argumentName).Type;
            TypeInfo typeInfo = semanticModel.GetTypeInfo(typeOfExpression);

            return ((INamedTypeSymbol)typeInfo.Type);
        }

        public static HashSet<string> GetArrayArguments(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel)
        {
            HashSet<string> ret = new();
            SeparatedSyntaxList<ExpressionSyntax>? expressions =
                attribute.GetArgument<ArrayCreationExpressionSyntax>(argumentName)?.Initializer.Expressions;

            if (expressions != null)
            {
                foreach (var expression in expressions)
                {
                    Optional<object> value = semanticModel.GetConstantValue(expression);
                    if (value.HasValue)
                    {
                        ret.Add(value.Value.ToString());
                    }
                }
            }

            return ret;
        }

        public static string GetConstantAttribute(
            this AttributeSyntax attribute,
            string argumentName,
            SemanticModel semanticModel)
        {
            var expression = attribute.GetArgument<ExpressionSyntax>(argumentName);
            Optional<object> value = semanticModel.GetConstantValue(expression);

            return value.HasValue ? value.Value.ToString() : null;
        }

        private static T GetArgument<T>(this AttributeSyntax attribute, string argumentName)
            where T: ExpressionSyntax
            => attribute
                .ArgumentList
                .Arguments
                .FirstOrDefault(p => p.NameEquals.Name.Identifier.ValueText == argumentName)?.Expression as T;

        public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
        {
            ITypeSymbol current = type;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        public static IEnumerable<ISymbol> GetAllMembers(this ITypeSymbol type)
            => type.GetBaseTypesAndThis().SelectMany(n => n.GetMembers());

        public static IEnumerable<IPropertySymbol> GetProperties(this INamedTypeSymbol symbol)
            => symbol.GetAllMembers()
                .Where(x => x.Kind == SymbolKind.Property)
                .OfType<IPropertySymbol>()
                .Where(p =>
                    !p.IsReadOnly && !p.IsStatic
                    && (p.DeclaredAccessibility == Accessibility.Public
                        || p.DeclaredAccessibility == Accessibility.Internal
                        || p.DeclaredAccessibility == Accessibility.ProtectedAndInternal));
    }
}
