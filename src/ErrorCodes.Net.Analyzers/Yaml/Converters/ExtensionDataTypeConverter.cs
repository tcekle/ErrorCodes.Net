using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ErrorCodes.Net.Analyzers.Yaml.Converters;

/// <summary>
/// Class implementing an <see cref="IYamlTypeConverter"/> for creating extension data
/// </summary>
public class ExtensionDataTypeConverter : IYamlTypeConverter
{
    /// <summary>
    /// Gets an instance of the <see cref="ExtensionDataTypeConverter"/>
    /// </summary>
    public static readonly IYamlTypeConverter Instance = new ExtensionDataTypeConverter();
    
    /// <summary>
    /// Gets a value indicating whether the current converter supports converting the specified type.
    /// </summary>
    public bool Accepts(Type type)
    {
        return type == typeof(ErrorCodeDefinition);
    }

    /// <summary>Reads an object's state from a YAML parser.</summary>
    public object ReadYaml(IParser parser, Type type)
    {
        ErrorCodeDefinition result = new ErrorCodeDefinition();
        parser.Consume<MappingStart>();

        while (true)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                ReadScalar(parser, scalar, result);
                continue;
            }
            
            if (parser.TryConsume<SequenceStart>(out var sequenceStart))
            {
                continue;
            }
            
            if (parser.TryConsume<MappingEnd>(out var mappingEnd))
            {
                break;
            }
        }
        
        return result;
    }

    /// <summary>
    /// Writes the specified object's state to a YAML emitter.
    /// </summary>
    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        throw new NotImplementedException();
    }
    
    private void ReadScalar(IParser parser, Scalar scalar, ErrorCodeDefinition result)
    {
        switch (scalar.Value)
        {
            case "name":
                result.Name = parser.Consume<Scalar>().Value;
                break;
            case "errorCode":
                result.ErrorCode = ConvertToUInt32(parser.Consume<Scalar>().Value);
                break;
            default:
                result.ExtensionData[scalar.Value] = ReadValue(parser);
                break;
        }
    }
    
    private uint ConvertToUInt32(string value)
    {
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return Convert.ToUInt32(value, 16);
        }
        
        return Convert.ToUInt32(value);
    }

    private object ReadValue(IParser parser)
    {
        if (parser.TryConsume<Scalar>(out var scalar))
        {
            return scalar.Value;
        }
        
        if (parser.TryConsume<SequenceStart>(out _))
        {
            var result = new Dictionary<string, object>();
            parser.Consume<MappingStart>();
            while (!parser.TryConsume<MappingEnd>(out _))
            {
                var key = parser.Consume<Scalar>().Value;
                var value = ReadValue(parser);
                result[key] = value;
            }

            parser.Consume<SequenceEnd>();
            return result;
        }
        
        throw new InvalidOperationException("Unexpected YAML token.");
    }
}
