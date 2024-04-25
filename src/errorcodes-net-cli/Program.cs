using System.CommandLine;
using errorcodes_net_cli.Extensions;
using errorcodes_net_cli.Validations;

var rootCommand = new RootCommand("ErrorCodes.Net command line tool");

rootCommand.AddCommands([
    new ValidateCommand()
]);

await rootCommand.InvokeAsync(args);

if (ErrorManager.Errors.Count == 0)
{
    Console.WriteLine("No errors detected!");
}
else
{
    if (ErrorManager.Errors.Any(x => x.ErrorType == ErrorType.Error))
    {
        Environment.ExitCode = 1;
    }

    foreach (var err in ErrorManager.Errors)
    {
        Console.WriteLine(err);
    }
}
