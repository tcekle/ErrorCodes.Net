using System.Collections.Generic;

namespace ErrorCodes.Net.Analyzers.Yaml;

/// <summary>
/// Class representing a collection of <see cref="ErrorTypeDefinition"/>.
/// </summary>
internal class ErrorTypeCollection
{
    /// <summary>
    /// Gets or sets the project ID for the error type collection.
    /// </summary>
    public uint ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets a <see cref="List{T}"/> of <see cref="ErrorTypeDefinition"/>
    /// </summary>
    public List<ErrorTypeDefinition> ErrorTypes { get; set; }
}