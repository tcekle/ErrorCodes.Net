using Microsoft.CodeAnalysis;

namespace errorcodes_net_cli.Validations;

/// <summary>
/// Class representing an error manager for collecting errors that occur during analysis
/// </summary>
internal static class ErrorManager
{
    /// <summary>
    /// Gets the list of errors that have occurred during analysis
    /// </summary>
    public static List<AnalyzerError> Errors { get; } = new();

    /// <summary>
    /// Adds a symbol error to the error list
    /// </summary>
    /// <param name="symbol">The <see cref="ISymbol"/> that the error occurred on</param>
    /// <param name="message">A message to include in the error</param>
    /// <param name="errorType">(optional) The <see cref="ErrorType"/> of the error</param>
    public static void AddSymbolError(ISymbol? symbol, string message, ErrorType errorType = ErrorType.Error)
    {
        Location? location = null;
        if (symbol?.Locations.Length > 0)
        {
            location = symbol.Locations[0];
        }

        AddLocationError(location, message, errorType);
    }

    /// <summary>
    /// Adds a location error to the error list
    /// </summary>
    /// <param name="location">The <see cref="Location"/> that the error occurred on</param>
    /// <param name="message">A message to include in the error</param>
    /// <param name="errorType">(optional) The <see cref="ErrorType"/> of the error</param>
    public static void AddLocationError(Location? location, string message, ErrorType errorType = ErrorType.Error)
    {
        Errors.Add(new AnalyzerError(message, errorType) { Location = location });
    }
}
