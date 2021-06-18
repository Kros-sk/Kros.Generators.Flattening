using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Kros.Generators.Flattening
{
    [Generator]
    internal class FlattenGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new FlattenReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            context.AddSource("FlattenSourceTypes.cs",
                SourceText.From(EmbeddedResource.GetContent("SourceTypes.cs"), Encoding.UTF8));

            if (context.SyntaxReceiver is FlattenReceiver actorSyntaxReciver)
            {
                foreach (ClassDeclarationSyntax candidate in actorSyntaxReciver.Candidates)
                {
                    var classModel = ClassModel.Create(candidate, context.Compilation, context);

                    if (classModel != null)
                    {
                        context.AddSource($"{classModel.Name}-Flatten.cs", SourceCodeGenerator.Generate(classModel));
                    }
                }
            }
        }
    }
}
