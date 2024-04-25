using ErrorCodes.Net.Generated;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace errorcodes_net_cli.Validations.Validators;

/// <summary>
/// Class to check the uniqueness of error codes
/// </summary>
internal static class CheckUniquenessValidator
{
    /// <summary>
    /// Check for the uniqueness of error codes
    /// </summary>
    /// <param name="solution">The <see cref="Solution"/> to inspect</param>
    internal static async Task CheckUniquenessOfErrors(Solution solution)
    {
        IMethodSymbol? errorCodeType = null;
        
        foreach (var project in solution.Projects)
        {
            var comp = await project.GetCompilationAsync();
            if (comp is null)
            {
                continue;
            }
            
            errorCodeType = comp.FindSymbol("ErrorCodes.Net.ErrorCodeInfo")?.Constructors.FirstOrDefault();
            if (errorCodeType != null)
            {
                break;
            }
        }

        if (errorCodeType is null)
        {
            throw new Exception($"{ErrorCodeLookup.ValidateCommand.ErrorCodeInfoTypeNotFound.FormattedErrorCode}: ErrorCodeInfo type not found.");
        }

        // Find usages of ErrorCodeInfo
        var results = await SymbolFinder.FindReferencesAsync(errorCodeType, solution);
        
        await CheckUniquenessOfGeneratedErrorCodes(results, errorCodeType);
    }
    
    private static async Task CheckUniquenessOfGeneratedErrorCodes(IEnumerable<ReferencedSymbol> referencedSymbols, IMethodSymbol errorCodeType)
    {
        int formattedErrorCodeIndex = GetArgumentIndex(errorCodeType, "formattedErrorCode");
        List<Tuple<string, string, Location>> formattedErrorCodeValues = new();
        HashSet<string> seenLocations = new HashSet<string>();
        
        foreach (var referencedSymbol in referencedSymbols)
        {
            foreach (var location in referencedSymbol.Locations)
            {
                string locationId = GetLocationIdentifier(location.Location);
                if (string.IsNullOrEmpty(locationId) || seenLocations.Contains(locationId))
                {
                    continue;
                }

                var objectCreation = await GetObjectCreationExpression(location);
                
                if (objectCreation != null && objectCreation.Type.ToString() == "ErrorCodeInfo")
                {
                    string? value = objectCreation.ArgumentList?.Arguments[formattedErrorCodeIndex].ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        throw new Exception($"{ErrorCodeLookup.UniquenessCheck.InvalidFormattedErrorParameter.FormattedErrorCode}: Invalid formatted error code parameter.");
                    }
                    
                    formattedErrorCodeValues.Add(new (
                        value, 
                        (objectCreation.Parent?.Parent as PropertyDeclarationSyntax)?.Identifier.ToString() ?? string.Empty, 
                        location.Location));
                }

                seenLocations.Add(locationId);
            }
        }

        CheckForDuplicates(formattedErrorCodeValues);
    }
    
    private static string GetLocationIdentifier(Location location)
    {
        if (!location.IsInSource)
        {
            return string.Empty;
        }

        var syntaxTree = location.SourceTree;
        var span = location.SourceSpan;
        var filePath = syntaxTree?.FilePath;

        return $"{filePath}@{span.Start}:{span.End}";
    }
    
    private static async Task<ObjectCreationExpressionSyntax?> GetObjectCreationExpression(ReferenceLocation referenceLocation)
    {
        var sem = await referenceLocation.Document.GetSemanticModelAsync();
        if (sem is null)
        {
            return null;
        }
                
        var root = await referenceLocation.Document.GetSyntaxRootAsync();
        if (root is null)
        {
            return null;
        }

        var l = root.SyntaxTree.GetLocation(root.Span);
                
        var node = root.FindNode(referenceLocation.Location.SourceSpan);
        
        if (node is ObjectCreationExpressionSyntax objectCreation)
        {
            return objectCreation;
        }

        return node.Parent as ObjectCreationExpressionSyntax;
    }

    private static void CheckForDuplicates(List<Tuple<string, string, Location>> formattedErrorCodeValues)
    {
        var duplicates = formattedErrorCodeValues.GroupBy(x => x.Item1)
            .Where(g => g.Count() > 1);
        
        foreach (var duplicate in duplicates)
        {
            foreach (var item in duplicate)
            {
                ErrorManager.AddLocationError(item.Item3, $"Duplicate error code found: {item.Item1} : {item.Item2}");
            }
        }
    }
    
    private static int GetArgumentIndex(IMethodSymbol methodSymbol, string argumentName)
    {
        for (int i = 0; i < methodSymbol.Parameters.Length; i++)
        {
            if (methodSymbol.Parameters[i].Name == argumentName)
            {
                return i;
            }
        }

        throw new Exception($"{ErrorCodeLookup.ValidateCommand.ConstructorParameterNotFound.FormattedErrorCode}: Constructor parameter '{argumentName}' not found.");
    }
}
