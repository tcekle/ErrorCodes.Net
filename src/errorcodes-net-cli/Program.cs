using System.CommandLine;
using errorcodes_net_cli.Extensions;
using errorcodes_net_cli.Generators;
using errorcodes_net_cli.Validations;

var rootCommand = new RootCommand("ErrorCodes.Net command line tool");

rootCommand.AddCommands([
    new ValidateCommand(),
    new GeneratorCommand()
]);

// InvokeAsync returns a non-zero code when System.CommandLine's exception handler catches an
// unhandled exception. Propagate it so a crashed validation can never be reported as success.
int exitCode = await rootCommand.InvokeAsync(args);

if (ErrorManager.Errors.Any(x => x.ErrorType == ErrorType.Error))
{
    exitCode = 1;
}

foreach (var err in ErrorManager.Errors)
{
    Console.WriteLine(err);
}

if (exitCode == 0 && ErrorManager.Errors.Count == 0)
{
    Console.WriteLine("Success");
}

Environment.ExitCode = exitCode;
return exitCode;
