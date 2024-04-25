using Microsoft.CodeAnalysis;

namespace errorcodes_net_cli.Validations;

/// <summary>
/// Extensions for symbol related types
/// </summary>
public static class SymbolExtensions
{
    /// <summary>
    /// Get all types in a namespace
    /// </summary>
    /// <param name="namespace">The namespace to search</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to stop the operation</param>
    /// <returns>A collection of <see cref="INamedTypeSymbol"/></returns>
    public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol @namespace, CancellationToken cancellationToken)
    {
        Queue<INamespaceOrTypeSymbol> symbols = new Queue<INamespaceOrTypeSymbol>();
        symbols.Enqueue(@namespace);

        while (symbols.Count > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();

            INamespaceOrTypeSymbol namespaceOrTypeSymbol = symbols.Dequeue();
            INamespaceSymbol? namespaceSymbol = namespaceOrTypeSymbol as INamespaceSymbol;
            if (namespaceSymbol == null)
            {
                INamedTypeSymbol typeSymbol = (INamedTypeSymbol)namespaceOrTypeSymbol;
                Array.ForEach(typeSymbol.GetTypeMembers().ToArray(), symbols.Enqueue);

                yield return typeSymbol;
            }
            else
            {
                Array.ForEach(namespaceSymbol.GetMembers().ToArray(), symbols.Enqueue);
            }
        }
    }
}
