using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Kros.Generators.Flattening
{
    internal sealed class FlattenReceiver : ISyntaxReceiver
    {
        private readonly List<ClassDeclarationSyntax> _candidates = new();

        public IEnumerable<ClassDeclarationSyntax> Candidates => _candidates;

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.HaveAttribute(ClassModel.FlattenAttributeName))
            {
                _candidates.Add(classSyntax);
            }
        }
    }
}
