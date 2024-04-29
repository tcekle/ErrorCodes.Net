using System.CommandLine;
using ErrorCodes.Net.Generated;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace errorcodes_net_cli.Validations;

using Extensions;
using Interfaces;
using ProgressReporters;
using Validators;

/// <summary>
/// Class implementing the <see cref="ICliCommand"/> for the 'validate' command
/// </summary>
internal class ValidateCommand : ICliCommand
{
    private enum ValidationType
    {
        CheckUniqueness
    }
    
    /// <summary>
    /// Gets the <see cref="ICliCommand.Command"/> to add to the list of available commands
    /// </summary>
    public Command Command { get; }

    /// <summary>
    /// Creates a new instance of <see cref="ValidateCommand"/>
    /// </summary>
    public ValidateCommand()
    {
        Command = new Command("validate", "Run validations on projects using error codes.");
        
        // Common options
        var solutionOption = new Option<FileInfo>("--solution", "The solution to validate")
        {
            IsRequired = true
        };
        
        var validationType = new Option<ValidationType> ("--validation-type", "The type of validation to run")
        {
            IsRequired = true
        };
        
        Command.AddOptions([solutionOption, validationType]);
        Command.SetHandler(RunValidation, solutionOption, validationType);
    }
    
    private async Task RunValidation(FileInfo solution, ValidationType validationType)
    {
        Initialize(solution);
        await AnalyzeSolution(solution, validationType);
    }

    private void Initialize(FileInfo solution)
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
        
        MSBuildLocator.RegisterInstance(instance);
    }

    private async Task AnalyzeSolution(FileInfo solutionFile, ValidationType validationType)
    {
        using (var workspace = MSBuildWorkspace.Create())
        {
            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += (o, e) => Console.WriteLine(e.Diagnostic.Message);
            
            Console.WriteLine($"Loading solution '{solutionFile.FullName}'");
            // Attach progress reporter so we print projects as they are loaded.
            var solution = await workspace.OpenSolutionAsync(solutionFile.FullName, new ConsoleProgressReporter());

            switch (validationType)
            {
                case ValidationType.CheckUniqueness:
                    await CheckUniquenessValidator.CheckUniquenessOfErrors(solution);
                    break;
                default:
                    throw new Exception($"{ErrorCodeLookup.ValidateCommand.UnknownValidationType.FormattedErrorCode}: Unknown validation type '{validationType}'.");
            }
        }
    }
}
