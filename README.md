# 🛠️ ErrorCodes.Net

**ErrorCodes.Net** is a .NET library and Roslyn analyzer that helps you define and manage structured, traceable error codes across your applications.

It enforces a consistent format:

```
0xPPMMTTTT
```

Where:

- `PP` = Product ID (e.g., `00` for Service A, `01` for Service B)
- `MM` = Project ID (e.g., `06`)
- `TT` = Error Type ID
- `TT` = Error Code

This structure ensures that each error code is unique, traceable, and easy to interpret.

## 🚀 Features

- ✅ Enforces consistent error code formatting via Roslyn analyzers  
- ✅ YAML-based configuration for defining error codes  
- ✅ Supports multiple error types per project  
- ✅ Generates C# code for defined error codes  
- ✅ Integrates seamlessly with your build process  

## ⬇️ Installation

Install the analyzer via NuGet:

```bash
dotnet add package ErrorCodes.Net.Analyzers
```

In your .csproj file, ensure the package is referenced with PrivateAssets="all" to prevent it from being exposed to consumers:

```xml
<PackageReference Include="ErrorCodes.Net.Analyzers" PrivateAssets="all" />
```

## ⚙️ Configuration

Create an `ErrorCodes.yaml` or `ErrorCodes.yml` file in your project root with the following structure:

```yaml
projectId: 6
namespace: ErrorCodes.Net.Generated
errorTypes:
  - name: TestErrors
    errors:
      - code: 1
        name: InvalidInput
      - code: 2
        name: NotFound
```

This configuration defines the project ID, the namespace for the generated code, and a list of error types with their corresponding error codes and messages.

## 🧪 Example Usage

Once configured, the analyzer will generate a static class with your error codes.

Make sure to include the YAML file in your `.csproj` so the analyzer can find it:

```xml
<ItemGroup>
  <AdditionalFiles Include="ErrorCodes.yaml" />
</ItemGroup>
```

Then you can use the generated error codes like this:

```csharp
using ErrorCodes.Net.Generated;

public class Example
{
    public void DoSomething()
    {
        var errorCode = ErrorCodeLookup.TestErrors.InvalidInput;
        Console.WriteLine(errorCode); // Outputs: 0x06010001
    }
}
```

See a full sample project [here](src/Samples/SampleConsole/).

## 🧰 Advanced Usage

You can define multiple error types within the same project:

```yaml
projectId: 6
namespace: ErrorCodes.Net.Generated
errorTypes:
  - name: ValidationErrors
    errors:
      - code: 1
        name: MissingField
  - name: DatabaseErrors
    errors:
      - code: 1
        name: ConnectionFailed
```

This will generate separate classes for each error type, such as `ValidationErrors` and `DatabaseErrors`, each containing their respective error codes.

## 📦 NuGet Package

The analyzer is available on NuGet:

[ErrorCodes.Net.Analyzers on NuGet](https://www.nuget.org/packages/ErrorCodes.Net.Analyzers)

## 🤝 Contributing

Contributions are welcome! If you have suggestions for improvements or new features, feel free to open an issue or submit a pull request.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
