using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Kros.Generators.Flattening.Helpers
{
    public static class GeneratorExecutionContextExtensions
    {
        private static readonly DiagnosticDescriptor _missingArgument = new(
            id: "KRF001",
            title: "Missing attribute argument",
            messageFormat: "Argument '{0}' of '{1}Attribute' is required",
            category: "Flattening",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor _exception = new(
            id: "KRF002",
            title: "Unexpected exception",
            messageFormat: "Unexpected expcetion '{0}'",
            category: "Flattening",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public static void ReportMissingArgument(
            this GeneratorExecutionContext context,
            AttributeSyntax attribute,
            string argumentName)
            => context.ReportDiagnostic(
                Diagnostic.Create(
                    _missingArgument,
                    attribute.GetLocation(),
                    argumentName,
                    (attribute.Name as IdentifierNameSyntax).Identifier.Text));

        public static void ReportException(
            this GeneratorExecutionContext context,
            Exception exception)
            => context.ReportDiagnostic(
                Diagnostic.Create(
                    _exception,
                    Location.None,
                    exception.Message));
    }
}
