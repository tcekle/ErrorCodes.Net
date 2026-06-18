using Microsoft.CodeAnalysis;

namespace errorcodes_net_cli.Validations;

/// <summary>
/// Extensions for the <see cref="Compilation"/> class
/// </summary>
public static class CompilationExtensions
{
    /// <summary>
    /// Find a symbol in the compilation from a full type name
    /// </summary>
    /// <param name="compilation">The <see cref="Compilation"/> to use</param>
    /// <param name="searchedType">The full name of the type to search for</param>
    /// <returns></returns>
    public static INamedTypeSymbol? FindSymbol(this Compilation compilation, string searchedType)
    {
        var splitFullName = searchedType.Split('.');
        var namespaceNames = splitFullName.Take(splitFullName.Length - 1).ToArray();
        var className = splitFullName.Last();

        if (namespaceNames.Length == 0)
        {
            return compilation.GlobalNamespace.GetAllTypes(new CancellationToken()).FirstOrDefault(n => n.Name == className);
        }

        var namespaces = compilation.GlobalNamespace.GetNamespaceMembers();
        INamespaceSymbol? namespaceContainingType = null;
        foreach (var name in namespaceNames)
        {
            // A compilation that does not reference the searched type will be missing the
            // namespace entirely (for example, projects that do not use ErrorCodes.Net).
            // Treat that as "not found" rather than throwing so callers can skip the project.
            namespaceContainingType = namespaces.FirstOrDefault(n => n.Name == name);
            if (namespaceContainingType is null)
            {
                return null;
            }

            namespaces = namespaceContainingType.GetNamespaceMembers();
        }

        return namespaceContainingType?.GetAllTypes(new CancellationToken()).FirstOrDefault(n => n.Name == className);
    }
}
