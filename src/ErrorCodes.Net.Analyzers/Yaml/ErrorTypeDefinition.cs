using System.Collections.Generic;

namespace ErrorCodes.Net.Analyzers.Yaml;

/// <summary>
/// Class representing a collection of <see cref="ErrorCodeDefinition"/>.
/// </summary>
internal class ErrorTypeDefinition
{
    /// <summary>
    /// Gets or sets the error type name.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the error type ID for the error type.
    /// </summary>
    public uint ErrorTypeId { get; set; }
    
    /// <summary>
    /// Gets or sets the description for the error type.
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Gets or sets a <see cref="List{T}"/> of <see cref="ErrorCodeDefinition"/>
    /// </summary>
    public List<ErrorCodeDefinition> ErrorCodes { get; set; }
}