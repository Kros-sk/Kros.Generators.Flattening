using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Kros.Generators.Flattening
{
    internal class ClassModel
    {
        private const string Attribute = "Attribute";
        private const string SourceTypeArgName = nameof(FlattenAttribute.SourceType);
        public static readonly string FlattenAttributeName = nameof(FlattenAttribute).TrimEnd(Attribute);

        private AttributeSyntax _flattenAttribute;
        private SemanticModel _semanticModel;
        private ClassDeclarationSyntax _syntax;
        private Compilation _compilation;
        private CompilationUnitSyntax _root;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public string Namespace { get; private set; }

        public string Name { get; private set; }

        public string Modifier { get; private set; }

        public IEnumerable<PropertyModel> Properties { get; private set; }

        public static ClassModel Create(ClassDeclarationSyntax syntax, Compilation compilation, GeneratorExecutionContext context)
        {
            var flattenAttribute = syntax.GetAttribute(FlattenAttributeName);

            if (!flattenAttribute.ContainsArguments(SourceTypeArgName))
            {
                context.ReportMissingArgument(flattenAttribute, SourceTypeArgName);
                return null;
            }

            ClassModel model = new();
            model._root = syntax.GetCompilationUnit();
            model._syntax = syntax;
            model._compilation = compilation;
            model._semanticModel = compilation.GetSemanticModel(syntax.SyntaxTree);
            model._flattenAttribute = flattenAttribute;

            model.BasicInformation();
            model.AddProperties();

            return model;
        }

        private void BasicInformation()
        {
            Name = _syntax.GetClassName();
            Modifier = _syntax.GetClassModifier();
            Namespace = _root.GetNamespace();
        }

        private void AddProperties()
        {
            var flatClass = _semanticModel.GetDeclaredSymbol(_syntax) as INamedTypeSymbol;
            var properties = new List<PropertyModel>();
            var existingProperties = new HashSet<string>(flatClass.GetProperties().Select(p => p.Name));
            var sourceType = _flattenAttribute.GetTypeArgument(nameof(FlattenAttribute.SourceType), _semanticModel);
            var propertiesToSkip = _flattenAttribute.GetArrayArguments(nameof(FlattenAttribute.Skip), _semanticModel);

            foreach (IPropertySymbol property in sourceType.GetProperties())
            {
                if (!existingProperties.Contains(property.Name) && !propertiesToSkip.Contains(property.Name))
                {
                    properties.Add(
                        new(property.DeclaredAccessibility.ToString().ToLower(), property.Type.ToString(), property.Name));
                }
            }

            Properties = properties;
        }
    }
}
