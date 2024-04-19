namespace ErrorCodes.Net.Analyzers.Yaml;

/// <summary>
/// Class representing an error code definition.
/// </summary>
internal class ErrorCodeDefinition
{
    /// <summary>
    /// Gets or sets the name of the error code.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the error code value.
    /// </summary>
    public uint ErrorCode { get; set; }
}