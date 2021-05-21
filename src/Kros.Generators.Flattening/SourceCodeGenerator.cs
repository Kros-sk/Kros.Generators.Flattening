using Kros.Generators.Flattening.Helpers;
using Microsoft.CodeAnalysis.Text;
using Scriban;
using System.Text;

namespace Kros.Generators.Flattening
{
    internal static class SourceCodeGenerator
    {
        public static SourceText Generate()
        {
            var template = Template.Parse(EmbeddedResource.GetContent("FlatClassTemplate.txt"));

            string output = template.Render(new { }, member => member.Name);

            return SourceText.From(output, Encoding.UTF8);
        }
    }
}
