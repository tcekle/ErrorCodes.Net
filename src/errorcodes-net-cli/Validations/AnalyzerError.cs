using Microsoft.CodeAnalysis;

namespace errorcodes_net_cli.Validations;

/// <summary>
/// Class representing an error from an analyzer
/// </summary>
internal class AnalyzerError
{
    /// <summary>
    /// Gets the <see cref="ErrorType"/>
    /// </summary>
    public ErrorType ErrorType { get; }

    /// <summary>
    /// Gets the error message
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the <see cref="Location"/> that the error occurred
    /// </summary>
    public Location? Location { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="AnalyzerError"/>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorType"></param>
    public AnalyzerError(string message, ErrorType errorType = ErrorType.Error)
    {
        Message = message;
        ErrorType = errorType;
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return $"{ErrorType.ToString()}: {Message}\r\nLocation: {Location?.ToString() ?? "N/A"}";
    }
}
