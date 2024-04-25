using System.CommandLine;
using errorcodes_net_cli.Interfaces;

namespace errorcodes_net_cli.Extensions;

/// <summary>
/// Extensions for the <see cref="RootCommand"/> and <see cref="Command"/> classes
/// </summary>
internal static class RootCommandExtensions
{
    /// <summary>
    /// Add multiple commands to the root command
    /// </summary>
    /// <param name="rootCommand">The <see cref="RootCommand"/> to add comands to</param>
    /// <param name="commands">The collection of <see cref="ICliCommand"/> containing the <see cref="Command"/> to add</param>
    public static void AddCommands(this RootCommand rootCommand, params ICliCommand[] commands)
    {
        foreach (var command in commands)
        {
            rootCommand.AddCommand(command.Command);
        }
    }
    
    /// <summary>
    /// Add multiple options to a command
    /// </summary>
    /// <param name="command">The <see cref="Command"/> to add options to</param>
    /// <param name="options">The collection of <see cref="Option{T}"/> to add</param>
    public static void AddOptions(this Command command, params Option[] options)
    {
        foreach (var option in options)
        {
            command.AddOption(option);
        }
    }
}
