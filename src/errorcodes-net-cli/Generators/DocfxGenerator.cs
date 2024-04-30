using System.Text;
using System.Text.RegularExpressions;
using ErrorCodes.Net.Analyzers.Yaml;
using ErrorCodes.Net.Analyzers.Yaml.Converters;
using Microsoft.CodeAnalysis;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace errorcodes_net_cli.Generators;

/// <summary>
/// Class to generate docfx error code documentation
/// </summary>
internal static class DocfxGenerator
{
    /// <summary>
    /// Generate the docfx error code documentation
    /// </summary>
    /// <param name="solution">The <see cref="Solution"/> to use</param>
    /// <param name="outputFile">The location to write the resulting document to</param>
    public static async Task GenerateDocfxErrors(Solution solution, string outputFile)
    {
        var yamlFiles = solution.Projects
            .SelectMany(p => p.AdditionalDocuments)
            .Where(d => d.Name.EndsWith(".yaml") || d.Name.EndsWith(".yml"));

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(ExtensionDataTypeConverter.Instance)
            .Build();
        
        List<ErrorTypeCollection> errorTypeCollections = new();
        
        foreach (var yamlFile in yamlFiles)
        {
            var content = await yamlFile.GetTextAsync();
            errorTypeCollections.Add(deserializer.Deserialize<ErrorTypeCollection>(content.ToString()));
        }
        
        string fileContents = CreateDocfxFile(errorTypeCollections, GetProductId(solution));
        
        await File.WriteAllTextAsync(outputFile, fileContents);
    }

    private static string CreateDocfxFile(List<ErrorTypeCollection> errorTypeCollections, uint productId)
    {
        List<ErrorEntry> errors = new();

        foreach (var errorCollection in errorTypeCollections)
        {
            foreach (var errorType in errorCollection.ErrorTypes)
            {
                foreach (var error in errorType.ErrorCodes)
                {
                    string documentation = string.Empty;
                    if (error.ExtensionData.TryGetValue("documentation", out object? value))
                    {
                        documentation = value as string ?? string.Empty;
                    }

                    errors.Add(new ErrorEntry($"{productId}x{errorCollection.ProjectId:X2}{errorType.ErrorTypeId:X2}{error.ErrorCode:X4}", 
                        error.Name,
                        documentation));
                }
            }
        }

        StringBuilder entryBuilder = new StringBuilder();
        foreach (var errorCode in errors.OrderBy(e => e.ErrorCode))
        {
            entryBuilder.AppendLine($"""
                                    ### {errorCode.ErrorCode} - {errorCode.Name}
                                    
                                    {errorCode.Documentation}
                                    
                                    """);
        }
        
        return $"""
                # Error Codes
                
                Below are the list of errors that you may see while using the software.
                
                {entryBuilder}
                """;
    }

    private static uint GetProductId(Solution solution)
    {
        if (string.IsNullOrEmpty(solution.FilePath))
        {
            return 0;
        }
        
        var solutionFileInfo = new FileInfo(solution.FilePath);
        string buildPropsFile = Path.Combine(solutionFileInfo.DirectoryName ?? string.Empty, "Directory.Build.Props");

        if (!File.Exists(buildPropsFile))
        {
            return 0;
        }
        
        var match = Regex.Match(File.ReadAllText(buildPropsFile), @"<ErrorCodesProductId>(\d+)</ErrorCodesProductId>");
        
        if (!match.Success)
        {
            return 0;
        }
        
        if (!uint.TryParse(match.Groups[1].Value, out uint productId))
        {
            return 0;
        }

        return productId;
    }

    private record ErrorEntry(string ErrorCode, string Name, string? Documentation);
}
