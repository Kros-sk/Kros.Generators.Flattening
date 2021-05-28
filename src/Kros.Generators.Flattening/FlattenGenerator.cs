using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Kros.Generators.Flattening
{
    [Generator]
    internal class FlattenGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("FlattenAttributes.cs",
                SourceText.From(EmbeddedResource.GetContent("GeneratedAttributes.cs"), Encoding.UTF8));

            context.AddSource("myclass2.cs", SourceCodeGenerator.Generate(new ClassModel()
            {
                Modifier = "public partial",
                Name = "Invoice",
                Namespace = "Kros.XXX",
                Version = "1.0.0"
            }));
        }
    }
}
