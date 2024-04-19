using System;

namespace ErrorCodes.Net;

/// <summary>
/// Class representing the generated error code information.
/// </summary>
public class ErrorCodeInfo
{
    /// <summary>
    /// Gets the formatted error code.
    /// </summary>
    public string FormattedErrorCode { get; }
    
    /// <summary>
    /// Gets the product ID from the error code.
    /// </summary>
    public uint ProductId { get; }
    
    /// <summary>
    /// Gets the project ID from the error code.
    /// </summary>
    public uint ProjectId { get; }
    
    /// <summary>
    /// Gets the error type ID from the error code.
    /// </summary>
    public uint ErrorTypeId { get; }
    
    /// <summary>
    /// Gets the error code from the error code.
    /// </summary>
    public uint ErrorCode { get; }
    
    /// <summary>
    /// Gets the full error code as a <see cref="uint"/> 
    /// </summary>
    public uint FullErrorCode { get; }

    /// <summary>
    /// Creates a new instance of <see cref="ErrorCodeInfo"/>
    /// </summary>
    /// <param name="formattedErrorCode">The formatted error code.</param>
    /// <param name="productId">The product ID of the error code.</param>
    /// <param name="projectId">The project ID of the error code.</param>
    /// <param name="errorTypeId">The error type ID of the error code.</param>
    /// <param name="errorCode">The error code from of error code.</param>
    public ErrorCodeInfo(string formattedErrorCode, uint productId, uint projectId, uint errorTypeId, uint errorCode)
    {
        FormattedErrorCode = formattedErrorCode;
        ProductId = productId;
        ProjectId = projectId;
        ErrorTypeId = errorTypeId;
        ErrorCode = errorCode;
        FullErrorCode = Convert.ToUInt32($"0x{projectId:X2}{errorTypeId:X2}{errorCode:X4}", 16);
    }
}