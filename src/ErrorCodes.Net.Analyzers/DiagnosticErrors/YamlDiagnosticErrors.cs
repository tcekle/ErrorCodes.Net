using Microsoft.CodeAnalysis;

namespace ErrorCodes.Net.Analyzers.DiagnosticErrors;

/// <summary>
/// Class containing diagnostic errors for YAML files
/// </summary>
internal static class YamlDiagnosticErrors
{
    /// <summary>
    /// A <see cref="DiagnosticDescriptor"/> for when the 'Namespace' field is missing in the YAML file
    /// </summary>
    public static readonly DiagnosticDescriptor MissingNamespaceDescriptor = new DiagnosticDescriptor(
        id: "ENET0001",
        title: "Namespace not set",
        messageFormat: "Namespace not set in ErrorCodes YAML file",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The YAML file must have a 'Namespace' field that is not empty."
    );
}
