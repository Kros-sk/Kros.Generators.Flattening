using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kros.Generators.Flattening
{
    internal class ClassModel
    {
        private const string Attribute = "Attribute";
        private const string SourceTypeArgName = nameof(FlattenAttribute.SourceType);
        public static readonly string FlattenAttributeName = nameof(FlattenAttribute).TrimEnd(Attribute);
        public static readonly string FlattenPropertyNameAttributeName = nameof(FlattenPropertyNameAttribute).TrimEnd(Attribute);

        private AttributeSyntax _flattenAttribute;
        private SemanticModel _semanticModel;
        private ClassDeclarationSyntax _syntax;
        private Compilation _compilation;
        private CompilationUnitSyntax _root;
        private GeneratorExecutionContext _context;
        private List<PropertyModel> _properties = new();

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);

        public string Namespace { get; private set; }

        public string Name { get; private set; }

        public string Modifier { get; private set; }

        public IEnumerable<PropertyModel> Properties => _properties;

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
            model._context = context;

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
            var existingProperties = new HashSet<string>(flatClass.GetProperties().Select(p => p.Name));
            var sourceType = _flattenAttribute.GetTypeArgument(nameof(FlattenAttribute.SourceType), _semanticModel);
            var propertiesToSkip = _flattenAttribute
                .GetArrayArguments(nameof(FlattenAttribute.Skip), _semanticModel, (s) => s.Replace(".", string.Empty));
            var doNotFlatten = _flattenAttribute
                .GetArrayArguments(nameof(FlattenAttribute.DoNotFlatten), _semanticModel, (s) => s.Replace(".", string.Empty));
            var namigMap = GetNamigMap();

            ProcessProperties(
                sourceType.GetProperties(),
                existingProperties,
                propertiesToSkip,
                doNotFlatten,
                namigMap,
                string.Empty);
        }

        private void ProcessProperties(
            IEnumerable<IPropertySymbol> properties,
            HashSet<string> existingProperties,
            HashSet<string> propertiesToSkip,
            HashSet<string> doNotFlatten,
            IDictionary<string, string> namingMap,
            string namePrefix)
        {
            foreach (IPropertySymbol property in properties)
            {
                string nameWithoutPrefix = GetName(namePrefix, namingMap, property);
                string name = namePrefix + nameWithoutPrefix;

                if (!existingProperties.Contains(name) && !propertiesToSkip.Contains(name))
                {
                    if (CanExpand(property) && !doNotFlatten.Contains(name))
                    {
                        ProcessProperties(
                            (property.Type as INamedTypeSymbol).GetProperties(), existingProperties, propertiesToSkip,
                            doNotFlatten, namingMap, namePrefix + nameWithoutPrefix);
                    }
                    else
                    {
                        _properties.Add(
                            new(property.DeclaredAccessibility.ToString().ToLower(), property.Type.ToString(), name));
                    }
                }
            }
        }

        private static bool CanExpand(IPropertySymbol property)
            => !property.Type.IsValueType
            && (property.Type.SpecialType != SpecialType.System_String)
            && (property.Type.ContainingNamespace?.ToString().StartsWith("System") == false);

        private static string GetName(
            string prefix,
            IDictionary<string, string> namingMap,
            IPropertySymbol property)
            => namingMap.ContainsKey(prefix + property.Name) ? namingMap[prefix + property.Name] : property.Name;

        private IDictionary<string, string> GetNamigMap()
        {
            Dictionary<string, string> ret = new(StringComparer.CurrentCultureIgnoreCase);
            var attributes = _syntax.GetAttributes(FlattenPropertyNameAttributeName);

            foreach (var attribute in attributes)
            {
                if (!attribute.ContainsArguments(nameof(FlattenPropertyNameAttribute.SourcePropertyName)))
                {
                    _context.ReportMissingArgument(attribute, nameof(FlattenPropertyNameAttribute.SourcePropertyName));
                    continue;
                }

                if (!attribute.ContainsArguments(nameof(FlattenPropertyNameAttribute.Name)))
                {
                    _context.ReportMissingArgument(attribute, nameof(FlattenPropertyNameAttribute.Name));

                    continue;
                }

                string sourcePropertyName =
                   attribute.GetConstantAttribute(nameof(FlattenPropertyNameAttribute.SourcePropertyName), _semanticModel)
                   .Replace(".", string.Empty);
                string name = attribute.GetConstantAttribute(nameof(FlattenPropertyNameAttribute.Name), _semanticModel);

                ret.Add(sourcePropertyName, name);
            }

            return ret;
        }
    }
}
