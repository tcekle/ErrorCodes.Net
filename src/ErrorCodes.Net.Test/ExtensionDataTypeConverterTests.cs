using ErrorCodes.Net.Analyzers.Yaml;
using ErrorCodes.Net.Analyzers.Yaml.Converters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ErrorCodes.Net.Test;

[TestClass]
public class ExtensionDataTypeConverterTests
{
    [TestMethod]
    public void ExtensionDataTypeConverter_DoesNotThrowForUnknownNodes()
    {
        string yaml = """
                      ---
                      projectId: 6
                      errorTypes:
                        - name: TestErrors
                          errorTypeId: 0
                          errorCodes:
                            - errorCode: 1
                              name: RunError
                              message:
                                - en: "An error occurred while running the application."
                                  es: "Se produjo un error al ejecutar la aplicación."
                              test: "test"
                      """;
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(ExtensionDataTypeConverter.Instance)
            .Build();

        deserializer.Deserialize<ErrorTypeCollection>(yaml);
    }
}
