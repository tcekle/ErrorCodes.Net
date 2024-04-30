using Microsoft.CodeAnalysis.MSBuild;

namespace errorcodes_net_cli.ProgressReporters;

/// <summary>
/// Class representing a console progress reporter
/// </summary>
internal class ConsoleProgressReporter : IProgress<ProjectLoadProgress>
{
    /// <summary>
    /// Reports a progress update.
    /// </summary>
    /// <param name="value">The value of the updated progress.</param>
    public void Report(ProjectLoadProgress value)
    {
        var projectDisplay = Path.GetFileName(value.FilePath);
        if (value.TargetFramework != null)
        {
            projectDisplay += $" ({value.TargetFramework})";
        }

        Console.WriteLine($"{value.Operation,-15} {value.ElapsedTime,-15:m\\:ss\\.fffffff} {projectDisplay}");
    }
}
