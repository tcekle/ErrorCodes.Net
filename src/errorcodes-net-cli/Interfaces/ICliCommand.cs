using System.CommandLine;

namespace errorcodes_net_cli.Interfaces;

/// <summary>
/// Interface representing a command line operation
/// </summary>
internal interface ICliCommand
{
    /// <summary>
    /// Gets the <see cref="Command"/> to add to the list of available commands
    /// </summary>
    Command Command { get; }
}
