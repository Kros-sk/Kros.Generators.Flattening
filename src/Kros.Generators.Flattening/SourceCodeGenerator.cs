using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Text;

namespace Kros.Generators.Flattening
{
    internal static class SourceCodeGenerator
    {
        public static SourceText Generate(ClassModel classModel)
        {
            var template = Template.Parse(EmbeddedResource.GetContent("FlatClassTemplate.txt"));

            string output = template.Render(classModel, member => member.Name);

            return SourceText.From(output, Encoding.UTF8);
        }
    }
}
