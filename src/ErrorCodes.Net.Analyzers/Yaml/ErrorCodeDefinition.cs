using System.Collections.Generic;

namespace ErrorCodes.Net.Analyzers.Yaml;

/// <summary>
/// Class representing an error code definition.
/// </summary>
public class ErrorCodeDefinition
{
    /// <summary>
    /// Gets or sets the name of the error code.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the error code value.
    /// </summary>
    public uint ErrorCode { get; set; }
    
    /// <summary>
    /// Gets or sets the data nodes that are not part of the standard definition.
    /// </summary>
    public Dictionary<string, object> ExtensionData { get; set; } = new();
}
