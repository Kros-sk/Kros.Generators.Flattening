using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kros.Generators.Flattening
{
    internal static class SourceCodeGenerator
    {
        public static SourceText Generate(ClassModel classModel)
        {
            var template = Template.Parse(EmbeddedResource.GetContent("FlatClassTemplate.txt"));

            if (classModel.CanGenerateMappingMethod)
            {
                GenerateFlattenMethodBody(classModel);
                GenerateToFullMethodBody(classModel);
            }

            string output = template.Render(classModel, member => member.Name);
            output = Format(output);

            return SourceText.From(output, Encoding.UTF8);
        }

        private static string Format(string output)
        {
            var tree = CSharpSyntaxTree.ParseText(output);
            var root = (CSharpSyntaxNode)tree.GetRoot();
            output = root.NormalizeWhitespace().ToFullString();

            return output;
        }

        private static void GenerateFlattenMethodBody(ClassModel classModel)
        {
            StringBuilder sb = new();

            GenerateFlattenMethodBody(classModel.SourceProperties, sb, string.Empty);

            classModel.ToFlattenTemplate = sb.ToString();
        }

        private static void GenerateFlattenMethodBody(
            IEnumerable<SourcePropertyModel> sourceProperties,
            StringBuilder sb,
            string prefix)
        {
            foreach (SourcePropertyModel prop in sourceProperties)
            {
                string nameWithPrefix = NameWithPrefix(prefix, prop);

                if (prop.SubProperties.Count > 0)
                {
                    sb.AppendLine($"if (source.{nameWithPrefix} != null)");
                    sb.AppendLine("{");
                    GenerateFlattenMethodBody(prop.SubProperties, sb, nameWithPrefix);
                    sb.AppendLine("}");
                }
                else
                {
                    sb.AppendLine($"{prop.TargetPropertyName} = source.{nameWithPrefix};");
                }
            }
        }

        private static void GenerateToFullMethodBody(ClassModel classModel)
        {
            StringBuilder sb = new();

            GenerateToFullMethodBody(classModel.SourceProperties, sb, string.Empty);

            classModel.ToComplexTemplate = sb.ToString();
        }

        private static void GenerateToFullMethodBody(
            List<SourcePropertyModel> sourceProperties,
            StringBuilder sb,
            string prefix,
            IEnumerable<string> ctorParams = null)
        {
            for (int i = 0; i < sourceProperties.Count; i++)
            {
                SourcePropertyModel prop = sourceProperties[i];
                string nameWithPrefix = NameWithPrefix(prefix, prop);
                if (prop.SubProperties.Count > 0)
                {
                    if (prop.CtorParameters is null)
                    {
                        sb.AppendLine($"if (ret.{nameWithPrefix} is null)");
                        sb.AppendLine("{");
                        sb.AppendLine($"ret.{nameWithPrefix} = new();");
                        sb.AppendLine("}");
                        GenerateToFullMethodBody(prop.SubProperties, sb, nameWithPrefix);
                    }
                    else
                    {
                        sb.Append($"ret.{nameWithPrefix} = new(");
                        GenerateToFullMethodBody(prop.SubProperties, sb, nameWithPrefix, prop.CtorParameters);
                        sb.AppendLine(");");
                    }
                }
                else
                {
                    if (ctorParams is null)
                    {
                        sb.AppendLine($"ret.{nameWithPrefix} = {prop.TargetPropertyName};");
                    }
                    else
                    {
                        sb.Append($"{GetCtorParam(ctorParams, prop)}: {prop.TargetPropertyName}");
                        if (i < sourceProperties.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                }
            }
        }

        private static string GetCtorParam(IEnumerable<string> ctorParams, SourcePropertyModel prop)
            => ctorParams.First(p => p.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));

        private static string NameWithPrefix(string prefix, SourcePropertyModel prop)
          => prefix + (string.IsNullOrEmpty(prefix) ? string.Empty : ".") + prop.Name;
    }
}
