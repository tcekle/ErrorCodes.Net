using ErrorCodes.Net.Analyzers;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace ErrorCodes.Net.Test;

using VerifyJsonCS = CSharpSourceGeneratorVerifier<ErrorCodeYamlLookupGenerator>;

[TestClass]
public class ErrorCodeYamlLookupGeneratorTests
{
    [TestMethod]
    public async Task ErrorCodeLookupGenerator_MultipleEnumValues_CompilesCorrectly()
    {
        var json = """
                   ---
                   projectId: 1
                   errorTypes:
                   - name: TestEnum
                     errorTypeId: 2
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                     - errorCode: 2
                       name: TestError2
                   """;

        var generated = """
                        // ------------------------------------------------------------------------------
                        //  <auto-generated>
                        //      This code was generated by ErrorCodes.Net.
                        //
                        //      Changes to this file may cause incorrect behavior and will be lost if
                        //      the code is regenerated.
                        //  </auto-generated>
                        // ------------------------------------------------------------------------------
                        #region Designer generated code
                        #pragma warning disable
                        using System.Collections.Generic;
                        using ErrorCodes.Net;
                        
                        namespace ErrorCodes.Net.Generated
                        {
                        
                            /// <summary>
                            /// Auto-generated class
                            /// </summary>
                            public class TestEnumErrors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01020001", 0, 1, 2, 1);
                                public ErrorCodeInfo TestError2 { get; } = new ErrorCodeInfo("0x01020002", 0, 1, 2, 2);
                            }
                        
                            /// <summary>
                            /// Generated lookup class for error codes
                            /// </summary>
                            public static class ErrorCodeLookup
                            {
                                public static TestEnumErrors TestEnum { get; } = new TestEnumErrors();
                        
                            }
                        }
                        #pragma warning restore
                        #endregion
                        """;

        await RunTest(json, generated);
    }
    
    [TestMethod]
    public async Task ErrorCodeLookupGenerator_SingleEnumValue_CompilesCorrectly()
    {
        var json = """
                   ---
                   projectId: 1
                   errorTypes:
                   - name: TestEnum
                     errorTypeId: 2
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                   """;

        var generated = """
                        // ------------------------------------------------------------------------------
                        //  <auto-generated>
                        //      This code was generated by ErrorCodes.Net.
                        //
                        //      Changes to this file may cause incorrect behavior and will be lost if
                        //      the code is regenerated.
                        //  </auto-generated>
                        // ------------------------------------------------------------------------------
                        #region Designer generated code
                        #pragma warning disable
                        using System.Collections.Generic;
                        using ErrorCodes.Net;

                        namespace ErrorCodes.Net.Generated
                        {
                        
                            /// <summary>
                            /// Auto-generated class
                            /// </summary>
                            public class TestEnumErrors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01020001", 0, 1, 2, 1);
                            }
                        
                            /// <summary>
                            /// Generated lookup class for error codes
                            /// </summary>
                            public static class ErrorCodeLookup
                            {
                                public static TestEnumErrors TestEnum { get; } = new TestEnumErrors();
                        
                            }
                        }
                        #pragma warning restore
                        #endregion
                        """;

        await RunTest(json, generated);
    }
    
    [TestMethod]
    public async Task ErrorCodeLookupGenerator_MultipleEnumTypes_CompilesCorrectly()
    {
        var json = """
                   ---
                   projectId: 1
                   errorTypes:
                   - name: TestEnum
                     errorTypeId: 2
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                   - name: TestEnumType2
                     errorTypeId: 3
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                   """;

        var generated = """
                        // ------------------------------------------------------------------------------
                        //  <auto-generated>
                        //      This code was generated by ErrorCodes.Net.
                        //
                        //      Changes to this file may cause incorrect behavior and will be lost if
                        //      the code is regenerated.
                        //  </auto-generated>
                        // ------------------------------------------------------------------------------
                        #region Designer generated code
                        #pragma warning disable
                        using System.Collections.Generic;
                        using ErrorCodes.Net;

                        namespace ErrorCodes.Net.Generated
                        {
                        
                            /// <summary>
                            /// Auto-generated class
                            /// </summary>
                            public class TestEnumErrors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01020001", 0, 1, 2, 1);
                            }
                            /// <summary>
                            /// Auto-generated class
                            /// </summary>
                            public class TestEnumType2Errors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01030001", 0, 1, 3, 1);
                            }
                        
                            /// <summary>
                            /// Generated lookup class for error codes
                            /// </summary>
                            public static class ErrorCodeLookup
                            {
                                public static TestEnumErrors TestEnum { get; } = new TestEnumErrors();
                                public static TestEnumType2Errors TestEnumType2 { get; } = new TestEnumType2Errors();
                        
                            }
                        }
                        #pragma warning restore
                        #endregion
                        """;

        await RunTest(json, generated);
    }
    
    [TestMethod]
    public async Task ErrorCodeLookupGenerator_EnumsWithSummary_AddsSummaryToGeneratedClass()
    {
        var json = """
                   ---
                   projectId: 1
                   errorTypes:
                   - name: TestEnum
                     errorTypeId: 2
                     description: Test error description
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                   """;

        var generated = """
                        // ------------------------------------------------------------------------------
                        //  <auto-generated>
                        //      This code was generated by ErrorCodes.Net.
                        //
                        //      Changes to this file may cause incorrect behavior and will be lost if
                        //      the code is regenerated.
                        //  </auto-generated>
                        // ------------------------------------------------------------------------------
                        #region Designer generated code
                        #pragma warning disable
                        using System.Collections.Generic;
                        using ErrorCodes.Net;

                        namespace ErrorCodes.Net.Generated
                        {
                        
                            /// <summary>
                            /// Test error description
                            /// </summary>
                            public class TestEnumErrors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01020001", 0, 1, 2, 1);
                            }
                        
                            /// <summary>
                            /// Generated lookup class for error codes
                            /// </summary>
                            public static class ErrorCodeLookup
                            {
                                public static TestEnumErrors TestEnum { get; } = new TestEnumErrors();
                        
                            }
                        }
                        #pragma warning restore
                        #endregion
                        """;

        await RunTest(json, generated);
    }
    
    [TestMethod]
    public async Task JsonErrorCodeLookupGenerator_MultipleEnumValues_CompilesCorrectly()
    {
        var json = """
                   ---
                   projectId: 1
                   errorTypes:
                   - name: TestEnum
                     errorTypeId: 2
                     errorCodes:
                     - errorCode: 1
                       name: TestError
                     - errorCode: 2
                       name: TestError2
                   
                   """;

        var generated = """
                        // ------------------------------------------------------------------------------
                        //  <auto-generated>
                        //      This code was generated by ErrorCodes.Net.
                        //
                        //      Changes to this file may cause incorrect behavior and will be lost if
                        //      the code is regenerated.
                        //  </auto-generated>
                        // ------------------------------------------------------------------------------
                        #region Designer generated code
                        #pragma warning disable
                        using System.Collections.Generic;
                        using ErrorCodes.Net;
                        
                        namespace ErrorCodes.Net.Generated
                        {
                        
                            /// <summary>
                            /// Auto-generated class
                            /// </summary>
                            public class TestEnumErrors
                            {
                                public ErrorCodeInfo TestError { get; } = new ErrorCodeInfo("0x01020001", 0, 1, 2, 1);
                                public ErrorCodeInfo TestError2 { get; } = new ErrorCodeInfo("0x01020002", 0, 1, 2, 2);
                            }
                        
                            /// <summary>
                            /// Generated lookup class for error codes
                            /// </summary>
                            public static class ErrorCodeLookup
                            {
                                public static TestEnumErrors TestEnum { get; } = new TestEnumErrors();
                        
                            }
                        }
                        #pragma warning restore
                        #endregion
                        """;

        await RunTest(json, generated);
    }
    
    private Task RunTest(string json, string generated)
    {
        return new VerifyJsonCS.Test
        {
            TestState = 
            {
                AdditionalFiles =
                {
                    ("ErrorCodes.yaml", json)
                },
                GeneratedSources =
                {
                    (typeof(ErrorCodeYamlLookupGenerator), ErrorCodeYamlLookupGenerator.GENERATED_FILE_NAME, SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                }
            }
        }.RunAsync();
    }
}