# ErrorCodes.Net

ErrorCodes.Net is a library for generating structured error codes with the format `0x12345678`.

How the value is broken down:

| `0x`       | `12`       | `34`       | `5678`     |
|------------|------------|------------|------------|
| Product ID | Project ID | Error Type | Error Code |

### Product ID

Denotes which product the error belongs to. For example:

**Service A** with Product ID of `0` will have the format `0xXXXXXXXX` while **Service B** with a Product ID of `1` will have the format `1xXXXXXXXX`

## Quick Start

1. Install `ErrorCodes.Net.Analyzers` using the NuGet package manager in your IDE or use the following command in the project directory:
    ```pwsh
    PS C:\YOURSRC> dotnet add package ErrorCodes.Net.Analyzers
    ```
2. In the project file, make sure to add the `PrivateAssets="all"` to the package:
    ```xml
    <PackageReference Include="ErrorCodes.Net.Analyzers" PrivateAssets="all" />
    ```
3. Create a file in your project with the name `ErrorCodes.yaml` or `ErrorCodes.yml` and fill it with an example:
    ```yaml
    ---
    projectId: 6
    errorTypes:
      - name: TestErrors
        errorTypeId: 0
        errorCodes:
          - errorCode: 1
            name: RunError
          - errorCode: 2
            name: LogsError
          - errorCode: 3
            name: UninstallError
    ```
4. Set the build action for the new file to `C# analyzer additional file` in Visual Studio or `AdditionalFiles` in Rider.
5. Rebuild the project.

If the rebuild was successful, you should be able to reference the error codes in the following way:

```csharp
string error = ErrorCodeLookup.TestErrors.RunError.FormattedErrorCode;
```

For the above example file, the results of the `FormatedErrorCode` are:

| Name           | Formatted output |
|----------------|------------------|
| RunError       | 0x060001         |
| LogsError      | 0x060002         |
| UninstallError | 0x060003         |

Example top-level console application:

**Program.cs**
```csharp
using ErrorCodes.Net.Generated;

Console.WriteLine(ErrorCodeLookup.TestErrors.RunError.FormattedErrorCode);
Console.WriteLine(ErrorCodeLookup.TestErrors.LogsError.FormattedErrorCode);
Console.WriteLine(ErrorCodeLookup.TestErrors.UninstallError.FormattedErrorCode);
```

**Output**
```pwsh
0x06000001
0x06000002
0x06000003
```

See the full sample project [here](src/Samples/SampleConsole/)