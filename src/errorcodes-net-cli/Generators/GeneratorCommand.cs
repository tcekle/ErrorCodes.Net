using System.CommandLine;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace errorcodes_net_cli.Generators;

using Extensions;
using Interfaces;
using ProgressReporters;

/// <summary>
/// Class implementing the <see cref="ICliCommand"/> for the 'generate' command
/// </summary>
internal class GeneratorCommand : ICliCommand
{
    private string _outputFolder = ".";
    private string _fileName = "error-codes.md";
    
    private enum GenerationType
    {
        GenerateDocfxErrors
    }
    
    /// <summary>
    /// Gets the <see cref="ICliCommand.Command"/> to add to the list of available commands
    /// </summary>
    public Command Command { get; }

    /// <summary>
    /// Creates a new instance of <see cref="GeneratorCommand"/>
    /// </summary>
    public GeneratorCommand()
    {
        Command = new Command("generate", "Run generators on projects using error codes.");
        
        // Common options
        var solutionOption = new Option<FileInfo>("--solution", "The solution to use for generation")
        {
            IsRequired = true
        };
        
        var generationType = new Option<GenerationType> ("--generation-type", "The type of generation to run")
        {
            IsRequired = true
        };
        
        var outputFolder = new Option<DirectoryInfo> ("--output-folder", "The output folder of the generated file")
        {
            IsRequired = false
        };
        outputFolder.AddAlias("-o");
        
        var fileName = new Option<FileInfo> ("--filename", "The output filename of the generated file")
        {
            IsRequired = false
        };
        fileName.AddAlias("-f");
        
        Command.AddOptions([solutionOption, generationType, outputFolder, fileName]);
        Command.SetHandler(RunGeneration, solutionOption, generationType, outputFolder, fileName);
    }
    
    private async Task RunGeneration(FileInfo solution, GenerationType generationType, DirectoryInfo? outputFolder, FileInfo? fileName)
    {
        Initialize(solution, outputFolder, fileName);
        await Generate(solution, generationType);
    }
    
    private void Initialize(FileInfo solution, DirectoryInfo? outputFolder, FileInfo? fileName)
    {
        if (!solution.Exists)
        {
            throw new Exception($"{ErrorCodeLookup.ValidateCommand.SolutionNotFound.FormattedErrorCode}: Solution '{solution.FullName}' not found.");
        }
        
        var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        var instance = visualStudioInstances.FirstOrDefault();

        if (instance is null)
        {
            throw new Exception($"{ErrorCodeLookup.ValidateCommand.MsBuildInstanceNotFound.FormattedErrorCode}: No MSBuild instance found. Please install Visual Studio.");
        }
        
        Console.WriteLine($"Using MSBuild at '{instance.MSBuildPath}' to load projects.");

        if (outputFolder is not null && outputFolder.Exists)
        {
            _outputFolder = outputFolder.FullName;
        }

        if (fileName is not null && fileName.Exists)
        {
            _fileName = fileName.Name;
        }
        
        MSBuildLocator.RegisterInstance(instance);
    }
    
    private async Task Generate(FileInfo solutionFile, GenerationType generationType)
    {
        using (var workspace = MSBuildWorkspace.Create())
        {
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);
            Console.WriteLine($"Loading solution '{solutionFile.FullName}'");
            
            // Attach progress reporter so we print projects as they are loaded.
            var solution = await workspace.OpenSolutionAsync(solutionFile.FullName, new ConsoleProgressReporter());
            
            switch (generationType)
            {
                case GenerationType.GenerateDocfxErrors:
                    await DocfxGenerator.GenerateDocfxErrors(solution, Path.Combine(_outputFolder, _fileName));
                    break;
                default:
                    throw new Exception($"{ErrorCodeLookup.GenerateCommand.UnknownGenerationType}: Unknown generation type '{generationType}'.");
            }
        }
    }
}
